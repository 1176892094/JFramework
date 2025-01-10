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

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace JFramework
{
    internal static partial class EditorSetting
    {
        [MenuItem("Tools/JFramework/构建 AB 资源", priority = 3)]
        private static async void BuildAsset()
        {
            UpdateAsset();
            var folderPath = Directory.CreateDirectory(GlobalSetting.remoteAssetPath);
            var packTarget = (BuildTarget)GlobalSetting.Instance.assetPlatform;
            BuildPipeline.BuildAssetBundles(GlobalSetting.remoteAssetPath, BuildAssetBundleOptions.None, packTarget);
            var elapseTime = EditorApplication.timeSinceStartup;

            var fileHash = new HashSet<string>();
            var isExists = File.Exists(GlobalSetting.remoteAssetPack);
            if (isExists)
            {
                var readJson = await File.ReadAllTextAsync(GlobalSetting.remoteAssetPack);
                fileHash = Service.Json.FromJson<List<PackData>>(readJson).Select(data => data.code).ToHashSet();
            }

            var filePacks = new List<PackData>();
            var fileInfos = folderPath.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                if (fileInfo.Extension != "")
                {
                    continue;
                }

                if (isExists && fileHash.Contains(GetHashCode(fileInfo.FullName)))
                {
                    filePacks.Add(new PackData(GetHashCode(fileInfo.FullName), fileInfo.Name, (int)fileInfo.Length));
                    continue;
                }

                await Task.Run(() =>
                {
                    var readBytes = File.ReadAllBytes(fileInfo.FullName);
                    readBytes = Service.Xor.Encrypt(readBytes);
                    File.WriteAllBytes(fileInfo.FullName, readBytes);
                });
                filePacks.Add(new PackData(GetHashCode(fileInfo.FullName), fileInfo.Name, (int)fileInfo.Length));
                Debug.Log(Service.Text.Format("加密AB包: {0}", fileInfo.FullName));
            }
            
            var saveJson = Service.Json.ToJson(filePacks);
            await File.WriteAllTextAsync(GlobalSetting.remoteAssetPack, saveJson);
            elapseTime = EditorApplication.timeSinceStartup - elapseTime;
            Debug.Log(Service.Text.Format("加密 AssetBundle 完成。耗时:<color=#00FF00> {0:F} </color>秒", elapseTime));
            AssetDatabase.Refresh();
        }

        private static string GetHashCode(string filePath)
        {
            using var provider = MD5.Create();
            using var fileStream = File.OpenRead(filePath);
            var computeHash = provider.ComputeHash(fileStream);
            var builder = new StringBuilder();
            foreach (var buffer in computeHash)
            {
                builder.Append(buffer.ToString("x2"));
            }

            return builder.ToString();
        }

        [Serializable]
        private struct PackData : IEquatable<PackData>
        {
            public string code;
            public string name;
            public int size;

            public PackData(string code, string name, int size)
            {
                this.code = code;
                this.name = name;
                this.size = size;
            }

            public static bool operator ==(PackData a, PackData b) => a.code == b.code;

            public static bool operator !=(PackData a, PackData b) => a.code != b.code;

            public bool Equals(PackData other)
            {
                return size == other.size && code == other.code && name == other.name;
            }

            public override bool Equals(object obj)
            {
                return obj is PackData other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (code != null ? code.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (name != null ? name.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ size;
                    return hashCode;
                }
            }
        }
    }
}
#endif