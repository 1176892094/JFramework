using UnityEngine;

namespace JYJFramework.Logger
{
    [Debug(typeof(RectTransform))]
    public class DebugRectTransform : Debugger
    {
        private RectTransform target;

        public override void OnInit()
        {
            target = Target as RectTransform;
        }

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Position:", GUILayout.Width(130));
            target.anchoredPosition = GUIExtensions.Vector2Field(target.anchoredPosition);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation:", GUILayout.Width(130));
            target.localRotation = Quaternion.Euler(GUIExtensions.Vector3Field(target.localRotation.eulerAngles));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale:", GUILayout.Width(130));
            target.localScale = GUIExtensions.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Size:", GUILayout.Width(130));
            target.sizeDelta = GUIExtensions.Vector2Field(target.sizeDelta);
            GUILayout.EndHorizontal();
        }
    }
}
