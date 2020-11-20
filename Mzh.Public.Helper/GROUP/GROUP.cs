using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.BLL.GROUP
{
    /// <summary>
    /// 拼团
    /// </summary>
    public class GROUP : MarshalByRefObject
    {
        /// <summary>
        /// 新增拼团活动
        /// </summary>
        public ResultModel CreateGroupInfo(GroupInfoModel model)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    bsp_groupinfos newGroupInfo = new bsp_groupinfos();
                    newGroupInfo.endtime = model.endtime;
                    newGroupInfo.groupoid = model.groupoid;
                    newGroupInfo.groupprice = model.groupprice;
                    newGroupInfo.grouptype = model.grouptype;
                    newGroupInfo.maxtime = model.maxtime;
                    newGroupInfo.needcount = model.needcount;
                    newGroupInfo.shopprice = model.shopprice;
                    newGroupInfo.starttime = model.starttime;
                    context.bsp_groupinfos.Add(newGroupInfo);
                    context.SaveChanges();
                    return ResultModel.Success("新增成功");
                }
                catch (Exception ex)
                {
                    Log.Error("CreateGroupInfo方法," + ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 删除拼团活动
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel DeleteGroupInfo(int groupInfoId)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    bsp_groupinfos GroupInfo = context.bsp_groupinfos.SingleOrDefault(t => t.groupinfoid == groupInfoId);
                    context.bsp_groupinfos.Remove(GroupInfo);
                    context.SaveChanges();
                    return ResultModel.Success("删除成功");
                }
                catch (Exception ex)
                {
                    Log.Error("DeleteGroupInfo方法," + ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 用户开团
        /// </summary>
        /// <returns></returns>
        //public ResultModel StartGroup(int groupInfoId,int uid)
        //{
        //    using (brnshopEntities context = new brnshopEntities())
        //    {
        //        try
        //        {
        //            bsp_groupinfos GroupInfo = context.bsp_groupinfos.SingleOrDefault(t => t.groupinfoid == groupInfoId);
        //            bsp_groups newGroup = new bsp_groups();
        //            newGroup.endtime = GroupInfo.endtime;
        //            context.bsp_groupinfos.Remove(newGroupInfo);
        //            context.SaveChanges();
        //            return ResultModel.Success("删除成功");
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error("StartGroup方法," + ex.ToString());
        //            return ResultModel.Error(ex.ToString());
        //        }
        //    }
        //}
    }
}
