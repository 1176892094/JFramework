// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  16:14
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace JFramework.Core
{
    public sealed class RequestManager : ScriptableObject
    {
        public event Action<int> OnLoadStart;
        public event Action<string, float> OnLoadUpdate;
        public event Action OnLoadComplete;
        private readonly List<string> updateAssets = new List<string>();
        [ShowInInspector, LabelText("本地资源")] private Dictionary<string, Bundle> localAssets = new Dictionary<string, Bundle>();
        [ShowInInspector, LabelText("远端资源")] private Dictionary<string, Bundle> remoteAssets = new Dictionary<string, Bundle>();

        public async Task<bool> UpdateAssetBundles()
        {
            if (!GlobalManager.Instance) return false;
            if (await GetRemoteData())
            {
                Debug.Log("解析远端对比文件完成");
                var remoteInfo = await File.ReadAllTextAsync(SettingManager.remoteInfoPath);
                var jsonData = JsonUtility.FromJson<Variables<Bundle>>(remoteInfo);
                remoteAssets = jsonData.value.ToDictionary(data => data.name);

                var clientInfo = await GetLocalData();
                if (clientInfo != null)
                {
                    jsonData = JsonUtility.FromJson<Variables<Bundle>>(clientInfo);
                    localAssets = jsonData.value.ToDictionary(data => data.name);
                }

                Debug.Log("解析本地对比文件完成");
                foreach (var fileName in remoteAssets.Keys)
                {
                    if (!localAssets.ContainsKey(fileName))
                    {
                        updateAssets.Add(fileName);
                    }
                    else
                    {
                        if (localAssets[fileName] != remoteAssets[fileName])
                        {
                            updateAssets.Add(fileName);
                        }

                        localAssets.Remove(fileName);
                    }
                }

                Debug.Log("删除弃用的资源文件");
                foreach (var path in localAssets.Keys.Select(SettingManager.GetPersistentPath).Where(File.Exists))
                {
                    File.Delete(path);
                }

                if (await GetAssetBundles())
                {
                    await File.WriteAllTextAsync(SettingManager.clientInfoPath, remoteInfo);
                    OnLoadComplete?.Invoke();
                    return true;
                }
            }

            Debug.Log("更新失败。");
            return false;
        }

        private static async Task<bool> GetRemoteData()
        {
            var reloads = 5;
            var success = false;
            while (!success && reloads-- > 0)
            {
                var fileUri = SettingManager.GetRemoteFilePath(SettingManager.clientInfoName);
                using (var request = UnityWebRequest.Head(fileUri))
                {
                    request.timeout = 1;
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log($"校验 {SettingManager.Instance.assetInfo} 失败\n");
                        continue;
                    }
                }

                using (var request = UnityWebRequest.Get(fileUri))
                {
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log($"下载 {SettingManager.Instance.assetInfo} 失败\n");
                        continue;
                    }

                    var contents = request.downloadHandler.text;
                    await File.WriteAllTextAsync(SettingManager.remoteInfoPath, contents);
                }

                success = true;
            }

            return success;
        }

        private static async Task<string> GetLocalData()
        {
            if (File.Exists(SettingManager.clientInfoPath))
            {
                return await File.ReadAllTextAsync(SettingManager.clientInfoPath);
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            using var request = UnityWebRequest.Get(SettingManager.streamingInfoPath);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
#else
            if (File.Exists(SettingManager.streamingInfoPath))
            {
                return await File.ReadAllTextAsync(SettingManager.streamingInfoPath);
            }
#endif
            return null;
        }

        private async Task<bool> GetAssetBundles()
        {
            var reloads = 5;
            var copyList = updateAssets.ToList();
            OnLoadStart?.Invoke(updateAssets.Count);
            while (updateAssets.Count > 0 && reloads-- > 0)
            {
                foreach (var fileName in copyList)
                {
                    var fileUri = SettingManager.GetRemoteFilePath(fileName);
                    using (var request = UnityWebRequest.Get(fileUri))
                    {
                        var result = request.SendWebRequest();
                        while (!result.isDone && GlobalManager.Instance)
                        {
                            OnLoadUpdate?.Invoke(fileName, request.downloadProgress);
                            await Task.Yield();
                        }

                        OnLoadUpdate?.Invoke(fileName, 1);
                        if (request.result != UnityWebRequest.Result.Success)
                        {
                            Debug.Log($"下载 {fileName} 文件失败\n");
                            continue;
                        }

                        var contents = request.downloadHandler.data;
                        await File.WriteAllBytesAsync(SettingManager.GetPersistentPath(fileName), contents);
                    }

                    if (updateAssets.Contains(fileName))
                    {
                        updateAssets.Remove(fileName);
                    }
                }
            }

            return updateAssets.Count == 0;
        }

        internal void OnDisable()
        {
            OnLoadStart = null;
            OnLoadUpdate = null;
            OnLoadComplete = null;
            localAssets.Clear();
            remoteAssets.Clear();
            updateAssets.Clear();
        }
    }
}