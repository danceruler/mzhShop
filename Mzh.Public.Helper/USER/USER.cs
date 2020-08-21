using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class USER: MarshalByRefObject
    {
        /// <summary>
        /// 调用login判断无用户信息后，小程序端通过GetUserInfo之后获取用户信息之后发送给服务端存储,返回用户信息
        /// </summary>
        public ResultModel SetUserInfo(ShowUserInfo showUserInfo)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var newuser = new bsp_users() {
                        admingid = 0,
                        avatar = showUserInfo.avater,
                        email = "",
                        gender = showUserInfo.gender,
                        liftbantime = DateTime.Now,
                        mobile = "",
                        nickname = showUserInfo.nickname,
                        openid = showUserInfo.openid,
                        password = "",
                        paycredits = 0,
                        rankcredits = 0,
                        salt = "",
                        username = showUserInfo.username,
                        userrid = 0,
                        verifyemail = 0,
                        verifymobile = 0
                    };
                    context.bsp_users.Add(newuser);
                    context.SaveChanges();
                    showUserInfo.uid = newuser.uid;
                    return ResultModel.Success("", showUserInfo);
                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error();
                }
            }
        }

        /// <summary>
        /// 客户端调用Login之后传回Code，根据code获取openid，如果已存在用户信息，则返回用户信息，否则返回错误状态值，前端再请求用户授权
        /// </summary>
        public ResultModel Login(string code)
        {
            string session2Url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code",
                                                WXPayHelper.appid, WXPayHelper.appsecret, code);

            var jsonstr = HttpHelper.HttpGet(session2Url,"");
            Code2SessionModel result = JsonConvert.DeserializeObject<Code2SessionModel>(jsonstr);
            if(result.errcode != 0)
            {
                Logger._.Error($"code2session接口请求失败,errcode:{result.errcode},errmsg:{result.errmsg}");
                return ResultModel.Error($"code2session接口请求失败,errcode:{result.errcode},errmsg:{result.errmsg}");
            }
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var user = context.bsp_users.SingleOrDefault(t => t.openid == result.openid);
                    if (user == null) return ResultModel.Fail(result.openid);
                    return ResultModel.Success("", user2ShowUser(user));
                }catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error("数据库操作异常");
                }
            }

        }

        private ShowUserInfo user2ShowUser(bsp_users user)
        {
            var ShowUser = new ShowUserInfo();
            ShowUser.uid = user.uid;
            ShowUser.avater = user.avatar;
            ShowUser.nickname = user.nickname;
            ShowUser.openid = user.openid;
            ShowUser.username = user.username;
            ShowUser.gender = user.gender;
            return ShowUser;
        }
    }
}
