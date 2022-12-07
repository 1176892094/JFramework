using System;
using System.Collections.Generic;

namespace JFramework
{
    public class EventManager : Singleton<EventManager>
    {
        private readonly Dictionary<string, IEventData> eventDict = new Dictionary<string, IEventData>();

        public void Listen<T, K, S>(string type, Action<T, K, S> action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T, K, S>)eventDict[type]).action += action;
            }
            else
            {
                eventDict.Add(type, new EventData<T, K, S>(action));
            }
        }

        public void Remove<T, K, S>(string type, Action<T, K, S> action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T, K, S>)eventDict[type]).action -= action;
            }
        }

        public void Send<T, K, S>(string type, T t, K k, S s)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T, K, S>)eventDict[type]).action?.Invoke(t, k, s);
            }
        }

        public void Listen<T, K>(string type, Action<T, K> action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T, K>)eventDict[type]).action += action;
            }
            else
            {
                eventDict.Add(type, new EventData<T, K>(action));
            }
        }

        public void Remove<T, K>(string type, Action<T, K> action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T, K>)eventDict[type]).action -= action;
            }
        }

        public void Send<T, K>(string type, T t, K k)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T, K>)eventDict[type]).action?.Invoke(t, k);
            }
        }

        public void Listen<T>(string type, Action<T> action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T>)eventDict[type]).action += action;
            }
            else
            {
                eventDict.Add(type, new EventData<T>(action));
            }
        }

        public void Remove<T>(string type, Action<T> action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T>)eventDict[type]).action -= action;
            }
        }

        public void Send<T>(string type, T t)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T>)eventDict[type]).action?.Invoke(t);
            }
        }
        
        public void Listen(string type, Action action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData)eventDict[type]).action += action;
            }
            else
            {
                eventDict.Add(type, new EventData(action));
            }
        }

        public void Remove(string type, Action action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData)eventDict[type]).action -= action;
            }
        }

        public void Send(string type)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData)eventDict[type]).action?.Invoke();
            }
        }
    }

    internal class EventData<T, K, S> : IEventData
    {
        public Action<T, K, S> action;
        public EventData(Action<T, K, S> action) => this.action += action;
    }

    internal class EventData<T, K> : IEventData
    {
        public Action<T, K> action;
        public EventData(Action<T, K> action) => this.action += action;
    }

    internal class EventData<T> : IEventData
    {
        public Action<T> action;
        public EventData(Action<T> action) => this.action += action;
    }

    internal class EventData : IEventData
    {
        public Action action;
        public EventData(Action action) => this.action += action;
    }

    internal interface IEventData
    {
    }
}