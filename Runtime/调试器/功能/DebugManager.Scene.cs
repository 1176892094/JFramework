using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JFramework.Interface;
using UnityEngine;
using UnityEngine.Profiling;


namespace JFramework.Core
{
    internal partial class DebugManager
    { 
        private readonly Dictionary<Type, Type> typeDict = new Dictionary<Type, Type>();
        private readonly List<Type> typeList = new List<Type>();
        private List<Component> componentList = new List<Component>();
        private List<Transform> objectList = new List<Transform>();
        private int componentIndex = -1;
        private int objectIndex = -1;
        private bool isAddComponent;
        private float objectMemory;
        private string objectFilter = "";
        private string componentFilter = "";
        private Vector2 sceneView = Vector2.zero;
        private Vector2 inspectorView = Vector2.zero;
        private IDebugComponent debugComponent;

        private void InitComponent()
        {
            var baseType = typeof(IDebugComponent);
            var assembly = Assembly.GetAssembly(baseType);
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (!typeof(IDebugComponent).IsAssignableFrom(type.BaseType)) continue;
                var attributes = type.GetCustomAttributes(typeof(DebugAttribute), true);
                foreach (var attribute in attributes)
                {
                    if (attribute is DebugAttribute debugger)
                    {
                        typeDict.Add(debugger.type, type);
                    }
                }
            }
        }

        private void RefreshGameObject()
        {
            objectList.Clear();
            objectList = FindObjectsOfType<Transform>().ToList();
            objectIndex = -1;
        }

        private void RefreshComponent()
        {
            componentList.Clear();
            if (objectIndex != -1 && objectIndex < objectList.Count)
            {
                componentList = objectList[objectIndex].GetComponents<Component>().ToList();
            }

            componentIndex = -1;
            isAddComponent = false;
            debugComponent = null;
        }

        private void ShowGameObject()
        {
            GUI.contentColor = Color.white;
            GUILayout.BeginVertical(DebugData.Box, DebugData.SceneBox);

            GUILayout.BeginHorizontal();
            objectFilter = GUILayout.TextField(objectFilter, DebugData.TextField, DebugData.HeightLow);
            GUILayout.EndHorizontal();

            SceneObject();

            GUILayout.EndVertical();
        }

        private void SceneObject()
        {
            sceneView = GUILayout.BeginScrollView(sceneView);
            for (int i = 0; i < objectList.Count; i++)
            {
                if (!objectList[i] || !objectList[i].name.Contains(objectFilter)) continue;
                if (objectList[i].gameObject.hideFlags == HideFlags.HideInHierarchy) continue;

                GUILayout.BeginHorizontal();
                GUI.contentColor = objectList[i].gameObject.activeSelf ? Color.cyan : Color.gray;
                bool value = objectIndex == i;
                if (GUILayout.Toggle(value, objectList[i].name) != value)
                {
                    if (objectIndex != i)
                    {
                        objectIndex = i;
#if ENABLE_PROFILER
                        objectMemory = Profiler.GetRuntimeMemorySizeLong(objectList[i].gameObject) / 1000f;
#endif
                        RefreshComponent();
                    }
                    else
                    {
                        objectIndex = -1;
                        RefreshComponent();
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                SelectObject(i);
            }

            GUILayout.EndScrollView();
        }

        private void SelectObject(int index)
        {
            //当前物体被选中
            if (objectIndex == index)
            {
                GUI.contentColor = objectList[index].gameObject.activeSelf ? Color.white : Color.gray;
                GUILayout.BeginVertical(DebugData.Box);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Tag: " + objectList[index].tag, DebugData.Label);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Layer: " + LayerMask.LayerToName(objectList[index].gameObject.layer), DebugData.Label);
                GUILayout.EndHorizontal();
#if ENABLE_PROFILER
                GUILayout.BeginHorizontal();
                GUILayout.Label("Memory" + ": " + objectMemory + "KB", DebugData.Label);
                GUILayout.EndHorizontal();
#endif
                GUILayout.EndVertical();
            }
        }

        private void ShowComponent()
        {
            GUI.contentColor = Color.white;
            GUILayout.BeginVertical(DebugData.Box, DebugData.SceneBox);
            //添加、删除组件
            if (objectIndex != -1)
            {
                GUILayout.BeginHorizontal();
                isAddComponent = GUILayout.Toggle(isAddComponent, DebugConst.AddComponent, DebugData.Button, DebugData.HeightLow);
                if (componentIndex != -1 && componentIndex < componentList.Count)
                {
                    if (componentList[componentIndex])
                    {
                        if (GUILayout.Button(DebugConst.RemoveComponent, DebugData.Button, DebugData.HeightLow))
                        {
                            if (componentList[componentIndex] is DebugManager)
                            {
                                Debug.LogWarning($"不能销毁组件{componentList[componentIndex].GetType().Name}!");
                            }
                            else
                            {
                                Destroy(componentList[componentIndex]);
                                RefreshComponent();
                                return;
                            }
                        }
                    }
                }

                GUILayout.EndHorizontal();
            }

            //被选中物体的组件列表
            inspectorView = GUILayout.BeginScrollView(inspectorView);
            if (objectIndex != -1)
            {
                //添加组件状态
                if (isAddComponent)
                {
                    GUILayout.BeginHorizontal();
                    componentFilter = GUILayout.TextField(componentFilter, DebugData.TextField, DebugData.HeightLow);
                    if (GUILayout.Button(DebugConst.Search, DebugData.Button, DebugData.HeightLow))
                    {
                        typeList.Clear();
                        Type baseType = typeof(Component);
                        Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
                        foreach (Assembly assembly in assemblyArray)
                        {
                            Type[] types = assembly.GetTypes();
                            foreach (Type type in types)
                            {
                                if (type.IsSubclassOf(baseType) && type.Name.Contains(componentFilter))
                                {
                                    string[] strArray = type.ToString().Split('.');
                                    if (strArray[0] == "UnityEngine")
                                    {
                                        typeList.Add(type);
                                    }
                                }
                            }
                        }
                    }

                    GUILayout.EndHorizontal();

                    foreach (var addComponent in typeList)
                    {
                        string fullName = addComponent.FullName;
                        if (fullName != null)
                        {
                            string[] strArray = fullName.Split('.');
                            if (GUILayout.Button(strArray[^1], DebugData.Button, DebugData.HeightLow))
                            {
                                objectList[objectIndex].gameObject.AddComponent(addComponent);
                                isAddComponent = false;
                                RefreshComponent();
                                return;
                            }
                        }
                    }
                }
                //预览组件状态
                else
                {
                    for (int i = 0; i < componentList.Count; i++)
                    {
                        if (componentList[i])
                        {
                            GUILayout.BeginHorizontal();
                            GUI.contentColor = Color.cyan;
                            bool value = componentIndex == i;
                            if (GUILayout.Toggle(value, componentList[i].GetType().Name) != value)
                            {
                                if (componentIndex != i)
                                {
                                    componentIndex = i;
                                    debugComponent = null;
                                    Type type = componentList[i].GetType();
                                    if (typeDict.ContainsKey(type))
                                    {
                                        Type localType = typeDict[type];
                                        debugComponent = (IDebugComponent)Activator.CreateInstance(localType);
                                        if (debugComponent != null)
                                        {
                                            debugComponent.Target = componentList[i];
                                        }
                                    }
                                }
                                else
                                {
                                    componentIndex = -1;
                                    debugComponent = null;
                                }
                            }

                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();

                            SelectComponent(i);
                        }
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }


        private void SelectComponent(int index)
        {
            //当前组件被选中
            if (componentIndex == index)
            {
                GUI.contentColor = Color.white;
                GUILayout.BeginVertical(DebugData.Box);

                if (debugComponent != null)
                {
                    debugComponent.OnDebugScene();
                }
                else
                {
                    GUILayout.Label("No Debug GUI!", DebugData.Label);
                }

                GUILayout.EndVertical();
            }
        }

        private void DebugScene()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(DebugConst.GameObject + " [" + objectList.Count + "]", DebugData.Button, DebugData.Height);
            GUI.contentColor = Color.white;
            if (GUILayout.Button(DebugConst.Refresh, DebugData.Button, DebugData.SceneBox, DebugData.Height))
            {
                RefreshGameObject();
                RefreshComponent();
                return;
            }

            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            ShowGameObject();
            ShowComponent();
            GUILayout.EndHorizontal();
        }
    }
}