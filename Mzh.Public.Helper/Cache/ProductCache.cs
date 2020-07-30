using Mzh.Public.Base;
using Mzh.Public.BLL.Cache;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Remoting
{
    public class ProductCache : MarshalByRefObject, ICache
    {
        public static List<ShowProductList> ProductList = new List<ShowProductList>();

        public static Dictionary<int, List<ShowProductImg>> ProductMainImgDic = new Dictionary<int, List<ShowProductImg>>();

        public static Dictionary<int, List<ShowProductImg>> ProductDetailImgDic = new Dictionary<int, List<ShowProductImg>>();

        public void Init()
        {
            InitProductImgDic();
            InitProductList();
        }

        /// <summary>
        /// 获取商品列表(条件)
        /// </summary>
        /// <returns></returns>
        public List<ShowProductList> GetProductList(int cateid = 0,int isonsale = -1)
        {
            string sqlWhere = "";
            if (cateid > 0) sqlWhere += $" AND bsp_cateproducts.cateid = {cateid} ";
            if(isonsale > -1) sqlWhere += $" AND bsp_products.state = {(byte)isonsale} ";
            return GetProductBySql(sqlWhere);
        }

        /// <summary>
        /// 从内存获取商品列表
        /// </summary>
        /// <returns></returns>
        public List<ShowProductList> GetProductListFromCache()
        {
            return ProductList;
        }

        /// <summary>
        /// 从内存获取商品详情
        /// </summary>
        /// <returns></returns>
        public ShowProductInfo GetProductInfoFromCache(int cateid,int pid)
        {
            return ProductList.FirstOrDefault(t => t.cateid == cateid).productInfos.FirstOrDefault(t => t.pid == pid);
        }

        /// <summary>
        /// 获取商品图片
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="ismain"></param>
        /// <returns></returns>
        public List<ShowProductImg> GetProductImgs(int pid,int ismain)
        {
            if (ismain == 1) return ProductMainImgDic[pid];
            else return ProductDetailImgDic[pid];
        }

        /// <summary>
        /// 将商品列表写入缓存
        /// </summary>
        public static void InitProductList()
        {
            ProductList = GetProductBySql("");
        }

        /// <summary>
        /// 将商品图片信息写入缓存
        /// </summary>
        public static void InitProductImgDic()
        {
            string sql = $@"SELECT* FROM dbo.bsp_productimages";
            SqlCommand cmd = new SqlCommand(sql);
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, cmd);
            DataRow[] drs = dt.Select($"ismain = 1");
            ProductMainImgDic.Clear();
            if (drs.Length > 0)
            {
                var mainimgDt = drs.CopyToDataTable();
                var pids = mainimgDt.AsEnumerable().Select(t => int.Parse(t["pid"].ToString())).Distinct().ToList();
                foreach(var pid in pids)
                {
                    ProductMainImgDic.Add(pid,
                        mainimgDt.Select($"pid = {pid}").CopyToDataTable().GetList<ShowProductImg>("").OrderBy(t => t.displayorder).ToList());
                }
            }
            drs = dt.Select($"ismain = 0");
            ProductDetailImgDic.Clear();
            if (drs.Length > 0)
            {
                var detailimgDt = drs.CopyToDataTable();
                var pids = detailimgDt.AsEnumerable().Select(t => int.Parse(t["pid"].ToString())).Distinct().ToList();
                foreach (var pid in pids)
                {
                    ProductDetailImgDic.Add(pid,
                        detailimgDt.Select($"pid = {pid}").CopyToDataTable().GetList<ShowProductImg>("").OrderBy(t => t.displayorder).ToList());
                }
            }
        }

        private static List<ShowProductList> GetProductBySql(string sqlWhere)
        {
            List<ShowProductList> _ProductList = new List<ShowProductList>();
            string sql = $@"SELECT bsp_categories.cateid,bsp_categories.name catecategoryName,
	                           bsp_products.pid,bsp_products.name,bsp_products.shopprice,bsp_products.marketprice,bsp_products.costprice,bsp_products.state,bsp_products.isbest,
	                           bsp_products.ishot,bsp_products.isnew,bsp_products.displayorder,bsp_products.weight,bsp_products.showimg,bsp_products.salecount,
	                           bsp_products.visitcount,bsp_products.reviewcount,bsp_products.addtime,bsp_products.description,bsp_products.isfullcut,
	                           bsp_productskus.recordid sku_recordid,bsp_productskus.skuguid sku_guid,bsp_productskus.pid sku_pid,bsp_productskus.attrid sku_attrid,bsp_attributevalues.attrvalueid sku_attrvalueid,bsp_attributes.remark sku_attrremark,
	                           bsp_attributevalues.attrvalue sku_inputvalue,bsp_attributes.name sku_inputattr,bsp_productskus.price sku_price,bsp_productskus.isdefaultprice sku_isdefaultprice,bsp_productskus.stock sku_stock
                        FROM bsp_cateproducts 
						JOIN bsp_products ON bsp_products.pid = bsp_cateproducts.pid
                        JOIN bsp_categories ON bsp_categories.cateid = bsp_cateproducts.cateid
                        JOIN bsp_productskus ON bsp_productskus.pid = bsp_products.pid
                        LEFT JOIN bsp_attributes ON bsp_attributes.attrid = bsp_productskus.attrid
                        LEFT JOIN bsp_attributevalues ON bsp_attributevalues.attrvalueid = bsp_productskus.attrvalueid
                        WHERE bsp_products.isdelete = 0 {sqlWhere}
                        ORDER BY bsp_categories.displayorder DESC,dbo.bsp_products.displayorder DESC";
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));

            var cateids = dt.AsEnumerable().Select(t => int.Parse(t["cateid"].ToString())).Distinct().ToList();

            foreach (var cid in cateids)
            {
                ShowProductList showProductList = new ShowProductList();
                showProductList.cateid = cid;
                showProductList.catecategoryName = dt.Select($"cateid = {cid}")[0]["catecategoryName"].ToString();
                var dtProduct = dt.Select($"cateid = {cid}").CopyToDataTable();
                showProductList.productInfos = dtProduct.GetList<ShowProductInfo>("").Distinct(new DistinctModel<ShowProductInfo>()).ToList();
                foreach (var productInfo in showProductList.productInfos)
                {
                    productInfo.attributes = new List<AttributeInfo>();
                    var dtSku = dtProduct.Select($"pid = {productInfo.pid}").CopyToDataTable();
                    var tempskuinfo = dtSku.GetList<TempShowSkuInfo>("");
                    var attrids = tempskuinfo.Select(t => t.sku_attrid).Distinct();
                    foreach (var attrid in attrids)
                    {
                        AttributeInfo attr = new AttributeInfo();
                        attr.attrid = attrid;
                        attr.name = tempskuinfo.FirstOrDefault(t => t.sku_attrid == attrid).sku_inputattr.Trim();
                        attr.remark = tempskuinfo.FirstOrDefault(t => t.sku_attrid == attrid).sku_attrremark;
                        attr.attributevalues = new List<AttributeValueInfo>();
                        var valueids = tempskuinfo.Where(t => t.sku_attrid == attrid).Select(t => t.sku_attrvalueid).Distinct();
                        foreach (var valueid in valueids)
                        {
                            var valueitem = tempskuinfo.FirstOrDefault(t => t.sku_attrvalueid == valueid);
                            AttributeValueInfo value = new AttributeValueInfo();
                            value.attrname = attr.name;
                            value.attrvalue = valueitem.sku_inputvalue.Trim();
                            value.attrvalueid = valueid;
                            value.price = valueitem.sku_isdefaultprice == 1 ? productInfo.shopprice : valueitem.sku_price.Value;
                            value.stock = valueitem.sku_stock;
                            attr.attributevalues.Add(value);
                        }
                        productInfo.attributes.Add(attr);
                    }


                    var sku_guids = tempskuinfo.Select(t => t.sku_guid).Distinct();
                    productInfo.skuCount = sku_guids.Count();
                    productInfo.skuInfos = new List<ShowSkuInfo>();
                    foreach (var sku_guid in sku_guids)
                    {
                        var skuitems = tempskuinfo.Where(t => t.sku_guid == sku_guid).ToList();
                        ShowSkuInfo skuinfo = new ShowSkuInfo();
                        skuinfo.sku_guid = sku_guid.ToString();
                        skuinfo.sku_price = skuitems[0].sku_price;
                        skuinfo.sku_isdefaultprice = skuitems[0].sku_isdefaultprice;
                        skuinfo.sku_stock = skuitems[0].sku_stock;
                        string sku_input = "";
                        foreach (var item in skuitems)
                        {
                            sku_input += $@"{item.sku_inputattr.Trim()}-{item.sku_inputvalue.Trim()} ";
                        }
                        skuinfo.sku_input = sku_input;
                        skuinfo.attrvalueids = skuitems.Select(t => t.sku_attrvalueid).ToList();
                        productInfo.skuInfos.Add(skuinfo);
                    }
                    productInfo.mainImgs = ProductMainImgDic.Keys.Contains(productInfo.pid) ? ProductMainImgDic[productInfo.pid] : new List<ShowProductImg>();
                    productInfo.detailImgs = ProductDetailImgDic.Keys.Contains(productInfo.pid) ? ProductDetailImgDic[productInfo.pid] : new List<ShowProductImg>();
                }
                _ProductList.Add(showProductList);
            }
            return _ProductList;
        }
    }
}
