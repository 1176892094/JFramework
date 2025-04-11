// *********************************************************************************
// # Project: Forest
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-12 15:01:52
// # Recently: 2025-01-12 15:01:52
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Linq;
using System.Runtime.CompilerServices;
using Astraia.Common;
using Mono.Cecil;
using Mono.Cecil.Cil;
using UnityEngine;

namespace Astraia.Editor
{
    internal static class RuntimeAttribute
    {
          /// <summary>
        /// 初始化读写流
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="module"></param>
        /// <param name="setters"></param>
        /// <param name="getters"></param>
        /// <param name="td"></param>
        public static void RuntimeInitializeOnLoad(AssemblyDefinition assembly, Module module, Setter setters, Getter getters, TypeDefinition td)
        {
            var method = new MethodDefinition(nameof(RuntimeInitializeOnLoad), MethodAttributes.Public | MethodAttributes.Static, module.Import(typeof(void)));

            AddRuntimeInitializeOnLoadAttribute(assembly, module, method);

            if (Resolve.IsEditor(assembly))
            {
                AddInitializeOnLoadAttribute(assembly, module, method);
            }

            var worker = method.Body.GetILProcessor();
            setters.InitializeSetters(worker);
            getters.InitializeGetters(worker);
            worker.Emit(OpCodes.Ret);
            td.Methods.Add(method);
        }

        /// <summary>
        /// 添加 RuntimeInitializeLoad 属性类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="module"></param>
        /// <param name="md"></param>
        private static void AddRuntimeInitializeOnLoadAttribute(AssemblyDefinition assembly, Module module, MethodDefinition md)
        {
            var ctor = module.RuntimeInitializeOnLoadMethodAttribute.GetConstructors().Last();
            var attribute = new CustomAttribute(assembly.MainModule.ImportReference(ctor));
            attribute.ConstructorArguments.Add(new CustomAttributeArgument(module.Import<RuntimeInitializeLoadType>(), RuntimeInitializeLoadType.BeforeSceneLoad));
            md.CustomAttributes.Add(attribute);
        }

        /// <summary>
        /// 添加 RuntimeInitializeLoad 属性标记
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="module"></param>
        /// <param name="md"></param>
        private static void AddInitializeOnLoadAttribute(AssemblyDefinition assembly, Module module, MethodDefinition md)
        {
            var ctor = module.InitializeOnLoadMethodAttribute.GetConstructors().First();
            var attribute = new CustomAttribute(assembly.MainModule.ImportReference(ctor));
            md.CustomAttributes.Add(attribute);
        }
        
        /// <summary>
        /// 在Process中调用
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resolver"></param>
        /// <param name="logger"></param>
        /// <param name="setter"></param>
        /// <param name="getter"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        public static bool Process(AssemblyDefinition assembly, IAssemblyResolver resolver, Logger logger, Setter setter, Getter getter, ref bool failed)
        {
            ProcessAssembly(assembly, resolver, logger, setter, getter, ref failed);
            return ProcessCustomCode(assembly, assembly, setter, getter, ref failed);
        }

        /// <summary>
        /// 处理网络代码
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resolver"></param>
        /// <param name="logger"></param>
        /// <param name="setter"></param>
        /// <param name="getter"></param>
        /// <param name="failed"></param>
        private static void ProcessAssembly(AssemblyDefinition assembly, IAssemblyResolver resolver, Logger logger, Setter setter, Getter getter, ref bool failed)
        {
            AssemblyNameReference assemblyRef = null;
            foreach (var reference in assembly.MainModule.AssemblyReferences)
            {
                if (reference.Name == Const.ASSEMBLY)
                {
                    assemblyRef = reference;
                    break;
                }
            }

            if (assemblyRef != null)
            {
                var assemblyDef = resolver.Resolve(assemblyRef);
                if (assemblyDef != null)
                {
                    ProcessCustomCode(assembly, assemblyDef, setter, getter, ref failed);
                }
                else
                {
                    logger.Error($"自动生成网络代码失败: {assemblyRef}");
                }
            }
            else
            {
                logger.Error("注册程序集 Astraia.Net.dll 失败");
            }
        }

        /// <summary>
        /// 处理本地代码
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="assemblyDef"></param>
        /// <param name="setter"></param>
        /// <param name="getter"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        private static bool ProcessCustomCode(AssemblyDefinition assembly, AssemblyDefinition assemblyDef, Setter setter, Getter getter, ref bool failed)
        {
            var changed = false;
            foreach (var td in assemblyDef.MainModule.Types)
            {
                if (td.IsAbstract && td.IsSealed)
                {
                    changed |= ProcessSetter(assembly, td, setter);
                    changed |= ProcessGetter(assembly, td, getter);
                }
            }

            foreach (var td in assemblyDef.MainModule.Types)
            {
                changed |= ProcessMessage(assembly.MainModule, setter, getter, td, ref failed);
            }

            return changed;
        }

        /// <summary>
        /// 加载声明的写入器
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="td"></param>
        /// <param name="setter"></param>
        /// <returns></returns>
        private static bool ProcessSetter(AssemblyDefinition assembly, TypeDefinition td, Setter setter)
        {
            var change = false;
            foreach (var md in td.Methods)
            {
                if (md.Parameters.Count != 2)
                    continue;

                if (!md.Parameters[0].ParameterType.Is<MemorySetter>())
                    continue;

                if (!md.ReturnType.Is(typeof(void)))
                    continue;

                if (!md.HasCustomAttribute<ExtensionAttribute>())
                    continue;

                if (md.HasGenericParameters)
                    continue;

                setter.Register(md.Parameters[1].ParameterType, assembly.MainModule.ImportReference(md));
                change = true;
            }

            return change;
        }

        /// <summary>
        /// 加载声明的读取器
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="td"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        private static bool ProcessGetter(AssemblyDefinition assembly, TypeDefinition td, Getter getter)
        {
            var change = false;
            foreach (var md in td.Methods)
            {
                if (md.Parameters.Count != 1)
                    continue;

                if (!md.Parameters[0].ParameterType.Is<MemoryGetter>())
                    continue;

                if (md.ReturnType.Is(typeof(void)))
                    continue;

                if (!md.HasCustomAttribute<ExtensionAttribute>())
                    continue;

                if (md.HasGenericParameters)
                    continue;

                getter.Register(md.ReturnType, assembly.MainModule.ImportReference(md));
                change = true;
            }

            return change;
        }

        /// <summary>
        /// 加载读写流的信息
        /// </summary>
        /// <param name="module"></param>
        /// <param name="setter"></param>
        /// <param name="getter"></param>
        /// <param name="td"></param>
        /// <param name="failed"></param>
        /// <returns></returns>
        private static bool ProcessMessage(ModuleDefinition module, Setter setter, Getter getter, TypeDefinition td, ref bool failed)
        {
            var change = false;
            if (!td.IsAbstract && !td.IsInterface && td.IsImplement<IMessage>())
            {
                getter.GetFunction(module.ImportReference(td), ref failed);
                setter.GetFunction(module.ImportReference(td), ref failed);
                change = true;
            }

            foreach (var nested in td.NestedTypes)
            {
                change |= ProcessMessage(module, setter, getter, nested, ref failed);
            }

            return change;
        }
    }
}