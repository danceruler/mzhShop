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
    public class BannerController : ApiController
    {
        /// <summary>
        /// 获取首页轮播图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<ShowBannerInfo> GetBanners()
        {
            BannerCache bannerCache = RemotingHelp.GetModelObject<BannerCache>();
            return bannerCache.GetBanners();
        }
    }
}
