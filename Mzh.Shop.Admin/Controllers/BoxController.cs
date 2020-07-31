using Mzh.Public.Model;
using Remoting;
using Remoting.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mzh.Shop.Admin.Controllers
{
    public class BoxController : Controller
    {
        // GET: Box
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BoxList()
        {
            BoxCache boxcache = RemotingHelp.GetModelObject<BoxCache>();
            ViewBag.boxlist = boxcache.GetBoxes();
            return View();
        }


        #region 接口

        /// <summary>
        /// 获取包厢列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBoxes()
        {
            BoxCache boxcache = RemotingHelp.GetModelObject<BoxCache>();
            return Json(
                boxcache.GetBoxes(),
                JsonRequestBehavior.AllowGet
                );
        }

        public ActionResult UpdateBox(int boxid, BoxState state, string name) 
        {
            BOX box = RemotingHelp.GetModelObject<BOX>();
            return Json(
                box.UpdateBox(boxid,state,name),
                JsonRequestBehavior.AllowGet
                );
        }

        public ActionResult AddBox(string code, string name)
        {
            BOX box = RemotingHelp.GetModelObject<BOX>();
            return Json(
                box.AddBox(code, name),
                JsonRequestBehavior.AllowGet
                );
        }

        public ActionResult DeleteBox(int boxid)
        {
            BOX box = RemotingHelp.GetModelObject<BOX>();
            return Json(
                box.DeleteBox(boxid),
                JsonRequestBehavior.AllowGet
                );
        }
        #endregion
    }
}