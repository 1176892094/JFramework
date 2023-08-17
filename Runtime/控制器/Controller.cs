using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework
{
    /// <summary>
    /// 控制器的抽象类
    /// </summary>
    /// <typeparam name="TCharacter">实现了IEntity接口的类型</typeparam>
    public abstract class Controller<TCharacter> : SerializedScriptableObject, IController where TCharacter : ICharacter
    {
        /// <summary>
        /// 控制器的所有者
        /// </summary>
        public TCharacter owner;
        
        /// <summary>
        /// 控制器初始化
        /// </summary>
        /// <param name="owner">传入所有者</param>
        void IController.Spawn(ICharacter owner)
        {
            this.owner = (TCharacter)owner;
            if (this is ISpawn spawn)
            {
                spawn.Spawn();
            }
        }

        /// <summary>
        /// 控制器销毁
        /// </summary>
        private void OnDestroy()
        {
            owner = default;
        }
    }
}