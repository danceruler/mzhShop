using Mzh.Public.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    /// <summary>
    /// 用于前端展示的商品列表
    /// </summary>
    [Serializable]
    public class ShowProductList
    {
        /// <summary>
        /// 分类id
        /// </summary>
        public int cateid { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string catecategoryName { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<ShowProductInfo> productInfos { get; set; }
    }

    /// <summary>
    /// 用于前端展示的商品信息类
    /// </summary>
    [Serializable]
    public class ShowProductInfo
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int pid { get; set; }
        //public string psn { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public short cateid { get; set; }
        //public int brandid { get; set; }
        //public int skugid { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 商城价
        /// </summary>
        public decimal shopprice { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        public decimal marketprice { get; set; }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal costprice { get; set; }
        /// <summary>
        /// 包装费
        /// </summary>
        public decimal packprice { get; set; }
        /// <summary>
        /// 商品状态（1上架0下架）
        /// </summary>
        public byte state { get; set; }
        /// <summary>
        /// 是否支持外卖
        /// </summary>
        public byte isbest { get; set; }
        /// <summary>
        /// 是否热销
        /// </summary>
        public byte ishot { get; set; }
        /// <summary>
        /// 是否新上商品
        /// </summary>
        public byte isnew { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int displayorder { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public int weight { get; set; }
        /// <summary>
        /// 商品展示图片
        /// </summary>
        public string showimg { get; set; }
        /// <summary>
        /// 销售量
        /// </summary>
        public int salecount { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int visitcount { get; set; }
        /// <summary>
        /// 评价量
        /// </summary>
        public int reviewcount { get; set; }
        //public int star1 { get; set; }
        //public int star2 { get; set; }
        //public int star3 { get; set; }
        //public int star4 { get; set; }
        //public int star5 { get; set; }
        /// <summary>
        /// 上架时间
        /// </summary>
        public System.DateTime addtime { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 是否满减商品
        /// </summary>
        public Nullable<int> isfullcut { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int stock { get; set; }


        /// <summary>
        /// 规格数量（为1时不需要选规格）
        /// </summary>
        public int skuCount { get; set; }
        /// <summary>
        /// 规格信息
        /// </summary>
        public List<ShowSkuInfo> skuInfos { get; set; }
        /// <summary>
        /// 商品主要图
        /// </summary>
        public List<ShowProductImg> mainImgs { get; set; }
        /// <summary>
        /// 商品详情图
        /// </summary>
        public List<ShowProductImg> detailImgs { get; set; }
    }

    /// <summary>
    /// 用于前端展示的商品规格信息
    /// </summary>
    [Serializable]
    public class ShowSkuInfo
    {
        /// <summary>
        /// skuid
        /// </summary>
        public int sku_recordid { get; set; }
        //public int skugid { get; set; }
        /// <summary>
        /// 产品id
        /// </summary>
        public int sku_pid { get; set; }
        /// <summary>
        /// 属性id
        /// </summary>
        public short sku_attrid { get; set; }
        /// <summary>
        /// 属性值id
        /// </summary>
        public int sku_attrvalueid { get; set; }
        /// <summary>
        /// 属性值文本
        /// </summary>
        public string sku_inputvalue { get; set; }
        /// <summary>
        /// 属性文本
        /// </summary>
        public string sku_inputattr { get; set; }
        /// <summary>
        /// 价格(为-1取商品默认价格)
        /// </summary>
        public Nullable<decimal> sku_price { get; set; }
        /// <summary>
        /// 是否商品默认价格
        /// </summary>
        public int sku_isdefaultprice { get; set; }
        /// <summary>
        /// 库存(为-1取商品库存)
        /// </summary>
        public int sku_stock { get; set; }
    }


    /// <summary>
    /// 创建商品时的商品图片类
    /// </summary>
    [Serializable]
    public class ShowProductImg
    {
        public string showimg { get; set; }
        public int displayorder { get; set; }
    }

    /// <summary>
    /// 用于展示的商品分类信息
    /// </summary>
    [Serializable]
    public class ShowCatecory
    {
        public int cateid { get; set; }
        public string name { get; set; }
        public int displayorder { get; set; }
        public int productCount { get; set; }
    }

}
