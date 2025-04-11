// // *********************************************************************************
// // # Project: Astraia
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 22:04:20
// // # Recently: 2025-04-09 22:04:20
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

namespace Astraia
{
    internal enum AssetPlatform : byte
    {
        StandaloneOSX = 2,
        StandaloneWindows = 5,
        IOS = 9,
        Android = 13,
        WebGL = 20
    }

    internal enum AssetMode : byte
    {
        Simulate,
        Authentic
    }
     
    internal enum BuildMode : byte
    {
        StreamingAssets,
        BuildPath,
    }
    
    public enum UIState : byte
    {
        Common = 1 << 0,
        Freeze = 1 << 1,
        Stable = 1 << 2,
        Vertical = 1 << 3,
        Horizontal = 1 << 4,
    }
    
    public enum UILayer : byte
    {
        Lowest,
        Low,
        Lower,
        Middle,
        Higher,
        High,
        Highest,
    }
}