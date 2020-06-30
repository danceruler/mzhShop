using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    public class FileCacheModel
    {
        public string objectname { get; set; }
        public string timestamp { get; set; }
        public DateTime expiretime { get; set; }
        public int requestcount { get; set; }
        public string ossurl { get; set; }
    }
}
