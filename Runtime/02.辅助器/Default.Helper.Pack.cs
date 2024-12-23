// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:35
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace JFramework
{
    internal sealed partial class DefaultHelper : IPackHelper
    {
        async Task<string> IPackHelper.LoadServerRequest(string packName, string packUri)
        {
            for (var i = 0; i < 5; i++)
            {
                using (var request = UnityWebRequest.Head(packUri))
                {
                    request.timeout = 1;
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        continue;
                    }
                }

                using (var request = UnityWebRequest.Get(packUri))
                {
                    await request.SendWebRequest();
                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(Service.Text.Format("请求服务器下载 {0} 失败!\n", packName));
                        continue;
                    }

                    return request.downloadHandler.text;
                }
            }

            return null;
        }

        async Task<byte[]> IPackHelper.LoadPacketRequest(string packName, string packUri)
        {
            using (var request = UnityWebRequest.Head(packUri))
            {
                request.timeout = 1;
                await request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(Service.Text.Format("请求服务器校验 {0} 失败!\n", packName));
                    return null;
                }
            }

            using (var request = UnityWebRequest.Get(packUri))
            {
                var result = request.SendWebRequest();
                while (!result.isDone && GlobalManager.Instance)
                {
                    Service.Event.Invoke(new PackUpdateEvent(packName, request.downloadProgress));
                    await Task.Yield();
                }

                Service.Event.Invoke(new PackUpdateEvent(packName, 1));
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(Service.Text.Format("请求服务器下载 {0} 失败!\n", packName));
                    return null;
                }

                return request.downloadHandler.data;
            }
        }

        async Task<string> IPackHelper.LoadClientRequest(string persistentData, string streamingAssets)
        {
            var packData = await LoadRequest(persistentData, streamingAssets);
            string result = default;
            if (packData.Key == 1)
            {
                result = await File.ReadAllTextAsync(packData.Value);
            }
            else if (packData.Key == 2)
            {
                using var request = UnityWebRequest.Get(packData.Value);
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    result = request.downloadHandler.text;
                }
            }

            return result;
        }

        async Task<object> IPackHelper.LoadAssetRequest(string persistentData, string streamingAssets)
        {
            var packData = await LoadRequest(persistentData, streamingAssets);
            byte[] result = default;
            if (packData.Key == 1)
            {
                result = await Task.Run(() => Service.Xor.Decrypt(File.ReadAllBytes(packData.Value)));
            }
            else if (packData.Key == 2)
            {
                using var request = UnityWebRequest.Get(packData.Value);
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    result = await Task.Run(() => Service.Xor.Decrypt(request.downloadHandler.data));
                }
            }

            return GlobalManager.Instance ? AssetBundle.LoadFromMemory(result) : null;
        }

        private static async Task<KeyValuePair<int, string>> LoadRequest(string persistentData, string streamingAssets)
        {
            if (File.Exists(persistentData))
            {
                return new KeyValuePair<int, string>(1, persistentData);
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            using var request = UnityWebRequest.Head(streamingAssets);
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                return new KeyValuePair<int, string>(2, streamingAssets);
            }

#else
            if (File.Exists(streamingAssets))
            {
                return new KeyValuePair<int, string>(1, streamingAssets);
            }

#endif
            await Task.CompletedTask;
            return new KeyValuePair<int, string>(0, string.Empty);
        }
    }
}