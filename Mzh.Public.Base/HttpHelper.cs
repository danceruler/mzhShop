using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Mzh.Public.Base
{
    public class HttpHelper
    {
        public static string HttpGet(string Url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (param == "" ? "" : "?") + param);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string HttpPost(string url, string param)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            Encoding encoding = Encoding.UTF8;
            byte[] byteArray = Encoding.UTF8.GetBytes(param);
            string responseData = String.Empty;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = byteArray.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
            }
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    responseData = reader.ReadToEnd();
                }
                return responseData;
            }
        }

        public static string HttpPost(string url, Dictionary<string, object> param)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            Encoding encoding = Encoding.UTF8;
            byte[] byteArray = Encoding.UTF8.GetBytes(new JavaScriptSerializer().Serialize(param));
            string responseData = String.Empty;
            req.Method = "POST";
            req.ContentType = "application/json;charset=UTF-8";
            req.ContentLength = byteArray.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
            }
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    responseData = reader.ReadToEnd();
                }
                return responseData;
            }
        }
    }
}
