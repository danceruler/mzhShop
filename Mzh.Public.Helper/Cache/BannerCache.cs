using Mzh.Public.Base;
using Mzh.Public.BLL.Cache;
using Mzh.Public.DAL;
using Mzh.Public.Model;
using Mzh.Public.Model.Model;
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
    /// 轮播图缓存
    /// </summary>
    public class BannerCache:MarshalByRefObject, ICache
    {
        public static List<ShowBannerInfo> banners = new List<ShowBannerInfo>();
        public void Init() 
        {
            InitBanners();
        }

        public LayuiTableApiResult GetBannersForAdmin()
        {
            return new LayuiTableApiResult() {
                code = 0,
                count = banners.Count,
                data = banners,
                msg = ""
            };
        }

        public List<ShowBannerInfo> GetBanners()
        {
            return banners;
        }

        public static void InitBanners()
        {
            string sql = $@"SELECT * 
                            FROM dbo.bsp_banners 
                            WHERE isshow = 1 
                            ORDER BY displayorder";
            SqlCommand cmd = new SqlCommand(sql);
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, cmd);
            banners = dt.GetList<ShowBannerInfo>("");
            banners.ForEach(t =>
            {
                t.ttype = ((BannerType)t.type).ToText();
                t.tstarttime = t.starttime.ToString("yyyy-MM-dd");
                t.tendtime = t.endtime.ToString("yyyy-MM-dd");
            });
        }

        /// <summary>
        /// 添加编辑轮播图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddBanner(AddBannerModel model)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    if(model.bannerid == 0)
                    {
                        var newbanner = new bsp_banners();
                        newbanner.displayorder = model.displayorder;
                        newbanner.endtime = model.endtime;
                        newbanner.img = model.img;
                        newbanner.isshow = 1;
                        newbanner.starttime = model.starttime;
                        newbanner.title = model.title;
                        newbanner.type = (byte)model.bannerType;
                        newbanner.url = model.url;
                        context.bsp_banners.Add(newbanner);
                    }
                    else
                    {
                        var banner = context.bsp_banners.SingleOrDefault(t => t.id == model.bannerid);
                        if (banner == null) return ResultModel.Fail("请刷新列表");
                        banner.displayorder = model.displayorder;
                        banner.endtime = model.endtime;
                        banner.img = model.img;
                        banner.starttime = model.starttime;
                        banner.type = (byte)model.bannerType;
                        banner.title = model.title;
                        banner.url = model.url;
                    }
                    context.SaveChanges();

                    tran.Commit();
                    InitBanners();
                    return ResultModel.Success("添加成功");
                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error();
                }
            }
        }

        /// <summary>
        /// 删除轮播图
        /// </summary>
        /// <returns></returns>
        public ResultModel DeleteBanner(int[] ids)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var banners = context.bsp_banners.Where(t => ids.Contains(t.id)).ToList();
                    context.bsp_banners.RemoveRange(banners);
                    context.SaveChanges();
                    InitBanners();
                    return ResultModel.Success("删除成功");
                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error();
                }
            }
        }

    }
}
