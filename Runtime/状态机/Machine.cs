using System.Collections.Generic;
using JFramework.Core;
using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 状态机的抽象类
    /// </summary>
    public abstract class Machine : Entity, IMachine
    {
        /// <summary>
        /// 存储状态的字典
        /// </summary>
        private Dictionary<string, IState> stateDict;

        /// <summary>
        /// 状态的接口
        /// </summary>
        private IState state;

        /// <summary>
        /// 醒来初始化字典
        /// </summary>
        protected override void Awake()
        {
            stateDict = new Dictionary<string, IState>();
        }

        /// <summary>
        /// 在OnUpdate中进行状态更新
        /// </summary>
        protected override void OnUpdate() => state?.OnUpdate();

        /// <summary>
        /// 添加状态机的状态
        /// </summary>
        /// <param name="name">状态名称</param>
        /// <param name="state">状态</param>
        public void ListenState(string name, IState state)
        {
            if (!stateDict.ContainsKey(name))
            {
                stateDict.Add(name, state);
                state.OnInit(this);
            }
        }

        /// <summary>
        /// 切换状态机的状态
        /// </summary>
        /// <param name="name">状态名称</param>
        public void ChangeState(string name)
        {
            state?.OnExit();
            state = stateDict[name];
            state?.OnEnter();
        }

        /// <summary>
        /// 延迟切换状态机的状态
        /// </summary>
        /// <param name="name">状态名称</param>
        /// <param name="time">延迟时间</param>
        public void ChangeState(string name, float time)
        {
            TimerManager.Instance.Listen(time, () => { ChangeState(name); }).SetTarget(gameObject);
        }

        /// <summary>
        /// 移除状态机的状态
        /// </summary>
        /// <param name="name">状态名称</param>
        public void RemoveState(string name)
        {
            if (stateDict.ContainsKey(name))
            {
                stateDict.Remove(name);
            }
        }
    }
}