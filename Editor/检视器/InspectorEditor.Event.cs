using System;
using UnityEditor;

namespace JFramework
{
    [InitializeOnLoad]
    internal static class InspectorEvent
    {
        public static Action<EditorWindow> OnMaximizedChanged;
        private static EditorWindow focusedWindow;
        private static bool isMaximized;

        static InspectorEvent()
        {
            EditorApplication.update -= Update;
            EditorApplication.update += Update;
            focusedWindow = EditorWindow.focusedWindow;
            isMaximized = focusedWindow != null && focusedWindow.maximized;
        }

        private static void Update()
        {
            if (focusedWindow != null && focusedWindow != EditorWindow.focusedWindow)
            {
                focusedWindow = EditorWindow.focusedWindow;
            }

            var maximized = focusedWindow != null && focusedWindow.maximized;
            if (maximized == isMaximized) return;
            OnMaximizedChanged?.Invoke(focusedWindow);
            isMaximized = maximized;
        }
    }
}