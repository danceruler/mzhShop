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
using WxPayAPI;

namespace Remoting
{
    public class ORDER : MarshalByRefObject
    {
        COUPON Coupon;

        /// <summary>
        /// 创建订单
        /// </summary>
        public ResultModel CreatOrder(CreateOrderModel createModel)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    //商品描述
                    string probody = "";

                    #region 新增订单
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
                    #endregion

                    #region 新增订单商品
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
                        probody += $"商品名称:{newop.name} 规格:{newop.skuinput} 数量:{newop.buycount} \r\n";
                    }
                    #endregion

                    #region 更新优惠券关联订单
                    if (createModel.coupons.Count > 0)
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
                    #endregion

                    #region 修改包厢信息
                    if(neworder.type == (int)OrderType.InShop && neworder.boxid > 0)
                    {
                        var box = context.bsp_boxes.SingleOrDefault(t => t.boxid == neworder.boxid);
                        if(box.state != (int)BoxState.Empty)
                        {
                            return ResultModel.Fail("该包厢已被占用，请重新选择包厢");
                        }
                        box.booktime = DateTime.Now;
                        box.oid = neworder.oid;
                        box.phone = neworder.mobile;
                        box.state = (int)BoxState.Book;
                        box.uid = neworder.uid;
                        context.SaveChanges();
                    }
                    #endregion

                    #region 创建预支付订单
                    var user = context.bsp_users.SingleOrDefault(t => t.uid == neworder.uid);
                    WXPayHelper wXPayHelper = new WXPayHelper();
                    var unifiedResult = wXPayHelper.unifiedorder(neworder.oid, 0, probody, user.openid, neworder.orderamount);
                    if (!unifiedResult.Item1)
                    {
                        return ResultModel.Fail("调用微信下单接口失败，详情见日志");
                    }
                    bsp_orderprepays newprepay = new bsp_orderprepays();
                    newprepay.addtime = DateTime.Now;
                    newprepay.appid = unifiedResult.Item2.GetValue("appid").ToString();
                    newprepay.device_info = unifiedResult.Item2.GetValue("device_info").ToString();
                    newprepay.ispay = false;
                    newprepay.mch_id = unifiedResult.Item2.GetValue("mch_id").ToString();
                    newprepay.nonce_str = unifiedResult.Item2.GetValue("nonce_str").ToString();
                    newprepay.notify_url = WXPayHelper.notify_url;
                    newprepay.oid = neworder.oid;
                    newprepay.openid = user.openid;
                    newprepay.paytime = null;
                    newprepay.prepayexpiretime = DateTime.Now.AddMinutes(120);
                    newprepay.prepayid = unifiedResult.Item2.GetValue("prepay_id").ToString();
                    newprepay.sign = unifiedResult.Item2.GetValue("sign").ToString();
                    newprepay.signType = WxPayData.SIGN_TYPE_MD5;
                    newprepay.spbill_create_ip = WXPayHelper.GetLocalIp();
                    newprepay.timeStamp = WXPayHelper.GetTimeStamp();
                    newprepay.total_fee = neworder.orderamount;
                    newprepay.transaction_id = "";
                    context.bsp_orderprepays.Add(newprepay);
                    context.SaveChanges();
                    #endregion

                    #region 写入统计数据
                    AddStatistics(false, neworder, context);
                    #endregion

                    tran.Commit();
                    return ResultModel.Success("");
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error();
                }
            }
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public List<ShowOrderInfo> GetOrderList(string sqlWhere,int page,int count)
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
            using (brnshopEntities context = new brnshopEntities())
            {
                DayStatisticsModel model = new DayStatisticsModel();
                DateTime beginTime = DateTime.Now.Date;
                DateTime endTime = beginTime.AddDays(1);
                var todayStat = new OrderStatistic();
                GetBaseStatistics(beginTime, endTime,0, model,ref todayStat);
                if(todayStat.id == 0)
                {
                    return null;
                }
                //获取其他统计信息
                var yestoday = DateTime.Now.AddDays(-1).Date;
                var yestodayStr = yestoday.ToString("yyyy-MM-dd");
                var beginTimeStr = beginTime.ToString("yyyy-MM-dd");
                var yestodayStatSql = $@"SELECT * FROM bsp_orderstatistics where time >= '{yestodayStr}' and time <'{beginTimeStr}' and type = 1";
                var yestodaydt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(yestodayStatSql));
                var yestodayStats = yestodaydt.GetList<OrderStatistic>("");
                model.TurnOverByYestoday = 0;
                if (yestodayStats.Count > 0)
                {
                    model.TurnOverByYestoday = model.TurnOver - yestodayStats[0].finishordersum;
                }

                var avgHistorySql = $@"select avg(finishordersum) avgfinishordersum from bsp_orderstatistics where type = 0 and time < '{beginTimeStr}'";
                var avgHistorydt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(avgHistorySql));
                model.TurnOverByAverage = 0;
                if (avgHistorydt != null&& avgHistorydt.Rows.Count > 0)
                {
                    model.TurnOverByAverage = model.TurnOver - decimal.Parse(avgHistorydt.Rows[0]["avgfinishordersum"].ToString());
                }
                return model;
            }
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
            var weekStat = new OrderStatistic();
            GetBaseStatistics(beginTime, endTime,1, model,ref weekStat);
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
            var monthStat = new OrderStatistic();
            GetBaseStatistics(beginTime, endTime, 2, model, ref monthStat);
            //获取其他统计信息
            return model;
        }

        /// <summary>
        /// 获取微信小程序用来支付的数据
        /// </summary>
        public void GetDataForPay(int oid)
        {

        }

        /// <summary>
        /// 微信支付回调
        /// </summary>
        /// <param name="notifyxml"></param>
        /// <returns></returns>
        public string PayNotify(string notifyxml)
        {
            Logger._.Info("微信支付回调数据:" + notifyxml);
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var notifydata = XMLHelper.FromXml(notifyxml);
                    if(notifydata["result_code"].ToString() == "SUCCESS")
                    {
                        int oid = int.Parse(notifydata["out_trade_no"].ToString());
                        var prepay = context.bsp_orderprepays.SingleOrDefault(t => t.oid == oid);
                        if (prepay == null)
                        {
                            Logger._.Error("微信支付回调找不到对应的预支付订单");
                            return $@"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[微信支付回调找不到对应的预支付订单]]></return_msg>
                                </xml>";
                        }
                        #region 检验通知数据和预支付数据的一致性
                        if(prepay.sign != notifydata["sign"].ToString())
                        {
                            return $@"<xml>
                                      <return_code><![CDATA[FAIL]]></return_code>
                                      <return_msg><![CDATA[签名失败]]></return_msg>
                                    </xml>";
                        }
                        #endregion

                        prepay.ispay = true;
                        prepay.paytime = DateTime.Now;
                        var order = context.bsp_orders.SingleOrDefault(t => t.oid == prepay.oid);
                        order.paytime = prepay.paytime.Value;
                        order.paymode = (int)PayMod.WxPay;
                        order.payfee = order.orderamount;
                        if (order.type == (int)OrderType.InShop)
                        {
                            order.orderstate = (int)OrderState.Booking;
                            //if(order.boxid > 0)
                            //{
                            //    var box = context.bsp_boxes.SingleOrDefault(t => t.boxid == order.boxid);
                            //    box.
                            //}
                        }
                        else
                        {
                            order.orderstate = (int)OrderState.WaitSend;
                        }
                        context.SaveChanges();

                        #region 写入统计数据
                        AddStatistics(true, order, context);
                        #endregion

                        tran.Commit();
                        return $@"<xml>
                                      <return_code><![CDATA[SUCCESS]]></return_code>
                                      <return_msg><![CDATA[OK]]></return_msg>
                                    </xml>";
                    }
                    
                    return $@"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[]]></return_msg>
                                </xml>";
                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return $@"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[数据操作失败]]></return_msg>
                                </xml>";
                }
            }

        }

        /// <summary>
        /// 用户确认收货接口
        /// </summary>
        public ResultModel QueryRecieve(int oid)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var order = context.bsp_orders.SingleOrDefault(t => t.oid == oid);
                    if (order.orderstate == (int)OrderState.Sending)
                    {
                        //修改订单状态
                        order.orderstate = (int)OrderState.WaitReview;
                        context.SaveChanges();
                        tran.Commit();
                        return ResultModel.Success("确认收货成功");
                    }
                    else
                        return ResultModel.Fail("请刷新订单列表");

                }catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 修改堂食订单为已使用
        /// </summary>
        /// <returns></returns>
        public ResultModel UseShopOder(int oid)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var order = context.bsp_orders.SingleOrDefault(t => t.oid == oid);
                    if (order.orderstate == (int)OrderState.Booking)
                    {
                        //修改订单状态
                        order.orderstate = (int)OrderState.WaitReview;
                        var box = context.bsp_boxes.SingleOrDefault(t => t.boxid == order.boxid);
                        box.state = (int)BoxState.Use;
                        context.SaveChanges();
                        tran.Commit();
                        return ResultModel.Success("修改成功");
                    }
                    else
                        return ResultModel.Fail("请刷新订单列表");

                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 用户申请退款
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public ResultModel ApplyRefund(int oid)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var order = context.bsp_orders.SingleOrDefault(t => t.oid == oid);
                    if (order.orderstate == (int)OrderState.Booking|| order.orderstate == (int)OrderState.WaitSend
                        ||order.orderstate == (int)OrderState.Sending)
                    {
                        //修改订单状态
                        order.orderstate = (int)OrderState.ApplyRefund;
                        var box = context.bsp_boxes.SingleOrDefault(t => t.boxid == order.boxid);
                        box.state = (int)BoxState.Use;
                        context.SaveChanges();
                        tran.Commit();
                        return ResultModel.Success("申请成功，请等待商家确认");
                    }
                    else if(order.orderstate == (int)OrderState.WaitReview)
                    {
                        return ResultModel.Fail("当前订单已确认，不允许退款");
                    }else 
                    //if(order.orderstate == (int)OrderState.ApplyRefund|| order.orderstate == (int)OrderState.Refunded|| order.orderstate == (int)OrderState.WaitPay)
                    {
                        return ResultModel.Fail("请刷新订单");
                    }
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 获得基本统计信息
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="type">0日1周2月</param>
        /// <param name="model"></param>
        private void GetBaseStatistics(DateTime beginTime,DateTime endTime,int type,BaseStatisticsModel model,ref OrderStatistic orderStatistic)
        {
            var beginTimeStr = beginTime.ToString("yyyy-MM-dd");
            var endTimeStr = endTime.ToString("yyyy-MM-dd");
            string sql = $@"SELECT * FROM bsp_orderstatistics where time >= '{beginTimeStr}' and time <'{endTimeStr}' and type = {type}";
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
            List<OrderStatistic> orderStatisticses = dt.GetList<OrderStatistic>("");
            if(orderStatisticses.Count > 0)
            {
                orderStatistic = orderStatisticses[0];
                model.TurnOver = orderStatistic.finishordersum;
                model.FinishOrderCount = orderStatistic.finishordercount;
                //订单类别统计
                model.OrderTypeStatistics = new List<OrderTypeStatistic>();
                OrderTypeStatistic shipstat = new OrderTypeStatistic();
                shipstat.Count = orderStatistic.shipordercount;
                shipstat.SUM = orderStatistic.shipordersum;
                shipstat.OrderType = OrderType.Ship;
                shipstat.TypeName = shipstat.OrderType.ToText();
                OrderTypeStatistic shopstat = new OrderTypeStatistic();
                shopstat.Count = orderStatistic.shopordercount;
                shopstat.SUM = orderStatistic.shopordersum;
                shopstat.OrderType = OrderType.InShop;
                shopstat.TypeName = shopstat.OrderType.ToText();
                model.OrderTypeStatistics.Add(shipstat);
                model.OrderTypeStatistics.Add(shopstat);
            }
        }

        /// <summary>
        /// 写入统计数据
        /// </summary>
        /// <param name="isfinish">创建订单为false，支付完成为true</param>
        /// <param name="sum">订单金额</param>
        /// <param name="ordertime">订单创建日期</param>
        private void AddStatistics(bool isfinish,bsp_orders order, brnshopEntities context)
        {
            //日统计
            var todaytimeStr = order.addtime.Date.ToString("yyyy-MM-dd");
            var todayStat = context.bsp_orderstatistics.Where(t => t.type == 0 && t.timestr == todaytimeStr).SingleOrDefault();
            if(todayStat == null)
            {
                todayStat = new bsp_orderstatistics();
                todayStat.type = 0;
                todayStat.time = order.addtime.Date;
                todayStat.timestr = todaytimeStr;
                todayStat.ordercount = 0;
                todayStat.ordersum = 0;
                todayStat.finishordercount = 0;
                todayStat.finishordersum = 0;
                todayStat.ordercountavg = 0;
                todayStat.ordersumavg = 0;
                todayStat.shipordercount = 0;
                todayStat.shipordersum = 0;
                todayStat.shopordercount = 0;
                todayStat.shopordersum = 0;
                context.bsp_orderstatistics.Add(todayStat);
            }
            if (!isfinish)
            {
                todayStat.ordercount += 1;
                todayStat.ordersum += order.orderamount;
            }
            else
            {
                todayStat.finishordercount += 1;
                todayStat.finishordersum += order.payfee;
                todayStat.shipordercount += order.type == (int)OrderType.InShop ? 0 : 1;
                todayStat.shipordersum += order.type == (int)OrderType.InShop ? 0 : order.payfee;
                todayStat.shopordercount += order.type == (int)OrderType.InShop ? 1 : 0;
                todayStat.shopordersum += order.type == (int)OrderType.InShop ? order.payfee : 0;
            }
            context.SaveChanges();

            //周统计
            var MondayStr = order.addtime.MonDay().ToString("yyyy-MM-dd");
            var weekStat = context.bsp_orderstatistics.Where(t => t.type == 1 && t.timestr == MondayStr).SingleOrDefault();
            if(weekStat == null)
            {
                weekStat = new bsp_orderstatistics();
                weekStat.type = 1;
                weekStat.time = order.addtime.MonDay();
                weekStat.timestr = MondayStr;
                weekStat.ordercount = 0;
                weekStat.ordersum = 0;
                weekStat.finishordercount = 0;
                weekStat.finishordersum = 0;
                weekStat.ordercountavg = 0;
                weekStat.ordersumavg = 0;
                weekStat.shipordercount = 0;
                weekStat.shipordersum = 0;
                weekStat.shopordercount = 0;
                weekStat.shopordersum = 0;
                context.bsp_orderstatistics.Add(weekStat);
            }
            if (!isfinish)
            {
                weekStat.ordercount += 1;
                weekStat.ordersum += order.orderamount;
            }
            else
            {
                weekStat.finishordercount += 1;
                weekStat.finishordersum += order.payfee;
                weekStat.shipordercount += order.type == (int)OrderType.InShop ? 0 : 1;
                weekStat.shipordersum += order.type == (int)OrderType.InShop ? 0 : order.payfee;
                weekStat.shopordercount += order.type == (int)OrderType.InShop ? 1 : 0;
                weekStat.shopordersum += order.type == (int)OrderType.InShop ? order.payfee : 0;
            }
            context.SaveChanges();

            //月统计
            var monthDayStr = order.addtime.FirstInMonth().ToString("yyyy-MM-dd");
            var monthStat = context.bsp_orderstatistics.Where(t => t.type == 2  && t.timestr == monthDayStr).SingleOrDefault();
            if (monthStat == null)
            {
                monthStat = new bsp_orderstatistics();
                monthStat.type = 2;
                monthStat.time = order.addtime.FirstInMonth();
                monthStat.timestr = monthDayStr;
                monthStat.ordercount = 0;
                monthStat.ordersum = 0;
                monthStat.finishordercount = 0;
                monthStat.finishordersum = 0;
                monthStat.ordercountavg = 0;
                monthStat.ordersumavg = 0;
                monthStat.shipordercount = 0;
                monthStat.shipordersum = 0;
                monthStat.shopordercount = 0;
                monthStat.shopordersum = 0;
                context.bsp_orderstatistics.Add(monthStat);
            }
            if (!isfinish)
            {
                monthStat.ordercount += 1;
                monthStat.ordersum += order.orderamount;
            }
            else
            {
                monthStat.finishordercount += 1;
                monthStat.finishordersum += order.payfee;
                monthStat.shipordercount += order.type == (int)OrderType.InShop ? 0 : 1;
                monthStat.shipordersum += order.type == (int)OrderType.InShop ? 0 : order.payfee;
                monthStat.shopordercount += order.type == (int)OrderType.InShop ? 1 : 0;
                monthStat.shopordersum += order.type == (int)OrderType.InShop ? order.payfee : 0;
            }
            context.SaveChanges();
        }
    }
}
