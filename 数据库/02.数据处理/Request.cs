// *********************************************************************************
// # Project: SQLServer
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-09-01 20:09:24
// # Recently: 2025-02-11 00:02:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Grapevine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JFramework.Net
{
    [RestResource]
    internal class RestRequest
    {
        [RestRoute("Post", "api/server/login")]
        public async Task Login(IHttpContext context)
        {
            byte[] readBytes;
            using (var memoryStream = new MemoryStream())
            {
                await context.Request.InputStream.CopyToAsync(memoryStream);
                readBytes = memoryStream.ToArray();
            }
            
            readBytes = Service.Xor.Decrypt(readBytes);
            var readJson = Encoding.UTF8.GetString(readBytes);
            readJson = Service.Zip.Decompress(readJson);
            var request = JsonConvert.DeserializeObject<LoginRequest>(readJson);
            
            var response = Program.Login(request);
            readJson = Service.Zip.Compress(response);
            readBytes = Encoding.UTF8.GetBytes(readJson);
            readBytes = Service.Xor.Encrypt(readBytes);
            context.Response.ContentType = "application/octet-stream";
            await context.Response.SendResponseAsync(readBytes);
        }
    }
    
    internal static class RestUtility
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
    }
}