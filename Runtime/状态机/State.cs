using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 状态的抽象类
    /// </summary>
    public abstract class State<T> : IState where T : IEntity
    {
        /// <summary>
        /// 状态的所有者
        /// </summary>
        protected T owner;

        /// <summary>
        /// 状态的初始化
        /// </summary>
        /// <param name="owner">传入状态的所有者</param>
        protected virtual void OnInit(T owner) => this.owner = owner;

        /// <summary>
        /// 进入该状态的方法
        /// </summary>
        protected abstract void OnEnter();

        /// <summary>
        /// 更新该状态的方法
        /// </summary>
        protected abstract void OnUpdate();

        /// <summary>
        /// 退出该状态的方法
        /// </summary>
        protected abstract void OnExit();

        /// <summary>
        /// 通过接口指向状态的初始化方法
        /// </summary>
        void IState.OnInit(object owner) => OnInit((T)owner);

        /// <summary>
        /// 通过接口指向状态的进入方法
        /// </summary>
        void IState.OnEnter() => OnEnter();

        /// <summary>
        /// 通过接口指向状态的更新方法
        /// </summary>
        void IState.OnUpdate() => OnUpdate();

        /// <summary>
        /// 通过接口指向状态的退出方法
        /// </summary>
        void IState.OnExit() => OnExit();
    }
}