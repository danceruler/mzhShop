using System;
using System.Collections.Generic;
using System.Linq;
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
            return View();
        }

        /// <summary>
        /// 销售统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Statistics()
        {
            return View();
        }

        #region 接口
        
        #endregion
    }
}