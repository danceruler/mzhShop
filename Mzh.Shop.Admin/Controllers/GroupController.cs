using Mzh.Public.Model.Model;
using Mzh.Shop.Admin.Unitiy;
using Remoting;
using Remoting.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mzh.Shop.Admin.Controllers
{
    [LoginFilter]
    public class GroupController : Controller
    {
        // GET: Group
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            COUPON coupon = RemotingHelp.GetModelObject<COUPON>();
            ViewBag.Coupons = coupon.GetRecpientCouponForGroup();
            return View();
        }

        public ActionResult Detail(int groupinfoid)
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            var groupList = group.GroupInfoListForAdmin().data as List<GroupInfoModel>;
            ViewBag.GroupInfo = groupList.SingleOrDefault(t => t.groupinfoid == groupinfoid);
            return View();
        }


        #region 接口
        /// <summary>
        /// 新增拼团活动
        /// </summary>
        public ActionResult CreateGroupInfo(GroupInfoModel model)
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            return Json(group.CreateGroupInfo(model),JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除拼团活动
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult DeleteGroupInfo(int groupInfoId)
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            return Json(group.DeleteGroupInfo(groupInfoId),JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取拼团列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupInfoListForAdmin()
        {
            GROUP group = RemotingHelp.GetModelObject<GROUP>();
            return Json(group.GroupInfoListForAdmin(),JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}