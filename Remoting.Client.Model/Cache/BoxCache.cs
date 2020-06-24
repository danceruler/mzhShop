using Mzh.Public.Base;
using Mzh.Public.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class BoxCache:MarshalByRefObject
    {
        public void Init()
        {
            throw new NotImplementedException();
        }

        public List<bsp_boxes> GetBoxes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加载包厢信息
        /// </summary>
        public static void InitBoxes()
        {
            throw new NotImplementedException();
        }
    }
}
