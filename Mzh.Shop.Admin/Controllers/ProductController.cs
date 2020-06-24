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
    public class ProductController : Controller
    {
        public ActionResult ProductType()
        {
            return View();
        }

        public ActionResult ProductTypeAdd()
        {
            return View();
        }

        public ActionResult Product(int cateid = 0)
        {
            ViewBag.cateid = cateid;
            return View();
        }

        [HttpGet]
        public ActionResult GetCategories()
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            var list = product.GetCategories();
            return Json(
                new LayuiApiResult() { 
                    code = 0,
                    msg = "",
                    count = list.Count,
                    data = list
                }, 
                JsonRequestBehavior.AllowGet);
        }
    }
}