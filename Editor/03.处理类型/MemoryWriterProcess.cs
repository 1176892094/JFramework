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
using JFramework.Net;
using Mono.Cecil;
using Mono.Cecil.Cil;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Editor
{
    internal class Writer
    {
        private readonly Dictionary<TypeReference, MethodReference> methods = new Dictionary<TypeReference, MethodReference>(new Comparer());
        private readonly Models models;
        private readonly Logger logger;
        private readonly TypeDefinition generate;
        private readonly AssemblyDefinition assembly;

        public Writer(AssemblyDefinition assembly, Models models, TypeDefinition generate, Logger logger)
        {
            this.logger = logger;
            this.models = models;
            this.assembly = assembly;
            this.generate = generate;
        }

        public void Register(TypeReference tr, MethodReference md)
        {
            var imported = assembly.MainModule.ImportReference(tr);
            methods[imported] = md;
        }

        public MethodReference GetFunction(TypeReference tr, ref bool failed)
        {
            if (methods.TryGetValue(tr, out var mr))
            {
                return mr;
            }

            var reference = assembly.MainModule.ImportReference(tr);
            return Process(reference, ref failed);
        }

        private MethodReference Process(TypeReference tr, ref bool failed)
        {
            if (tr.IsArray)
            {
                if (tr.IsMultidimensionalArray())
                {
                    logger.Error($"无法为多维数组 {tr.Name} 生成写入器", tr);
                    return null;
                }

                return AddCollection(tr, tr.GetElementType(), nameof(Net.Extensions.WriteArray), ref failed);
            }

            var td = tr.Resolve();
            if (td == null)
            {
                logger.Error($"无法为空类型 {tr.Name} 生成写入器", tr);
                return null;
            }

            if (tr.IsByReference)
            {
                logger.Error($"无法为反射 {tr.Name} 生成写入器", tr);
                return null;
            }

            if (td.IsEnum)
            {
                return AddEnum(tr, ref failed);
            }

            if (tr.Is(typeof(ArraySegment<>)))
            {
                return AddArraySegment(tr, ref failed);
            }

            if (tr.Is(typeof(List<>)))
            {
                var genericInstance = (GenericInstanceType)tr;
                var elementType = genericInstance.GenericArguments[0];
                return AddCollection(tr, elementType, nameof(Net.Extensions.WriteList), ref failed);
            }

            if (tr.IsDerivedFrom<NetworkBehaviour>() || tr.Is<NetworkBehaviour>())
            {
                return AddNetworkBehaviour(tr);
            }

            if (td.IsDerivedFrom<Component>())
            {
                logger.Error($"无法为组件 {tr.Name} 生成写入器", tr);
                return null;
            }

            if (tr.Is<Object>())
            {
                logger.Error($"无法为对象 {tr.Name} 生成写入器", tr);
                return null;
            }

            if (tr.Is<ScriptableObject>())
            {
                logger.Error($"无法为可视化脚本 {tr.Name} 生成写入器", tr);
                return null;
            }

            if (td.HasGenericParameters)
            {
                logger.Error($"无法为泛型参数 {tr.Name} 生成写入器", tr);
                return null;
            }

            if (td.IsInterface)
            {
                logger.Error($"无法为接口 {tr.Name} 生成写入器", tr);
                return null;
            }

            if (td.IsAbstract)
            {
                logger.Error($"无法为抽象或泛型 {tr.Name} 生成写入器", tr);
                return null;
            }

            return AddActivator(tr, ref failed);
        }

        private MethodDefinition AddEnum(TypeReference tr, ref bool failed)
        {
            var md = AddMethod(tr);
            var worker = md.Body.GetILProcessor();
            var mr = GetFunction(tr.Resolve().GetEnumUnderlyingType(), ref failed);
            worker.Emit(OpCodes.Ldarg_0);
            worker.Emit(OpCodes.Ldarg_1);
            worker.Emit(OpCodes.Call, mr);
            worker.Emit(OpCodes.Ret);
            return md;
        }
        
        private MethodDefinition AddArraySegment(TypeReference tr, ref bool failed)
        {
            var genericInstance = (GenericInstanceType)tr;
            var elementType = genericInstance.GenericArguments[0];
            return AddCollection(tr, elementType, nameof(Net.Extensions.WriteArraySegment), ref failed);
        }

        private MethodDefinition AddCollection(TypeReference tr, TypeReference element, string name, ref bool failed)
        {
            var md = AddMethod(tr);
            var func = GetFunction(element, ref failed);

            if (func == null)
            {
                logger.Error($"无法为 {tr} 生成写入器", tr);
                failed = true;
                return md;
            }
            
            var extensions = assembly.MainModule.ImportReference(typeof(Net.Extensions));
            var mr = Helper.GetMethod(extensions, assembly, logger, name, ref failed);

            var method = new GenericInstanceMethod(mr);
            method.GenericArguments.Add(element);
            var worker = md.Body.GetILProcessor();
            worker.Emit(OpCodes.Ldarg_0);
            worker.Emit(OpCodes.Ldarg_1);
            worker.Emit(OpCodes.Call, method);
            worker.Emit(OpCodes.Ret);
            return md;
        }

        private MethodReference AddNetworkBehaviour(TypeReference tr)
        {
            if (!methods.TryGetValue(models.Import<NetworkBehaviour>(), out var mr))
            {
                throw new MissingMethodException("获取 NetworkBehaviour 方法丢失");
            }

            Register(tr, mr);
            return mr;
        }

        private MethodDefinition AddActivator(TypeReference tr, ref bool failed)
        {
            var md = AddMethod(tr);
            var worker = md.Body.GetILProcessor();
            var td = tr.Resolve();

            if (!td.IsValueType)
            {
                AddNullCheck(worker, ref failed);
            }

            if (!AddFields(tr, worker, ref failed))
            {
                return null;
            }

            worker.Emit(OpCodes.Ret);
            return md;
        }

        private MethodDefinition AddMethod(TypeReference tr)
        {
            var md = new MethodDefinition($"Write{NetworkManager.GetStableId(tr.FullName)}", Const.RAW_ATTRS, models.Import(typeof(void)));
            md.Parameters.Add(new ParameterDefinition("writer", ParameterAttributes.None, models.Import<MemoryWriter>()));
            md.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, tr));
            md.Body.InitLocals = true;
            Register(tr, md);
            generate.Methods.Add(md);
            return md;
        }

        private void AddNullCheck(ILProcessor worker, ref bool failed)
        {
            var nop = worker.Create(OpCodes.Nop);
            worker.Emit(OpCodes.Ldarg_1);
            worker.Emit(OpCodes.Brtrue, nop);
            worker.Emit(OpCodes.Ldarg_0);
            worker.Emit(OpCodes.Ldc_I4_0);
            worker.Emit(OpCodes.Call, GetFunction(models.Import<bool>(), ref failed));
            worker.Emit(OpCodes.Ret);
            worker.Append(nop);

            worker.Emit(OpCodes.Ldarg_0);
            worker.Emit(OpCodes.Ldc_I4_1);
            worker.Emit(OpCodes.Call, GetFunction(models.Import<bool>(), ref failed));
        }

        private bool AddFields(TypeReference tr, ILProcessor worker, ref bool failed)
        {
            foreach (var field in tr.Resolve().FindAllPublicFields())
            {
                var mr = GetFunction(field.FieldType, ref failed);
                if (mr == null)
                {
                    return false;
                }

                worker.Emit(OpCodes.Ldarg_0);
                worker.Emit(OpCodes.Ldarg_1);
                worker.Emit(OpCodes.Ldfld, assembly.MainModule.ImportReference(field));
                worker.Emit(OpCodes.Call, mr);
            }

            return true;
        }

        internal void InitializeWriters(ILProcessor worker)
        {
            var module = assembly.MainModule;
            var reader = module.ImportReference(typeof(Writer<>));
            var func = module.ImportReference(typeof(Action<,>));
            var tr = module.ImportReference(typeof(MemoryWriter));
            var fr = module.ImportReference(typeof(Writer<>).GetField(nameof(Writer<object>.write)));
            var mr = module.ImportReference(typeof(Action<,>).GetConstructors()[0]);
            foreach (var (type, method) in methods)
            {
                worker.Emit(OpCodes.Ldnull);
                worker.Emit(OpCodes.Ldftn, method);
                var instance = func.MakeGenericInstanceType(tr, type);
                worker.Emit(OpCodes.Newobj, mr.MakeHostInstanceGeneric(assembly.MainModule, instance));
                instance = reader.MakeGenericInstanceType(type);
                worker.Emit(OpCodes.Stsfld, fr.SpecializeField(assembly.MainModule, instance));
            }
        }
    }
}