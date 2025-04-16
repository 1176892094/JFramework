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
using System.IO;

namespace Astraia
{
    public static partial class Service
    {
        public static class Xor
        {
            private const int LENGTH = 16;

            public static unsafe byte[] Encrypt(byte[] data)
            {
                var key = new byte[LENGTH];
                Random.Next(key);

                using var ms = new MemoryStream();
                ms.Write(key, 0, key.Length);

                var buffer = new byte[1024];

                fixed (byte* pData = data, pKey = key, pBuffer = buffer)
                {
                    for (var i = 0; i < data.Length; i += buffer.Length)
                    {
                        var length = Math.Min(buffer.Length, data.Length - i);

                        for (var j = 0; j < length; j++)
                        {
                            pBuffer[j] = (byte)(pData[i + j] ^ pKey[(i + j) % key.Length]);
                        }

                        ms.Write(buffer, 0, length);
                    }
                }
                
                return ms.ToArray();
            }

            public static unsafe byte[] Decrypt(byte[] data)
            {
                var key = new byte[LENGTH];
                Buffer.BlockCopy(data, 0, key, 0, key.Length);

                using var ms = new MemoryStream();

                var buffer = new byte[1024];
                
                fixed (byte* pData = data, pKey = key, pBuffer = buffer)
                {
                    for (var i = LENGTH; i < data.Length; i += buffer.Length)
                    {
                        var length = Math.Min(buffer.Length, data.Length - i);

                        for (var j = 0; j < length; j++)
                        {
                            pBuffer[j] = (byte)(pData[i + j] ^ pKey[(i + j - LENGTH) % key.Length]);
                        }

                        ms.Write(buffer, 0, length);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}