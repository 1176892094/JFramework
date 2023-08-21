using JFramework.Core;
using JFramework.Interface;
using UnityEngine;

// ReSharper disable All

namespace JFramework
{
    public static partial class Extensions
    {
        /// <summary>
        /// 侦听实体的更新事件
        /// </summary>
        public static void Listen(this IUpdate entity)
        {
            if (!GlobalManager.Runtime) return;
            GlobalManager.OnUpdate += entity.OnUpdate;
        }

        /// <summary>
        /// 移除实体的更新
        /// </summary>
        public static void Remove(this IUpdate entity)
        {
            if (!GlobalManager.Runtime) return;
            GlobalManager.OnUpdate -= entity.OnUpdate;
        }

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