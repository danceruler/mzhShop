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
    
    public partial class bsp_files
    {
        public int id { get; set; }
        public string client { get; set; }
        public Nullable<System.DateTime> uploadtime { get; set; }
        public string requesturl { get; set; }
        public string ossurl { get; set; }
        public string name { get; set; }
        public string timestamp { get; set; }
        public string suffixname { get; set; }
        public string objectname { get; set; }
        public int requestcount { get; set; }
    }
}
