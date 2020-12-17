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
        /// <summary>
        /// 对比昨天订单量的差值
        /// </summary>
        public decimal OrderCountByYestoday { get; set; }
        /// <summary>
        /// 对比平均订单量差值
        /// </summary>
        public decimal OrderCountByAverage { get; set; }

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
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal SUM { get; set; }
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

    /// <summary>
    /// 同数据库表orderstatistic结构
    /// </summary>
    public class OrderStatistic
    {
        public int id { get; set; }
        /// <summary>
        /// 0天1周2月
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// type为1，为当天日期;type为2，为当周周一日期；type为3，为当月第一天日期
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// 日期字符串
        /// </summary>
        public string timestr { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public int ordercount { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal ordersum { get; set; }
        /// <summary>
        /// 完成订单量
        /// </summary>
        public int finishordercount { get; set; }
        /// <summary>
        /// 完成订单总金额
        /// </summary>
        public decimal finishordersum { get; set; }
        /// <summary>
        /// 日平均订单量（完成）
        /// </summary>
        public int ordercountavg { get; set; }
        /// <summary>
        /// 日平均订单总金额（完成）
        /// </summary>
        public decimal ordersumavg { get; set; }
        /// <summary>
        /// 外卖订单数量（完成）
        /// </summary>
        public int shipordercount { get; set; }
        /// <summary>
        /// 外卖订单总金额（完成）
        /// </summary>
        public decimal shipordersum { get; set; }
        /// <summary>
        /// 堂食订单数量（完成）
        /// </summary>
        public int shopordercount { get; set; }
        /// <summary>
        /// 堂食订单总金额（完成）
        /// </summary>
        public decimal shopordersum { get; set; }
        /// <summary>
        /// 预定订单数量(完成)
        /// </summary>
        public int orderordercount { get; set; }
        /// <summary>
        /// 预定订单总金额(完成)
        /// </summary>
        public decimal orderordersum { get; set; }
        /// <summary>
        /// 拼团订单数量(完成)
        /// </summary>
        public int groupordercount { get; set; }
        /// <summary>
        /// 拼团订单总金额(完成)
        /// </summary>
        public decimal groupordersum { get; set; }
        /// <summary>
        /// 秒杀订单数量(完成)
        /// </summary>
        public int seckillcount { get; set; }
        /// <summary>
        /// 秒杀订单总金额(完成)
        /// </summary>
        public decimal seckillsum { get; set; }
    }

}
