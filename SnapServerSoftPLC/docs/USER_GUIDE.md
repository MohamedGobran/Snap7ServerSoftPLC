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
- ✅ **Variable Management** - Define variables with different data types
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
- **BOOL** - Boolean (1 byte) - `true`/`false`
- **BYTE** - Unsigned 8-bit (1 byte) - `0-255`
- **WORD** - Unsigned 16-bit (2 bytes) - `0-65535`
- **DWORD** - Unsigned 32-bit (4 bytes) - `0-4294967295`
- **INT** - Signed 16-bit (2 bytes) - `-32768 to 32767`
- **DINT** - Signed 32-bit (4 bytes) - `-2147483648 to 2147483647`
- **REAL** - 32-bit float (4 bytes) - Decimal numbers
- **STRING** - Text string (256 bytes) - Text data

### Adding Variables

1. **Select data block** first
2. **Click "Add Variable"** button
3. **Enter Details**:
   - **Name**: Variable identifier (e.g., "MotorStart")
   - **Data Type**: Select from dropdown
   - **Offset**: Byte position in data block (must not overlap)
   - **Comment**: Optional description
4. **Click "OK"** to create

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
- ✅ **Offset validation** - No overlapping memory regions
- ✅ **Size checking** - Variables must fit within data block
- ✅ **Type safety** - Values validated against data type

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
- `Error loading configuration` - Database corruption
- `Cannot resize DB100: Variables exceed new size` - Size conflict

### Getting Help
- **Application Log** - Check log panel for detailed error messages
- **Database Tool** - Use DB Browser to inspect saved configuration
- **Network Tools** - Use `netstat -an | find "102"` to check port usage
- **Windows Event Viewer** - Check for system-level errors

---

## Advanced Features

### Auto-Start Configuration
- Configure PLC to start automatically when application launches
- Useful for production environments
- Setting saved in database

### Network Security
- Application binds to all network interfaces (0.0.0.0:102)
- Consider network segmentation for industrial environments
- Use Windows Firewall to restrict client connections

### Data Backup Strategy
- **Regular backups** of `plc_config.db`
- **Export configurations** using database tools
- **Version control** for configuration changes

---

*This user guide covers the essential features of the Snap7 Soft PLC Server. For technical details and development information, see the Developer Documentation.*