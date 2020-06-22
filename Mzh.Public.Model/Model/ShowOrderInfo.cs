using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    /// <summary>
    /// 用于前端展示的订单类
    /// </summary>
    public class ShowOrderInfo
    {
        public int oid { get; set; }
        //public string osn { get; set; }
        public int uid { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public byte orderstate { get; set; }
        /// <summary>
        /// 商品总金额
        /// </summary>
        public decimal productamount { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal orderamount { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal surplusmoney { get; set; }
        //public int parentid { get; set; }
        /// <summary>
        /// 是否评价
        /// </summary>
        public byte isreview { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime addtime { get; set; }
        /// <summary>
        /// 配送单号
        /// </summary>
        public string shipsn { get; set; }
        //public string shipsystemname { get; set; }
        //public string shipfriendname { get; set; }
        /// <summary>
        /// 配送时间（开始时间）
        /// </summary>
        public System.DateTime shiptime { get; set; }
        /// <summary>
        /// 支付单号
        /// </summary>
        public string paysn { get; set; }
        //public string paysystemname { get; set; }
        //public string payfriendname { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public byte paymode { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public System.DateTime paytime { get; set; }
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
        //public string phone { get; set; }
        //public string email { get; set; }
        //public string zipcode { get; set; }
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
        /// 支付金额
        /// </summary>
        public decimal payfee { get; set; }
        /// <summary>
        /// 是否满减
        /// </summary>
        public int fullcut { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal discount { get; set; }
        //public int paycreditcount { get; set; }
        //public decimal paycreditmoney { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public int couponmoney { get; set; }
        public int weight { get; set; }
        /// <summary>
        /// 用户备注
        /// </summary>
        public string buyerremark { get; set; }
        public string ip { get; set; }
        /// <summary>
        /// 订单商品
        /// </summary>
        public List<ShowOrderProductInfo> orderProducts { get; set; }
        /// <summary>
        /// 使用的优惠券
        /// </summary>
        public List<ShowCouponInfo> coupons { get; set; }
    }

    /// <summary>
    /// 用于前端展示的订单商品类
    /// </summary>
    public class ShowOrderProductInfo
    {
        /// <summary>
        /// 订单商品id
        /// </summary>
        public int op_recordid { get; set; }
        //public int oid { get; set; }
        //public int uid { get; set; }
        //public string sid { get; set; }
        /// <summary>
        /// 订单商品商品id
        /// </summary>
        public int op_pid { get; set; }
        //public string psn { get; set; }
        /// <summary>
        /// 订单商品分类id
        /// </summary>
        public short op_cateid { get; set; }
        /// <summary>
        /// 订单商品品牌id
        /// </summary>
        public int op_brandid { get; set; }
        /// <summary>
        /// 订单商品名称
        /// </summary>
        public string op_name { get; set; }
        /// <summary>
        /// 订单商品展示图片
        /// </summary>
        public string op_showimg { get; set; }
        /// <summary>
        /// 订单商品折扣价
        /// </summary>
        public decimal op_discountprice { get; set; }
        /// <summary>
        /// 订单商品市场价
        /// </summary>
        public decimal op_shopprice { get; set; }
        //public decimal op_costprice { get; set; }
        /// <summary>
        /// 订单商品售价
        /// </summary>
        public decimal op_marketprice { get; set; }
        /// <summary>
        /// 订单商品重量
        /// </summary>
        public int op_weight { get; set; }
        //public byte op_isreview { get; set; }
        //public int op_realcount { get; set; }
        /// <summary>
        /// 订单商品购买数量
        /// </summary>
        public int op_buycount { get; set; }
        //public int op_sendcount { get; set; }
        /// <summary>
        /// 订单商品类型
        /// </summary>
        public byte op_type { get; set; }
        //public int op_paycredits { get; set; }
        //public int op_coupontypeid { get; set; }
        //public int extcode1 { get; set; }
        //public int extcode2 { get; set; }
        //public int extcode3 { get; set; }
        //public int extcode4 { get; set; }
        //public int extcode5 { get; set; }
        public System.DateTime op_addtime { get; set; }
    }

    /// <summary>
    /// 用于前端展示的优惠券信息
    /// </summary>
    public class ShowCouponInfo {

    }
}
