using System;
using UnityEngine;

namespace JFramework
{
    /// <summary>
    /// Mono生命周期控制器
    /// </summary>
    internal class GlobalController : MonoBehaviour
    {
        /// <summary>
        /// Update更新的事件
        /// </summary>
        private event Action UpdateAction;

        /// <summary>
        /// 设置加载场景不销毁
        /// </summary>
        private void Awake() => DontDestroyOnLoad(gameObject);

        /// <summary>
        /// 侦听添加到事件中的OnUpdate的方法
        /// </summary>
        private void Update() => UpdateAction?.Invoke();

        /// <summary>
        /// 在Update中侦听事件的方法
        /// </summary>
        /// <param name="action">传入的OnUpdate方法</param>
        public void Listen(Action action) => UpdateAction += action;

        /// <summary>
        /// 在Update中移除事件的方法
        /// </summary>
        /// <param name="action">传入的OnUpdate方法</param>
        public void Remove(Action action) => UpdateAction -= action;
    }
}