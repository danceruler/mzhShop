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
            string sqlWhere = $" AND bsp_coupons.oid in ({string.Join(",",oids.ToArray())})";
            return GetCouponList(sqlWhere, 1, 100);
        }
        /// <summary>
        /// 获取某笔订单使用的优惠券信息
        /// </summary>
        /// <returns></returns>
        public List<ShowCouponInfo> GetCouponListByOid(int oid)
        {
            string sqlWhere = $" AND bsp_coupons.oid = {oid}";
            return GetCouponList(sqlWhere, 1, 100);
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
            List<ShowCouponInfo> couponlist;

            string sql = $@"SELECT TOP {count} bsp_coupons.couponid,bsp_coupons.uid,bsp_coupons.coupontypeid,bsp_coupons.oid,bsp_coupons.usetime,bsp_coupons.useip,
	                               bsp_coupons.activatetime,bsp_coupons.createtime,bsp_coupons.createip,bsp_coupons.expiretime,bsp_coupons.isuse,
	                               bsp_coupontypes.coupontypeid ct_coupontypeid,dbo.bsp_coupontypes.state ct_state,bsp_coupontypes.name ct_name,
	                               bsp_coupontypes.getmode ct_getmode,bsp_coupontypes.usemode ct_usemode,bsp_coupontypes.sendstarttime ct_sendstarttime,
	                               bsp_coupontypes.sendendtime ct_sendendtime,bsp_coupontypes.useexpiretime ct_useexpiretime,bsp_coupontypes.usestarttime ct_usestarttime,
	                               bsp_coupontypes.useendtime ct_useendtime,bsp_coupontypes.type ct_type,bsp_coupontypes.isstack ct_isstack,bsp_coupontypes.fullmoney ct_fullmoney,
	                               bsp_coupontypes.cutmoney ct_cutmoney,bsp_coupontypes.discount ct_discount
                            FROM dbo.bsp_coupons
                            JOIN dbo.bsp_coupontypes ON bsp_coupontypes.coupontypeid = bsp_coupons.coupontypeid
                            WHERE 1 = 1 
                            AND bsp_coupons.couponid NOT IN (
                                SELECT TOP {(page - 1) * count} bsp_coupons.couponid
                                FROM dbo.bsp_coupons
                                JOIN dbo.bsp_coupontypes ON bsp_coupontypes.coupontypeid = bsp_coupons.coupontypeid
                                WHERE 1 = 1 {sqlWhere}         
                                ORDER BY bsp_coupons.createtime DESC
                            )
                            {sqlWhere}
                            ORDER BY bsp_coupons.createtime DESC";
            SqlCommand sqlCommand = new SqlCommand(sql);
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, sqlCommand);
            couponlist = dt.GetList<ShowCouponInfo>("");
            foreach(var coupon in couponlist)
            {
                coupon.typeInfo = dt.Select($"couponid = {coupon.couponid}")[0].TransObjFromTable<ShowCouponTypeInfo>();
            }
            return couponlist;
        }
    }
}
