using Mzh.Public.Base;
using Mzh.Public.DAL;
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
    /// 商家信息和整体配置类
    /// </summary>
    public class SYS : MarshalByRefObject
    {
        /// <summary>
        /// 获取商家信息
        /// </summary>
        /// <returns></returns>
        public ShowSYSModel GetSYSInfo()
        {
            string sql = $@"SELECT * FROM bsp_businesses";
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
            ShowSYSModel model = dt.GetList<ShowSYSModel>("")[0];
            return model;
        }

        /// <summary>
        /// 编辑商家信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel EditSYSInfo(ShowSYSModel model)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var sys = context.bsp_businesses.FirstOrDefault();
                    sys.address = model.address;
                    sys.name = model.name;
                    sys.businessEnd = sys.businessEnd;
                    sys.businessStart = sys.businessStart;
                    sys.businessTimeStr = sys.businessTimeStr;
                    sys.canSendRadius = sys.canSendRadius;
                    sys.description = sys.description;
                    sys.latitude = sys.latitude;
                    sys.longitude = sys.longitude;
                    sys.showimg = sys.showimg;
                    context.SaveChanges();
                    return ResultModel.Success("修改成功");
                }
                catch(Exception ex)
                {
                    Logger._.Error(ex);
                    return ResultModel.Error();
                }
            }
        }

    }
}
