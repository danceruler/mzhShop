using Mzh.Public.Base;
using Mzh.Public.BLL.HELP;
using Mzh.Public.Model.Model;
using Remoting.Client.Model;
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
            var file = Request.Files["file"];
            var name = file.FileName;
            var stream = file.InputStream;
            StreamFileHelper streamFileHelper = new StreamFileHelper();
            UPLOAD upload = RemotingHelp.GetModelObject<UPLOAD>();
            return Json(upload.Upload(streamFileHelper.StreamToBytes(stream), name, "webadmin"),
                JsonRequestBehavior.AllowGet);
        }
    }
}