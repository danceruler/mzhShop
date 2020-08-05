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
    public class BaseStatisticsModel
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
        /// 订单类别统计信息
        /// </summary>
        public List<OrderTypeStatistic> OrderTypeStatistics { get; set; }
        /// <summary>
        /// 商品评价统计信息
        /// </summary>
        public List<OrderEvaluationStatistic> OrderEvaluationStatistic { get; set; }
        /// <summary>
        /// 时间订单统计信息
        /// </summary>
        public List<OrderCountByTimeStatistic> OrderCountByTimeStatistics { get; set; }
    }

    /// <summary>
    /// 当天统计数据类
    /// </summary>
    [Serializable]
    public class DayStatisticsModel : BaseStatisticsModel
    {
        
        /// <summary>
        /// 对比昨天相同时间营业额的差值
        /// </summary>
        public decimal TurnOverByYestoday { get; set; }
        /// <summary>
        /// 对比平均营业额差值
        /// </summary>
        public decimal TurnOverByAverage { get; set; }
    }

    /// <summary>
    /// 周统计数据类
    /// </summary>
    [Serializable]
    public class WeekStatisticsModel : BaseStatisticsModel
    {
        /// <summary>
        /// 对比上周相同时间营业额的差值
        /// </summary>
        public decimal TurnOverByLastWeek { get; set; }
        /// <summary>
        /// 平均每天营业额
        /// </summary>
        public decimal DayAverageTurnOver { get; set; }
    }

    /// <summary>
    /// 月统计数据类
    /// </summary>
    [Serializable]
    public class MonthStatisticsModel : BaseStatisticsModel
    {
        /// <summary>
        /// 对比上月相同时间营业额的差值
        /// </summary>
        public decimal TurnOverByLastMonth { get; set; }
        /// <summary>
        /// 平均每天营业额
        /// </summary>
        public decimal DayAverageTurnOver { get; set; }
    }

    /// <summary>
    /// 订单类别统计类
    /// </summary>
    [Serializable]
    public class OrderTypeStatistic
    {
        /// <summary>
        /// 类别
        /// </summary>
        public OrderType OrderType { get; set; }
        /// <summary>
        /// 类别名
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 商品评价统计类
    /// </summary>
    [Serializable]
    public class OrderEvaluationStatistic
    {
        /// <summary>
        /// 星级
        /// </summary>
        public int Star { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 时间订单统计类(用于绘制图表)
    /// </summary>
    [Serializable]
    public class OrderCountByTimeStatistic
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 时间字符串（用于显示）
        /// </summary>
        public string TimeStr { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal Amount { get; set; }
    }


}
