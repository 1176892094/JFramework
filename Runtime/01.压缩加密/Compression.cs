// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-09-03  15:09
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace JFramework
{
    public static class Compression
    {
        public static string Compress(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionMode.Compress, true))
            {
                gzip.Write(bytes, 0, bytes.Length);
            }

            return Convert.ToBase64String(output.ToArray());
        }

        public static string Decompress(string message)
        {
            var bytes = Convert.FromBase64String(message);
            using var input = new MemoryStream(bytes);
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var reader = new StreamReader(gzip, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        
        public static async Task<string> CompressAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            using var output = new MemoryStream();
            await using (var gzip = new GZipStream(output, CompressionMode.Compress, true))
            {
                await gzip.WriteAsync(bytes, 0, bytes.Length);
            }

            return Convert.ToBase64String(output.ToArray());
        }

        public static async Task<string> DecompressAsync(string message)
        {
            var bytes = Convert.FromBase64String(message);
            using var input = new MemoryStream(bytes);
            await using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var reader = new StreamReader(gzip, Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }
    }
}