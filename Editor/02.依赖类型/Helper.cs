// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-06-08  03:06
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace JFramework.Editor
{
    internal static class Helper
    {
        public static bool IsEditorAssembly(AssemblyDefinition ad)
        {
            return ad.MainModule.AssemblyReferences.Any(reference => reference.Name.StartsWith(nameof(UnityEditor)));
        }

        public static MethodReference GetMethod(TypeReference tr, AssemblyDefinition ad, Logger log, string name, ref bool failed)
        {
            if (tr == null)
            {
                log.Error($"没有无法解析方法: {name}");
                failed = true;
                return null;
            }

            var mr = GetMethod(tr, ad, log, method => method.Name == name, ref failed);
            if (mr == null)
            {
                log.Error($"在类型 {tr.Name} 中没有找到名称 {name} 的方法", tr);
                failed = true;
            }

            return mr;
        }

        public static MethodReference GetMethod(TypeReference tr, AssemblyDefinition ad, Logger log, Func<MethodDefinition, bool> func,
            ref bool failed)
        {
            foreach (var md in tr.Resolve().Methods.Where(func))
            {
                return ad.MainModule.ImportReference(md);
            }

            log.Error($"在类型 {tr.Name} 中没有找到方法", tr);
            failed = true;
            return null;
        }

        public static MethodReference TryResolveMethodInParents(TypeReference tr, AssemblyDefinition ad, string name)
        {
            if (tr == null)
            {
                return null;
            }
        
            foreach (var md in tr.Resolve().Methods.Where(md => md.Name == name))
            {
                MethodReference mr = md;
                if (tr.IsGenericInstance)
                {
                    mr = mr.MakeHostInstanceGeneric(tr.Module, (GenericInstanceType)tr);
                }
        
                return ad.MainModule.ImportReference(mr);
            }
        
            return TryResolveMethodInParents(tr.Resolve().BaseType.ApplyGenericParameters(tr), ad, name);
        }

        public static MethodDefinition ResolveDefaultPublicCtor(TypeReference tr)
        {
            return tr.Resolve().Methods.FirstOrDefault(md => md.Name == Const.CTOR && md.Resolve().IsPublic && md.Parameters.Count == 0);
        }

        public static MethodReference ResolveProperty(TypeReference tr, AssemblyDefinition ad, string name)
        {
            return (from pd in tr.Resolve().Properties where pd.Name == name select ad.MainModule.ImportReference(pd.GetMethod))
                .FirstOrDefault();
        }
    }
}

internal class Comparer : IEqualityComparer<TypeReference>
{
    public bool Equals(TypeReference x, TypeReference y) => x?.FullName == y?.FullName;

    public int GetHashCode(TypeReference obj) => obj.FullName.GetHashCode();
}