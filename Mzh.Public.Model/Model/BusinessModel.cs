using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    [Serializable]
    public class BusinessModel
    {
        public int businessid { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Nullable<System.DateTime> addtime { get; set; }
        public string description { get; set; }
        public string showimg { get; set; }
        public Nullable<decimal> latitude { get; set; }
        public Nullable<decimal> longitude { get; set; }
        public Nullable<decimal> canSendRadius { get; set; }
        public string businessTimeStr { get; set; }
        public int businessStart { get; set; }
        public int businessEnd { get; set; }
        public string telphone { get; set; }
        public List<BusinessImgModel> imgs { get; set; }
    }
    [Serializable]
    public class BusinessImgModel
    {
        public int bi_businessimgid { get; set; }
        public int bi_businessid { get; set; }
        public string bi_img { get; set; }
    }
}
