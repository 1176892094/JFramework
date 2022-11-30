using UnityEngine;

namespace JFramework.Debug
{
    [Debug(typeof(RectTransform))]
    internal class DebugRectTransform : DebugComponent
    {
        private RectTransform target;

        public override void OnInit() => target = (RectTransform)Target;

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Position:", DebugStyle.Label, DebugStyle.Component);
            target.anchoredPosition = DebugStyle.Vector2Field(target.anchoredPosition);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation:", DebugStyle.Label, DebugStyle.Component);
            target.localRotation = Quaternion.Euler(DebugStyle.Vector3Field(target.localRotation.eulerAngles));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale:", DebugStyle.Label, DebugStyle.Component);
            target.localScale = DebugStyle.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Size:", DebugStyle.Label, DebugStyle.Component);
            target.sizeDelta = DebugStyle.Vector2Field(target.sizeDelta);
            GUILayout.EndHorizontal();
        }
    }
}