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
    
    [Serializable]
    public partial class bsp_boxes
    {
        public int boxid { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int state { get; set; }
        public int oid { get; set; }
        public int uid { get; set; }
        public Nullable<System.DateTime> booktime { get; set; }
        public string phone { get; set; }
        public string username { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<decimal> bookprice { get; set; }
        public int type { get; set; }
    }
}
