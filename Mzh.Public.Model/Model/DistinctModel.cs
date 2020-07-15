using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    public class DistinctModel<TModel> : IEqualityComparer<TModel>
    {
        public bool Equals(TModel x, TModel y)
        {
            if(typeof(TModel) == typeof(ShowProductInfo))
            {
                ShowProductInfo t = x as ShowProductInfo;
                ShowProductInfo tt = y as ShowProductInfo;
                if (t != null && tt != null) return t.pid == tt.pid;
                return false;
            }
            else if (typeof(TModel) == typeof(ShowOrderInfo))
            {
                ShowOrderInfo t = x as ShowOrderInfo;
                ShowOrderInfo tt = y as ShowOrderInfo;
                if (t != null && tt != null) return t.oid == tt.oid;
                return false;
            }
            else if (typeof(TModel) == typeof(ShowOrderProductInfo))
            {
                ShowOrderProductInfo t = x as ShowOrderProductInfo;
                ShowOrderProductInfo tt = y as ShowOrderProductInfo;
                if (t != null && tt != null) return t.op_pid == tt.op_pid&&t.op_skuid == tt.op_skuid;
                return false;
            }
            return false;
        }

        public int GetHashCode(TModel obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
