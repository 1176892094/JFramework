    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    public abstract class InspectorEditor
    {
        private double initTime;
        private List<EditorWindow> failedWindows;

        private static VisualElement GetMainContainer(EditorWindow w)
        {
            return w != null ? GetVisualElement(w.rootVisualElement, "unity-inspector-main-container") : null;
        }

        private static VisualElement GetVisualElement(VisualElement element, string className)
        {
            for (int i = 0; i < element.childCount; i++)
            {
                VisualElement ve = element[i];
                if (ve.ClassListContains(className)) return ve;
                ve = GetVisualElement(ve, className);
                if (ve != null) return ve;
            }

            return null;
        }

        protected void InitInspector()
        {
            failedWindows = new List<EditorWindow>();

            Object[] windows = Resources.FindObjectsOfTypeAll(Reflection.Type);
            foreach (var obj in windows)
            {
                var window = (EditorWindow)obj;
                if (window == null) continue;
                if (!InjectBar(window))
                {
                    failedWindows.Add(window);
                }
            }

            if (failedWindows.Count > 0)
            {
                initTime = EditorApplication.timeSinceStartup;
                EditorApplication.update += TryReinit;
            }
        }

        private bool InjectBar(EditorWindow w)
        {
            VisualElement mainContainer = GetMainContainer(w);
            if (mainContainer == null) return false;
            if (mainContainer.childCount < 2) return false;
            VisualElement editorsList = GetVisualElement(mainContainer, "unity-inspector-editors-list");
            return OnInject(w, mainContainer, editorsList);
        }

        protected abstract bool OnInject(EditorWindow wnd, VisualElement mainContainer, VisualElement editorsList);

        protected void OnMaximizedChanged(EditorWindow w)
        {
            Object[] windows = Resources.FindObjectsOfTypeAll(Reflection.Type);
            foreach (var obj in windows)
            {
                var window = (EditorWindow)obj;
                if (window != null) InjectBar(window);
            }
        }

        private void TryReinit()
        {
            if (EditorApplication.timeSinceStartup - initTime <= 0.5) return;
            EditorApplication.update -= TryReinit;
            if (failedWindows != null)
            {
                TryReinit(failedWindows);
                failedWindows = null;
            }
        }

        private void TryReinit(List<EditorWindow> windows)
        {
            foreach (EditorWindow window in windows)
            {
                if (window == null) continue;
                InjectBar(window);
            }
        }
    }