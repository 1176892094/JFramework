namespace JFramework
{
    /// <summary>
    /// 单例的抽象类
    /// </summary>
    /// <typeparam name="T">传入单例的所有者</typeparam>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        /// <summary>
        /// 单例自身
        /// </summary>
        private static T instance;

        /// <summary>
        /// 单例的所有者
        /// </summary>
        public static T Instance => instance ??= new T();

        /// <summary>
        /// 单例初始化
        /// </summary>
        internal virtual void Awake() => instance ??= (T)this;

        /// <summary>
        /// 单例清除
        /// </summary>
        internal virtual void Destroy() => instance = default;
    }
}