// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-06-06  05:06
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Linq;
using JFramework.Net;
using Mono.Cecil;

namespace JFramework.Editor
{
    internal static class MonoBehaviourProcess
    {
        /// <summary>
        /// 处理MonoBehaviour
        /// </summary>
        /// <param name="td"></param>
        /// <param name="logger"></param>
        /// <param name="failed"></param>
        public static void Process(TypeDefinition td, Logger logger, ref bool failed)
        {
            ProcessVar(td, logger, ref failed);
            ProcessRpc(td, logger, ref failed);
        }

        /// <summary>
        /// 处理网络变量
        /// </summary>
        /// <param name="td"></param>
        /// <param name="logger"></param>
        /// <param name="failed"></param>
        private static void ProcessVar(TypeDefinition td, Logger logger, ref bool failed)
        {
            foreach (var fd in td.Fields.Where(fd => fd.HasCustomAttribute<SyncVarAttribute>()))
            {
                logger.Error($"网络变量 {fd.Name} 必须在 NetworkBehaviour 中使用。", fd);
                failed = true;
            }
        }

        /// <summary>
        /// 处理远程调用
        /// </summary>
        /// <param name="td"></param>
        /// <param name="logger"></param>
        /// <param name="failed"></param>
        private static void ProcessRpc(TypeDefinition td, Logger logger, ref bool failed)
        {
            foreach (var md in td.Methods)
            {
                if (md.HasCustomAttribute<ServerRpcAttribute>())
                {
                    logger.Error($"ServerRpc {md.Name} 必须在 NetworkBehaviour 中使用。", md);
                    failed = true;
                }

                if (md.HasCustomAttribute<ClientRpcAttribute>())
                {
                    logger.Error($"ClientRpc {md.Name} 必须在 NetworkBehaviour 中使用。", md);
                    failed = true;
                }

                if (md.HasCustomAttribute<TargetRpcAttribute>())
                {
                    logger.Error($"TargetRpc {md.Name} 必须在 NetworkBehaviour 中使用。", md);
                    failed = true;
                }
            }
        }
    }
}