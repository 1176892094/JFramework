using JFramework.Interface;

namespace JFramework
{
    public static class ControllerExtension
    {
        public static T As<T>(this IController controller) where T : IController
        {
            return (T)controller;
        }
    }
}