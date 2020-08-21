using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting.Client.Model.SYS
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 编辑商家信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel EditSYSInfo(ShowSYSModel model)
        {
            throw new NotImplementedException();
        }

    }
}
