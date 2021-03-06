﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        /// 堂食订单制作中或者预定中
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
        /// 评价完成（暂时没有这个）
        /// </summary>
        Reviewed = 8,
        /// <summary>
        /// 超时未支付
        /// </summary>
        EndForNoPay = 9,
    }

    /// <summary>
    /// 包厢餐桌状态
    /// </summary>
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

    /// <summary>
    /// 支付方式
    /// </summary>
    public enum PayMod
    {
        Cash = 1,
        AliPay = 2,
        WxPay = 3,
        CreditCard = 4,
        Other = 5
    }

    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        Ship = 1,
        InShop = 2,
        Order = 3,
        SECKILL = 4,
        GROUP = 5
    }

    /// <summary>
    /// 优惠券类型
    /// </summary>
    public enum CouponType
    {
        /// <summary>
        /// 满减优惠券
        /// </summary>
        NormalFullcut = 1,
        /// <summary>
        /// 折扣优惠券
        /// </summary>
        NormalDiscount = 2,
        /// <summary>
        /// 指定商品满减优惠券
        /// </summary>
        AppointProductFullcut = 3,
        /// <summary>
        /// 指定商品折扣优惠券
        /// </summary>
        AppointProductDiscount = 4
    }

    /// <summary>
    /// 轮播图类型
    /// </summary>
    public enum BannerType
    {
        /// <summary>
        /// 前往指定商品详情页
        /// </summary>
        ToProduct = 1,
        /// <summary>
        /// 前往指定网页链接
        /// </summary>
        ToLinkUrl = 2,
        /// <summary>
        /// 仅用于展示
        /// </summary>
        OnlyShow = 3,
    }

    public static class EnumToText
    {
        public static string ToText(this ProductState enumModel)
        {
            switch (enumModel)
            {
                case ProductState.OnSale:
                    return "在售";
                case ProductState.UnderCarriage:
                    return "下架";
                default:
                    return "";
            }
        }

        public static string ToText(this OrderState enumModel)
        {
            switch (enumModel)
            {
                case OrderState.WaitPay:
                    return "待付款";
                case OrderState.WaitSend:
                    return "待发货";
                case OrderState.Sending:
                    return "配送中";
                case OrderState.Booking:
                    return "制作中/预定中";
                case OrderState.WaitReview:
                    return "待评价";
                case OrderState.ApplyRefund:
                    return "申请退款中";
                case OrderState.Refunded:
                    return "退款完成";
                case OrderState.Reviewed:
                    return "评价完成";
                case OrderState.EndForNoPay:
                    return "支付超时";
                default:
                    return "";
            }
        }

        public static string ToText(this OrderType enumModel)
        {
            switch (enumModel)
            {
                case OrderType.InShop:
                    return "堂食";
                case OrderType.Ship:
                    return "外卖";
                case OrderType.Order:
                    return "预定";
                case OrderType.GROUP:
                    return "拼团";
                case OrderType.SECKILL:
                    return "秒杀";
                default:
                    return "";
            }
        }

        public static string ToText(this PayMod enumModel)
        {
            switch (enumModel)
            {
                case PayMod.Cash:
                    return "现金";
                case PayMod.AliPay:
                    return "支付宝";
                case PayMod.WxPay:
                    return "微信";
                case PayMod.CreditCard:
                    return "银行卡";
                case PayMod.Other:
                    return "其他";
                default:
                    return "";
            }
        }

        public static string ToText(this CouponType enumModel)
        {
            switch (enumModel)
            {
                case CouponType.AppointProductDiscount:
                    return "指定商品折扣优惠券";
                case CouponType.AppointProductFullcut:
                    return "指定商品满减优惠券";
                case CouponType.NormalDiscount:
                    return "折扣优惠券";
                case CouponType.NormalFullcut:
                    return "满减优惠券";
                default:
                    return "";
            }
        }

        public static string ToText(this BannerType enumModel)
        {
            switch (enumModel)
            {
                case BannerType.OnlyShow:
                    return "仅用于展示";
                case BannerType.ToLinkUrl:
                    return "跳转到指定的网页链接";
                case BannerType.ToProduct:
                    return "跳转到指定的商品详情页";
                default:
                    return "";
            }
        }
    }
}
