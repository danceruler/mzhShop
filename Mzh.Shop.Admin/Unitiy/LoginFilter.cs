using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mzh.Shop.Admin.Unitiy
{
    public class LoginFilter: ActionFilterAttribute
    {
        //该方法会在action方法执行之前调用  
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.Cookies.AllKeys.Contains("userinfo"))
            {
                filterContext.HttpContext.Response.Redirect("/Home/Login");
            }
            base.OnActionExecuting(filterContext);
        }

    }
}