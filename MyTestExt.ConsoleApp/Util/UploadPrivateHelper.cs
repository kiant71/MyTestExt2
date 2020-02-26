using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Util;
using MyTestExt.Utils;
using MyTestExt.Utils.Json;

namespace MyTestExt.ConsoleApp.Util
{
    /// <summary>
    /// 上传到私有云
    /// </summary>
    public class UploadPrivateHelper
    {
        #region MyRegion
        /// <summary>
        /// 文档服务器地址
        /// </summary>
        private string _documentServer { get; set; }

        /// <summary>
        /// 服务器key
        /// </summary>
        private string _documentKey { get; set; }

        /// <summary>
        /// API 上传数据大小: 1M
        /// </summary>
        private int _uploadSize { get; } = 1024 * 1024;

        #endregion

        public UploadPrivateHelper(string documentServer, string documentKey)
        {
            _documentServer = documentServer;
            _documentKey = documentKey;
        }

        /// <summary>
        /// 私有云 上传，并返回 服务器全路径
        /// </summary>
        public string Upload(UploadPrivateModel post, Stream stream)
        {
            if (string.IsNullOrWhiteSpace(post?.FileName))
                throw new Exception("无效的参数!");

            var api = "";
            try
            {
                var reader = new BinaryReader(stream);
                var postSize = stream.Length;
                stream.Position = 0L;
                var uploadSize = _uploadSize;
                var uploadPosition = 0L;

                while (uploadPosition < postSize)
                {
                    var uploadData = uploadPosition + uploadSize < postSize
                        ? reader.ReadBytes(uploadSize)
                        : reader.ReadBytes((int)(postSize - uploadPosition));


                    #region 续传的情况下，地址栏变动

                    api = _documentServer + "/uploadfile/{key}/{fileName}/{extName}".ToLower();
                    var apiParams = new SortedDictionary<string, object>
                    {
                        ["key".ToLower()] = _documentKey,
                        ["fileName".ToLower()] = string.IsNullOrWhiteSpace(post.FileServerName)
                            ? "null"  // 初次上传写 null
                            : post.FileServerName,
                        ["extName".ToLower()] = post.Ext
                    };
                    foreach (var para in apiParams)
                    {
                        api = api.Replace("{" + para.Key + "}", para.Value.ToString());
                    } 
                    #endregion


                    var uri = new Uri(api);
                    var content = new ByteArrayContent(uploadData);
                    content.Headers.ContentType = GetMediaTypeHeaderValue(post.Ext);
                    var client = new HttpClient();
                    var response = client.PostAsync(uri, content).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        #region 分段上传成功

                        var apiResStr = response.Content.ReadAsStringAsync().Result ?? "";
                        var apiResObj = JsonNet.Deserialize<UploadPrivateRspApiModel>(apiResStr);
                        if (apiResObj != null && apiResObj.IsSuccess)
                        {
                            // 第一段上传后，初始化 fileName参数，供后面续传用
                            if (uploadPosition == 0)
                            {
                                post.FileServerName = apiResObj.FileServerName;
                                post.FullUrlName = apiResObj.FullUrlNameGet(_documentServer);
                            }
                        }
                        else
                        {
                            var msg = "私有云上传网络请求失败，请重试!";
                            LogHelper.Error(string.Format(msg + " url:{0}, apiResStr:{1}", api, apiResStr));
                            throw new Exception(msg); // 发生错误立即返回报错
                        }
                        #endregion
                    }
                    else
                    {
                        #region 分段上传失败：response.StatusCode != HttpStatusCode.OK
                        var msg = "私有云上传网络请求失败，请重试!";
                        LogHelper.Error(string.Format(msg + " url:{0}, StatusCode:{1}", api, response.StatusCode));
                        throw new Exception(msg); // 发生错误立即返回报错 
                        #endregion
                    }

                    // 下一个起始点
                    uploadPosition += uploadData.Length;
                }  // end.of. while (uploadPosition < postSize)
            }
            catch (Exception e)
            {
                post.FileServerName = "";
                post.FullUrlName = "";

                var msg = "私有云上传网络请求失败，请重试!";
                LogHelper.Error(e, string.Format(msg + " url:{0},", api));
                throw new Exception(msg); // 发生错误立即返回报错 
            }

            
            return post.FullUrlName;
        }


        public MediaTypeHeaderValue GetMediaTypeHeaderValue(string extName)
        {
            switch (extName.Replace(".", "").ToLower())
            {
                case "pdf":
                    return new MediaTypeHeaderValue("application/pdf");
                case "jpg":
                case "jpeg":
                    return new MediaTypeHeaderValue("image/jpg");
                case "doc":
                case "docx":
                    return new MediaTypeHeaderValue("application/msword");
                default:
                    return new MediaTypeHeaderValue("multipart/form-data");
            }
        }
    }

    #region model
    public class UploadPrivateModel
    {
        /// <summary>
        /// 原始文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件总大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 文件扩展名，不带(.)号，ex:jpg
        /// </summary>
        public string Ext
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FileName))
                    return "";

                var spearIndex = FileName.LastIndexOf('.');
                if (spearIndex > 0 && spearIndex + 1 < FileName.Length)
                    return FileName.Substring(spearIndex + 1);
                return "";
            }
        }
        
        /// <summary>
        /// 分块上传的拼接标识（相对路径），如果为空则是第一次上传(null)，否则是上一次传递时返回的 服务器文件名
        /// </summary>
        public string FileServerName { get; set; } = "null";

        /// <summary>
        /// 上传后全路径
        /// </summary>
        public string FullUrlName { get; set; }
    }

    /// <summary>
    /// API返回数据，同时该结构也返回到前端
    /// </summary>
    public class UploadPrivateRspApiModel
    {
        public bool IsSuccess => ErrorCode == 0 && string.IsNullOrWhiteSpace(ErrorMsg);

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; } = "";

        /// <summary>
        /// 需要注意的是如果上传的是图片，返回的文件名则是缩略图（小图）的地址，
        /// 例如：414a30ad0c654c4b8a636a81d4a9bf78_2014_01_01_e6eb435d22c04517a6dc153106e7492b-s.jpg，如果要获取原图文件名则把文件名中的-s去掉即可
        /// </summary>
        public string FileName { get; set; } = "";

        /// <summary>
        /// 服务器文件名，注意不带 -s，带扩展名
        /// 分段上传使用，第一次传null，第二次和第三次传api返回结果中的服务器文件名
        /// </summary>
        public string FileServerName => (FileName ?? "").Replace("-s", "");

        /// <summary>
        /// 上传到服务器后获取的服务器(全路径)文件地址
        /// </summary>
        public string FullUrlNameGet(string downServer)
        {
            return downServer + "/downloadfile/" + FileServerName;
        }
    }


    #endregion
}
