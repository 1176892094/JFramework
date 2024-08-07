// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-06-03  23:06
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Core;
using Random = UnityEngine.Random;

namespace JFramework
{
    [Serializable]
    public struct Variable<T>
    {
        public T origin;
        public byte[] buffer;
        public int offset;

        public T Value
        {
            get
            {
                if (buffer == null)
                {
                    Value = origin;
                }
                else
                {
                    var target = new byte[buffer.Length];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        target[i] = (byte)(buffer[i] - offset);
                    }

                    if (!origin.Equals(target.Read<T>()))
                    {
                        GlobalManager.Cheat();
                    }
                }

                return origin;
            }
            set
            {
                buffer = value.Write();
                origin = buffer.Read<T>();
                offset = Random.Range(1, byte.MaxValue);
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = (byte)(buffer[i] + offset);
                }
            }
        }

        public Variable(T value = default)
        {
            offset = 0;
            buffer = null;
            origin = default;
            Value = value;
        }

        public static implicit operator T(Variable<T> secret)
        {
            return secret.Value;
        }

        public static implicit operator Variable<T>(T value)
        {
            return new Variable<T>(value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}