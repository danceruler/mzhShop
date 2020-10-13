using Mzh.Public.Base;
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
    public class COUPON : MarshalByRefObject
    {
        /// <summary>
        /// 获取多笔订单使用的
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public List<ShowCouponInfo> GetCouponListByOids(List<int> oids)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取某笔订单使用的优惠券信息
        /// </summary>
        /// <returns></returns>
        public List<ShowCouponInfo> GetCouponListByOid(int oid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取优惠券列表
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<ShowCouponInfo> GetCouponList(string sqlWhere, int page, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取首页可领用优惠券列表
        /// </summary>
        /// <returns></returns>
        public List<ShowCouponTypeInfo> GetRecpientCoupon(int uid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 用户领用优惠券接口
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="couponTypeid"></param>
        public ResultModel RecpientCoupon(int uid, int couponTypeid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取用户可用的优惠券列表，前端发起创建订单请求时判断优惠券使用逻辑
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<ShowCouponInfo> GetCanUseCoupons(int uid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有的优惠券类型（用于后台配置）
        /// </summary>
        /// <returns></returns>
        public LayuiTableApiResult GetCouponType()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 结束发放优惠券
        /// </summary>
        /// <param name="coupontypeids"></param>
        /// <returns></returns>
        public ResultModel StopCoupon(int[] coupontypeids)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新增优惠券（后台）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddCouponType(ShowCouponTypeInfo model)
        {
            throw new NotImplementedException();
        }
    }
}
