// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-24  23:40
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using JFramework.Core;
using JFramework.Interface;
using UnityEngine;

// ReSharper disable All

namespace JFramework
{
    /// <summary>
    /// 实体拓展
    /// </summary>
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
        /// 角色接口的控制器注册
        /// </summary>
        /// <param name="character"></param>
        public static T Register<T>(this ICharacter character) where T : ScriptableObject, IController
        {
            return ControllerManager.Register<T>(character);
        }
        
        /// <summary>
        /// 角色接口的控制器获取
        /// </summary>
        /// <param name="character"></param>
        public static T Get<T>(this ICharacter character) where T : ScriptableObject, IController
        {
            return ControllerManager.Get<T>(character);
        }

        /// <summary>
        /// 角色接口的控制器卸载
        /// </summary>
        /// <param name="character"></param>
        public static void UnRegister(this ICharacter character)
        {
            ControllerManager.UnRegister(character);
        }
    }
}