using Mzh.Public.Base;
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
    public class ORDER : MarshalByRefObject
    {
        /// <summary>
        /// 创建订单
        /// </summary>
        public void CreatOrder(CreateOrderModel createModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        private List<ShowOrderInfo> GetOrderList(string sqlWhere,int page,int count)
        {
            throw new NotImplementedException();
        }
    }
}
