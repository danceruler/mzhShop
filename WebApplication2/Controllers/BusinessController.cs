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
    public class BusinessController : ApiController
    {
        public ResultModel AddOrUpdateBusiness(BusinessModel businessModel)
        {
            BusinessCache bcache = RemotingHelp.GetModelObject<BusinessCache>();
            return bcache.AddOrUpdateBusiness(businessModel);
        }

        public ResultModel GetBusiness()
        {
            BusinessCache bcache = RemotingHelp.GetModelObject<BusinessCache>();
            return bcache.GetBusiness();
        }
    }
}
