using Mzh.Public.Base;
using Mzh.Public.BLL.Cache;
using Mzh.Public.DAL;
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
        public static List<bsp_banners> banners = new List<bsp_banners>();
        public void Init() 
        {
            InitBanners();
        }

        public List<bsp_banners> GetBanners()
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
            banners = dt.GetList<bsp_banners>("");
        }
    }
}
