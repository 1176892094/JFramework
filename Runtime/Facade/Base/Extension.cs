using UnityEngine;

namespace JFramework.Basic
{
    public static class Extension
    {
        public static T GetSystem<T>(this IGetSystem self) where T : class, ISystem
        {
            return self.GetFacade().GetSystem<T>();
        }

        public static T GetModel<T>(this IGetModel self) where T : class, IModel
        {
            return self.GetFacade().GetModel<T>();
        }

        public static T GetExtend<T>(this IGetExtend self) where T : class, IExtend
        {
            return self.GetFacade().GetExtend<T>();
        }

        public static void SendCommand<T>(this ISendCommand self) where T : ICommand, new()
        {
            self.GetFacade().SendCommand<T>();
        }

        public static void SendCommand<T>(this ISendCommand self, T command) where T : ICommand
        {
            self.GetFacade().SendCommand(command);
        }

        public static TResult SendQuery<TResult>(this ISendQuery self, IQuery<TResult> query)
        {
            return self.GetFacade().SendQuery(query);
        }

        public static IEvent AutoDestroy(this IEvent e, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<EventObject>();
            if (!trigger)
            {
                trigger = gameObject.AddComponent<EventObject>();
            }

            trigger.Register(e);
            return e;
        }
    }
}