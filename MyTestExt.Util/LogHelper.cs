using System;
using MyTestExt.Utils.Json;

namespace MyTestExt.Utils
{
    public class LogHelper
    {
        private static readonly object lockObj = new object();
        private static log4net.ILog _logerror;
        private static log4net.ILog _logdebug;
        private static log4net.ILog _loginfo;
        private static log4net.ILog Loginfo
        {
            get
            {
                if (_loginfo == null)
                {
                    Init();
                }
                return _loginfo;
            }
        }

        private static log4net.ILog Logerror
        {
            get
            {
                if (_logerror == null)
                {
                    Init();
                }
                return _logerror;
            }
        }

        private static log4net.ILog Logdebug
        {
            get
            {
                if (_logdebug == null)
                {
                    Init();
                }
                return _logdebug;
            }
        }
        private static void Init()
        {
            lock (lockObj)
            {
                _loginfo = log4net.LogManager.GetLogger("loginfo");
                _logerror = log4net.LogManager.GetLogger("logerror");
                _logdebug = log4net.LogManager.GetLogger("logdebug");
            }
        }



        /// <summary>
        /// 格式化输出文字
        /// </summary>
        public static void Info(string format, params object[] args)
        {
            if (args == null || args.Length == 0)
                WriteInfoLog(format);
            else
                WriteInfoLog(string.Format(format, args));
        }

        public static void WriteInfoLog(string info)
        {
            if (Loginfo.IsInfoEnabled)
            {
                Loginfo.Info(info);
            }
        }



        /// <summary>
        /// 记录错误信息，同时也开启Debug 记录
        /// </summary>
        public static void Error(string msg, object req, object rsp, Exception ex = null)
        {
            DebugRequest(msg, req, rsp);

            Error(ex, "发生错误: {0} <BR> Request: {1} &lt;BR&gt; result: {2}"
                , msg, JsonNet.Serialize(req), JsonNet.Serialize(rsp));
        }

        /// <summary>
        /// 格式化输出文字
        /// </summary>
        public static void Error(Exception ex, string format = "", params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format))
                WriteErrorLog(ex);
            else if (args == null || args.Length == 0)
                WriteErrorLog(ex, format);
            else
                WriteErrorLog(ex, string.Format(format, args));
        }

        public static void Error(string format, params object[] args)
        {
            if (args == null || args.Length == 0)
                WriteErrorLog(null, format);
            else
                WriteErrorLog(null, string.Format(format, args));
        }

        public static void WriteErrorLog(Exception ex, string info = "")
        {
            if (!string.IsNullOrWhiteSpace(info) && ex == null)
            {
                Logerror.ErrorFormat("【附加信息】:{0}<br>", new object[] { info });
            }
            else if (!string.IsNullOrWhiteSpace(info) && ex != null)
            {
                string errorMsg = BeautyErrorMsg(ex);
                Logerror.ErrorFormat("【附加信息】:{0}<br>{1}", new object[] { info, errorMsg });
            }
            else if (string.IsNullOrWhiteSpace(info) && ex != null)
            {
                string errorMsg = BeautyErrorMsg(ex);
                Logerror.Error(errorMsg);
            }
        }

        /// <summary>
        /// 美化错误信息
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>错误信息</returns>
        private static string BeautyErrorMsg(Exception ex)
        {
            string errorMsg = string.Format("【异常类型】：{0} <br>【异常信息】：{1} <br>【堆栈调用】：{2}", new object[] { ex.GetType().Name, ex.Message, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            errorMsg = errorMsg.Replace("位置", "<strong style=\"color:red\">位置</strong>");
            return errorMsg;
        }




        /// <summary>
        /// 开启调试日志（true-日志增加 DEBUG文件）
        /// webSite取config.xml IsLogDebug节，
        /// AppService 取config.xml.SystemConfig.LogDebugFlag="true"
        /// 或者取  app.config/web.config.appSettings.LogDebugFlag
        /// </summary>
        public static bool IsLogDebug { get; set; } = false;

        /// <summary>
        /// 开启调试日志（true-日志增加 DEBUG文件）
        /// </summary>
        public static void DebugRequest(string msg, object req, object rsp)
        {
            //Debug("Call Request Done! Msg: {0} <BR> Request: {1} &lt;BR&gt; result: {2}"
            //    , msg, JsonNet.Serialize(req), ConvertValue.HtmlEncode(JsonNet.Serialize(rsp)));
        }

        /// <summary>
        /// 格式化输出文字
        /// </summary>
        public static void Debug(string format, params object[] args)
        {
            if (!Logdebug.IsDebugEnabled || !IsLogDebug)
                return;

            if (args == null || args.Length == 0)
                Logdebug.Info(format);
            else
                Logdebug.InfoFormat(format, args);
            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    Error(ex, format);
            //}
        }


    }
}
