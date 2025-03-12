// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 21:01:21
// # Recently: 2025-01-10 21:01:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JFramework.Net
{
    internal class Program
    {
        public static Setting Setting;
        public static Process Process;

        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            var transport = new Transport();
            try
            {
                Log.Info("运行服务器...");
                if (!File.Exists("setting.json"))
                {
                    var contents = JsonConvert.SerializeObject(new Setting(), Formatting.Indented);
                    await File.WriteAllTextAsync("setting.json", contents);

                    Log.Warn("请将 setting.json 文件配置正确并重新运行。");
                    Console.ReadKey();
                    Environment.Exit(0);
                    return;
                }

                Setting = JsonConvert.DeserializeObject<Setting>(await File.ReadAllTextAsync("setting.json"));
                
                Log.Info("加载程序集...");
                Assembly.LoadFile(Path.GetFullPath("JFramework.dll"));
                Assembly.LoadFile(Path.GetFullPath("JFramework.Kcp.dll"));
                
                Log.Info("初始化传输类...");
                Process = new Process(transport);
                
                transport.OnServerError = Process.ServerError;
                transport.OnServerConnect = Process.ServerConnect;
                transport.OnServerReceive = Process.ServerReceive;
                transport.OnServerDisconnect = Process.ServerDisconnect;
                transport.port = Setting.RestPort;
                transport.StartServer();

                Log.Info("开始进行传输...");
                if (Setting.UseEndPoint)
                {
                    Log.Info("开启REST服务...");
                    if (!RestUtility.StartServer(Setting.RestPort))
                    {
                        Log.Error("请以管理员身份运行或检查端口是否被占用。");
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                Console.ReadKey();
                Environment.Exit(0);
            }

            while (true)
            {
                transport.Update();
                await Task.Delay(Setting.UpdateTime);
            }
        }
    }
}