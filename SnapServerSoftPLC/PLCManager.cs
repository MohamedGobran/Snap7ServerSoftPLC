using Snap7;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnapServerSoftPLC
{
    public class PLCManager : IDisposable
    {
        private S7Server server;
        private Dictionary<int, PLCDataBlock> dataBlocks;
        private PLCDatabase database;
        private bool isRunning = false;

        public bool IsRunning => isRunning;
        public IReadOnlyDictionary<int, PLCDataBlock> DataBlocks => dataBlocks.AsReadOnly();

        public event EventHandler<string>? StatusChanged;
        public event EventHandler<PLCEventArgs>? PLCEvent;
        public event EventHandler? DataBlocksChanged;

        public PLCManager()
        {
            server = new S7Server();
            dataBlocks = new Dictionary<int, PLCDataBlock>();
            database = new PLCDatabase();

            // Set up server event callback
            server.SetEventsCallBack(OnServerEvent, IntPtr.Zero);

            // Load configuration from database
            LoadConfiguration();
        }

        public bool StartPLC()
        {
            try
            {
                // Get bind address from configuration
                var (port, autoStart, serverName, bindAddress, rack, slot) = database.GetServerConfig();
                
                int result;
                if (bindAddress == "0.0.0.0")
                {
                    // Bind to all interfaces (default behavior)
                    result = server.Start();
                    StatusChanged?.Invoke(this, $"Starting PLC server on all interfaces, port {port}");
                }
                else
                {
                    // Bind to specific IP address
                    result = server.StartTo(bindAddress);
                    StatusChanged?.Invoke(this, $"Starting PLC server on {bindAddress}:{port}");
                }
                
                if (result == 0)
                {
                    isRunning = true;
                    StatusChanged?.Invoke(this, $"PLC Started Successfully - Clients can connect to {bindAddress}:{port} (Rack: {rack}, Slot: {slot})");
                    
                    // Register all loaded data blocks with the server
                    RegisterLoadedDataBlocks();
                    
                    return true;
                }
                else
                {
                    StatusChanged?.Invoke(this, $"Failed to start PLC: {server.ErrorText(result)}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error starting PLC: {ex.Message}");
                return false;
            }
        }

        public bool StopPLC()
        {
            try
            {
                int result = server.Stop();
                isRunning = false;
                StatusChanged?.Invoke(this, "PLC Stopped");
                return result == 0;
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error stopping PLC: {ex.Message}");
                return false;
            }
        }

        public bool AddDataBlock(int number, int size = 1024, string name = "", string comment = "")
        {
            try
            {
                if (dataBlocks.ContainsKey(number))
                {
                    StatusChanged?.Invoke(this, $"Data Block DB{number} already exists");
                    return false;
                }

                var dataBlock = new PLCDataBlock(number, size);
                if (!string.IsNullOrEmpty(name))
                    dataBlock.Name = name;
                dataBlock.Comment = comment;

                // Register the data block with the server
                byte[] dataArray = dataBlock.Data;
                StatusChanged?.Invoke(this, $"Attempting to register DB{number} with area code: 0x{S7Server.srvAreaDB:X2}");
                int result = server.RegisterArea(S7Server.srvAreaDB, number, ref dataArray, dataBlock.Data.Length);
                if (result == 0)
                {
                    dataBlock.IsRegistered = true;
                    dataBlocks[number] = dataBlock;
                    
                    // Auto-save to database
                    try
                    {
                        database.SaveDataBlock(dataBlock);
                    }
                    catch (Exception dbEx)
                    {
                        StatusChanged?.Invoke(this, $"Warning: Failed to save DB{number} to database: {dbEx.Message}");
                    }
                    
                    StatusChanged?.Invoke(this, $"Data Block DB{number} added successfully");
                    return true;
                }
                else
                {
                    StatusChanged?.Invoke(this, $"Failed to register DB{number}: {server.ErrorText(result)}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error adding data block DB{number}: {ex.Message}");
                return false;
            }
        }

        public bool RemoveDataBlock(int number)
        {
            try
            {
                if (!dataBlocks.ContainsKey(number))
                {
                    StatusChanged?.Invoke(this, $"Data Block DB{number} does not exist");
                    return false;
                }

                // Unregister from server
                int result = server.UnregisterArea(S7Server.srvAreaDB, number);

                dataBlocks.Remove(number);
                
                // Auto-save to database (remove from database)
                try
                {
                    database.DeleteDataBlock(number);
                }
                catch (Exception dbEx)
                {
                    StatusChanged?.Invoke(this, $"Warning: Failed to remove DB{number} from database: {dbEx.Message}");
                }
                
                StatusChanged?.Invoke(this, $"Data Block DB{number} removed");

                return result == 0;
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error removing data block DB{number}: {ex.Message}");
                return false;
            }
        }

        public PLCDataBlock? GetDataBlock(int number)
        {
            return dataBlocks.TryGetValue(number, out var dataBlock) ? dataBlock : null;
        }

        public bool AddVariableToDataBlock(int dbNumber, string name, string dataType, int offset, string comment = "", int bitOffset = 0)
        {
            try
            {
                var dataBlock = GetDataBlock(dbNumber);
                if (dataBlock == null)
                {
                    StatusChanged?.Invoke(this, $"Data Block DB{dbNumber} does not exist");
                    return false;
                }

                // Enhanced validation using MemoryManager with bit addressing support
                var validationResult = ValidateVariableAddition(dataBlock, name, dataType, offset, bitOffset);
                if (!validationResult.IsValid)
                {
                    StatusChanged?.Invoke(this, $"Validation failed: {validationResult.ErrorMessage}");
                    return false;
                }

                dataBlock.AddVariable(name, dataType, offset, comment, bitOffset);
                
                // Auto-save to database
                try
                {
                    database.SaveDataBlock(dataBlock);
                }
                catch (Exception dbEx)
                {
                    StatusChanged?.Invoke(this, $"Warning: Failed to save variable '{name}' to database: {dbEx.Message}");
                }
                
                StatusChanged?.Invoke(this, $"Variable '{name}' added to DB{dbNumber} at offset {offset}");
                return true;
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error adding variable '{name}': {ex.Message}");
                return false;
            }
        }

        public (bool IsValid, string ErrorMessage) ValidateVariableAddition(PLCDataBlock dataBlock, string name, string dataType, int offset, int bitOffset = 0, string? excludeVariableName = null)
        {
            // Name validation
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Variable name cannot be empty");

            // Name uniqueness
            if (dataBlock.Variables.Any(v => v.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase) && 
                                           (excludeVariableName == null || v.Name != excludeVariableName)))
                return (false, $"Variable '{name}' already exists in this data block");

            // Data type validation
            var validTypes = new[] { "BOOL", "BYTE", "WORD", "DWORD", "INT", "DINT", "REAL", "STRING" };
            if (!validTypes.Contains(dataType))
                return (false, $"Invalid data type: {dataType}");

            // Bit offset validation
            if (dataType == "BOOL")
            {
                if (bitOffset < 0 || bitOffset > 7)
                    return (false, $"Bit offset must be between 0 and 7 for BOOL variables");
            }
            else if (bitOffset != 0)
            {
                return (false, $"Bit offset must be 0 for non-BOOL data types");
            }

            // Bounds checking
            if (offset < 0 || offset >= dataBlock.Size)
                return (false, $"Offset {offset} is out of bounds (0-{dataBlock.Size-1})");

            // Get variable size for memory footprint calculation
            var tempVar = new PLCVariable { DataType = dataType, BitOffset = bitOffset };
            int memoryFootprint = tempVar.GetMemoryFootprint();
            
            if (offset + memoryFootprint > dataBlock.Size)
                return (false, $"Variable at offset {offset} with size {memoryFootprint} exceeds data block bounds");

            // Collision detection based on data type
            if (dataType == "BOOL")
            {
                // Check for bit-level collision with other BOOL variables
                var conflictingBool = dataBlock.Variables
                    .Where(v => v.Name != excludeVariableName && v.DataType == "BOOL" && 
                               v.Offset == offset && v.BitOffset == bitOffset)
                    .FirstOrDefault();
                    
                if (conflictingBool != null)
                    return (false, $"Bit address {offset}.{bitOffset} is already occupied by variable '{conflictingBool.Name}'");

                // Check if the byte is occupied by a non-BOOL variable
                var conflictingNonBool = dataBlock.Variables
                    .Where(v => v.Name != excludeVariableName && v.DataType != "BOOL" &&
                               v.Offset <= offset && v.Offset + v.GetSize() > offset)
                    .FirstOrDefault();
                    
                if (conflictingNonBool != null)
                    return (false, $"Byte {offset} is occupied by {conflictingNonBool.DataType} variable '{conflictingNonBool.Name}'");
            }
            else
            {
                // For non-BOOL variables, check for any collision (both BOOL and non-BOOL)
                for (int i = 0; i < tempVar.GetSize(); i++)
                {
                    int checkOffset = offset + i;
                    
                    // Check for BOOL variables in any of the bytes this variable occupies
                    var conflictingBool = dataBlock.Variables
                        .Where(v => v.Name != excludeVariableName && v.DataType == "BOOL" && 
                                   v.Offset == checkOffset)
                        .FirstOrDefault();
                        
                    if (conflictingBool != null)
                        return (false, $"Byte {checkOffset} contains BOOL variable '{conflictingBool.Name}' at bit {conflictingBool.BitOffset}");
                    
                    // Check for overlapping non-BOOL variables
                    var conflictingNonBool = dataBlock.Variables
                        .Where(v => v.Name != excludeVariableName && v.DataType != "BOOL" &&
                                   v.Offset <= checkOffset && v.Offset + v.GetSize() > checkOffset)
                        .FirstOrDefault();
                        
                    if (conflictingNonBool != null)
                        return (false, $"Overlaps with {conflictingNonBool.DataType} variable '{conflictingNonBool.Name}' at offset {conflictingNonBool.Offset}");
                }
            }

            return (true, string.Empty);
        }

        public int GetNextAvailableOffset(int dbNumber, string dataType)
        {
            var dataBlock = GetDataBlock(dbNumber);
            if (dataBlock == null) return -1;

            var tempVar = new PLCVariable { DataType = dataType };
            return BitAddressingHelper.GetNextAvailableOffset(dataBlock.Variables, dataBlock.Size, tempVar.GetSize());
        }

        public bool SetVariableValue(int dbNumber, string variableName, object value)
        {
            try
            {
                var dataBlock = GetDataBlock(dbNumber);
                if (dataBlock == null)
                {
                    StatusChanged?.Invoke(this, $"Data Block DB{dbNumber} does not exist");
                    return false;
                }

                dataBlock.SetVariableValue(variableName, value);
                
                // Auto-save variable value to database
                try
                {
                    database.UpdateVariableValue(dbNumber, variableName, value);
                }
                catch (Exception dbEx)
                {
                    StatusChanged?.Invoke(this, $"Warning: Failed to save variable value to database: {dbEx.Message}");
                }
                
                return true;
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error setting variable value: {ex.Message}");
                return false;
            }
        }

        public object? GetVariableValue(int dbNumber, string variableName)
        {
            var dataBlock = GetDataBlock(dbNumber);
            return dataBlock?.GetVariableValue(variableName);
        }

        public ServerStatusInfo GetServerStatus()
        {
            int serverStatus = 0, cpuStatus = 0, clientsCount = 0;

            try
            {
                serverStatus = server.ServerStatus;
                cpuStatus = server.CpuStatus;
                clientsCount = server.ClientsCount;
                return new ServerStatusInfo
                {
                    ServerStatus = serverStatus,
                    CpuStatus = cpuStatus,
                    ClientsCount = clientsCount,
                    IsRunning = isRunning,
                    DataBlockCount = dataBlocks.Count
                };
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error getting server status: {ex.Message}");
                return new ServerStatusInfo
                {
                    IsRunning = isRunning,
                    DataBlockCount = dataBlocks.Count
                };
            }
        }

        private void LoadConfiguration()
        {
            try
            {
                // Load server configuration
                var (port, autoStart, serverName, bindAddress, rack, slot) = database.GetServerConfig();
                int serverPort = port;
                server.SetParam(S7Consts.p_u16_LocalPort, ref serverPort);
                
                // Load data blocks from database
                var savedDataBlocks = database.LoadAllDataBlocks();
                foreach (var dataBlock in savedDataBlocks)
                {
                    dataBlocks[dataBlock.Number] = dataBlock;
                }
                
                StatusChanged?.Invoke(this, $"Configuration loaded: {savedDataBlocks.Count} data blocks, Port: {port}, Bind: {bindAddress}");
                StatusChanged?.Invoke(this, $"Client connection parameters: Rack={rack}, Slot={slot} (informational)");
                
                // Auto-start if configured
                if (autoStart)
                {
                    StatusChanged?.Invoke(this, "Auto-starting PLC...");
                    StartPLC();
                }
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error loading configuration: {ex.Message}");
            }
        }

        private void RegisterLoadedDataBlocks()
        {
            int registeredCount = 0;
            int failedCount = 0;

            foreach (var dataBlock in dataBlocks.Values)
            {
                try
                {
                    byte[] dataArray = dataBlock.Data;
                    int result = server.RegisterArea(S7Server.srvAreaDB, dataBlock.Number, ref dataArray, dataBlock.Data.Length);
                    
                    if (result == 0)
                    {
                        dataBlock.IsRegistered = true;
                        registeredCount++;
                        
                        // Write variable values to data array
                        foreach (var variable in dataBlock.Variables)
                        {
                            dataBlock.SetVariableValue(variable.Name, variable.Value);
                        }
                        
                        // Update database with registration status
                        try
                        {
                            database.SaveDataBlock(dataBlock);
                        }
                        catch (Exception dbEx)
                        {
                            StatusChanged?.Invoke(this, $"Warning: Failed to update DB{dataBlock.Number} status in database: {dbEx.Message}");
                        }
                    }
                    else
                    {
                        failedCount++;
                        StatusChanged?.Invoke(this, $"Failed to register DB{dataBlock.Number}: {server.ErrorText(result)}");
                    }
                }
                catch (Exception ex)
                {
                    failedCount++;
                    StatusChanged?.Invoke(this, $"Error registering DB{dataBlock.Number}: {ex.Message}");
                }
            }

            if (registeredCount > 0)
            {
                StatusChanged?.Invoke(this, $"Registered {registeredCount} data blocks with server");
                // Trigger UI refresh to show updated registration status
                DataBlocksChanged?.Invoke(this, EventArgs.Empty);
            }
            if (failedCount > 0)
            {
                StatusChanged?.Invoke(this, $"Failed to register {failedCount} data blocks");
            }
        }

        public void SaveConfiguration()
        {
            try
            {
                // Save all data blocks and their variables
                foreach (var dataBlock in dataBlocks.Values)
                {
                    database.SaveDataBlock(dataBlock);
                }
                StatusChanged?.Invoke(this, "Configuration saved successfully");
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error saving configuration: {ex.Message}");
            }
        }

        public void SetAutoStart(bool autoStart)
        {
            try
            {
                var (port, _, serverName, bindAddress, rack, slot) = database.GetServerConfig();
                database.SaveServerConfig(port, autoStart, serverName, bindAddress, rack, slot);
                StatusChanged?.Invoke(this, $"Auto-start set to: {autoStart}");
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error setting auto-start: {ex.Message}");
            }
        }

        public (int port, bool autoStart, string serverName, string bindAddress, int rack, int slot) GetConfiguration()
        {
            return database.GetServerConfig();
        }

        public void SetNetworkConfiguration(int port, string bindAddress, int rack, int slot, bool autoStart, string serverName)
        {
            try
            {
                database.SaveServerConfig(port, autoStart, serverName, bindAddress, rack, slot);
                StatusChanged?.Invoke(this, $"Network configuration updated: {bindAddress}:{port}, Rack: {rack}, Slot: {slot}");
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error setting network configuration: {ex.Message}");
            }
        }

        // Advanced Variable Management
        public bool UpdateVariable(int dbNumber, string oldName, string newName, string newDataType, int newOffset, string newComment, int newBitOffset = 0)
        {
            try
            {
                var dataBlock = GetDataBlock(dbNumber);
                if (dataBlock == null)
                {
                    StatusChanged?.Invoke(this, $"Data Block DB{dbNumber} does not exist");
                    return false;
                }

                var variable = dataBlock.Variables.Find(v => v.Name == oldName);
                if (variable == null)
                {
                    StatusChanged?.Invoke(this, $"Variable '{oldName}' not found in DB{dbNumber}");
                    return false;
                }

                // Enhanced validation using MemoryManager with exclusion for current variable
                var validationResult = ValidateVariableAddition(dataBlock, newName, newDataType, newOffset, newBitOffset, oldName);
                if (!validationResult.IsValid)
                {
                    StatusChanged?.Invoke(this, $"Update validation failed: {validationResult.ErrorMessage}");
                    return false;
                }

                // Store old values for rollback if needed
                string oldDataType = variable.DataType;
                int oldOffset = variable.Offset;
                int oldBitOffset = variable.BitOffset;
                object? oldValue = variable.Value;

                // Update variable properties
                variable.Name = newName;
                variable.DataType = newDataType;
                variable.Offset = newOffset;
                variable.BitOffset = newDataType == "BOOL" ? newBitOffset : 0;
                variable.Comment = newComment;
                
                // Reset value if data type changed, otherwise preserve current value
                if (oldDataType != newDataType)
                {
                    variable.Value = GetDefaultValueForDataType(newDataType);
                    StatusChanged?.Invoke(this, $"Variable data type changed from {oldDataType} to {newDataType}, value reset to default");
                }
                else if (oldOffset != newOffset)
                {
                    // Update the value in memory at new offset
                    dataBlock.SetVariableValue(newName, variable.Value);
                }

                // Auto-save to database
                try
                {
                    database.SaveDataBlock(dataBlock);
                }
                catch (Exception dbEx)
                {
                    StatusChanged?.Invoke(this, $"Warning: Failed to save updated variable to database: {dbEx.Message}");
                }

                string changes = BuildChangesSummary(oldName, newName, oldDataType, newDataType, oldOffset, newOffset);
                StatusChanged?.Invoke(this, $"Variable '{newName}' updated successfully in DB{dbNumber}{changes}");
                return true;
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error updating variable: {ex.Message}");
                return false;
            }
        }

        private string BuildChangesSummary(string oldName, string newName, string oldDataType, string newDataType, int oldOffset, int newOffset)
        {
            var changes = new List<string>();
            
            if (oldName != newName)
                changes.Add($"name: {oldName} → {newName}");
            if (oldDataType != newDataType)
                changes.Add($"type: {oldDataType} → {newDataType}");
            if (oldOffset != newOffset)
                changes.Add($"offset: {oldOffset} → {newOffset}");
                
            return changes.Count > 0 ? $" (Changes: {string.Join(", ", changes)})" : "";
        }

        public bool DeleteVariable(int dbNumber, string variableName)
        {
            try
            {
                var dataBlock = GetDataBlock(dbNumber);
                if (dataBlock == null)
                {
                    StatusChanged?.Invoke(this, $"Data Block DB{dbNumber} does not exist");
                    return false;
                }

                var variable = dataBlock.Variables.Find(v => v.Name == variableName);
                if (variable == null)
                {
                    StatusChanged?.Invoke(this, $"Variable '{variableName}' not found in DB{dbNumber}");
                    return false;
                }

                dataBlock.Variables.Remove(variable);

                // Auto-save to database
                try
                {
                    database.SaveDataBlock(dataBlock);
                }
                catch (Exception dbEx)
                {
                    StatusChanged?.Invoke(this, $"Warning: Failed to save changes to database: {dbEx.Message}");
                }

                StatusChanged?.Invoke(this, $"Variable '{variableName}' deleted from DB{dbNumber}");
                return true;
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error deleting variable: {ex.Message}");
                return false;
            }
        }

        // Advanced DataBlock Management
        public bool UpdateDataBlock(int dbNumber, string newName, int newSize, string newComment)
        {
            try
            {
                var dataBlock = GetDataBlock(dbNumber);
                if (dataBlock == null)
                {
                    StatusChanged?.Invoke(this, $"Data Block DB{dbNumber} does not exist");
                    return false;
                }

                // Check if any variables would exceed new size
                int maxOffset = 0;
                foreach (var variable in dataBlock.Variables)
                {
                    int endOffset = variable.Offset + variable.GetSize();
                    if (endOffset > maxOffset)
                        maxOffset = endOffset;
                }

                if (maxOffset > newSize)
                {
                    StatusChanged?.Invoke(this, $"Cannot resize DB{dbNumber}: Variables extend beyond new size {newSize}");
                    return false;
                }

                // Update properties
                string oldName = dataBlock.Name;
                dataBlock.Name = newName;
                dataBlock.Comment = newComment;

                // Handle size change
                if (dataBlock.Size != newSize)
                {
                    // Need to re-register with server if running
                    bool wasRegistered = dataBlock.IsRegistered;
                    
                    if (wasRegistered && isRunning)
                    {
                        // Unregister old
                        server.UnregisterArea(S7Server.srvAreaDB, dbNumber);
                    }

                    // Update size and data array
                    dataBlock.Size = newSize;
                    byte[] oldData = dataBlock.Data;
                    dataBlock.Data = new byte[newSize];
                    
                    // Copy existing data
                    int copyLength = Math.Min(oldData.Length, newSize);
                    Array.Copy(oldData, dataBlock.Data, copyLength);

                    // Re-register if it was registered
                    if (wasRegistered && isRunning)
                    {
                        byte[] dataArray = dataBlock.Data;
                        int result = server.RegisterArea(S7Server.srvAreaDB, dbNumber, ref dataArray, dataBlock.Data.Length);
                        dataBlock.IsRegistered = (result == 0);
                        
                        if (result != 0)
                        {
                            StatusChanged?.Invoke(this, $"Warning: Failed to re-register DB{dbNumber} after resize: {server.ErrorText(result)}");
                        }
                    }
                }

                // Auto-save to database
                try
                {
                    database.SaveDataBlock(dataBlock);
                }
                catch (Exception dbEx)
                {
                    StatusChanged?.Invoke(this, $"Warning: Failed to save updated data block to database: {dbEx.Message}");
                }

                StatusChanged?.Invoke(this, $"Data Block DB{dbNumber} updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error updating data block: {ex.Message}");
                return false;
            }
        }

        public bool CopyDataBlock(int sourceDbNumber, int targetDbNumber, string newName = "")
        {
            try
            {
                var sourceBlock = GetDataBlock(sourceDbNumber);
                if (sourceBlock == null)
                {
                    StatusChanged?.Invoke(this, $"Source Data Block DB{sourceDbNumber} does not exist");
                    return false;
                }

                if (dataBlocks.ContainsKey(targetDbNumber))
                {
                    StatusChanged?.Invoke(this, $"Target Data Block DB{targetDbNumber} already exists");
                    return false;
                }

                // Create copy
                var targetBlock = new PLCDataBlock(targetDbNumber, sourceBlock.Size)
                {
                    Name = string.IsNullOrEmpty(newName) ? $"{sourceBlock.Name}_Copy" : newName,
                    Comment = $"Copy of DB{sourceDbNumber} - {sourceBlock.Comment}",
                    IsRegistered = false
                };

                // Copy data
                Array.Copy(sourceBlock.Data, targetBlock.Data, sourceBlock.Size);

                // Copy variables
                foreach (var sourceVar in sourceBlock.Variables)
                {
                    var targetVar = new PLCVariable
                    {
                        Name = sourceVar.Name,
                        DataType = sourceVar.DataType,
                        Offset = sourceVar.Offset,
                        Value = sourceVar.Value,
                        Comment = sourceVar.Comment
                    };
                    targetBlock.Variables.Add(targetVar);
                }

                // Add to collection
                dataBlocks[targetDbNumber] = targetBlock;

                // Register with server if running
                if (isRunning)
                {
                    byte[] dataArray = targetBlock.Data;
                    int result = server.RegisterArea(S7Server.srvAreaDB, targetDbNumber, ref dataArray, targetBlock.Data.Length);
                    
                    if (result == 0)
                    {
                        targetBlock.IsRegistered = true;
                        StatusChanged?.Invoke(this, $"Data Block DB{targetDbNumber} registered with server");
                    }
                    else
                    {
                        StatusChanged?.Invoke(this, $"Warning: Failed to register copied DB{targetDbNumber}: {server.ErrorText(result)}");
                    }
                }

                // Auto-save to database
                try
                {
                    database.SaveDataBlock(targetBlock);
                }
                catch (Exception dbEx)
                {
                    StatusChanged?.Invoke(this, $"Warning: Failed to save copied data block to database: {dbEx.Message}");
                }

                StatusChanged?.Invoke(this, $"Data Block DB{sourceDbNumber} copied to DB{targetDbNumber} successfully");
                return true;
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error copying data block: {ex.Message}");
                return false;
            }
        }

        private object GetDefaultValueForDataType(string dataType)
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

        private void OnServerEvent(IntPtr usrPtr, ref Snap7.S7Server.USrvEvent serverEvent, int size)
        {
            try
            {
                // Convert timestamp from platform-dependent IntPtr to DateTime
                DateTime eventTime = server.EvtTimeToDateTime(serverEvent.EvtTime);

                // Handle connection events
                if (serverEvent.EvtCode == Snap7.S7Server.evcClientAdded)
                {
                    StatusChanged?.Invoke(this, $"[{eventTime:HH:mm:ss}] New client connected");
                }
                else if (serverEvent.EvtCode == Snap7.S7Server.evcClientRejected)
                {
                    StatusChanged?.Invoke(this, $"[{eventTime:HH:mm:ss}] Client connection rejected by server");
                }
                else if (serverEvent.EvtCode == Snap7.S7Server.evcClientDisconnected)
                {
                    StatusChanged?.Invoke(this, $"[{eventTime:HH:mm:ss}] Client disconnected");
                }
                else if (serverEvent.EvtCode == Snap7.S7Server.evcClientException)
                {
                    StatusChanged?.Invoke(this, $"[{eventTime:HH:mm:ss}] Client exception occurred");
                }
                else if (serverEvent.EvtCode == Snap7.S7Server.evcServerStarted)
                {
                    StatusChanged?.Invoke(this, $"[{eventTime:HH:mm:ss}] Server started event");
                }
                else if (serverEvent.EvtCode == Snap7.S7Server.evcServerStopped)
                {
                    StatusChanged?.Invoke(this, $"[{eventTime:HH:mm:ss}] Server stopped event");
                }
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke(this, $"Error in server event handler: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (isRunning)
                StopPLC();

            // Save configuration before disposing
            SaveConfiguration();
            database?.Dispose();
            
            // S7Server has destructor that calls Srv_Destroy, no explicit Dispose needed
        }
    }

    public class PLCEventArgs : EventArgs
    {
        public string EventType { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class ServerStatusInfo
    {
        public int ServerStatus { get; set; }
        public int CpuStatus { get; set; }
        public int ClientsCount { get; set; }
        public bool IsRunning { get; set; }
        public int DataBlockCount { get; set; }
    }
}