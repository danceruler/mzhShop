using Mzh.Public.Base;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Cache
{
    public class ProductCache : ICache
    {
        public static List<ShowProductList> ProductList = new List<ShowProductList>();

        public void Init()
        {
            InitProductList();
        }

        /// <summary>
        /// 将商品列表写入缓存
        /// </summary>
        public static void InitProductList()
        {
            string sql = $@"SELECT bsp_categories.cateid,bsp_categories.name catecategoryName,
	                           bsp_products.pid,bsp_products.name,bsp_products.shopprice,bsp_products.marketprice,bsp_products.costprice,bsp_products.state,
	                           bsp_products.ishot,bsp_products.isnew,bsp_products.displayorder,bsp_products.weight,bsp_products.showimg,bsp_products.salecount,
	                           bsp_products.visitcount,bsp_products.reviewcount,bsp_products.addtime,bsp_products.description,bsp_products.isfullcut,
	                           bsp_productskus.recordid sku_recordid,bsp_productskus.pid sku_pid,bsp_productskus.attrid sku_attrid,bsp_attributevalues.attrvalueid sku_attrvalueid,
	                           bsp_attributevalues.attrvalue sku_inputvalue,bsp_attributes.name sku_inputattr,bsp_productskus.price sku_price,bsp_productskus.isdefaultprice sku_isdefaultprice
                        FROM bsp_products
                        JOIN bsp_categories ON bsp_categories.cateid = bsp_products.cateid
                        LEFT JOIN bsp_productskus ON bsp_productskus.pid = bsp_products.pid
                        LEFT JOIN bsp_attributes ON bsp_attributes.attrid = bsp_productskus.attrid
                        LEFT JOIN bsp_attributevalues ON bsp_attributevalues.attrvalueid = bsp_productskus.attrvalueid
                        WHERE bsp_products.state = 1";
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, sql);

            var cateids = dt.AsEnumerable().Select(t => int.Parse(t["cateid"].ToString())).Distinct().ToList();

            foreach(var cateid in cateids)
            {
                ShowProductList showProductList = new ShowProductList();
                showProductList.cateid = cateid;
                showProductList.catecategoryName = dt.Select($"cateid = {cateid}")[0]["catecategoryName"].ToString();
                var dtProduct = dt.Select($"cateid = {cateid}").CopyToDataTable();
                showProductList.productInfos = dtProduct.GetList<ShowProductInfo>("").Distinct(new DistinctTest<ShowProductInfo>()).ToList();
                //foreach(var dr in drs)
                //{
                //    ShowProductInfo showProductInfo = new ShowProductInfo();
                //    showProductInfo.addtime = DateTime.Parse(dr["addtime"].ToString());
                //    showProductInfo.cateid = (short)cateid;
                //    showProductInfo.costprice = decimal.Parse(dr["costprice"].ToString());
                //    showProductInfo.description = dr["description"].ToString();
                //    showProductInfo.displayorder = int.Parse(dr["displayorder"].ToString());
                //    show
                //}
            }
        }


        class DistinctTest<TModel> : IEqualityComparer<TModel>
        {
            public bool Equals(TModel x, TModel y)
            {
                //Test
                ShowProductInfo t = x as ShowProductInfo;
                ShowProductInfo tt = y as ShowProductInfo;
                if (t != null && tt != null) return t.pid == tt.pid;
                return false;
            }

            public int GetHashCode(TModel obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
