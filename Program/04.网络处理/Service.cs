// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:17
// # Recently: 2025-01-10 21:01:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Grapevine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using HttpStatusCode = Grapevine.HttpStatusCode;

namespace JFramework.Net
{
    [RestResource]
    internal class RestService
    {
        [RestRoute("Get", "/api/compressed/servers")]
        public async Task ServerListCompressed(IHttpContext context)
        {
            if (Program.Setting.RequestRoom)
            {
                var json = JsonConvert.SerializeObject(Program.Process.roomInfo);
                await context.Response.SendResponseAsync(RestUtility.Compress(json));
            }
            else
            {
                await context.Response.SendResponseAsync(HttpStatusCode.Forbidden);
            }
        }
    }

    public static class RestUtility
    {
        public static bool StartServer(ushort port)
        {
            try
            {
                var builder = new ConfigurationBuilder();
                var config = builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("application.json", true, true).Build();
                var server = new RestServerBuilder(new ServiceCollection(), config, ConfigureServices, ConfigureServer).Build();
                server.Router.Options.SendExceptionMessages = false;
                server.Start();
                return true;
            }
            catch
            {
                return false;
            }

            void ConfigureServices(IServiceCollection services)
            {
                services.AddLogging(builder => builder.AddConsole());
                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.None);
            }

            void ConfigureServer(IRestServer server)
            {
                server.Prefixes.Add(Service.Text.Format("http://*:{0}/", port));
            }
        }

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
    }
}