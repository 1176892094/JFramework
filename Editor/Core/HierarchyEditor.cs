using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using Object = UnityEngine.Object;

namespace JFramework.Editor
{
    [InitializeOnLoad]
    internal class HierarchyEditor
    {
        private static HierarchyEditor instance;
        private float afterName;
        private Event currentEvent = Event.current;
        private readonly HierarchyObject icon = new HierarchyObject();
        private readonly GUIContent tooltipContent = new GUIContent();
        private readonly Dictionary<int, Object> selectedComponents = new Dictionary<int, Object>();
        private Action<Rect, Object, int> DisplayObjectContextMenuDelegate;
        private Func<GameObject, Rect, bool, bool> IconSelectorShowAtPositionDelegate;

        static HierarchyEditor()
        {
            instance = new HierarchyEditor();
        }

        public HierarchyEditor()
        {
            InternalReflection();
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyOnGUI;
        }

        private void InternalReflection()
        {
            MethodInfo DisplayObjectContextMenu = typeof(EditorUtility).GetMethods(BindingFlags.Static | BindingFlags.NonPublic).Single(method => method.Name == nameof(DisplayObjectContextMenu) && method.GetParameters()[1].ParameterType == typeof(Object));
            DisplayObjectContextMenuDelegate = Delegate.CreateDelegate(typeof(Action<Rect, Object, int>), DisplayObjectContextMenu) as Action<Rect, Object, int>;
        }

        private void HierarchyOnGUI(int selectionID, Rect selectionRect)
        {
            currentEvent = Event.current;
            icon.Dispose();
            icon.ID = selectionID;
            icon.rect = selectionRect;
            icon.gameObject = (GameObject)EditorUtility.InstanceIDToObject(icon.ID);
            if (icon.gameObject == null) return;
            icon.Enable();
            icon.nameRect = icon.rect;
            GUIStyle nameStyle = new GUIStyle(TreeView.DefaultStyles.label);
            icon.nameRect.width = nameStyle.CalcSize(new GUIContent(icon.gameObject.name)).x;
            icon.nameRect.x += 8;
            afterName = 0;
            afterName = icon.nameRect.x + icon.nameRect.width;
            afterName += 8;
            ShowGrid();
            ShowComponents();
        }

        private void ShowGrid()
        {
            if (currentEvent.type != EventType.Repaint) return;
            var rect = icon.rect;
            rect.xMin = 32;
            rect.y += 15;
            rect.width += 16;
            rect.height = 1;
            Color guiColor = GUI.color;
            GUI.color = new Color(0,0,0,0.2f);
            GUI.DrawTexture(rect, GetTexture(), ScaleMode.StretchToFill);
            GUI.color = guiColor;
        }

        private void ShowComponents()
        {
            List<Object> components = icon.gameObject.GetComponents(typeof(Component)).ToList<Object>();
            Renderer rendererComponent = icon.gameObject.GetComponent<Renderer>();
            bool hasMaterial = rendererComponent != null && rendererComponent.sharedMaterial != null;
            if (hasMaterial)
            {
                foreach (var sharedMat in rendererComponent.sharedMaterials)
                {
                    components.Add(sharedMat);
                }
            }

            int length = components.Count;
            afterName += 4;

            for (int i = 0; i < length; ++i)
            {
                Object component = components[i];
                if (component == null) continue;
                Type comType = component.GetType();
                Rect rect = RectFromLeft(icon.nameRect, 12, ref afterName);
                if (hasMaterial && i == length - rendererComponent.sharedMaterials.Length)
                {
                    foreach (var sharedMaterial in rendererComponent.sharedMaterials)
                    {
                        if (sharedMaterial == null) continue;
                        ComponentIcon(sharedMaterial, comType, rect, true);
                        rect = RectFromLeft(icon.nameRect, 12, ref afterName);
                    }

                    break;
                }

                ComponentIcon(component, comType, rect, false);
                afterName += 2;
            }
        }

        private void ComponentIcon(Object component, Type componentType, Rect rect, bool isMaterial)
        {
            int comHash = component.GetHashCode();
            if (currentEvent.type == EventType.Repaint)
            {
                Texture image = EditorGUIUtility.ObjectContent(component, componentType).image;
                string tooltip = isMaterial ? component.name : componentType.Name;
                tooltipContent.tooltip = tooltip;
                GUI.Box(rect, tooltipContent, GUIStyle.none);
                GUI.DrawTexture(rect, image, ScaleMode.ScaleToFit);
            }

            if (rect.Contains(currentEvent.mousePosition))
            {
                if (currentEvent.type == EventType.MouseDown)
                {
                    if (currentEvent.button == 0)
                    {
                        selectedComponents.Clear();
                        selectedComponents.Add(comHash, component);
                        DisplayObjectContextMenuDelegate(rect, component, 0);
                        currentEvent.Use();
                        return;
                    }
                }
            }

            if (selectedComponents.Count > 0 && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && !rect.Contains(currentEvent.mousePosition))
            {
                selectedComponents.Clear();
            }
        }

        private Rect RectFromLeft(Rect rect, float width, ref float usedWidth)
        {
            rect.xMin = 0;
            rect.x += usedWidth;
            rect.width = width;
            usedWidth += width;
            return rect;
        }

        private Texture2D GetTexture()
        {
            Texture2D pixelWhite = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            pixelWhite.SetPixel(0, 0, Color.white);
            pixelWhite.Apply();
            return pixelWhite;
        }
    }
}