using Mzh.Public.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
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
        public Tuple<bool, WxPayData> unifiedorder(int oid,int pid,string body,string openid,decimal totalfee)
        {
            WxPayData data = new WxPayData();
            data.SetValue("appid", appid);
            data.SetValue("mch_id", mch_id);
            data.SetValue("device_info", "service");
            data.SetValue("nonce_str", GetRandomString(16));
            data.SetValue("timestamp", GetTimeStamp());
            data.SetValue("sign_type", WxPayData.SIGN_TYPE_MD5);
            data.SetValue("body", body);//商品描述
            data.SetValue("out_trade_no", oid);//商家订单号
            data.SetValue("total_fee", totalfee);//交易金额
            data.SetValue("spbill_create_ip", GetLocalIp());
            data.SetValue("product_id", pid);//商品ID
            data.SetValue("notify_url", notify_url);//支付成功回调地址
            data.SetValue("openid", openid);//用户openid
            data.SetValue("sign", data.MakeSign());

            Logger._.Info("请求下单接口请求数据:" + data.ToJson());
            WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            Logger._.Info("请求下单接口返回数据:" + result.ToJson());
            //HttpService.Post(res)
            if(result.GetValue("return_code").ToString() == "SUCCESS"&& result.GetValue("result_code").ToString() == "SUCCESS")
            {
                return new Tuple<bool, WxPayData>(true, result);
            }
            return new Tuple<bool, WxPayData>(false, result);
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
            string key = ConfigurationManager.AppSettings["key"].ToString();    //商户平台 API安全里面设置的KEY
                                                                                //排序  
            dic = dic.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);
            //连接字段  
            var sign = dic.Aggregate("", (current, d) => current + (d.Key + "=" + d.Value + "&"));
            sign += "key=" + key;
            //MD5  
            // sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sign, "MD5").ToUpper();  
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
