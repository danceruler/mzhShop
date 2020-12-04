using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using Remoting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    /// <summary>
    /// 拼团
    /// </summary>
    public class GROUP : MarshalByRefObject
    {
        COUPON coupon = new COUPON();

        /// <summary>
        /// 新增拼团活动
        /// </summary>
        public ResultModel CreateGroupInfo(GroupInfoModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除拼团活动
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel DeleteGroupInfo(int groupInfoId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 用户开团（支付回调时调用）
        /// </summary>
        /// <returns></returns>
        public ResultModel StartGroup(int groupInfoId, int uid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 用户参团（支付回调时调用）
        /// </summary>
        /// <returns></returns>
        public ResultModel JoinGroup(int GroupId,int uid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取首页拼团列表
        /// </summary>
        /// <returns></returns>
        public ResultModel GroupInfoList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取首页拼团列表
        /// </summary>
        /// <returns></returns>
        public LayuiTableApiResult GroupInfoListForAdmin()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取当前拼团活动正在进行的团
        /// </summary>
        /// <returns></returns>
        public ResultModel GetRunningGroupListByInfo(int groupInfoId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 用户发起拼团支付
        /// </summary>
        /// <param name="isstart">1开团 2参团</param>
        /// <param name="gid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ResultModel PayGroup(int isstart,int gid,int uid,decimal totalfee)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取用户的拼团列表
        /// </summary>
        /// <returns></returns>
        public ResultModel GroupListByUid(int uid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新拼团状态
        /// </summary>
        public void UpdateGroupState()
        {
            throw new NotImplementedException();
        }
    }
}
