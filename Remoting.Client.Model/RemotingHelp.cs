using Mzh.Public.Base;
using Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Remoting.Client.Model
{
    public class RemotingHelp
    {
        private static TcpClientChannel TcpChannel;

        private static bool IsConnect { get; set; } = true;

        static RemotingHelp()
        {
            AppConfig.ClientInit();
            TcpChannel = new TcpClientChannel();
            ChannelServices.RegisterChannel(TcpChannel, false);
            if(AppConfig.Client == "WebApi"&& !GetModelObject<Hello>().CheckClient(AppConfig.WebApiId, 1)) IsConnect = false;
            if (AppConfig.Client == "WebAdmin"&& !GetModelObject<Hello>().CheckClient(AppConfig.WebAdminId, 2)) IsConnect = false;
            if (string.IsNullOrWhiteSpace(AppConfig.Client)) IsConnect = false;
            //var httpChannel = new HttpChannel();
            //ChannelServices.RegisterChannel(httpChannel, false);
        }

        public static TModel GetModelObject<TModel>() where TModel : class
        {
            if (!IsConnect) return null;
            return (TModel)Activator.GetObject(typeof(TModel), $"tcp://localhost:{AppConfig.TcpPort}/"+ typeof(TModel).Name);
        }
    }
}
