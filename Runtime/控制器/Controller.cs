using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 控制器的抽象类
    /// </summary>
    /// <typeparam name="T">实现了IEntity接口的类型</typeparam>
    public abstract class Controller<T> : ScriptableObject,IController where T : IEntity
    {
        /// <summary>
        /// 控制器的所有者
        /// </summary>
        protected T owner;

        /// <summary>
        /// 控制器初始化
        /// </summary>
        /// <param name="owner">传入控制器的所有者</param>
        protected virtual void OnInit(T owner) => this.owner = owner;

        void IController.OnInit(object owner) => OnInit((T)owner);
    }

    public static class ControllerExtension
    {
        public static T Get<T>(this IController controller) where T : IController
        {
            return (T)controller;
        }
    }
}