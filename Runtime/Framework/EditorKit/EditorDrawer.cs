// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  15:18
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    [CustomPropertyDrawer(typeof(FolderAttribute))]
    internal class FolderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.isArray && property.arrayElementType == "string")
            {
                DrawArray(position, property, label);
            }
            else if (property.propertyType == SerializedPropertyType.String)
            {
                DrawField(position, property, label);
            }

            EditorGUI.EndProperty();
        }

        private static void DrawArray(Rect position, SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight;
            var count = property.FindPropertyRelative("Array.size");
            var rect = new Rect(position.x, position.y, position.width, height);
            EditorGUI.PropertyField(rect, count, new GUIContent(label.text + " Size"));
            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel += 1;

            for (int i = 0; i < count.intValue; i++)
            {
                var element = property.GetArrayElementAtIndex(i);
                if (element.propertyType != SerializedPropertyType.String) continue;
                rect = new Rect(position.x, position.y + height * (i + 1), position.width, height);
                DrawField(rect, element, new GUIContent($"Element {i}"));
            }

            EditorGUI.indentLevel = indentLevel;
        }

        private static void DrawField(Rect position, SerializedProperty property, GUIContent label)
        {
            Object asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

            if (asset != null)
            {
                asset = (SceneAsset)EditorGUI.ObjectField(position, label, asset, typeof(SceneAsset), true);
            }
            else
            {
                asset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(property.stringValue);
                asset = (DefaultAsset)EditorGUI.ObjectField(position, label, asset, typeof(DefaultAsset), true);
            }

            if (asset != null)
            {
                property.stringValue = AssetDatabase.GetAssetPath(asset);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isArray && property.arrayElementType == "string")
            {
                var count = property.FindPropertyRelative("Array.size");
                return EditorGUIUtility.singleLineHeight * (count.intValue + 1);
            }

            return base.GetPropertyHeight(property, label);
        }
    }

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