using System;
using System.Collections.Generic;
using System.Linq;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    public static partial class Extensions
    {
        /// <summary>
        /// 将当前枚举切换到下一枚举
        /// 若当前为最后一个，则循环至第一个
        /// </summary>
        /// <param name="current">当前枚举的拓展</param>
        /// <typeparam name="T">传入任何枚举类型</typeparam>
        /// <returns>返回当前枚举的下一个值</returns>
        public static T ToNext<T>(this T current) where T : Enum
        {
            var enumArray = (T[])Enum.GetValues(typeof(T));
            var currIndex = Array.IndexOf(enumArray, current);
            var nextIndex = (currIndex + 1) % enumArray.Length;
            return enumArray[nextIndex];
        }

        /// <summary>
        /// 将当前枚举切换到上一枚举
        /// 若当前为第一个，则循环至最后一个
        /// </summary>
        /// <param name="current">当前枚举的拓展</param>
        /// <typeparam name="T">传入任何枚举类型</typeparam>
        /// <returns>返回当前枚举的上一个值</returns>
        public static T ToLast<T>(this T current) where T : Enum
        {
            var enumArray = (T[])Enum.GetValues(typeof(T));
            var currIndex = Array.IndexOf(enumArray, current);
            var lastIndex = (currIndex - 1 + enumArray.Length) % enumArray.Length;
            return enumArray[lastIndex];
        }
        
        /// <summary>
        /// 对哈希表进行基于栈的拓展
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="func"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Pop<T>(this HashSet<T> objects, Func<T> func = null)
        {
            T @object = default;
            if (objects.Count > 0)
            {
                @object = objects.First();
                objects.Remove(@object);
            }
            else
            {
                if (func != null)
                {
                    @object = func();
                }
            }

            return @object;
        }
        
        /// <summary>
        /// 继承ICharacter后可以使用 GetOrAddCtrl
        /// </summary>
        /// <param name="character"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddCtrl<T>(this ICharacter character) where T : ScriptableObject, IController
        {
            return CtrlManager.GetOrAddCtrl<T>(character.entityId);
        }

        /// <summary>
        /// 继承ICharacter后可以使用 Dispose
        /// </summary>
        /// <param name="character"></param>
        public static void Destroy(this ICharacter character)
        {
            CtrlManager.Destroy(character.entityId);
        }
    }
}