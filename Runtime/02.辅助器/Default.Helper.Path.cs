// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:45
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework
{
    [Serializable]
    internal sealed partial class DefaultHelper : IPathHelper
    {
        string IPathHelper.assetPlatform => GlobalSetting.Instance.assetPlatform.ToString();
        bool IPathHelper.assetPackMode => GlobalSetting.Instance.assetPackMode == GlobalSetting.AssetPackMode.Authentic;
        string IPathHelper.assetPackPath => GlobalSetting.Instance.assetPackPath;
        string IPathHelper.assetPackName => GlobalSetting.Instance.assetPackName;
        string IPathHelper.assetRemotePath => GlobalSetting.Instance.assetRemotePath;
        string IPathHelper.streamingAssetsPath => Application.streamingAssetsPath;
        string IPathHelper.persistentDataPath => Application.persistentDataPath;
    }
}