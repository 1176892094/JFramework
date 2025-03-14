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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Grapevine;
using JFramework;
using Newtonsoft.Json;

namespace Runtime
{
    [RestResource]
    public class MySqlService
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
            
            var response = Login(request);
            readJson = Service.Zip.Compress(response);
            readBytes = Encoding.UTF8.GetBytes(readJson);
            readBytes = Service.Xor.Encrypt(readBytes);
            context.Response.ContentType = "application/octet-stream";
            await context.Response.SendResponseAsync(readBytes);
        }

        private static string Login(LoginRequest request)
        {
            var watch = new Stopwatch();
            watch.Start();
            var database = Settings.GetConnection(request.username, request.password);
            var response = new LoginResponse();
            if (string.IsNullOrEmpty(database))
            {
                return JsonConvert.SerializeObject(response);
            }

            try
            {
                var connection = new MySqlProxy(database);
                if (!string.IsNullOrEmpty(request.settingManager.deviceId))
                {
                    response = UpdateLoginTime(connection, request, response);
                }

                if (response.errorCode == 0)
                {
                    if (response.userData < 0)
                    {
                        response.userData = 0;
                    }
                    else if (response.userData < request.settingManager.userData)
                    {
                        response.userData = request.settingManager.userData;
                    }

                    var userId = UpdateOrInsert(connection, request, response.userData);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        response.userId = userId;
                    }

                    watch.Stop();
                    Log.Info($"用户 {response.userId} 数据更新成功。耗时 {(float)watch.ElapsedMilliseconds / 1000} 秒");
                }

                return JsonConvert.SerializeObject(response);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                response.errorCode = 2;
                return JsonConvert.SerializeObject(response);
            }
        }

        private static LoginResponse UpdateLoginTime(MySqlProxy connection, LoginRequest request, LoginResponse response)
        {
            var parameter = new Dictionary<string, object>
            {
                { "@userName", request.settingManager.deviceId }
            };
            var dataTables = MySqlUtils.Select<LoginTable>(connection, "userName = @userName", parameter);
            foreach (var dataTable in dataTables)
            {
                if (dataTable.loginTime > DateTime.Parse(request.settingManager.loginTime))
                {
                    Log.Error($"用户 {dataTable.userId} 数据更新失败！");
                    response.errorCode = 1;
                }

                response.userId = dataTable.userId.ToString();
                response.userData = dataTable.userData;
                break;
            }


            return response;
        }

        private static string UpdateOrInsert(MySqlProxy connection, LoginRequest request, int userData)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@userName", request.settingManager.deviceId }
            };
            var dataTables = MySqlUtils.Select<LoginTable>(connection, "userName = @userName", parameters);
            if (dataTables.Count == 0)
            {
                parameters = new Dictionary<string, object>
                {
                    { "userName", request.settingManager.deviceId },
                    { "userData", userData },
                    { "settingManager", JsonConvert.SerializeObject(request.settingManager) },
                    { "playerData1", JsonConvert.SerializeObject(request.playerData1) },
                    { "playerData2", JsonConvert.SerializeObject(request.playerData2) },
                    { "playerData3", JsonConvert.SerializeObject(request.playerData3) },
                    { "playerData4", JsonConvert.SerializeObject(request.playerData4) },
                };
                MySqlUtils.Insert<LoginTable>(connection, parameters);
                parameters = new Dictionary<string, object>
                {
                    { "@userName", request.settingManager.deviceId }
                };
                dataTables = MySqlUtils.Select<LoginTable>(connection, "userName = @userName", parameters);
                foreach (var dataTable in dataTables)
                {
                    return dataTable.userId.ToString();
                }

                return string.Empty;
            }

            parameters = new Dictionary<string, object>
            {
                { "userData", userData },
                { "settingManager", JsonConvert.SerializeObject(request.settingManager) },
                { "playerData1", JsonConvert.SerializeObject(request.playerData1) },
                { "playerData2", JsonConvert.SerializeObject(request.playerData2) },
                { "playerData3", JsonConvert.SerializeObject(request.playerData3) },
                { "playerData4", JsonConvert.SerializeObject(request.playerData4) },
                { "updateTime", DateTime.Now },
            };

            var parameter = new KeyValuePair<string, object>("userName", request.settingManager.deviceId);
            MySqlUtils.Update<LoginTable>(connection, parameter, parameters);
            return string.Empty;
        }
    }
}