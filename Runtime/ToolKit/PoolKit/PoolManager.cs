// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:57
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework
{
    public sealed partial class GlobalManager
    {
        /// <summary>
        /// 对象池管理器
        /// </summary>
        public sealed class PoolManager : Controller
        {
            /// <summary>
            /// 对象池组
            /// </summary>
            [ShowInInspector] private readonly Dictionary<string, GameObject> parents = new Dictionary<string, GameObject>();

            /// <summary>
            /// 对象池容器
            /// </summary>
            [ShowInInspector] private readonly Dictionary<string, IPool<GameObject>> pools = new Dictionary<string, IPool<GameObject>>();

            /// <summary>
            /// 对象池管理器
            /// </summary>
            private Transform poolManager;

            /// <summary>
            /// 获取 PoolManager 对象
            /// </summary>
            private void Awake()
            {
                poolManager = owner.transform.Find("PoolManager");
            }

            /// <summary>
            /// 对象池管理器异步获取对象
            /// </summary>
            /// <param name="path">弹出对象的路径</param>
            public async Task<GameObject> Pop(string path)
            {
                if (!Runtime) return default;
                if (pools.TryGetValue(path, out var pool) && pool.Count > 0)
                {
                    var obj = pool.Pop();
                    if (obj != null)
                    {
                        obj.SetActive(true);
                        obj.transform.SetParent(null);
                        return obj;
                    }
                }

                var o = await Asset.Load<GameObject>(path);
                DontDestroyOnLoad(o);
                o.name = path;
                return o;
            }

            /// <summary>
            /// 对象池管理器异步获取对象
            /// </summary>
            /// <param name="path">弹出对象的路径</param>
            public async Task<T> Pop<T>(string path)
            {
                if (!Runtime) return default;
                if (pools.TryGetValue(path, out var pool) && pool.Count > 0)
                {
                    var obj = pool.Pop();
                    if (obj != null)
                    {
                        obj.SetActive(true);
                        obj.transform.SetParent(null);
                        return obj.GetComponent<T>();
                    }
                }

                var o = await Asset.Load<GameObject>(path);
                DontDestroyOnLoad(o);
                o.name = path;
                return o.GetComponent<T>();
            }

            /// <summary>
            /// 对象池管理器异步获取对象
            /// </summary>
            /// <param name="path">弹出对象的路径</param>
            /// <param name="action"></param>
            public void PopAsync<T>(string path, Action<T> action)
            {
                if (!Runtime) return;
                if (pools.TryGetValue(path, out var pool) && pool.Count > 0)
                {
                    var obj = pool.Pop();
                    if (obj != null)
                    {
                        obj.SetActive(true);
                        obj.transform.SetParent(null);
                        action?.Invoke(obj.GetComponent<T>());
                        return;
                    }
                }

                Asset.LoadAsync<GameObject>(path, obj =>
                {
                    obj.name = path;
                    DontDestroyOnLoad(obj);
                    action?.Invoke(obj.GetComponent<T>());
                });
            }

            /// <summary>
            /// 对象池管理器异步获取对象
            /// </summary>
            /// <param name="path">弹出对象的路径</param>
            /// <param name="action"></param>
            public void PopAsync(string path, Action<GameObject> action)
            {
                if (!Runtime) return;
                if (pools.TryGetValue(path, out var pool) && pool.Count > 0)
                {
                    var obj = pool.Pop();
                    if (obj != null)
                    {
                        obj.SetActive(true);
                        obj.transform.SetParent(null);
                        action?.Invoke(obj);
                        return;
                    }
                }

                Asset.LoadAsync<GameObject>(path, obj =>
                {
                    obj.name = path;
                    DontDestroyOnLoad(obj);
                    action?.Invoke(obj);
                });
            }

            /// <summary>
            /// 对象池管理器推入对象
            /// </summary>
            /// <param name="obj">对象的实例</param>
            public void Push(GameObject obj)
            {
                if (!Runtime) return;
                if (obj == null) return;
                if (!parents.TryGetValue(obj.name, out var parent))
                {
                    parent = new GameObject(obj.name + "-Pool");
                    parent.transform.SetParent(poolManager);
                    pools.Add(obj.name, new Pool<GameObject>(obj));
                    obj.transform.SetParent(parent.transform);
                    parents.Add(obj.name, parent);
                    obj.SetActive(false);
                    return;
                }

                if (pools.TryGetValue(obj.name, out var pool))
                {
                    if (!pool.Push(obj)) return;
                    obj.transform.SetParent(parent.transform);
                    obj.SetActive(false);
                }
            }

            /// <summary>
            /// 控制器销毁
            /// </summary>
            private void OnDestroy()
            {
                foreach (var pool in pools.Values)
                {
                    pool.Dispose();
                }

                pools.Clear();
                parents.Clear();
            }
        }
    }
}