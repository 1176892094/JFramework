using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    [InitializeOnLoad]
    internal class HierarchyEditor
    {
        private static HierarchyEditor instance;
        private float nameDistance;
        private Event currentEvent;
        private Object selectComponent;
        private readonly GUIContent content = new GUIContent();
        private readonly HierarchyIcon icon = new HierarchyIcon();
        private Action<Rect, Object, int> DisplayObjectContextMenuDelegate;
        private Func<GameObject, Rect, bool, bool> IconSelectorShowAtPositionDelegate;

        static HierarchyEditor()
        {
            instance = new HierarchyEditor();
        }

        private HierarchyEditor()
        {
            InternalReflection();
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyOnGUI;
        }


        private void InternalReflection()
        {
            MethodInfo DisplayObjectContextMenu = typeof(EditorUtility)
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                .Single(method => method.Name == nameof(DisplayObjectContextMenu) &&
                                  method.GetParameters()[1].ParameterType == typeof(Object));
            DisplayObjectContextMenuDelegate =
                (Action<Rect, Object, int>)Delegate.CreateDelegate(typeof(Action<Rect, Object, int>),
                    DisplayObjectContextMenu);
        }

        private void HierarchyOnGUI(int instanceId, Rect selectionRect)
        {
            currentEvent = Event.current;
            icon.Dispose();
            icon.ID = instanceId;
            icon.iconRect = selectionRect;
            icon.gameObject = (GameObject)EditorUtility.InstanceIDToObject(icon.ID);
            if (icon.gameObject == null) return;
            icon.Display();
            icon.nameRect = icon.iconRect;
            var nameStyle = new GUIStyle(TreeView.DefaultStyles.label);
            icon.nameRect.width = nameStyle.CalcSize(new GUIContent(icon.gameObject.name)).x;
            icon.nameRect.x += EditorConst.Int8;
            nameDistance = icon.nameRect.x + icon.nameRect.width;
            nameDistance += EditorConst.Int8;
            ShowSplitLine();
            ShowComponent();
        }

        private void ShowSplitLine()
        {
            if (currentEvent.type != EventType.Repaint) return;
            var rect = icon.iconRect;
            rect.xMin = EditorConst.Int32;
            rect.y += EditorConst.Int16 - 1;
            rect.width += EditorConst.Int16;
            rect.height = 1;
            var color = GUI.color;
            GUI.color = new Color(0, 0, 0, 0.2f);
            GUI.DrawTexture(rect, GetTexture(), ScaleMode.StretchToFill);
            GUI.color = color;
        }

        private void ShowComponent()
        {
            var renderer = icon.gameObject.GetComponent<Renderer>();
            var components = icon.gameObject.GetComponents(typeof(Component)).ToList<Object>();

            var hasMaterial = renderer != null && renderer.sharedMaterial != null;
            if (hasMaterial)
            {
                foreach (var material in renderer.sharedMaterials)
                {
                    components.Add(material);
                }
            }

            var count = components.Count;
            nameDistance += EditorConst.Int4;

            for (int i = 0; i < count; ++i)
            {
                var component = components[i];
                if (component == null) continue;
                var type = component.GetType();
                var rect = ComponentPosition(icon.nameRect, EditorConst.Int12, ref nameDistance);
                if (hasMaterial && i == count - renderer.sharedMaterials.Length)
                {
                    foreach (var material in renderer.sharedMaterials)
                    {
                        if (material == null) continue;
                        ComponentIcon(material, type, rect, true);
                        rect = ComponentPosition(icon.nameRect, EditorConst.Int12, ref nameDistance);
                    }

                    break;
                }

                ComponentIcon(component, type, rect, false);
                nameDistance += EditorConst.Int2;
            }
        }

        private void ComponentIcon(Object component, Type componentType, Rect rect, bool isMaterial)
        {
            if (currentEvent.type == EventType.Repaint)
            {
                var image = EditorGUIUtility.ObjectContent(component, componentType).image;
                var tooltip = isMaterial ? component.name : componentType.Name;
                content.tooltip = tooltip;
                GUI.Box(rect, content, GUIStyle.none);
                GUI.DrawTexture(rect, image, ScaleMode.ScaleToFit);
            }

            if (rect.Contains(currentEvent.mousePosition))
            {
                if (currentEvent.type == EventType.MouseDown)
                {
                    if (currentEvent.button == 0)
                    {
                        selectComponent = component;
                        DisplayObjectContextMenuDelegate(rect, component, 0);
                        currentEvent.Use();
                        return;
                    }
                }
            }

            if (selectComponent != null && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 &&
                !rect.Contains(currentEvent.mousePosition))
            {
                selectComponent = null;
            }
        }

        private Rect ComponentPosition(Rect rect, float width, ref float result)
        {
            rect.xMin = 0;
            rect.x += result;
            rect.width = width;
            result += width;
            return rect;
        }

        private Texture2D GetTexture()
        {
            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            return texture;
        }

        private class HierarchyIcon
        {
            public int ID;
            public Rect iconRect;
            public Rect nameRect;
            public GameObject gameObject;

            public void Display()
            {
                var pos = Event.current.mousePosition;
                var isHover = pos.x >= 0 && pos.x <= iconRect.xMax + EditorConst.Int16 && pos.y >= iconRect.y && pos.y < iconRect.yMax;
                if (!isHover) return;
                var rect = new Rect(EditorConst.Int32 + 0.5f, iconRect.y, EditorConst.Int16, iconRect.height);
                var isShow = EditorGUI.Toggle(rect, GUIContent.none, gameObject.activeSelf);
                gameObject.SetActive(isShow);
            }

            public void Dispose()
            {
                ID = int.MinValue;
                iconRect = Rect.zero;
                nameRect = Rect.zero;
                gameObject = null;
            }
        }
    }
}