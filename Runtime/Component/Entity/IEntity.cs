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
    /// 注入接口 (根据子物体名称 查找 并注入值)
    /// </summary>
    public interface IInject : IEntity
    {
    }

    /// <summary>
    /// 角色接口
    /// </summary>
    public interface ICharacter : IEntity
    {
    }
}