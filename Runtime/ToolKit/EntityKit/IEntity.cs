// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-21  20:53
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

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
}