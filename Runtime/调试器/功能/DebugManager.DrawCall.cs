#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JFramework.Core
{
    internal sealed partial class DebugManager
    {
        private Vector2 scrollDrawCallView;

        private void DebugDrawCall()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(DebugConst.DrawCallInformation, DebugData.Label, DebugData.HeightLow);
            GUILayout.EndHorizontal();

            scrollDrawCallView = GUILayout.BeginScrollView(scrollDrawCallView, DebugData.Box);
#if UNITY_EDITOR
            GUILayout.Label(DebugConst.DrawCalls + ": " + UnityStats.drawCalls, DebugData.Label);
            GUILayout.Label(DebugConst.Batches + ": " + UnityStats.batches, DebugData.Label);
            GUILayout.Label(DebugConst.StaticBatchedDrawCalls + ": " + UnityStats.staticBatchedDrawCalls, DebugData.Label);
            GUILayout.Label(DebugConst.StaticBatches + ": " + UnityStats.staticBatches, DebugData.Label);
            GUILayout.Label(DebugConst.DynamicBatchedDrawCalls + ": " + UnityStats.dynamicBatchedDrawCalls, DebugData.Label);
            GUILayout.Label(DebugConst.DynamicBatches + ": " + UnityStats.dynamicBatches, DebugData.Label);

            if (UnityStats.triangles > 10000)
            {
                GUILayout.Label(DebugConst.Triangles + ": " + UnityStats.triangles / 10000 + "W", DebugData.Label);
            }
            else
            {
                GUILayout.Label(DebugConst.Triangles + ": " + UnityStats.triangles, DebugData.Label);
            }

            if (UnityStats.vertices > 10000)
            {
                GUILayout.Label(DebugConst.Vertices + ": " + UnityStats.vertices / 10000 + "W", DebugData.Label);
            }
            else
            {
                GUILayout.Label(DebugConst.Vertices + ": " + UnityStats.vertices, DebugData.Label);
            }
#else
            GUILayout.Label("只有在编辑器模式下才能显示DrawCall！",DebugData.Label);
#endif
            GUILayout.EndScrollView();
        }
    }
}