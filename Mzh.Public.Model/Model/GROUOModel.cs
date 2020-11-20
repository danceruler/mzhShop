using Mzh.Public.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    /// <summary>
    /// 拼团信息类
    /// </summary>
    [Serializable]
    public class GroupInfoModel
    {
        /// <summary>
        /// 拼团id
        /// </summary>
        public int groupinfoid { get; set; }
        /// <summary>
        /// 拼团类型（1.优惠券2.商品） 目前默认1
        /// </summary>
        public int grouptype { get; set; }
        /// <summary>
        /// 拼团对象（根据拼团类型确定是哪个对象的id）目前默认优惠券id
        /// </summary>
        public int groupoid { get; set; }
        /// <summary>
        /// 冗余字段，产品id
        /// </summary>
        public int pid { get; set; }
        /// <summary>
        /// 拼团活动开始时间
        /// </summary>
        public DateTime? starttime { get; set; }
        /// <summary>
        /// 拼团活动结束时间
        /// </summary>
        public DateTime? endtime { get; set; }
        /// <summary>
        /// 拼团价
        /// </summary>
        public decimal groupprice { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal? shopprice { get; set; }
        /// <summary>
        /// 需要人数
        /// </summary>
        public int needcount { get; set; }
        /// <summary>
        /// 超时时间（s）
        /// </summary>
        public int maxtime { get; set; }
    }

    /// <summary>
    /// 具体的拼团信息
    /// </summary>
    [Serializable]
    public class GroupModel
    {
        /// <summary>
        /// 拼团id
        /// </summary>
        public int groupid { get; set; }
        /// <summary>
        /// 拼团类型（1.优惠券2.商品） 目前默认1
        /// </summary>
        public int grouptype { get; set; }
        /// <summary>
        /// 拼团对象（根据拼团类型确定是哪个对象的id）目前默认优惠券id
        /// </summary>
        public int groupoid { get; set; }
        /// <summary>
        /// 冗余字段，产品id
        /// </summary>
        public int pid { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? starttime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endtime { get; set; }
        /// <summary>
        /// 发起人id
        /// </summary>
        public int startuid { get; set; }
        /// <summary>
        /// 拼团价
        /// </summary>
        public decimal groupprice { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal? shopprice { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool? isfinish { get; set; }
        /// <summary>
        /// 需要人数
        /// </summary>
        public int needcount { get; set; }
        /// <summary>
        /// 当前人数
        /// </summary>
        public int nowcount { get; set; }
        /// <summary>
        /// 是否失败
        /// </summary>
        public bool? isfail { get; set; }
        /// <summary>
        /// 失败原因 暂时没用
        /// </summary>
        public int failtype { get; set; }
        /// <summary>
        /// 超时时间（s）
        /// </summary>
        public int maxtime { get; set; }
        /// <summary>
        /// 拼团明细
        /// </summary>
        public List<GroupDetailModel> details { get; set; }
    }

    /// <summary>
    /// 拼团明细（参与人信息）
    /// </summary>
    public class GroupDetailModel
    {
        /// <summary>
        /// 拼团明细GUID
        /// </summary>
        public int gd_groupdetailid { get; set; }
        /// <summary>
        /// 团id
        /// </summary>
        public int gd_groupid { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int gd_uid { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int gd_sno { get; set; }
        /// <summary>
        /// 支付时间（参与时间）
        /// </summary>
        public Nullable<System.DateTime> gd_paytime { get; set; }
    }
}
