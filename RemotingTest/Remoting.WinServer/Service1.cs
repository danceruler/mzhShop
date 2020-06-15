using Mzh.Public.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Remoting.WinServer
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                AppConfig.ServerInit();

                var tcpChannel = new TcpServerChannel(AppConfig.TcpPort);
                ChannelServices.RegisterChannel(tcpChannel, false);
                var httpChannel = new HttpChannel(AppConfig.HttpPort);
                ChannelServices.RegisterChannel(httpChannel, false);
                Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "\\Mzh.Public.BLL.dll");
                Type[] types = assembly.GetTypes();
                foreach (var type in types)
                {
                    RemotingConfiguration.RegisterWellKnownServiceType(type, type.Name, WellKnownObjectMode.SingleCall);
                }
            }catch(Exception ex)
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\log.txt", ex.ToString());
            }
            
        }

        protected override void OnStop()
        {
        }
    }
}
