// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-06-06  05:06
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using JFramework.Net;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace JFramework.Editor
{
    internal enum InvokeMode : byte
    {
        ServerRpc,
        ClientRpc,
        TargetRpc,
    }

    internal static class NetworkDelegateProcess
    {
        /// <summary>
        /// ClientRpc方法
        /// </summary>
        /// <param name="models"></param>
        /// <param name="readers"></param>
        /// <param name="logger"></param>
        /// <param name="td"></param>
        /// <param name="md"></param>
        /// <param name="func"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        public static MethodDefinition ProcessClientRpc(Models models, Reader readers, Logger logger, TypeDefinition td, MethodDefinition md, MethodDefinition func, ref bool failed)
        {
            var rpcName = Process.GenerateMethodName(Const.INV_METHOD, md);
            var rpc = new MethodDefinition(rpcName, Const.RPC_ATTRS, models.Import(typeof(void)));
            var worker = rpc.Body.GetILProcessor();
            var label = worker.Create(OpCodes.Nop);
            NetworkClientActive(worker, models, md.Name, label, "ClientRpc");

            worker.Emit(OpCodes.Ldarg_0);
            worker.Emit(OpCodes.Castclass, td);

            if (!ReadArguments(md, readers, logger, worker, InvokeMode.ClientRpc, ref failed))
            {
                return null;
            }

            worker.Emit(OpCodes.Callvirt, func);
            worker.Emit(OpCodes.Ret);
            NetworkBehaviourProcess.AddInvokeParameters(models, rpc.Parameters);
            td.Methods.Add(rpc);
            return rpc;
        }

        /// <summary>
        /// ClientRpc方法体
        /// </summary>
        /// <param name="models"></param>
        /// <param name="writers"></param>
        /// <param name="logger"></param>
        /// <param name="td"></param>
        /// <param name="md"></param>
        /// <param name="ca"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        public static MethodDefinition ProcessClientRpcInvoke(Models models, Writer writers, Logger logger, TypeDefinition td, MethodDefinition md, CustomAttribute ca, ref bool failed)
        {
            var rpc = BaseInvokeMethod(logger, td, md, ref failed);
            var worker = md.Body.GetILProcessor();
            NetworkBehaviourProcess.WriteInitLocals(worker, models);
            NetworkBehaviourProcess.WritePopWriter(worker, models);

            if (!WriteArguments(worker, writers, logger, md, InvokeMode.ClientRpc, ref failed))
            {
                return null;
            }
            
            worker.Emit(OpCodes.Ldarg_0);
            worker.Emit(OpCodes.Ldstr, md.FullName);
            worker.Emit(OpCodes.Ldc_I4, (int)NetworkManager.GetStableId(md.FullName));
            worker.Emit(OpCodes.Ldloc_0);
            worker.Emit(OpCodes.Ldc_I4, ca.GetFieldType<int>());
            worker.Emit(OpCodes.Callvirt, models.sendClientRpcInternal);
            NetworkBehaviourProcess.WritePushWriter(worker, models);
            worker.Emit(OpCodes.Ret);
            return rpc;
        }

        /// <summary>
        /// ServerRpc方法
        /// </summary>
        /// <param name="models"></param>
        /// <param name="readers"></param>
        /// <param name="logger"></param>
        /// <param name="td"></param>
        /// <param name="md"></param>
        /// <param name="func"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        public static MethodDefinition ProcessServerRpc(Models models, Reader readers, Logger logger, TypeDefinition td, MethodDefinition md, MethodDefinition func, ref bool failed)
        {
            var rpcName = Process.GenerateMethodName(Const.INV_METHOD, md);
            var rpc = new MethodDefinition(rpcName, Const.RPC_ATTRS, models.Import(typeof(void)));
            var worker = rpc.Body.GetILProcessor();
            var label = worker.Create(OpCodes.Nop);
            NetworkServerActive(worker, models, md.Name, label, "ServerRpc");

            worker.Emit(OpCodes.Ldarg_0);
            worker.Emit(OpCodes.Castclass, td);

            if (!ReadArguments(md, readers, logger, worker, InvokeMode.ServerRpc, ref failed))
            {
                return null;
            }

            foreach (var definition in md.Parameters)
            {
                if (IsNetworkClient(definition, InvokeMode.ServerRpc))
                {
                    worker.Emit(OpCodes.Ldarg_2);
                }
            }
            worker.Emit(OpCodes.Callvirt, func);
            worker.Emit(OpCodes.Ret);
            NetworkBehaviourProcess.AddInvokeParameters(models, rpc.Parameters);
            td.Methods.Add(rpc);
            return rpc;
        }

        /// <summary>
        /// ServerRpc方法体
        /// </summary>
        /// <param name="models"></param>
        /// <param name="writers"></param>
        /// <param name="logger"></param>
        /// <param name="td"></param>
        /// <param name="md"></param>
        /// <param name="ca"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        public static MethodDefinition ProcessServerRpcInvoke(Models models, Writer writers, Logger logger, TypeDefinition td, MethodDefinition md, CustomAttribute ca, ref bool failed)
        {
            var rpc = BaseInvokeMethod(logger, td, md, ref failed);
            var worker = md.Body.GetILProcessor();
            NetworkBehaviourProcess.WriteInitLocals(worker, models);
            NetworkBehaviourProcess.WritePopWriter(worker, models);

            if (!WriteArguments(worker, writers, logger, md, InvokeMode.ServerRpc, ref failed))
            {
                return null;
            }
            
            worker.Emit(OpCodes.Ldarg_0);
            worker.Emit(OpCodes.Ldstr, md.FullName);
            worker.Emit(OpCodes.Ldc_I4, (int)NetworkManager.GetStableId(md.FullName));
            worker.Emit(OpCodes.Ldloc_0);
            worker.Emit(OpCodes.Ldc_I4, ca.GetFieldType<int>());
            worker.Emit(OpCodes.Call, models.sendServerRpcInternal);
            NetworkBehaviourProcess.WritePushWriter(worker, models);
            worker.Emit(OpCodes.Ret);

            return rpc;
        }

        /// <summary>
        /// TargetRpc方法
        /// </summary>
        /// <param name="models"></param>
        /// <param name="readers"></param>
        /// <param name="logger"></param>
        /// <param name="td"></param>
        /// <param name="md"></param>
        /// <param name="func"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        public static MethodDefinition ProcessTargetRpc(Models models, Reader readers, Logger logger, TypeDefinition td, MethodDefinition md, MethodDefinition func, ref bool failed)
        {
            var rpcName = Process.GenerateMethodName(Const.INV_METHOD, md);
            var rpc = new MethodDefinition(rpcName, Const.RPC_ATTRS, models.Import(typeof(void)));
            var worker = rpc.Body.GetILProcessor();
            var label = worker.Create(OpCodes.Nop);
            NetworkClientActive(worker, models, md.Name, label, "TargetRpc");

            worker.Emit(OpCodes.Ldarg_0);
            worker.Emit(OpCodes.Castclass, td);

            if (HasNetworkClient(md))
            {
                worker.Emit(OpCodes.Ldnull);
            }

            if (!ReadArguments(md, readers, logger, worker, InvokeMode.TargetRpc, ref failed))
            {
                return null;
            }

            worker.Emit(OpCodes.Callvirt, func);
            worker.Emit(OpCodes.Ret);
            NetworkBehaviourProcess.AddInvokeParameters(models, rpc.Parameters);
            td.Methods.Add(rpc);
            return rpc;
        }

        /// <summary>
        /// TargetRpc方法体
        /// </summary>
        /// <param name="models"></param>
        /// <param name="writers"></param>
        /// <param name="logger"></param>
        /// <param name="td"></param>
        /// <param name="md"></param>
        /// <param name="ca"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        public static MethodDefinition ProcessTargetRpcInvoke(Models models, Writer writers, Logger logger, TypeDefinition td, MethodDefinition md, CustomAttribute ca, ref bool failed)
        {
            var rpc = BaseInvokeMethod(logger, td, md, ref failed);
            var worker = md.Body.GetILProcessor();
            NetworkBehaviourProcess.WriteInitLocals(worker, models);
            NetworkBehaviourProcess.WritePopWriter(worker, models);

            if (!WriteArguments(worker, writers, logger, md, InvokeMode.TargetRpc, ref failed))
            {
                return null;
            }
            
            worker.Emit(OpCodes.Ldarg_0);
            worker.Emit(HasNetworkClient(md) ? OpCodes.Ldarg_1 : OpCodes.Ldnull);
            worker.Emit(OpCodes.Ldstr, md.FullName);
            worker.Emit(OpCodes.Ldc_I4, (int)NetworkManager.GetStableId(md.FullName));
            worker.Emit(OpCodes.Ldloc_0);
            worker.Emit(OpCodes.Ldc_I4, ca.GetFieldType<int>());
            worker.Emit(OpCodes.Callvirt, models.sendTargetRpcInternal);
            NetworkBehaviourProcess.WritePushWriter(worker, models);
            worker.Emit(OpCodes.Ret);
            return rpc;
        }

        /// <summary>
        /// 写入参数
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="writers"></param>
        /// <param name="logger"></param>
        /// <param name="method"></param>
        /// <param name="mode"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        private static bool WriteArguments(ILProcessor worker, Writer writers, Logger logger, MethodDefinition method, InvokeMode mode, ref bool failed)
        {
            var skipFirst = mode == InvokeMode.TargetRpc && HasNetworkClient(method);
            var argument = 1;
            foreach (var pd in method.Parameters)
            {
                if (argument == 1 && skipFirst)
                {
                    argument += 1;
                    continue;
                }

                if (IsNetworkClient(pd, mode))
                {
                    argument += 1;
                    continue;
                }

                var writer = writers.GetFunction(pd.ParameterType, ref failed);
                if (writer == null)
                {
                    logger.Error($"{method.Name} 有无效的参数 {pd}。不支持类型 {pd.ParameterType}。", method);
                    failed = true;
                    return false;
                }

                worker.Emit(OpCodes.Ldloc_0);
                worker.Emit(OpCodes.Ldarg, argument);
                worker.Emit(OpCodes.Call, writer);
                argument += 1;
            }

            return true;
        }

        /// <summary>
        /// 读取参数
        /// </summary>
        /// <param name="method"></param>
        /// <param name="readers"></param>
        /// <param name="logger"></param>
        /// <param name="worker"></param>
        /// <param name="mode"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        private static bool ReadArguments(MethodDefinition method, Reader readers, Logger logger, ILProcessor worker, InvokeMode mode, ref bool failed)
        {
            var skipFirst = mode == InvokeMode.TargetRpc && HasNetworkClient(method);
            var argument = 1;
            foreach (var pd in method.Parameters)
            {
                if (argument == 1 && skipFirst)
                {
                    argument += 1;
                    continue;
                }

                if (IsNetworkClient(pd, mode))
                {
                    argument += 1;
                    continue;
                }

                var reader = readers.GetFunction(pd.ParameterType, ref failed);

                if (reader == null)
                {
                    logger.Error($"{method.Name} 有无效的参数 {pd}。不支持类型 {pd.ParameterType}。", method);
                    failed = true;
                    return false;
                }

                worker.Emit(OpCodes.Ldarg_1);
                worker.Emit(OpCodes.Call, reader);

                if (pd.ParameterType.Is<float>())
                {
                    worker.Emit(OpCodes.Conv_R4);
                }
                else if (pd.ParameterType.Is<double>())
                {
                    worker.Emit(OpCodes.Conv_R8);
                }
            }

            return true;
        }
        
        /// <summary>
        /// 判断指定连接参数
        /// </summary>
        /// <param name="md"></param>
        /// <returns></returns>
        private static bool HasNetworkClient(MethodDefinition md)
        {
            if (md.Parameters.Count <= 0)
            {
                return false;
            }

            var tr = md.Parameters[0].ParameterType;
            return tr.Is<NetworkClient>() || tr.IsDerivedFrom<NetworkClient>();
        }

        /// <summary>
        /// 发送连接
        /// </summary>
        /// <param name="pd"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool IsNetworkClient(ParameterDefinition pd, InvokeMode mode)
        {
            if (mode != InvokeMode.ServerRpc)
            {
                return false;
            }

            var tr = pd.ParameterType;
            return tr.Is<NetworkClient>() || tr.Resolve().IsDerivedFrom<NetworkClient>();
        }

        /// <summary>
        /// 注入网络客户端是否活跃
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="models"></param>
        /// <param name="mdName"></param>
        /// <param name="label"></param>
        /// <param name="error"></param>
        private static void NetworkClientActive(ILProcessor worker, Models models, string mdName, Instruction label, string error)
        {
            worker.Emit(OpCodes.Call, models.NetworkClientActiveRef);
            worker.Emit(OpCodes.Brtrue, label);
            worker.Emit(OpCodes.Ldstr, $"{error} 远程调用 {mdName} 方法，但是客户端不是活跃的。");
            worker.Emit(OpCodes.Call, models.logErrorRef);
            worker.Emit(OpCodes.Ret);
            worker.Append(label);
        }

        /// <summary>
        /// 注入网络服务器是否活跃
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="models"></param>
        /// <param name="mdName"></param>
        /// <param name="label"></param>
        /// <param name="error"></param>
        private static void NetworkServerActive(ILProcessor worker, Models models, string mdName, Instruction label, string error)
        {
            worker.Emit(OpCodes.Call, models.NetworkServerActiveRef);
            worker.Emit(OpCodes.Brtrue, label);
            worker.Emit(OpCodes.Ldstr, $"{error} 远程调用 {mdName} 方法，但是服务器不是活跃的。");
            worker.Emit(OpCodes.Call, models.logErrorRef);
            worker.Emit(OpCodes.Ret);
            worker.Append(label);
        }

        /// <summary>
        /// 处理基本的Rpc方法
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="td"></param>
        /// <param name="md"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        private static MethodDefinition BaseInvokeMethod(Logger logger, TypeDefinition td, MethodDefinition md, ref bool failed)
        {
            var newName = Process.GenerateMethodName(Const.RPC_METHOD, md);
            var method = new MethodDefinition(newName, md.Attributes, md.ReturnType)
            {
                IsPublic = false,
                IsFamily = true
            };

            foreach (var pd in md.Parameters)
            {
                method.Parameters.Add(new ParameterDefinition(pd.Name, ParameterAttributes.None, pd.ParameterType));
            }

            (method.Body, md.Body) = (md.Body, method.Body);

            foreach (var point in md.DebugInformation.SequencePoints)
            {
                method.DebugInformation.SequencePoints.Add(point);
            }

            md.DebugInformation.SequencePoints.Clear();

            foreach (var info in md.CustomDebugInformations)
            {
                method.CustomDebugInformations.Add(info);
            }

            md.CustomDebugInformations.Clear();
            (md.DebugInformation.Scope, method.DebugInformation.Scope) = (method.DebugInformation.Scope, md.DebugInformation.Scope);
            td.Methods.Add(method);
            ProcessBaseMethod(logger, td, method, ref failed);
            return method;
        }

        /// <summary>
        /// 处理修正的Rpc方法
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="td"></param>
        /// <param name="md"></param>
        /// <param name="failed"></param>
        private static void ProcessBaseMethod(Logger logger, TypeDefinition td, MethodDefinition md, ref bool failed)
        {
            var fullName = md.Name;
            if (!fullName.StartsWith(Const.RPC_METHOD))
            {
                return;
            }

            var name = md.Name.Substring(Const.RPC_METHOD.Length);

            foreach (var instruction in md.Body.Instructions)
            {
                if (IsInvokeToMethod(instruction, out var methodDef))
                {
                    var newName = Process.GenerateMethodName("", methodDef);
                    if (newName == name)
                    {
                        var baseType = td.BaseType.Resolve();
                        var baseMethod = baseType.GetMethodInBaseType(fullName);

                        if (baseMethod == null)
                        {
                            logger.Error($"找不到base方法: {fullName}", md);
                            failed = true;
                            return;
                        }

                        if (!baseMethod.IsVirtual)
                        {
                            logger.Error($"找不到virtual的方法: {fullName}", md);
                            failed = true;
                            return;
                        }

                        instruction.Operand = baseMethod;
                    }
                }
            }
        }

        private static bool IsInvokeToMethod(Instruction instr, out MethodDefinition md)
        {
            if (instr.OpCode == OpCodes.Call && instr.Operand is MethodDefinition method)
            {
                md = method;
                return true;
            }

            md = null;
            return false;
        }
    }
}