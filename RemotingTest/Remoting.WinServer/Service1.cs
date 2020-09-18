using Mzh.Public.Base;
using Quartz;
using Quartz.Impl;
using RemotingTest.Server.Job;
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
using System.Threading;
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
                Hello.InitAll();
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(60000);
                        if (DateTime.Now > AppConfig.expiretime)
                        {
                            tcpChannel.StopListening(tcpChannel);
                            this.Stop();
                            break;
                        }
                    }
                });
                #region 启动定时任务
                StartJob();
                #endregion
            }
            catch (Exception ex)
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\log.txt", ex.ToString());
            }
            
        }

        protected override void OnStop()
        {
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
