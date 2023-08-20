using JFramework.Interface;
using UnityEngine;

// ReSharper disable All

namespace JFramework
{
    /// <summary>
    /// 控制器的抽象类
    /// </summary>
    /// <typeparam name="T">实现了ICharacter接口的类型</typeparam>
    public abstract class Controller<T> : ScriptableObject, IController where T : ICharacter
    {
        /// <summary>
        /// 控制器的所有者
        /// </summary>
        public T owner;

        /// <summary>
        /// 控制器注册角色
        /// </summary>
        /// <param name="owner"></param>
        void IController.Register(ICharacter owner)
        {
            this.owner = (T)owner;
            (this as IRegister)?.Register();
        }
    }
}