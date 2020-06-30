using Mzh.Public.Base;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.BLL.HELP
{
    public class UPLOAD : MarshalByRefObject
    {
        public ResultModel Upload(byte[] content, string filename, string client)
        {
            throw new NotImplementedException();
        }

        public ResultModel GetFilePath(string objectname)
        {
            throw new NotImplementedException();
        }
    }
}
