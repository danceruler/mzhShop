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
    }
}
