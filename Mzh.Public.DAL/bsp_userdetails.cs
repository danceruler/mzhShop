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
    
    public partial class bsp_userdetails
    {
        public int uid { get; set; }
        public System.DateTime lastvisittime { get; set; }
        public string lastvisitip { get; set; }
        public short lastvisitrgid { get; set; }
        public System.DateTime registertime { get; set; }
        public string registerip { get; set; }
        public short registerrgid { get; set; }
        public byte gender { get; set; }
        public string realname { get; set; }
        public System.DateTime bday { get; set; }
        public string idcard { get; set; }
        public short regionid { get; set; }
        public string address { get; set; }
        public string bio { get; set; }
    }
}
