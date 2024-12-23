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
    internal partial class DebugManager
    {
        private readonly Dictionary<string, List<Reference>> poolDataInfos = new Dictionary<string, List<Reference>>();
        private Vector2 assemblyView = Vector2.zero;

        private Pool pool = Pool.Heap;
        private Vector2 referenceView = Vector2.zero;

        private void ReferenceWindow()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = pool == Pool.Heap ? Color.white : Color.gray;
            if (GUILayout.Button("Heap", Height30))
            {
                pool = Pool.Heap;
            }

            GUI.contentColor = pool == Pool.Event ? Color.white : Color.gray;
            if (GUILayout.Button("Event", Height30))
            {
                pool = Pool.Event;
            }

            GUI.contentColor = pool == Pool.Pool ? Color.white : Color.gray;
            if (GUILayout.Button("Pool", Height30))
            {
                pool = Pool.Pool;
            }


            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();

            switch (pool)
            {
                case Pool.Event:
                    Draw(Service.Event.Reference(), "事件池", "触发数\t事件数\t添加次数\t移除次数");
                    break;
                case Pool.Heap:
                    Draw(Service.Heap.Reference(), "引用池", "未使用\t使用中\t使用次数\t释放次数");
                    break;
                case Pool.Pool:
                    Draw(Service.Pool.Reference(), "对象池", "未激活\t激活中\t出队次数\t入队次数");

                    break;
            }
        }

        private void Draw(Reference[] objectInfos, string message, string module)
        {
            poolDataInfos.Clear();
            foreach (var poolInfo in objectInfos)
            {
                var assemblyName = Service.Text.Format("{0} - {1}", poolInfo.assetType.Assembly.GetName().Name, message);
                if (!poolDataInfos.TryGetValue(assemblyName, out var results))
                {
                    results = new List<Reference>();
                    poolDataInfos.Add(assemblyName, results);
                }

                results.Add(poolInfo);
            }

            GUILayout.BeginHorizontal();
            assemblyView = GUILayout.BeginScrollView(assemblyView, "Box", BoxWidth);
            foreach (var poolData in poolDataInfos)
            {
                poolData.Value.Sort(Comparison);
                GUILayout.BeginVertical("Box");
                GUILayout.Label(poolData.Key, Height20);
                foreach (var data in poolData.Value)
                {
                    if (string.IsNullOrEmpty(data.assetPath))
                    {
                        GUILayout.Label(Service.Text.Format("{0}", data.assetType.Name), Height20);
                    }
                    else
                    {
                        GUILayout.Label(Service.Text.Format("{0} - {1}", data.assetType.Name, data.assetPath), Height20);
                    }
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndScrollView();

            referenceView = GUILayout.BeginScrollView(referenceView, "Box");
            foreach (var poolData in poolDataInfos)
            {
                poolData.Value.Sort(Comparison);
                GUILayout.BeginVertical("Box");
                GUILayout.Label(module, Height20);
                foreach (var data in poolData.Value)
                {
                    GUILayout.Label(data.ToString(), Height20);
                }

                GUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        private static int Comparison(Reference origin, Reference target)
        {
            return string.Compare(origin.assetType.Name, target.assetType.Name, StringComparison.Ordinal);
        }
    }
}