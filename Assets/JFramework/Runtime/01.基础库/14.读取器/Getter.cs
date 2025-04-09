// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 21:01:21
// # Recently: 2025-01-11 18:01:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Runtime.CompilerServices;
using JFramework.Common;

// ReSharper disable All

namespace JFramework
{
    public static class Getter<T>
    {
        public static Func<MemoryGetter, T> getter;
    }

    public class MemoryGetter : IDisposable
    {
        public ArraySegment<byte> buffer;
        public int position;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe T Get<T>() where T : unmanaged
        {
            T value;
            fixed (byte* ptr = &buffer.Array[buffer.Offset + position])
            {
                value = *(T*)ptr;
            }

            position += sizeof(T);
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? Getable<T>() where T : unmanaged
        {
            return Get<byte>() != 0 ? Get<T>() : default(T?);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Invoke<T>()
        {
            return Getter<T>.getter != null ? Getter<T>.getter.Invoke(this) : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset(ArraySegment<byte> segment)
        {
            buffer = segment;
            position = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MemoryGetter Pop(ArraySegment<byte> segment)
        {
            var getter = PoolManager.Dequeue<MemoryGetter>();
            getter.Reset(segment);
            return getter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(MemoryGetter getter)
        {
            PoolManager.Enqueue(getter);
        }

        public override string ToString()
        {
            return BitConverter.ToString(buffer.Array, buffer.Offset, buffer.Count);
        }
        
        void IDisposable.Dispose()
        {
            PoolManager.Enqueue(this);
        }
        
        public byte[] GetBytes(byte[] bytes, int count)
        {
            if (buffer.Count - position < count)
            {
                throw new OverflowException("读取器剩余容量不够!");
            }

            Buffer.BlockCopy(buffer.Array, buffer.Offset + position, bytes, 0, count);
            position += count;
            return bytes;
        }
        
        public ArraySegment<byte> GetArraySegment(int count)
        {
            if (buffer.Count - position < count)
            {
                throw new OverflowException("读取器剩余容量不够!");
            }

            var segment = new ArraySegment<byte>(buffer.Array, buffer.Offset + position, count);
            position += count;
            return segment;
        }
    }
}