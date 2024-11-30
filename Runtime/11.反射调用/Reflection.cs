// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  15:09
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JFramework
{
    public static class Reflection
    {
        public const BindingFlags Static = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        public const BindingFlags Instance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static readonly Dictionary<string, Assembly> assemblyCaches = new Dictionary<string, Assembly>();

        public static Assembly GetAssembly(string name)
        {
            if (assemblyCaches.TryGetValue(name, out var assemblyCache))
            {
                return assemblyCache;
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            assemblyCache = assemblies.FirstOrDefault(assembly => assembly.GetName().Name == name);

            if (assemblyCache != null)
            {
                assemblyCaches[name] = assemblyCache;
            }

            return assemblyCache;
        }

        public static Type GetType(string name)
        {
            var index = name.LastIndexOf(',');
            if (index < 0)
            {
                return Type.GetType(name);
            }

            var assembly = GetAssembly(name.Substring(index + 1).Trim());
            return assembly.GetType(name.Substring(0, index));
        }
    }
}