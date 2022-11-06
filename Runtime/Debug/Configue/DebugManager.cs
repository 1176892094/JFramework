using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Profiling;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Net.Mail;
using System.Text;
using System.Net;
using JYJFramework;

namespace JYJFramework
{
    public class DebugManager : SingletonMono<DebugManager>
    {
        private DebugData debugData;
        private bool AllowDebugging = true;
        private GUISkin debugController;
        private readonly float MinWidth = 180;
        private readonly float MinHeight = 60;
        private float MaxWidth;
        private float MaxHeight;
        
        private DebugType debugType = DebugType.Console;
        private Rect minWindowRect;
        private Rect maxWindowRect;
        //FPS
        private int curFPS;
        private int maxFPS;
        private int minFPS = 60;
        private Color colorFPS = Color.white;
        private float showFPSTime;
        private bool expansion;
        //Console
        private readonly List<LogData> logList = new List<LogData>();
        private int logIndex = -1;
        private int logInfo;
        private int logWarn;
        private int logError;
        private int logException;
        private int logFatal;
        private bool showLogInfo = true;
        private bool showLogWarn = true;
        private bool showLogError = true;
        private bool showLogException = true;
        private bool showLogFatal = true;
        private Vector2 scrollLogView = Vector2.zero;
        private Vector2 scrollcurLogView = Vector2.zero;
        //Scene
        private List<Transform> objectList = new List<Transform>();
        private int curObjectIndex = -1;
        private float curObjectMemory;
        private List<Component> objectComponents = new List<Component>();
        private int curComponentIndex = -1;
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
            MaxWidth = Screen.width;
            MaxHeight = Screen.height;
            minWindowRect = new Rect(0, 0, 200, 120);
            maxWindowRect = new Rect(0, 0, MaxWidth, MaxHeight);
            debugData = ResourceManager.Load<DebugData>("DebugData");
            debugController = ResourceManager.Load<GUISkin>("DebugController");
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
                object[] attrs = type.GetCustomAttributes(typeof(DebugAttribute), true);
                foreach (object attr in attrs)
                {
                    if (attr is DebugAttribute debugger)
                    {
                        debugComponents.Add(debugger.InspectedType, type);
                    }
                }
            }
        }
        private void Update()
        {
            if (!AllowDebugging) return;
            FPSUpdate();
        }
        private void OnGUI()
        {
            if (!AllowDebugging) return;
            GUI.skin = debugController;
            if (expansion)
            {
                maxWindowRect = GUI.Window(0, maxWindowRect,ExpansionGUIWindow, debugData.GetData("Debugger"));
            }
            else
            {
                minWindowRect = GUI.Window(0, minWindowRect, ShrinkGUIWindow, debugData.GetData("Debugger"));
            }
        }

        private void OnDestroy() => Application.logMessageReceived -= LogMessageReceived;

        #endregion

        #region Additional Function

        /// <summary>
        /// 刷新FPS
        /// </summary>
        private void FPSUpdate()
        {
            float time = Time.realtimeSinceStartup - showFPSTime;
            if (time < 1) return;
            curFPS = (int)(1.0f / Time.deltaTime);
            showFPSTime = Time.realtimeSinceStartup;
            if (curFPS > maxFPS) maxFPS = curFPS;
            if (curFPS < minFPS) minFPS = curFPS;
        }

        /// <summary>
        /// 日志回调
        /// </summary>
        private void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            LogData log = new LogData();
            log.time = DateTime.Now.ToString("HH:mm:ss");
            log.message = condition;
            log.stackTrace = stackTrace;
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
            log.showName = "[" + log.time + "] [" + log.type + "] " + log.message;
            logList.Add(log);

            if (logWarn > 0)
            {
                colorFPS = Color.yellow;
            }
            if (logException > 0)
            {
                colorFPS = Color.magenta;
            }
            if (logError > 0)
            {
                colorFPS = Color.red;
            }
            if (logFatal > 0)
            {
                colorFPS = new Color(1f, 0.5f, 0);
            }
        }
        /// <summary>
        /// 屏幕截图
        /// </summary>
        private IEnumerator ScreenShot()
        {
            string path = "";
#if UNITY_EDITOR
            path = Application.dataPath + "/Screen/";
#endif
            if (path != "")
            {
                AllowDebugging = false;
                yield return new WaitForEndOfFrame();
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                texture.Apply();
                string title = DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(path + title, bytes);
                AllowDebugging = true;
            }
            else
            {
                Debug.LogWarning("当前平台不支持截屏！");
                yield return 0;
            }
        }
        /// <summary>
        /// 刷新场景中所有物体
        /// </summary>
        private void RefreshSceneTransforms()
        {
            objectList.Clear();
            objectList = FindObjectsOfType<Transform>().ToList();
            curObjectIndex = -1;
        }
        /// <summary>
        /// 刷新当前选择物体的所有组件
        /// </summary>
        private void RefreshTransformComponents()
        {
            objectComponents.Clear();
            if (curObjectIndex != -1 && curObjectIndex < objectList.Count)
            {
                objectComponents = objectList[curObjectIndex].GetComponents<Component>().ToList();
            }
            curComponentIndex = -1;
            isAddComponent = false;
            curDebugger = null;
        }
        
        #endregion

        #region GUI Function

        /// <summary>
        /// 展开的GUI窗口
        /// </summary>
        private void ExpansionGUIWindow(int windowId)
        {
            GUILayout.Space(20);
            ExpansionTitleGUI();
            switch (debugType)
            {
                case DebugType.Console:
                    ExpansionConsoleGUI();
                    break;
                case DebugType.Scene:
                    ExpansionSceneGUI();
                    break;
                case DebugType.Memory:
                    ExpansionMemoryGUI();
                    break;
                case DebugType.DrawCall:
                    ExpansionDrawCallGUI();
                    break;
                case DebugType.System:
                    ExpansionSystemGUI();
                    break;
                case DebugType.Screen:
                    ExpansionScreenGUI();
                    break;
                case DebugType.Time:
                    ExpansionTimeGUI();
                    break;
                case DebugType.Environment:
                    ExpansionEnvironmentGUI();
                    break;
            }
        }
        private void ExpansionTitleGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = colorFPS;
            if (GUILayout.Button(debugData.GetData("FPS") + ": " + curFPS, GUILayout.Width(MinWidth),GUILayout.Height(MinHeight)))
            {
                expansion = false;
            }
            GUI.contentColor = (debugType == DebugType.Console ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("Console"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Console;
            }
            GUI.contentColor = (debugType == DebugType.Scene ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("Scene"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Scene;
                RefreshSceneTransforms();
                RefreshTransformComponents();
            }
            GUI.contentColor = (debugType == DebugType.Memory ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("Memory"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Memory;
            }
            GUI.contentColor = (debugType == DebugType.DrawCall ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("DrawCall"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.DrawCall;
            }
            GUI.contentColor = (debugType == DebugType.System ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("System"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.System;
            }
            GUI.contentColor = (debugType == DebugType.Screen ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("Screen"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Screen;
            }
            GUI.contentColor = (debugType == DebugType.Time ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("Time"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Time;
            }
            GUI.contentColor = (debugType == DebugType.Environment ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("Environment"), GUILayout.Height(MinHeight)))
            {
                debugType = DebugType.Environment;
            }
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
        private void ExpansionConsoleGUI()
        {
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            if (GUILayout.Button(debugData.GetData("Clear"), GUILayout.Height(MinHeight)))
            {
                logList.Clear();
                logFatal = 0;
                logWarn = 0;
                logError = 0;
                logException = 0;
                logInfo = 0;
                logIndex = -1;
                colorFPS = Color.white;
            }
            GUI.contentColor = (showLogInfo ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("Log") + " [" + logInfo + "]", GUILayout.Height(MinHeight)))
            {
                showLogInfo = !showLogInfo;
            }
            GUI.contentColor = (showLogWarn ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("Warning") + " [" + logWarn + "]",  GUILayout.Height(MinHeight)))
            {
                showLogWarn = !showLogWarn;
            }
            GUI.contentColor = (showLogException ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("Exception") + " [" + logException + "]",  GUILayout.Height(MinHeight)))
            {
                showLogException = !showLogException;
            }
            GUI.contentColor = (showLogError ? Color.white : Color.gray);
            if (GUILayout.Button(debugData.GetData("Error") + " [" + logError + "]",  GUILayout.Height(MinHeight)))
            {
                showLogError = !showLogError;
            }
            GUI.contentColor = (showLogFatal ? Color.white : Color.gray);
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
        private void ExpansionSceneGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(debugData.GetData("GameObjects") + " [" +objectList.Count + "]",  "Button",GUILayout.Height(MinHeight));
            GUI.contentColor = Color.white;
            if (GUILayout.Button(debugData.GetData("Refresh"), GUILayout.Width((MaxWidth - 25) / 2),GUILayout.Height(MinHeight)))
            {
                RefreshSceneTransforms();
                RefreshTransformComponents();
                return;
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            
            #region 场景物体列表
            GUILayout.BeginHorizontal();
            GUI.contentColor = Color.white;
            GUILayout.BeginVertical("Box", GUILayout.Width((MaxWidth - 25) / 2));

            GUILayout.BeginHorizontal();
            objectFilter = GUILayout.TextField(objectFilter,GUILayout.Height(MinHeight-10));
            GUILayout.EndHorizontal();

            //场景物体列表
            scrollSceneView = GUILayout.BeginScrollView(scrollSceneView);
            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList[i] && objectList[i].name.Contains(objectFilter))
                {
                    if (objectList[i].gameObject.hideFlags == HideFlags.HideInHierarchy) continue;
                    GUILayout.BeginHorizontal();
                    GUI.contentColor = objectList[i].gameObject.activeSelf ? Color.cyan : Color.gray;
                    bool value = curObjectIndex == i;
                    if (GUILayout.Toggle(value, objectList[i].name,"Label") != value)
                    {
                        if (curObjectIndex != i)
                        {
                            curObjectIndex = i;
#if ENABLE_PROFILER
                            curObjectMemory = Profiler.GetRuntimeMemorySizeLong(objectList[i].gameObject) / 1000f;
#endif
                            RefreshTransformComponents();
                        }
                        else
                        {
                            curObjectIndex = -1;
                            RefreshTransformComponents();
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    //当前物体被选中
                    if (curObjectIndex == i)
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
                        GUILayout.Label("Memory" + ": " + curObjectMemory + "KB");
                        GUILayout.EndHorizontal();
#endif
                        GUILayout.EndVertical();
                    }
                }
            }
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
            #endregion

            #region 被选中物体的组件列表
            GUI.contentColor = Color.white;
            GUILayout.BeginVertical("Box", GUILayout.Width((MaxWidth - 25) / 2));

            //添加、删除组件
            if (curObjectIndex != -1)
            {
                GUILayout.BeginHorizontal();
                isAddComponent = GUILayout.Toggle(isAddComponent, debugData.GetData("Add Component"), "Button",GUILayout.Height(MinHeight-10));
                if (curComponentIndex != -1 && curComponentIndex < objectComponents.Count)
                {
                    if (objectComponents[curComponentIndex])
                    {
                        if (GUILayout.Button(debugData.GetData("Delete Component"),GUILayout.Height(MinHeight-10)))
                        {
                            if (objectComponents[curComponentIndex] is DebugManager)
                            {
                                Debug.LogWarning("不能销毁组件 " + objectComponents[curComponentIndex].GetType().Name + " ！");
                            }
                            else
                            {
                                Destroy(objectComponents[curComponentIndex]);
                                RefreshTransformComponents();
                                return;
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }

            //被选中物体的组件列表
            scrollInspectorView = GUILayout.BeginScrollView(scrollInspectorView);
            if (curObjectIndex != -1)
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
                                if (type.IsSubclassOf(baseType))
                                {
                                    if (type.Name.Contains(componentFilter))
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
                                objectList[curObjectIndex].gameObject.AddComponent(addComponent);
                                isAddComponent = false;
                                RefreshTransformComponents();
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
                            bool value = curComponentIndex == i;
                            if (GUILayout.Toggle(value, objectComponents[i].GetType().Name,"Label") != value)
                            {
                                if (curComponentIndex != i)
                                {
                                    curComponentIndex = i;
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
                                    curComponentIndex = -1;
                                    curDebugger = null;
                                }
                            }
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();

                            //当前组件被选中
                            if (curComponentIndex == i)
                            {
                                GUI.contentColor = Color.white;
                                GUILayout.BeginVertical("Box");

                                if (curDebugger != null)
                                {
                                    curDebugger.OnDebuggerGUI();
                                }
                                else
                                {
                                    GUILayout.Label("No Debugger GUI!");
                                }

                                GUILayout.EndVertical();
                            }
                        }
                    }
                }
            }
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
            #endregion

            GUILayout.EndHorizontal();
        }
        private void ExpansionMemoryGUI()
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
        private void ExpansionDrawCallGUI()
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
        private void ExpansionSystemGUI()
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
        private void ExpansionScreenGUI()
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
                StartCoroutine(ScreenShot());
            }
            GUILayout.EndHorizontal();
        }
        
        private void ExpansionTimeGUI()
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
        private void ExpansionEnvironmentGUI()
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
            if (Application.internetReachability == NetworkReachability.NotReachable)
                internetState = debugData.GetData("NotReachable");
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                internetState = debugData.GetData("ReachableViaLocalAreaNetwork");
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
                internetState = debugData.GetData("ReachableViaCarrierDataNetwork");
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
                expansion = true;
            }
            GUI.contentColor = Color.white;
        }
        #endregion
    }
    #region Additional Type
    public struct LogData
    {
        public string time;
        public string type;
        public string message;
        public string stackTrace;
        public string showName;
    }
    public enum DebugType
    {
        Console,
        Scene,
        Memory,
        DrawCall,
        System,
        Screen,
        Time,
        Environment
    }
    #endregion
}