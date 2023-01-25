using UnityEngine;

namespace JFramework
{
    internal static class DebugData
    {
        private static GUISkin Skin => GUI.skin;
        public static GUIStyle Box => Skin.box;
        public static GUIStyle Label => Skin.label;
        public static GUIStyle Button => Skin.button;
        public static GUIStyle Window => Skin.window;
        public static GUIStyle TextField => Skin.textField;
        private static float ScreenWidth => Screen.width;
        private static float ScreenHeight => Screen.height;
        public static float WindowWidth => ScreenWidth / WindowScale;
        public static float WindowHeight => ScreenHeight / WindowScale;
        public static float WindowScale => ScreenWidth / 1920 + ScreenHeight / 1080;
        public static GUILayoutOption Component => GUILayout.Width(60);
        public static GUILayoutOption Width => GUILayout.Width(80);
        public static GUILayoutOption Height => GUILayout.Height(30);
        public static GUILayoutOption HeightLow => GUILayout.Height(25);
        public static GUILayoutOption HeightLog => GUILayout.Height(WindowHeight * 0.4f);
        public static GUILayoutOption WindowBox => GUILayout.Height(WindowHeight - 130);
        public static GUILayoutOption SceneBox => GUILayout.Width((WindowWidth - 25) / 2);

        public static Vector3 Vector3Field(Vector3 value)
        {
            string x = GUILayout.TextField(value.x.ToString("F"), TextField);
            string y = GUILayout.TextField(value.y.ToString("F"), TextField);
            string z = GUILayout.TextField(value.z.ToString("F"), TextField);
            if (x == value.x.ToString("F") && y == value.y.ToString("F") && z == value.z.ToString("F")) return value;
            if (float.TryParse(x, out var x2) && float.TryParse(y, out var y2) && float.TryParse(z, out var z2))
            {
                return new Vector3(x2, y2, z2);
            }

            return value;
        }

        public static Vector2 Vector2Field(Vector2 value)
        {
            string x = GUILayout.TextField(value.x.ToString("F"), TextField);
            string y = GUILayout.TextField(value.y.ToString("F"), TextField);
            if (x == value.x.ToString("F") && y == value.y.ToString("F")) return value;
            if (float.TryParse(x, out var x2) && float.TryParse(y, out var y2))
            {
                return new Vector3(x2, y2);
            }

            return value;
        }

        public static float FloatField(float value)
        {
            string f = GUILayout.TextField(value.ToString("F"), TextField);
            if (f == value.ToString("F")) return value;
            return float.TryParse(f, out var f2) ? f2 : value;
        }

        public static int IntField(int value)
        {
            string f = GUILayout.TextField(value.ToString(), TextField);
            if (f == value.ToString()) return value;
            return int.TryParse(f, out var f2) ? f2 : value;
        }
    }

    internal struct LogData
    {
        public string type;
        public string message;
        public string dateTime;
        public string showTitle;
        internal string stackTrace;
    }

    internal enum DebugType
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

    internal struct DebugConst
    {
        public const string JFramework = "JFRAMEWORK";
        public const string Debugger = "DEBUGGER";
        public const string FPS = "FPS";
        public const string Console = "控制台";
        public const string Scene = "场景";
        public const string Memory = "内存";
        public const string DrawCall = "绘制";
        public const string System = "系统";
        public const string Screen = "屏幕";
        public const string Time = "时间";
        public const string Environment = "环境";
        public const string Clear = "清理";
        public const string Log = "Log";
        public const string Error = "Error";
        public const string Warning = "Warning";
        public const string Exception = "Exception";
        public const string Assert = "Assert";
        public const string Refresh = "刷新";
        public const string GameObject = "游戏物体数量";
        public const string Search = "查找";
        public const string AddComponent = "添加组件";
        public const string RemoveComponent = "移除组件";
        public const string MemoryInformation = "内存信息";
        public const string TotalMemory = "全部内存";
        public const string UsedMemory = "已用内存";
        public const string FreeMemory = "空闲内存";
        public const string TotalMonoMemory = "全部Mono堆内存";
        public const string UsedMonoMemory = "已用Mono堆内存";
        public const string UninstallResources = "释放未使用资源";
        public const string GarbageCollections = "垃圾回收";
        public const string DrawCallInformation = "绘制呼叫信息";
        public const string DrawCalls = "绘制呼叫总数";
        public const string Batches = "绘制批处理总数";
        public const string StaticBatchedDrawCalls = "静态批处理减少数";
        public const string StaticBatches = "静态批处理总数";
        public const string DynamicBatchedDrawCalls = "动态批处理减少数";
        public const string DynamicBatches = "动态批处理总数";
        public const string Triangles = "三角面总数";
        public const string Vertices = "顶点总数";
        public const string SystemInformation = "系统信息";
        public const string OperatingSystem = "操作系统";
        public const string SystemMemory = "系统内存";
        public const string Processor = "处理器";
        public const string NumberOfProcessor = "处理器数量";
        public const string GraphicsDeviceName = "显卡名称";
        public const string GraphicsDeviceType = "显卡类型";
        public const string GraphicsMemory = "显卡内存";
        public const string GraphicsDeviceID = "显卡标识";
        public const string GraphicsDeviceVendor = "显卡供应商";
        public const string GraphicsDeviceVendorID = "显卡供应商标识";
        public const string DeviceModel = "设备模式";
        public const string DeviceName = "设备名称";
        public const string DeviceType = "设备类型";
        public const string DeviceUniqueIdentifier = "设备唯一标识符";
        public const string ScreenInformation = "屏幕信息";
        public const string DPI = "DPI";
        public const string Resolution = "程序分辨率";
        public const string DeviceResolution = "设备分辨率";
        public const string DeviceSleep = "设备休眠";
        public const string NeverSleep = "从不休眠";
        public const string SystemSetting = "沿用系统设置";
        public const string ScreenCapture = "截屏";
        public const string FullScreen = "全屏";
        public const string TimeInformation = "时间信息";
        public const string CurrentTime = "当前时间";
        public const string ElapseTime = "时间流逝";
        public const string TimeScale = "时间速率";
        public const string Multiple = "倍";
        public const string EnvironmentInformation = "环境信息";
        public const string ProductName = "项目名称";
        public const string ProductIdentifier = "项目标识";
        public const string ProductVersion = "项目版本";
        public const string ProductDataPath = "项目路径";
        public const string CompanyName = "公司名称";
        public const string UnityVersion = "Unity版本";
        public const string HasProLicense = "Unity专业版";
        public const string InternetState = "网络状态";
        public const string NotReachable = "无网络连接";
        public const string ReachableViaLocalAreaNetwork = "WIFI网络连接中";
        public const string ReachableViaCarrierDataNetwork = "数据网络连接中";
        public const string Quit = "退出程序";
        public const string Min = "最小值";
        public const string Max = "最大值";
    }
}