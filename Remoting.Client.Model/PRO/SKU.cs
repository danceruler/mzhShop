using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class SKU : MarshalByRefObject
    {
        /// <summary>
        /// 添加规格属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel AddAttribute(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加规格属性值
        /// </summary>
        /// <param name="attrid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel AddAttributeValue(short attrid, string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 商品添加sku信息
        /// </summary>
        public ResultModel AddSKU(int pid, int valueid, int isdefaultprice, decimal price)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 商品删除SKU信息
        /// </summary>
        /// <returns></returns>
        public ResultModel DeleteSKU(int pid, int valueid)
        {
            throw new NotImplementedException();
        }
    }
}
