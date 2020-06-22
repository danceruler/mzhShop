using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model
{
    /// <summary>
    /// 商品状态
    /// </summary>
    public enum ProductState
    {
        OnSale = 1, 
        UnderCarriage = 0
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 待付款
        /// </summary>
        WaitPay = 1,
        /// <summary>
        /// 外卖订单待发货
        /// </summary>
        WaitSend = 2,
        /// <summary>
        /// 外卖订单派送中
        /// </summary>
        Sending = 3,
        /// <summary>
        /// 堂食订单待使用
        /// </summary>
        Booking = 4,
        /// <summary>
        /// 待评价
        /// </summary>
        WaitReview = 5,
        /// <summary>
        /// 申请退款中
        /// </summary>
        ApplyRefund = 6,

        /// <summary>
        /// 退款完成
        /// </summary>
        Refunded = 7,
        /// <summary>
        /// 评价完成
        /// </summary>
        Reviewed = 8,
    }

    public enum BoxState
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Empty = 0,
        /// <summary>
        /// 有客
        /// </summary>
        Use = 1,
        /// <summary>
        /// 预定
        /// </summary>
        Book = 2,
    }

    public enum PayMod
    {
        Cash = 1,
        AliPay = 2,
        WxPay = 3,
        CreditCard = 4,
        Other = 5
    }
}
