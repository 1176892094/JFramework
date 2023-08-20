using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework.Core
{
    using Components = Dictionary<Type, ScriptableObject>;

    /// <summary>
    /// 控制器管理器
    /// </summary>
    internal static class ControllerManager
    {
        /// <summary>
        /// 全局控制器容器
        /// </summary>
        public static readonly Dictionary<ICharacter, Components> characters = new Dictionary<ICharacter, Components>();

        /// <summary>
        /// 获取或添加控制器
        /// </summary>
        /// <param name="character"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Register<T>(ICharacter character) where T : ScriptableObject, IController
        {
            if (!characters.TryGetValue(character, out Components components))
            {
                components = new Components();
                characters.Add(character, components);
            }

            if (!components.ContainsKey(typeof(T)))
            {
                var component = ScriptableObject.CreateInstance<T>();
                components.Add(typeof(T), component);
                component.Register(character);
            }

            return (T)characters[character][typeof(T)];
        }

        /// <summary>
        /// 销毁指定控制器
        /// </summary>
        /// <param name="character"></param>
        public static void UnRegister(ICharacter character)
        {
            if (characters.TryGetValue(character, out Components components))
            {
                foreach (var component in components.Values)
                {
                    Object.Destroy(component);
                }

                components.Clear();
                characters.Remove(character);
            }
        }

        /// <summary>
        /// 销毁所有控制器
        /// </summary>
        internal static void Clear()
        {
            var copies = characters.Keys.ToList();
            foreach (var character in copies)
            {
                character.UnRegister();
            }

            characters.Clear();
        }
    }
}