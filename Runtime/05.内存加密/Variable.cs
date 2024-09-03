// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-06-03  23:06
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework
{
    [Serializable]
    public struct Variable<T>
    {
        public T origin;
        public int buffer;
        public int offset;

        public T Value
        {
            get
            {
                if (offset == 0)
                {
                    this = new Variable<T>(origin);
                }

                if (buffer != unchecked(target + offset))
                {
                    GlobalManager.OnAntiCheat();
                    return default;
                }

                return origin;
            }
            set
            {
                origin = value;
                offset = UnityEngine.Random.Range(1, ushort.MaxValue);
                buffer = unchecked(target + offset);
            }
        }

        private int target => origin != null ? origin.GetHashCode() : 0;

        public Variable(T value = default)
        {
            offset = 0;
            buffer = 0;
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