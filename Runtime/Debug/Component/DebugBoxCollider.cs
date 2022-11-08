using UnityEngine;

namespace JYJFramework.Logger
{
    [Debug(typeof(BoxCollider))]
    public class DebugBoxCollider : Debugger
    {
        private BoxCollider target;
        
        public override void OnInit()
        {
            target = Target as BoxCollider;
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
            GUILayout.Label("Size: ", GUILayout.Width(130));
            target.size = GUIExtensions.Vector3Field(target.size);
            GUILayout.EndHorizontal();
        }
    }
}
