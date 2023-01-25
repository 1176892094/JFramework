using UnityEngine;

namespace JFramework.Interface
{
    /// <summary>
    /// 事件变量接口
    /// </summary>
    public interface IEventValue
    {
        /// <summary>
        /// 设置事件变量绑定的游戏对象
        /// </summary>
        /// <param name="target">传入绑定的游戏对象，事件会随着对象销毁而释放</param>
        void SetTarget(GameObject target);

        /// <summary>
        /// 释放事件变量
        /// </summary>
        void Dispose();
    }
}