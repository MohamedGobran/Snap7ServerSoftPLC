# Snap7 Soft PLC Server

A professional Windows desktop application that emulates a Siemens S7 PLC, providing full compatibility with S7 protocol clients for industrial automation development, testing, and simulation.

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)
![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)
![Status](https://img.shields.io/badge/status-Active-green.svg)

## 🚀 Overview

The Snap7 Soft PLC Server transforms your Windows computer into a virtual Siemens S7 PLC that can accept connections from industrial automation software, SCADA systems, and development tools like TIA Portal and WinCC. Perfect for testing, development, training, and simulation without requiring physical PLC hardware.

### ✨ Key Features

- **🔧 S7 Server Emulation** - Full compatibility with S7 protocol clients
- **📊 Advanced Data Block Management** - Create, edit, copy, and manage PLC data blocks
- **🎯 Bit-Level Addressing** - Precise BOOL variable positioning with intelligent conflict detection
- **📦 Group Variable Operations** - Mass creation of variables with flexible naming patterns
- **🌐 Network Configuration** - Advanced network binding and server parameter configuration
- **💾 Persistent Storage** - SQLite database for reliable configuration persistence
- **📱 Professional UI** - Intuitive Windows Forms interface with real-time monitoring
- **⚡ Real-time Validation** - Immediate feedback and error prevention
- **🔍 Memory Optimization** - Intelligent address suggestions and usage visualization

## 📁 Solution Structure

```
Snap7Server/
├── SnapServerSoftPLC.sln              # Visual Studio solution file
└── SnapServerSoftPLC/
    ├── SnapServerSoftPLC/             # Main application project
    │   ├── Core Components/
    │   │   ├── PLCManager.cs          # Main business logic controller
    │   │   ├── PLCDatabase.cs         # SQLite data access layer
    │   │   ├── PLCDataBlock.cs        # Data models and structures
    │   │   ├── BitAddressingHelper.cs # Bit-level addressing operations
    │   │   ├── MemoryManager.cs       # Memory optimization utilities
    │   │   └── Snap7.cs               # Native Snap7 library wrapper
    │   ├── UI Components/
    │   │   ├── Form1.cs               # Main application window
    │   │   ├── *Dialog.cs             # Various configuration dialogs
    │   │   ├── BitAware*Dialog.cs     # Advanced bit addressing dialogs
    │   │   ├── GroupAddVariableDialog.cs  # Group variable creation
    │   │   └── NetworkConfigDialog.cs     # Network configuration
    │   ├── docs/                      # 📚 Documentation
    │   │   ├── USER_GUIDE.md          # Complete user documentation
    │   │   └── DEVELOPER_GUIDE.md     # Technical developer reference
    │   └── Native/
    │       ├── snap7.dll              # Snap7 native library (64-bit)
    │       └── README.txt             # Native library information
    └── Shrp7Client/                   # Sample S7 client project
        └── [Client implementation files]
```

## 📖 Documentation

### 📋 For Users
**[📖 User Guide](SnapServerSoftPLC/SnapServerSoftPLC/docs/USER_GUIDE.md)** - Complete guide covering:
- Installation and setup instructions
- User interface walkthrough
- Data block and variable management
- Advanced features (bit addressing, group operations, network configuration)
- Client connection examples
- Troubleshooting and performance tips

### 👨‍💻 For Developers
**[🔧 Developer Guide](SnapServerSoftPLC/SnapServerSoftPLC/docs/DEVELOPER_GUIDE.md)** - Technical documentation including:
- System architecture and design patterns
- Core component API reference
- Database schema and data models
- Development setup and build instructions
- Testing strategies and validation
- Contributing guidelines and code standards

## 🚀 Quick Start

### Prerequisites
- Windows 10/11 (64-bit recommended)
- .NET 9.0 Runtime
- Administrative privileges (for network access)

### Installation
1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Snap7Server
   ```

2. **Download Snap7 Library**
   - Visit [Snap7 SourceForge](https://sourceforge.net/projects/snap7/)
   - Download latest version and extract `snap7.dll` (64-bit)
   - Place in `SnapServerSoftPLC/SnapServerSoftPLC/Native/` folder

3. **Build and Run**
   ```bash
   dotnet restore
   dotnet build --configuration Release
   dotnet run --project SnapServerSoftPLC/SnapServerSoftPLC
   ```

4. **Configure Windows Firewall**
   - Allow application through Windows Firewall
   - Ensure port 102 is open for TCP connections

### First Use
1. **Start the Application** - Launch SnapServerSoftPLC.exe
2. **Create Data Block** - Click "Add DB" to create your first data block
3. **Add Variables** - Use "Add Variable" with various data types and addressing modes
4. **Start PLC Server** - Click "Start PLC" to begin accepting client connections
5. **Connect Clients** - Use your IP address and port 102 for S7 client connections

## 🔧 Advanced Features

### Bit-Level Addressing
- **Precise BOOL Positioning** - Address BOOL variables to specific bits (e.g., 5.3)
- **Conflict Detection** - Real-time validation prevents memory overlaps
- **Auto-Placement** - Intelligent suggestions for optimal bit positioning

### Group Variable Operations  
- **Mass Creation** - Create up to 1000 variables simultaneously
- **Naming Patterns** - Flexible naming with numbered, indexed, or custom patterns
- **Sequential Addressing** - Automatic address calculation for groups

### Network Configuration
- **Flexible Binding** - Configure specific IP addresses or bind to all interfaces
- **S7 Parameters** - Rack/Slot configuration for precise S7 compatibility
- **Connection Testing** - Pre-flight network configuration validation

## 🏗️ Projects

### SnapServerSoftPLC (Main Application)
The core soft PLC server application featuring:
- **Windows Forms UI** with professional design and real-time monitoring
- **SQLite Database** for persistent configuration storage
- **Snap7 Integration** via P/Invoke for native performance
- **Advanced Memory Management** with bit-level addressing support
- **Network Configuration** with flexible binding options

### Shrp7Client (Sample Client)
A demonstration S7 client application showing:
- **S7 Connection Setup** with proper configuration
- **Data Read/Write Operations** to the soft PLC
- **Error Handling** and connection management examples
- **Integration Patterns** for industrial applications

## 🤝 Contributing

We welcome contributions! Please see our [Developer Guide](SnapServerSoftPLC/SnapServerSoftPLC/docs/DEVELOPER_GUIDE.md) for:
- Development workflow and standards
- Code style guidelines
- Testing requirements
- Pull request process

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

- **📖 Documentation**: Check the [User Guide](SnapServerSoftPLC/SnapServerSoftPLC/docs/USER_GUIDE.md) and [Developer Guide](SnapServerSoftPLC/SnapServerSoftPLC/docs/DEVELOPER_GUIDE.md)
- **🐛 Issues**: Report bugs or request features via GitHub Issues
- **💡 Discussions**: Join community discussions for help and ideas

## 🏷️ Version History

### Latest Release
- ✅ **Bit-Level Addressing** - Precise BOOL variable positioning
- ✅ **Group Variable Operations** - Mass creation with naming patterns  
- ✅ **Enhanced Network Configuration** - Flexible binding and S7 parameters
- ✅ **Advanced Memory Management** - Real-time usage visualization and optimization
- ✅ **Enhanced Validation** - Real-time conflict detection and prevention

---

**Ready to simulate industrial automation without physical hardware? Get started with the [User Guide](SnapServerSoftPLC/SnapServerSoftPLC/docs/USER_GUIDE.md) today!** 🚀