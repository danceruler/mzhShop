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
    
    public partial class bsp_coupontypes
    {
        public int coupontypeid { get; set; }
        public byte state { get; set; }
        public string name { get; set; }
        public int money { get; set; }
        public int count { get; set; }
        public byte sendmode { get; set; }
        public byte getmode { get; set; }
        public byte usemode { get; set; }
        public short userranklower { get; set; }
        public int orderamountlower { get; set; }
        public short limitcateid { get; set; }
        public int limitbrandid { get; set; }
        public byte limitproduct { get; set; }
        public System.DateTime sendstarttime { get; set; }
        public System.DateTime sendendtime { get; set; }
        public int useexpiretime { get; set; }
        public System.DateTime usestarttime { get; set; }
        public System.DateTime useendtime { get; set; }
        public int type { get; set; }
        public int isstack { get; set; }
        public decimal fullmoney { get; set; }
        public decimal cutmoney { get; set; }
        public double discount { get; set; }
    }
}
