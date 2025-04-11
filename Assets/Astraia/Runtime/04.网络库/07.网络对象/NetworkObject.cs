// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:50
// # Recently: 2024-12-22 23:12:53
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JFramework.Common;
using UnityEngine;

namespace JFramework.Net
{
    public sealed partial class NetworkObject : MonoBehaviour
    {
        [SerializeField] internal EntityMode entityMode;
        
        [SerializeField] internal uint objectId;
        
        [SerializeField] internal ulong sceneId;
        
        [SerializeField] internal string assetId;

        private int frameCount;

        internal EntityState entityState;

        internal NetworkClient connection;

        internal NetworkBehaviour[] entities;

        internal MemorySetter owner = new MemorySetter();

        internal MemorySetter observer = new MemorySetter();

        private void Awake()
        {
            entities = GetComponentsInChildren<NetworkBehaviour>(true);
            if (entities == null)
            {
                Debug.LogError("网络对象持有的 NetworkEntity 为空", gameObject);
                return;
            }

            if (entities.Length > 64)
            {
                Debug.LogError("网络对象持有的 NetworkBehaviour 的数量不能超过 64");
                return;
            }

            for (byte i = 0; i < entities.Length; ++i)
            {
                entities[i].@object = this;
                entities[i].componentId = i;
            }
        }

        public void Reset()
        {
            objectId = 0;
            connection = null;
            owner.position = 0;
            observer.position = 0;
            entityMode = EntityMode.None;
            entityState = EntityState.None;
        }

        private void OnValidate()
        {
            var assetType = Service.Find.Type("JFramework.NetworkSetter, Unity.JFramework.CodeGen");
            var assetData = assetType.GetMethod("Validate", Service.Find.Static);
            if (assetData != null)
            {
                var assetPair = ((string, ulong))assetData.Invoke(null, new object[] { assetId, sceneId, gameObject });
                assetId = assetPair.Item1;
                sceneId = assetPair.Item2;
            }
        }

        private void OnDestroy()
        {
            if ((entityMode & EntityMode.Server) == EntityMode.Server && (entityState & EntityState.Destroy) == 0)
            {
                NetworkManager.Server.Despawn(gameObject);
            }

            if ((entityMode & EntityMode.Client) != 0)
            {
                NetworkManager.Client.spawns.Remove(objectId);
            }

            owner = null;
            observer = null;
            entities = null;
            connection = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsDirty(ulong mask, int index)
        {
            return (mask & (ulong)(1 << index)) != 0;
        }

        internal void InvokeMessage(byte index, ushort function, InvokeMode mode, MemoryGetter getter, NetworkClient client = null)
        {
            if (this == null)
            {
                Debug.LogWarning(Service.Text.Format("调用了已经删除的网络对象。{0} [{1}] {2}", mode, function, objectId));
                return;
            }

            if (index >= entities.Length)
            {
                Debug.LogWarning(Service.Text.Format("网络对象{0}，没有找到组件{1}", objectId, index));
                return;
            }

            if (!NetworkAttribute.Invoke(function, mode, client, getter, entities[index]))
            {
                Debug.LogError(Service.Text.Format("无法调用{0} [{1}] 网络对象: {2} 网络标识: {3}", mode, function, gameObject.name, objectId));
            }
        }

        internal void Synchronization(int frame)
        {
            if (frameCount != frame)
            {
                frameCount = frame;
                owner.position = 0;
                observer.position = 0;
                ServerSerialize(false, owner, observer);
                ClearDirty(true);
            }
        }

        internal void ClearDirty(bool total)
        {
            foreach (var entity in entities)
            {
                if (entity.IsDirty() || total)
                {
                    entity.ClearDirty();
                }
            }
        }

        internal void OnStartClient()
        {
            if ((entityState & EntityState.Spawn) != 0)
            {
                return;
            }

            entityState |= EntityState.Spawn;

            foreach (var entity in entities)
            {
                try
                {
                    (entity as IStartClient)?.OnStartClient();
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e, entity.gameObject);
                }
            }
        }

        internal void OnStopClient()
        {
            if ((entityState & EntityState.Spawn) == 0)
            {
                return;
            }

            foreach (var entity in entities)
            {
                try
                {
                    (entity as IStopClient)?.OnStopClient();
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e, entity.gameObject);
                }
            }
        }

        internal void OnStartServer()
        {
            foreach (var entity in entities)
            {
                try
                {
                    (entity as IStartServer)?.OnStartServer();
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e, entity.gameObject);
                }
            }
        }

        internal void OnStopServer()
        {
            foreach (var entity in entities)
            {
                try
                {
                    (entity as IStopServer)?.OnStopServer();
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e, entity.gameObject);
                }
            }
        }

        private void OnStartAuthority()
        {
            foreach (var entity in entities)
            {
                try
                {
                    (entity as IStartAuthority)?.OnStartAuthority();
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e, entity.gameObject);
                }
            }
        }

        private void OnStopAuthority()
        {
            foreach (var entity in entities)
            {
                try
                {
                    (entity as IStopAuthority)?.OnStopAuthority();
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e, entity.gameObject);
                }
            }
        }
        
        internal void OnNotifyAuthority()
        {
            if ((entityState & EntityState.Authority) == 0 && (entityMode & EntityMode.Owner) != 0)
            {
                OnStartAuthority();
            }
            else if ((entityState & EntityState.Authority) != 0 && (entityMode & EntityMode.Owner) == 0)
            {
                OnStopAuthority();
            }

            if ((entityMode & EntityMode.Owner) != 0)
            {
                entityState |= EntityState.Authority;
            }
            else
            {
                entityState &= ~EntityState.Authority;
            }
        }
    }
}