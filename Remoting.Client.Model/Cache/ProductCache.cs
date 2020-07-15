using Mzh.Public.Base;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class ProductCache : MarshalByRefObject
    {
        public static List<ShowProductList> ProductList = new List<ShowProductList>();
        public void Init()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <returns></returns>
        public List<ShowProductList> GetProductList(int cateid = 0, int isonsale = -1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从内存获取商品列表
        /// </summary>
        /// <returns></returns>
        public List<ShowProductList> GetProductListFromCache()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从内存获取商品详情
        /// </summary>
        /// <returns></returns>
        public ShowProductInfo GetProductInfoFromCache(int cateid, int pid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取商品图片
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="ismain"></param>
        /// <returns></returns>
        public List<ShowProductImg> GetProductImgs(int pid, int ismain)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将商品列表写入缓存
        /// </summary>
        public static void InitProductList()
        {
            throw new NotImplementedException();
        }

    }
}
