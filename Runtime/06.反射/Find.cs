// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 21:01:21
// # Recently: 2025-01-11 18:01:32
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable All

namespace JFramework
{
    public static partial class Service
    {
        public static class Find
        {
            public const BindingFlags Static = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            public const BindingFlags Entity = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            private static readonly Dictionary<string, Type> cachedType = new Dictionary<string, Type>();
            private static readonly Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

            public static Assembly Assembly(string name)
            {
                if (Find.assemblies.TryGetValue(name, out var assembly))
                {
                    return assembly;
                }

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var current in assemblies)
                {
                    if (current.GetName().Name == name)
                    {
                        assembly = current;
                        break;
                    }
                }

                if (assembly != null)
                {
                    Find.assemblies[name] = assembly;
                }

                return assembly;
            }

            public static Type Type(string name)
            {
                if (Find.cachedType.TryGetValue(name, out var cachedType))
                {
                    return cachedType;
                }

                var index = name.LastIndexOf(',');
                if (index < 0)
                {
                    return System.Type.GetType(name);
                }

                var assembly = Assembly(name.Substring(index + 1).Trim());
                if (assembly != null)
                {
                    cachedType = assembly.GetType(name.Substring(0, index));
                    Find.cachedType.Add(name, cachedType);
                }

                return cachedType;
            }
        }
    }
}