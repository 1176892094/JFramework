using UnityEngine;

namespace JYJFramework.Logger
{
    [Debug(typeof(CapsuleCollider))]
    public class DebugCapsuleCollider : Debugger
    {
        private CapsuleCollider target;

        public override void OnInit()
        {
            target = Target as CapsuleCollider;
        }

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = target.enabled ? Color.white : Color.gray;
            target.enabled = GUILayout.Toggle(target.enabled, "IsEnabled","Button");
            target.isTrigger = GUILayout.Toggle(target.isTrigger, "IsTrigger","Button");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Center: ", GUILayout.Width(130));
            target.center = GUIExtensions.Vector3Field(target.center);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Direction: ", GUILayout.Width(130));
            target.direction = GUIExtensions.IntField(target.direction);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Height: ", GUILayout.Width(130));
            target.height = GUIExtensions.FloatField(target.height);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius: ", GUILayout.Width(130));
            target.radius = GUIExtensions.FloatField(target.radius);
            GUILayout.EndHorizontal();
        }
    }
}
