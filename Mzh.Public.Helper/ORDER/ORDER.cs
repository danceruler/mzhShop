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
        COUPON Coupon;


        /// <summary>
        /// 创建订单
        /// </summary>
        public void CreatOrder(CreateOrderModel createModel)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    //新增订单
                    bsp_orders neworder = new bsp_orders();
                    neworder.address = createModel.address;
                    neworder.addtime = DateTime.Now;
                    neworder.besttime = createModel.besttime;
                    neworder.boxid = createModel.boxid;
                    neworder.buyerremark = createModel.buyerremark;
                    neworder.consignee = createModel.consignee;
                    neworder.couponmoney = createModel.couponmoney;
                    neworder.discount = createModel.discount;
                    neworder.fullcut = createModel.fullcut;
                    neworder.mobile = createModel.mobile;
                    neworder.orderamount = createModel.orderamount;
                    neworder.orderstate = (int)OrderState.WaitPay;
                    neworder.productamount = createModel.cart.productamount;
                    neworder.regionid = createModel.regionid;
                    neworder.shipfee = createModel.shipfee;
                    neworder.type = createModel.type;
                    neworder.uid = createModel.uid;
                    neworder.weight = createModel.weight;
                    context.bsp_orders.Add(neworder);
                    context.SaveChanges();

                    //新增订单商品
                    foreach (var cartitem in createModel.cart.items)
                    {
                        bsp_orderproducts newop = new bsp_orderproducts();
                        newop.addtime = neworder.addtime;
                        newop.buycount = cartitem.count;
                        newop.cateid = cartitem.productinfo.cateid;
                        newop.costprice = cartitem.productinfo.costprice;
                        newop.shopprice = cartitem.productinfo.shopprice;
                        newop.marketprice = cartitem.productinfo.marketprice;
                        newop.inputattr = "";
                        newop.inputvalue = "";
                        newop.skuinput = cartitem.skuinput;
                        newop.name = cartitem.productinfo.name;
                        newop.oid = neworder.oid;
                        newop.pid = cartitem.productinfo.pid;
                        newop.showimg = cartitem.productinfo.showimg;
                        newop.skuid = 0;
                        newop.skuguid = cartitem.skuguid;
                        newop.uid = neworder.uid;
                        newop.weight = cartitem.count * cartitem.productinfo.weight;
                        context.bsp_orderproducts.Add(newop);
                        context.SaveChanges();
                    }

                    //更新优惠券关联订单
                    if(createModel.coupons.Count > 0)
                    {
                        foreach(var c in createModel.coupons)
                        {
                            bsp_coupons coupon = context.bsp_coupons.SingleOrDefault(t => t.couponid == c.couponid);
                            coupon.oid = neworder.oid;
                            coupon.isuse = 1;
                            coupon.usetime = neworder.addtime;
                            context.SaveChanges();
                        }
                    }

                    //创建预支付订单


                    tran.Commit();
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.Message);
                    tran.Rollback();
                }
            }
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        private List<ShowOrderInfo> GetOrderList(string sqlWhere,int page,int count)
        {
            List<ShowOrderInfo> orderlist = new List<ShowOrderInfo>();
            string sql = $@"SELECT TOP {count} bsp_orders.oid,bsp_orders.uid,bsp_orders.orderstate,bsp_orders.type,bsp_orders.productamount,bsp_orders.orderamount,
	                               bsp_orders.surplusmoney,bsp_orders.isreview,bsp_orders.addtime,shipsn,bsp_orders.shiptime,bsp_orders.paysn,bsp_orders.paymode,
	                               bsp_orders.paytime,bsp_orders.regionid,bsp_orders.consignee,bsp_orders.mobile,bsp_orders.address,bsp_orders.besttime,bsp_orders.shipfee,
	                               bsp_orders.payfee,bsp_orders.fullcut,bsp_orders.discount,bsp_orders.couponmoney,bsp_orders.weight,bsp_orders.buyerremark,bsp_orders.ip,bsp_orders.boxid,
	                               bsp_orderproducts.recordid op_recordid,bsp_orderproducts.pid op_pid,bsp_orderproducts.cateid op_cateid,bsp_orderproducts.brandid op_brandid,
	                               bsp_orderproducts.name op_name,bsp_orderproducts.showimg op_showimg,bsp_orderproducts.discountprice op_discountprice,
	                               bsp_orderproducts.shopprice op_shopprice,bsp_orderproducts.marketprice op_maketprice,bsp_orderproducts.weight op_weight,
	                               bsp_orderproducts.buycount op_buycount,bsp_orderproducts.type op_type,bsp_orderproducts.addtime op_addtime,
                                   bsp_orderproducts.skuid op_skuid,bsp_orderproducts.inputattr op_inputattr,bsp_orderproducts.inputvalue op_inputvalue
                            FROM dbo.bsp_orders
                            LEFT JOIN dbo.bsp_orderproducts ON bsp_orderproducts.oid = bsp_orders.oid
                            WHERE 1 = 1 AND bsp_orders.oid NOT IN (
                                SELECT TOP {(page - 1) * count} bsp_orders.oid
                                FROM dbo.bsp_orders
                                LEFT JOIN dbo.bsp_orderproducts ON bsp_orderproducts.oid = bsp_orders.oid
                                WHERE 1 = 1 
                                {sqlWhere}
                                ORDER BY bsp_orders.addtime desc
                            )
                            {sqlWhere}
                            ORDER BY bsp_orders.addtime desc";
            SqlCommand sqlCommand = new SqlCommand(sql);
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, sqlCommand);
            if(dt == null || dt.Rows.Count == 0)
            {
                return orderlist;
            }
            orderlist = dt.GetList<ShowOrderInfo>("").Distinct(new DistinctModel<ShowOrderInfo>()).ToList();

            //获取订单相关的优惠券信息
            List<int> oids = orderlist.Select(t => t.oid).ToList();
            List<ShowCouponInfo> couponList = Coupon.GetCouponListByOids(oids);

            foreach (var order in orderlist)
            {
                //订单状态文本
                order.torderstate = ((OrderState)order.orderstate).ToText();
                //订单类型文本
                order.ttype = ((OrderType)order.type).ToText();
                //支付方式文本
                order.tpaymode = ((PayMod)order.paymode).ToText();

                //订单商品信息
                DataTable opdt = dt.Select($"oid = {order.oid}").CopyToDataTable();
                order.orderProducts = opdt.GetList<ShowOrderProductInfo>("").Distinct(new DistinctModel<ShowOrderProductInfo>()).ToList();

                //订单优惠券信息
                order.coupons = couponList.Where(t => t.oid == order.oid).ToList();
                
                //订单包厢信息
                if((OrderType)order.type == OrderType.InShop)
                {
                    order.box = BoxCache.boxes.SingleOrDefault(t => t.boxid == order.boxid);
                }
                else
                {
                    order.box = null;
                }
            }
            return orderlist;
        }

        /// <summary>
        /// 获取今日统计数据
        /// </summary>
        /// <returns></returns>
        public DayStatisticsModel GetDayStatistics()
        {
            DayStatisticsModel model = new DayStatisticsModel();
            DateTime beginTime = DateTime.Now.Date;
            DateTime endTime = beginTime.AddDays(1);
            GetBaseStatistics(beginTime, endTime,model);
            //获取其他统计信息
            return model;
        }

        /// <summary>
        /// 获取周统计数据
        /// </summary>
        /// <returns></returns>
        public WeekStatisticsModel GetWeekStatistics()
        {
            WeekStatisticsModel model = new WeekStatisticsModel();
            DateTime beginTime = DateTime.Now.Date;
            DateTime endTime = beginTime.AddDays(1);
            GetBaseStatistics(beginTime, endTime, model);
            //获取其他统计信息
            return model;
        }

        /// <summary>
        /// 获取月统计数据
        /// </summary>
        /// <returns></returns>
        public MonthStatisticsModel GetMonthStatistics()
        {
            MonthStatisticsModel model = new MonthStatisticsModel();
            DateTime beginTime = DateTime.Now.Date;
            DateTime endTime = beginTime.AddDays(1);
            GetBaseStatistics(beginTime, endTime, model);
            //获取其他统计信息
            return model;
        }

        /// <summary>
        /// 获得基本统计信息
        /// </summary>
        private void GetBaseStatistics(DateTime beginTime,DateTime endTime,BaseStatisticsModel model)
        {
            var beginTimeStr = beginTime.ToString("yyyy-MM-dd");
            var endTimeStr = endTime.ToString("yyyy-MM-dd");
            List<ShowOrderInfo> orders = GetOrderList($@" and bsp_orders.addtime > '{beginTimeStr}' and bsp_orders.addtime < '{endTimeStr} and orderstate > 4 and orderstate <> 6 and orderstate <> 7", 1, 100000);
            model.TurnOver = orders.Count == 0?0: orders.Select(t => t.surplusmoney).Sum();
            model.FinishOrderCount = orders.Count;
            model.OrderCountByTimeStatistics = new List<OrderCountByTimeStatistic>();

        }
    }
}
