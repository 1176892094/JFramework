// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-14 16:12:44
// # Recently: 2024-12-22 20:12:47
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEditor;
using UnityEngine;

namespace JFramework
{
    [CustomPropertyDrawer(typeof(Variable<>))]
    internal class VariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var color = GUI.color;
            GUI.color = Color.green;
            var content = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            GUI.color = color;
            var relative = property.FindPropertyRelative("origin");
            EditorGUI.PropertyField(content, relative, GUIContent.none);
            EditorGUI.EndProperty();
        }
    }
}