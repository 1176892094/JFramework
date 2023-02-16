using JFramework.Interface;

namespace JFramework
{
    public static class EntityExtension
    {
        public static T As<T>(this IEntity entity) where T : IEntity
        {
            return (T)entity;
        }
    }
}