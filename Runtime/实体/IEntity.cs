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
        /// 实体得到控制器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetController<T>() where T : ScriptableObject, IController;

        /// <summary>
        /// 实体增加控制器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddController<T>() where T : ScriptableObject, IController;
    }
}