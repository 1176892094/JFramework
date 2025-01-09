// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-06-06  05:06
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Net;
using Mono.Cecil;
using UnityEditor;
using UnityEngine;

namespace JFramework.Editor
{
    internal class Models
    {
        /// <summary>
        /// 注入的指定程序集
        /// </summary>
        private readonly AssemblyDefinition assembly;

        /// <summary>
        /// 当网络变量值改变时调用的方法
        /// </summary>
        public readonly MethodReference HookMethodRef;

        /// <summary>
        /// 网络行为被标记改变
        /// </summary>
        public readonly MethodReference NetworkBehaviourDirtyRef;

        /// <summary>
        /// Rpc委托的构造函数
        /// </summary>
        public readonly MethodReference RpcDelegateRef;

        /// <summary>
        /// 日志出现错误
        /// </summary>
        public readonly MethodReference logErrorRef;

        /// <summary>
        /// 获取NetworkClient.isActive
        /// </summary>
        public readonly MethodReference NetworkClientActiveRef;

        /// <summary>
        /// 获取NetworkServer.isActive
        /// </summary>
        public readonly MethodReference NetworkServerActiveRef;

        /// <summary>
        /// 对ArraySegment的构造函数的注入
        /// </summary>
        public readonly MethodReference ArraySegmentRef;

        /// <summary>
        /// 创建SO方法
        /// </summary>
        public readonly MethodReference CreateInstanceMethodRef;

        /// <summary>
        /// 读取泛型的 NetworkBehaviour
        /// </summary>
        public readonly MethodReference ReadNetworkBehaviourGeneric;

        /// <summary>
        /// NetworkBehaviour.SendServerRpcInternal
        /// </summary>
        public readonly MethodReference sendServerRpcInternal;

        /// <summary>
        /// NetworkBehaviour.SendTargetRpcInternal
        /// </summary>
        public readonly MethodReference sendTargetRpcInternal;

        /// <summary>
        /// NetworkBehaviour.SendClientRpcInternal
        /// </summary>
        public readonly MethodReference sendClientRpcInternal;

        /// <summary>
        /// NetworkBehaviour.SyncVarSetterGeneral
        /// </summary>
        public readonly MethodReference syncVarSetterGeneral;

        /// <summary>
        /// NetworkBehaviour.SyncVarSetterGameObject
        /// </summary>
        public readonly MethodReference syncVarSetterGameObject;

        /// <summary>
        /// NetworkBehaviour.SyncVarSetterNetworkObject
        /// </summary>
        public readonly MethodReference syncVarSetterNetworkObject;

        /// <summary>
        /// NetworkBehaviour.SyncVarSetterNetworkBehaviour
        /// </summary>
        public readonly MethodReference syncVarSetterNetworkBehaviour;

        /// <summary>
        /// NetworkBehaviour.SyncVarGetterGeneral
        /// </summary>
        public readonly MethodReference syncVarGetterGeneral;

        /// <summary>
        /// NetworkBehaviour.SyncVarGetterGameObject
        /// </summary>
        public readonly MethodReference syncVarGetterGameObject;

        /// <summary>
        /// NetworkBehaviour.SyncVarGetterNetworkObject
        /// </summary>
        public readonly MethodReference syncVarGetterNetworkObject;

        /// <summary>
        /// NetworkBehaviour.SyncVarGetterNetworkBehaviour
        /// </summary>
        public readonly MethodReference syncVarGetterNetworkBehaviour;

        /// <summary>
        /// NetworkBehaviour.GetSyncVarGameObject
        /// </summary>
        public readonly MethodReference getSyncVarGameObject;

        /// <summary>
        /// NetworkBehaviour.GetSyncVarNetworkObject
        /// </summary>
        public readonly MethodReference getSyncVarNetworkObject;

        /// <summary>
        /// NetworkBehaviour.GetSyncVarNetworkBehaviour
        /// </summary>
        public readonly MethodReference getSyncVarNetworkBehaviour;

        /// <summary>
        /// NetworkUtilsRpc.RegisterServerRpc
        /// </summary>
        public readonly MethodReference registerServerRpcRef;

        /// <summary>
        /// NetworkUtilsRpc.RegisterClientRpc
        /// </summary>
        public readonly MethodReference registerClientRpcRef;

        /// <summary>
        /// NetworkWriter.Pop
        /// </summary>
        public readonly MethodReference PopWriterRef;

        /// <summary>
        /// NetworkWriter.Push
        /// </summary>
        public readonly MethodReference PushWriterRef;

        /// <summary>
        /// Type.GetTypeFromHandle
        /// </summary>
        public readonly MethodReference getTypeFromHandleRef;

        /// <summary>
        /// InitializeOnLoadMethodAttribute
        /// </summary>
        public readonly TypeDefinition InitializeOnLoadMethodAttribute;

        /// <summary>
        /// RuntimeInitializeOnLoadMethodAttribute
        /// </summary>
        public readonly TypeDefinition RuntimeInitializeOnLoadMethodAttribute;

        /// <summary>
        /// 导入类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public TypeReference Import<T>() => Import(typeof(T));

        /// <summary>
        /// 导入类型反射器
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public TypeReference Import(Type t) => assembly.MainModule.ImportReference(t);

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="logger"></param>
        /// <param name="failed"></param>
        public Models(AssemblyDefinition assembly, Logger logger, ref bool failed)
        {
            this.assembly = assembly;

            var HookMethodType = Import(typeof(Action<,>));
            HookMethodRef = Helper.GetMethod(HookMethodType, assembly, logger, ".ctor", ref failed);

            var ArraySegmentType = Import(typeof(ArraySegment<>));
            ArraySegmentRef = Helper.GetMethod(ArraySegmentType, assembly, logger, Const.CTOR, ref failed);
            
            var NetworkClientType = Import(typeof(NetworkManager.Client));
            NetworkClientActiveRef = Helper.GetMethod(NetworkClientType, assembly, logger, "get_isActive", ref failed);
            var NetworkServerType = Import(typeof(NetworkManager.Server));
            NetworkServerActiveRef = Helper.GetMethod(NetworkServerType, assembly, logger, "get_isActive", ref failed);

            var StreamExtensionType = Import(typeof(Net.Extensions));
            ReadNetworkBehaviourGeneric = Helper.GetMethod(StreamExtensionType, assembly, logger, method => method.Name == nameof(Net.Extensions.ReadNetworkBehaviour) && method.HasGenericParameters, ref failed);

            var NetworkBehaviourType = Import<NetworkBehaviour>();
            NetworkBehaviourDirtyRef = Helper.ResolveProperty(NetworkBehaviourType, assembly, "syncVarDirty");

            syncVarSetterGeneral = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SyncVarSetterGeneral", ref failed);
            syncVarSetterGameObject = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SyncVarSetterGameObject", ref failed);
            syncVarSetterNetworkObject = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SyncVarSetterNetworkObject", ref failed);
            syncVarSetterNetworkBehaviour = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SyncVarSetterNetworkBehaviour", ref failed);

            syncVarGetterGeneral = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SyncVarGetterGeneral", ref failed);
            syncVarGetterGameObject = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SyncVarGetterGameObject", ref failed);
            syncVarGetterNetworkObject = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SyncVarGetterNetworkObject", ref failed);
            syncVarGetterNetworkBehaviour = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SyncVarGetterNetworkBehaviour", ref failed);

            getSyncVarGameObject = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "GetSyncVarGameObject", ref failed);
            getSyncVarNetworkObject = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "GetSyncVarNetworkObject", ref failed);
            getSyncVarNetworkBehaviour = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "GetSyncVarNetworkBehaviour", ref failed);

            sendServerRpcInternal = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SendServerRpcInternal", ref failed);
            sendClientRpcInternal = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SendClientRpcInternal", ref failed);
            sendTargetRpcInternal = Helper.GetMethod(NetworkBehaviourType, assembly, logger, "SendTargetRpcInternal", ref failed);

            var InvokeType = Import(typeof(NetworkDelegate));
            registerServerRpcRef = Helper.GetMethod(InvokeType, assembly, logger, "RegisterServerRpc", ref failed);
            registerClientRpcRef = Helper.GetMethod(InvokeType, assembly, logger, "RegisterClientRpc", ref failed);

            var RpcDelegateType = Import<InvokeDelegate>();
            RpcDelegateRef = Helper.GetMethod(RpcDelegateType, assembly, logger, ".ctor", ref failed);

            var ScriptableObjectType = Import<ScriptableObject>();
            CreateInstanceMethodRef = Helper.GetMethod(ScriptableObjectType, assembly, logger, method => method.Name == "CreateInstance" && method.HasGenericParameters, ref failed);

            var DebugType = Import(typeof(Debug));
            logErrorRef = Helper.GetMethod(DebugType, assembly, logger, method => method.Name == "LogError" && method.Parameters.Count == 1 && method.Parameters[0].ParameterType.FullName == typeof(object).FullName, ref failed);

            var Type = Import(typeof(Type));
            getTypeFromHandleRef = Helper.GetMethod(Type, assembly, logger, "GetTypeFromHandle", ref failed);

            var NetworkWriterType = Import(typeof(MemoryWriter));
            PopWriterRef = Helper.GetMethod(NetworkWriterType, assembly, logger, "Pop", ref failed);
            PushWriterRef = Helper.GetMethod(NetworkWriterType, assembly, logger, "Push", ref failed);

            if (Helper.IsEditorAssembly(assembly))
            {
                var InitializeOnLoadMethodAttributeType = Import(typeof(InitializeOnLoadMethodAttribute));
                InitializeOnLoadMethodAttribute = InitializeOnLoadMethodAttributeType.Resolve();
            }

            var RuntimeInitializeOnLoadMethodAttributeType = Import(typeof(RuntimeInitializeOnLoadMethodAttribute));
            RuntimeInitializeOnLoadMethodAttribute = RuntimeInitializeOnLoadMethodAttributeType.Resolve();
        }
    }
}