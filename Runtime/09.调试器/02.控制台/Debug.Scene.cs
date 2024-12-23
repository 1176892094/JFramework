// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-18 20:12:46
// # Recently: 2024-12-22 20:12:39
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    internal partial class DebugManager
    {
        private readonly List<Type> cachedTypes = new List<Type>();
        private readonly List<Component> components = new List<Component>();
        private readonly List<GameObject> gameObjects = new List<GameObject>();
        private bool cachedComponent;
        private string componentFilter = "";

        private int componentIndex = -1;
        private Vector2 componentView = Vector2.zero;
        private string gameObjectFilter = "";

        private int gameObjectIndex = -1;
        private Vector2 gameObjectView = Vector2.zero;


        private void SceneWindow()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Service.Text.Format("GameObject [{0}]", gameObjects.Count), "Button", BoxWidth, Height30);
            if (GUILayout.Button("Refresh", Height30))
            {
                UpdateGameObject();
                UpdateComponent();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("Box", BoxWidth);
            ShowGameObject();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("Box", BoxWidth);
            ShowComponent();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        private void UpdateGameObject()
        {
            gameObjects.Clear();
            var objects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (var component in objects)
            {
                gameObjects.Add(component);
            }

            gameObjects.Sort(Comparison);
            gameObjectIndex = -1;
        }

        private static int Comparison(GameObject origin, GameObject target)
        {
            return string.Compare(origin.name, target.name, StringComparison.Ordinal);
        }

        private void UpdateComponent()
        {
            components.Clear();
            if (gameObjectIndex != -1 && gameObjectIndex < gameObjects.Count)
            {
                var objects = gameObjects[gameObjectIndex].GetComponents<Component>();
                foreach (var component in objects)
                {
                    components.Add(component);
                }
            }

            componentIndex = -1;
            cachedComponent = false;
        }

        private void ShowGameObject()
        {
            GUILayout.BeginHorizontal();
            gameObjectFilter = GUILayout.TextField(gameObjectFilter, Height25);
            GUILayout.EndHorizontal();

            gameObjectView = GUILayout.BeginScrollView(gameObjectView);
            for (var i = 0; i < gameObjects.Count; i++)
            {
                var target = gameObjects[i];
                if (target && target.name.Contains(gameObjectFilter))
                {
                    GUILayout.BeginHorizontal();
                    GUI.contentColor = target.activeSelf ? Color.white : Color.gray;
                    var selected = gameObjectIndex == i;
                    if (GUILayout.Toggle(selected, " " + target.name) != selected)
                    {
                        gameObjectIndex = gameObjectIndex != i ? i : -1;
                        UpdateComponent();
                    }

                    GUILayout.EndHorizontal();

                    if (gameObjectIndex == i)
                    {
                        GUILayout.BeginVertical("Box");

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Tag: " + target.tag, Width160);
                        GUILayout.Label("Layer: " + LayerMask.LayerToName(target.layer));
                        GUILayout.EndHorizontal();

                        GUILayout.EndVertical();
                    }
                }
            }

            GUILayout.EndScrollView();
        }

        private void ShowComponent()
        {
            if (gameObjectIndex != -1)
            {
                GUILayout.BeginHorizontal();
                if (cachedComponent)
                {
                    componentFilter = GUILayout.TextField(componentFilter, Height25);
                }
                else
                {
                    if (componentIndex != -1 && componentIndex < components.Count && components[componentIndex])
                    {
                        if (GUILayout.Button("Remove Component", Height25))
                        {
                            var component = components[componentIndex];
                            if (component is DebugManager or Transform)
                            {
                                Debug.LogWarning("无法销毁组件: " + component.GetType().Name);
                            }
                            else
                            {
                                Destroy(component);
                                UpdateComponent();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Add Component", Height25))
                        {
                            cachedComponent = !cachedComponent;
                        }
                    }
                }

                GUILayout.EndHorizontal();
            }

            componentView = GUILayout.BeginScrollView(componentView);

            if (gameObjectIndex != -1)
            {
                if (cachedComponent)
                {
                    foreach (var cachedType in cachedTypes)
                    {
                        if (cachedType.FullName == null)
                        {
                            continue;
                        }

                        if (!cachedType.FullName.Contains(componentFilter))
                        {
                            continue;
                        }

                        if (GUILayout.Button(cachedType.FullName, Height25))
                        {
                            gameObjects[gameObjectIndex].gameObject.AddComponent(cachedType);
                            cachedComponent = false;
                            UpdateComponent();
                            break;
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < components.Count; ++i)
                    {
                        var component = components[i];
                        if (component == null)
                        {
                            continue;
                        }

                        GUILayout.BeginHorizontal();
                        var selected = componentIndex == i;
                        if (GUILayout.Toggle(selected, component.GetType().Name) != selected)
                        {
                            componentIndex = componentIndex != i ? i : -1;
                        }

                        GUILayout.EndHorizontal();
                    }
                }
            }

            GUILayout.EndScrollView();
        }
    }
}