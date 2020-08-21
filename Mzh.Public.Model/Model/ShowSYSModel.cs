using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    /// <summary>
    /// 商家信息和系统配置类
    /// </summary>
    [Serializable]
    public class ShowSYSModel
    {
        /// <summary>
        /// 商家地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 商家名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 商家描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 商家介绍图片
        /// </summary>
        public string showimg { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal longitude { get; set; }
        /// <summary>
        /// 可以配送的范围半径
        /// </summary>
        public decimal canSendRadius { get; set; }
        /// <summary>
        /// 营业时间描述文字
        /// </summary>
        public string businessTimeStr { get; set; }
        /// <summary>
        /// 每日开始营业时间的秒数，比如3600表示1点开业
        /// </summary>
        public int businessStart { get; set; }
        /// <summary>
        /// 每日结束营业的时间,比如3600*23表示23点打样
        /// </summary>
        public int businessEnd { get; set; }

    }
}
