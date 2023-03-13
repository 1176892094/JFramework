namespace JFramework
{
    /// <summary>
    /// 基于Mono的单例对象
    /// </summary>
    /// <typeparam name="T">所属的单例对象</typeparam>
    public abstract class MonoSingleton<T> : Entity where T : MonoSingleton<T>
    {
        /// <summary>
        /// 所属单例对象
        /// </summary>
        private static T instance;

        /// <summary>
        /// 返回单例对象
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = (T)FindObjectOfType(typeof(T));
                return instance;
            }
        }

        /// <summary>
        /// 醒来时在全局管理器中注册
        /// </summary>
        protected override void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
        }

        /// <summary>
        /// 释放时将单例置空
        /// </summary>
        protected override void OnDestroy()
        {
            instance = null;
        }
    }
}