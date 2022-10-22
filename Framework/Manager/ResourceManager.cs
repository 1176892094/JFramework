using System;
using System.Collections;
using JYJFramework.Async;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JYJFramework
{
    public static class ResourceManager
    {
        public static T Load<T>(string name) where T : Object
        {
            T resource = Resources.Load<T>(name);
            if (resource is GameObject)
            {
                return Object.Instantiate(resource);
            }

            return resource;
        }

        public static async void LoadAsync<T>(string name, Action<T> callback) where T : Object
        {
            await LoadCompleted(name, callback);
        }

        private static IEnumerator LoadCompleted<T>(string name, Action<T> callback) where T : Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(name);
            yield return request;
            if (request.asset is GameObject)
            {
                T obj = Object.Instantiate(request.asset) as T;
                if (obj == null)
                {
                    Debug.LogWarning(name + "未获取到！");
                    yield return null;
                }
                
                callback(obj);
            }
            else
            {
                T obj = request.asset as T;
                if (obj == null)
                {
                    Debug.LogWarning(name + "未获取到！");
                    yield return null;
                }

                callback(obj);
            }
        }
    }
}