using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class PRODUCT:MarshalByRefObject
    {
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <returns></returns>
        public ResultModel AddCateGory(string name, int displayorder = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="cateid"></param>
        /// <returns></returns>
        public ResultModel DeleteCateGory(int cateid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddProduct(AddProductModel model)
        {
            throw new NotImplementedException();
        }
    }
}
