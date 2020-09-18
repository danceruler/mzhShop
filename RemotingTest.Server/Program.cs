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
using System.Threading;
using System.Threading.Tasks;
using Mzh.Public.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;
using Remoting;
using RemotingTest;
using RemotingTest.Server.Job;

namespace RemotingTest.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            AppConfig.ServerInit();
            //var a = EncryptHelp.Encrypt3Des(DateTime.Now.ToString("2021-09-18 11:15"));
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
            Hello.InitAll();
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (DateTime.Now > AppConfig.expiretime)
                    {
                        tcpChannel.StopListening(tcpChannel);
                        break;
                    }
                }
            });

            #region 启动定时任务
            StartJob();
            #endregion

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }

        public static async void StartJob()
        {
            //1、调度器
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = await sf.GetScheduler();
            //2、创建一个任务
            IJobDetail orderjob = JobBuilder.Create<OrderJob>()
              .WithIdentity("OrderJob", "group1")
              .Build();

            //3、创建一个触发器
            //DateTimeOffset runTime = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("OrderJobTrigger", "group1")
                .WithCronSchedule("0/30 * * * * ?")     //30秒执行一次                                                      
                .Build();

            await sched.ScheduleJob(orderjob, trigger);
            //启动任务
            await sched.Start();
        }
    }

    
}
