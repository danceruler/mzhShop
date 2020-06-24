using Mzh.Public.Model.Model;
using Newtonsoft.Json;
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
    public class ValuesController : ApiController
    {
        [HttpGet]
        public object Menus()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var jsontext = System.IO.File.ReadAllText($"{baseDir}\\json\\init.json");
            return JsonConvert.DeserializeObject(jsontext);
        }
    }
}
