using System;
using JFramework.Core;
using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework
{
    /// <summary>
    /// 实体的抽象类
    /// </summary>
    [Serializable]
    public abstract class Entity : SerializedMonoBehaviour, IEntity,IUpdate
    {
        /// <summary>
        /// 实体生成
        /// </summary>
        /// <param name="value">传入生成参数</param>
        public virtual void Spawn(params object[] value) { }

        /// <summary>
        /// 实体更新
        /// </summary>
        protected virtual void OnUpdate() { }

        /// <summary>
        /// 实体销毁
        /// </summary>
        public virtual void Despawn() { }

        /// <summary>
        /// 实体启用
        /// </summary>
        protected virtual void OnEnable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.OnUpdate += OnUpdate;
        }

        /// <summary>
        /// 实体禁用
        /// </summary>
        protected virtual void OnDisable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.OnUpdate -= OnUpdate;
        }

        /// <summary>
        /// 实体销毁
        /// </summary>
        private void OnDestroy()
        {
            try
            {
                Despawn();
            }
            catch (Exception e)
            {
                GlobalManager.Logger(DebugOption.Custom, $"{name.Sky()} => {nameof(OnDestroy).Green()} 发生异常\n{e}");
            }
        }

        /// <summary>
        /// 实体接口调用实体更新方法
        /// </summary>
        void IUpdate.Update() => OnUpdate();
    }
}