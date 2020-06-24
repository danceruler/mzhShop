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
        /// 获取商品列表
        /// </summary>
        /// <returns></returns>
        public List<ShowProductList> GetProductList()
        {
            return ProductList;
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
            List<ShowProductList> _ProductList = new List<ShowProductList>();
            string sql = $@"SELECT bsp_categories.cateid,bsp_categories.name catecategoryName,
	                           bsp_products.pid,bsp_products.name,bsp_products.shopprice,bsp_products.marketprice,bsp_products.costprice,bsp_products.state,
	                           bsp_products.ishot,bsp_products.isnew,bsp_products.displayorder,bsp_products.weight,bsp_products.showimg,bsp_products.salecount,
	                           bsp_products.visitcount,bsp_products.reviewcount,bsp_products.addtime,bsp_products.description,bsp_products.isfullcut,
	                           bsp_productskus.recordid sku_recordid,bsp_productskus.pid sku_pid,bsp_productskus.attrid sku_attrid,bsp_attributevalues.attrvalueid sku_attrvalueid,
	                           bsp_attributevalues.attrvalue sku_inputvalue,bsp_attributes.name sku_inputattr,bsp_productskus.price sku_price,bsp_productskus.isdefaultprice sku_isdefaultprice
                        FROM bsp_products
                        JOIN bsp_categories ON bsp_categories.cateid = bsp_products.cateid
                        JOIN bsp_productskus ON bsp_productskus.pid = bsp_products.pid
                        LEFT JOIN bsp_attributes ON bsp_attributes.attrid = bsp_productskus.attrid
                        LEFT JOIN bsp_attributevalues ON bsp_attributevalues.attrvalueid = bsp_productskus.attrvalueid
                        WHERE bsp_products.state = 1
                        ORDER BY bsp_categories.displayorder DESC,dbo.bsp_products.displayorder DESC";
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, sql);

            var cateids = dt.AsEnumerable().Select(t => int.Parse(t["cateid"].ToString())).Distinct().ToList();

            foreach(var cateid in cateids)
            {
                ShowProductList showProductList = new ShowProductList();
                showProductList.cateid = cateid;
                showProductList.catecategoryName = dt.Select($"cateid = {cateid}")[0]["catecategoryName"].ToString();
                var dtProduct = dt.Select($"cateid = {cateid}").CopyToDataTable();
                showProductList.productInfos = dtProduct.GetList<ShowProductInfo>("").Distinct(new DistinctModel<ShowProductInfo>()).ToList();
                foreach(var productInfo in showProductList.productInfos)
                {
                    var dtSku = dtProduct.Select($"pid = {productInfo.pid}").CopyToDataTable();
                    productInfo.skuCount = dtSku.Rows.Count;
                    productInfo.skuInfos = dtSku.GetList<ShowSkuInfo>("").Distinct(new DistinctModel<ShowSkuInfo>()).ToList();
                    productInfo.mainImgs = ProductMainImgDic[productInfo.pid];
                    productInfo.detailImgs = ProductDetailImgDic[productInfo.pid];
                }
                _ProductList.Add(showProductList);
            }
            ProductList = _ProductList;
        }

        /// <summary>
        /// 将商品图片信息写入缓存
        /// </summary>
        public static void InitProductImgDic()
        {
            string sql = $@"SELECT* FROM dbo.bsp_productimages";
            SqlCommand cmd = new SqlCommand(sql);
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, sql);
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

    }
}
