using UnityEngine;

namespace JFramework
{
    public static class GUIStyles
    {
        public static GUIStyle Box;
        public static GUIStyle Label;
        public static GUIStyle Button;
        public static GUIStyle Window;
        public static GUIStyle TextField;
        public static GUILayoutOption[] MinGUIWindow;
        public static GUILayoutOption[] MinHeight;
        public static GUILayoutOption[] MinHeightFix;
        public static GUILayoutOption[] Component;
        private static GUISkin Skin;

        public static void Enable()
        {
            if (Skin != null) return;
            Skin = ResourceManager.Load<GUISkin>("DebugController");
            Box = Skin.FindStyle("Box");
            Label = Skin.FindStyle("Label");
            Button = Skin.FindStyle("Button");
            Window = Skin.FindStyle("Window");
            TextField = Skin.FindStyle("TextField");
            MinGUIWindow = new GUILayoutOption[] { GUILayout.Width(180), GUILayout.Height(60) };
            MinHeight = new GUILayoutOption[] { GUILayout.Height(60) };
            Component = new GUILayoutOption[] { GUILayout.Width(130) };
            MinHeightFix = new GUILayoutOption[] { GUILayout.Height(50) };
        }


        public static Vector3 Vector3Field(Vector3 value)
        {
            string x = GUILayout.TextField(value.x.ToString("F"), TextField);
            string y = GUILayout.TextField(value.y.ToString("F"), TextField);
            string z = GUILayout.TextField(value.z.ToString("F"), TextField);
            if (x == value.x.ToString("F") && y == value.y.ToString("F") && z == value.z.ToString("F")) return value;
            if (float.TryParse(x, out var x2) && float.TryParse(y, out var y2) && float.TryParse(z, out var z2))
            {
                return new Vector3(x2, y2, z2);
            }

            return value;
        }

        public static Vector2 Vector2Field(Vector2 value)
        {
            string x = GUILayout.TextField(value.x.ToString("F"), TextField);
            string y = GUILayout.TextField(value.y.ToString("F"), TextField);
            if (x == value.x.ToString("F") && y == value.y.ToString("F")) return value;
            if (float.TryParse(x, out var x2) && float.TryParse(y, out var y2))
            {
                return new Vector3(x2, y2);
            }

            return value;
        }

        public static float FloatField(float value)
        {
            string f = GUILayout.TextField(value.ToString("F"), TextField);
            if (f == value.ToString("F")) return value;
            return float.TryParse(f, out var f2) ? f2 : value;
        }

        public static int IntField(int value)
        {
            string f = GUILayout.TextField(value.ToString(), TextField);
            if (f == value.ToString()) return value;
            return int.TryParse(f, out var f2) ? f2 : value;
        }
    }
}