using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace SnapServerSoftPLC
{
    public class PLCDatabase : IDisposable
    {
        private readonly string connectionString;
        private SqliteConnection? connection;

        public PLCDatabase(string databasePath = "plc_config.db")
        {
            connectionString = $"Data Source={databasePath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // Create ServerConfig table
            var createServerConfigTable = @"
                CREATE TABLE IF NOT EXISTS ServerConfig (
                    Id INTEGER PRIMARY KEY,
                    Port INTEGER NOT NULL DEFAULT 102,
                    AutoStart BOOLEAN NOT NULL DEFAULT 0,
                    ServerName TEXT NOT NULL DEFAULT 'Snap7 Soft PLC',
                    BindAddress TEXT NOT NULL DEFAULT '0.0.0.0',
                    Rack INTEGER NOT NULL DEFAULT 0,
                    Slot INTEGER NOT NULL DEFAULT 1,
                    LastModified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                );";

            // Create DataBlocks table
            var createDataBlocksTable = @"
                CREATE TABLE IF NOT EXISTS DataBlocks (
                    Number INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Size INTEGER NOT NULL,
                    Comment TEXT,
                    IsRegistered BOOLEAN NOT NULL DEFAULT 0,
                    CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    LastModified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                );";

            // Create Variables table
            var createVariablesTable = @"
                CREATE TABLE IF NOT EXISTS Variables (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DBNumber INTEGER NOT NULL,
                    Name TEXT NOT NULL,
                    DataType TEXT NOT NULL,
                    Offset INTEGER NOT NULL,
                    BitOffset INTEGER NOT NULL DEFAULT 0,
                    Value TEXT,
                    Comment TEXT,
                    CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    LastModified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (DBNumber) REFERENCES DataBlocks (Number) ON DELETE CASCADE,
                    UNIQUE(DBNumber, Name)
                );";

            // Create indexes for better performance
            var createIndexes = @"
                CREATE INDEX IF NOT EXISTS idx_variables_db ON Variables(DBNumber);
                CREATE INDEX IF NOT EXISTS idx_variables_name ON Variables(DBNumber, Name);";

            using var command = new SqliteCommand(createServerConfigTable, connection);
            command.ExecuteNonQuery();

            command.CommandText = createDataBlocksTable;
            command.ExecuteNonQuery();

            command.CommandText = createVariablesTable;
            command.ExecuteNonQuery();

            command.CommandText = createIndexes;
            command.ExecuteNonQuery();

            // Perform database migration for new columns
            MigrateDatabase();

            // Insert default server config if not exists (after migration)
            command.CommandText = @"
                INSERT OR IGNORE INTO ServerConfig (Id, Port, AutoStart, ServerName, BindAddress, Rack, Slot) 
                VALUES (1, 102, 0, 'Snap7 Soft PLC', '0.0.0.0', 0, 1);";
            command.ExecuteNonQuery();
        }

        private SqliteConnection GetConnection()
        {
            if (connection == null || connection.State != System.Data.ConnectionState.Open)
            {
                connection?.Dispose();
                connection = new SqliteConnection(connectionString);
                connection.Open();
            }
            return connection;
        }

        private void MigrateDatabase()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            // Check if new ServerConfig columns exist, if not add them
            var checkServerColumns = @"
                SELECT COUNT(*) as count 
                FROM pragma_table_info('ServerConfig') 
                WHERE name IN ('BindAddress', 'Rack', 'Slot')";

            using var checkServerCommand = new SqliteCommand(checkServerColumns, connection);
            var existingServerColumns = Convert.ToInt32(checkServerCommand.ExecuteScalar());

            if (existingServerColumns < 3)
            {
                try
                {
                    // Add missing ServerConfig columns
                    var alterServerCommands = new[]
                    {
                        "ALTER TABLE ServerConfig ADD COLUMN BindAddress TEXT DEFAULT '0.0.0.0'",
                        "ALTER TABLE ServerConfig ADD COLUMN Rack INTEGER DEFAULT 0", 
                        "ALTER TABLE ServerConfig ADD COLUMN Slot INTEGER DEFAULT 1"
                    };

                    foreach (var alterCommand in alterServerCommands)
                    {
                        try
                        {
                            using var command = new SqliteCommand(alterCommand, connection);
                            command.ExecuteNonQuery();
                        }
                        catch (SqliteException ex)
                        {
                            // Column might already exist, continue
                            if (!ex.Message.Contains("duplicate column name"))
                            {
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ServerConfig migration warning: {ex.Message}");
                }
            }

            // Check if BitOffset column exists in Variables table, if not add it
            var checkVariableColumns = @"
                SELECT COUNT(*) as count 
                FROM pragma_table_info('Variables') 
                WHERE name = 'BitOffset'";

            using var checkVariableCommand = new SqliteCommand(checkVariableColumns, connection);
            var hasBitOffset = Convert.ToInt32(checkVariableCommand.ExecuteScalar()) > 0;

            if (!hasBitOffset)
            {
                try
                {
                    using var command = new SqliteCommand("ALTER TABLE Variables ADD COLUMN BitOffset INTEGER NOT NULL DEFAULT 0", connection);
                    command.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine("Added BitOffset column to Variables table");
                }
                catch (SqliteException ex)
                {
                    // Column might already exist, continue
                    if (!ex.Message.Contains("duplicate column name"))
                    {
                        System.Diagnostics.Debug.WriteLine($"BitOffset migration warning: {ex.Message}");
                    }
                }
            }
        }

        // Server Configuration Methods
        public void SaveServerConfig(int port, bool autoStart, string serverName, string bindAddress = "0.0.0.0", int rack = 0, int slot = 1)
        {
            using var connection = GetConnection();
            using var command = new SqliteCommand(@"
                INSERT OR REPLACE INTO ServerConfig (Id, Port, AutoStart, ServerName, BindAddress, Rack, Slot, LastModified) 
                VALUES (1, @port, @autoStart, @serverName, @bindAddress, @rack, @slot, CURRENT_TIMESTAMP)", connection);

            command.Parameters.AddWithValue("@port", port);
            command.Parameters.AddWithValue("@autoStart", autoStart);
            command.Parameters.AddWithValue("@serverName", serverName);
            command.Parameters.AddWithValue("@bindAddress", bindAddress);
            command.Parameters.AddWithValue("@rack", rack);
            command.Parameters.AddWithValue("@slot", slot);
            command.ExecuteNonQuery();
        }

        public (int port, bool autoStart, string serverName, string bindAddress, int rack, int slot) GetServerConfig()
        {
            using var connection = GetConnection();
            using var command = new SqliteCommand("SELECT Port, AutoStart, ServerName, BindAddress, Rack, Slot FROM ServerConfig WHERE Id = 1", connection);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return (
                    reader.GetInt32(0), // Port
                    reader.GetBoolean(1), // AutoStart  
                    reader.GetString(2), // ServerName
                    reader.GetString(3), // BindAddress
                    reader.GetInt32(4), // Rack
                    reader.GetInt32(5)  // Slot
                );
            }
            return (102, false, "Snap7 Soft PLC", "0.0.0.0", 0, 1);
        }

        // DataBlock Methods
        public void SaveDataBlock(PLCDataBlock dataBlock)
        {
            using var connection = GetConnection();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Insert or update DataBlock
                using var command = new SqliteCommand(@"
                    INSERT OR REPLACE INTO DataBlocks (Number, Name, Size, Comment, IsRegistered, LastModified)
                    VALUES (@number, @name, @size, @comment, @isRegistered, CURRENT_TIMESTAMP)", connection, transaction);

                command.Parameters.AddWithValue("@number", dataBlock.Number);
                command.Parameters.AddWithValue("@name", dataBlock.Name);
                command.Parameters.AddWithValue("@size", dataBlock.Size);
                command.Parameters.AddWithValue("@comment", dataBlock.Comment ?? "");
                command.Parameters.AddWithValue("@isRegistered", dataBlock.IsRegistered);
                command.ExecuteNonQuery();

                // Delete existing variables for this DB
                command.CommandText = "DELETE FROM Variables WHERE DBNumber = @number";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@number", dataBlock.Number);
                command.ExecuteNonQuery();

                // Insert variables
                foreach (var variable in dataBlock.Variables)
                {
                    SaveVariable(dataBlock.Number, variable, connection, transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private void SaveVariable(int dbNumber, PLCVariable variable, SqliteConnection connection, SqliteTransaction transaction)
        {
            using var command = new SqliteCommand(@"
                INSERT INTO Variables (DBNumber, Name, DataType, Offset, BitOffset, Value, Comment, LastModified)
                VALUES (@dbNumber, @name, @dataType, @offset, @bitOffset, @value, @comment, CURRENT_TIMESTAMP)", 
                connection, transaction);

            command.Parameters.AddWithValue("@dbNumber", dbNumber);
            command.Parameters.AddWithValue("@name", variable.Name);
            command.Parameters.AddWithValue("@dataType", variable.DataType);
            command.Parameters.AddWithValue("@offset", variable.Offset);
            command.Parameters.AddWithValue("@bitOffset", variable.BitOffset);
            command.Parameters.AddWithValue("@value", variable.Value?.ToString() ?? "");
            command.Parameters.AddWithValue("@comment", variable.Comment ?? "");
            command.ExecuteNonQuery();
        }

        public void DeleteDataBlock(int dbNumber)
        {
            using var connection = GetConnection();
            using var command = new SqliteCommand("DELETE FROM DataBlocks WHERE Number = @number", connection);
            command.Parameters.AddWithValue("@number", dbNumber);
            command.ExecuteNonQuery();
        }

        public List<PLCDataBlock> LoadAllDataBlocks()
        {
            var dataBlocks = new List<PLCDataBlock>();
            using var connection = GetConnection();

            // Load DataBlocks
            using var command = new SqliteCommand("SELECT * FROM DataBlocks ORDER BY Number", connection);
            using var reader = command.ExecuteReader();

            var dbInfo = new List<(int number, string name, int size, string comment, bool isRegistered)>();
            while (reader.Read())
            {
                dbInfo.Add((
                    reader.GetInt32(0), // Number
                    reader.GetString(1), // Name
                    reader.GetInt32(2), // Size
                    reader.IsDBNull(3) ? "" : reader.GetString(3), // Comment
                    reader.GetBoolean(4) // IsRegistered
                ));
            }
            reader.Close();

            // Load variables for each DataBlock
            foreach (var (number, name, size, comment, isRegistered) in dbInfo)
            {
                var dataBlock = new PLCDataBlock(number, size)
                {
                    Name = name,
                    Comment = comment,
                    IsRegistered = false // Will be set to true when registered with server
                };

                // Load variables (order by offset, then bit offset for proper addressing display)
                using var varCommand = new SqliteCommand(
                    "SELECT * FROM Variables WHERE DBNumber = @dbNumber ORDER BY Offset, BitOffset", connection);
                varCommand.Parameters.AddWithValue("@dbNumber", number);
                using var varReader = varCommand.ExecuteReader();

                while (varReader.Read())
                {
                    var variable = new PLCVariable
                    {
                        Name = varReader.GetString(2), // Name (column index 2)
                        DataType = varReader.GetString(3), // DataType (column index 3)
                        Offset = varReader.GetInt32(4), // Offset (column index 4)
                        BitOffset = varReader.IsDBNull(5) ? 0 : varReader.GetInt32(5), // BitOffset (column index 5)
                        Comment = varReader.IsDBNull(7) ? "" : varReader.GetString(7) // Comment (column index 7, shifted due to BitOffset)
                    };

                    // Parse and set value based on data type
                    var valueStr = varReader.IsDBNull(6) ? "" : varReader.GetString(6); // Value (column index 6, shifted due to BitOffset)
                    variable.Value = ParseValueFromString(variable.DataType, valueStr);
                    
                    dataBlock.Variables.Add(variable);
                }
                varReader.Close();

                dataBlocks.Add(dataBlock);
            }

            return dataBlocks;
        }

        private object ParseValueFromString(string dataType, string valueStr)
        {
            if (string.IsNullOrEmpty(valueStr))
                return GetDefaultValueForType(dataType);

            try
            {
                return dataType switch
                {
                    "BOOL" => bool.Parse(valueStr),
                    "BYTE" => byte.Parse(valueStr),
                    "WORD" => ushort.Parse(valueStr),
                    "DWORD" => uint.Parse(valueStr),
                    "INT" => short.Parse(valueStr),
                    "DINT" => int.Parse(valueStr),
                    "REAL" => float.Parse(valueStr),
                    "STRING" => valueStr,
                    _ => valueStr
                };
            }
            catch
            {
                return GetDefaultValueForType(dataType);
            }
        }

        private object GetDefaultValueForType(string dataType)
        {
            return dataType switch
            {
                "BOOL" => false,
                "BYTE" => (byte)0,
                "WORD" => (ushort)0,
                "DWORD" => (uint)0,
                "INT" => (short)0,
                "DINT" => 0,
                "REAL" => 0.0f,
                "STRING" => "",
                _ => false
            };
        }

        public void UpdateVariableValue(int dbNumber, string variableName, object value)
        {
            using var connection = GetConnection();
            using var command = new SqliteCommand(@"
                UPDATE Variables 
                SET Value = @value, LastModified = CURRENT_TIMESTAMP 
                WHERE DBNumber = @dbNumber AND Name = @name", connection);

            command.Parameters.AddWithValue("@value", value?.ToString() ?? "");
            command.Parameters.AddWithValue("@dbNumber", dbNumber);
            command.Parameters.AddWithValue("@name", variableName);
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}