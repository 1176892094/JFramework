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
using System.Collections.Generic;
using System.Linq;
using JFramework.Net;
using Mono.Cecil;
using UnityEngine;

namespace JFramework.Editor
{
    internal class Process
    {
        private bool failed;
        private Models models;
        private Writer writer;
        private Reader reader;
        private SyncVarAccess access;
        private TypeDefinition process;
        private AssemblyDefinition assembly;
        private readonly Logger logger;

        public Process(Logger logger)
        {
            this.logger = logger;
        }

        public bool Execute(AssemblyDefinition assembly, IAssemblyResolver resolver, out bool change)
        {
            failed = false;
            change = false;
            try
            {
                this.assembly = assembly;
                
                if (assembly.MainModule.GetTypes().Any(type => type.Namespace == Const.GEN_SPACE && type.Name == Const.GEN_NAME))
                {
                    return true;
                }
                
                access = new SyncVarAccess();
                models = new Models(assembly, logger, ref failed);
                process = new TypeDefinition(Const.GEN_SPACE, Const.GEN_NAME, Const.GEN_ATTRS, models.Import<object>());
                writer = new Writer(assembly, models, process, logger);
                reader = new Reader(assembly, models, process, logger);
                change = RuntimeInitialize.Process(assembly, resolver, logger, writer, reader, ref failed);
                
                var mainModule = assembly.MainModule;
                
                change |= ProcessModule(mainModule);
                if (failed)
                {
                    return false;
                }
                
                if (change)
                {
                    SyncVarReplace.Process(mainModule, access);
                    mainModule.Types.Add(process);
                    RuntimeInitialize.RuntimeInitializeOnLoad(assembly, models, writer, reader, process);
                }

                return true;
            }
            catch (Exception e)
            {
                failed = true;
                logger.Error(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// 处理 NetworkBehaviour
        /// </summary>
        /// <param name="td"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        private bool ProcessNetworkBehavior(TypeDefinition td, ref bool failed)
        {
            if (!td.IsClass) return false;
            if (!td.IsDerivedFrom<NetworkBehaviour>())
            {
                if (td.IsDerivedFrom<MonoBehaviour>())
                {
                    MonoBehaviourProcess.Process(td, logger, ref failed);
                }

                return false;
            }
            var behaviours = new List<TypeDefinition>();

            TypeDefinition parent = td;
            while (parent != null)
            {
                if (parent.Is<NetworkBehaviour>())
                {
                    break;
                }

                try
                {
                    behaviours.Insert(0, parent);
                    parent = parent.BaseType.Resolve();
                }
                catch (AssemblyResolutionException)
                {
                    break;
                }
            }

            bool changed = false;
            foreach (TypeDefinition behaviour in behaviours)
            {
                changed |= new NetworkBehaviourProcess(assembly, access, models, writer, reader, logger, behaviour).Process(ref failed);
            }

            return changed;
        }

        /// <summary>
        /// 处理功能
        /// </summary>
        /// <param name="md"></param>
        /// <returns></returns>
        private bool ProcessModule(ModuleDefinition md)
        {
            return md.Types.Where(td => td.IsClass && td.BaseType.CanResolve()).Aggregate(false, (current, td) => current | ProcessNetworkBehavior(td, ref failed));
        }

        /// <summary>
        /// 处理方法中的参数
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="md"></param>
        /// <returns></returns>
        public static string GenerateMethodName(string prefix, MethodDefinition md)
        {
            prefix = md.Name + prefix;
            // return md.Parameters.Aggregate(prefix, (s, parameter) => s + "_" + NetworkManager.GetStableId(parameter.ParameterType.Name));
            return prefix;
        }
    }
}