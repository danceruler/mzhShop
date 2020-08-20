using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    /// <summary>
    /// 前端用户信息类
    /// </summary>
    public class ShowUserInfo
    {
        public int uid { get; set; }
        //public string mobile { get; set; }
        //public string password { get; set; }
        public string nickname { get; set; }
        public string username { get; set; }
        public string avater { get; set; }
        public string openid { get; set; }
        public int gender { get; set; }

    }
}
