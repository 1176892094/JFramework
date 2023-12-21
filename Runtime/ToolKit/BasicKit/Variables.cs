// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-21  20:55
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 属性数值列表类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Variables<T> : IVariable where T : struct
    {
        /// <summary>
        /// 泛型列表数值
        /// </summary>
        public List<T> value;

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        public Variables()
        {
            value = new List<T>();
        }

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="value"></param>
        public Variables(List<T> value)
        {
            this.value = value;
        }
    }
}