using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    /// <summary>
    /// 用来创建订单的类
    /// </summary>
    public class CreateOrderModel
    {
        #region 订单相关信息
        /// <summary>
        /// 订单类型1外卖订单2堂食订单
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal orderamount { get; set; }
        /// <summary>
        /// 地区id
        /// </summary>
        public short regionid { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string consignee { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 预约配送时间
        /// </summary>
        public System.DateTime besttime { get; set; }
        /// <summary>
        /// 配送费
        /// </summary>
        public decimal shipfee { get; set; }
        /// <summary>
        /// 是否满减
        /// </summary>
        public int fullcut { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal discount { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public int couponmoney { get; set; }
        public int weight { get; set; }
        /// <summary>
        /// 用户备注
        /// </summary>
        public string buyerremark { get; set; }
        /// <summary>
        /// 如果是堂食选择包厢的订单,记录预定的包厢id
        /// </summary>
        public int boxid { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public int uid { get; set; }
        #endregion
        /// <summary>
        /// 购物车信息
        /// </summary>
        public CartModel cart { get; set; }
        /// <summary>
        /// 使用的优惠券id
        /// </summary>
        public List<ShowCouponInfo> coupons { get; set; }
    }

    /// <summary>
    /// 购物车模型
    /// </summary>
    public class CartModel
    {
        public List<CartItem> items { get; set; }
        /// <summary>
        /// 商品总金额（包括包装费）
        /// </summary>
        public decimal productamount { get; set; }
    }

    /// <summary>
    /// 购物车内的购买模型
    /// </summary>
    public class CartItem
    {
        public ShowProductInfo productinfo { get; set; }
        public int count { get; set; }
        /// <summary>
        /// 实际总价
        /// </summary>
        public decimal shopprice { get; set; }
        /// <summary>
        /// 总原价
        /// </summary>
        public decimal marketprice { get; set; }
        public int skuid { get; set; }
        public string inputattr { get; set; }
        public string inputvalue { get; set; }
    }
}
