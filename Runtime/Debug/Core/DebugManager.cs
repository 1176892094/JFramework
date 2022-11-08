using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Profiling;
using System.IO;
using System.Linq;
using System.Reflection;
using JFramework.Async;
using JYJFramework.Logger;

namespace JFramework.Logger
{
    public class DebugManager : SingletonMono<DebugManager>
    {
        private GUISkin controller;
        private DebugData debugData;
        private int curFPS;
        private bool isDebug;
        private bool isExtend;
        private float MinWidth;
        private float MaxWidth;
        private float MinHeight;
        private float MaxHeight;
        private float FPSTimer;
        private Color colorFPS;
        private DebugType debugType;
        private Rect dragWindowRect;
        //Console
        private readonly List<LogData> logList = new List<LogData>();
        private int logIndex = -1;
        private int logInfo;
        private int logWarn;
        private int logError;
        private int logFatal;
        private int logException;
        private bool showLogInfo = true;
        private bool showLogWarn = true;
        private bool showLogError = true;
        private bool showLogFatal = true;
        private bool showLogException = true;
        private Vector2 scrollLogView = Vector2.zero;
        private Vector2 scrollcurLogView = Vector2.zero;
        //Scene
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
        private Debugger curDebugger;
        private Vector2 scrollSceneView = Vector2.zero;
        private Vector2 scrollInspectorView = Vector2.zero;
        //Memory
        private long minTotalReservedMemory = 10000;
        private long maxTotalReservedMemory;
        private long minTotalAllocatedMemory = 10000;
        private long maxTotalAllocatedMemory;
        private long minTotalUnusedReservedMemory = 10000;
        private long maxTotalUnusedReservedMemory;
        private long minMonoHeapSize = 10000;
        private long maxMonoHeapSize;
        private long minMonoUsedSize = 10000;
        private long maxMonoUsedSize;
        //DrawCall
        private Vector2 scrollDrawCallView = Vector2.zero;
        //System
        private Vector2 scrollSystemView = Vector2.zero;

        #region Lifecycle Function

        protected override void Awake()
        {
            base.Awake();
            isDebug = true;
            MinWidth = 180;
            MinHeight = 60;
            colorFPS = Color.white;
            MaxWidth = Screen.width;
            MaxHeight = Screen.height;
            dragWindowRect = new Rect(0, 0, 200, 120);
            debugData = ResourceManager.Load<DebugData>("DebugData");
            controller = ResourceManager.Load<GUISkin>("DebugController");
            debugData.InitData();
            Application.logMessageReceived += LogMessageReceived;
        }

        private void Start()
        {
            //获取所有调试组件
            Type baseType = typeof(Debugger);
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

        private void Update()
        {
            if (!isDebug) return;
            float time = Time.realtimeSinceStartup - FPSTimer;
            if (time < 1) return;
            curFPS = (int)(1.0f / Time.deltaTime);
            FPSTimer = Time.realtimeSinceStartup;
        }
        private void OnGUI()
        {
            if (!isDebug) return;
            GUI.skin = controller;
            if (!isExtend)
            {
                dragWindowRect = GUI.Window(0, dragWindowRect, ShrinkGUIWindow, debugData.GetData("Debugger"));
            }
            else
            {
                GUI.Window(0, new Rect(0, 0, MaxWidth, MaxHeight), ExtendWindowGUI, debugData.GetData("Debugger"));
            }
        }
        private void OnDestroy() => Application.logMessageReceived -= LogMessageReceived;

        #endregion

        #region Additional Function
        
        private void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            LogData log = new LogData
            {
                dateTime = DateTime.Now.ToString("HH:mm:ss"),
                message = condition,
                stackTrace = stackTrace
            };
            switch (type)
            {
                case LogType.Assert:
                    log.type = "Fatal";
                    logFatal += 1;
                    break;
                case LogType.Error:
                    log.type = "Error";
                    logError += 1;
                    break;
                case LogType.Warning:
                    log.type = "Warning";
                    logWarn += 1;
                    break;
                case LogType.Exception:
                    log.type = "Exception";
                    logException += 1;
                    break;
                case LogType.Log:
                    log.type = "Log";
                    logInfo += 1;
                    break;
            }
            log.showName = "[" + log.dateTime + "] [" + log.type + "] " + log.message;
            logList.Add(log);
            
            if (logFatal > 0)
            {
                colorFPS = new Color(1f, 0.5f, 0);
            }
            else if (logError > 0)
            {
                colorFPS = Color.red;
            }
            else if (logException > 0)
            {
                colorFPS = Color.magenta;
            }
            else if (logWarn > 0)
            {
                colorFPS = Color.yellow;
            }
        }

        private async void ScreenShot()
        {
            string path = "";
#if UNITY_EDITOR
            path = Application.dataPath + "/Screen/";
#endif
            if (path != "")
            {
                isDebug = false;
                await new WaitForEndOfFrame();
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                texture.Apply();
                string title = DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
                byte[] bytes = texture.EncodeToPNG();
                isDebug = true;
                await File.WriteAllBytesAsync(path + title, bytes);
            }
            else
            {
                Debug.LogWarning("当前平台不支持截屏！");
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
            GUILayout.BeginVertical("Box", GUILayout.Width((MaxWidth - 25) / 2));
            GUILayout.BeginHorizontal();
            objectFilter = GUILayout.TextField(objectFilter,GUILayout.Height(MinHeight-10));
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
                if (GUILayout.Toggle(value, objectList[i].name,"Label") != value)
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
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Tag: " + objectList[i].tag);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Layer: " + LayerMask.LayerToName(objectList[i].gameObject.layer));
                    GUILayout.EndHorizontal();
#if ENABLE_PROFILER
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Memory" + ": " + objectMemory + "KB");
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
            GUILayout.BeginVertical("Box", GUILayout.Width((MaxWidth - 25) / 2));
            //添加、删除组件
            if (objectIndex != -1)
            {
                GUILayout.BeginHorizontal();
                isAddComponent = GUILayout.Toggle(isAddComponent, debugData.GetData("Add Component"), "Button",GUILayout.Height(MinHeight-10));
                if (componentIndex != -1 && componentIndex < objectComponents.Count)
                {
                    if (objectComponents[componentIndex])
                    {
                        if (GUILayout.Button(debugData.GetData("Delete Component"),GUILayout.Height(MinHeight-10)))
                        {
                            if (objectComponents[componentIndex] is DebugManager)
                            {
                                Debug.LogWarning("不能销毁组件 " + objectComponents[componentIndex].GetType().Name + " ！");
                            }
                            else
                            {
                                Destroy(objectComponents[componentIndex]);
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
                    componentFilter = GUILayout.TextField(componentFilter, GUILayout.Height(MinHeight-10));
                    if (GUILayout.Button(debugData.GetData("Search"),GUILayout.Height(MinHeight-10)))
                    {
                        addComponents.Clear();
                        Type baseType = typeof(Component);
                        Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
                        foreach (Assembly assembly in assemblys)
                        {
                            Type[] types = assembly.GetTypes();
                            foreach (Type type in types)
                            {
                                if (type.IsSubclassOf(baseType)&&type.Name.Contains(componentFilter))
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
                            if (GUILayout.Button(strArray[^1],GUILayout.Height(MinHeight-10)))
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
                            if (GUILayout.Toggle(value, objectComponents[i].GetType().Name,"Label") != value)
                            {
                                if (componentIndex != i)
                                {
                                    componentIndex = i;
                                    curDebugger = null;
                                    Type type = objectComponents[i].GetType();
                                    if (debugComponents.ContainsKey(type))
                                    {
                                        Type debuggerType = debugComponents[type];
                                        curDebugger = Activator.CreateInstance(debuggerType) as Debugger;
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
                                GUILayout.BeginVertical("Box");

                                if (curDebugger != null)
                                {
                                    curDebugger.OnDebugGUI();
                                }
                                else
                                {
                                    GUILayout.Label("No Debug GUI!");
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

        #endregion

        #region GUI Function

        /// <summary>
        /// 展开的GUI窗口
        /// </summary>
        private void ExtendWindowGUI(int windowId)
        {
            GUILayout.Space(20);
            ExtendTitleGUI();
            switch (debugType)
            {
                case DebugType.Console:
                    ExtendConsoleGUI();
                    break;
                case DebugType.Scene:
                    ExtendSceneGUI();
                    break;
                case DebugType.Memory:
                    ExtendMemoryGUI();
                    break;
                case DebugType.DrawCall:
                    ExtendDrawCallGUI();
                    break;
                case DebugType.System:
                    ExtendSystemGUI();
                    break;
                case DebugType.Screen:
                    ExtendScreenGUI();
                    break;
                case DebugType.Time:
                    ExtendTimeGUI();
                    break;
                case DebugType.Environment:
                    ExtendEnvironmentGUI();
                    break;
            }
        }
        private void ExtendTitleGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = colorFPS;
            if (GUILayout.Button(debugData.GetData("FPS") + ": " + curFPS, GUILayout.Width(MinWidth),GUILayout.Height(MinHeight)))
            {
                isExtend = false;
            }
            GUI.contentColor = debugType == DebugType.Console ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Console"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Console;
            }
            GUI.contentColor = debugType == DebugType.Scene ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Scene"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Scene;
                RefreshGameObject();
                RefreshComponent();
            }
            GUI.contentColor = debugType == DebugType.Memory ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Memory"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Memory;
            }
            GUI.contentColor = debugType == DebugType.DrawCall ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("DrawCall"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.DrawCall;
            }
            GUI.contentColor = debugType == DebugType.System ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("System"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.System;
            }
            GUI.contentColor = debugType == DebugType.Screen ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Screen"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Screen;
            }
            GUI.contentColor = debugType == DebugType.Time ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Time"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Time;
            }
            GUI.contentColor = debugType == DebugType.Environment ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Environment"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Environment;
            }
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
        private void ExtendConsoleGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            if (GUILayout.Button(debugData.GetData("Clear"), GUILayout.Height(MinHeight)))
            {
                logList.Clear();
                logIndex = -1;
                logInfo = 0;
                logWarn = 0;
                logError = 0;
                logFatal = 0;
                logException = 0;
                colorFPS = Color.white;
            }
            GUI.contentColor = showLogInfo ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Log") + " [" + logInfo + "]", GUILayout.Height(MinHeight)))
            {
                showLogInfo = !showLogInfo;
            }
            GUI.contentColor = showLogWarn ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Warning") + " [" + logWarn + "]",  GUILayout.Height(MinHeight)))
            {
                showLogWarn = !showLogWarn;
            }
            GUI.contentColor = showLogException ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Exception") + " [" + logException + "]",  GUILayout.Height(MinHeight)))
            {
                showLogException = !showLogException;
            }
            GUI.contentColor = showLogError ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Error") + " [" + logError + "]",  GUILayout.Height(MinHeight)))
            {
                showLogError = !showLogError;
            }
            GUI.contentColor = showLogFatal ? Color.white : Color.gray;
            if (GUILayout.Button(debugData.GetData("Fatal") + " [" + logFatal + "]" , GUILayout.Height(MinHeight)))
            {
                showLogFatal = !showLogFatal;
            }
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            scrollLogView = GUILayout.BeginScrollView(scrollLogView, "Box", GUILayout.Height(Screen.height * 0.5f));
            for (int i = 0; i < logList.Count; i++)
            {
                bool show = false;
                Color color = Color.white;
                switch (logList[i].type)
                {
                    case "Fatal":
                        show = showLogFatal;
                        color = new Color(1, 0.5f, 0);
                        break;
                    case "Exception":
                        show = showLogException;
                        color = Color.magenta;
                        break;
                    case "Error":
                        show = showLogError;
                        color = Color.red;
                        break;
                    case "Log":
                        show = showLogInfo;
                        color = Color.white;
                        break;
                    case "Warning":
                        show = showLogWarn;
                        color = Color.yellow;
                        break;
                }

                if (show)
                {
                    GUILayout.BeginHorizontal();
                    GUI.contentColor = color;
                    if (GUILayout.Toggle(logIndex == i, logList[i].showName, "Label"))
                    {
                        logIndex = i;
                    }

                    GUI.contentColor = Color.white;
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();

            scrollcurLogView = GUILayout.BeginScrollView(scrollcurLogView, "Box");
            if (logIndex != -1)
            {
                GUILayout.Label(logList[logIndex].message + "\r\n\r\n" + logList[logIndex].stackTrace);
            }
            GUILayout.EndScrollView();
        }
        private void ExtendSceneGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(debugData.GetData("GameObjects") + " [" +objectList.Count + "]",  "Button",GUILayout.Height(MinHeight));
            GUI.contentColor = Color.white;
            if (GUILayout.Button(debugData.GetData("Refresh"), GUILayout.Width((MaxWidth - 25) / 2),GUILayout.Height(MinHeight)))
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
        private void ExtendMemoryGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("Memory Information"), GUILayout.Height(MinHeight));
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("Box", GUILayout.Height(MaxHeight - 260));
            long memory = Profiler.GetTotalReservedMemoryLong() / 1000000;
            if (memory > maxTotalReservedMemory) maxTotalReservedMemory = memory;
            if (memory < minTotalReservedMemory) minTotalReservedMemory = memory;
            GUILayout.Label(debugData.GetData("Total Memory") + ": " + memory + "MB        " +
                "[" + debugData.GetData("Min") + ": " + minTotalReservedMemory + "  " + debugData.GetData("Max") + ": " + maxTotalReservedMemory + "]");

            memory = Profiler.GetTotalAllocatedMemoryLong() / 1000000;
            if (memory > maxTotalAllocatedMemory) maxTotalAllocatedMemory = memory;
            if (memory < minTotalAllocatedMemory) minTotalAllocatedMemory = memory;
            GUILayout.Label(debugData.GetData("Used Memory") + ": " + memory + "MB        " +
                "[" + debugData.GetData("Min") + ": " + minTotalAllocatedMemory + "  " + debugData.GetData("Max") + ": " + maxTotalAllocatedMemory + "]");

            memory = Profiler.GetTotalUnusedReservedMemoryLong() / 1000000;
            if (memory > maxTotalUnusedReservedMemory) maxTotalUnusedReservedMemory = memory;
            if (memory < minTotalUnusedReservedMemory) minTotalUnusedReservedMemory = memory;
            GUILayout.Label(debugData.GetData("Free Memory") + ": " + memory + "MB        " +
                "[" + debugData.GetData("Min") + ": " + minTotalUnusedReservedMemory + "  " + debugData.GetData("Max") + ": " + maxTotalUnusedReservedMemory + "]");

            memory = Profiler.GetMonoHeapSizeLong() / 1000000;
            if (memory > maxMonoHeapSize) maxMonoHeapSize = memory;
            if (memory < minMonoHeapSize) minMonoHeapSize = memory;
            GUILayout.Label(debugData.GetData("Total Mono Memory") + ": " + memory + "MB        " +
                "[" + debugData.GetData("Min") + ": " + minMonoHeapSize + "  " + debugData.GetData("Max") + ": " + maxMonoHeapSize + "]");

            memory = Profiler.GetMonoUsedSizeLong() / 1000000;
            if (memory > maxMonoUsedSize) maxMonoUsedSize = memory;
            if (memory < minMonoUsedSize) minMonoUsedSize = memory;
            GUILayout.Label(debugData.GetData("Used Mono Memory") + ": " + memory + "MB        " +
                "[" + debugData.GetData("Min") + ": " + minMonoUsedSize + "  " + debugData.GetData("Max") + ": " + maxMonoUsedSize + "]");

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(debugData.GetData("Uninstall unused resources"), GUILayout.Height(MinHeight)))
            {
                Resources.UnloadUnusedAssets();
            }
            if (GUILayout.Button(debugData.GetData("Garbage Collection"), GUILayout.Height(MinHeight)))
            {
                GC.Collect();
            }
            GUILayout.EndHorizontal();
        }
        private void ExtendDrawCallGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("DrawCall Information"), GUILayout.Height(MinHeight));
            GUILayout.EndHorizontal();
            scrollDrawCallView = GUILayout.BeginScrollView(scrollDrawCallView, "Box");
#if UNITY_EDITOR
            GUILayout.Label(debugData.GetData("DrawCalls") + ": " + UnityEditor.UnityStats.drawCalls);
            GUILayout.Label(debugData.GetData("Batches") + ": " + UnityEditor.UnityStats.batches);
            GUILayout.Label(debugData.GetData("Static Batched DrawCalls") + ": " + UnityEditor.UnityStats.staticBatchedDrawCalls);
            GUILayout.Label(debugData.GetData("Static Batches") + ": " + UnityEditor.UnityStats.staticBatches);
            GUILayout.Label(debugData.GetData("Dynamic Batched DrawCalls") + ": " + UnityEditor.UnityStats.dynamicBatchedDrawCalls);
            GUILayout.Label(debugData.GetData("Dynamic Batches") + ": " + UnityEditor.UnityStats.dynamicBatches);
            if (UnityEditor.UnityStats.triangles > 10000)
            {
                GUILayout.Label(debugData.GetData("Triangles") + ": " + UnityEditor.UnityStats.triangles / 10000 + "W");
            }
            else
            {
                GUILayout.Label(debugData.GetData("Triangles") + ": " + UnityEditor.UnityStats.triangles);
            }
            if (UnityEditor.UnityStats.vertices > 10000)
            {
                GUILayout.Label(debugData.GetData("Vertices") + ": " + UnityEditor.UnityStats.vertices / 10000 + "W");
            }
            else
            {
                GUILayout.Label(debugData.GetData("Vertices") + ": " + UnityEditor.UnityStats.vertices);
            }
#else
            GUILayout.Label("只有在编辑器模式下才能显示DrawCall！");
#endif
            GUILayout.EndScrollView();
        }
        private void ExtendSystemGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("System Information"), GUILayout.Height(MinHeight));
            GUILayout.EndHorizontal();
            scrollSystemView = GUILayout.BeginScrollView(scrollSystemView, "Box");
            GUILayout.Label(debugData.GetData("Operating System") + ": " + SystemInfo.operatingSystem);
            GUILayout.Label(debugData.GetData("System Memory") + ": " + SystemInfo.systemMemorySize + "MB");
            GUILayout.Label(debugData.GetData("Processor") + ": " + SystemInfo.processorType);
            GUILayout.Label(debugData.GetData("Number Of Processor") + ": " + SystemInfo.processorCount);
            GUILayout.Label(debugData.GetData("Graphics Device Name") + ": " + SystemInfo.graphicsDeviceName);
            GUILayout.Label(debugData.GetData("Graphics Device Type") + ": " + SystemInfo.graphicsDeviceType);
            GUILayout.Label(debugData.GetData("Graphics Memory") + ": " + SystemInfo.graphicsMemorySize + "MB");
            GUILayout.Label(debugData.GetData("Graphics DeviceID") + ": " + SystemInfo.graphicsDeviceID);
            GUILayout.Label(debugData.GetData("Graphics Device Vendor") + ": " + SystemInfo.graphicsDeviceVendor);
            GUILayout.Label(debugData.GetData("Graphics Device Vendor ID") + ": " + SystemInfo.graphicsDeviceVendorID);
            GUILayout.Label(debugData.GetData("Device Model") + ": " + SystemInfo.deviceModel);
            GUILayout.Label(debugData.GetData("Device Name") + ": " + SystemInfo.deviceName);
            GUILayout.Label(debugData.GetData("Device Type") + ": " + SystemInfo.deviceType);
            GUILayout.Label(debugData.GetData("Device Unique Identifier") + ": " + SystemInfo.deviceUniqueIdentifier);
            GUILayout.EndScrollView();
        }
        private void ExtendScreenGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("Screen Information"), GUILayout.Height(MinHeight));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box", GUILayout.Height(250), GUILayout.Height(MaxHeight - 260));
            GUILayout.Label(debugData.GetData("DPI") + ": " + Screen.dpi);
            GUILayout.Label(debugData.GetData("Resolution") + ": " + Screen.width + " x " + Screen.height);
            GUILayout.Label(debugData.GetData("Device Resolution") + ": " + Screen.currentResolution.ToString());
            GUILayout.Label(debugData.GetData("Device Sleep") + ": " + (Screen.sleepTimeout == SleepTimeout.NeverSleep ? debugData.GetData("Never Sleep") : debugData.GetData("System Setting")));
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(debugData.GetData("Device Sleep"), GUILayout.Height(MinHeight)))
            {
                Screen.sleepTimeout = Screen.sleepTimeout == SleepTimeout.NeverSleep ? SleepTimeout.SystemSetting : SleepTimeout.NeverSleep;
            }
            if (GUILayout.Button(debugData.GetData("Screen Capture"), GUILayout.Height(MinHeight)))
            {
                ScreenShot();
            }
            GUILayout.EndHorizontal();
        }
        private void ExtendTimeGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("Time Information"), GUILayout.Height(MinHeight));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box", GUILayout.Height(MaxHeight - 260));
            GUILayout.Label(debugData.GetData("Current Time") + ": " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
            GUILayout.Label(debugData.GetData("Elapse Time") + ": " + (int)Time.realtimeSinceStartup);
            GUILayout.Label(debugData.GetData("Time Scale") + ": " + Time.timeScale);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("0.1 " + debugData.GetData("Multiple"), GUILayout.Height(MinHeight)))
            {
                Time.timeScale = 0.1f;
            }
            if (GUILayout.Button("0.2 " + debugData.GetData("Multiple"), GUILayout.Height(MinHeight)))
            {
                Time.timeScale = 0.2f;
            }
            if (GUILayout.Button("0.5 " + debugData.GetData("Multiple"), GUILayout.Height(MinHeight)))
            {
                Time.timeScale = 0.5f;
            }
            if (GUILayout.Button("1 " + debugData.GetData("Multiple"), GUILayout.Height(MinHeight)))
            {
                Time.timeScale = 1;
            }
            if (GUILayout.Button("2 " + debugData.GetData("Multiple"), GUILayout.Height(MinHeight)))
            {
                Time.timeScale = 2;
            }
            if (GUILayout.Button("5 " + debugData.GetData("Multiple"), GUILayout.Height(MinHeight)))
            {
                Time.timeScale = 5;
            }
            if (GUILayout.Button("10 " + debugData.GetData("Multiple"), GUILayout.Height(MinHeight)))
            {
                Time.timeScale = 10;
            }
            GUILayout.EndHorizontal();
        }
        private void ExtendEnvironmentGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.Label(debugData.GetData("Environment Information"), GUILayout.Height(MinHeight));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box", GUILayout.Height(MaxHeight - 260));
            GUILayout.Label(debugData.GetData("Product Name") + ": " + Application.productName);
            GUILayout.Label(debugData.GetData("Product Identifier") + ": " + Application.identifier);
            GUILayout.Label(debugData.GetData("Product Version") + ": " + Application.version);
            GUILayout.Label(debugData.GetData("Product DataPath") + ": " + Application.dataPath);
            GUILayout.Label(debugData.GetData("Company Name") + ": " + Application.companyName);
            GUILayout.Label(debugData.GetData("Unity Version") + ": " + Application.unityVersion);
            GUILayout.Label(debugData.GetData("Has Pro License") + ": " + Application.HasProLicense());
            string internetState = debugData.GetData("NotReachable");
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    internetState = debugData.GetData("NotReachable");
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    internetState = debugData.GetData("ReachableViaLocalAreaNetwork");
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    internetState = debugData.GetData("ReachableViaCarrierDataNetwork");
                    break;
            }
            GUILayout.Label(debugData.GetData("Internet State") + ": " + internetState);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(debugData.GetData("Quit"), GUILayout.Height(MinHeight)))
            {
                Application.Quit();
            }
            GUILayout.EndHorizontal();
        }
        /// <summary>
        /// 收缩的GUI窗口
        /// </summary>
        private void ShrinkGUIWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 40));
            GUI.contentColor = colorFPS;
            GUILayout.Space(20);
            if (GUILayout.Button(debugData.GetData("FPS") + ": " + curFPS, GUILayout.Width(MinWidth), GUILayout.Height(MinHeight)))
            {
                isExtend = true;
            }
            GUI.contentColor = Color.white;
        }
        #endregion
    }
    #region Additional Type
    public struct LogData
    {
        public string type;
        public string dateTime;
        public string message;
        public string stackTrace;
        public string showName;
    }

    public enum DebugType
    {
        Console = 0,
        Scene = 1,
        Memory = 2,
        DrawCall = 3,
        System = 4,
        Screen = 5,
        Time = 6,
        Environment = 7
    }

    #endregion
}