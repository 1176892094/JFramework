namespace JFramework
{
    /// <summary>
    /// 调用单例后自动创建一个单例对象
    /// </summary>
    /// <typeparam name="T">传入单例的所有者</typeparam>
    public abstract class BaseSingleton<T> where T : BaseSingleton<T>, new()
    {
        /// <summary>
        /// 单例自身
        /// </summary>
        private static T instance;

        /// <summary>
        /// 实现安全的单例调用
        /// </summary>
        public static T Instance => instance ??= new T();

        /// <summary>
        /// 单例初始化
        /// </summary>
        protected virtual void Awake() => instance ??= (T)this;

        /// <summary>
        /// 销毁时将单例置空
        /// </summary>
        public virtual void Destroy() => instance = default;
    }
}