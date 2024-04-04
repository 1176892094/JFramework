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


namespace JFramework
{
    [Serializable]
    public struct SecretFloat : IVariable
    {
        public byte offset;
        public byte[] origin;
        public byte[] buffer;

        private bool Empty => origin == null || origin.Length == 0;

        private float Value
        {
            get
            {
                var target = new byte[buffer.Length];
                Buffer.BlockCopy(buffer, 0, target, 0, target.Length);
                for (var i = 0; i < target.Length; i++)
                {
                    target[i] = (byte)(target[i] - offset);
                }

                if (!Secret.IsEquals(origin, target))
                {
                    Secret.AntiCheat();
                    return default;
                }

                return BitConverter.ToSingle(target);
            }
            set
            {
                origin = BitConverter.GetBytes(value);
                buffer = new byte[origin.Length];
                offset = (byte)Secret.Random(0, byte.MaxValue);
                Buffer.BlockCopy(origin, 0, buffer, 0, origin.Length);
                for (var i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = (byte)(buffer[i] + offset);
                }
            }
        }

        public SecretFloat(float value)
        {
            origin = BitConverter.GetBytes(value);
            buffer = new byte[origin.Length];
            offset = (byte)Secret.Random(0, byte.MaxValue);
            Buffer.BlockCopy(origin, 0, buffer, 0, origin.Length);
            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(buffer[i] + offset);
            }
        }

        public static implicit operator float(SecretFloat secret)
        {
            if (secret.Empty)
            {
                secret = new SecretFloat(default);
            }

            return secret.Value;
        }

        public static implicit operator SecretFloat(float value)
        {
            return new SecretFloat(value);
        }

        public static implicit operator string(SecretFloat secret)
        {
            return secret.Value.ToString("F");
        }

        public override string ToString()
        {
            return Value.ToString("F");
        }
    }
}