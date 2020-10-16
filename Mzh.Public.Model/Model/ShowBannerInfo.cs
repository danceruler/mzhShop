using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    /// <summary>
    /// 用于前端展示的轮播图信息
    /// </summary>
    [Serializable]
    public class ShowBannerInfo
    {
        public int id { get; set; }
        public byte type { get; set; }
        public string ttype { get; set; }
        public System.DateTime starttime { get; set; }
        public string tstarttime { get; set; }
        public System.DateTime endtime { get; set; }
        public string tendtime { get; set; }
        public byte isshow { get; set; }
        public string title { get; set; }
        public string img { get; set; }
        public string url { get; set; }
        public int displayorder { get; set; }

    }
}
