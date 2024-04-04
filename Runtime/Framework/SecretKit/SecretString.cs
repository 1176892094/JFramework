// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-4-4  17:46
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Text;
using JFramework.Interface;


namespace JFramework
{
    [Serializable]
    public struct SecretString : IVariable
    {
        public byte offset;
        public byte[] origin;
        public byte[] buffer;

        private bool Empty => origin == null || origin.Length == 0;

        private string Value
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

                return Encoding.UTF8.GetString(target);
            }
            set
            {
                origin = Encoding.UTF8.GetBytes(value);
                buffer = new byte[origin.Length];
                offset = (byte)Secret.Random(0, byte.MaxValue);
                Buffer.BlockCopy(origin, 0, buffer, 0, origin.Length);
                for (var i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = (byte)(buffer[i] + offset);
                }
            }
        }

        public SecretString(string value)
        {
            origin = Encoding.UTF8.GetBytes(value);
            buffer = new byte[origin.Length];
            offset = (byte)Secret.Random(0, byte.MaxValue);
            Buffer.BlockCopy(origin, 0, buffer, 0, origin.Length);
            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(buffer[i] + offset);
            }
        }

        public static implicit operator string(SecretString secret)
        {
            if (secret.Empty)
            {
                secret = new SecretString("");
            }

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