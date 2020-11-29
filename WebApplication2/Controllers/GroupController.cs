using Mzh.Public.Model.Model;
using Remoting;
using Remoting.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    public class GroupController : ApiController
    {
        /// <summary>
        /// 新增拼团活动
        /// </summary>
        public ResultModel CreateGroupInfo(GroupInfoModel model)
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            return group.CreateGroupInfo(model);
        }

        /// <summary>
        /// 删除拼团活动
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel DeleteGroupInfo(int groupInfoId)
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            return group.DeleteGroupInfo(groupInfoId);
        }

        /// <summary>
        /// 用户开团（支付回调时调用）
        /// </summary>
        /// <returns></returns>
        public ResultModel StartGroup(int groupInfoId, int uid)
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            return group.StartGroup(groupInfoId, uid);
        }

        /// <summary>
        /// 用户参团（支付回调时调用）
        /// </summary>
        /// <returns></returns>
        public ResultModel JoinGroup(int GroupId, int uid)
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            return group.JoinGroup(GroupId, uid);
        }

        /// <summary>
        /// 获取首页拼团列表
        /// </summary>
        /// <returns></returns>
        public ResultModel GroupInfoList()
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            return group.GroupInfoList();
        }

        /// <summary>
        /// 获取当前拼团活动正在进行的团
        /// </summary>
        /// <returns></returns>
        public ResultModel GetRunningGroupListByInfo(int groupInfoId)
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            return group.GetRunningGroupListByInfo(groupInfoId);
        }

        /// <summary>
        /// 用户发起拼团支付
        /// </summary>
        /// <param name="isstart">1开团 2参团</param>
        /// <param name="gid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ResultModel PayGroup(int isstart, int gid, int uid, decimal totalfee)
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            return group.PayGroup(isstart, gid, uid, totalfee);
        }
    }
}
