using JFramework.Core;
using JFramework.Interface;
using UnityEngine;

// ReSharper disable All

namespace JFramework
{
    public static partial class Extensions
    {
        /// <summary>
        /// 角色接口的注册
        /// </summary>
        /// <param name="character"></param>
        public static T Register<T>(this ICharacter character) where T : ScriptableObject, IController
        {
            return ControllerManager.Register<T>(character);
        }

        /// <summary>
        /// 角色接口的卸载
        /// </summary>
        /// <param name="character"></param>
        public static void UnRegister(this ICharacter character)
        {
            ControllerManager.UnRegister(character);
        }
    }
}