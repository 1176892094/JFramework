using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    using Controllers = Dictionary<Type, ScriptableObject>;

    /// <summary>
    /// 控制器管理器
    /// </summary>
    internal static class ControllerManager
    {
        /// <summary>
        /// 全局控制器容器
        /// </summary>
        internal static readonly Dictionary<ICharacter, Controllers> characters = new Dictionary<ICharacter, Controllers>();

        /// <summary>
        /// 获取或添加控制器
        /// </summary>
        /// <param name="character"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Register<T>(ICharacter character) where T : ScriptableObject, IController
        {
            if (!characters.TryGetValue(character, out Controllers controllers))
            {
                controllers = new Controllers();
                characters.Add(character, controllers);
            }

            if (!controllers.ContainsKey(typeof(T)))
            {
                var controller = ScriptableObject.CreateInstance<T>();
                controllers.Add(typeof(T), controller);
                controller.Spawn(character);
            }

            return (T)characters[character][typeof(T)];
        }

        /// <summary>
        /// 销毁指定控制器
        /// </summary>
        /// <param name="character"></param>
        public static void Destroy(ICharacter character)
        {
            if (characters.TryGetValue(character, out Controllers controllers))
            {
                foreach (var controller in controllers.Values)
                {
                    Object.Destroy(controller);
                }

                controllers.Clear();
                characters.Remove(character);
            }
        }

        /// <summary>
        /// 销毁所有控制器
        /// </summary>
        internal static void Clear()
        {
            var controllers = characters.Values.SelectMany(dictionary => dictionary.Values);
            foreach (var controller in controllers)
            {
                Object.Destroy(controller);
            }

            characters.Clear();
        }
    }
}