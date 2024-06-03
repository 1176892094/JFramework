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
using System.Text;
using JFramework.Core;
using Random = UnityEngine.Random;

namespace JFramework
{
    [Serializable]
    public struct SecretInt
    {
        public int origin;
        public int buffer;
        public int offset;

        public int Value
        {
            get
            {
                if (offset == 0)
                {
                    this = new SecretInt(0);
                }

                var target = buffer - offset;
                if (!origin.Equals(target))
                {
                    GlobalManager.Cheat();
                }

                return target;
            }
            set
            {
                origin = value;
                unchecked
                {
                    offset = Random.Range(1, int.MaxValue - value);
                    buffer = value + offset;
                }
            }
        }

        public SecretInt(int value)
        {
            origin = 0;
            buffer = 0;
            offset = 0;
            Value = value;
        }

        public static implicit operator int(SecretInt secret)
        {
            return secret.Value;
        }

        public static implicit operator SecretInt(int value)
        {
            return new SecretInt(value);
        }

        public static implicit operator bool(SecretInt secret)
        {
            return secret.Value != 0;
        }

        public static implicit operator SecretInt(bool secret)
        {
            return new SecretInt(secret ? 1 : 0);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [Serializable]
    public struct SecretFloat
    {
        public float origin;
        public float buffer;
        public int offset;

        public float Value
        {
            get
            {
                if (offset == 0)
                {
                    this = new SecretFloat(0);
                }

                var target = buffer - offset;
                if (Math.Abs(origin - target) > 0.1f)
                {
                    GlobalManager.Cheat();
                }

                return target;
            }
            set
            {
                origin = value;
                unchecked
                {
                    offset = Random.Range(1, short.MaxValue - (int)value);
                    buffer = value + offset;
                }
            }
        }

        public SecretFloat(float value)
        {
            origin = 0;
            buffer = 0;
            offset = 0;
            Value = value;
        }

        public static implicit operator float(SecretFloat secret)
        {
            return secret.Value;
        }

        public static implicit operator SecretFloat(float value)
        {
            return new SecretFloat(value);
        }

        public override string ToString()
        {
            return Value.ToString("F");
        }
    }

    [Serializable]
    public struct SecretString
    {
        public string origin;
        public byte[] buffer;
        public int offset;

        public string Value
        {
            get
            {
                if (origin.IsEmpty())
                {
                    this = new SecretString("");
                }

                var target = new byte[buffer.Length];
                for (int i = 0; i < buffer.Length; i++)
                {
                    target[i] = (byte)(buffer[i] - offset);
                }

                if (!origin.Equals(Encoding.UTF8.GetString(target)))
                {
                    GlobalManager.Cheat();
                }

                return origin;
            }
            set
            {
                origin = value;
                offset = Random.Range(0, byte.MaxValue);
                buffer = Encoding.UTF8.GetBytes(origin);
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = (byte)(buffer[i] + offset);
                }
            }
        }

        public SecretString(string value)
        {
            origin = "";
            buffer = null;
            offset = 0;
            Value = value;
        }

        public static implicit operator string(SecretString secret)
        {
            return secret.Value;
        }

        public static implicit operator SecretString(string value)
        {
            return new SecretString(value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}