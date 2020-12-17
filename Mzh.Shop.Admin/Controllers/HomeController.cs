using Mzh.Public.Base;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Mzh.Shop.Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.Cookies.AllKeys.Contains("userinfo"))
            {
                return View();
            }
            else
            {
                return View("Login");
            }
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult PostLogin(string username,string md5psw)
        {
            ///登陆成功
            if(username == "admin" && md5psw == EncryptHelp.GenerateMD5("shuizhuji123@"))
            {
                HttpCookie _userInfoCookies = new HttpCookie("userinfo");
                _userInfoCookies["UserName"] = username;
                _userInfoCookies["PassWord"] = "*******";
                Response.Cookies.Add(_userInfoCookies);
                return Json(ResultModel.Success("登陆成功"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ResultModel.Fail("用户名或者密码错误"), JsonRequestBehavior.AllowGet);
            }
        }
    }
}