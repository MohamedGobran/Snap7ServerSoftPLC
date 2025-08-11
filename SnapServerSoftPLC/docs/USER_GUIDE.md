# Snap7 Soft PLC Server - User Guide

## Table of Contents
1. [Overview](#overview)
2. [Installation](#installation)
3. [Getting Started](#getting-started)
4. [User Interface](#user-interface)
5. [Data Block Management](#data-block-management)
6. [Variable Management](#variable-management)
7. [Client Connection](#client-connection)
8. [Configuration Persistence](#configuration-persistence)
9. [Troubleshooting](#troubleshooting)

---

## Overview

The Snap7 Soft PLC Server is a Windows desktop application that emulates a Siemens S7 PLC. It acts as a virtual PLC that can accept connections from S7 clients such as TIA Portal, WinCC, SCADA systems, and other industrial automation software.

### Key Features
- ✅ **S7 Server Emulation** - Full compatibility with S7 protocol clients
- ✅ **Data Block Management** - Create, edit, copy, and delete data blocks
- ✅ **Advanced Variable Management** - Define variables with different data types
- ✅ **Bit-Level Addressing** - Precise BOOL variable positioning with bit addressing
- ✅ **Group Variable Creation** - Mass creation of variables with custom naming patterns
- ✅ **Network Configuration** - Advanced network binding and server settings
- ✅ **Memory Optimization** - Intelligent memory usage and conflict detection
- ✅ **Real-time Monitoring** - View client connections and server status
- ✅ **Persistent Configuration** - SQLite database for configuration storage
- ✅ **Professional UI** - Intuitive Windows Forms interface

---

## Installation

### Prerequisites
- Windows 10/11 (64-bit recommended)
- .NET 9.0 Runtime
- Administrative privileges (for network access)

### Steps
1. **Download** the application executable
2. **Extract** to desired folder (e.g., `C:\Program Files\Snap7SoftPLC\`)
3. **Download Snap7 Library**:
   - Visit https://sourceforge.net/projects/snap7/
   - Download latest version (e.g., `snap7-full-1.4.2.7z`)
   - Extract and copy `snap7.dll` (64-bit) from `build\bin\win64\` to application folder
4. **Configure Windows Firewall**:
   - Allow application through firewall
   - Ensure port 102 is open for TCP connections

---

## Getting Started

### First Launch
1. **Start** the application (`SnapServerSoftPLC.exe`)
2. The application will create a configuration database (`plc_config.db`)
3. **Configure** your first data block (see Data Block Management)
4. **Start the PLC** by clicking the green "Start PLC" button

### Quick Setup Example
1. Click **"Start PLC"** to start the server
2. Click **"Add DB"** to create your first data block
   - DB Number: `100`
   - Name: `Motors`
   - Size: `1024` bytes
3. Click **"Add Variable"** to add variables:
   - Name: `MotorStart`, Type: `BOOL`, Offset: `0`
   - Name: `MotorSpeed`, Type: `REAL`, Offset: `4`
4. Your soft PLC is now ready for client connections!

---

## User Interface

### Main Window Layout

```
┌─────────────────────────────────────────────────────────────────────┐
│ [Start PLC] [Stop PLC]    │ Server Status                           │
│                           │ Status: Running ● Clients: 2           │
├───────────────────────────┼─────────────────────────────────────────┤
│ Data Blocks               │ Variables                               │
│ ┌─────────────────────────┐ │ ┌─────────────────────────────────────┐ │
│ │ DB100 - Motors [1024] ✓ │ │ │ Name     │Type │Offset│Value│Comment│ │
│ │ DB200 - Sensors [512] ✓ │ │ │ MotorStart│BOOL │  0   │false│Start  │ │
│ │ DB300 - Outputs [256] ✓ │ │ │ MotorSpeed│REAL │  4   │ 0.0 │RPM    │ │
│ └─────────────────────────┘ │ └─────────────────────────────────────┘ │
│ [Add DB][Remove][Edit][Copy] │ [Add Var][Update][Edit][Delete]         │
├─────────────────────────────┴─────────────────────────────────────────┤
│ Log                                                                   │
│ [19:34:00] PLC Started Successfully                                   │
│ [19:34:24] Data Block DB100 registered successfully                  │
│ [19:35:15] Client connected from 192.168.1.50                       │
└─────────────────────────────────────────────────────────────────────┘
```

### Control Elements

**Server Controls:**
- 🟢 **Start PLC** - Start the S7 server (Port 102)
- 🔴 **Stop PLC** - Stop the S7 server
- 📊 **Status Panel** - Shows server status and connected clients

**Data Block Controls:**
- ➕ **Add DB** - Create new data block
- ❌ **Remove DB** - Delete selected data block
- ✏️ **Edit DB** - Modify data block properties
- 📋 **Copy DB** - Duplicate data block with variables

**Variable Controls:**
- ➕ **Add Variable** - Create new variable in selected data block
- ➕ **Add Group** - Create multiple variables at once with patterns
- 🎯 **Bit-Aware Add** - Add BOOL variables with precise bit addressing
- 🔄 **Update Value** - Change variable value
- ✏️ **Edit Variable** - Modify variable properties
- 🗑️ **Delete Variable** - Remove variable from data block

---

## Data Block Management

### Creating Data Blocks

1. **Click "Add DB"** button
2. **Enter Details**:
   - **DB Number**: Unique identifier (1-65535)
   - **Name**: Descriptive name (e.g., "Motors", "Sensors")
   - **Size**: Memory size in bytes (1-65536)
   - **Comment**: Optional description
3. **Click "OK"** to create

### Editing Data Blocks

1. **Select data block** from list
2. **Click "Edit DB"** button
3. **Modify properties**:
   - Change name or comment
   - Resize data block (⚠️ warning if reducing size)
4. **Click "OK"** to save changes

### Copying Data Blocks

1. **Select source data block**
2. **Click "Copy DB"** button
3. **Specify**:
   - **Target DB Number**: New unique number
   - **New Name**: Name for copied data block
4. **Click "Copy"** - All variables and values are duplicated

### Data Block Status Indicators
- ✅ **Green checkmark** - Successfully registered with server
- ❌ **Red X** - Registration failed (check log for details)

---

## Variable Management

### Supported Data Types
- **BOOL** - Boolean (1 bit) - `true`/`false` - Supports precise bit addressing (e.g., 5.3)
- **BYTE** - Unsigned 8-bit (1 byte) - `0-255`
- **WORD** - Unsigned 16-bit (2 bytes) - `0-65535`
- **DWORD** - Unsigned 32-bit (4 bytes) - `0-4294967295`
- **INT** - Signed 16-bit (2 bytes) - `-32768 to 32767`
- **DINT** - Signed 32-bit (4 bytes) - `-2147483648 to 2147483647`
- **REAL** - 32-bit float (4 bytes) - Decimal numbers
- **STRING** - Text string (256 bytes) - Text data

### Adding Variables

#### Single Variable Addition
1. **Select data block** first
2. **Click "Add Variable"** button
3. **Enter Details**:
   - **Name**: Variable identifier (e.g., "MotorStart")
   - **Data Type**: Select from dropdown
   - **Offset**: Byte position in data block (must not overlap)
   - **Bit Offset**: For BOOL variables only (0-7)
   - **Comment**: Optional description
4. **Click "Auto"** to automatically find next available address
5. **Click "OK"** to create

#### Bit-Aware Variable Addition
1. **Select data block** first
2. **Click "Add Variable"** → **"Bit-Aware Add"**
3. **Advanced Features**:
   - **Real-time conflict detection** - Shows conflicts as you type
   - **Available address suggestions** - Lists all free addresses
   - **Byte usage visualization** - See which bits are occupied
   - **Auto-offset calculation** - Automatically finds optimal placement
4. **Address Display**: Shows full address (e.g., "5.3" for byte 5, bit 3)
5. **Validation**: Real-time validation with error messages

#### Group Variable Addition
1. **Select data block** first
2. **Click "Add Variable"** → **"Add Group"**
3. **Configure Group**:
   - **Base Name**: Starting name for variables
   - **Data Type**: Type for all variables in group
   - **Count**: Number of variables to create (1-1000)
   - **Start Offset**: Starting memory address
   - **Auto-calculate offset**: Let system find optimal placement
4. **Naming Patterns**:
   - **Numbered**: Var1, Var2, Var3...
   - **Indexed**: Var[0], Var[1], Var[2]...
   - **Custom Pattern**: Use {BaseName}_{Index} or {BaseName}{Index1}
5. **BOOL-Specific Options**:
   - **Sequential bits**: Pack bits efficiently (0.0, 0.1, 0.2...)
   - **Separate bytes**: Each BOOL gets own byte (0.0, 1.0, 2.0...)
6. **Preview**: See all variables before creating
7. **Validation**: Ensures no conflicts with existing variables

### Editing Variables

1. **Select data block and variable**
2. **Click "Edit Variable"** button
3. **Modify properties**:
   - Change name, type, offset, or comment
   - System validates against conflicts and size limits
4. **Click "OK"** to save

### Updating Variable Values

1. **Select variable** in the list
2. **Click "Update Value"** button
3. **Enter new value**:
   - BOOL: Checkbox for true/false
   - Numbers: Enter numeric value
   - String: Enter text
4. **Click "OK"** to update

### Variable Validation
- ✅ **Name uniqueness** - No duplicate names in same data block
- ✅ **Address validation** - No overlapping memory regions
- ✅ **Bit-level conflict detection** - Prevents BOOL variable conflicts
- ✅ **Size checking** - Variables must fit within data block
- ✅ **Type safety** - Values validated against data type
- ✅ **Real-time feedback** - Immediate validation with descriptive messages
- ✅ **Memory optimization** - Suggests optimal placement for variables

---

## Client Connection

### Connection Information
**Clients connect using these parameters:**
- **IP Address**: Your computer's IP address (e.g., `192.168.1.100`)
- **Port**: `102` (standard S7 port)
- **Rack**: `0` (default)
- **Slot**: `1` or `2` (common values)

### Finding Your IP Address
1. **Windows**: Run `ipconfig` in Command Prompt
2. **Look for**: "IPv4 Address" under your network adapter
3. **Example**: `192.168.1.100`

### Client Examples

**TIA Portal:**
```
Connection Type: S7 Connection
IP Address: 192.168.1.100
Rack: 0
Slot: 1
```

**WinCC/SCADA:**
```
Driver: Siemens S7 TCP/IP
Station: 192.168.1.100:102
PLC Type: S7-300/400/1200/1500
```

**Python (python-snap7):**
```python
import snap7
client = snap7.client.Client()
client.connect('192.168.1.100', 0, 1)
```

### Connection Monitoring
- **Client Counter** - Shows active connections in status panel
- **Connection Log** - All connections logged with timestamps
- **Real-time Updates** - Status updates every second

---

## Configuration Persistence

### Automatic Saving
- **SQLite Database** - All configuration stored in `plc_config.db`
- **Auto-save** - Changes automatically saved when made
- **Backup** - Simply copy the `.db` file for backup

### What Gets Saved
- ✅ **Server settings** (port, auto-start)
- ✅ **Data blocks** (number, name, size, comment)
- ✅ **Variables** (name, type, offset, current values)
- ✅ **Timestamps** (creation and modification dates)

### Manual Database Access
- **Tool**: DB Browser for SQLite (free download)
- **File**: `plc_config.db` in application folder
- **Tables**: `ServerConfig`, `DataBlocks`, `Variables`

### Configuration Restore
1. **Application restart** - Configuration automatically loaded
2. **Auto-start** - PLC can start automatically if configured
3. **Data blocks** - All blocks and variables restored
4. **Values** - Last saved variable values restored

---

## Troubleshooting

### Common Issues

**❌ PLC Won't Start**
- **Check**: Snap7.dll is in application folder
- **Verify**: Port 102 not used by another application
- **Solution**: Use Task Manager to close conflicting applications

**❌ Clients Can't Connect**
- **Check**: Windows Firewall settings
- **Verify**: Correct IP address and port 102
- **Solution**: Add firewall exception for application

**❌ Data Block Registration Failed**
- **Check**: Log messages for error details
- **Verify**: Data block size is reasonable (≤ 65KB)
- **Solution**: Try smaller data block or restart PLC

**❌ Database Errors**
- **Check**: `plc_config.db` file permissions
- **Verify**: Disk space available
- **Solution**: Run as administrator or move to user folder

### Performance Tips
- 🎯 **Limit data block size** to 8KB or less for best performance
- 🎯 **Use appropriate data types** - BOOL for simple flags, REAL for decimal values
- 🎯 **Optimize BOOL placement** - Use sequential bit addressing for efficiency
- 🎯 **Group similar variables** - Use group creation for better memory layout
- 🎯 **Monitor memory usage** - Check bit and byte usage displays
- 🎯 **Organize variables** by offset to avoid gaps in memory
- 🎯 **Monitor client count** - Too many clients can slow performance

### Log Analysis
**Success Messages:**
- `PLC Started Successfully` - Server running
- `Data Block DB100 registered successfully` - DB ready for clients
- `Configuration loaded: X data blocks` - Startup successful

**Warning Messages:**
- `Failed to register DB100: Unknown Area code` - Check snap7.dll
- `Variable 'X' already exists` - Duplicate variable names
- `Warning: Failed to save to database` - Database permission issues

**Error Messages:**
- `Failed to start PLC: Address already in use` - Port 102 busy
- `Failed to bind to IP address` - Invalid network configuration
- `Error loading configuration` - Database corruption
- `Cannot resize DB100: Variables exceed new size` - Size conflict
- `Address X.Y conflicts with variable` - Bit-level addressing conflict
- `No available bit addresses` - Data block memory full
- `Variable name already exists` - Duplicate variable names

### Getting Help
- **Application Log** - Check log panel for detailed error messages
- **Memory Usage Display** - Real-time memory and bit usage information
- **Address Validation** - Use bit-aware dialogs for conflict detection
- **Network Configuration Tool** - Test binding before applying settings
- **Database Tool** - Use DB Browser to inspect saved configuration
- **Network Tools** - Use `netstat -an | find "102"` to check port usage
- **Windows Event Viewer** - Check for system-level errors

---

## Advanced Features

### Network Configuration
- **Advanced Network Dialog** - Configure server binding and connection parameters
- **Bind Address Options**:
  - `0.0.0.0` - All network interfaces (default)
  - `127.0.0.1` - Localhost only
  - Specific IP addresses - Bind to particular network adapter
- **Connection Parameters**:
  - **Port**: Default 102, configurable 1-65535
  - **Rack/Slot**: S7 addressing parameters (Rack 0, Slot 1/2)
  - **Server Name**: Custom identification string
- **Auto-Start**: Automatic server startup when application launches
- **Binding Test**: Test network configuration before applying

### Bit-Level Memory Management
- **Intelligent Bit Addressing** - Optimal placement of BOOL variables
- **Memory Usage Visualization** - Real-time display of byte and bit usage
- **Conflict Prevention** - Advanced validation prevents memory overlaps
- **Address Optimization** - Automatic suggestions for efficient memory use
- **Multi-level Validation** - Byte-level and bit-level conflict detection

### Group Operations
- **Mass Variable Creation** - Create hundreds of variables efficiently
- **Flexible Naming Patterns** - Multiple naming conventions supported
- **Sequential Addressing** - Automatic address calculation
- **Custom Patterns** - User-defined naming with placeholders
- **Preview and Validation** - See results before committing changes

### Auto-Start Configuration
- Configure PLC to start automatically when application launches
- Useful for production environments
- Setting saved in database and network configuration

### Network Security
- **Configurable Binding** - Control which interfaces accept connections
- **IP Address Validation** - Ensures valid network configuration
- **Port Configuration** - Non-standard ports for security
- Consider network segmentation for industrial environments
- Use Windows Firewall to restrict client connections

### Data Backup Strategy
- **Regular backups** of `plc_config.db`
- **Export configurations** using database tools
- **Version control** for configuration changes
- **Memory layout preservation** - Bit-level addressing maintained across backups

---

*This user guide covers the essential features of the Snap7 Soft PLC Server. For technical details and development information, see the Developer Documentation.*