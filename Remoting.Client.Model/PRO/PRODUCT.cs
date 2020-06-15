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
        [SoapMethod(XmlNamespace = "Remoting", SoapAction = "Remoting#GetName")]
        public string GetName(string name)
        {
            throw new  NotImplementedException();
        }
    }
}
