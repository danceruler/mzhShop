﻿using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model;
using Mzh.Public.Model.Model;
using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
            string sqlWhere = $" AND bsp_coupons.oid in ({string.Join(",", oids.ToArray())})";
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
	                               bsp_coupontypes.cutmoney ct_cutmoney,bsp_coupontypes.discount ct_discount,ISNULL(bsp_couponproducts.pid,0) ct_pid
                            FROM dbo.bsp_coupons
                            JOIN dbo.bsp_coupontypes ON bsp_coupontypes.coupontypeid = bsp_coupons.coupontypeid
                            LEFT JOIN bsp_couponproducts ON bsp_couponproducts.coupontypeid = bsp_coupontypes.coupontypeid
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
            foreach (var coupon in couponlist)
            {
                coupon.typeInfo = dt.Select($"couponid = {coupon.couponid}")[0].TransObjFromTable<ShowCouponTypeInfo>();
            }
            return couponlist;
        }

        /// <summary>
        /// 获取首页可领用优惠券列表
        /// </summary>
        /// <returns></returns>
        public List<ShowCouponTypeInfo> GetRecpientCoupon(int uid)
        {
            try
            {
                string sql = $@"SELECT bsp_coupontypes.coupontypeid ct_coupontypeid,
                                   state ct_state,
                                   name ct_name,
                                   money ct_money,
                                   count ct_count,
                                   sendmode ct_sendmode,
                                   getmode ct_getmode,
                                   usemode ct_usemode,
                                   userranklower ct_userranklower,
                                   orderamountlower ct_orderamountlower,
                                   limitcateid ct_limitcateid,
                                   limitbrandid ct_limitbrandid,
                                   limitproduct ct_limitproduct,
                                   sendstarttime ct_sendstarttime,
                                   sendendtime ct_sendendtime,
                                   useexpiretime ct_useexpiretime,
                                   usestarttime ct_usestarttime,
                                   useendtime ct_useendtime,
                                   type ct_type,
                                   isstack ct_isstack,
                                   fullmoney ct_fullmoney,
                                   cutmoney ct_cutmoney,
                                   discount ct_discount,
                                   ISNULL(bsp_couponproducts.pid,0) ct_pid
                            FROM bsp_coupontypes
                            LEFT JOIN 
                            (SELECT bsp_coupons.coupontypeid,bsp_coupons.createtime
                            FROM bsp_coupons
                            WHERE uid = {uid}) TEMP ON TEMP.coupontypeid = bsp_coupontypes.coupontypeid AND TEMP.createtime >= bsp_coupontypes.sendstarttime AND TEMP.createtime <= bsp_coupontypes.sendendtime
                            LEFT JOIN bsp_couponproducts ON bsp_couponproducts.coupontypeid = bsp_coupontypes.coupontypeid
                            WHERE GETDATE() >= bsp_coupontypes.sendstarttime AND GETDATE() <= bsp_coupontypes.sendendtime AND TEMP.coupontypeid IS NULL 
                                  and bsp_coupontypes.coupontypeid not in (select groupoid from bsp_groupinfos where starttime < getdate() and endtime > getdate())
                                  AND bsp_coupontypes.isforgroup = 0
                            ";
                DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
                List<ShowCouponTypeInfo> result = dt.GetList<ShowCouponTypeInfo>("");
                result.ForEach(t => {
                    t.t_ct_isstack = t.ct_isstack == 1 ? "可以叠加" : "不可以叠加";
                    t.t_ct_type = EnumToText.ToText((CouponType)t.ct_type);
                    t.t_ct_fullcut = t.ct_fullmoney.ToString() + '/' + t.ct_cutmoney.ToString();
                });
                return result;
            }
            catch (Exception ex)
            {
                Logger._.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取用于拼团选择的优惠券
        /// </summary>
        /// <returns></returns>
        public List<ShowCouponTypeInfo> GetRecpientCouponForGroup()
        {
            try
            {
                string sql = $@"SELECT bsp_coupontypes.coupontypeid ct_coupontypeid,
                                   state ct_state,
                                   name ct_name,
                                   money ct_money,
                                   count ct_count,
                                   sendmode ct_sendmode,
                                   getmode ct_getmode,
                                   usemode ct_usemode,
                                   userranklower ct_userranklower,
                                   orderamountlower ct_orderamountlower,
                                   limitcateid ct_limitcateid,
                                   limitbrandid ct_limitbrandid,
                                   limitproduct ct_limitproduct,
                                   sendstarttime ct_sendstarttime,
                                   sendendtime ct_sendendtime,
                                   useexpiretime ct_useexpiretime,
                                   usestarttime ct_usestarttime,
                                   useendtime ct_useendtime,
                                   type ct_type,
                                   isstack ct_isstack,
                                   fullmoney ct_fullmoney,
                                   cutmoney ct_cutmoney,
                                   discount ct_discount,
                                   ISNULL(bsp_couponproducts.pid,0) ct_pid
                            FROM bsp_coupontypes
                            LEFT JOIN bsp_couponproducts ON bsp_couponproducts.coupontypeid = bsp_coupontypes.coupontypeid
                            WHERE GETDATE() >= bsp_coupontypes.sendstarttime AND GETDATE() <= bsp_coupontypes.sendendtime AND bsp_coupontypes.isforgroup = 1
                            ";
                DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
                List<ShowCouponTypeInfo> result = dt.GetList<ShowCouponTypeInfo>("");
                result.ForEach(t => {
                    t.t_ct_isstack = t.ct_isstack == 1 ? "可以叠加" : "不可以叠加";
                    t.t_ct_type = EnumToText.ToText((CouponType)t.ct_type);
                    t.t_ct_fullcut = t.ct_fullmoney.ToString() + '/' + t.ct_cutmoney.ToString();
                });
                return result;
            }
            catch (Exception ex)
            {
                Logger._.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 用户领用优惠券接口
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="couponTypeid"></param>
        public ResultModel RecpientCoupon(int uid, int couponTypeid,bool isFromGroup = false)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var coupontype = context.bsp_coupontypes.SingleOrDefault(t => t.coupontypeid == couponTypeid);
                    var coupon = context.bsp_coupons.SingleOrDefault(t => t.uid == uid && t.coupontypeid == couponTypeid&t.createtime >= coupontype.sendstarttime &t.createtime <= coupontype.sendendtime);

                    if(coupon != null&&!isFromGroup)
                    {
                        return ResultModel.Fail("您已经领取了该优惠券");
                    }
                    var newcoupon = new bsp_coupons();
                    newcoupon.uid = uid;
                    newcoupon.activateip = WXPayHelper.GetPublicIp();
                    newcoupon.activatetime = DateTime.Now;
                    newcoupon.couponsn = WXPayHelper.GetTimeStamp();
                    newcoupon.coupontypeid = couponTypeid;
                    newcoupon.createip = "";
                    newcoupon.createoid = 0;
                    newcoupon.createtime = DateTime.Now;
                    newcoupon.createuid = uid;
                    newcoupon.useip = "";
                    newcoupon.usetime = DateTime.Parse("1997-01-01");
                    newcoupon.isuse = 0;
                    if (coupontype.useexpiretime == 0)
                    {
                        newcoupon.expiretime = coupontype.useendtime;
                    }
                    else
                    {
                        newcoupon.expiretime = DateTime.Now.AddSeconds(coupontype.useexpiretime);
                    }
                    context.bsp_coupons.Add(newcoupon);
                    context.SaveChanges();

                    return ResultModel.Success();
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex);
                    return ResultModel.Error();
                }
            }

        }

        /// <summary>
        /// 获取用户可用的优惠券列表，前端发起创建订单请求时判断优惠券使用逻辑
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<ShowCouponInfo> GetCanUseCoupons(int uid)
        {
            return GetCouponList($"AND bsp_coupons.uid = {uid} AND bsp_coupons.isuse = 0 AND bsp_coupons.expiretime > getdate()",1,100);
        }

        /// <summary>
        /// 获取所有的优惠券类型（用于后台配置）
        /// </summary>
        /// <returns></returns>
        public LayuiTableApiResult GetCouponType()
        {
            LayuiTableApiResult result = new LayuiTableApiResult();
            try
            {
                string sql = $@"SELECT bsp_coupontypes.coupontypeid ct_coupontypeid,
                                   state ct_state,
                                   name ct_name,
                                   money ct_money,
                                   count ct_count,
                                   sendmode ct_sendmode,
                                   getmode ct_getmode,
                                   usemode ct_usemode,
                                   userranklower ct_userranklower,
                                   orderamountlower ct_orderamountlower,
                                   limitcateid ct_limitcateid,
                                   limitbrandid ct_limitbrandid,
                                   limitproduct ct_limitproduct,
                                   sendstarttime ct_sendstarttime,
                                   sendendtime ct_sendendtime,
                                   useexpiretime ct_useexpiretime,
                                   usestarttime ct_usestarttime,
                                   useendtime ct_useendtime,
                                   type ct_type,
                                   isstack ct_isstack,
                                   fullmoney ct_fullmoney,
                                   cutmoney ct_cutmoney,
                                   discount ct_discount,
                                   ISNULL(bsp_couponproducts.pid,0) ct_pid
                            FROM bsp_coupontypes
                            LEFT JOIN bsp_couponproducts ON bsp_couponproducts.coupontypeid = bsp_coupontypes.coupontypeid
                            ORDER BY sendendtime DESC
                            ";
                DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
                List<ShowCouponTypeInfo> list = dt.GetList<ShowCouponTypeInfo>("");
                list.ForEach(t => {
                    t.t_ct_isstack = t.ct_isstack == 1 ? "可以叠加" : "不可以叠加";
                    t.t_ct_type = EnumToText.ToText((CouponType)t.ct_type);
                    t.t_ct_fullcut = t.ct_fullmoney.ToString() + '/' +t.ct_cutmoney.ToString();
                    t.t_ct_sendstarttime = t.ct_sendstarttime.ToString("yyyy-MM-dd");
                    t.t_ct_sendendtime = t.ct_sendendtime.ToString("yyyy-MM-dd");
                });

                result.code = 0;
                result.msg = "";
                result.count = list.Count;
                result.data = list;
                return result;
            }
            catch (Exception ex)
            {
                Logger._.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取所有的优惠券类型（用于拼团）
        /// </summary>
        /// <returns></returns>
        public List<ShowCouponTypeInfo> GetCouponTypeForGroup()
        {
            try
            {
                string sql = $@"SELECT bsp_coupontypes.coupontypeid ct_coupontypeid,
                                   state ct_state,
                                   name ct_name,
                                   money ct_money,
                                   count ct_count,
                                   sendmode ct_sendmode,
                                   getmode ct_getmode,
                                   usemode ct_usemode,
                                   userranklower ct_userranklower,
                                   orderamountlower ct_orderamountlower,
                                   limitcateid ct_limitcateid,
                                   limitbrandid ct_limitbrandid,
                                   limitproduct ct_limitproduct,
                                   sendstarttime ct_sendstarttime,
                                   sendendtime ct_sendendtime,
                                   useexpiretime ct_useexpiretime,
                                   usestarttime ct_usestarttime,
                                   useendtime ct_useendtime,
                                   type ct_type,
                                   isstack ct_isstack,
                                   fullmoney ct_fullmoney,
                                   cutmoney ct_cutmoney,
                                   discount ct_discount,
                                   ISNULL(bsp_couponproducts.pid,0) ct_pid
                            FROM bsp_coupontypes
                            LEFT JOIN bsp_couponproducts ON bsp_couponproducts.coupontypeid = bsp_coupontypes.coupontypeid
                            ORDER BY sendendtime DESC
                            ";
                DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
                List<ShowCouponTypeInfo> list = dt.GetList<ShowCouponTypeInfo>("");
                list.ForEach(t => {
                    t.t_ct_isstack = t.ct_isstack == 1 ? "可以叠加" : "不可以叠加";
                    t.t_ct_type = EnumToText.ToText((CouponType)t.ct_type);
                    t.t_ct_fullcut = t.ct_fullmoney.ToString() + '/' + t.ct_cutmoney.ToString();
                    t.t_ct_sendstarttime = t.ct_sendstarttime.ToString("yyyy-MM-dd");
                    t.t_ct_sendendtime = t.ct_sendendtime.ToString("yyyy-MM-dd");
                });

                return list;
            }
            catch (Exception ex)
            {
                Logger._.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 结束发放优惠券
        /// </summary>
        /// <param name="coupontypeids"></param>
        /// <returns></returns>
        public ResultModel StopCoupon(int[] coupontypeids)
        {
            
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    foreach(var coupontypeid in coupontypeids)
                    {
                        var coupontype = context.bsp_coupontypes.SingleOrDefault(t => t.coupontypeid == coupontypeid);
                        coupontype.sendendtime = DateTime.Now;
                        context.SaveChanges();
                    }
                    tran.Commit();
                    return ResultModel.Success("结束发放成功");
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    Logger._.Error(ex);
                    return ResultModel.Error();
                }
            }
        }

        /// <summary>
        /// 新增优惠券（后台）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddCouponType(ShowCouponTypeInfo model)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    //新增
                    if(model.ct_coupontypeid == 0)
                    {
                        bsp_coupontypes newcoupontype = new bsp_coupontypes();
                        newcoupontype.cutmoney = model.ct_cutmoney;
                        newcoupontype.discount = model.ct_discount;
                        newcoupontype.fullmoney = model.ct_fullmoney;
                        newcoupontype.count = 0;
                        newcoupontype.getmode = 0;
                        newcoupontype.isstack = model.ct_isstack;
                        newcoupontype.limitbrandid = 0;
                        newcoupontype.limitcateid = 0;
                        newcoupontype.limitproduct = 0;
                        newcoupontype.money = 0;
                        newcoupontype.name = model.ct_name;
                        newcoupontype.orderamountlower = 0;
                        newcoupontype.sendendtime = model.ct_sendendtime;
                        newcoupontype.sendmode = 0;
                        newcoupontype.sendstarttime = model.ct_sendstarttime;
                        newcoupontype.state = 0;
                        newcoupontype.type = model.ct_type;
                        newcoupontype.useendtime = model.ct_useexpiretime == 1?model.ct_useendtime:DateTime.Now;
                        newcoupontype.useexpiretime = model.ct_useexpiretime;
                        newcoupontype.usemode = 0;
                        newcoupontype.userranklower = 0;
                        newcoupontype.usestarttime = model.ct_useexpiretime == 1 ? model.ct_usestarttime : DateTime.Now;
                        newcoupontype.isforgroup = model.ct_isforgroup;
                        context.bsp_coupontypes.Add(newcoupontype);
                        context.SaveChanges();

                        if(model.ct_pid > 0)
                        {
                            bsp_couponproducts bsp_Couponproduct = new bsp_couponproducts();
                            bsp_Couponproduct.coupontypeid = newcoupontype.coupontypeid;
                            bsp_Couponproduct.pid = model.ct_pid;
                            context.bsp_couponproducts.Add(bsp_Couponproduct);
                            context.SaveChanges();
                        }
                    }
                    tran.Commit();
                    return ResultModel.Success("添加成功");
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    Logger._.Error(ex);
                    return ResultModel.Error();
                }
            }
        }
    }
}
