using System;
using JFramework.Core;

namespace JFramework.Interface
{
    /// <summary>
    /// 控制器接口
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// 控制器生成时调用
        /// </summary>
        /// <param name="owner">传入控制器的拥有者</param>
        /// <returns></returns>
        IController Spawn(IEntity owner);

        /// <summary>
        /// 控制器销毁
        /// </summary>
        void Despawn();
        
        /// <summary>
        /// 控制器销毁
        /// </summary>
        void Destroy()
        {
            try
            {
                Despawn();
            }
            catch (Exception e)
            {
                Log.Info(DebugOption.Custom, e.ToString());
            }
        }
    }
}