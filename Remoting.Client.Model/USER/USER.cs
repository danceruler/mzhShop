using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class USER : MarshalByRefObject
    {
        /// <summary>
        /// 调用login判断无用户信息后，小程序端通过GetUserInfo之后获取用户信息之后发送给服务端存储,返回用户信息
        /// </summary>
        public ResultModel SetUserInfo(ShowUserInfo showUserInfo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 客户端调用Login之后传回Code，根据code获取openid，如果已存在用户信息，则返回用户信息，否则返回错误状态值，前端再请求用户授权
        /// </summary>
        public ResultModel Login(string code)
        {
            throw new NotImplementedException();
        }

    }
}
