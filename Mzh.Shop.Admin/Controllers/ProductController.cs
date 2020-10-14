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

        public ActionResult Product(int cateid = 0,int isonsale = -1)
        {
            ProductCache pcache = RemotingHelp.GetModelObject<ProductCache>();
            ViewBag.productList = pcache.GetProductList(cateid, isonsale);
            ViewBag.cateid = cateid;
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            ViewBag.cates = product.GetCategories();
            ViewBag.isonsale = isonsale;
            return View();
        }

        public ActionResult ProductAdd()
        {
            return View();
        }

        public ActionResult ProductEdit(int cateid,int pid)
        {
            ProductCache pcache = RemotingHelp.GetModelObject<ProductCache>();
            ViewBag.ProductInfo = pcache.GetProductInfoFromCache(cateid, pid);
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            ViewBag.allcates = product.GetCategories();
            ViewBag.pcates = product.GetCategories(pid);
            return View();
        }

        public ActionResult ProductEditSKU(int cateid,int pid,string pname)
        {
            ViewBag.cateid = cateid;
            ViewBag.pid = pid;
            ViewBag.pname = pname;
            return View();
        }

        public ActionResult ProductEditSKUDetail(string skuguid,string sku_input, int isdefaultprice,decimal price,int stock)
        {
            ViewBag.skuguid = skuguid;
            ViewBag.sku_input = sku_input;
            ViewBag.isdefaultprice = isdefaultprice;
            ViewBag.price = price;
            ViewBag.stock = stock;
            return View();
        }

        public ActionResult SkuAddAttribute()
        {
            SKU sku = RemotingHelp.GetModelObject<SKU>();
            ViewBag.Attributes = sku.GetAttributeInfos();
            return View();
        }

        /// <summary>
        /// 添加编辑属性值
        /// </summary>
        /// <returns></returns>
        public ActionResult SkuAddValue(int attrid,string attributename,int valueid = 0,string value = "", string price = "-1", string stock = "-1")
        {
            SKU sku = RemotingHelp.GetModelObject<SKU>();
            ViewBag.attrid = attrid;
            ViewBag.attributename = attributename;
            ViewBag.value = value;
            ViewBag.price = price;
            ViewBag.stock = stock;
            ViewBag.valueid = valueid;
            ViewBag.values = sku.GetAttributeValueInfos(attrid);
            return View();
        }

        #region 接口
        [HttpGet]
        public ActionResult GetCategories(int pid = 0)
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            var list = product.GetCategories(pid);
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

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProduct(AddProductModel model)
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            return Json(
                product.AddProduct(model),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePorduct(AddProductModel model)
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            return Json(
                product.UpdatePorduct(model),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取已经存在的属性列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAttributeInfos()
        {
            SKU sku = RemotingHelp.GetModelObject<SKU>();
            return Json(
                sku.GetAttributeInfos(),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取已经存在的属性值列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAttributeValueInfos(int attrid)
        {
            SKU sku = RemotingHelp.GetModelObject<SKU>();
            return Json(
                sku.GetAttributeValueInfos(attrid),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除商品的规格信息
        /// </summary>
        /// <param name="type">1表示删除某个属性的规格，2为删除某个属性值的规格</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteProductSku(int pid, int type, int id)
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            return Json(
                product.DeleteProductSku(pid,type,id),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Deleteproduct(int pid)
        {
            PRODUCT product = RemotingHelp.GetModelObject<PRODUCT>();
            return Json(
                product.Deleteproduct(pid),
                JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取商品的sku信息
        /// </summary>
        /// <param name="cateid"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetProductSKUInfoFromCache(int cateid,int pid)
        {
            ProductCache pcache = RemotingHelp.GetModelObject<ProductCache>();
            var skuinfos = pcache.GetProductInfoFromCache(cateid, pid).skuInfos;
            List<ShowSkuInfoForLayuiTable> list = new List<ShowSkuInfoForLayuiTable>();
            foreach(var skuinfo in skuinfos)
            {
                ShowSkuInfoForLayuiTable newitem = new ShowSkuInfoForLayuiTable() {
                    sku_guid = skuinfo.sku_guid,
                    sku_input = skuinfo.sku_input,
                    sku_isdefaultprice = skuinfo.sku_isdefaultprice,
                    sku_isdefaultprice_text = skuinfo.sku_isdefaultprice == 0 ? "否" : "是",
                    sku_price = skuinfo.sku_price,
                    sku_price_text = skuinfo.sku_price == -1 ? "默认" : skuinfo.sku_price.ToString(),
                    sku_stock = skuinfo.sku_stock,
                    sku_stock_text = skuinfo.sku_stock == -1 ? "默认" : skuinfo.sku_stock.ToString()
                };
                list.Add(newitem);
            }
            return Json(
                new LayuiTableApiResult()
                {
                    code = 0,
                    msg = "",
                    count = list.Count,
                    data = list
                },
               JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新sku信息
        /// </summary>
        /// <param name="skuguid"></param>
        /// <param name="isdefaultprice"></param>
        /// <param name="price"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateSku(string skuguid, int isdefaultprice, decimal price, int stock)
        {
            SKU sku = RemotingHelp.GetModelObject<SKU>();
            return Json(sku.UpdateSku(skuguid, isdefaultprice, price, stock),
                JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class ShowSkuInfoForLayuiTable: ShowSkuInfo
    {
        public string sku_isdefaultprice_text { get; set; }
        public string sku_price_text { get; set; }
        public string sku_stock_text { get; set; }
    }
}