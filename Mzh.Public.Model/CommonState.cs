using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 等待支付
        /// </summary>
        WaitPay = 1,
        /// <summary>
        /// 等待发货or制作中
        /// </summary>
        WaitDelivery = 2,
        /// <summary>
        /// 运输中
        /// </summary>
        InTransit = 3,
        /// <summary>
        /// 确认收货后
        /// </summary>
        Finish = 4,
        /// <summary>
        /// 申请退款中
        /// </summary>
        RefundApplying = 5,
        /// <summary>
        /// 交易彻底结束，不能再做任何操作（包括确认收货一定时间后和退款完成后）
        /// </summary>
        END = 99,
    }

    /// <summary>
    /// 商品状态
    /// </summary>
    public enum ProductState
    {
        /// <summary>
        /// 在售
        /// </summary>
        OnSale = 1,
        /// <summary>
        /// 下架
        /// </summary>
        SoldOut = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 2,
    }
}
