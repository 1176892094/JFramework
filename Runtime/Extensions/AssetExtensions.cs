// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  17:34
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// ScriptableAsset拓展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 拓展 ScriptableObject 能够被持久化保存
        /// </summary>
        /// <param name="obj">传入ScriptableObject</param>
        public static void Save(this ScriptableObject obj)
        {
            if (obj.name.IsEmpty())
            {
                obj.name = obj.GetType().Name;
            }

            GlobalManager.Json.Save(obj, obj.name);
        }

        /// <summary>
        /// 拓展 ScriptableObject 能够被持久化加载
        /// </summary>
        /// <param name="obj">传入ScriptableObject</param>
        public static void Load(this ScriptableObject obj)
        {
            if (obj.name.IsEmpty())
            {
                obj.name = obj.GetType().Name;
            }

            GlobalManager.Json.Load(obj);
        }

        /// <summary>
        /// 拓展 ScriptableObject 能够被持久化保存并加密
        /// </summary>
        /// <param name="obj">传入ScriptableObject</param>
        public static void Encrypt(this ScriptableObject obj)
        {
            if (obj.name.IsEmpty())
            {
                obj.name = obj.GetType().Name;
            }

            GlobalManager.Json.Encrypt(obj, obj.name);
        }

        /// <summary>
        /// 拓展 ScriptableObject 能够被持久化加载并解密
        /// </summary>
        /// <param name="obj">传入ScriptableObject</param>
        public static void Decrypt(this ScriptableObject obj)
        {
            if (obj.name.IsEmpty())
            {
                obj.name = obj.GetType().Name;
            }

            GlobalManager.Json.Decrypt(obj);
        }
    }
}