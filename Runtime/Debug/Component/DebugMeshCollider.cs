using UnityEngine;

namespace JYJFramework.Logger
{
    [Debug(typeof(MeshCollider))]
    public class DebugMeshCollider : Debugger
    {
        private MeshCollider target;

        public override void OnInit()
        {
            target = Target as MeshCollider;
        }

        public override void OnDebuggerGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled","Button");
            target.convex = GUILayout.Toggle(target.convex, "Convex","Button");
            GUILayout.EndHorizontal();
        }
    }
}
