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
    
    public partial class bsp_orders
    {
        public int oid { get; set; }
        public string osn { get; set; }
        public int uid { get; set; }
        public byte orderstate { get; set; }
        public decimal productamount { get; set; }
        public decimal orderamount { get; set; }
        public decimal surplusmoney { get; set; }
        public int parentid { get; set; }
        public byte isreview { get; set; }
        public System.DateTime addtime { get; set; }
        public string shipsn { get; set; }
        public string shipsystemname { get; set; }
        public string shipfriendname { get; set; }
        public Nullable<System.DateTime> shiptime { get; set; }
        public string paysn { get; set; }
        public string paysystemname { get; set; }
        public string payfriendname { get; set; }
        public byte paymode { get; set; }
        public Nullable<System.DateTime> paytime { get; set; }
        public short regionid { get; set; }
        public string consignee { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string zipcode { get; set; }
        public string address { get; set; }
        public Nullable<System.DateTime> besttime { get; set; }
        public decimal shipfee { get; set; }
        public decimal payfee { get; set; }
        public int fullcut { get; set; }
        public decimal discount { get; set; }
        public int paycreditcount { get; set; }
        public decimal paycreditmoney { get; set; }
        public int couponmoney { get; set; }
        public int weight { get; set; }
        public string buyerremark { get; set; }
        public string ip { get; set; }
        public int type { get; set; }
        public int boxid { get; set; }
        public Nullable<decimal> boxprice { get; set; }
        public Nullable<decimal> bookprice { get; set; }
    }
}
