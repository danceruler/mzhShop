using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class ORDER : MarshalByRefObject
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        public ResultModel CreatOrder(CreateOrderModel createModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public List<ShowOrderInfo> GetOrderList(string sqlWhere, int page, int count)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取今日统计数据
        /// </summary>
        /// <returns></returns>
        public DayStatisticsModel GetDayStatistics()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取周统计数据
        /// </summary>
        /// <returns></returns>
        public WeekStatisticsModel GetWeekStatistics()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取月统计数据
        /// </summary>
        /// <returns></returns>
        public MonthStatisticsModel GetMonthStatistics()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取微信小程序用来支付的数据
        /// </summary>
        public void GetDataForPay(int oid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 微信支付回调
        /// </summary>
        /// <param name="notifyxml"></param>
        /// <returns></returns>
        public string PayNotify(string notifyxml)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 用户确认收货接口
        /// </summary>
        public ResultModel QueryRecieve(int oid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改堂食订单为已使用
        /// </summary>
        /// <returns></returns>
        public ResultModel UseShopOder(int oid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 用户申请退款
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public ResultModel ApplyRefund(int oid)
        {
            throw new NotImplementedException();
        }
    }
}
