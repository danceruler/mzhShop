using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.BLL.Cache
{
    public class FileCache : MarshalByRefObject, ICache
    {
        public static List<FileCacheModel> files = new List<FileCacheModel>();
        private readonly static object GetFileObject = new object();

        public void Init()
        {
        }

        /// <summary>
        /// 从缓存根据对象名获取文件地址
        /// </summary>
        /// <param name="objectname"></param>
        /// <returns></returns>
        public static string GetFile(string objectname)
        {
            lock (GetFileObject)
            {
                try
                {
                    var filecache = files.OrderByDescending(t => t.expiretime).FirstOrDefault(t => t.objectname == objectname);
                    if (filecache == null || filecache.expiretime < DateTime.Now.AddMinutes(3))
                    {
                        OSSHelper oSSHelper = new OSSHelper();
                        DateTime expireTime = DateTime.Now.AddMinutes(30);
                        string ossurl = oSSHelper.GetFilePath(objectname, expireTime);
                        using (brnshopEntities context = new brnshopEntities())
                        {
                            var bsp_file = context.bsp_files.SingleOrDefault(t => t.objectname == objectname);
                            bsp_file.ossurl = ossurl;
                            if (filecache == null)
                            {
                                FileCacheModel newfilecache = new FileCacheModel();
                                newfilecache.expiretime = expireTime;
                                newfilecache.objectname = objectname;
                                newfilecache.ossurl = ossurl;
                                newfilecache.requestcount = 1;
                                newfilecache.timestamp = bsp_file.timestamp;
                                files.Add(newfilecache);
                                //if(files.Count() > 999)
                                //{
                                //    files.RemoveAt(0);
                                //}//控制filecache数量
                            }
                            else
                            {
                                filecache.ossurl = ossurl;
                                filecache.expiretime = expireTime;
                                filecache.requestcount++;
                                bsp_file.requestcount += filecache.requestcount;
                                filecache.requestcount = 0;
                            }
                            context.SaveChanges();
                        }
                        Logger._.Info(files.Count.ToString());
                        return ossurl;
                    }//缓存中没有或三分钟内要过期就请求oss获取地址存入缓存和数据库
                    else
                    {
                        filecache.requestcount++;
                        return filecache.ossurl;
                    }
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return "";
                }
            }
        }
    }
}
