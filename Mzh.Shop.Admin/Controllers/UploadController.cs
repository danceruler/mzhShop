using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
            var files = Request.Files;
            foreach (HttpPostedFileBase file in files)
            {
                var name = file.FileName;
                var stream = file.InputStream;
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();
            }
            return Json(ResultModel.Success(),JsonRequestBehavior.AllowGet);
        }
    }
}