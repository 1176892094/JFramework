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
        /// 实体控制器获取
        /// </summary>
        /// <param name="entity"></param>
        public static T Get<T>(this IEntity entity) where T : ScriptableObject, IController
        {
            return (T)ControllerManager.Register(entity, typeof(T));
        }

        /// <summary>
        /// 实体卸载控制器
        /// </summary>
        /// <param name="entity"></param>
        public static void UnRegister(this IEntity entity)
        {
            ControllerManager.UnRegister(entity);
        }
    }
}