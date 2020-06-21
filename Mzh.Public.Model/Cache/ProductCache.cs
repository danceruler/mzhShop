using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Cache
{
    public class ProductCache : ICache
    {
        public static List<ShowProductInfo> ProductList = new List<ShowProductInfo>();

        public void Init()
        {
            InitProductList();
        }

        /// <summary>
        /// 将商品列表写入缓存
        /// </summary>
        public static void InitProductList()
        {

        }
    }
}
