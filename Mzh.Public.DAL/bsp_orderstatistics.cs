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
    
    public partial class bsp_orderstatistics
    {
        public int id { get; set; }
        public int type { get; set; }
        public Nullable<System.DateTime> time { get; set; }
        public string timestr { get; set; }
        public int ordercount { get; set; }
        public decimal ordersum { get; set; }
        public int finishordercount { get; set; }
        public decimal finishordersum { get; set; }
        public int ordercountavg { get; set; }
        public decimal ordersumavg { get; set; }
        public int shipordercount { get; set; }
        public decimal shipordersum { get; set; }
        public int shopordercount { get; set; }
        public decimal shopordersum { get; set; }
        public int orderordercount { get; set; }
        public decimal orderordersum { get; set; }
        public int groupordercount { get; set; }
        public decimal groupordersum { get; set; }
        public int seckillcount { get; set; }
        public decimal seckillsum { get; set; }
    }
}
