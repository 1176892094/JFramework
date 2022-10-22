using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JYJFramework
{
    public static class PoolManager
    {
        private static readonly Dictionary<string, PoolData> poolDict = new Dictionary<string, PoolData>();
        private static GameObject poolManager;

        public static void PopObject(string path, UnityAction<GameObject> callback)
        {
            string[] strArray = path.Split('/');
            string name = strArray[^1];
            if (poolDict.ContainsKey(name) && poolDict[name].poolList.Count > 0)
            {
                GameObject obj = poolDict[name].Pop();
                if (obj != null)
                {
                    callback(obj);
                }
                else
                {
                    poolDict.Remove(name);
                    ResourceManager.LoadAsync<GameObject>(path, o =>
                    {
                        o.name = name;
                        callback(o);
                    });
                }
            }
            else
            {
                ResourceManager.LoadAsync<GameObject>(path, o =>
                {
                    o.name = name;
                    callback(o);
                });
            }
        }

        public static void PushObject(string name, GameObject obj)
        {
            if (poolManager == null)
            {
                poolManager = new GameObject("PoolManager");
            }
            
            if (poolDict.ContainsKey(name))
            {
                if (obj == null)
                {
                    Debug.LogWarning(name + "已被销毁,无法推入对象！");
                    poolDict[name].Pop();
                    return;
                }

                poolDict[name].Push(obj);
            }
            else
            {
                poolDict.Add(name, new PoolData(obj, poolManager));
            }
        }
    }

    public class PoolData
    {
        private readonly GameObject prefab;
        public readonly Stack<GameObject> poolList;

        public PoolData(GameObject prefab, GameObject poolList)
        {
            this.prefab = new GameObject(prefab.name + "Group");
            this.prefab.transform.SetParent(poolList.transform);
            this.poolList = new Stack<GameObject>();
            Push(prefab);
        }

        public GameObject Pop()
        {
            GameObject obj = poolList.Pop();
            if (obj == null) return null;
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }

        public void Push(GameObject obj)
        {
            poolList.Push(obj);
            if (prefab != null) obj.transform.SetParent(prefab.transform);
            obj.SetActive(false);
        }
    }
}