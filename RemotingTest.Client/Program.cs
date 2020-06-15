using Mzh.Public.Base;
using Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemotingTest.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Hello hello = RemotingHelp.GetModelObject<Hello>();
            hello.Test();
            Console.ReadKey();
        }
    }
}
