using System;

namespace JFramework.Basic
{
    public class BaseManager<T> : BaseBehaviour where T : BaseManager<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = (T)FindObjectOfType(typeof(T));
                return instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        protected override void OnUpdate()
        {
        }
    }
}