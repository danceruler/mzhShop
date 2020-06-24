using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Model.Model
{
    [Serializable]
    public class ResultModel
    {
        /// <summary>
        /// 1成功2失败3异常
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 携带对象
        /// </summary>
        public object obj { get; set; }

        public static ResultModel Success(string msg = "",object obj = null)
        {
            return new ResultModel()
            {
                state = 1,
                msg = msg,
                obj = obj
            };
        }

        public static ResultModel Fail(string msg = "", object obj = null)
        {
            return new ResultModel()
            {
                state = 2,
                msg = msg,
                obj = obj
            };
        }

        public static ResultModel Error(string msg = "", object obj = null)
        {
            return new ResultModel()
            {
                state = 3,
                msg = msg,
                obj = obj
            };
        }
    }

    [Serializable]
    public class LayuiApiResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public object data { get; set; }

    }
}
