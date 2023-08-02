using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JFramework.Core;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JFramework
{
    public static class AssetEditor
    {
#if UNITY_EDITOR
        /// <summary>
        /// AssetBundle 标签导入
        /// </summary>
        private static AssetImporter importer;

        /// <summary>
        /// 更新 AssetBundles 标签
        /// </summary>
        [MenuItem("Tools/JFramework/Update AssetBundles", priority = 1)]
        public static void Update()
        {
            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();

            foreach (string assetBundleName in assetBundleNames)
            {
                if (AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName).Length == 0)
                {
                    AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
                }
            }

            if (!Directory.Exists(AssetSetting.FILE_PATH))
            {
                Directory.CreateDirectory(AssetSetting.FILE_PATH);
            }

            string[] guids = AssetDatabase.FindAssets("t:Object", new[] { AssetSetting.FILE_PATH });
            var enumerable = guids.Select(AssetDatabase.GUIDToAssetPath).Where(asset => !AssetDatabase.IsValidFolder(asset));
            foreach (var path in enumerable)
            {
                var array = path.Replace('\\', '/').Split('/');
                importer = AssetImporter.GetAtPath(path);
                if (importer != null)
                {
                    var assetBundleName = importer.assetBundleName;
                    if (assetBundleName != array[2].ToLower())
                    {
                        Debug.Log($"增加 AssetBundles 资源: {path.Green()}");
                        importer.assetBundleName = array[2];
                        importer.SaveAndReimport();
                    }
                }
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 构建 AssetBundles
        /// </summary>
        [MenuItem("Tools/JFramework/Build AssetBundles", priority = 2)]
        public static void Build()
        {
            if (!Directory.Exists(AssetSetting.SAVE_PATH))
            {
                Directory.CreateDirectory(AssetSetting.SAVE_PATH);
            }

            if (!Directory.Exists(AssetSetting.localSavePath))
            {
                Directory.CreateDirectory(AssetSetting.localSavePath);
            }

            BuildPipeline.BuildAssetBundles(AssetSetting.localSavePath, BuildAssetBundleOptions.ChunkBasedCompression, AssetSetting.Target);
            var directory = Directory.CreateDirectory(AssetSetting.localSavePath);
            var fileInfos = directory.GetFiles();
            var fileList = new List<AssetData>();
            foreach (var info in fileInfos)
            {
                if (info.Extension == "")
                {
                    fileList.Add(new AssetData(info.Name, info.Length.ToString(), GetMD5(info.FullName)));
                }
            }

            var saveJson = JsonConvert.SerializeObject(fileList);
            File.WriteAllText(AssetSetting.localInfoPath, saveJson);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 获取MD5码
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static string GetMD5(string filePath)
        {
            using var file = new FileStream(filePath, FileMode.Open);
            var md5 = new MD5CryptoServiceProvider();
            var md5Info = md5.ComputeHash(file);
            var builder = PoolManager.Pop<StringBuilder>();
            foreach (var info in md5Info)
            {
                builder.Append(info.ToString("X2"));
            }

            var result = builder.ToString();
            PoolManager.Push(builder);
            builder.Clear();
            return result;
        }

        /// <summary>
        /// 上传 AssetBundle 到服务器
        /// </summary>
        [MenuItem("Tools/JFramework/Upload AssetBundles", priority = 3)]
        public static async void Upload()
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory(AssetSetting.localSavePath);
            var fileInfos = directoryInfo.GetFiles();
            foreach (var info in fileInfos)
            {
                if (info.Extension is "" or ".txt")
                {
                    await Upload(info.FullName, info.Name);
                }
            }
        }

        /// <summary>
        /// 上传指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        private static async Task Upload(string filePath, string fileName)
        {
            var request = new UnityWebRequest(AssetSetting.GetRemoteFile(fileName), "POST");
            var fileData = await File.ReadAllBytesAsync(filePath);
            request.uploadHandler = new UploadHandlerRaw(fileData);
            request.SetRequestHeader("Content-Type", "application/octet-stream");
            var operation = request.SendWebRequest();
            if (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("文件上传失败： " + request.error);
            }
            else
            {
                Debug.Log("文件上传成功");
            }
        }

        /// <summary>
        /// 当前项目路径
        /// </summary>
        [MenuItem("Tools/JFramework/CurrentProjectPath", priority = 11)]
        private static void CurrentProjectPath() => Process.Start(Application.dataPath);

        /// <summary>
        /// 持久化路径
        /// </summary>
        [MenuItem("Tools/JFramework/PersistentDataPath", priority = 12)]
        private static void PersistentDataPath() => Process.Start(Application.persistentDataPath);

        /// <summary>
        /// 资源流路径
        /// </summary>
        [MenuItem("Tools/JFramework/StreamingAssetsPath", priority = 13)]
        private static void StreamingAssetsPath()
        {
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Process.Start(Application.streamingAssetsPath);
            }
            else
            {
                Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
                Process.Start(Application.streamingAssetsPath);
            }
        }
#endif
    }
}