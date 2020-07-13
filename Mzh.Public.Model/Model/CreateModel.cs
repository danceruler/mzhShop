using Mzh.Public.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    /// <summary>
    /// 用来创建订单的类
    /// </summary>
    [Serializable]
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
    [Serializable]
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
    [Serializable]
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

    /// <summary>
    /// 用来创建商品的类
    /// </summary>
    [Serializable]
    public class AddProductModel
    {
        public int pid { get; set; }
        public List<int> cateids { get; set; }
        public int state { get; set; }
        public string name { get; set; }
        public decimal shopprice { get; set; }
        public decimal marketprice { get; set; }
        public decimal costprice { get; set; }
        public byte isbest { get; set; }
        public byte ishot { get; set; }
        public byte isnew { get; set; }
        public int displayorder { get; set; }
        public int weight { get; set; }
        public string showimg { get; set; }
        public string description { get; set; }
        public Nullable<int> isfullcut { get; set; }
        public decimal packprice { get; set; }
        public List<ProductImgModel> mainImgs { get; set; }
        public List<ProductImgModel> detailImgs { get; set; }
        public List<AttributeInfo> skuInfos { get; set; }
    }

    /// <summary>
    /// 创建商品时的商品图片类
    /// </summary>
    [Serializable]
    public class ProductImgModel
    {
        public string showimg { get; set; }
        public int displayorder { get; set; }
    }



    /// <summary>
    /// 属性
    /// </summary>
    [Serializable]
    public class AttributeInfo
    {
        public int attrid { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public List<AttributeValueInfo> attributevalues { get; set; }
    }

    /// <summary>
    /// 属性值
    /// </summary>
    [Serializable]
    public partial class AttributeValueInfo
    {
        public int attrvalueid { get; set; }
        public string attrvalue { get; set; }
        public string attrname { get; set; }
        public int price { get; set; }
        public int stock { get; set; }
    }
}
