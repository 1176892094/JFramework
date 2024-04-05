// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-4-4  17:24
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using JFramework.Interface;
using UnityEngine;
using Random = UnityEngine.Random;


namespace JFramework
{
    [Serializable]
    public struct SecretFloat : IVariable
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
                    Secret.AntiCheat();
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
                    Debug.LogWarning(value + " " + buffer + "  " + offset);
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
}