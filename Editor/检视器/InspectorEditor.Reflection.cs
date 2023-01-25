using System;
using System.Reflection;

namespace JFramework
{
    internal static class Reflection
    {
        private static Type type;

        public static Type Type => type ??= GetTypeFromEditor("InspectorWindow");

        private static Assembly editor => Assembly.Load("UnityEditor");

        private static Type GetTypeFromEditor(string name)
        {
            return editor.GetType("UnityEditor." + name);
        }
    }
}