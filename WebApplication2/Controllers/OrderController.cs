using Mzh.Public.Model;
using Mzh.Public.Model.Model;
using Remoting;
using Remoting.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    public class OrderController : ApiController
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        [HttpPost]
        public ResultModel CreatOrder(CreateOrderModel createModel)
        {
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            return order.CreatOrder(createModel);
        }

        /// <summary>
        /// 用户确认收货接口
        /// </summary>
        [HttpPost]
        public ResultModel QueryRecieve(int oid)
        {
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            return order.QueryRecieve(oid);
        }

        /// <summary>
        /// 用户申请退款
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultModel ApplyRefund(int oid)
        {
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            return order.ApplyRefund(oid);
        }

        /// <summary>
        /// 获取微信小程序用来支付的数据
        /// </summary>
        [HttpPost]
        public ResultModel GetDataForPay(int oid)
        {
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            return order.GetDataForPay(oid);
        }

        /// <summary>
        /// 获取用户的某个状态的订单列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="orderstate"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost]
        public List<ShowOrderInfo> GetUserOrderList(int uid, OrderState orderstate, int page, int count)
        {
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            return order.GetUserOrderList(uid,orderstate,page,count);
        }
    }
}
