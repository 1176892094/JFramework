using System;
using System.Collections;
using JFramework.Async;
using JFramework.Basic;
using UnityEngine;
using Logger = JFramework.Basic.Logger;
using Object = UnityEngine.Object;

namespace JFramework
{
    public static class ResourceManager
    {
        public static T Load<T>(string name) where T : Object
        {
            T resource = Resources.Load<T>(name);
            return resource is GameObject ? Object.Instantiate(resource) : resource;
        }

        public static async void LoadAsync<T>(string name, Action<T> callback) where T : Object
        {
            await LoadCompleted(name, callback);
        }

        private static IEnumerator LoadCompleted<T>(string name, Action<T> callback) where T : Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(name);
            yield return request;
            if (request == null) yield break;
            if (request.asset == null)
            {
                Logger.LogWarning(name + "未获取到！");
                yield break;
            }

            if (request.asset is GameObject)
            {
                callback((T)Object.Instantiate(request.asset));
            }
            else
            {
                callback((T)request.asset);
            }
        }
    }
}