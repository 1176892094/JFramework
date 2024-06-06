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
using UnityEngine;

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
            var index = path.LastIndexOf('/');
            if (index < 0)
            {
                bundle = "";
                asset = path;
                return;
            }

            bundle = path.Substring(0, index).ToLower();
            asset = path.Substring(index + 1);
        }

        public override string ToString()
        {
            return $"资源包：{bundle} 资源名称：{asset}";
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

        public static bool operator !=(Bundle a, Bundle b) => a.code != b.code;

        private bool Equals(Bundle other) => size == other.size && code == other.code && name == other.name;

        public override bool Equals(object obj) => obj is Bundle other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(size, code, name);
    }

    [Serializable]
    internal struct Pool<T> : IPool<T>
    {
        public List<T> pool;

        public int count => pool.Count;

        public Pool(T obj) => pool = new List<T>() { obj };

        public T Pop()
        {
            if (count > 0)
            {
                var obj = pool[0];
                pool.Remove(obj);
                return obj;
            }

            return Activator.CreateInstance<T>();
        }

        public bool Push(T obj)
        {
            if (!pool.Contains(obj))
            {
                pool.Add(obj);
                return true;
            }

            return false;
        }

        public void Dispose() => pool.Clear();
    }
}