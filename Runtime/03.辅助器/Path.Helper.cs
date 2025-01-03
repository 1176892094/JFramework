// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:28
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;

namespace JFramework
{
    public interface IPathHelper : IBaseHelper
    {
        bool assetPackMode { get; }
        string assetPlatform { get; }
        string assetPackPath { get; }
        string assetPackName { get; }
        string assetRemotePath { get; }
        string streamingAssetsPath { get; }
        string persistentDataPath { get; }
    }
}