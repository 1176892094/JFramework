using UnityEngine;

namespace JFramework.Logger
{
    [Debug(typeof(RectTransform))]
    internal class DebugRectTransform : DebugComponent
    {
        private RectTransform target;

        public override void OnInit() => target = (RectTransform)Target;

        public override void OnDebugGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Position:", GUIStyles.Label,GUIStyles.Component);
            target.anchoredPosition = GUIStyles.Vector2Field(target.anchoredPosition);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation:", GUIStyles.Label,GUIStyles.Component);
            target.localRotation = Quaternion.Euler(GUIStyles.Vector3Field(target.localRotation.eulerAngles));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale:", GUIStyles.Label,GUIStyles.Component);
            target.localScale = GUIStyles.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Size:", GUIStyles.Label,GUIStyles.Component);
            target.sizeDelta = GUIStyles.Vector2Field(target.sizeDelta);
            GUILayout.EndHorizontal();
        }
    }
}
