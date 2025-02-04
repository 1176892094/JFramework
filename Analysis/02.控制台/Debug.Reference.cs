// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 21:12:36
// # Recently: 2024-12-22 20:12:31
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    public partial class DebugManager
    {
        private readonly Dictionary<string, List<Reference>> poolData = new Dictionary<string, List<Reference>>();
        private Pool option = Pool.Heap;


        private void ReferenceWindow()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = option == Pool.Heap ? Color.white : Color.gray;
            if (GUILayout.Button("Heap", Height30))
            {
                option = Pool.Heap;
            }

            GUI.contentColor = option == Pool.Event ? Color.white : Color.gray;
            if (GUILayout.Button("Event", Height30))
            {
                option = Pool.Event;
            }

            GUI.contentColor = option == Pool.Pool ? Color.white : Color.gray;
            if (GUILayout.Button("Pool", Height30))
            {
                option = Pool.Pool;
            }
            
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            switch (option)
            {
                case Pool.Event:
                    Draw(ServiceEventRef.Invoke(), "事件池", "触发数\t事件数\t添加次数\t移除次数");
                    break;
                case Pool.Heap:
                    Draw(ServicePoolRef.Invoke(), "引用池", "未使用\t使用中\t使用次数\t释放次数");
                    break;
                case Pool.Pool:
                    Draw(PoolManagerRef.Invoke(), "对象池", "未激活\t激活中\t出队次数\t入队次数");
                    break;
            }
        }

        private void Draw(Reference[] objectInfos, string message, string module)
        {
            poolData.Clear();
            foreach (var poolInfo in objectInfos)
            {
                var assemblyName = Service.Text.Format("{0} - {1}", poolInfo.assetType.Assembly.GetName().Name, message);
                if (!poolData.TryGetValue(assemblyName, out var results))
                {
                    results = new List<Reference>();
                    poolData.Add(assemblyName, results);
                }

                results.Add(poolInfo);
            }

            consoleView = GUILayout.BeginScrollView(consoleView, "Box");
            foreach (var pool in poolData)
            {
                pool.Value.Sort(Comparison);
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical("Box", PoolWidth);
                GUILayout.Label(pool.Key, Height20);
                foreach (var data in pool.Value)
                {
                    var assetName = data.assetType.Name;
                    if (!string.IsNullOrEmpty(data.assetPath))
                    {
                        assetName = Service.Text.Format("{0} - {1}", data.assetType.Name, data.assetPath);
                    }

                    GUILayout.Label(assetName, Height20);
                }

                GUILayout.EndVertical();

                GUILayout.BeginVertical("Box");
                GUILayout.Label(module, Height20);
                foreach (var data in pool.Value)
                {
                    GUILayout.Label(data.ToString(), Height20);
                }

                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
        }

        private static int Comparison(Reference origin, Reference target)
        {
            return string.Compare(origin.assetType.Name, target.assetType.Name, StringComparison.Ordinal);
        }
    }
}