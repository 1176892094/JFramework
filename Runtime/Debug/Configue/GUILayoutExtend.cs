using System;
using UnityEngine;

namespace JYJFramework
{
    public static class GUILayoutExtend
    {
        public static Vector3 Vector3Field(Vector3 value)
        {
            string x = GUILayout.TextField(value.x.ToString("F"));
            string y = GUILayout.TextField(value.y.ToString("F"));
            string z = GUILayout.TextField(value.z.ToString("F"));
            if (x == value.x.ToString("F") && y == value.y.ToString("F") && z == value.z.ToString("F")) return value;
            if (float.TryParse(x, out var x2) && float.TryParse(y, out var y2) && float.TryParse(z, out var z2))
            {
                return new Vector3(x2, y2, z2);
            }

            return value;
        }

        public static Vector2 Vector2Field(Vector2 value)
        {
            string x = GUILayout.TextField(value.x.ToString("F"));
            string y = GUILayout.TextField(value.y.ToString("F"));
            if (x == value.x.ToString("F") && y == value.y.ToString("F")) return value;
            if (float.TryParse(x, out var x2) && float.TryParse(y, out var y2))
            {
                return new Vector3(x2, y2);
            }

            return value;
        }

        public static float FloatField(float value)
        {
            string f = GUILayout.TextField(value.ToString("F"));
            if (f == value.ToString("F")) return value;
            return float.TryParse(f, out var f2) ? f2 : value;
        }

        public static int IntField(int value)
        {
            string f = GUILayout.TextField(value.ToString());
            if (f == value.ToString()) return value;
            return int.TryParse(f, out var f2) ? f2 : value;
        }
    }
}