// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-14 16:12:46
// # Recently: 2024-12-22 20:12:44
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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
            var length = property.FindPropertyRelative("Array.size");
            var itemRect = new Rect(position.x, position.y, position.width, height);
            EditorGUI.PropertyField(itemRect, length, new GUIContent(label.text + " Size"));

            var indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel += 1;

            for (var i = 0; i < length.intValue; i++)
            {
                var element = property.GetArrayElementAtIndex(i);
                if (element.propertyType != SerializedPropertyType.String)
                {
                    continue;
                }

                itemRect = new Rect(position.x, position.y + height * (i + 1), position.width, height);
                DrawField(itemRect, element, new GUIContent(Service.Text.Format("Element {0}", i)));
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

            property.stringValue = asset != null ? AssetDatabase.GetAssetPath(asset) : null;
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
}
#endif