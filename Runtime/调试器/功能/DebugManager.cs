using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    internal sealed partial class DebugManager : MonoBehaviour
    {
        [ShowInInspector, LabelText("开启Debug调试工具"), BoxGroup("调试选项")]
        public static bool IsShowDebug;

        [ShowInInspector, LabelText("显示Json加载信息"), BoxGroup("调试选项")]
        public static bool IsShowJson;

        [ShowInInspector, LabelText("显示Data加载信息"), BoxGroup("调试选项")]
        public static bool IsShowData;

        [ShowInInspector, LabelText("显示Asset加载信息"), BoxGroup("调试选项")]
        public static bool IsShowAsset;

        [ShowInInspector, LabelText("显示Event事件信息"), BoxGroup("调试选项")]
        public static bool IsShowEvent;

        private int FPS;
        private bool IsExtend;
        private Rect minRect;
        private Color FPSColor = Color.white;
        private DebugType debugType;


        private void Awake()
        {
            InitComponent();
            Application.logMessageReceived += LogMessageReceived;
        }

        private void Start()
        {
            TimerManager.Instance.Pop(1).SetLoop(-1, _ =>
            {
                if (!IsShowDebug) return;
                FPS = (int)(1.0f / Time.deltaTime);
            }).Unscale().SetTarget(gameObject);
        }

        private void OnGUI()
        {
            if (!IsShowDebug) return;
            Matrix4x4 cachedMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.Scale(new Vector3(DebugData.WindowScale, DebugData.WindowScale, 1f));

            if (IsExtend)
            {
                Rect screenRect = new Rect(0, 0, DebugData.WindowWidth, DebugData.WindowHeight);
                GUI.Window(0, screenRect, MaxWindow, DebugConst.JFramework + " " + DebugConst.Debugger,
                    DebugData.Window);
            }
            else
            {
                minRect.size = new Vector2(100, 60);
                minRect = GUI.Window(0, minRect, MinWindow, DebugConst.Debugger, DebugData.Window);
            }

            GUI.matrix = cachedMatrix;
        }

        private void MinWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            GUI.contentColor = FPSColor;

            if (GUILayout.Button($"{DebugConst.FPS}: {FPS}", DebugData.Button, DebugData.Width, DebugData.Height))
            {
                IsExtend = true;
            }

            GUI.contentColor = Color.white;
        }

        private void MaxWindow(int windowId)
        {
            DebugTitle();
            switch (debugType)
            {
                case DebugType.Console:
                    DebugConsole();
                    break;
                case DebugType.Scene:
                    DebugScene();
                    break;
                case DebugType.Memory:
                    DebugMemory();
                    break;
                case DebugType.DrawCall:
                    DebugDrawCall();
                    break;
                case DebugType.System:
                    DebugSystem();
                    break;
                case DebugType.Screen:
                    DebugScreen();
                    break;
                case DebugType.Environment:
                    DebugProject();
                    break;
                case DebugType.Time:
                    DebugTime();
                    break;
            }
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= LogMessageReceived;
        }
    }
}