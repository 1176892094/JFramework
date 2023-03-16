using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JFramework
{
    internal abstract class InspectorEditor
    {
        private static Type inspector;
        private static Type Inspector => inspector ??= GetTypeByEditor("InspectorWindow");
        private static Type GetTypeByEditor(string name) => assembly.GetType("UnityEditor." + name);
        private static Assembly assembly => Assembly.Load("UnityEditor");
        private List<EditorWindow> failedWindows;
        private double sinceStartTime;

        protected void Awake()
        {
            failedWindows = new List<EditorWindow>();
            var windows = Resources.FindObjectsOfTypeAll(Inspector);
            foreach (var window in windows)
            {
                var editor = (EditorWindow)window;
                if (editor == null) continue;
                if (!TryAddMenuItem(editor))
                {
                    failedWindows.Add(editor);
                }
            }

            if (failedWindows.Count <= 0) return;
            sinceStartTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += Update;
        }

        private void Update()
        {
            if (EditorApplication.timeSinceStartup - sinceStartTime <= 0.5) return;
            EditorApplication.update -= Update;
            if (failedWindows == null) return;
            GetMenuItem(failedWindows);
            failedWindows = null;
        }

        private void GetMenuItem(IEnumerable<EditorWindow> windows)
        {
            foreach (var window in windows.Where(window => window != null))
            {
                TryAddMenuItem(window);
            }
        }

        private bool TryAddMenuItem(EditorWindow window)
        {
            var container = TryGetVisualElement(window);
            if (container == null) return false;
            if (container.childCount < 2) return false;
            var element = GetVisualElement(container, "unity-inspector-editors-list");
            return InitInspector(window, element);
        }

        private static VisualElement TryGetVisualElement(EditorWindow window)
        {
            return window != null ? GetVisualElement(window.rootVisualElement, "unity-inspector-main-container") : null;
        }

        private static VisualElement GetVisualElement(VisualElement element, string className)
        {
            for (var i = 0; i < element.childCount; i++)
            {
                var visualElement = element[i];
                if (visualElement.ClassListContains(className)) return visualElement;
                visualElement = GetVisualElement(visualElement, className);
                if (visualElement != null) return visualElement;
            }

            return null;
        }

        protected abstract bool InitInspector(EditorWindow w, VisualElement element);

        protected void OnMaximizedChanged(EditorWindow w)
        {
            var windows = Resources.FindObjectsOfTypeAll(Inspector);
            foreach (var window in windows)
            {
                var editor = (EditorWindow)window;
                if (editor != null)
                {
                    TryAddMenuItem(editor);
                }
            }
        }
    }
}