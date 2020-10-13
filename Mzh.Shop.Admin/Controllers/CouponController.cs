using Mzh.Public.Model.Model;
using Remoting;
using Remoting.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mzh.Shop.Admin.Controllers
{
    public class CouponController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CouponAdd(int coupontypeid = 0)
        {
            ViewBag.coupontypeid = coupontypeid;
            ViewBag.coupontype = null;
            if (coupontypeid > 0)
            {
                COUPON coupon = RemotingHelp.GetModelObject<COUPON>();
                ViewBag.coupontype = (coupon.GetCouponType().data as List<ShowCouponTypeInfo>).SingleOrDefault(t => t.ct_coupontypeid == coupontypeid);
            }
            return View();
        }

        #region 接口
        /// <summary>
        /// 获取所有的优惠券类型（用于后台配置）
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCouponType()
        {
            COUPON coupon = RemotingHelp.GetModelObject<COUPON>();
            return Json(coupon.GetCouponType(),JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 结束发放优惠券
        /// </summary>
        /// <param name="coupontypeids"></param>
        /// <returns></returns>
        public ActionResult StopCoupon(int[] coupontypeids)
        {
            COUPON coupon = RemotingHelp.GetModelObject<COUPON>();
            return Json(coupon.StopCoupon(coupontypeids), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增优惠券（后台）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddCouponType(ShowCouponTypeInfo model)
        {
            COUPON coupon = RemotingHelp.GetModelObject<COUPON>();
            return Json(coupon.AddCouponType(model),JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}