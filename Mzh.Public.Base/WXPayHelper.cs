using Mzh.Public.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WxPayAPI;

namespace Mzh.Public.Base
{
    public class WXPayHelper
    {
        public static string appid = ConfigurationManager.AppSettings["appid"];
        public static string appsecret = ConfigurationManager.AppSettings["appsecret"];
        public static string mch_id = ConfigurationManager.AppSettings["mch_id"];
        public static string apisecret = ConfigurationManager.AppSettings["apisecret"];
        public static string notify_url = ConfigurationManager.AppSettings["notify_url"];
        public static string refund_notify_url = ConfigurationManager.AppSettings["refund_notify_url"];

        private static string unifiedorderUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 调用统一下单接口
        /// </summary>
        public Tuple<bool, SortedDictionary<string, object>> unifiedorder(int oid,int pid,string body,string openid,decimal totalfee)
        {

            var RandomStr = GetRandomString(16);
            body = body.Length > 20 ? body.Substring(0, 20) : body;
            WxPayData data = new WxPayData();
            Dictionary<string, string> dic = new Dictionary<string, string>() {
                { "appid", appid},
                { "body", body.Replace("\r\n","")},
                { "mch_id", mch_id},
                { "nonce_str", RandomStr},
                { "notify_url", notify_url},
                { "openid", openid},
                { "out_trade_no", oid.ToString()},
                { "spbill_create_ip", GetPublicIp()},
                { "total_fee", ((int)(totalfee*100)).ToString() },
                //{ "total_fee", "1" },
                { "trade_type", "JSAPI" },
            };
            var requestXML = $@"<xml>
                                   <appid>{appid}</appid>
                                   <body>{body.Replace("\r\n", "")}</body>
                                   <mch_id>{mch_id}</mch_id>
                                   <nonce_str>{RandomStr}</nonce_str>
                                   <notify_url>http://wxpay.wxutil.com/pub_v2/pay/notify.v2.php</notify_url>
                                   <openid>oUpF8uMuAJO_M2pxb1Q9zNjWeS6o</openid>
                                   <out_trade_no>1415659990</out_trade_no>
                                   <spbill_create_ip>14.23.150.211</spbill_create_ip>
                                   <total_fee>1</total_fee>
                                   <trade_type>JSAPI</trade_type>
                                   <sign>0CB01533B8C1EF103065174F50BCA001</sign>
                                </xml>";

            data.SetValue("appid", appid);
            data.SetValue("body", body.Replace("\r\n", ""));//商品描述
            data.SetValue("mch_id", mch_id);
            data.SetValue("nonce_str", RandomStr);
            data.SetValue("notify_url", notify_url);//支付成功回调地址
            //data.SetValue("timestamp", GetTimeStamp());
            data.SetValue("openid", openid);//用户openid
            data.SetValue("out_trade_no", oid.ToString());//商家订单号
            data.SetValue("sign", GetSignString(dic));
            data.SetValue("spbill_create_ip", GetPublicIp());
            data.SetValue("total_fee", ((int)(totalfee * 100)).ToString());//交易金额
            //data.SetValue("total_fee", "1");//交易金额
            data.SetValue("trade_type", "JSAPI");
            //data.SetValue("product_id", pid.ToString());//商品ID
            
            //var b = data.CheckSign(WxPayData.SIGN_TYPE_MD5);
            //data.MakeSign(WxPayData.SIGN_TYPE_MD5);
            var a = data.ToXml();
            Logger._.Info("请求下单接口请求数据:" + data.ToJson());
            //WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            
            var response = HttpHelper.HttpPost(unifiedorderUrl, a);
            var responseDic = XMLHelper.FromXml(response);
            Logger._.Info("请求下单接口返回数据:" + response);
            //HttpService.Post(res)
            //if(result.GetValue("return_code").ToString() == "SUCCESS"&& result.GetValue("result_code").ToString() == "SUCCESS")
            //{
            //    return new Tuple<bool, WxPayData>(true, result);
            //}
            //return new Tuple<bool, WxPayData>(false, result);
            if (responseDic["return_code"].ToString() == "SUCCESS"&& responseDic["result_code"].ToString() == "SUCCESS")
            {
                return new Tuple<bool, SortedDictionary<string, object>>(true, responseDic);
            }
            return new Tuple<bool, SortedDictionary<string, object>>(false, responseDic);
        }

        #region 生成随机字符串 
        public static string GetRandomString(int CodeCount)
        {
            string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(allCharArray.Length - 1);
                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }
                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }
        #endregion

        #region 签名
        public string GetSignString(Dictionary<string, string> dic)
        {
            string key = apisecret;    //商户平台 API安全里面设置的KEY
                                                                                //排序  
            dic = dic.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
            //连接字段  
            var sign = dic.Aggregate("", (current, d) => current + (d.Key + "=" + d.Value + "&"));
            sign += "key=" + key;
            //MD5  
            //sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sign, "MD5").ToUpper();  
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sign))).Replace("-", null);
            return sign;
        }
        #endregion

        #region 时间戳
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion

        public static string GetLocalIp()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }

        /// <summary>
        /// 功能：获取本地的外网IP地址
        /// 作者：黄海
        /// 时间：2016-07-22
        /// </summary>
        /// <returns></returns>
        public static string GetPublicIp()
        {
            var urlList = new List<string>
            {
                "http://ip.qq.com/",
                "http://pv.sohu.com/cityjson?ie=utf-8",
                "http://ip.taobao.com/service/getIpInfo2.php?ip=myip"
            };
            var tempip = "";
            foreach (var a in urlList)
            {
                try
                {
                    var req = WebRequest.Create(a);
                    req.Timeout = 20000;
                    var response = req.GetResponse();
                    var resStream = response.GetResponseStream();
                    if (resStream != null)
                    {
                        var sr = new StreamReader(resStream, Encoding.UTF8);
                        var htmlinfo = sr.ReadToEnd();
                        //匹配IP的正则表达式
                        var r = new Regex("((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]\\d|\\d)\\.){3}(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]\\d|[1-9])", RegexOptions.None);
                        var mc = r.Match(htmlinfo);
                        //获取匹配到的IP
                        tempip = mc.Groups[0].Value;
                        resStream.Close();
                        sr.Close();
                        response.Dispose();
                    }
                    return tempip;
                }
                catch (Exception err)
                {
                    Console.WriteLine("当前探测URL:" + a + ",错误描述：" + err.ToString());
                }
            }
            return tempip;
        }
    }

    public class Code2SessionModel
    {
        public string openid { get; set; }
        public string session_key { get; set; }
        public string unionid { get; set; }
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }


}
