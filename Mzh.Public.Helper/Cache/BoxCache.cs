using Mzh.Public.Base;
using Mzh.Public.BLL.Cache;
using Mzh.Public.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class BoxCache:MarshalByRefObject, ICache
    {
        /// <summary>
        /// 包厢信息
        /// </summary>
        public static List<bsp_boxes> boxes = new List<bsp_boxes>();

        public void Init()
        {
            InitBoxes();
        }

        public List<bsp_boxes> GetBoxes()
        {
            return boxes;
        }

        /// <summary>
        /// 加载包厢信息
        /// </summary>
        public static void InitBoxes()
        {
            string sql = $@"SELECT * FROM bsp_boxes WHERE STATE = 1";
            DataTable boxdt = SqlManager.FillDataTable(AppConfig.ConnectionString, sql);
            boxes = boxdt.GetList<bsp_boxes>(null);
        }
    }
}
