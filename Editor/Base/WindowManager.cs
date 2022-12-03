using System;
using System.Reflection;
using UnityEditor;

[InitializeOnLoad]
public static class WindowManager
{
    public static Action<EditorWindow> OnMaximizedChanged;

    private static EditorWindow focusedWindow;
    private static bool isMaximized;

    static WindowManager()
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

public static class Reflection
{
    private static Assembly editor;
    
    private static Type type;

    private static Assembly Editor
    {
        get
        {
            if (editor == null) editor = Assembly.Load("UnityEditor");
            return editor;
        }
    }
    
    public static Type Type => type ??= GetEditorType("InspectorWindow");

    private static Type GetEditorType(string name)
    {
        return Editor.GetType("UnityEditor." + name);
    }
}