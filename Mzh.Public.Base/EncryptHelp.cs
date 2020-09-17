using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Base
{
    public class EncryptHelp
    {

        #region 3des加密

        /// <summary>
        /// 3des ecb模式加密
        /// </summary>
        /// <param name="aStrString">待加密的字符串</param>
        /// <param name="aStrKey">密钥</param>
        /// <param name="iv">加密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="mode">运算模式</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt3Des(string aStrString, string aStrKey = "12345678qwertyui87654321", CipherMode mode = CipherMode.ECB, string iv = "1234abcd")
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(aStrKey),
                    Mode = mode
                };
                if (mode == CipherMode.CBC)
                {
                    des.IV = Encoding.UTF8.GetBytes(iv);
                }
                des.Padding = PaddingMode.PKCS7;
                var desEncrypt = des.CreateEncryptor();
                byte[] buffer = Encoding.UTF8.GetBytes(aStrString);
                return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        #endregion

        #region 3des解密

        /// <summary>
        /// des 解密
        /// </summary>
        /// <param name="aStrString">加密的字符串</param>
        /// <param name="aStrKey">密钥</param>
        /// <param name="iv">解密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="mode">运算模式</param>
        /// <returns>解密的字符串</returns>
        public static string Decrypt3Des(string aStrString, CipherMode mode = CipherMode.ECB, string aStrKey = "12345678qwertyui87654321", string iv = "1234abcd")
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(aStrKey),
                    Mode = mode,
                    Padding = PaddingMode.PKCS7
                };
                if (mode == CipherMode.CBC)
                {
                    des.IV = Encoding.UTF8.GetBytes(iv);
                }
                var desDecrypt = des.CreateDecryptor();
                var result = "";
                byte[] buffer = Convert.FromBase64String(aStrString);
                result = Encoding.UTF8.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                return result;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        #endregion


        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }


        public static List<string> randomWords = new List<string>();

        /// <summary>
        /// 获取随机字符串（由a-z,A-Z组成的随机字符串）
        /// </summary>
        /// <returns></returns>
        public static string GetRandomStr(int length)
        {
            if(randomWords.Count == 0)
            {
                for(int i = 0;i< 26; i++)
                {
                    randomWords.Add(((char)('A' + i)).ToString());
                    randomWords.Add(((char)('a' + i)).ToString());
                }
            }

            string result = "";
            for(int i = 0; i < length; i++)
            {
                var index = new Random(Guid.NewGuid().GetHashCode()).Next(0, randomWords.Count - 1);
                result += randomWords[index];
            }
            return result;
        }

        /// <summary>
        /// 微信支付MD5签名算法，ASCII码字典序排序0,A,B,a,b
        /// </summary>
        /// <param name="InDict">待签名名键值对</param>
        /// <param name="TenPayV3_Key">用于签名的Key</param>
        /// <returns>MD5签名字符串</returns>
        public static string EncryptMD5(string aStr)
        {
            MD5 md5 = MD5.Create();
            //需要将字符串转成字节数组
            byte[] buffer = Encoding.Default.GetBytes(aStr);
            //加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择
            byte[] md5buffer = md5.ComputeHash(buffer);
            string str = string.Empty;
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            foreach (byte b in md5buffer)
            {
                //得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                //但是在和对方测试过程中，发现我这边的MD5加密编码，经常出现少一位或几位的问题；
                //后来分析发现是 字符串格式符的问题， X 表示大写， x 表示小写， 
                //X2和x2表示不省略首位为0的十六进制数字；
                str += b.ToString("x2");
            }
            return str;
        }
    }
}
