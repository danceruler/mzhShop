using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using Mzh.Public.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remoting;
using RemotingTest;

namespace RemotingTest.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            AppConfig.ServerInit();

            var tcpChannel = new TcpServerChannel(AppConfig.TcpPort);
            ChannelServices.RegisterChannel(tcpChannel, false);
            var httpChannel = new HttpChannel(AppConfig.HttpPort);
            ChannelServices.RegisterChannel(httpChannel, false);
            Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "\\Mzh.Public.BLL.dll");
            Type[] types = assembly.GetTypes();
            foreach(var type in types)
            {
                RemotingConfiguration.RegisterWellKnownServiceType(type, type.Name, WellKnownObjectMode.SingleCall);
            }
            ProductCache.InitProductImgDic();

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
    }
}
