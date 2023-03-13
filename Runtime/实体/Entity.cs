using System;
using JFramework.Interface;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// 实体的抽象类
    /// </summary>
    public abstract class Entity : MonoBehaviour, IEntity
    {
        /// <summary>
        /// 实体醒来
        /// </summary>
        protected virtual void Awake()
        {
        }

        /// <summary>
        /// 实体开始
        /// </summary>
        protected virtual void Start()
        {
        }

        /// <summary>
        /// 实体更新
        /// </summary>
        [Obsolete("[JFramework] 请使用OnUpdate来代替Update管理生命周期", true)]
        private void Update()
        {
        }

        /// <summary>
        /// 实体销毁
        /// </summary>
        protected virtual void OnDestroy()
        {
        }

        /// <summary>
        /// 实体启用
        /// </summary>
        private void OnEnable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Instance.Listen(this);
            Enable();
        }

        /// <summary>
        /// 实体禁用
        /// </summary>
        private void OnDisable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Instance.Remove(this);
            Disable();
        }

        /// <summary>
        /// 实体初始化
        /// </summary>
        /// <param name="args">初始化参数</param>
        protected virtual void OnInit(params object[] args)
        {
        }

        /// <summary>
        /// 实体生命周期
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        /// 自定义实体启用
        /// </summary>
        protected virtual void Enable()
        {
        }

        /// <summary>
        /// 自定义实体禁用
        /// </summary>
        protected virtual void Disable()
        {
        }

        /// <summary>
        /// 实体接口Id
        /// </summary>
        int IEntity.Id { get; set; }

        /// <summary>
        /// 实体接口初始阿虎
        /// </summary>
        /// <param name="args">传入的参数</param>
        void IEntity.OnInit(params object[] args) => OnInit(args);

        /// <summary>
        /// 实体接口更新
        /// </summary>
        void IEntity.OnUpdate() => OnUpdate();
    }
}