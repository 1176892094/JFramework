// *********************************************************************************
// # Project: SQLServer
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-09-01 20:09:11
// # Recently: 2025-02-11 00:02:13
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;
using Grapevine;
using JFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Runtime
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new Program().StartServer(20975);
        }

        public void StartServer(ushort port)
        {
            Log.Info = Info;
            Log.Warn = Warn;
            Log.Error = Error;
            try
            {
                var builder = new ConfigurationBuilder();
                var config = builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("application.json", true, true).Build();
                var server = new RestServerBuilder(new ServiceCollection(), config, ConfigureServices, ConfigureServer).Build();
                server.Router.Options.SendExceptionMessages = false;
                server.Start();
                Log.Info("按下任意键停止服务器。");
                Console.ReadKey();
                server.Stop();
            }
            catch
            {
                // ignored
            }

            void ConfigureServices(IServiceCollection services)
            {
                services.AddLogging(builder => builder.AddConsole());
                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.None);
            }

            void ConfigureServer(IRestServer server)
            {
                server.Prefixes.Add($"http://*:{port}/");
            }
            
            void Info(string message)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Service.Text.Format("[{0}] {1}", DateTime.Now.ToString("MM-dd HH:mm:ss"), message));
            }

            void Warn(string message)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Service.Text.Format("[{0}] {1}", DateTime.Now.ToString("MM-dd HH:mm:ss"), message));
            }

            void Error(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Service.Text.Format("[{0}] {1}", DateTime.Now.ToString("MM-dd HH:mm:ss"), message));
            }
        }
    }
}