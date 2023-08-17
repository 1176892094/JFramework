using JFramework.Core;

namespace JFramework.Interface
{
    /// <summary>
    /// 更新接口
    /// </summary>
    public interface IUpdate : IEntity
    {
        /// <summary>
        /// 实体更新
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 侦听实体的更新事件
        /// </summary>
        void Listen()
        {
            if (!GlobalManager.Runtime) return;
            GlobalManager.OnUpdate += OnUpdate;
        }

        /// <summary>
        /// 移除实体的更新
        /// </summary>
        void Remove()
        {
            if (!GlobalManager.Runtime) return;
            GlobalManager.OnUpdate -= OnUpdate;
        }
    }
}