using System;
using UnityEditor;

namespace JFramework
{
    [InitializeOnLoad]
    internal static class InspectorView
    {
        public static Action<EditorWindow> OnMaximizedChanged;
        private static EditorWindow focusedWindow;
        private static bool isMaximized;

        static InspectorView()
        {
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

            bool maximized = focusedWindow != null && focusedWindow.maximized;
            if (maximized != isMaximized)
            {
                isMaximized = maximized;
                OnMaximizedChanged?.Invoke(focusedWindow);
            }
        }
    }
}