// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-08-25  03:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework.Core
{
    public static partial class AssetManager
    {
        [Serializable]
        private struct AssetData
        {
            public string asset;
            public string bundle;

            public AssetData(string path)
            {
                var index = path.LastIndexOf('/');
                if (index < 0)
                {
                    bundle = "";
                    asset = path;
                    return;
                }

                bundle = path.Substring(0, index).ToLower();
                asset = path.Substring(index + 1);
            }

            public override string ToString()
            {
                return $"资源包：{bundle} 资源名称：{asset}";
            }
        }
    }
}