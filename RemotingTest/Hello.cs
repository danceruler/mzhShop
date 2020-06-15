using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RemotingTest
{
    public class Hello : MarshalByRefObject
    {
        public Hello()
        {
            Console.WriteLine("调用构造函数");
        }

        ~Hello()
        {
            Console.WriteLine("析构函数被调用");
        }

        //[SoapMethod(XmlNamespace = "RemotingTest", SoapAction = "RemotingTest#Greeting")]
        public string Greeting(string word)
        {
            throw new NotImplementedException();
            //Console.WriteLine("打个招呼吧，我是客户端！"+ word);
            //return String.Format("Hello {0},are you ok?", word);
        }
    }
}
