using Mzh.Public.Model.Model;
using Remoting;
using Remoting.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication2.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        /// 客户端调用Login之后传回Code，根据code获取openid，如果已存在用户信息，则返回用户信息，否则返回错误状态值，前端再请求用户授权
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ResultModel Login(string code)
        {
            USER user = RemotingHelp.GetModelObject<USER>();
            return user.Login(code);
        }

        /// <summary>
        /// 调用login判断无用户信息后，小程序端通过GetUserInfo之后获取用户信息之后发送给服务端存储,返回用户信息
        /// </summary>
        /// <param name="showUserInfo"></param>
        /// <returns></returns>
        public ResultModel SetUserInfo(ShowUserInfo showUserInfo)
        {
            USER user = RemotingHelp.GetModelObject<USER>();
            return user.SetUserInfo(showUserInfo);
        }
    }

}
