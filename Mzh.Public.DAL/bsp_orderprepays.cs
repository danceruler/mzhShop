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
    
    public partial class bsp_orderprepays
    {
        public string prepayid { get; set; }
        public int oid { get; set; }
        public string appid { get; set; }
        public string mch_id { get; set; }
        public string device_info { get; set; }
        public string nonce_str { get; set; }
        public string sign { get; set; }
        public string timeStamp { get; set; }
        public string signType { get; set; }
        public string notify_url { get; set; }
        public Nullable<decimal> total_fee { get; set; }
        public Nullable<System.DateTime> addtime { get; set; }
        public string openid { get; set; }
        public bool ispay { get; set; }
        public Nullable<System.DateTime> paytime { get; set; }
        public string spbill_create_ip { get; set; }
        public string transaction_id { get; set; }
        public Nullable<System.DateTime> prepayexpiretime { get; set; }
    }
}
