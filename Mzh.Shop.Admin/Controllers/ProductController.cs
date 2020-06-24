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
    }
}