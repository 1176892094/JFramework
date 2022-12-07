#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JFramework
{
    internal class DebugDrawCall
    {
        private readonly DebugSetting setting;
        private Vector2 scrollDrawCallView;
        public DebugDrawCall(DebugSetting setting) => this.setting = setting;

        public void ExtendDrawCallGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(setting.GetData("DrawCall Information"), DebugStyle.Label, DebugStyle.MinHeight);
            GUILayout.EndHorizontal();

            scrollDrawCallView = GUILayout.BeginScrollView(scrollDrawCallView, DebugStyle.Box);
#if UNITY_EDITOR
            GUILayout.Label(setting.GetData("DrawCalls") + ": " + UnityStats.drawCalls, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Batches") + ": " + UnityStats.batches, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Static Batched DrawCalls") + ": " + UnityStats.staticBatchedDrawCalls, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Static Batches") + ": " + UnityStats.staticBatches, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Dynamic Batched DrawCalls") + ": " + UnityStats.dynamicBatchedDrawCalls, DebugStyle.Label);
            GUILayout.Label(setting.GetData("Dynamic Batches") + ": " + UnityStats.dynamicBatches, DebugStyle.Label);
            if (UnityStats.triangles > 10000)
            {
                GUILayout.Label(setting.GetData("Triangles") + ": " + UnityStats.triangles / 10000 + "W", DebugStyle.Label);
            }
            else
            {
                GUILayout.Label(setting.GetData("Triangles") + ": " + UnityStats.triangles, DebugStyle.Label);
            }

            if (UnityStats.vertices > 10000)
            {
                GUILayout.Label(setting.GetData("Vertices") + ": " + UnityStats.vertices / 10000 + "W", DebugStyle.Label);
            }
            else
            {
                GUILayout.Label(setting.GetData("Vertices") + ": " + UnityStats.vertices, DebugStyle.Label);
            }
#else
            GUILayout.Label("只有在编辑器模式下才能显示DrawCall！",DebugStyle.Label);
#endif
            GUILayout.EndScrollView();
        }
    }
}