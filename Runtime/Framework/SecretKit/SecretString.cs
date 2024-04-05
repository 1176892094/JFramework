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
using JFramework.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;


namespace JFramework
{
    [Serializable]
    public struct SecretString : IVariable
    {
        public string origin;
        public string buffer;
        public int offset;
        
        public string Value
        {
            get
            {
                if (origin.IsEmpty())
                {
                    return "";
                }
                var target = buffer.Remove(offset, 1);
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
                    offset = Random.Range(0, value.Length);
                    buffer = value.Insert(offset, "A");
                }
            }
        }

        public SecretString(string value)
        {
            origin = "";
            offset = 0;
            buffer = "";
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