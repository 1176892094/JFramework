using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal abstract class InspectorEditor
    {
        private List<EditorWindow> failedWindows;
        private double startTime;
        
        protected void Init()
        {
            failedWindows = new List<EditorWindow>();
            var windows = Resources.FindObjectsOfTypeAll(Reflection.Type);
            foreach (var w in windows)
            {
                var window = (EditorWindow)w;
                if (window == null) continue;
                if (!TryAddMenuItem(window))
                {
                    failedWindows.Add(window);
                }
            }

            if (failedWindows.Count > 0)
            {
                startTime = EditorApplication.timeSinceStartup;
                EditorApplication.update += Update;
            }
        }

        private void Update()
        {
            if (EditorApplication.timeSinceStartup - startTime <= 0.5) return;
            EditorApplication.update -= Update;
            if (failedWindows == null) return;
            GetMenuItem(failedWindows);
            failedWindows = null;
        }

        private void GetMenuItem(List<EditorWindow> windows)
        {
            foreach (EditorWindow window in windows)
            {
                if (window == null) continue;
                TryAddMenuItem(window);
            }
        }

        private bool TryAddMenuItem(EditorWindow w)
        {
            var container = GetContainer(w);
            if (container == null) return false;
            if (container.childCount < 2) return false;
            var element = GetVisualElement(container, "unity-inspector-editors-list");
            return Show(w, element);
        }

        private static VisualElement GetContainer(EditorWindow w)
        {
            return w != null ? GetVisualElement(w.rootVisualElement, "unity-inspector-main-container") : null;
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

        protected abstract bool Show(EditorWindow w, VisualElement element);

        protected void OnMaximizedChanged(EditorWindow w)
        {
            Object[] windows = Resources.FindObjectsOfTypeAll(Reflection.Type);
            foreach (var obj in windows)
            {
                var window = (EditorWindow)obj;
                if (window != null) TryAddMenuItem(window);
            }
        }
    }
}