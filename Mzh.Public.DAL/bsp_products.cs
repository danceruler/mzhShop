//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mzh.Public.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class bsp_products
    {
        public int pid { get; set; }
        public string psn { get; set; }
        public short cateid { get; set; }
        public int brandid { get; set; }
        public int skugid { get; set; }
        public string name { get; set; }
        public decimal shopprice { get; set; }
        public decimal marketprice { get; set; }
        public decimal costprice { get; set; }
        public byte state { get; set; }
        public byte isbest { get; set; }
        public byte ishot { get; set; }
        public byte isnew { get; set; }
        public int displayorder { get; set; }
        public int weight { get; set; }
        public string showimg { get; set; }
        public int salecount { get; set; }
        public int visitcount { get; set; }
        public int reviewcount { get; set; }
        public int star1 { get; set; }
        public int star2 { get; set; }
        public int star3 { get; set; }
        public int star4 { get; set; }
        public int star5 { get; set; }
        public System.DateTime addtime { get; set; }
        public string description { get; set; }
        public Nullable<int> isfullcut { get; set; }
        public decimal packprice { get; set; }
        public int stock { get; set; }
        public int isdelete { get; set; }
        public Nullable<System.DateTime> startseckilltime { get; set; }
        public Nullable<System.DateTime> endseckilltime { get; set; }
        public decimal seckillprice { get; set; }
        public int rmddisplayorder { get; set; }
    }
}
