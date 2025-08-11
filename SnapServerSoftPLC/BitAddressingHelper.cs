using System;
using System.Collections.Generic;
using System.Linq;

namespace SnapServerSoftPLC
{
    /// <summary>
    /// Helper class for bit-level addressing operations in PLC data blocks
    /// </summary>
    public static class BitAddressingHelper
    {
        /// <summary>
        /// Finds the next available bit address for a BOOL variable
        /// </summary>
        /// <param name="variables">List of existing variables</param>
        /// <param name="dataBlockSize">Size of the data block</param>
        /// <returns>Tuple of (byteOffset, bitOffset) or (-1, -1) if no space available</returns>
        public static (int byteOffset, int bitOffset) GetNextAvailableBitAddress(List<PLCVariable> variables, int dataBlockSize)
        {
            for (int byteOffset = 0; byteOffset < dataBlockSize; byteOffset++)
            {
                // Check if this byte is completely occupied by non-BOOL variables
                var nonBoolInByte = variables.FirstOrDefault(v => 
                    v.DataType != "BOOL" && 
                    v.Offset <= byteOffset && 
                    v.Offset + v.GetSize() > byteOffset);
                    
                if (nonBoolInByte != null)
                    continue; // Skip this byte as it's occupied by a non-BOOL variable

                // Check each bit in the byte
                for (int bitOffset = 0; bitOffset < 8; bitOffset++)
                {
                    if (IsBitAddressAvailable(variables, byteOffset, bitOffset, dataBlockSize))
                    {
                        return (byteOffset, bitOffset);
                    }
                }
            }
            
            return (-1, -1); // No available bit address found
        }

        /// <summary>
        /// Checks if a specific bit address is available
        /// </summary>
        /// <param name="variables">List of existing variables</param>
        /// <param name="byteOffset">The byte offset to check</param>
        /// <param name="bitOffset">The bit offset (0-7) to check</param>
        /// <param name="dataBlockSize">Size of the data block</param>
        /// <param name="excludeVariableName">Variable name to exclude from conflict check (for editing)</param>
        /// <returns>True if the bit address is available</returns>
        public static bool IsBitAddressAvailable(List<PLCVariable> variables, int byteOffset, int bitOffset, int dataBlockSize, string? excludeVariableName = null)
        {
            // Bounds check
            if (byteOffset < 0 || byteOffset >= dataBlockSize || bitOffset < 0 || bitOffset > 7)
                return false;

            // Check for bit collision with other BOOL variables
            var conflictingBool = variables.FirstOrDefault(v => 
                v.Name != excludeVariableName && 
                v.DataType == "BOOL" && 
                v.Offset == byteOffset && 
                v.BitOffset == bitOffset);
                
            if (conflictingBool != null)
                return false;

            // Check if the byte is occupied by a non-BOOL variable
            var conflictingNonBool = variables.FirstOrDefault(v => 
                v.Name != excludeVariableName && 
                v.DataType != "BOOL" && 
                v.Offset <= byteOffset && 
                v.Offset + v.GetSize() > byteOffset);
                
            return conflictingNonBool == null;
        }

        /// <summary>
        /// Checks if a byte is completely occupied by non-BOOL variables
        /// </summary>
        /// <param name="variables">List of existing variables</param>
        /// <param name="byteOffset">The byte offset to check</param>
        /// <returns>True if the byte is completely occupied</returns>
        public static bool IsByteCompletelyOccupied(List<PLCVariable> variables, int byteOffset)
        {
            // Check if occupied by non-BOOL variable
            var nonBoolVar = variables.FirstOrDefault(v => 
                v.DataType != "BOOL" && 
                v.Offset <= byteOffset && 
                v.Offset + v.GetSize() > byteOffset);
                
            if (nonBoolVar != null)
                return true;

            // Check if all 8 bits are occupied by BOOL variables
            var boolVarsInByte = variables
                .Where(v => v.DataType == "BOOL" && v.Offset == byteOffset)
                .Select(v => v.BitOffset)
                .ToHashSet();
                
            return boolVarsInByte.Count == 8;
        }

        /// <summary>
        /// Gets a list of available bit positions in a specific byte
        /// </summary>
        /// <param name="variables">List of existing variables</param>
        /// <param name="byteOffset">The byte offset to check</param>
        /// <returns>List of available bit positions (0-7)</returns>
        public static List<int> GetAvailableBitsInByte(List<PLCVariable> variables, int byteOffset)
        {
            var availableBits = new List<int>();
            
            // Check if byte is occupied by non-BOOL variable
            var nonBoolVar = variables.FirstOrDefault(v => 
                v.DataType != "BOOL" && 
                v.Offset <= byteOffset && 
                v.Offset + v.GetSize() > byteOffset);
                
            if (nonBoolVar != null)
                return availableBits; // No bits available

            // Find which bits are occupied by BOOL variables
            var occupiedBits = variables
                .Where(v => v.DataType == "BOOL" && v.Offset == byteOffset)
                .Select(v => v.BitOffset)
                .ToHashSet();

            // Return unoccupied bits
            for (int bit = 0; bit < 8; bit++)
            {
                if (!occupiedBits.Contains(bit))
                    availableBits.Add(bit);
            }

            return availableBits;
        }

        /// <summary>
        /// Gets available memory regions for a specific data type and size
        /// </summary>
        /// <param name="variables">List of existing variables</param>
        /// <param name="dataBlockSize">Size of the data block</param>
        /// <param name="requiredSize">Required size in bytes</param>
        /// <returns>List of available memory regions</returns>
        public static List<MemoryRegion> GetAvailableRegions(List<PLCVariable> variables, int dataBlockSize, int requiredSize)
        {
            var regions = new List<MemoryRegion>();
            
            if (requiredSize <= 0)
                return regions;

            for (int offset = 0; offset <= dataBlockSize - requiredSize; offset++)
            {
                if (IsOffsetAvailableForSize(variables, offset, requiredSize))
                {
                    regions.Add(new MemoryRegion
                    {
                        StartOffset = offset,
                        Size = requiredSize,
                        EndOffset = offset + requiredSize
                    });
                }
            }

            return regions;
        }

        /// <summary>
        /// Checks if an offset is available for a variable of specific size
        /// </summary>
        /// <param name="variables">List of existing variables</param>
        /// <param name="offset">Starting offset</param>
        /// <param name="size">Size in bytes</param>
        /// <param name="excludeVariableName">Variable name to exclude from conflict check</param>
        /// <returns>True if the offset is available</returns>
        public static bool IsOffsetAvailableForSize(List<PLCVariable> variables, int offset, int size, string? excludeVariableName = null)
        {
            for (int i = 0; i < size; i++)
            {
                int checkOffset = offset + i;
                
                // Check for BOOL variables in this byte
                var boolVar = variables.FirstOrDefault(v => 
                    v.Name != excludeVariableName && 
                    v.DataType == "BOOL" && 
                    v.Offset == checkOffset);
                    
                if (boolVar != null)
                    return false;
                
                // Check for overlapping non-BOOL variables
                var nonBoolVar = variables.FirstOrDefault(v => 
                    v.Name != excludeVariableName && 
                    v.DataType != "BOOL" &&
                    v.Offset <= checkOffset && 
                    v.Offset + v.GetSize() > checkOffset);
                    
                if (nonBoolVar != null)
                    return false;
            }
            
            return true;
        }

        /// <summary>
        /// Gets the next available byte-aligned offset for a variable of specific size
        /// </summary>
        /// <param name="variables">List of existing variables</param>
        /// <param name="dataBlockSize">Size of the data block</param>
        /// <param name="requiredSize">Required size in bytes</param>
        /// <returns>Next available offset or -1 if no space</returns>
        public static int GetNextAvailableOffset(List<PLCVariable> variables, int dataBlockSize, int requiredSize)
        {
            for (int offset = 0; offset <= dataBlockSize - requiredSize; offset++)
            {
                if (IsOffsetAvailableForSize(variables, offset, requiredSize))
                {
                    return offset;
                }
            }
            
            return -1;
        }

        /// <summary>
        /// Gets a formatted string showing bit usage for a specific byte
        /// </summary>
        /// <param name="variables">List of existing variables</param>
        /// <param name="byteOffset">The byte offset to analyze</param>
        /// <returns>Formatted string showing bit usage</returns>
        public static string GetByteUsageString(List<PLCVariable> variables, int byteOffset)
        {
            // Check for non-BOOL variable occupying this byte
            var nonBoolVar = variables.FirstOrDefault(v => 
                v.DataType != "BOOL" && 
                v.Offset <= byteOffset && 
                v.Offset + v.GetSize() > byteOffset);
                
            if (nonBoolVar != null)
            {
                int relativeOffset = byteOffset - nonBoolVar.Offset;
                return $"Used by {nonBoolVar.DataType} '{nonBoolVar.Name}' (byte {relativeOffset})";
            }

            // Check for BOOL variables
            var boolVars = variables
                .Where(v => v.DataType == "BOOL" && v.Offset == byteOffset)
                .OrderBy(v => v.BitOffset)
                .ToList();
                
            if (boolVars.Any())
            {
                var bitUsage = boolVars.Select(v => $"{v.BitOffset}({v.Name})");
                return $"BITS({string.Join(", ", bitUsage)})";
            }

            return "Available";
        }
    }

    /// <summary>
    /// Represents a memory region in a data block
    /// </summary>
    public class MemoryRegion
    {
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
        public int Size { get; set; }
        public bool IsOccupied { get; set; }
        public string? VariableName { get; set; }
        public string? Description { get; set; }
    }
}