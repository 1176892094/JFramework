using System;
using System.Linq;
using System.Reflection;

namespace JFramework
{
    /// <summary>
    /// 反射工具类
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// 实例类
        /// </summary>
        public const BindingFlags Instance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// 静态类
        /// </summary>
        public const BindingFlags Static = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// 获取指定名称的程序集
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.FirstOrDefault(assembly => assembly.GetName().Name == name);
        }

        /// <summary>
        /// 获取所有继承 T 的类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Type[] GetTypes<T>(Assembly assembly)
        {
            var types = assembly.GetTypes();
            return Array.FindAll(types, type => typeof(T).IsAssignableFrom(type));
        }

        /// <summary>
        /// 根据 Attribute 获取字段
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static FieldInfo GetField<T>(Type type) where T : Attribute
        {
            var fields = type.GetFields(Instance);
            return fields.FirstOrDefault(field => field.GetCustomAttributes(typeof(T), false).Length > 0);
        }

        /// <summary>
        /// 根据 String 获取字段
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FieldInfo GetField(Type type, string name)
        {
            var fields = type.GetFields(Instance);
            return fields.FirstOrDefault(field => field.GetCustomAttribute<UIFindAttribute>(false)?.find == name);
        }
    }
}