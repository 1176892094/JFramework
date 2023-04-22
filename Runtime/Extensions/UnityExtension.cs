using System;
using UnityEngine;

namespace JFramework
{
    public static class UnityExtension
    {
        /// <summary>
        /// 基于强制转换的拓展，方便查找引用
        /// </summary>
        /// <param name="obj">任何对象</param>
        /// <typeparam name="T">可转换为任何对象</typeparam>
        /// <returns>返回转换的对象类型</returns>
        public static T As<T>(this object obj) => (T)obj;

        /// <summary>
        /// 基于强制转换的拓展，方便查找引用
        /// </summary>
        /// <param name="obj">任何对象</param>
        /// <param name="index">对象组索引</param>
        /// <typeparam name="T">可转换为任何对象</typeparam>
        /// <returns>返回转换的对象类型</returns>
        public static T As<T>(this object[] obj, int index = 0) => (T)obj[index];

        /// <summary>
        /// 比较int和enum的值是否相等
        /// </summary>
        /// <param name="value">int的值</param>
        /// <param name="type">enum的类型</param>
        /// <returns>返回是否相等</returns>
        public static bool Compare(this int value, Enum type) => value == type.As<int>();

        /// <summary>
        /// 比较string和enum的值是否相等
        /// </summary>
        /// <param name="value">string的值</param>
        /// <param name="type">enum的类型</param>
        /// <returns>返回字符串是否相等</returns>
        public static bool Compare(this string value, Enum type) => value == type.ToString();
    }
}