using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace JFramework.Debug
{
    internal class DebugScene
    {
        private readonly DebugData debugData;
        private List<Transform> objectList = new List<Transform>();
        private int objectIndex = -1;
        private float objectMemory;
        private List<Component> objectComponents = new List<Component>();
        private int componentIndex = -1;
        private readonly Dictionary<Type, Type> debugComponents = new Dictionary<Type, Type>();
        private readonly List<Type> addComponents = new List<Type>();
        private string objectFilter = "";
        private string componentFilter = "";
        private bool isAddComponent;
        private DebugComponent curDebugger;
        private Vector2 scrollSceneView = Vector2.zero;
        private Vector2 scrollInspectorView = Vector2.zero;
        public DebugScene(DebugData debugData) => this.debugData = debugData;
        public void Start()
        {
            //获取所有调试组件
            Type baseType = typeof(DebugComponent);
            Assembly assembly = Assembly.GetAssembly(baseType);
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.BaseType != baseType) continue;
                object[] attributes = type.GetCustomAttributes(typeof(DebugAttribute), true);
                foreach (object attribute in attributes)
                {
                    if (attribute is DebugAttribute debugger)
                    {
                        debugComponents.Add(debugger.InspectedType, type);
                    }
                }
            }
        }

        public void RefreshGameObject()
        {
            objectList.Clear();
            objectList = Object.FindObjectsOfType<Transform>().ToList();
            objectIndex = -1;
        }

        public void RefreshComponent()
        {
            objectComponents.Clear();
            if (objectIndex != -1 && objectIndex < objectList.Count)
            {
                objectComponents = objectList[objectIndex].GetComponents<Component>().ToList();
            }

            componentIndex = -1;
            isAddComponent = false;
            curDebugger = null;
        }

        private void ShowGameObject()
        {
            GUI.contentColor = Color.white;
            GUILayout.BeginVertical(DebugStyle.Box, GUILayout.Width((debugData.MaxWidth - 25) / 2));
            GUILayout.BeginHorizontal();
            objectFilter = GUILayout.TextField(objectFilter, DebugStyle.TextField, DebugStyle.MinHeightFix);
            GUILayout.EndHorizontal();
            //场景物体列表
            scrollSceneView = GUILayout.BeginScrollView(scrollSceneView);
            for (int i = 0; i < objectList.Count; i++)
            {
                if (!objectList[i] || !objectList[i].name.Contains(objectFilter)) continue;
                if (objectList[i].gameObject.hideFlags == HideFlags.HideInHierarchy) continue;
                GUILayout.BeginHorizontal();
                GUI.contentColor = objectList[i].gameObject.activeSelf ? Color.cyan : Color.gray;
                bool value = objectIndex == i;
                if (GUILayout.Toggle(value, objectList[i].name, DebugStyle.Label) != value)
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

                //当前物体被选中
                if (objectIndex == i)
                {
                    GUI.contentColor = objectList[i].gameObject.activeSelf ? Color.white : Color.gray;
                    GUILayout.BeginVertical(DebugStyle.Box);
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Tag: " + objectList[i].tag, DebugStyle.Label);
                    GUILayout.EndHorizontal();
                    
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Layer: " + LayerMask.LayerToName(objectList[i].gameObject.layer), DebugStyle.Label);
                    GUILayout.EndHorizontal();
#if ENABLE_PROFILER
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Memory" + ": " + objectMemory + "KB", DebugStyle.Label);
                    GUILayout.EndHorizontal();
#endif
                    GUILayout.EndVertical();
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void ShowComponent()
        {
            GUI.contentColor = Color.white;
            GUILayout.BeginVertical(DebugStyle.Box, GUILayout.Width((debugData.MaxWidth - 25) / 2));
            //添加、删除组件
            if (objectIndex != -1)
            {
                GUILayout.BeginHorizontal();
                isAddComponent = GUILayout.Toggle(isAddComponent, debugData.GetData("Add Component"), DebugStyle.Button, DebugStyle.MinHeightFix);
                if (componentIndex != -1 && componentIndex < objectComponents.Count)
                {
                    if (objectComponents[componentIndex])
                    {
                        if (GUILayout.Button(debugData.GetData("Delete Component"), DebugStyle.Button, DebugStyle.MinHeightFix))
                        {
                            if (objectComponents[componentIndex] is DebugManager)
                            {
                                Logger.LogWarning("不能销毁组件 " + objectComponents[componentIndex].GetType().Name + " ！");
                            }
                            else
                            {
                                Object.Destroy(objectComponents[componentIndex]);
                                RefreshComponent();
                                return;
                            }
                        }
                    }
                }

                GUILayout.EndHorizontal();
            }

            //被选中物体的组件列表
            scrollInspectorView = GUILayout.BeginScrollView(scrollInspectorView);
            if (objectIndex != -1)
            {
                //添加组件状态
                if (isAddComponent)
                {
                    GUILayout.BeginHorizontal();
                    componentFilter = GUILayout.TextField(componentFilter, DebugStyle.TextField, DebugStyle.MinHeightFix);
                    if (GUILayout.Button(debugData.GetData("Search"), DebugStyle.Button, DebugStyle.MinHeightFix))
                    {
                        addComponents.Clear();
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
                                        addComponents.Add(type);
                                    }
                                }
                            }
                        }
                    }

                    GUILayout.EndHorizontal();

                    foreach (var addComponent in addComponents)
                    {
                        string fullName = addComponent.FullName;
                        if (fullName != null)
                        {
                            string[] strArray = fullName.Split('.');
                            if (GUILayout.Button(strArray[^1], DebugStyle.Button, DebugStyle.MinHeightFix))
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
                    for (int i = 0; i < objectComponents.Count; i++)
                    {
                        if (objectComponents[i])
                        {
                            GUILayout.BeginHorizontal();
                            GUI.contentColor = Color.cyan;
                            bool value = componentIndex == i;
                            if (GUILayout.Toggle(value, objectComponents[i].GetType().Name, DebugStyle.Label) != value)
                            {
                                if (componentIndex != i)
                                {
                                    componentIndex = i;
                                    curDebugger = null;
                                    Type type = objectComponents[i].GetType();
                                    if (debugComponents.ContainsKey(type))
                                    {
                                        Type debuggerType = debugComponents[type];
                                        curDebugger = Activator.CreateInstance(debuggerType) as DebugComponent;
                                        if (curDebugger != null)
                                        {
                                            curDebugger.Target = objectComponents[i];
                                            curDebugger.OnInit();
                                        }
                                    }
                                }
                                else
                                {
                                    componentIndex = -1;
                                    curDebugger = null;
                                }
                            }

                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();

                            //当前组件被选中
                            if (componentIndex == i)
                            {
                                GUI.contentColor = Color.white;
                                GUILayout.BeginVertical(DebugStyle.Box);

                                if (curDebugger != null)
                                {
                                    curDebugger.OnDebugGUI();
                                }
                                else
                                {
                                    GUILayout.Label("No Debug GUI!", DebugStyle.Label);
                                }

                                GUILayout.EndVertical();
                            }
                        }
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
        
        public void ExtendSceneGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(debugData.GetData("GameObjects") + " [" + objectList.Count + "]", DebugStyle.Button, DebugStyle.MinHeight);
            GUI.contentColor = Color.white;
            if (GUILayout.Button(debugData.GetData("Refresh"), DebugStyle.Button, GUILayout.Width((debugData.MaxWidth - 25) / 2), GUILayout.Height(60)))
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