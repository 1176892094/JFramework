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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;

namespace JFramework
{
    public static partial class Extensions
    {
        private static readonly Dictionary<Type, Delegate> writers = new Dictionary<Type, Delegate>()
        {
            { typeof(byte), new Func<byte, byte[]>(Serialize) },
            { typeof(sbyte), new Func<sbyte, byte[]>(Serialize) },
            { typeof(char), new Func<char, byte[]>(value => Serialize((ushort)value)) },
            { typeof(short), new Func<short, byte[]>(Serialize) },
            { typeof(ushort), new Func<ushort, byte[]>(Serialize) },
            { typeof(int), new Func<int, byte[]>(Serialize) },
            { typeof(uint), new Func<uint, byte[]>(Serialize) },
            { typeof(bool), new Func<bool, byte[]>(value => Serialize((byte)(value ? 1 : 0))) },
            { typeof(long), new Func<long, byte[]>(Serialize) },
            { typeof(ulong), new Func<ulong, byte[]>(Serialize) },
            { typeof(float), new Func<float, byte[]>(Serialize) },
            { typeof(double), new Func<double, byte[]>(Serialize) },
            { typeof(string), new Func<string, byte[]>(value => Encoding.UTF8.GetBytes(value ?? string.Empty)) },
        };

        private static readonly Dictionary<Type, Delegate> readers = new Dictionary<Type, Delegate>()
        {
            { typeof(byte), new Func<byte[], byte>(Deserialize<byte>) },
            { typeof(sbyte), new Func<byte[], sbyte>(Deserialize<sbyte>) },
            { typeof(char), new Func<byte[], char>(value => (char)Deserialize<ushort>(value)) },
            { typeof(short), new Func<byte[], short>(Deserialize<short>) },
            { typeof(ushort), new Func<byte[], ushort>(Deserialize<ushort>) },
            { typeof(int), new Func<byte[], int>(Deserialize<int>) },
            { typeof(uint), new Func<byte[], uint>(Deserialize<uint>) },
            { typeof(bool), new Func<byte[], bool>(value => Deserialize<byte>(value) != 0) },
            { typeof(long), new Func<byte[], long>(Deserialize<long>) },
            { typeof(ulong), new Func<byte[], ulong>(Deserialize<ulong>) },
            { typeof(float), new Func<byte[], float>(Deserialize<float>) },
            { typeof(double), new Func<byte[], double>(Deserialize<double>) },
            { typeof(string), new Func<byte[], string>(Encoding.UTF8.GetString) },
        };

        internal static byte[] Write<T>(this T value)
        {
            if (writers.TryGetValue(typeof(T), out var func))
            {
                return ((Func<T, byte[]>)func).Invoke(value);
            }

            return default;
        }

        internal static T Read<T>(this byte[] bytes)
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