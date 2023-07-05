using System;
using JFramework.Core;

namespace JFramework.Interface
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 实体更新
        /// </summary>
        void Update();

        /// <summary>
        /// 实体销毁
        /// </summary>
        void Despawn();

        /// <summary>
        /// 实体启用
        /// </summary>
        void Enable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.OnUpdate += Update;
        }

        /// <summary>
        /// 实体禁用
        /// </summary>
        void Disable()
        {
            if (!GlobalManager.Runtime) return;
            GlobalManager.OnUpdate -= Update;
        }

        /// <summary>
        /// 实体销毁
        /// </summary>
        void Destroy()
        {
            try
            {
                Despawn();
            }
            catch (Exception e)
            {
                Log.Info(DebugOption.Custom, e.ToString());
            }
        }
    }

    /// <summary>
    /// 泛型实体接口
    /// </summary>
    /// <typeparam name="T">物体生成事件</typeparam>
    public interface IEntity<in T> : IEntity where T : IEvent
    {
        /// <summary>
        /// 实体生成
        /// </summary>
        /// <param name="data"></param>
        void Spawn(T data);
    }
}