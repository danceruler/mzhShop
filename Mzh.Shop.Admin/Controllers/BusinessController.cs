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
    public class BusinessController : Controller
    {
        public ActionResult Index()
        {
            BusinessCache businesscache = RemotingHelp.GetModelObject<BusinessCache>();
            ViewBag.Business = businesscache.GetBusiness().obj;
            return View();
        }

        public ActionResult AddOrUpdateBusiness(BusinessModel businessModel)
        {
            BusinessCache bcache = RemotingHelp.GetModelObject<BusinessCache>();
            return Json(bcache.AddOrUpdateBusiness(businessModel),JsonRequestBehavior.AllowGet);
        }
    }
}