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

        public ActionResult ProductTypeAdd(int cateid = 0)
        {
            ViewBag.cateid = cateid;
            ViewBag.cate = null;
            if(cateid > 0)
            {
                PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
                ViewBag.cate = product.GetCategories().Where(t => t.cateid == cateid).SingleOrDefault();
            }
            return View();
        }

        public ActionResult Product(int cateid = 0)
        {
            ProductCache pcache = RemotingHelp.GetModelObject<ProductCache>();
            ViewBag.productList = pcache.GetProductList();
            ViewBag.cateid = cateid;
            return View();
        }

        public ActionResult ProductAdd()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCategories()
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            var list = product.GetCategories();
            return Json(
                new LayuiTableApiResult() { 
                    code = 0,
                    msg = "",
                    count = list.Count,
                    data = list
                }, 
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCateGory(string name, int displayorder = 0)
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            return Json(
               product.AddCateGory(name,displayorder),
               JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="cateid"></param>
        /// <returns></returns>
        public ActionResult DeleteCateGory(int cateid)
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            return Json(
               product.DeleteCateGory(cateid),
               JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        /// <summary>
        /// 批量删除分类
        /// </summary>
        /// <param name="cateids"></param>
        /// <returns></returns>
        public ActionResult DeleteCateGories(int[] cateids)
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            return Json(
               product.DeleteCateGories(cateids),
               JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        /// <summary>
        /// 修改商品分类信息
        /// </summary>
        /// <param name="cateid"></param>
        /// <param name="name"></param>
        /// <param name="displayorder"></param>
        /// <returns></returns>
        public ActionResult UpdateCateGory(int cateid, string name, int displayorder)
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            return Json(
                product.UpdateCateGory(cateid,name,displayorder),
                JsonRequestBehavior.AllowGet);
        }


    }
}