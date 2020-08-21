using Mzh.Public.Base;
using Mzh.Public.BLL.Cache;
using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.BLL.HELP
{
    public class UPLOAD : MarshalByRefObject
    {
        public ResultModel Upload(byte[] content, string filename,string client)
        {
            OSSHelper osshelper = new OSSHelper();
            var suffixname = filename.Split('.')[filename.Split('.').Length - 1];
            var timestamp = EncryptHelp.GetTimeStamp();
            var objectName = EncryptHelp.GetRandomStr(4) + timestamp + "." + suffixname;
            if (!osshelper.Upload(content, filename, objectName, OSSHelper.GetContentTypeBySuffix(suffixname)))
                return ResultModel.Error("oss文件上传失败，请联系管理员");
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    bsp_files newfile = new bsp_files();
                    newfile.client = client;
                    newfile.name = filename;
                    newfile.ossurl = "";
                    newfile.requesturl = "";
                    newfile.suffixname = suffixname;
                    newfile.timestamp = timestamp;
                    newfile.uploadtime = DateTime.Now;
                    newfile.objectname = objectName;
                    newfile.requestcount = 0;
                    newfile.domain = ConfigurationManager.AppSettings["filedomain"];
                    context.bsp_files.Add(newfile);
                    context.SaveChanges();

                    //DateTime expirationTime = DateTime.Now.AddMinutes(30);
                    //GetFilePath(objectName);
                    return ResultModel.Success(newfile.domain+newfile.objectname);
                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        public ResultModel GetFilePath(string objectname)
        {
            return ResultModel.Success("", FileCache.GetFile(objectname));
        }
    }
}
