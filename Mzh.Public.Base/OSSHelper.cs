using Aliyun.OSS;
using Aliyun.OSS.Util;
using Mzh.Public.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Base
{
    public class OSSHelper
    {
        private string endpoint = ConfigurationManager.AppSettings["ossendpoint"];
        private string accessKeyId = ConfigurationManager.AppSettings["ossaccessKeyId"];
        private string accessKeySecret = ConfigurationManager.AppSettings["ossaccessKeySecret"];
        private string bucketName = ConfigurationManager.AppSettings["ossbucketName"];
        private static Dictionary<string, string> SuffixContentType = new Dictionary<string, string>()
        {
            { "png","image/png" },
            { "jpeg","image/jpeg" },
            { "jpg","image/jpeg" },
            { "jpe","image/jpeg" },
            { "gif","image/gif" },
        };

        public bool Upload(byte[] content,string filename,string objectName,string contentType)
        {
            var objectContent = content;
            // 创建OssClient实例。
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            try
            {
                StreamFileHelper streamFileHelper = new StreamFileHelper();
                // 上传文件。
                var metadata = new ObjectMetadata()
                {
                    // 指定文件类型。
                    ContentType = contentType,
                    // 设置缓存过期时间，格式是格林威治时间（GMT）。
                    ExpirationTime = DateTime.Parse("2025-10-12T00:00:00.000Z"),
                };
                metadata.CacheControl = "No-Cache";
                var saveAsFilename = filename;
                var contentDisposition = string.Format("attachment;filename*=utf-8''{0}", HttpUtils.EncodeUri(saveAsFilename, "utf-8"));
                metadata.ContentDisposition = contentDisposition;
                PutObjectRequest request = new PutObjectRequest(bucketName, objectName, streamFileHelper.BytesToStream(objectContent));
                request.Metadata = metadata;
                var putResult = client.PutObject(request);
                return true;
            }
            catch (Exception ex)
            {
                Logger._.Error(ex);
                return false;
            }
        }

        public string GetFilePath(string objectname,DateTime expirationTime)
        {
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            var uri = client.GeneratePresignedUri(bucketName, objectname, expirationTime);
            var url = uri.AbsoluteUri;
            return url;
        }

        public static string GetContentTypeBySuffix(string suffixname)
        {
            if (SuffixContentType.Keys.Contains(suffixname))
                return SuffixContentType[suffixname];
            else
                return "application/octet-stream";
        }
    }
}
