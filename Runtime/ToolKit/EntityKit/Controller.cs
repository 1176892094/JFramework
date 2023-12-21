// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-21  17:47
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 控制器
    /// </summary>
    public abstract class Controller : ScriptableObject
    {
        /// <summary>
        /// 所有者
        /// </summary>
        private GameObject instance;

        /// <summary>
        /// 所有者的游戏对象
        /// </summary>
        public GameObject owner => instance ??= GlobalManager.Entity.instance;
    }
}