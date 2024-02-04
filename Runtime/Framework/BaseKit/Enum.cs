// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  13:29
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

namespace JFramework
{
    public enum UILayer : byte
    {
        Bottom = 0,
        Normal = 1,
        Middle = 2,
        Height = 3,
        Ignore = 4
    }

    public enum UIState : byte
    {
        Default = 0,
        Freeze = 1,
        DontDestroy = 2
    }

    internal enum TimerState : byte
    {
        Run = 0,
        Stop = 1,
        Complete = 2
    }

    internal enum AssetPlatform : byte
    {
        StandaloneOSX = 2,
        StandaloneWindows = 5,
        IOS = 9,
        Android = 13,
        WebGL = 20
    }
}