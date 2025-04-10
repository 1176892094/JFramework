// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:31
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.Text;
using JFramework.Common;
using UnityEngine;

namespace JFramework
{
    public abstract class Attribute<TOwner, T> : Agent<TOwner> where TOwner : Component
    {
        private readonly Dictionary<T, Variable<float>> attributes = new Dictionary<T, Variable<float>>();

        public int GetInt(T key)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, 0);
            }

            return (int)attributes[key].Value;
        }

        public void SetInt(T key, int value)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, 0);
            }

            attributes[key] = value;
        }

        public bool GetBool(T key)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, 0);
            }

            return attributes[key].Value > 0;
        }

        public void SetBool(T key, bool value)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, 0);
            }

            attributes[key] = value ? 1 : 0;
        }

        public float GetFloat(T key)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, 0);
            }

            return attributes[key].Value;
        }

        public void SetFloat(T key, float value)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, 0);
            }

            attributes[key] = value;
        }

        public override void OnHide()
        {
            base.OnHide();
            attributes.Clear();
        }

        public override string ToString()
        {
            var builder = HeapManager.Dequeue<StringBuilder>();
            foreach (var attribute in attributes)
            {
                builder.AppendFormat("{0} : {1}", attribute.Key, attribute.Value);
            }

            var message = builder.ToString();
            builder.Length = 0;
            HeapManager.Enqueue(builder);
            return message;
        }
    }
}