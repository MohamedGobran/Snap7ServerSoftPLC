using System;
using System.Collections.Generic;
using System.Linq;

namespace SnapServerSoftPLC
{
    public static class MemoryManager
    {
        public static List<MemoryRegion> GetMemoryMap(PLCDataBlock dataBlock)
        {
            var regions = new List<MemoryRegion>();
            
            if (dataBlock.Variables.Count == 0)
            {
                // Entire block is free
                regions.Add(new MemoryRegion 
                { 
                    StartOffset = 0, 
                    EndOffset = dataBlock.Size, 
                    VariableName = null 
                });
                return regions;
            }

            // Sort variables by offset
            var sortedVars = dataBlock.Variables.OrderBy(v => v.Offset).ToList();
            int currentOffset = 0;

            foreach (var variable in sortedVars)
            {
                int varStart = variable.Offset;
                int varEnd = variable.Offset + variable.GetSize();

                // Add free space before this variable
                if (currentOffset < varStart)
                {
                    regions.Add(new MemoryRegion
                    {
                        StartOffset = currentOffset,
                        EndOffset = varStart,
                        VariableName = null
                    });
                }

                // Add occupied space for this variable
                regions.Add(new MemoryRegion
                {
                    StartOffset = varStart,
                    EndOffset = varEnd,
                    VariableName = variable.Name
                });

                currentOffset = varEnd;
            }

            // Add remaining free space at the end
            if (currentOffset < dataBlock.Size)
            {
                regions.Add(new MemoryRegion
                {
                    StartOffset = currentOffset,
                    EndOffset = dataBlock.Size,
                    VariableName = null
                });
            }

            return regions;
        }

        public static List<MemoryRegion> GetAvailableRegions(PLCDataBlock dataBlock, int requiredSize = 1)
        {
            return GetMemoryMap(dataBlock)
                .Where(r => !r.IsOccupied && r.Size >= requiredSize)
                .ToList();
        }

        public static int GetNextAvailableOffset(PLCDataBlock dataBlock, int requiredSize)
        {
            var availableRegions = GetAvailableRegions(dataBlock, requiredSize);
            return availableRegions.Count > 0 ? availableRegions[0].StartOffset : -1;
        }

        public static bool IsOffsetValid(PLCDataBlock dataBlock, int offset, int size, string? excludeVariableName = null)
        {
            int endOffset = offset + size;
            
            // Check bounds
            if (offset < 0 || endOffset > dataBlock.Size)
                return false;

            // Check for overlaps with existing variables
            return !dataBlock.Variables
                .Where(v => excludeVariableName == null || v.Name != excludeVariableName)
                .Any(v => DoRegionsOverlap(offset, endOffset, v.Offset, v.Offset + v.GetSize()));
        }

        private static bool DoRegionsOverlap(int start1, int end1, int start2, int end2)
        {
            return start1 < end2 && start2 < end1;
        }

        public static string GetMemoryUsageInfo(PLCDataBlock dataBlock)
        {
            var regions = GetMemoryMap(dataBlock);
            int usedBytes = regions.Where(r => r.IsOccupied).Sum(r => r.Size);
            int freeBytes = dataBlock.Size - usedBytes;
            double usagePercent = (double)usedBytes / dataBlock.Size * 100;
            
            return $"Used: {usedBytes}/{dataBlock.Size} bytes ({usagePercent:F1}%), Free: {freeBytes} bytes";
        }
    }
}