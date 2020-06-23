using Mzh.Public.Base;
using Remoting;
using Remoting.Client.Model;
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
            
            SKU sku = RemotingHelp.GetModelObject<SKU>();
            for (int i = 24; i <= 53; i++)
            {
                sku.AddSKU(i, 164, 0, 100);
            }
            RemotingHelp.GetModelObject<ProductCache>().Init();
            Console.ReadKey();
        }
    }
}
