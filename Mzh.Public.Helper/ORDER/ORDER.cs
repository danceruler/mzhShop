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
using System.Transactions;
using WxPayAPI;

namespace Remoting
{
    public class ORDER : MarshalByRefObject
    {
        COUPON Coupon = new COUPON();
        GROUP Group = new GROUP();

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
                    neworder.address = createModel.address == null?"": createModel.address;
                    neworder.addtime = DateTime.Now;
                    neworder.besttime = createModel.besttime == DateTime.MinValue?DateTime.Now: createModel.besttime;
                    neworder.boxid = createModel.boxid;
                    neworder.buyerremark = createModel.buyerremark;
                    neworder.consignee = createModel.consignee == null?"":createModel.consignee;
                    neworder.couponmoney = createModel.couponmoney;
                    neworder.discount = createModel.discount;
                    neworder.fullcut = createModel.fullcut;
                    neworder.mobile = createModel.mobile == null?"": createModel.mobile;
                    neworder.orderamount = createModel.orderamount;
                    neworder.orderstate = (int)OrderState.WaitPay;
                    neworder.productamount = createModel.cart.productamount;
                    neworder.regionid = createModel.regionid;
                    neworder.shipfee = createModel.shipfee;
                    neworder.type = createModel.type;
                    neworder.uid = createModel.uid;
                    neworder.weight = createModel.weight;
                    neworder.shipsn = "";
                    neworder.shipsystemname = "";
                    neworder.shipfriendname = "";
                    neworder.paysn = "";
                    neworder.paysystemname = "";
                    neworder.payfriendname = "";
                    neworder.phone = "";
                    neworder.email = "";
                    neworder.zipcode = "";
                    neworder.ip = WXPayHelper.GetLocalIp();
                    neworder.boxprice = createModel.boxprice;
                    neworder.bookprice = createModel.bookprice;
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
                        newop.sid = "";
                        newop.psn = "";
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
                    if((neworder.type == (int)OrderType.InShop || neworder.type == (int)OrderType.Order) && neworder.boxid > 0)
                    {
                        var box = context.bsp_boxes.SingleOrDefault(t => t.boxid == neworder.boxid);
                        if(box.state != (int)BoxState.Empty && box.type == 1)
                        {
                            tran.Rollback();
                            return ResultModel.Fail("该包厢已被占用，请重新选择包厢");
                        }
                        box.booktime = DateTime.Now;
                        box.oid = neworder.oid;
                        box.phone = neworder.mobile;
                        box.state = neworder.type == (int)OrderType.InShop ? (int)BoxState.Use : (int)BoxState.Book;
                        box.uid = neworder.uid;
                        box.username = neworder.consignee;
                        box.phone = neworder.mobile;
                        context.SaveChanges();
                    }
                    #endregion

                    #region 创建预支付订单
                    if(neworder.orderamount > 0)
                    {
                        var user = context.bsp_users.SingleOrDefault(t => t.uid == neworder.uid);
                        WXPayHelper wXPayHelper = new WXPayHelper();
                        var unifiedResult = wXPayHelper.unifiedorder(neworder.oid, 0, probody == ""?"预定包厢":probody, user.openid, neworder.orderamount);
                        if (!unifiedResult.Item1)
                        {
                            return ResultModel.Fail("调用微信下单接口失败，详情见日志");
                        }
                        var timestamp = WXPayHelper.GetTimeStamp();
                        bsp_orderprepays newprepay = new bsp_orderprepays();
                        newprepay.addtime = DateTime.Now;
                        newprepay.appid = unifiedResult.Item2["appid"].ToString();
                        newprepay.device_info = "";
                        newprepay.ispay = false;
                        newprepay.mch_id = "";
                        newprepay.nonce_str = unifiedResult.Item2["nonce_str"].ToString();
                        newprepay.notify_url = WXPayHelper.notify_url;
                        newprepay.oid = neworder.oid;
                        newprepay.openid = user.openid;
                        newprepay.paytime = null;
                        newprepay.prepayexpiretime = DateTime.Now.AddMinutes(120);
                        newprepay.prepayid = unifiedResult.Item2["prepay_id"].ToString();
                        newprepay.sign = unifiedResult.Item2["sign"].ToString();
                        newprepay.signType = WxPayData.SIGN_TYPE_MD5;
                        newprepay.spbill_create_ip = WXPayHelper.GetLocalIp();
                        newprepay.timeStamp = timestamp;
                        newprepay.total_fee = neworder.orderamount;
                        newprepay.transaction_id = "";
                        context.bsp_orderprepays.Add(newprepay);
                        context.SaveChanges();
                    }
                    
                    #endregion

                    #region 写入统计数据
                    AddStatistics(false, neworder, context);
                    #endregion

                    tran.Commit();
                    BoxCache.InitBoxes();
                    return ResultModel.Success("", neworder.oid);
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString()+ex.StackTrace);
                    tran.Rollback();
                    return ResultModel.Error();
                }
            }
        }

        /// <summary>
        /// 获取用户的某个状态的订单列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="orderstate"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ShowOrderInfo> GetUserOrderList(int uid,OrderState orderstate,int page,int count)
        {
            return GetOrderList($@"AND bsp_orders.uid = {uid} AND bsp_orders.orderstate = {(int)orderstate}", page, count);
        }

        /// <summary>
        /// 获取用户的订单列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ShowOrderInfo> GetUserAllOrderList(int uid,int page,int count)
        {
            return GetOrderList($@"AND bsp_orders.uid = {uid}", page, count);
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
                                   bsp_orderproducts.skuid op_skuid,bsp_orderproducts.inputattr op_inputattr,bsp_orderproducts.inputvalue op_inputvalue,bsp_orderproducts.skuinput op_skuinput
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
                //if(todayStat.id == 0)
                //{
                //    return model;
                //}
                //获取其他统计信息
                var yestoday = DateTime.Now.AddDays(-1).Date;
                var yestodayStr = yestoday.ToString("yyyy-MM-dd");
                var beginTimeStr = beginTime.ToString("yyyy-MM-dd");
                var yestodayStatSql = $@"SELECT * FROM bsp_orderstatistics where time >= '{yestodayStr}' and time <'{beginTimeStr}' and type = 1";
                var yestodaydt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(yestodayStatSql));
                var yestodayStats = yestodaydt.GetList<OrderStatistic>("");
                model.TurnOverByYestoday = 0;
                model.OrderCountByYestoday = 0;
                if (yestodayStats.Count > 0)
                {
                    model.TurnOverByYestoday = model.TurnOver - yestodayStats[0].finishordersum;
                    model.OrderCountByYestoday = model.FinishOrderCount - yestodayStats[0].finishordercount;
                }
                

                var avgHistorySql = $@"select avg(finishordersum) avgfinishordersum,avg(finishordercount) avgfinishordercount from bsp_orderstatistics where type = 0 and time < '{beginTimeStr}'";
                var avgHistorydt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(avgHistorySql));
                model.TurnOverByAverage = 0;
                model.OrderCountByAverage = 0;
                if (avgHistorydt != null&& avgHistorydt.Rows.Count > 0)
                {
                    model.TurnOverByAverage = model.TurnOver - decimal.Parse(avgHistorydt.Rows[0]["avgfinishordersum"].ToString());
                    model.OrderCountByAverage = model.FinishOrderCount - int.Parse(avgHistorydt.Rows[0]["avgfinishordercount"].ToString());
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
            DateTime beginTime = DateTime.Now.AddDays(-6).Date;
            DateTime endTime = DateTime.Now.Date;
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
            DateTime beginTime = DateTime.Now.AddMonths(-1).AddDays(1).Date;
            DateTime endTime = DateTime.Now.Date;
            var monthStat = new OrderStatistic();
            GetBaseStatistics(beginTime, endTime, 2, model, ref monthStat);
            //获取其他统计信息
            return model;
        }

        /// <summary>
        /// 获取微信小程序用来支付的数据
        /// </summary>
        public ResultModel GetDataForPay(int oid)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var prepay = context.bsp_orderprepays.SingleOrDefault(t => t.oid == oid);
                    if (prepay.addtime.Value <= DateTime.Now.AddMinutes(-119))
                    {
                        return ResultModel.Fail("该订单已经超时未支付");
                    }

                    var timestamp = WXPayHelper.GetTimeStamp();
                    string aSign = $@"appId={prepay.appid}&nonceStr={prepay.nonce_str}&package=prepay_id={prepay.prepayid}&signType=MD5&timeStamp={timestamp}&key={WXPayHelper.apisecret}";

                    WxpayDataForApi model = new WxpayDataForApi();
                    model.appId = prepay.appid;
                    model.nonceStr = prepay.nonce_str;
                    model.package = $@"prepay_id={prepay.prepayid}";
                    model.paySign = EncryptHelp.EncryptMD5(aSign);
                    model.signType = WxPayData.SIGN_TYPE_MD5;
                    model.timeStamp = timestamp;

                    return ResultModel.Success("",model);

                }
                catch (Exception ex)
                {
                    return ResultModel.Error(ex.ToString());
                }
            }    
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
                        //#region 检验通知数据和预支付数据的一致性
                        //if(prepay.sign != notifydata["sign"].ToString())
                        //{
                        //    return $@"<xml>
                        //              <return_code><![CDATA[FAIL]]></return_code>
                        //              <return_msg><![CDATA[签名失败]]></return_msg>
                        //            </xml>";
                        //}
                        //#endregion
                        if (prepay.ispay)
                        {
                            return $@"<xml>
                                      <return_code><![CDATA[SUCCESS]]></return_code>
                                      <return_msg><![CDATA[OK]]></return_msg>
                                    </xml>";
                        }

                        prepay.ispay = true;
                        prepay.paytime = DateTime.Now;
                        var order = context.bsp_orders.SingleOrDefault(t => t.oid == prepay.oid);
                        order.paytime = prepay.paytime.Value;
                        order.paymode = (int)PayMod.WxPay;
                        order.payfee = order.orderamount;
                        if (order.type == (int)OrderType.InShop)
                        {
                            order.orderstate = (int)OrderState.Booking;
                            WebSocketHelper.Send("收到一笔新的堂食订单!");
                        }
                        else if(order.type == (int)OrderType.Ship)
                        {
                            order.orderstate = (int)OrderState.WaitSend;
                            WebSocketHelper.Send("收到一笔新的外卖订单!");
                        }
                        else if(order.type == (int)OrderType.Order)
                        {
                            order.orderstate = (int)OrderState.Booking;
                            WebSocketHelper.Send("收到一笔新的预定订单!");
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
        /// 微信支付回调(拼团)
        /// </summary>
        /// <param name="notifyxml"></param>
        /// <returns></returns>
        public string GroupPayNotify(string notifyxml)
        {
            Logger._.Info("微信支付回调数据:" + notifyxml);
            try
            {
                var notifydata = XMLHelper.FromXml(notifyxml);
                if (notifydata["result_code"].ToString() == "SUCCESS")
                {
                    string transaction_id = notifydata["transaction_id"].ToString();
                    string out_trade_no = notifydata["out_trade_no"].ToString();
                    string[] out_trade_no_array = out_trade_no.Split('a');
                    int isstart = int.Parse(out_trade_no_array[0].ToString());
                    int gid = int.Parse(out_trade_no_array[1].ToString());
                    int uid = int.Parse(out_trade_no_array[2].ToString());

                    
                    //开团
                    if(isstart == 1)
                    {
                        var sql = $@"select * from bsp_groups where startuid = {uid} and groupinfoid = {gid}";
                        DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
                        if(dt != null && dt.Rows.Count == 0)
                        {
                            Group.StartGroup(gid, uid, out_trade_no, transaction_id);
                        }
                    }
                    else
                    {
                        var sql = $@"select * from bsp_groupdetails where uid = {uid} and groupid = {gid}";
                        DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
                        if (dt != null && dt.Rows.Count == 0)
                        {
                            Group.JoinGroup(gid, uid, out_trade_no, transaction_id);
                        }
                        
                    }

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
            catch (Exception ex)
            {
                Logger._.Error("GroupPayNotify,"+ex.ToString());
                return $@"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[数据操作失败]]></return_msg>
                                </xml>";
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
                        if(order.boxid > 0)
                        {
                            var box = context.bsp_boxes.SingleOrDefault(t => t.boxid == order.boxid);
                            box.state = (int)BoxState.Use;
                        }
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
        /// 修改外卖订单为已发货
        /// </summary>
        /// <returns></returns>
        public ResultModel SendOrder(int oid)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var order = context.bsp_orders.SingleOrDefault(t => t.oid == oid);
                    if (order.orderstate == (int)OrderState.WaitSend)
                    {
                        //修改订单状态
                        order.orderstate = (int)OrderState.Sending;
                        order.shiptime = DateTime.Now;
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
                        context.SaveChanges();
                        tran.Commit();
                        WebSocketHelper.Send("收到一笔退款申请!");
                        return ResultModel.Success("申请成功，请等待商家确认");
                    }
                    else if(order.orderstate == (int)OrderState.WaitReview)
                    {
                        return ResultModel.Fail("当前订单已完成，不允许退款");
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
            string sql = $@"SELECT * FROM bsp_orderstatistics where time >= '{beginTimeStr}' and time <='{endTimeStr}' and type = {type}";
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


                model.OrderCountByTimeStatistics = new List<OrderCountByTimeStatistic>();
                string ORDERsql = "";
                var etime = DateTime.Now;
                if (type == 0)
                {
                    etime = orderStatisticses[0].time.Date.AddDays(1);
                    ORDERsql = $@"SELECT DATEPART(hh,paytime) time, SUM(bsp_orders.payfee) payfee,COUNT(oid) count
                                FROM bsp_orders 
                                WHERE paytime IS NOT NULL AND paytime BETWEEN '{orderStatisticses[0].time}' AND '{etime}'
                                GROUP BY DATEPART(hh,paytime)";
                    var ORDERdt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(ORDERsql));
                    for(var i = 0; i <= 23; i++)
                    {
                        var ORDERdrs = ORDERdt.Select($@"time = {i}");
                        if (ORDERdrs.Length > 0)
                        {
                            model.OrderCountByTimeStatistics.Add(new OrderCountByTimeStatistic()
                            {
                                Amount = decimal.Parse(ORDERdrs[0]["payfee"].ToString()),
                                Count = int.Parse(ORDERdrs[0]["count"].ToString()),
                                Time = DateTime.Now.AddHours(i),
                                TimeStr = i+":00"
                            });
                        }
                        else
                        {
                            model.OrderCountByTimeStatistics.Add(new OrderCountByTimeStatistic()
                            {
                                Amount = 0,
                                Count = 0,
                                Time = DateTime.Now.AddHours(i),
                                TimeStr = i + ":00"
                            });
                        }
                    }
                }
                if(type == 1||type == 2)
                {
                    etime = type == 1?orderStatisticses[0].time.Date.AddDays(7): orderStatisticses[0].time.Date.AddMonths(1);
                    ORDERsql = $@"SELECT CONVERT(varchar(100), paytime, 23) time, SUM(bsp_orders.payfee) payfee,COUNT(oid) count
                                FROM bsp_orders 
                                WHERE paytime IS NOT NULL AND paytime BETWEEN '{orderStatisticses[0].time}' AND '{etime}'
                                GROUP BY CONVERT(varchar(100), paytime, 23)";
                    var ORDERdt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(ORDERsql));
                    for (var i = orderStatisticses[0].time; i < etime; i = i.AddDays(1))
                    {
                        var format_i = i.ToString("yyyy-MM-dd");
                        var ORDERdrs = ORDERdt.Select($@"time = '{format_i}'");
                        if (ORDERdrs.Length > 0)
                        {
                            model.OrderCountByTimeStatistics.Add(new OrderCountByTimeStatistic()
                            {
                                Amount = decimal.Parse(ORDERdrs[0]["payfee"].ToString()),
                                Count = int.Parse(ORDERdrs[0]["count"].ToString()),
                                Time = i,
                                TimeStr = ORDERdrs[0]["time"].ToString()
                            });
                        }
                        else
                        {
                            model.OrderCountByTimeStatistics.Add(new OrderCountByTimeStatistic()
                            {
                                Amount = 0,
                                Count = 0,
                                Time = i,
                                TimeStr = i.ToString("yyyy-MM-dd")
                            });
                        }
                    }
                }

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

        /// <summary>
        /// 商家同意退款
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public ResultModel QueryRefund(int oid)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var order = context.bsp_orders.SingleOrDefault(t => t.oid == oid);
                    order.orderstate = (int)OrderState.Refunded;
                    context.SaveChanges();
                    tran.Commit();
                    return ResultModel.Success("修改成功");
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        #region 定时任务
        /// <summary>
        /// 自动更新订单状态（1.支付超时，2.自动确认收货）
        /// </summary>
        public void UpdateOrderState()
        {
            
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    //支付超时
                    var maxtpayime = DateTime.Now.AddMinutes(-119);
                    var orders = context.bsp_orders.Where(t => t.orderstate == (int)OrderState.WaitPay && t.addtime <= maxtpayime).ToList();
                    orders.ForEach(o =>
                    {
                        o.orderstate = (int)OrderState.EndForNoPay;
                    });
                    context.SaveChanges();

                    //确认收货超时
                    var maxquerytime = DateTime.Now.AddMinutes(-119);
                    orders = context.bsp_orders.Where(t => t.orderstate == (int)OrderState.Sending && t.shiptime.Value <= maxquerytime).ToList();
                    orders.ForEach(o =>
                    {
                        o.orderstate = (int)OrderState.WaitReview;
                    });
                    context.SaveChanges();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Logger._.Error("更新订单状态任务执行失败："+ ex.ToString() + ex.StackTrace);
                }
            }
        }
        #endregion
    }
}
