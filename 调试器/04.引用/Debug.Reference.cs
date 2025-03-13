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
        private Pool windowOption = Pool.Heap;


        private void ReferenceWindow()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = windowOption == Pool.Heap ? Color.white : Color.gray;
            if (GUILayout.Button("Heap", GUILayout.Height(30)))
            {
                windowOption = Pool.Heap;
            }

            GUI.contentColor = windowOption == Pool.Event ? Color.white : Color.gray;
            if (GUILayout.Button("Event", GUILayout.Height(30)))
            {
                windowOption = Pool.Event;
            }

            GUI.contentColor = windowOption == Pool.Pool ? Color.white : Color.gray;
            if (GUILayout.Button("Pool", GUILayout.Height(30)))
            {
                windowOption = Pool.Pool;
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            switch (windowOption)
            {
                case Pool.Event:
                    Draw(Service.Pool.Reference(), "事件池", "触发数\t事件数\t添加次数\t移除次数");
                    break;
                case Pool.Heap:
                    Draw(Service.Event.Reference(), "引用池", "未使用\t使用中\t使用次数\t释放次数");
                    break;
                case Pool.Pool:
                    Draw(PoolManager.Reference(), "对象池", "未激活\t激活中\t出队次数\t入队次数");
                    break;
            }
        }

        private void Draw(Reference[] references, string message, string module)
        {
            poolData.Clear();
            foreach (var reference in references)
            {
                var assemblyName = Service.Text.Format("{0} - {1}", reference.assetType.Assembly.GetName().Name, message);
                if (!poolData.TryGetValue(assemblyName, out var results))
                {
                    results = new List<Reference>();
                    poolData.Add(assemblyName, results);
                }

                results.Add(reference);
            }

            screenView = GUILayout.BeginScrollView(screenView, "Box");
            foreach (var poolPair in poolData)
            {
                poolPair.Value.Sort(Comparison);
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical("Box", GUILayout.Width((screenWidth - 50) / 2));
                GUILayout.Label(poolPair.Key, GUILayout.Height(20));
                foreach (var data in poolPair.Value)
                {
                    var assetName = data.assetType.Name;
                    if (!string.IsNullOrEmpty(data.assetPath))
                    {
                        assetName = Service.Text.Format("{0} - {1}", data.assetType.Name, data.assetPath);
                    }

                    GUILayout.Label(assetName, GUILayout.Height(20));
                }

                GUILayout.EndVertical();

                GUILayout.BeginVertical("Box");
                GUILayout.Label(module, GUILayout.Height(20));
                foreach (var data in poolPair.Value)
                {
                    GUILayout.Label(data.ToString(), GUILayout.Height(20));
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

        private enum Pool
        {
            Heap,
            Event,
            Pool,
        }
    }
}