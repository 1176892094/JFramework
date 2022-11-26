#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JFramework.Logger
{
    internal class DebugDrawCall
    {
        private readonly DebugData debugData;
        private Vector2 scrollDrawCallView = Vector2.zero;
        public DebugDrawCall(DebugData debugData) => this.debugData = debugData;

        public void ExtendDrawCallGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("DrawCall Information"), GUIStyles.Label, GUIStyles.MinHeight);
            GUILayout.EndHorizontal();

            scrollDrawCallView = GUILayout.BeginScrollView(scrollDrawCallView, GUIStyles.Box);
#if UNITY_EDITOR
            GUILayout.Label(debugData.GetData("DrawCalls") + ": " + UnityStats.drawCalls, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Batches") + ": " + UnityStats.batches, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Static Batched DrawCalls") + ": " + UnityStats.staticBatchedDrawCalls, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Static Batches") + ": " + UnityStats.staticBatches, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Dynamic Batched DrawCalls") + ": " + UnityStats.dynamicBatchedDrawCalls, GUIStyles.Label);
            GUILayout.Label(debugData.GetData("Dynamic Batches") + ": " + UnityStats.dynamicBatches, GUIStyles.Label);
            if (UnityStats.triangles > 10000)
            {
                GUILayout.Label(debugData.GetData("Triangles") + ": " + UnityStats.triangles / 10000 + "W", GUIStyles.Label);
            }
            else
            {
                GUILayout.Label(debugData.GetData("Triangles") + ": " + UnityStats.triangles, GUIStyles.Label);
            }

            if (UnityStats.vertices > 10000)
            {
                GUILayout.Label(debugData.GetData("Vertices") + ": " + UnityStats.vertices / 10000 + "W", GUIStyles.Label);
            }
            else
            {
                GUILayout.Label(debugData.GetData("Vertices") + ": " + UnityStats.vertices, GUIStyles.Label);
            }
#else
            GUILayout.Label("只有在编辑器模式下才能显示DrawCall！",GUIStyles.Label);
#endif
            GUILayout.EndScrollView();
        }
    }
}