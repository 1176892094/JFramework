using UnityEngine;

namespace JFramework
{
    [Debug(typeof(MeshCollider))]
    internal class DebugComponentMeshCollider : DebugComponent<MeshCollider>
    {
        protected override void OnDebugScene()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled", DebugData.Button);
            target.convex = GUILayout.Toggle(target.convex, "Convex", DebugData.Button);
            GUILayout.EndHorizontal();
        }
    }
}