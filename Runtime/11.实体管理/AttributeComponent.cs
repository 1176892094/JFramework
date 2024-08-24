// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-25  01:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;

namespace JFramework
{
    [Serializable]
    public abstract class AttributeComponent<T1, T2> : Component<T1> where T1 : IEntity where T2 : Enum
    {
        private readonly Dictionary<T2, Variable<float>> attributes = new Dictionary<T2, Variable<float>>();

        public float Get(T2 key)
        {
            attributes.TryAdd(key, 0);
            return attributes[key];
        }

        public void Set(T2 key, float value)
        {
            attributes.TryAdd(key, 0);
            attributes[key] = value;
        }

        public void Add(T2 key, float value)
        {
            attributes.TryAdd(key, 0);
            attributes[key] += value;
        }

        public void Sub(T2 key, float value)
        {
            attributes.TryAdd(key, 0);
            attributes[key] -= value;
        }

        protected virtual void OnDestroy()
        {
            attributes.Clear();
        }
    }
}