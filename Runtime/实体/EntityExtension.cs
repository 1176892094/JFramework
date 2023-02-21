using JFramework.Interface;

namespace JFramework
{
    /// <summary>
    /// 实体拓展
    /// </summary>
    public static class EntityExtension
    {
        /// <summary>
        /// 转化为对应的实体
        /// </summary>
        /// <param name="entity">当前实体对象</param>
        /// <typeparam name="T">目标实体对象</typeparam>
        /// <returns>返回目标实体对象</returns>
        public static T As<T>(this IEntity entity) where T : IEntity => (T)entity;
    }
}