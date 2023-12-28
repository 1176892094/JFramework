// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-21  18:48
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
        /// <summary>
        /// 侦听实体的更新事件
        /// </summary>
        public static void Listen(this IUpdate entity)
        {
            if (GlobalManager.Runtime)
            {
                GlobalManager.OnUpdate += entity.OnUpdate;
            }
        }

        /// <summary>
        /// 移除实体的更新
        /// </summary>
        public static void Remove(this IUpdate entity)
        {
            if (GlobalManager.Runtime)
            {
                GlobalManager.OnUpdate -= entity.OnUpdate;
            }
        }

        /// <summary>
        /// 实体控制器获取
        /// </summary>
        /// <param name="gameObject"></param>
        public static T GetControl<T>(this GameObject gameObject) where T : Controller
        {
            return (T)GlobalManager.Entity.Register(gameObject, typeof(T));
        }

        /// <summary>
        /// 实体卸载控制器
        /// </summary>
        /// <param name="gameObject"></param>
        public static void UnRegister(this GameObject gameObject)
        {
            GlobalManager.Entity.UnRegister(gameObject);
        }
    }
}