using System;
using UnityEngine;
using UnityEngine.Profiling;

namespace JFramework
{
    internal class DebugMemory
    {
        private readonly DebugSetting setting;
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
        public DebugMemory(DebugSetting setting) => this.setting = setting;
        public void ExtendMemoryGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(setting.GetData("Memory Information"), DebugStyle.Label,  DebugStyle.MinHeight);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginVertical(DebugStyle.Box, GUILayout.Height(setting.MaxHeight - 260));
            long memory = Profiler.GetTotalReservedMemoryLong() / 1000000;
            if (memory > maxTotalReservedMemory) maxTotalReservedMemory = memory;
            if (memory < minTotalReservedMemory) minTotalReservedMemory = memory;
            GUILayout.Label(setting.GetData("Total Memory") + ": " + memory + "MB        " + "[" + setting.GetData("Min") + ": " + minTotalReservedMemory + "  " + setting.GetData("Max") + ": " + maxTotalReservedMemory + "]", DebugStyle.Label);
            memory = Profiler.GetTotalAllocatedMemoryLong() / 1000000;
            if (memory > maxTotalAllocatedMemory) maxTotalAllocatedMemory = memory;
            if (memory < minTotalAllocatedMemory) minTotalAllocatedMemory = memory;
            GUILayout.Label(setting.GetData("Used Memory") + ": " + memory + "MB        " + "[" + setting.GetData("Min") + ": " + minTotalAllocatedMemory + "  " + setting.GetData("Max") + ": " + maxTotalAllocatedMemory + "]", DebugStyle.Label);
            memory = Profiler.GetTotalUnusedReservedMemoryLong() / 1000000;
            if (memory > maxTotalUnusedReservedMemory) maxTotalUnusedReservedMemory = memory;
            if (memory < minTotalUnusedReservedMemory) minTotalUnusedReservedMemory = memory;
            GUILayout.Label(setting.GetData("Free Memory") + ": " + memory + "MB        " + "[" + setting.GetData("Min") + ": " + minTotalUnusedReservedMemory + "  " + setting.GetData("Max") + ": " + maxTotalUnusedReservedMemory + "]", DebugStyle.Label);
            memory = Profiler.GetMonoHeapSizeLong() / 1000000;
            if (memory > maxMonoHeapSize) maxMonoHeapSize = memory;
            if (memory < minMonoHeapSize) minMonoHeapSize = memory;
            GUILayout.Label(setting.GetData("Total Mono Memory") + ": " + memory + "MB        " + "[" + setting.GetData("Min") + ": " + minMonoHeapSize + "  " + setting.GetData("Max") + ": " + maxMonoHeapSize + "]", DebugStyle.Label);
            memory = Profiler.GetMonoUsedSizeLong() / 1000000;
            if (memory > maxMonoUsedSize) maxMonoUsedSize = memory;
            if (memory < minMonoUsedSize) minMonoUsedSize = memory;
            GUILayout.Label(setting.GetData("Used Mono Memory") + ": " + memory + "MB        " + "[" + setting.GetData("Min") + ": " + minMonoUsedSize + "  " + setting.GetData("Max") + ": " + maxMonoUsedSize + "]", DebugStyle.Label);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(setting.GetData("Uninstall unused resources"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                Resources.UnloadUnusedAssets();
            }

            if (GUILayout.Button(setting.GetData("Garbage Collection"), DebugStyle.Button, DebugStyle.MinHeight))
            {
                GC.Collect();
            }

            GUILayout.EndHorizontal();
        }
    }
}