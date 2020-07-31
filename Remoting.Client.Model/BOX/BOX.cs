using Mzh.Public.Model;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class BOX : MarshalByRefObject
    {
        /// <summary>
        /// 更新包厢
        /// </summary>
        /// <param name="boxid"></param>
        /// <param name="state"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel UpdateBox(int boxid, BoxState state, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新增包厢
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel AddBox(string code, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除包厢
        /// </summary>
        /// <param name="boxid"></param>
        /// <returns></returns>
        public ResultModel DeleteBox(int boxid)
        {
            throw new NotImplementedException();
        }
    }
}
