using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    /// <summary>
    /// 用于前端展示的优惠券信息
    /// </summary>
    [Serializable]
    public class ShowCouponInfo
    {
        public int couponid { get; set; }
        //public string couponsn { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// 优惠券类型id
        /// </summary>
        public int coupontypeid { get; set; }
        /// <summary>
        /// 订单id（已使用的情况下才有）
        /// </summary>
        public int oid { get; set; }
        /// <summary>
        /// 使用时间
        /// </summary>
        public System.DateTime usetime { get; set; }
        /// <summary>
        /// 使用ip
        /// </summary>
        public string useip { get; set; }
        //public int money { get; set; }
        /// <summary>
        /// 有效开始时间
        /// </summary>
        public System.DateTime activatetime { get; set; }
        //public string activateip { get; set; }
        //public int createuid { get; set; }
        //public int createoid { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime createtime { get; set; }
        public string createip { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public Nullable<System.DateTime> expiretime { get; set; }
        /// <summary>
        /// 是否已经使用
        /// </summary>
        public int isuse { get; set; }
        /// <summary>
        /// 优惠券类型信息
        /// </summary>
        public ShowCouponTypeInfo typeInfo { get; set; }
    }

    [Serializable]
    public class ShowCouponTypeInfo
    {
        public int ct_coupontypeid { get; set; }
        /// <summary>
        /// 优惠券类型状态
        /// </summary>
        public byte ct_state { get; set; }
        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string ct_name { get; set; }
        //public int money { get; set; }
        //public int count { get; set; }
        //public byte sendmode { get; set; }
        /// <summary>
        /// 获取方式
        /// </summary>
        public byte ct_getmode { get; set; }
        /// <summary>
        /// 使用方式
        /// </summary>
        public byte ct_usemode { get; set; }
        //public short userranklower { get; set; }
        //public int orderamountlower { get; set; }
        //public short limitcateid { get; set; }
        //public int limitbrandid { get; set; }
        //public byte limitproduct { get; set; }
        /// <summary>
        /// 优惠券开始发放时间
        /// </summary>
        public System.DateTime ct_sendstarttime { get; set; }
        public string t_ct_sendstarttime { get; set; }
        /// <summary>
        /// 优惠券结束发放时间
        /// </summary>
        public System.DateTime ct_sendendtime { get; set; }
        public string t_ct_sendendtime { get; set; }
        /// <summary>
        /// 优惠券是否指定过期时间（为1为指定时间，否则表示过期的秒数）
        /// </summary>
        public int ct_useexpiretime { get; set; }
        /// <summary>
        /// useexpiretime为1时有效，开始时间
        /// </summary>
        public System.DateTime ct_usestarttime { get; set; }
        /// <summary>
        /// useexpiretime为1时有效，结束时间
        /// </summary>
        public System.DateTime ct_useendtime { get; set; }
        /// <summary>
        /// 优惠券类型
        /// </summary>
        public int ct_type { get; set; }
        public string t_ct_type { get; set; }
        /// <summary>
        /// 是否可以和其他优惠券叠加（多张优惠券最多只能存在一张不可叠加的优惠券）
        /// </summary>
        public int ct_isstack { get; set; }
        public string t_ct_isstack { get; set; }
        /// <summary>
        /// 为满减时的满价
        /// </summary>
        public decimal ct_fullmoney { get; set; }
        /// <summary>
        /// 为满减时的减价
        /// </summary>
        public decimal ct_cutmoney { get; set; }

        /// <summary>
        /// 100(满价)/20(减价)
        /// </summary>
        public string t_ct_fullcut { get; set; }
        /// <summary>
        /// 为折扣时的折扣
        /// </summary>
        public double ct_discount { get; set; }
        /// <summary>
        /// 优惠券关联产品（仅在优惠券为指定商品时有值）
        /// </summary>
        public int ct_pid { get; set; }
    }
}
