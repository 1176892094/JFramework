using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace JFramework.Core
{
    internal sealed partial class DebugManager
    {
        private long minTotalReservedMemory = 10000;
        private long maxTotalReservedMemory;
        private long minTotalAllocatedMemory = 10000;
        private long maxTotalAllocatedMemory;
        private long minTotalUnusedReservedMemory = 10000;
        private long maxTotalUnusedReservedMemory;
        private long minMonoHeapSize = 10000;
        private long maxMonoHeapSize;
        private long minMonoUsedSize = 10000;
        private long maxMonoUsedSize;
        
        private void DebugMemory()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(DebugConst.MemoryInformation, DebugData.Label, DebugData.HeightLow);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(DebugData.Box,DebugData. WindowBox);
            long memory = Profiler.GetTotalReservedMemoryLong() / 1000000;
            if (memory > maxTotalReservedMemory) maxTotalReservedMemory = memory;
            if (memory < minTotalReservedMemory) minTotalReservedMemory = memory;
            GUILayout.Label(DebugConst.TotalMemory + ": " + memory + "MB        " + "[" + DebugConst.Min + ": " + minTotalReservedMemory + "  " + DebugConst.Max + ": " + maxTotalReservedMemory + "]", DebugData.Label);
            memory = Profiler.GetTotalAllocatedMemoryLong() / 1000000;
            if (memory > maxTotalAllocatedMemory) maxTotalAllocatedMemory = memory;
            if (memory < minTotalAllocatedMemory) minTotalAllocatedMemory = memory;
            GUILayout.Label(DebugConst.UsedMemory + ": " + memory + "MB        " + "[" + DebugConst.Min + ": " + minTotalAllocatedMemory + "  " + DebugConst.Max + ": " + maxTotalAllocatedMemory + "]", DebugData.Label);
            memory = Profiler.GetTotalUnusedReservedMemoryLong() / 1000000;
            if (memory > maxTotalUnusedReservedMemory) maxTotalUnusedReservedMemory = memory;
            if (memory < minTotalUnusedReservedMemory) minTotalUnusedReservedMemory = memory;
            GUILayout.Label(DebugConst.FreeMemory + ": " + memory + "MB        " + "[" + DebugConst.Min + ": " + minTotalUnusedReservedMemory + "  " + DebugConst.Max + ": " + maxTotalUnusedReservedMemory + "]", DebugData.Label);
            memory = Profiler.GetMonoHeapSizeLong() / 1000000;
            if (memory > maxMonoHeapSize) maxMonoHeapSize = memory;
            if (memory < minMonoHeapSize) minMonoHeapSize = memory;
            GUILayout.Label(DebugConst.TotalMonoMemory + ": " + memory + "MB        " + "[" + DebugConst.Min + ": " + minMonoHeapSize + "  " + DebugConst.Max + ": " + maxMonoHeapSize + "]", DebugData.Label);
            memory = Profiler.GetMonoUsedSizeLong() / 1000000;
            if (memory > maxMonoUsedSize) maxMonoUsedSize = memory;
            if (memory < minMonoUsedSize) minMonoUsedSize = memory;
            GUILayout.Label(DebugConst.UsedMonoMemory+ ": " + memory + "MB        " + "[" + DebugConst.Min + ": " + minMonoUsedSize + "  " + DebugConst.Max + ": " + maxMonoUsedSize + "]", DebugData.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
      
            if (GUILayout.Button(DebugConst.GarbageCollections, DebugData.Button, DebugData.Height))
            {
                GC.Collect();
            }

            GUILayout.EndHorizontal();
        }
    }
}