using Mzh.Public.Model.Model;
using Mzh.Shop.Admin.Unitiy;
using Remoting;
using Remoting.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mzh.Shop.Admin.Controllers
{
    [LoginFilter]
    public class BannerController : Controller
    {
        // GET: Banner
        public ActionResult Banner()
        {
            return View();
        }

        public ActionResult BannerAdd(int bannerid = 0)
        {
            ViewBag.bannerid = bannerid;
            if(bannerid > 0)
            {
                BannerCache bannerCache = RemotingHelp.GetModelObject<BannerCache>();
                ViewBag.banner = bannerCache.GetBanners().SingleOrDefault(t => t.id == bannerid);
            }
            ProductCache productCache = RemotingHelp.GetModelObject<ProductCache>();
            ViewBag.products = productCache.GetNoRepeatProducts();
            return View();
        }

        #region 接口
        public ActionResult GetBannersForAdmin()
        {
            BannerCache bannerCache = RemotingHelp.GetModelObject<BannerCache>();
            return Json(bannerCache.GetBannersForAdmin(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加编辑轮播图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddBanner(AddBannerModel model)
        {
            BannerCache bannerCache = RemotingHelp.GetModelObject<BannerCache>();
            return Json(bannerCache.AddBanner(model), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除轮播图
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteBanner(int[] ids)
        {
            BannerCache bannerCache = RemotingHelp.GetModelObject<BannerCache>();
            return Json(bannerCache.DeleteBanner(ids), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}