using System;
using UnityEngine;

namespace JFramework
{
    public static class UnityExtension
    {
        /// <summary>
        /// 判断对象是否为Component类
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>返回是否为组件</returns>
        public static bool IsComponent(this Type type) => typeof(Component).IsAssignableFrom(type);

        /// <summary>
        /// 判断对象是否为Component类
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>返回是否为组件</returns>
        public static bool IsScriptable(this Type type) => typeof(ScriptableObject).IsAssignableFrom(type);

        /// <summary>
        /// 比较int和enum的值是否相等
        /// </summary>
        /// <param name="value">int的值</param>
        /// <param name="type">enum的类型</param>
        /// <returns>返回是否相等</returns>
        public static bool Compare(this int value, Enum type) => value == Convert.ToInt32(type);

        /// <summary>
        /// 比较string和enum的值是否相等
        /// </summary>
        /// <param name="value">string的值</param>
        /// <param name="type">enum的类型</param>
        /// <returns>返回字符串是否相等</returns>
        public static bool Compare(this string value, Enum type) => value == type.ToString();

        /// <summary>
        /// 将当前枚举切换到下一枚举
        /// 若当前为最后一个，则循环至第一个
        /// </summary>
        /// <param name="current">当前枚举的拓展</param>
        /// <typeparam name="T">传入任何枚举类型</typeparam>
        /// <returns>返回当前枚举的下一个值</returns>
        public static T ToNext<T>(this T current) where T : Enum
        {
            var enumArray = (T[])Enum.GetValues(typeof(T));
            var currIndex = Array.IndexOf(enumArray, current);
            var nextIndex = (currIndex + 1) % enumArray.Length;
            return enumArray[nextIndex];
        }
        
        /// <summary>
        /// 将当前枚举切换到上一枚举
        /// 若当前为第一个，则循环至最后一个
        /// </summary>
        /// <param name="current">当前枚举的拓展</param>
        /// <typeparam name="T">传入任何枚举类型</typeparam>
        /// <returns>返回当前枚举的上一个值</returns>
        public static T ToLast<T>(this T current) where T : Enum
        {
            var enumArray = (T[])Enum.GetValues(typeof(T));
            var currIndex = Array.IndexOf(enumArray, current);
            var lastIndex = (currIndex - 1 + enumArray.Length) % enumArray.Length;
            return enumArray[lastIndex];
        }
    }
}