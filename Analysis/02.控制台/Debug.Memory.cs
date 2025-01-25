// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-15 22:12:17
// # Recently: 2024-12-22 20:12:47
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace JFramework
{
    public partial class DebugManager
    {
        private readonly Dictionary<string, MemoryData> memoryData = new Dictionary<string, MemoryData>
        {
            { "已保留的内存总量", new MemoryData(Profiler.GetTotalReservedMemoryLong) },
            { "已分配的内存总量", new MemoryData(Profiler.GetTotalAllocatedMemoryLong) },
            { "未使用的内存总量", new MemoryData(Profiler.GetTotalUnusedReservedMemoryLong) },
            { "图形资源使用内存", new MemoryData(Profiler.GetAllocatedMemoryForGraphicsDriver) },
            { "Mono分配的托管堆", new MemoryData(Profiler.GetMonoHeapSizeLong) },
            { "Mono使用的托管堆", new MemoryData(Profiler.GetMonoUsedSizeLong) },
        };

        private void MemoryWindow()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" 内存信息", Height25);
            GUILayout.EndHorizontal();

            consoleView = GUILayout.BeginScrollView(consoleView, "Box");
            foreach (var data in memoryData)
            {
                GUILayout.Label(data.Key + ":  \t" + data.Value.GetString());
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("垃圾回收", Height30))
            {
                GC.Collect();
            }

            GUILayout.EndHorizontal();
        }
    }
}