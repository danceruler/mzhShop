using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mzh.Shop.Admin.Controllers
{
    public class UploadController : Controller
    {
        
        [HttpPost]
        public ActionResult File()
        {
            var files = Request.Files["file"];
            //foreach(HttpPostedFileBase file in files)
            //{
            //    var name = file.FileName;
            //    var stream = file.InputStream;
            //}
            return Json(ResultModel.Success(),JsonRequestBehavior.AllowGet);
        }
    }
}