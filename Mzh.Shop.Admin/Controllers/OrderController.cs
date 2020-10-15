using Mzh.Public.Model;
using Mzh.Public.Model.Model;
using Remoting;
using Remoting.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace Mzh.Shop.Admin.Controllers
{
    public class OrderController : Controller
    {
        /// <summary>
        /// 今日统计
        /// </summary>
        /// <returns></returns>
        public ActionResult TodayStatistics()
        {
            return View();
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            ViewBag.startTime = DateTime.Now.Date.ToString("yyyy-MM-dd");
            ViewBag.endTime = DateTime.Now.Date.ToString("yyyy-MM-dd");
            return View();
        }

        /// <summary>
        /// 订单列表页
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="orderState"></param>
        /// <returns></returns>
        //public ActionResult ListOrder(DateTime startTime, DateTime endTime,int page,int count)
        //{
        //    var sTime = startTime.ToString("yyyy-MM-dd HH:mm:ss");
        //    var eTime = endTime.ToString("yyyy-MM-dd HH:mm:ss");
        //    ORDER order = RemotingHelp.GetModelObject<ORDER>();
        //    var sqlWhere = $@" AND bsp_orders.addtime between '{sTime}' AND '{eTime}'";
        //    var list = order.GetOrderList(sqlWhere, page, count);
        //    ViewBag.OrderList = list;
        //    ViewBag.page = page;
        //    return PartialView();
        //}

        /// <summary>
        /// 销售统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Statistics()
        {
            return View();
        }

        #region 接口
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public ActionResult GetOrderList(DateTime startTime, DateTime endTime, int page, int count,string orderid)
        {
            var sTime = startTime.ToString("yyyy-MM-dd HH:mm:ss");
            var eTime = endTime.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
            var sqlWhere = $@" AND bsp_orders.addtime between '{sTime}' AND '{eTime}'";
            if (!string.IsNullOrWhiteSpace(orderid))
            {
                sqlWhere = $@" AND bsp_orders.oid = {orderid}";
            }
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            var list = order.GetOrderList(sqlWhere, page, count);
            return Json(
                list,
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改堂食订单为已使用
        /// </summary>
        /// <returns></returns>
        public ActionResult UseShopOder(int oid)
        {
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            return Json(
                order.UseShopOder(oid),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改外卖订单为已发货
        /// </summary>
        /// <returns></returns>
        public ActionResult SendOrder(int oid)
        {
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            return Json(
                order.SendOrder(oid),
                JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取今日统计数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDayStatistics()
        {
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            return Json(
                order.GetDayStatistics(),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取周统计数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWeekStatistics()
        {
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            return Json(
                order.GetWeekStatistics(),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取月统计数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMonthStatistics()
        {
            ORDER order = RemotingHelp.GetModelObject<ORDER>();
            return Json(
                order.GetMonthStatistics(),
                JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}