// // *********************************************************************************
// // # Project: Astraia
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 21:04:10
// // # Recently: 2025-04-09 21:04:10
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System;
using System.Buffers;

namespace Astraia
{
    public static partial class Service
    {
        public static class Xor
        {
            private const int STACKALLOC_SIZE = 256 * 1024;
            private const int LENGTH = 16;

            public static unsafe byte[] Encrypt(ReadOnlySpan<byte> data)
            {
                Span<byte> key = new byte[LENGTH];
                Random.Next(key);

                byte[] buffer = null;

                byte[] result;
                try
                {
                    var length = data.Length + LENGTH;

                    var ms = length > STACKALLOC_SIZE ? buffer = ArrayPool<byte>.Shared.Rent(length) : stackalloc byte[length];
                    key.CopyTo(ms);

                    var destination = ms.Slice(LENGTH);
                    for (var i = 0; i < data.Length; ++i)
                        destination[i] = (byte)(data[i] ^ key[i % key.Length]);

                    result = ms.ToArray();
                }
                finally
                {
                    if (buffer != null)
                        ArrayPool<byte>.Shared.Return(buffer);
                }

                return result;
            }

            public static unsafe byte[] Decrypt(ReadOnlySpan<byte> data)
            {
                Span<byte> key = stackalloc byte[LENGTH];
                data.Slice(0, key.Length).CopyTo(key);

                byte[] buffer = null;

                byte[] result;

                try
                {
                    var length = data.Length - LENGTH;
                    var ms = length > STACKALLOC_SIZE ? buffer = ArrayPool<byte>.Shared.Rent(length) : stackalloc byte[length];

                    data = data.Slice(LENGTH);
                    for (var i = 0; i < data.Length; ++i)
                        ms[i] = (byte)(data[i] ^ key[i % key.Length]);

                    result = ms.ToArray();
                }
                finally
                {
                    if (buffer != null)
                        ArrayPool<byte>.Shared.Return(buffer);
                }

                return result;
            }
        }
    }
}