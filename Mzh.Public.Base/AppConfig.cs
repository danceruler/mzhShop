using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Base
{
    public class AppConfig
    {
        public static string ConnectionString { get; set; }
        public static string WebApiId { get; set; }
        public static string WebAdminId { get; set; }
        public static int TcpPort { get; set; }
        public static int HttpPort { get; set; }
        public static string Client { get; set; } = "";

        public static void ClientInit()
        {
            var configJsonStr = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\config.json");
            var configJson = JsonConvert.DeserializeObject<JObject>(configJsonStr);
            var tcpport = int.Parse(configJson["remotingConfig"]["tcpport"].ToString());
            var httpport = int.Parse(configJson["remotingConfig"]["httpport"].ToString());
            WebApiId = configJson["clientIDs"]["WebApi"].ToString();
            if (!string.IsNullOrWhiteSpace(WebApiId)) Client = "WebApi";
            WebAdminId = configJson["clientIDs"]["WebAdmin"].ToString();
            if (!string.IsNullOrWhiteSpace(WebAdminId)) Client = "WebAdmin";
            TcpPort = tcpport;
            HttpPort = httpport;
        }

        public static void ServerInit()
        {
            var configJsonStr = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\config.json");
            var configJson = JsonConvert.DeserializeObject<JObject>(configJsonStr);
            var tcpport = int.Parse(configJson["remotingConfig"]["tcpport"].ToString());
            var httpport = int.Parse(configJson["remotingConfig"]["httpport"].ToString());
            var connectionString = EncryptHelp.Decrypt3Des(configJson["connectionString"].ToString());
            var WebApiid = EncryptHelp.Decrypt3Des(configJson["clientIDs"]["WebApi"].ToString());
            var WebAdminid = EncryptHelp.Decrypt3Des(configJson["clientIDs"]["WebAdmin"].ToString());
            ConnectionString = connectionString;
            WebApiId = WebApiid;
            WebAdminId = WebAdminid;
            TcpPort = tcpport;
            HttpPort = httpport;
        }

    }
}
