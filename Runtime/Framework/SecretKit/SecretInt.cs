// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-4-4  17:25
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Interface;
using Random = UnityEngine.Random;

namespace JFramework
{
    [Serializable]
    public struct SecretInt : IVariable
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
                    Secret.AntiCheat();
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
            return new SecretInt(1);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}