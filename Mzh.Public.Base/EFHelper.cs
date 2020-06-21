using Mzh.Public.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Base
{
    public class EFHelper
    {
        public static brnshopEntities Entity { get; private set; }

        static EFHelper()
        {
            Entity = new brnshopEntities();
        }
    }
}
