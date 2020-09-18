using Mzh.Public.DAL;
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
    public class BoxController : ApiController
    {
        /// <summary>
        /// 获取包厢列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<bsp_boxes> GetBoxes()
        {
            BoxCache bcache = RemotingHelp.GetModelObject<BoxCache>();
            return bcache.GetBoxes();
        }
    }
}
