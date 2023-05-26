namespace JFramework
{
    /// <summary>
    /// 调用后会在场景中寻找相应的游戏对象
    /// </summary>
    /// <typeparam name="T">所属的单例对象</typeparam>
    public abstract class MonoSingleton<T> : Entity where T : MonoSingleton<T>
    {
        /// <summary>
        /// 单例自身
        /// </summary>
        private static T instance;

        /// <summary>
        /// 实现安全的单例调用
        /// </summary>
        public static T Instance => instance ??= FindObjectOfType<T>();

        /// <summary>
        /// 单例初始化
        /// </summary>
        protected virtual void Awake() => instance ??= (T)this;

        /// <summary>
        /// 释放时将单例置空
        /// </summary>
        public override void Despawn() => instance = null;
    }
}