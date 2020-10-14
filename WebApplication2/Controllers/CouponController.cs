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
    public class CouponController : ApiController
    {
        /// <summary>
        /// 获取首页可领用优惠券列表
        /// </summary>
        /// <returns></returns>
        public List<ShowCouponTypeInfo> GetRecpientCoupon(int uid)
        {
            COUPON coupon = RemotingHelp.GetModelObject<COUPON>();
            return coupon.GetRecpientCoupon(uid);
        }

        /// <summary>
        /// 用户领用优惠券接口
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="couponTypeid"></param>
        public ResultModel RecpientCoupon(int uid, int couponTypeid)
        {
            COUPON coupon = RemotingHelp.GetModelObject<COUPON>();
            return coupon.RecpientCoupon(uid,couponTypeid);

        }

        /// <summary>
        /// 获取用户可用的优惠券列表，前端发起创建订单请求时判断优惠券使用逻辑
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<ShowCouponTypeInfo> GetCanUseCoupons(int uid)
        {
            COUPON coupon = RemotingHelp.GetModelObject<COUPON>();
            return coupon.GetRecpientCoupon(uid);
        }
    }
}
