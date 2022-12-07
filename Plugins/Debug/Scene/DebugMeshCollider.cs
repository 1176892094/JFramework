using UnityEngine;

namespace JFramework
{
    [Debug(typeof(MeshCollider))]
    internal class DebugMeshCollider : DebugComponent
    {
        private MeshCollider target;

        public override void OnInit() => target = (MeshCollider)Target;

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled", DebugStyle.Button);
            target.convex = GUILayout.Toggle(target.convex, "Convex", DebugStyle.Button);
            GUILayout.EndHorizontal();
        }
    }
}