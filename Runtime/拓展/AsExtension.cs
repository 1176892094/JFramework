using System;
using JFramework.Interface;

namespace JFramework
{
    public static class AsExtension
    {
        public static T As<T>(this IData data) where T : IData => (T)data;
        
        public static T As<T>(this IDataTable data) where T : IDataTable => (T)data;
        
        public static T As<T>(this IEntity entity) where T : IEntity => (T)entity;
        
        public static T As<T>(this IController controller) where T : IController => (T)controller;
        
        public static T As<T>(this int value) where T : struct => (T)Enum.ToObject(typeof(T), value);

        public static int Int(this Enum value) => Convert.ToInt32(value);
    }
}