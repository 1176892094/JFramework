// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-08-04  03:08
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace JFramework
{
    internal static class Serialization
    {
        private static readonly Dictionary<Type, Delegate> writers = new Dictionary<Type, Delegate>()
        {
            { typeof(int), new Func<int, byte[]>(Serialize) },
            { typeof(bool), new Func<bool, byte[]>(value => Serialize((byte)(value ? 1 : 0))) },
            { typeof(long), new Func<long, byte[]>(Serialize) },
            { typeof(float), new Func<float, byte[]>(Serialize) },
            { typeof(double), new Func<double, byte[]>(Serialize) },
            { typeof(string), new Func<string, byte[]>(value => Encoding.UTF8.GetBytes(value ?? string.Empty)) },
            { typeof(Vector2), new Func<Vector2, byte[]>(Serialize) },
            { typeof(Vector3), new Func<Vector3, byte[]>(Serialize) },
            { typeof(Vector2Int), new Func<Vector2Int, byte[]>(Serialize) },
            { typeof(Vector3Int), new Func<Vector3Int, byte[]>(Serialize) },
        };

        private static readonly Dictionary<Type, Delegate> readers = new Dictionary<Type, Delegate>()
        {
            { typeof(int), new Func<byte[], int>(Deserialize<int>) },
            { typeof(bool), new Func<byte[], bool>(value => Deserialize<byte>(value) != 0) },
            { typeof(long), new Func<byte[], long>(Deserialize<long>) },
            { typeof(float), new Func<byte[], float>(Deserialize<float>) },
            { typeof(double), new Func<byte[], double>(Deserialize<double>) },
            { typeof(string), new Func<byte[], string>(Encoding.UTF8.GetString) },
            { typeof(Vector2), new Func<byte[], Vector2>(Deserialize<Vector2>) },
            { typeof(Vector3), new Func<byte[], Vector3>(Deserialize<Vector3>) },
            { typeof(Vector2Int), new Func<byte[], Vector2Int>(Deserialize<Vector2Int>) },
            { typeof(Vector3Int), new Func<byte[], Vector3Int>(Deserialize<Vector3Int>) },
        };

        public static byte[] Write<T>(T value)
        {
            if (writers.TryGetValue(typeof(T), out var func))
            {
                return ((Func<T, byte[]>)func).Invoke(value);
            }

            return default;
        }

        public static T Read<T>(byte[] bytes)
        {
            if (readers.TryGetValue(typeof(T), out var func))
            {
                return ((Func<byte[], T>)func).Invoke(bytes);
            }

            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe byte[] Serialize<T>(T value) where T : unmanaged
        {
            var data = new byte[sizeof(T)];
            fixed (byte* ptr = &data[0])
            {
#if UNITY_ANDROID
                var buffer = stackalloc T[1] { value };
                UnsafeUtility.MemCpy(ptr, buffer, sizeof(T));
#else
                *(T*)ptr = value;
#endif
            }

            return data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe T Deserialize<T>(byte[] data) where T : unmanaged
        {
            T value;
            fixed (byte* ptr = &data[0])
            {
#if UNITY_ANDROID
                var buffer = stackalloc T[1];
                UnsafeUtility.MemCpy(buffer, ptr, sizeof(T));
                value = buffer[0];
#else
                value = *(T*)ptr;
#endif
            }

            return value;
        }
    }
}