using System;
using System.Collections.Generic;

namespace JFramework
{
    public static class EventManager
    {
        private static readonly Dictionary<string, IEventData> eventDict = new Dictionary<string, IEventData>();

        public static void AddEventListener<T1, T2>(string type, Action<T1, T2> action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T1, T2>)eventDict[type]).actionList += action;
            }
            else
            {
                eventDict.Add(type, new EventData<T1, T2>(action));
            }
        }

        public static void AddEventListener<T>(string type, Action<T> action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T>)eventDict[type]).actionList += action;
            }
            else
            {
                eventDict.Add(type, new EventData<T>(action));
            }
        }

        public static void AddEventListener(string type, Action action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData)eventDict[type]).actionList += action;
            }
            else
            {
                eventDict.Add(type, new EventData(action));
            }
        }

        public static void RemoveEventListener<T1, T2>(string type, Action<T1, T2> action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T1, T2>)eventDict[type]).actionList -= action;
            }
        }

        public static void RemoveEventListener<T>(string type, Action<T> action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T>)eventDict[type]).actionList -= action;
            }
        }

        public static void RemoveEventListener(string type, Action action)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData)eventDict[type]).actionList -= action;
            }
        }

        public static void OnEventTrigger<T1, T2>(string type, T1 data1, T2 data2)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T1, T2>)eventDict[type]).actionList?.Invoke(data1, data2);
            }
        }

        public static void OnEventTrigger<T>(string type, T data)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData<T>)eventDict[type]).actionList?.Invoke(data);
            }
        }

        public static void OnEventTrigger(string type)
        {
            if (eventDict.ContainsKey(type))
            {
                ((EventData)eventDict[type]).actionList?.Invoke();
            }
        }
    }

    public class EventData<T1, T2> : IEventData
    {
        public Action<T1, T2> actionList;
        public EventData(Action<T1, T2> action) => actionList += action;
    }

    public class EventData<T> : IEventData
    {
        public Action<T> actionList;
        public EventData(Action<T> action) => actionList += action;
    }

    public class EventData : IEventData
    {
        public Action actionList;
        public EventData(Action action) => actionList += action;
    }

    public interface IEventData
    {
    }
}