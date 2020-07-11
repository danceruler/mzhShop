using Mzh.Public.Model.Model;
using Remoting;
using Remoting.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    public class ProductController : ApiController
    {
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<ShowProductList> GetProductList()
        {
            ProductCache productCache = RemotingHelp.GetModelObject<ProductCache>();
            return productCache.GetProductList();
        }

        /// <summary>
        /// 新增编辑商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public AddProductModel AddProduct()
        {
            return new AddProductModel();
        }
    }
}
