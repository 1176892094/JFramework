using UnityEngine;

namespace JFramework
{
    [Debug(typeof(RectTransform))]
    internal class DebugComponentRectTransform : DebugComponent<RectTransform>
    {
        protected override void OnDebugScene()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Position:", DebugData.Label, DebugData.Component);
            target.anchoredPosition = DebugData.Vector2Field(target.anchoredPosition);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation:", DebugData.Label, DebugData.Component);
            target.localRotation = Quaternion.Euler(DebugData.Vector3Field(target.localRotation.eulerAngles));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Scale:", DebugData.Label, DebugData.Component);
            target.localScale = DebugData.Vector3Field(target.localScale);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Size:", DebugData.Label, DebugData.Component);
            target.sizeDelta = DebugData.Vector2Field(target.sizeDelta);
            GUILayout.EndHorizontal();
        }
    }
}