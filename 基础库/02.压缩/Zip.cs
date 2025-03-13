// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-11 18:01:28
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace JFramework
{
    public static partial class Service
    {
        public static class Zip
        {
            public static string Compress(string data)
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                using var buffer = new MemoryStream();
                using (var stream = new GZipStream(buffer, CompressionMode.Compress, true))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                return Convert.ToBase64String(buffer.ToArray());
            }

            public static string Decompress(string data)
            {
                var bytes = Convert.FromBase64String(data);
                using var buffer = new MemoryStream(bytes);
                using var stream = new GZipStream(buffer, CompressionMode.Decompress);
                using var reader = new StreamReader(stream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
    }
}