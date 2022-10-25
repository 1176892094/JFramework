using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using Object = UnityEngine.Object;

namespace JYJFramework.Editor
{
    [InitializeOnLoad]
    public class HierarchyEditor
    {
        private static HierarchyEditor instance;
        private float afterName;
        private Event currentEvent = Event.current;
        private readonly RowItem rowItem = new RowItem();
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
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyOnGUI;
        }

        private void InternalReflection()
        {
            MethodInfo DisplayObjectContextMenu = typeof(EditorUtility)
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic).Single(
                    method => method.Name == nameof(DisplayObjectContextMenu) &&
                              method.GetParameters()[1].ParameterType == typeof(Object));
            DisplayObjectContextMenuDelegate =
                Delegate.CreateDelegate(typeof(Action<Rect, Object, int>), DisplayObjectContextMenu) as
                    Action<Rect, Object, int>;
        }

        private void HierarchyOnGUI(int selectionID, Rect selectionRect)
        {
            currentEvent = Event.current;
            rowItem.Dispose();
            rowItem.ID = selectionID;
            rowItem.rect = selectionRect;
            rowItem.rowIndex = (int) (selectionRect.y / selectionRect.height);
            rowItem.gameObject = (GameObject)EditorUtility.InstanceIDToObject(rowItem.ID);
            if (rowItem.gameObject == null) return;
            Rect rect = new Rect(selectionRect) { x = 32, width = 16 };
            rowItem.gameObject.SetActive(GUI.Toggle(rect, rowItem.gameObject.activeSelf, string.Empty));
            rowItem.nameRect = rowItem.rect;
            GUIStyle nameStyle = new GUIStyle(TreeView.DefaultStyles.label);
            rowItem.nameRect.width = nameStyle.CalcSize(new GUIContent(rowItem.gameObject.name)).x;
            rowItem.nameRect.x += 8;
            afterName = 0;
            afterName = rowItem.nameRect.x + rowItem.nameRect.width;
            afterName += 8;
            ShowGrid();
            ShowComponents();
        }

        private void ShowGrid()
        {
            if (currentEvent.type != EventType.Repaint) return;
            var rect = rowItem.rect;
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
            List<Object> components = rowItem.gameObject.GetComponents(typeof(Component)).ToList<Object>();
            Renderer rendererComponent = rowItem.gameObject.GetComponent<Renderer>();
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
                Type comType = component.GetType();
                Rect rect = RectFromLeft(rowItem.nameRect, 12, ref afterName);
                if (hasMaterial && i == length - rendererComponent.sharedMaterials.Length)
                {
                    foreach (var sharedMaterial in rendererComponent.sharedMaterials)
                    {
                        if (sharedMaterial == null) continue;
                        ComponentIcon(sharedMaterial, comType, rect, true);
                        rect = RectFromLeft(rowItem.nameRect, 12, ref afterName);
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
                if (selectedComponents.ContainsKey(comHash))
                {
                    Color guiColor = GUI.color;
                    GUI.color = new Color(0, 1, 0, 0.2f);
                    GUI.DrawTexture(rect, GetTexture(), ScaleMode.StretchToFill);
                    GUI.color = guiColor;
                }

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
                        if (currentEvent.control)
                        {
                            if (!selectedComponents.ContainsKey(comHash))
                            {
                                selectedComponents.Add(comHash, component);
                            }
                            else
                            {
                                selectedComponents.Remove(comHash);
                            }

                            currentEvent.Use();
                            return;
                        }

                        selectedComponents.Clear();
                        selectedComponents.Add(comHash, component);
                        currentEvent.Use();
                        return;
                    }

                    if (currentEvent.button == 1)
                    {
                        if (currentEvent.control)
                        {
                            GenericMenu componentGenericMenu = new GenericMenu();
                            componentGenericMenu.AddItem(new GUIContent("Remove All Component"), false, () =>
                            {
                                if (!selectedComponents.ContainsKey(comHash))
                                    selectedComponents.Add(comHash, component);
                                foreach (var selectedComponent in selectedComponents.ToList())
                                {
                                    if (selectedComponent.Value is Material) continue;
                                    selectedComponents.Remove(selectedComponent.Key);
                                    Undo.DestroyObjectImmediate(selectedComponent.Value);
                                }

                                selectedComponents.Clear();
                            });
                            componentGenericMenu.ShowAsContext();
                        }
                        else
                        {
                            DisplayObjectContextMenuDelegate(rect, component, 0);
                        }

                        currentEvent.Use();
                        return;
                    }
                }
            }

            if (selectedComponents.Count > 0 &&
                currentEvent.type == EventType.MouseDown &&
                currentEvent.button == 0 &&
                !currentEvent.control &&
                !rect.Contains(currentEvent.mousePosition))
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

        private Rect RectFromRight(Rect rect, float width, float usedWidth)
        {
            usedWidth += width;
            rect.x = rect.x + rect.width - usedWidth;
            rect.width = width;
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

    public class RowItem
    {
        public int ID = int.MinValue;
        public Rect rect = Rect.zero;
        public Rect nameRect = Rect.zero;
        public GameObject gameObject;
        public int rowIndex;

        public void Dispose()
        {
            ID = int.MinValue;
            gameObject = null;
            rect = Rect.zero;
            nameRect = Rect.zero;
            rowIndex = 0;
        }
    }
}