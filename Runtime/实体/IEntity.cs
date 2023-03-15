using UnityEngine;

namespace JFramework.Interface
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 实体接口生成方法
        /// </summary>
        /// <param name="value">可以传入任何参数进行生成</param>
        void Spawn(params object[] value);

        /// <summary>
        /// 实体的更新方法
        /// </summary>
        void Update();

        /// <summary>
        /// 控制器的获取方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>() where T : ScriptableObject, IController;
    }
}