using System;
using UnityEditor;
using UnityEngine;

namespace JFramework.Excel
{
    internal static class ExcelStyle
    {
        public static GUIStyle Label;
        public static GUIStyle Button;
        public static GUIStyle TextField;
        public static GUILayoutOption[] nameOptions;
        public static GUILayoutOption[] valueOptions;
        private static GUISkin Skin;

        public static void Enable()
        {
            if (Skin != null) return;
            Skin = Resources.Load<GUISkin>("JFramworkGUI");
            Label = new GUIStyle(EditorStyles.label);
            Button = new GUIStyle(EditorStyles.miniButton);
            TextField = new GUIStyle(EditorStyles.textField);
            nameOptions = new[] { GUILayout.Width(120), GUILayout.Height(20) };
            valueOptions = new[] { GUILayout.Width(300), GUILayout.Height(20) };
        }

        public static object EnumPopup(Enum selected, params GUILayoutOption[] options)
        {
            var array = Enum.GetValues(selected.GetType());
            var length = array.Length;

            var enumString = new string[length];
            for (var i = 0; i < length; i++)
            {
                var fields = selected.GetType().GetFields();
                foreach (var field in fields)
                {
                    if (field.Name.Equals(array.GetValue(i).ToString()))
                    {
                        enumString[i] = field.Name;
                    }
                }
            }

            var index = EditorGUILayout.Popup(selected.GetHashCode(), enumString, options);

            return Enum.ToObject(selected.GetType(), index);
        }
    }
}