using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapitest
{
    class Program
    {
        static string domain = "http://localhost:18634/";
        static void Main(string[] args)
        {
            Model model = new Model()
            {
                grant_type = "password",
                guest = "guest",
                password = "kWS/rVmF3u4="
            };
            var accesstoken = HttpHelper.HttpPost(domain + "token", new Dictionary<string, object>() {
                { "grant_type", "password" },
                { "guest", "guest" },
                { "password", "kWS/rVmF3u4=" },
            });
            //var accesstoken = HttpHelper.HttpPost(domain + "token", JsonConvert.SerializeObject(model));
        }
    }
}
