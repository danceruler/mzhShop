using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RemotingTest
{
    public class DataService : MarshalByRefObject
    {
        public DataService()
        {

        }



        public TModel GetDomainModel<TModel>(string model = null) where TModel : class
        {
            throw new NotImplementedException();
        }
    }
}
