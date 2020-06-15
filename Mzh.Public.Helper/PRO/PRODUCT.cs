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
        public string GetName(string name)
        {
            return "MyName is "+name+": "+DateTime.Now.Millisecond;
        }
    }
}
