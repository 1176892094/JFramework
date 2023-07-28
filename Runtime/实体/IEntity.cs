using UnityEngine;

namespace JFramework.Interface
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 实体Id
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 实体变换
        /// </summary>
        Transform transform { get; }

        /// <summary>
        /// 实体对象
        /// </summary>
        GameObject gameObject { get; }

        /// <summary>
        /// 实体更新
        /// </summary>
        void Update();
    }
    
    public interface ICharacter : IEntity
    {
    }
}