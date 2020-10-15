using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class BannerCache:MarshalByRefObject
    {
        public void Init()
        {
            throw new NotImplementedException();
        }

        public List<bsp_banners> GetBanners()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加编辑轮播图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddBanner(AddBannerModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除轮播图
        /// </summary>
        /// <returns></returns>
        public ResultModel DeleteBanner(int id)
        {
            throw new NotImplementedException();
        }
    }
}
