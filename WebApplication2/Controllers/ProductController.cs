﻿using Mzh.Public.Base;
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
        /// 小程序获取商品列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<ShowProductList> GetProductListFromCache()
        {
            ProductCache productCache = RemotingHelp.GetModelObject<ProductCache>();
            return productCache.GetProductListFromCache();
        }


    }
}
