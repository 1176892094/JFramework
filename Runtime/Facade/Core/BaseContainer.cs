using System;
using System.Collections.Generic;

namespace JFramework.Basic
{
    public class BaseContainer
    {
        private readonly Dictionary<Type, object> dataDict = new Dictionary<Type, object>();

        public void Register<T>(T data)
        {
            var key = typeof(T);

            if (dataDict.ContainsKey(key))
            {
                dataDict[key] = data;
            }
            else
            {
                dataDict.Add(key, data);
            }
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);

            if (dataDict.TryGetValue(key, out var data))
            {
                return data as T;
            }

            return null;
        }
    }
}