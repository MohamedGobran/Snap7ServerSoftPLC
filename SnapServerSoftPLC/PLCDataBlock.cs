using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SnapServerSoftPLC
{
    public class PLCVariable
    {
        public string Name { get; set; } = "";
        public string DataType { get; set; } = "BOOL";
        public int Offset { get; set; }
        public int BitOffset { get; set; } = 0; // For BOOL variables: 0-7, ignored for other types
        public object Value { get; set; } = false;
        public string Comment { get; set; } = "";
        
        /// <summary>
        /// Gets the full address string for the variable
        /// </summary>
        public string FullAddress => DataType == "BOOL" ? $"{Offset}.{BitOffset}" : Offset.ToString();
        
        /// <summary>
        /// Gets the size in bytes that this variable occupies
        /// </summary>
        public int GetSize()
        {
            return DataType switch
            {
                "BOOL" => 0, // BOOL variables share bytes, don't count as full bytes
                "BYTE" => 1,
                "WORD" => 2,
                "DWORD" => 4,
                "INT" => 2,
                "DINT" => 4,
                "REAL" => 4,
                "STRING" => 256,
                _ => 1
            };
        }
        
        /// <summary>
        /// Gets the actual memory footprint (for BOOL it returns the byte it occupies)
        /// </summary>
        public int GetMemoryFootprint()
        {
            return DataType == "BOOL" ? 1 : GetSize();
        }
        
        /// <summary>
        /// Validates that the bit offset is valid for the data type
        /// </summary>
        public bool IsValidBitOffset()
        {
            if (DataType != "BOOL") return BitOffset == 0;
            return BitOffset >= 0 && BitOffset <= 7;
        }
        
        /// <summary>
        /// Gets the absolute bit position in the data block
        /// </summary>
        public int GetAbsoluteBitPosition()
        {
            return DataType == "BOOL" ? (Offset * 8) + BitOffset : Offset * 8;
        }
    }

    public class PLCDataBlock
    {
        public int Number { get; set; }
        public string Name { get; set; } = "";
        public int Size { get; set; } = 1024;
        public byte[] Data { get; set; }
        public List<PLCVariable> Variables { get; set; } = new List<PLCVariable>();
        public string Comment { get; set; } = "";
        public bool IsRegistered { get; set; } = false;

        public PLCDataBlock(int number, int size = 1024)
        {
            Number = number;
            Size = size;
            Data = new byte[size];
            Name = $"DB{number}";
        }

        public void AddVariable(string name, string dataType, int offset, string comment = "", int bitOffset = 0)
        {
            var variable = new PLCVariable
            {
                Name = name,
                DataType = dataType,
                Offset = offset,
                BitOffset = dataType == "BOOL" ? bitOffset : 0,
                Comment = comment,
                Value = GetDefaultValue(dataType)
            };
            
            Variables.Add(variable);
        }

        private object GetDefaultValue(string dataType)
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

        public void SetVariableValue(string name, object value)
        {
            var variable = Variables.Find(v => v.Name == name);
            if (variable != null)
            {
                variable.Value = value;
                WriteValueToData(variable);
            }
        }

        public object? GetVariableValue(string name)
        {
            var variable = Variables.Find(v => v.Name == name);
            if (variable != null)
            {
                return ReadValueFromData(variable);
            }
            return null;
        }

        private void WriteValueToData(PLCVariable variable)
        {
            try
            {
                switch (variable.DataType)
                {
                    case "BOOL":
                        WriteBoolToData(variable.Offset, variable.BitOffset, (bool)variable.Value);
                        break;
                    case "BYTE":
                        Data[variable.Offset] = (byte)variable.Value;
                        break;
                    case "WORD":
                        var wordBytes = BitConverter.GetBytes((ushort)variable.Value);
                        if (BitConverter.IsLittleEndian) Array.Reverse(wordBytes);
                        Array.Copy(wordBytes, 0, Data, variable.Offset, 2);
                        break;
                    case "DWORD":
                        var dwordBytes = BitConverter.GetBytes((uint)variable.Value);
                        if (BitConverter.IsLittleEndian) Array.Reverse(dwordBytes);
                        Array.Copy(dwordBytes, 0, Data, variable.Offset, 4);
                        break;
                    case "INT":
                        var intBytes = BitConverter.GetBytes((short)variable.Value);
                        if (BitConverter.IsLittleEndian) Array.Reverse(intBytes);
                        Array.Copy(intBytes, 0, Data, variable.Offset, 2);
                        break;
                    case "DINT":
                        var dintBytes = BitConverter.GetBytes((int)variable.Value);
                        if (BitConverter.IsLittleEndian) Array.Reverse(dintBytes);
                        Array.Copy(dintBytes, 0, Data, variable.Offset, 4);
                        break;
                    case "REAL":
                        var realBytes = BitConverter.GetBytes((float)variable.Value);
                        if (BitConverter.IsLittleEndian) Array.Reverse(realBytes);
                        Array.Copy(realBytes, 0, Data, variable.Offset, 4);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error writing variable {variable.Name}: {ex.Message}");
            }
        }

        private object ReadValueFromData(PLCVariable variable)
        {
            try
            {
                return variable.DataType switch
                {
                    "BOOL" => ReadBoolFromData(variable.Offset, variable.BitOffset),
                    "BYTE" => Data[variable.Offset],
                    "WORD" => ReadBigEndianUInt16(Data, variable.Offset),
                    "DWORD" => ReadBigEndianUInt32(Data, variable.Offset),
                    "INT" => ReadBigEndianInt16(Data, variable.Offset),
                    "DINT" => ReadBigEndianInt32(Data, variable.Offset),
                    "REAL" => ReadBigEndianSingle(Data, variable.Offset),
                    _ => false
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading variable {variable.Name}: {ex.Message}");
                return GetDefaultValue(variable.DataType);
            }
        }

        private ushort ReadBigEndianUInt16(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
            {
                return (ushort)((data[offset] << 8) | data[offset + 1]);
            }
            return BitConverter.ToUInt16(data, offset);
        }

        private uint ReadBigEndianUInt32(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
            {
                return (uint)((data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3]);
            }
            return BitConverter.ToUInt32(data, offset);
        }

        private short ReadBigEndianInt16(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
            {
                return (short)((data[offset] << 8) | data[offset + 1]);
            }
            return BitConverter.ToInt16(data, offset);
        }

        private int ReadBigEndianInt32(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
            {
                return (data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3];
            }
            return BitConverter.ToInt32(data, offset);
        }

        private float ReadBigEndianSingle(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte[] bytes = new byte[4];
                bytes[0] = data[offset + 3];
                bytes[1] = data[offset + 2];
                bytes[2] = data[offset + 1];
                bytes[3] = data[offset];
                return BitConverter.ToSingle(bytes, 0);
            }
            return BitConverter.ToSingle(data, offset);
        }
        
        /// <summary>
        /// Writes a boolean value to a specific bit within a byte
        /// </summary>
        private void WriteBoolToData(int byteOffset, int bitOffset, bool value)
        {
            if (byteOffset >= Data.Length || bitOffset < 0 || bitOffset > 7)
                return;
                
            byte currentByte = Data[byteOffset];
            byte bitMask = (byte)(1 << bitOffset);
            
            if (value)
            {
                Data[byteOffset] = (byte)(currentByte | bitMask);
            }
            else
            {
                Data[byteOffset] = (byte)(currentByte & ~bitMask);
            }
        }
        
        /// <summary>
        /// Reads a boolean value from a specific bit within a byte
        /// </summary>
        private bool ReadBoolFromData(int byteOffset, int bitOffset)
        {
            if (byteOffset >= Data.Length || bitOffset < 0 || bitOffset > 7)
                return false;
                
            byte currentByte = Data[byteOffset];
            byte bitMask = (byte)(1 << bitOffset);
            
            return (currentByte & bitMask) != 0;
        }
        
        /// <summary>
        /// Gets memory usage information for the data block
        /// </summary>
        public string GetMemoryUsageInfo()
        {
            int usedBytes = 0;
            var occupiedBytes = new HashSet<int>();
            
            foreach (var variable in Variables)
            {
                if (variable.DataType == "BOOL")
                {
                    occupiedBytes.Add(variable.Offset);
                }
                else
                {
                    for (int i = 0; i < variable.GetSize(); i++)
                    {
                        occupiedBytes.Add(variable.Offset + i);
                    }
                }
            }
            
            usedBytes = occupiedBytes.Count;
            double percentage = Size > 0 ? (double)usedBytes / Size * 100 : 0;
            
            return $"Memory Usage: {usedBytes}/{Size} bytes ({percentage:F1}%)";
        }
        
        /// <summary>
        /// Gets usage information for a specific byte showing which bits are used
        /// </summary>
        public string GetByteUsageInfo(int byteOffset)
        {
            var boolVarsInByte = Variables
                .Where(v => v.DataType == "BOOL" && v.Offset == byteOffset)
                .OrderBy(v => v.BitOffset)
                .ToList();
                
            if (boolVarsInByte.Any())
            {
                var usedBits = boolVarsInByte.Select(v => $"{v.BitOffset}({v.Name})");
                return $"Byte {byteOffset}: BITS({string.Join(", ", usedBits)})";
            }
            
            var nonBoolVar = Variables.FirstOrDefault(v => 
                v.DataType != "BOOL" && 
                v.Offset <= byteOffset && 
                v.Offset + v.GetSize() > byteOffset);
                
            if (nonBoolVar != null)
            {
                int relativeOffset = byteOffset - nonBoolVar.Offset;
                return $"Byte {byteOffset}: Used by {nonBoolVar.DataType} '{nonBoolVar.Name}' (byte {relativeOffset})";
            }
            
            return $"Byte {byteOffset}: Available";
        }
    }
}