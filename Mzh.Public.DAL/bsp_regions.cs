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
    
    public partial class bsp_regions
    {
        public short regionid { get; set; }
        public string name { get; set; }
        public string spell { get; set; }
        public string shortspell { get; set; }
        public int displayorder { get; set; }
        public short parentid { get; set; }
        public byte layer { get; set; }
        public short provinceid { get; set; }
        public string provincename { get; set; }
        public short cityid { get; set; }
        public string cityname { get; set; }
    }
}
