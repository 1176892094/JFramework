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
using System.Collections.Generic;

namespace JFramework
{
    [Serializable]
    public struct Variable<T> : IEquatable<Variable<T>>
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
                    Value = origin;
                }

                if (buffer != unchecked(origin.GetHashCode() + offset))
                {
                    EventManager.Invoke<OnAntiCheatEvent>();
                }

                return origin;
            }
            set
            {
                origin = value;
                origin ??= (T)(object)string.Empty;
                offset = Random.Range(1, ushort.MaxValue);
                buffer = unchecked(origin.GetHashCode() + offset);
            }
        }

        public Variable(T value = default)
        {
            origin = value;
            origin ??= (T)(object)string.Empty;
            offset = Random.Range(1, ushort.MaxValue);
            buffer = unchecked(origin.GetHashCode() + offset);
        }

        public static implicit operator T(Variable<T> secret)
        {
            return secret.Value;
        }

        public static implicit operator Variable<T>(T value)
        {
            return new Variable<T>(value);
        }
        
        public bool Equals(Variable<T> other)
        {
            return EqualityComparer<T>.Default.Equals(origin, other.origin) && buffer == other.buffer && offset == other.offset;
        }

        public override bool Equals(object obj)
        {
            return obj is Variable<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(origin, buffer, offset);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}