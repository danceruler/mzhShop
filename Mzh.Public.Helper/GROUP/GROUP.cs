using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using NLog.Fluent;
using Remoting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    /// <summary>
    /// 拼团
    /// </summary>
    public class GROUP : MarshalByRefObject
    {
        COUPON coupon = new COUPON();

        /// <summary>
        /// 新增拼团活动
        /// </summary>
        public ResultModel CreateGroupInfo(GroupInfoModel model)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    bsp_groupinfos newGroupInfo = new bsp_groupinfos();
                    newGroupInfo.endtime = model.endtime;
                    newGroupInfo.groupoid = model.groupoid;
                    newGroupInfo.groupprice = model.groupprice;
                    newGroupInfo.grouptype = model.grouptype;
                    newGroupInfo.maxtime = model.maxtime;
                    newGroupInfo.needcount = model.needcount;
                    newGroupInfo.shopprice = model.shopprice;
                    newGroupInfo.starttime = model.starttime;
                    context.bsp_groupinfos.Add(newGroupInfo);
                    context.SaveChanges();
                    return ResultModel.Success("新增成功");
                }
                catch (Exception ex)
                {
                    Logger._.Error("CreateGroupInfo方法,", ex);
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 删除拼团活动
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel DeleteGroupInfo(int groupInfoId)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    bsp_groupinfos GroupInfo = context.bsp_groupinfos.SingleOrDefault(t => t.groupinfoid == groupInfoId);
                    context.bsp_groupinfos.Remove(GroupInfo);
                    context.SaveChanges();
                    return ResultModel.Success("删除成功");
                }
                catch (Exception ex)
                {
                    Logger._.Error("DeleteGroupInfo方法,", ex);
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 用户开团（支付回调时调用）
        /// </summary>
        /// <returns></returns>
        public ResultModel StartGroup(int groupInfoId, int uid,string outtradeno,string transaction_id)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    bsp_groupinfos GroupInfo = context.bsp_groupinfos.SingleOrDefault(t => t.groupinfoid == groupInfoId);
                    bsp_groups newGroup = new bsp_groups();
                    newGroup.endtime = DateTime.Now.AddSeconds(GroupInfo.maxtime);
                    newGroup.failtype = 0;
                    newGroup.groupoid = GroupInfo.groupoid;
                    newGroup.groupprice = GroupInfo.groupprice;
                    newGroup.grouptype = GroupInfo.grouptype;
                    newGroup.isfail = false;
                    newGroup.isfinish = false;
                    newGroup.maxtime = GroupInfo.maxtime;
                    newGroup.needcount = GroupInfo.needcount;
                    newGroup.nowcount = 1;
                    newGroup.shopprice = GroupInfo.shopprice;
                    newGroup.starttime = DateTime.Now;
                    newGroup.startuid = uid;
                    newGroup.groupinfoid = groupInfoId;
                    context.bsp_groups.Add(newGroup);
                    context.SaveChanges();

                    bsp_groupdetails newGroupetail = new bsp_groupdetails();
                    newGroupetail.groupid = newGroup.groupid;
                    newGroupetail.paytime = DateTime.Now;
                    newGroupetail.sno = 1;
                    newGroupetail.uid = uid;
                    newGroupetail.isgetcoupon = false;
                    newGroupetail.paytime = DateTime.Parse("1997-01-27");
                    newGroupetail.transaction_id = transaction_id;
                    newGroupetail.outtradeno = outtradeno;
                    context.bsp_groupdetails.Add(newGroupetail);
                    context.SaveChanges();
                    ORDER.AddStatistics(true, newGroup, newGroupetail, context);
                    tran.Commit();
                    return ResultModel.Success("开团成功");
                }
                catch (Exception ex)
                {
                    Logger._.Error("StartGroup方法,",ex);
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 用户参团（支付回调时调用）
        /// </summary>
        /// <returns></returns>
        public ResultModel JoinGroup(int GroupId,int uid, string outtradeno, string transaction_id)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    bsp_groups Group = context.bsp_groups.SingleOrDefault(t => t.groupid == GroupId);
                    Group.nowcount += 1;
                    //拼团成功，发放优惠券
                    if (Group.needcount <= Group.nowcount)
                    {
                        Group.isfinish = true;
                        Group.endtime = DateTime.Now;
                        var groupdetails = context.bsp_groupdetails.Where(t => t.groupid == GroupId).ToList();
                        foreach (var groupdetail in groupdetails)
                        {
                            if (!groupdetail.isgetcoupon.Value)
                            {
                                coupon.RecpientCoupon(groupdetail.uid, Group.groupoid);
                                groupdetail.isgetcoupon = true;
                            }
                        }
                    }

                    bsp_groupdetails newGroupetail = new bsp_groupdetails();
                    newGroupetail.groupid = Group.groupid;
                    newGroupetail.paytime = DateTime.Now;
                    newGroupetail.sno = Group.nowcount;
                    newGroupetail.uid = uid;
                    newGroupetail.isgetcoupon = false;
                    newGroupetail.paytime = DateTime.Parse("1997-01-27");
                    newGroupetail.transaction_id = transaction_id;
                    newGroupetail.outtradeno = outtradeno;
                    if (Group.needcount <= Group.nowcount)
                    {
                        coupon.RecpientCoupon(newGroupetail.uid, Group.groupoid);
                        newGroupetail.isgetcoupon = true;
                    }
                    context.bsp_groupdetails.Add(newGroupetail);
                    context.SaveChanges();

                    ORDER.AddStatistics(true, Group, newGroupetail, context);
                    tran.Commit();
                    return ResultModel.Success("参团成功");
                }
                catch (Exception ex)
                {
                    Logger._.Error("JoinGroup方法,",ex);
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 获取首页拼团列表
        /// </summary>
        /// <returns></returns>
        public ResultModel GroupInfoList()
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    string sql = $@"select * from bsp_groupinfos";
                    List<GroupInfoModel> groupInfos = context.Database.SqlQuery<GroupInfoModel>(sql).ToList();
                    var couponTypes = coupon.GetCouponTypeForGroup();
                    foreach (var groupInfo in groupInfos)
                    {
                        groupInfo.couponTypeInfo = couponTypes.SingleOrDefault(t => t.ct_coupontypeid == groupInfo.groupoid);
                        bool isfind = false;
                        foreach(var products in ProductCache.ProductList)
                        {
                            
                            foreach(var product in products.productInfos)
                            {
                                if(product.pid == groupInfo.couponTypeInfo.ct_pid)
                                {
                                    groupInfo.productInfo = product;
                                    isfind = true;
                                    break;
                                }
                            }
                            if (isfind) break;
                        }
                        var couponTypeInfodes = "";
                        if(groupInfo.couponTypeInfo.ct_type == 1)
                        {
                            couponTypeInfodes = $@" 满{Math.Round(groupInfo.couponTypeInfo.ct_fullmoney,2)}减{Math.Round(groupInfo.couponTypeInfo.ct_cutmoney,2)}";
                        }
                        else
                        {
                            couponTypeInfodes = $@" {(int)groupInfo.couponTypeInfo.ct_discount}折";
                        }
                        groupInfo.title = groupInfo.couponTypeInfo.ct_name + couponTypeInfodes;
                    }
                    return ResultModel.Success("", groupInfos);
                }
                catch (Exception ex)
                {
                    Logger._.Error("GroupInfoList方法,", ex);
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 获取首页拼团列表
        /// </summary>
        /// <returns></returns>
        public LayuiTableApiResult GroupInfoListForAdmin()
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                LayuiTableApiResult result = new LayuiTableApiResult();
                
                try
                {
                    string sql = $@"select * from bsp_groupinfos";
                    List<GroupInfoModel> groupInfos = context.Database.SqlQuery<GroupInfoModel>(sql).ToList();
                    var couponTypes = coupon.GetCouponTypeForGroup();
                    foreach (var groupInfo in groupInfos)
                    {
                        groupInfo.couponTypeInfo = couponTypes.SingleOrDefault(t => t.ct_coupontypeid == groupInfo.groupoid);
                        bool isfind = false;
                        foreach (var products in ProductCache.ProductList)
                        {

                            foreach (var product in products.productInfos)
                            {
                                if (product.pid == groupInfo.couponTypeInfo.ct_pid)
                                {
                                    groupInfo.productInfo = product;
                                    isfind = true;
                                    break;
                                }
                            }
                            if (isfind) break;
                        }
                        var couponTypeInfodes = "";
                        if (groupInfo.couponTypeInfo.ct_type == 1)
                        {
                            couponTypeInfodes = $@" 满{Math.Round(groupInfo.couponTypeInfo.ct_fullmoney, 2)}减{Math.Round(groupInfo.couponTypeInfo.ct_cutmoney, 2)}";
                        }
                        else
                        {
                            couponTypeInfodes = $@" {(int)groupInfo.couponTypeInfo.ct_discount}折";
                        }
                        groupInfo.title = groupInfo.couponTypeInfo.ct_name + couponTypeInfodes;
                    }

                    result.code = 0;
                    result.msg = "";
                    result.count = groupInfos.Count;
                    result.data = groupInfos;
                    return result;
                }
                catch (Exception ex)
                {
                    Logger._.Error("GroupInfoList方法," ,ex);
                    result.code = 1;
                    result.msg = "GroupInfoList方法," + ex.ToString();
                    return result;
                }
            }
        }

        /// <summary>
        /// 获取当前拼团活动正在进行的团
        /// </summary>
        /// <returns></returns>
        public ResultModel GetRunningGroupListByInfo(int groupInfoId)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    string sql = $@"select bsp_groups.*,
	                                       bsp_groupdetails.groupdetailid gd_groupdetailid,
	                                       bsp_groupdetails.groupid gd_groupid,
	                                       bsp_groupdetails.paytime gd_paytime,
	                                       bsp_groupdetails.sno gd_sno,
	                                       bsp_groupdetails.uid gd_uid
                                    from bsp_groups 
                                    left join bsp_groupdetails on bsp_groupdetails.groupid = bsp_groups.groupid
                                    where groupinfoid = {groupInfoId} and isfinish = 0 and isfail = 0";
                    var dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
                    var groups = dt.GetList<GroupModel>("").Distinct(new DistinctModel<GroupModel>()).ToList();

                    foreach(var group in groups)
                    {
                        //拼团信息
                        DataTable gddt = dt.Select($"groupid = {group.groupid}").CopyToDataTable();
                        group.details = gddt.GetList<GroupDetailModel>("");
                    }

                    return ResultModel.Success("", groups);
                }
                catch (Exception ex)
                {
                    Logger._.Error("GroupInfoList方法,",ex);
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 用户发起拼团支付
        /// </summary>
        /// <param name="isstart">1开团 2参团</param>
        /// <param name="gid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ResultModel PayGroup(int isstart,int gid,int uid,decimal totalfee)
        {
            try
            {
                using (brnshopEntities context = new brnshopEntities())
                {
                    if(isstart == 1)
                    {
                        var groupCount = context.bsp_groups.Where(t => t.startuid == uid & t.groupinfoid == gid).Count();
                        if(groupCount > 0)
                        {
                            return ResultModel.Fail("您已参与过该团");
                        }
                    }
                    else
                    {
                        var groupCount = context.bsp_groupdetails.Where(t => t.uid == uid & t.groupid == gid).Count();
                        if (groupCount > 0)
                        {
                            return ResultModel.Fail("您已参与过该团");
                        }
                    }

                    var user = context.bsp_users.SingleOrDefault(t => t.uid == uid);
                    WXPayHelper wXPayHelper = new WXPayHelper();
                    var unifiedorderResult = wXPayHelper.unifiedorderForGroup(isstart, gid, uid, "拼团", user.openid, totalfee);
                    if (!unifiedorderResult.Item1)
                    {
                        return ResultModel.Fail("拼团支付调用微信下单接口失败，详情见日志");
                    }
                    SortedDictionary<string, object> payDic = unifiedorderResult.Item2 as SortedDictionary<string, object>;

                    var timestamp = WXPayHelper.GetTimeStamp();
                    string aSign = $@"appId={payDic["appid"]}&nonceStr={payDic["nonce_str"].ToString()}&package=prepay_id={payDic["prepay_id"].ToString()}&signType=MD5&timeStamp={timestamp}&key={WXPayHelper.apisecret}";
                    WxpayDataForApi model = new WxpayDataForApi();
                    model.appId = payDic["appid"].ToString();
                    model.nonceStr = payDic["nonce_str"].ToString();
                    model.package = $@"prepay_id={payDic["prepay_id"].ToString()}";
                    model.paySign = EncryptHelp.EncryptMD5(aSign);
                    model.signType = WxPayAPI.WxPayData.SIGN_TYPE_MD5;
                    model.timeStamp = timestamp;

                    return ResultModel.Success("", model);
                }
            }
            catch(Exception ex)
            {
                Logger._.Error("PayGroup方法," , ex);
                return ResultModel.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 获取用户的拼团列表
        /// </summary>
        /// <returns></returns>
        public ResultModel GroupListByUid(int uid)
        {
            try
            {
                string sql = $@"  select bsp_groups.groupid ,
                                             bsp_groups.grouptype ,
                                             bsp_groups.groupoid ,
                                             bsp_groups.starttime ,
                                             bsp_groups.endtime ,
                                             bsp_groups.startuid ,
                                             bsp_groups.groupprice ,
                                             bsp_groups.shopprice ,
                                             bsp_groups.isfinish ,
                                             bsp_groups.needcount ,
                                             bsp_groups.nowcount ,
                                             bsp_groups.isfail ,
                                             bsp_groups.failtype ,
                                             bsp_groups.maxtime,
		                                     bsp_groupdetails.groupdetailid gd_groupdetailid,
                                             bsp_groupdetails.groupid gd_groupid,
                                             bsp_groupdetails.uid gd_uid,
                                             bsp_groupdetails.sno gd_sno,
                                             bsp_groupdetails.paytime gd_paytime,
                                             bsp_groupdetails.outtradeno gd_outtradeno,
                                             bsp_groupdetails.transaction_id gd_transaction_id
                                            from bsp_groups
                                            join bsp_groupdetails on bsp_groupdetails.groupid = bsp_groups.groupid
                                            where bsp_groups.groupid IN (SELECT groupid FROM dbo.bsp_groupdetails WHERE uid = {uid})
                                            order by bsp_groups.starttime desc";
                DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
                List<GroupModel> groups = dt.GetList<GroupModel>("").Distinct(new DistinctModel<GroupModel>()).ToList();
                var couponTypes = coupon.GetCouponTypeForGroup();
                foreach (var group in groups)
                {
                    DataTable opdt = dt.Select($"groupid = {group.groupid}").CopyToDataTable();
                    group.details = opdt.GetList<GroupDetailModel>("").Distinct(new DistinctModel<GroupDetailModel>()).ToList();

                    group.couponTypeInfo = couponTypes.SingleOrDefault(t => t.ct_coupontypeid == group.groupoid);

                    bool isfind = false;
                    foreach (var products in ProductCache.ProductList)
                    {
                        foreach (var product in products.productInfos)
                        {
                            if (product.pid == group.couponTypeInfo.ct_pid)
                            {
                                group.productInfo = product;
                                isfind = true;
                                break;
                            }
                        }
                        if (isfind) break;
                    }
                    var couponTypeInfodes = "";
                    if (group.couponTypeInfo.ct_type == 1)
                    {
                        couponTypeInfodes = $@" 满{Math.Round(group.couponTypeInfo.ct_fullmoney, 2)}减{Math.Round(group.couponTypeInfo.ct_cutmoney, 2)}";
                    }
                    else
                    {
                        couponTypeInfodes = $@" {(int)group.couponTypeInfo.ct_discount}折";
                    }
                    group.title = group.couponTypeInfo.ct_name + couponTypeInfodes;
                }

                return ResultModel.Success("成功", groups);

            }
            catch (Exception ex)
            {
                Logger._.Error("GroupListByUid方法," ,ex);
                return ResultModel.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 更新拼团状态
        /// </summary>
        public void UpdateGroupState()
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    //支付超时
                    var groups = context.bsp_groups.Where(t => t.isfail == false && t.isfinish == false && t.endtime <= DateTime.Now).ToList();
                    groups.ForEach(g =>
                    {
                        g.isfail = true;
                    });
                    context.SaveChanges();

                    //确认收货超时
                    //var maxquerytime = DateTime.Now.AddMinutes(-119);
                    //orders = context.bsp_orders.Where(t => t.orderstate == (int)OrderState.Sending && t.shiptime.Value <= maxquerytime).ToList();
                    //orders.ForEach(o =>
                    //{
                    //    o.orderstate = (int)OrderState.WaitReview;
                    //});
                    context.SaveChanges();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Logger._.Error("更新拼团状态任务执行失败：" , ex);
                }
            }
        }
    }
}
