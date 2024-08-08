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
using System.Linq;
using System.Reflection;

namespace JFramework
{
    public static class Reflection
    {
        public const BindingFlags Static = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        public const BindingFlags Instance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static Assembly GetAssembly(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.FirstOrDefault(assembly => assembly.GetName().Name == name);
        }

        public static Type[] GetTypes<T>(Assembly assembly)
        {
            var types = assembly.GetTypes();
            return Array.FindAll(types, type => typeof(T).IsAssignableFrom(type));
        }

        public static PropertyInfo GetProperty<T>(Type type) where T : Attribute
        {
            var properties = type.GetProperties(Instance);
            return properties.FirstOrDefault(field => field.GetCustomAttributes(typeof(T), false).Length > 0);
        }
    }
}