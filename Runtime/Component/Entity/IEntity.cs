using JFramework.Core;
using UnityEngine;

namespace JFramework.Interface
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 对象变换
        /// </summary>
        Transform transform { get; }

        /// <summary>
        /// 游戏对象
        /// </summary>
        GameObject gameObject { get; }
    }

    /// <summary>
    /// 更新接口
    /// </summary>
    public interface IUpdate : IEntity
    {
        /// <summary>
        /// 实体更新
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 侦听实体的更新事件
        /// </summary>
        void Listen() => GlobalManager.Listen(this);

        /// <summary>
        /// 移除实体的更新
        /// </summary>
        void Remove() => GlobalManager.Remove(this);
    }

    /// <summary>
    /// 角色接口
    /// </summary>
    public interface ICharacter : IEntity
    {
    }

    /// <summary>
    /// 注入接口 (根据子物体名称 查找 并注入值)
    /// </summary>
    public interface IInject : IEntity
    {
    }
}