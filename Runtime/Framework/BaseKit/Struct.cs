// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  13:35
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using JFramework.Interface;
using Sirenix.OdinInspector;

namespace JFramework
{
    [Serializable]
    internal struct Json
    {
        public string name;
        public byte[] key;
        public byte[] iv;

        public Json(string name)
        {
            this.name = name;
            iv = Array.Empty<byte>();
            key = Array.Empty<byte>();
        }

        public Json(string name, byte[] key, byte[] iv)
        {
            this.iv = iv;
            this.key = key;
            this.name = name;
        }

        public static implicit operator bool(Json data)
        {
            return data.key != null && data.key.Length != 0 && data.iv != null && data.iv.Length != 0;
        }
    }

    [Serializable]
    internal struct Asset
    {
        public string asset;
        public string bundle;

        public Asset(string path)
        {
            var split = path.Split('/');
            bundle = split[0].ToLower();
            asset = split[1].TrimEnd();
        }
    }

    [Serializable]
    internal struct Bundle
    {
        public string code;
        public string name;
        public string size;

        public Bundle(string code, string name, string size)
        {
            this.code = code;
            this.name = name;
            this.size = size;
        }

        public static bool operator ==(Bundle a, Bundle b) => a.code == b.code;

        public static bool operator !=(Bundle a, Bundle b) => !(a == b);

        private bool Equals(Bundle other) => size == other.size && code == other.code && name == other.name;

        public override bool Equals(object obj) => obj is Bundle other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(size, code, name);
    }
    
    [Serializable]
    internal struct Pool<T> : IPool<T>
    {
        [ShowInInspector] private Stack<T> pool;

        public int Count => pool.Count;

        public Pool(T obj)
        {
            pool = new Stack<T>();
            pool.Push(obj);
        }

        public T Pop()
        {
            return pool.TryPop(out var obj) ? obj : Activator.CreateInstance<T>();
        }

        public bool Push(T obj)
        {
            if (!pool.Contains(obj))
            {
                pool.Push(obj);
                return true;
            }

            return false;
        }

        public void Dispose() => pool.Clear();
    }
}