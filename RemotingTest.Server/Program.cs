﻿using System;
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
                Logger._.Info(type.Name);
                Logger._.Warn(type.Name);
                Logger._.Fatal(type.Name);
                Logger._.Trace(type.Name);
                Logger._.Debug(type.Name);
                Logger._.Error(type.Name);
                RemotingConfiguration.RegisterWellKnownServiceType(type, type.Name, WellKnownObjectMode.SingleCall);
            }
            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
    }
}
