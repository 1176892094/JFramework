// *********************************************************************************
// # Project: Astraia
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 21:01:21
// # Recently: 2025-01-11 18:01:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Runtime.CompilerServices;
using Astraia.Common;

// ReSharper disable All

namespace Astraia
{
    public static class Setter<T>
    {
        public static Action<MemorySetter, T> setter;
    }

    public class MemorySetter : IDisposable
    {
        public byte[] buffer = new byte[1500];
        public int position;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Set<T>(T value) where T : unmanaged
        {
            Resize(position + sizeof(T));
            fixed (byte* ptr = &buffer[position])
            {
                *(T*)ptr = value;
            }

            position += sizeof(T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Setable<T>(T? value) where T : unmanaged
        {
            if (!value.HasValue)
            {
                Set((byte)0);
                return;
            }

            Set((byte)1);
            Set(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke<T>(T value)
        {
            Setter<T>.setter?.Invoke(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            position = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MemorySetter Pop()
        {
            var setter = HeapManager.Dequeue<MemorySetter>();
            setter.Reset();
            return setter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Push(MemorySetter setter)
        {
            HeapManager.Enqueue(setter);
        }

        public override string ToString()
        {
            return BitConverter.ToString(buffer, 0, position);
        }
        
        void IDisposable.Dispose()
        {
            HeapManager.Enqueue(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resize(int length)
        {
            if (buffer.Length < length)
            {
                Array.Resize(ref buffer, Math.Max(length, buffer.Length * 2));
            }
        }

        public void SetBytes(byte[] segment, int offset, int count)
        {
            Resize(position + count);
            Buffer.BlockCopy(segment, offset, buffer, position, count);
            position += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ArraySegment<byte>(MemorySetter setter)
        {
            return new ArraySegment<byte>(setter.buffer, 0, setter.position);
        }
    }
}