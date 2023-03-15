using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    public static class EntityExtension
    {
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            var component = obj.GetComponent<T>();
            if (component == null) obj.AddComponent<T>();
            return component;
        }

        public static T GetOrAddController<T>(this IEntity entity) where T : ScriptableObject, IController
        {
            var controller = entity.GetController<T>();
            if (controller == null) entity.AddController<T>();
            return controller;
        }
    }
}