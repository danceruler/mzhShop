using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    /// <summary>
    /// 统计数据类
    /// </summary>
    [Serializable]
    public class StatisticsModel
    {
        /// <summary>
        /// 营业额
        /// </summary>
        public decimal TurnOver { get; set; }
        /// <summary>
        /// 已完成订单数
        /// </summary>
        public int FinishOrderCount { get; set; }
        /// <summary>
        /// 订单数据
        /// </summary>
        public List<ShowOrderInfo> OrderList { get; set; }
    }
}
