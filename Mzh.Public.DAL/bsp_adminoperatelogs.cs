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
    
    public partial class bsp_adminoperatelogs
    {
        public int logid { get; set; }
        public int uid { get; set; }
        public string nickname { get; set; }
        public short admingid { get; set; }
        public string admingtitle { get; set; }
        public string operation { get; set; }
        public string description { get; set; }
        public string ip { get; set; }
        public System.DateTime operatetime { get; set; }
    }
}
