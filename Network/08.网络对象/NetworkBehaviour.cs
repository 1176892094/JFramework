// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-21 23:12:50
// # Recently: 2024-12-22 22:12:01
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************


using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace JFramework.Net
{
    public abstract partial class NetworkBehaviour : MonoBehaviour
    {
        internal byte componentId;
        
        public SyncMode syncDirection;

        public float syncInterval;
        
        private ulong syncVarHook;

        private double syncVarTime;
        
        protected ulong syncVarDirty { get; set; }

        public NetworkObject @object { get; internal set; }

        public uint objectId => @object.objectId;

        public bool isOwner => (@object.entityMode & EntityMode.Owner) != 0;

        public bool isServer => (@object.entityMode & EntityMode.Server) != 0;

        public bool isClient => (@object.entityMode & EntityMode.Client) != 0;

        public bool isVerify
        {
            get
            {
                if (isClient && isServer)
                {
                    return syncDirection == SyncMode.Server || isOwner;
                }

                if (isClient)
                {
                    return syncDirection == SyncMode.Client && isOwner;
                }

                return syncDirection == SyncMode.Server;
            }
        }

        public NetworkClient connection => @object.connection;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsDirty() => syncVarDirty != 0UL && Time.unscaledTimeAsDouble - syncVarTime >= syncInterval;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SetSyncVarDirty(ulong dirty) => syncVarDirty |= dirty;

        private bool GetSyncVarHook(ulong dirty) => (syncVarHook & dirty) != 0UL;

        private void SetSyncVarHook(ulong dirty, bool value) => syncVarHook = value ? syncVarHook | dirty : syncVarHook & ~dirty;

        public void ClearDirty()
        {
            syncVarDirty = 0UL;
            syncVarTime = Time.unscaledTimeAsDouble;
        }

        internal void Serialize(MemoryWriter writer, bool status)
        {
            var headerPosition = writer.position;
            writer.WriteByte(0);
            var contentPosition = writer.position;

            try
            {
                OnSerialize(writer, status);
            }
            catch (Exception e)
            {
                Debug.LogError(Service.Text.Format("序列化对象失败。对象名称: {0}[{1}][{2}]\n{3}", name, GetType(), @object.sceneId, e));
            }

            var endPosition = writer.position;
            writer.position = headerPosition;
            var size = endPosition - contentPosition;
            var safety = (byte)(size & 0xFF);
            writer.WriteByte(safety);
            writer.position = endPosition;
        }

        internal bool Deserialize(MemoryReader reader, bool status)
        {
            var result = true;
            var safety = reader.ReadByte();
            var chunkStart = reader.position;
            try
            {
                OnDeserialize(reader, status);
            }
            catch (Exception e)
            {
                Debug.LogError(Service.Text.Format("反序列化对象失败。对象名称: {0}[{1}][{2}]\n{3}", name, GetType(), @object.sceneId, e));
                result = false;
            }

            var size = reader.position - chunkStart;
            var sizeHash = (byte)(size & 0xFF);
            if (sizeHash != safety)
            {
                Debug.LogError(Service.Text.Format("反序列化字节不匹配。读取字节: {0} 哈希对比:{1}/{2}", size, sizeHash, safety));
                var cleared = (uint)size & 0xFFFFFF00;
                reader.position = chunkStart + (int)(cleared | safety);
                result = false;
            }

            return result;
        }

        protected virtual void OnSerialize(MemoryWriter writer, bool status)
        {
            SerializeSyncVars(writer, status);
        }

        protected virtual void OnDeserialize(MemoryReader reader, bool status)
        {
            DeserializeSyncVars(reader, status);
        }

        protected virtual void SerializeSyncVars(MemoryWriter writer, bool status)
        {
        }

        protected virtual void DeserializeSyncVars(MemoryReader reader, bool status)
        {
        }
    }
}