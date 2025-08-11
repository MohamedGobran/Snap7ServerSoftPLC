# Snap7 Soft PLC Server - Developer Guide

## Table of Contents
1. [Architecture Overview](#architecture-overview)
2. [Project Structure](#project-structure)
3. [Core Components](#core-components)
4. [Database Schema](#database-schema)
5. [API Reference](#api-reference)
6. [Development Setup](#development-setup)
7. [Testing](#testing)
8. [Deployment](#deployment)
9. [Contributing](#contributing)

---

## Architecture Overview

### System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    Snap7 Soft PLC Server                       │
├─────────────────────────────────────────────────────────────────┤
│  Form1 (UI Layer)                                               │
│  ├─ Windows Forms Controls                                      │
│  ├─ Event Handlers                                              │
│  └─ UI State Management                                         │
├─────────────────────────────────────────────────────────────────┤
│  PLCManager (Business Logic)                                    │
│  ├─ Data Block Management                                       │
│  ├─ Variable Management                                         │
│  ├─ Server Lifecycle                                            │
│  └─ Configuration Management                                    │
├─────────────────────────────────────────────────────────────────┤
│  PLCDatabase (Data Access Layer)                                │
│  ├─ SQLite Operations                                           │
│  ├─ CRUD Operations                                             │
│  ├─ Transaction Management                                      │
│  └─ Data Persistence                                            │
├─────────────────────────────────────────────────────────────────┤
│  Data Models                                                    │
│  ├─ PLCDataBlock                                                │
│  ├─ PLCVariable                                                 │
│  └─ ServerStatusInfo                                            │
├─────────────────────────────────────────────────────────────────┤
│  Snap7 Integration                                              │
│  ├─ S7Server (Native Wrapper)                                  │
│  ├─ P/Invoke Declarations                                       │
│  └─ snap7.dll (Native Library)                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Technology Stack
- **.NET 9.0** - Core framework
- **Windows Forms** - UI framework
- **SQLite** - Configuration persistence
- **Snap7** - S7 protocol implementation
- **P/Invoke** - Native library integration

---

## Project Structure

```
SnapServerSoftPLC/
├── docs/
│   ├── USER_GUIDE.md
│   ├── DEVELOPER_GUIDE.md
│   └── API_REFERENCE.md
├── Native/
│   ├── snap7.dll (64-bit)
│   └── README.txt
├── Core Components/
│   ├── PLCManager.cs          # Main business logic
│   ├── PLCDatabase.cs         # Data access layer
│   ├── PLCDataBlock.cs        # Data models
│   └── Snap7.cs               # Native wrapper
├── UI Components/
│   ├── Form1.cs               # Main form
│   ├── Form1.Designer.cs      # UI layout
│   ├── AddDataBlockDialog.cs  # Dialog forms
│   ├── EditDataBlockDialog.cs
│   ├── AddVariableDialog.cs
│   ├── EditVariableDialog.cs
│   ├── UpdateValueDialog.cs
│   └── CopyDataBlockDialog.cs
├── Configuration/
│   ├── Program.cs             # Entry point
│   └── SnapServerSoftPLC.csproj
└── Runtime/
    ├── plc_config.db          # SQLite database
    └── Application logs
```

---

## Core Components

### 1. PLCManager Class

**Purpose**: Central business logic coordinator

```csharp
public class PLCManager : IDisposable
{
    // Core functionality
    public bool StartPLC()
    public bool StopPLC() 
    public bool AddDataBlock(int number, int size, string name, string comment)
    public bool RemoveDataBlock(int number)
    
    // Advanced management
    public bool UpdateDataBlock(int dbNumber, string newName, int newSize, string newComment)
    public bool CopyDataBlock(int sourceDbNumber, int targetDbNumber, string newName)
    public bool UpdateVariable(int dbNumber, string oldName, string newName, ...)
    public bool DeleteVariable(int dbNumber, string variableName)
    
    // Configuration
    public void SaveConfiguration()
    public (int port, bool autoStart, string serverName) GetConfiguration()
}
```

**Key Features:**
- ✅ Server lifecycle management
- ✅ Data block CRUD operations
- ✅ Variable management with validation
- ✅ Automatic persistence
- ✅ Event-driven architecture

### 2. PLCDatabase Class

**Purpose**: SQLite data access layer with full CRUD operations

```csharp
public class PLCDatabase : IDisposable
{
    // Server configuration
    public void SaveServerConfig(int port, bool autoStart, string serverName)
    public (int port, bool autoStart, string serverName) GetServerConfig()
    
    // Data block operations
    public void SaveDataBlock(PLCDataBlock dataBlock)
    public void DeleteDataBlock(int dbNumber)
    public List<PLCDataBlock> LoadAllDataBlocks()
    
    // Variable operations
    public void UpdateVariableValue(int dbNumber, string variableName, object value)
}
```

**Key Features:**
- ✅ Transaction-safe operations
- ✅ Automatic schema creation
- ✅ Indexed queries for performance
- ✅ Type-safe data conversion
- ✅ Error handling and recovery

### 3. PLCDataBlock Class

**Purpose**: Data model for PLC data blocks

```csharp
public class PLCDataBlock
{
    public int Number { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
    public byte[] Data { get; set; }
    public List<PLCVariable> Variables { get; set; }
    public bool IsRegistered { get; set; }
    
    // Methods
    public void AddVariable(string name, string dataType, int offset, string comment)
    public void SetVariableValue(string name, object value)
    public object? GetVariableValue(string name)
}
```

### 4. Snap7 Integration

**Native Library Integration:**
```csharp
// P/Invoke declarations for Snap7 server
[DllImport(S7Consts.Snap7LibName)]
protected static extern IntPtr Srv_Create();

[DllImport(S7Consts.Snap7LibName)]
protected static extern int Srv_Start(IntPtr Server);

[DllImport(S7Consts.Snap7LibName)]
protected static extern int Srv_RegisterArea(IntPtr Server, Int32 AreaCode, 
    Int32 Index, IntPtr pUsrData, Int32 Size);
```

**Server Area Codes:**
- `S7Server.srvAreaDB = 5` - Data blocks
- `S7Server.srvAreaMK = 2` - Memory bits  
- `S7Server.srvAreaPE = 0` - Process inputs
- `S7Server.srvAreaPA = 1` - Process outputs

---

## Database Schema

### Tables Structure

```sql
-- Server configuration
CREATE TABLE ServerConfig (
    Id INTEGER PRIMARY KEY,
    Port INTEGER NOT NULL DEFAULT 102,
    AutoStart BOOLEAN NOT NULL DEFAULT 0,
    ServerName TEXT NOT NULL DEFAULT 'Snap7 Soft PLC',
    LastModified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Data blocks
CREATE TABLE DataBlocks (
    Number INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Size INTEGER NOT NULL,
    Comment TEXT,
    IsRegistered BOOLEAN NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    LastModified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Variables within data blocks
CREATE TABLE Variables (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    DBNumber INTEGER NOT NULL,
    Name TEXT NOT NULL,
    DataType TEXT NOT NULL,
    Offset INTEGER NOT NULL,
    Value TEXT,
    Comment TEXT,
    CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    LastModified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (DBNumber) REFERENCES DataBlocks (Number) ON DELETE CASCADE,
    UNIQUE(DBNumber, Name)
);

-- Indexes for performance
CREATE INDEX idx_variables_db ON Variables(DBNumber);
CREATE INDEX idx_variables_name ON Variables(DBNumber, Name);
```

### Data Types Mapping

| PLC Type | C# Type | SQLite Storage | Size (bytes) |
|----------|---------|----------------|--------------|
| BOOL     | bool    | TEXT          | 1            |
| BYTE     | byte    | TEXT          | 1            |
| WORD     | ushort  | TEXT          | 2            |
| DWORD    | uint    | TEXT          | 4            |
| INT      | short   | TEXT          | 2            |
| DINT     | int     | TEXT          | 4            |
| REAL     | float   | TEXT          | 4            |
| STRING   | string  | TEXT          | 256          |

---

## API Reference

### PLCManager Public Methods

#### Server Management
```csharp
// Start the S7 server on port 102
public bool StartPLC()
// Returns: true if successful, false on error
// Events: StatusChanged with success/error message

// Stop the S7 server
public bool StopPLC()
// Returns: true if successful
// Events: StatusChanged with status message

// Get current server status and client count
public ServerStatusInfo GetServerStatus()
// Returns: ServerStatusInfo with current status
```

#### Data Block Operations
```csharp
// Create new data block
public bool AddDataBlock(int number, int size = 1024, string name = "", string comment = "")
// Parameters:
//   number: DB number (1-65535)
//   size: Memory size in bytes (1-65536)
//   name: Display name (optional)
//   comment: Description (optional)
// Returns: true if created successfully
// Auto-saves to database

// Update existing data block
public bool UpdateDataBlock(int dbNumber, string newName, int newSize, string newComment)
// Features: 
//   - Validates variable compatibility with new size
//   - Re-registers with server if size changed
//   - Preserves existing data when expanding
// Auto-saves to database

// Copy data block with all variables
public bool CopyDataBlock(int sourceDbNumber, int targetDbNumber, string newName = "")
// Features:
//   - Copies all variables and current values
//   - Registers new data block with server
//   - Auto-generates name if not provided
// Auto-saves to database

// Remove data block
public bool RemoveDataBlock(int number)
// Features:
//   - Unregisters from server
//   - Removes from database
//   - Cascades to delete all variables
```

#### Variable Operations
```csharp
// Add variable to data block
public bool AddVariableToDataBlock(int dbNumber, string name, string dataType, 
    int offset, string comment = "")
// Validation:
//   - Name uniqueness within data block
//   - Offset + size fits within data block
//   - No memory overlap with existing variables
// Auto-saves to database

// Update variable properties
public bool UpdateVariable(int dbNumber, string oldName, string newName, 
    string newDataType, int newOffset, string newComment)
// Features:
//   - Full property modification
//   - Validation against conflicts
//   - Value reset on data type change
// Auto-saves to database

// Delete variable
public bool DeleteVariable(int dbNumber, string variableName)
// Features:
//   - Removes from data block
//   - Updates database immediately
// Auto-saves to database

// Update variable value
public bool SetVariableValue(int dbNumber, string variableName, object value)
// Features:
//   - Type-safe value conversion
//   - Updates memory and database
//   - Real-time server synchronization
```

#### Configuration Management
```csharp
// Save all configuration to database
public void SaveConfiguration()
// Saves: All data blocks, variables, and settings

// Load configuration from database
private void LoadConfiguration()
// Called automatically in constructor

// Get server configuration
public (int port, bool autoStart, string serverName) GetConfiguration()

// Set auto-start behavior
public void SetAutoStart(bool autoStart)
```

### Event Handling

#### StatusChanged Event
```csharp
public event EventHandler<string>? StatusChanged;

// Usage:
plcManager.StatusChanged += (sender, message) => {
    Console.WriteLine($"Status: {message}");
};
```

**Event Messages:**
- `"PLC Started Successfully"`
- `"Data Block DB100 added successfully"`
- `"Variable 'MotorStart' added to DB100"`
- `"Configuration loaded: 5 data blocks"`
- `"Error: Failed to register DB100: <reason>"`

---

## Development Setup

### Prerequisites
- **Visual Studio 2022** or **Visual Studio Code**
- **.NET 9.0 SDK**
- **Git** for version control
- **DB Browser for SQLite** (optional, for database inspection)

### Building from Source

1. **Clone Repository**
```bash
git clone <repository-url>
cd SnapServerSoftPLC
```

2. **Restore Dependencies**
```bash
dotnet restore
```

3. **Add Snap7 Library**
   - Download snap7.dll (64-bit) from SourceForge
   - Place in `Native/` folder

4. **Build**
```bash
dotnet build --configuration Release
```

5. **Run**
```bash
dotnet run
```

### Development Environment

**Recommended IDE Settings:**
- **Target Framework**: net9.0-windows
- **Platform Target**: x64 (for snap7.dll compatibility)
- **Nullable**: Enable
- **ImplicitUsings**: Enable

**NuGet Dependencies:**
```xml
<PackageReference Include="Microsoft.Data.Sqlite" Version="9.0.0" />
```

### Code Style Guidelines

**Naming Conventions:**
- **Classes**: PascalCase (`PLCManager`)
- **Methods**: PascalCase (`StartPLC`)
- **Properties**: PascalCase (`IsRunning`)
- **Fields**: camelCase with underscore (`_isRunning`)
- **Constants**: PascalCase (`S7AreaDB`)

**Error Handling:**
```csharp
try
{
    // Operation
    result = server.Start();
    if (result != 0)
    {
        StatusChanged?.Invoke(this, $"Failed: {server.ErrorText(result)}");
        return false;
    }
}
catch (Exception ex)
{
    StatusChanged?.Invoke(this, $"Error: {ex.Message}");
    return false;
}
```

---

## Testing

### Unit Testing Strategy

**Test Categories:**
1. **Data Model Tests** - PLCDataBlock, PLCVariable validation
2. **Database Tests** - CRUD operations, transactions
3. **Server Integration Tests** - Snap7 server operations
4. **UI Tests** - Form behavior, dialog validation

**Example Test Structure:**
```csharp
[TestClass]
public class PLCManagerTests
{
    private PLCManager manager;
    
    [TestInitialize]
    public void Setup()
    {
        manager = new PLCManager();
    }
    
    [TestMethod]
    public void AddDataBlock_ValidInput_ReturnsTrue()
    {
        // Arrange
        int dbNumber = 100;
        int size = 1024;
        
        // Act
        bool result = manager.AddDataBlock(dbNumber, size);
        
        // Assert
        Assert.IsTrue(result);
        Assert.IsNotNull(manager.GetDataBlock(dbNumber));
    }
}
```

### Enhanced Manual Testing Checklist

**Server Operations:**
- ✅ Start/Stop PLC server
- ✅ Network configuration and binding tests
- ✅ Multiple start attempts (should fail gracefully)
- ✅ Client connections and disconnections
- ✅ Server status monitoring
- ✅ Auto-start functionality

**Data Block Management:**
- ✅ Create data blocks (various sizes)
- ✅ Edit data block properties
- ✅ Copy data blocks with variables (including bit offsets)
- ✅ Delete data blocks (with confirmation)
- ✅ Memory usage visualization

**Enhanced Variable Management:**
- ✅ Add variables (all data types)
- ✅ Bit-aware BOOL variable creation
- ✅ Group variable creation with different patterns
- ✅ Edit variable properties (including bit offsets)
- ✅ Update variable values
- ✅ Delete variables
- ✅ Advanced validation (bit conflicts, address optimization)
- ✅ Memory region analysis and suggestions
- ✅ Real-time conflict detection

**Bit-Level Addressing:**
- ✅ BOOL variable bit positioning (0.0 to 7.7 addressing)
- ✅ Conflict detection between BOOL and other variables
- ✅ Sequential bit addressing in group creation
- ✅ Bit usage visualization
- ✅ Auto-offset calculation for optimal placement

**Group Operations:**
- ✅ Mass variable creation (1-1000 variables)
- ✅ Different naming patterns (numbered, indexed, custom)
- ✅ Sequential and individual bit addressing modes
- ✅ Preview validation before creation
- ✅ Transaction rollback on conflicts

**Network Configuration:**
- ✅ Bind address configuration (all interfaces, localhost, specific IP)
- ✅ Port configuration (1-65535)
- ✅ Rack/Slot parameter configuration
- ✅ Network binding test functionality
- ✅ Configuration persistence

**Persistence:**
- ✅ Configuration save/restore
- ✅ Application restart with data recovery
- ✅ Database integrity after power loss

---

## Deployment

### Release Build

1. **Build Release Configuration**
```bash
dotnet publish --configuration Release --self-contained false --output ./publish
```

2. **Include Dependencies**
   - Copy `snap7.dll` to output folder
   - Ensure all runtime dependencies included

3. **Create Installer** (Optional)
   - Use WiX Toolset or similar
   - Include .NET 9.0 Runtime requirement
   - Set up Windows Firewall rules

### Distribution Package

**Required Files:**
```
SnapServerSoftPLC/
├── SnapServerSoftPLC.exe
├── SnapServerSoftPLC.dll
├── Microsoft.Data.Sqlite.dll
├── snap7.dll
├── docs/
│   ├── USER_GUIDE.md
│   └── DEVELOPER_GUIDE.md
└── README.txt
```

### System Requirements

**Minimum:**
- Windows 10 version 1809
- .NET 9.0 Runtime
- 50 MB disk space
- 128 MB RAM

**Recommended:**
- Windows 11
- 4 GB RAM
- SSD storage
- Dedicated network adapter for industrial use

---

## Contributing

### Development Workflow

1. **Fork Repository**
2. **Create Feature Branch**
   ```bash
   git checkout -b feature/new-feature-name
   ```
3. **Implement Changes**
   - Follow code style guidelines
   - Add unit tests
   - Update documentation
4. **Test Thoroughly**
   - Run all existing tests
   - Test new functionality
   - Verify no regressions
5. **Submit Pull Request**
   - Clear description of changes
   - Reference related issues
   - Include screenshots for UI changes

### Code Review Checklist

**Functionality:**
- ✅ Feature works as designed
- ✅ Error handling implemented
- ✅ No memory leaks
- ✅ Thread-safe operations

**Code Quality:**
- ✅ Follows naming conventions
- ✅ Proper error handling
- ✅ Adequate logging
- ✅ Performance considerations

**Documentation:**
- ✅ XML documentation comments
- ✅ Updated user guide
- ✅ API changes documented

### Known Limitations

**Current Constraints:**
- Windows-only (due to snap7.dll dependency)
- Single server instance per application
- Limited to S7 data block areas
- No encryption/authentication
- Maximum 1000 variables per group operation
- Bit addressing limited to BOOL data type

**Recent Enhancements (Latest Version):**
- ✅ **Bit-level addressing** - Precise BOOL variable positioning
- ✅ **Group variable operations** - Mass creation with patterns
- ✅ **Advanced memory management** - Real-time usage visualization
- ✅ **Network configuration** - Flexible binding and S7 parameters
- ✅ **Enhanced validation** - Real-time conflict detection
- ✅ **Memory optimization** - Intelligent address suggestions
- ✅ **Extended database schema** - Bit offset storage

**Future Enhancements:**
- Linux support (with libsnap7.so)
- Multiple server instances
- TLS encryption support
- User authentication
- Web-based configuration interface
- OPC-UA server capability
- Advanced memory defragmentation
- Variable import/export functionality
- Real-time memory mapping visualization

---

## Architecture Decisions

### Why SQLite?
- **Self-contained** - No external database server required
- **ACID compliance** - Transaction safety for configuration
- **Performance** - Fast for typical PLC data sizes
- **Tooling** - Excellent tools for inspection and backup

### Why Windows Forms?
- **Native Windows integration** - Familiar UI paradigms
- **Rapid development** - Visual designer and mature framework
- **Performance** - Direct Win32 API access
- **Deployment** - Single executable possible

### Why P/Invoke for Snap7?
- **Performance** - Direct native library calls
- **Compatibility** - Works with official Snap7 releases
- **Control** - Full access to all Snap7 server features
- **Reliability** - Mature and stable library

---

*This developer guide provides comprehensive technical information for developers working with the Snap7 Soft PLC Server. For usage information, see the User Guide.*