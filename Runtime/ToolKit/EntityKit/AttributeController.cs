// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-12-21  23:05
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-11-15  18:31
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework
{
    /// <summary>
    /// 属性控制器
    /// </summary>
    /// <typeparam name="TAttribute">持有属性枚举</typeparam>
    public abstract class AttributeController<TAttribute> : Controller where TAttribute : Enum
    {
        /// <summary>
        /// 存储 实体所持有的属性
        /// </summary>
        [ShowInInspector] private readonly Dictionary<TAttribute, IVariable> attributes = new Dictionary<TAttribute, IVariable>();

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public TValue Get<TValue>(TAttribute key) where TValue : struct
        {
            if (!attributes.TryGetValue(key, out var variable))
            {
                variable = new Variable<TValue>();
                attributes.Add(key, variable);
            }

            return ((Variable<TValue>)variable).value;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TValue"></typeparam>
        public void Set<TValue>(TAttribute key, TValue value) where TValue : struct
        {
            if (!attributes.TryGetValue(key, out var variable))
            {
                variable = new Variable<TValue>();
                attributes.Add(key, variable);
            }

            ((Variable<TValue>)variable).value = value;
        }
    }
}