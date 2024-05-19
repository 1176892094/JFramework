// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-05-19  19:05
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    [CustomPropertyDrawer(typeof(SecretInt))]
    public class SecretIntDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            contentPosition.width -= 15;
            SerializedProperty myIntProperty = property.FindPropertyRelative("origin");
            EditorGUI.PropertyField(contentPosition, myIntProperty, GUIContent.none);
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(SecretFloat))]
    public class SecretFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            contentPosition.width -= 15;
            SerializedProperty myIntProperty = property.FindPropertyRelative("origin");
            EditorGUI.PropertyField(contentPosition, myIntProperty, GUIContent.none);
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(SecretString))]
    public class SecretStringDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            contentPosition.width -= 15;
            SerializedProperty myIntProperty = property.FindPropertyRelative("origin");
            EditorGUI.PropertyField(contentPosition, myIntProperty, GUIContent.none);
            EditorGUI.EndProperty();
        }
    }
}
#endif