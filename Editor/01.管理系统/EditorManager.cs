// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-12 17:12:30
// # Recently: 2024-12-22 20:12:24
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEditor;

namespace JFramework
{
    [InitializeOnLoad]
    internal static class EditorManager
    {
        private static bool maximized;
        private static bool initialized;
        private static double timeSinceStartup;
        private static EditorWindow focusedWindow;

        static EditorManager()
        {
            EditorApplication.update -= Update;
            EditorApplication.update += Update;
            focusedWindow = EditorWindow.focusedWindow;
            maximized = focusedWindow != null && focusedWindow.maximized;
        }

        public static event Action OnInitialized;
        public static event Action<EditorWindow> OnWindowFocused;
        public static event Action<EditorWindow> OnWindowMaximized;

        private static void Update()
        {
            if (focusedWindow != EditorWindow.focusedWindow)
            {
                focusedWindow = EditorWindow.focusedWindow;
                OnWindowFocused?.Invoke(focusedWindow);
            }

            if (!initialized)
            {
                if (timeSinceStartup < EditorApplication.timeSinceStartup)
                {
                    initialized = true;
                    timeSinceStartup = EditorApplication.timeSinceStartup + 0.2;
                    OnInitialized?.Invoke();
                }
            }

            if (focusedWindow == null)
            {
                return;
            }

            if (maximized != focusedWindow.maximized)
            {
                maximized = focusedWindow.maximized;
                OnWindowMaximized?.Invoke(focusedWindow);
            }
        }
    }
}