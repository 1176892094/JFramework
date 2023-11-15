// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-11-14  19:25
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace JFramework.Editor
{
    /// <summary>
    /// 绘制文件夹
    /// </summary>
    [CustomPropertyDrawer(typeof(AssetPathAttribute))]
    internal class FolderDrawer : PropertyDrawer
    {
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
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

        /// <summary>
        /// 绘制数组
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
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

        /// <summary>
        /// 绘制字段
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        private static void DrawField(Rect position, SerializedProperty property, GUIContent label)
        {
            var folder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(property.stringValue);
            folder = (DefaultAsset)EditorGUI.ObjectField(position, label, folder, typeof(DefaultAsset), true);

            if (folder != null)
            {
                property.stringValue = AssetDatabase.GetAssetPath(folder);
            }
        }

        /// <summary>
        /// 绘制高度
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
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