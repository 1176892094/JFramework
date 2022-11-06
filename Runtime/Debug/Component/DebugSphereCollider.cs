using UnityEngine;

namespace JYJFramework
{
    [Debug(typeof(SphereCollider))]
    public class DebugSphereCollider : Debugger
    {
        private SphereCollider target;

        public override void OnInit()
        {
            target = Target as SphereCollider;
        }

        public override void OnDebuggerGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled","Button");
            target.isTrigger = GUILayout.Toggle(target.isTrigger, "IsTrigger","Button");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Center: ", GUILayout.Width(130));
            target.center = GUILayoutExtend.Vector3Field(target.center);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius: ", GUILayout.Width(130));
            target.radius = GUILayoutExtend.FloatField(target.radius);
            GUILayout.EndHorizontal();
        }
    }
}
