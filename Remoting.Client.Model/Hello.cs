using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class Hello : MarshalByRefObject
    {

        public string Greeting(string word)
        {
            throw new NotImplementedException();
        }

        public bool CheckClient(string clientId, int type)
        {
            throw new NotImplementedException();
        }

        public void Test()
        {
            throw new NotImplementedException();
        }
    }
}
