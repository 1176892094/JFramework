using System;
using JFramework.Interface;

namespace JFramework
{
    public static class AsExtension
    {
        public static T As<T>(this object obj) => (T)obj;

        public static T As<T>(this int value) where T : Enum => (T)Enum.ToObject(typeof(T), value);

        public static int ToInt32(this Enum value) => Convert.ToInt32(value);
    }
}