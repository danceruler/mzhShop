using Mzh.Public.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class Hello : MarshalByRefObject
    {
        [SoapMethod(XmlNamespace = "Remoting", SoapAction = "Remoting#Greeting")]
        public string Greeting(string word)
        {
            Console.WriteLine("打个招呼吧,我是服务端！" + word);
            return String.Format("Hello {0},are you ok?", word);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="type">1.WEBAPI 2.WENADMIN</param>
        /// <returns></returns>
        public bool CheckClient(string clientId,int type)
        {
            if (AppConfig.WebApiId == EncryptHelp.Decrypt3Des(clientId) && type == 1) return true;
            if (AppConfig.WebAdminId == EncryptHelp.Decrypt3Des(clientId) && type == 2) return true;
            return false;
        }

        public void Test()
        {
            var temp = AppConfig.ConnectionString;
        }

        public static void InitAll()
        {
            ProductCache productCache = new ProductCache();
            productCache.Init();
            BoxCache boxCache = new BoxCache();
            boxCache.Init();
            BannerCache bannerCache = new BannerCache();
            bannerCache.Init();
            BusinessCache businessCache = new BusinessCache();
            businessCache.InitBusinessCache();
        }
    }
}
