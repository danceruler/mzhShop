using Mzh.Public.BLL.HELP;
using Remoting.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult GetUrl(string objectname)
        {
            UPLOAD upload = RemotingHelp.GetModelObject<UPLOAD>();
            var result = upload.GetFilePath(objectname);
            return Redirect(result.obj.ToString());
        }
    }
}