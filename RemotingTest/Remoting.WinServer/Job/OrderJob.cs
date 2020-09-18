using Quartz;
using Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotingTest.Server.Job
{
    public class OrderJob:IJob
    {
        /// <summary>
        /// 作业调度定时执行的方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            ORDER order = new ORDER();
            order.UpdateOrderState();
        }
    }
}
