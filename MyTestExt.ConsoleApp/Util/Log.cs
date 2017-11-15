using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Util
{
    public class Log
    {


        private static string fileName;
        private static TextWriter textWriter = Console.Out;
        private static FileStream fileStream = null;
        private static DateTime dateTime;
        private static string logFileFlag = string.Empty;
        private static string path;
        private static object lockWriteLog = new object();

        /// <summary>
        /// 日志文件名
        /// </summary>
        public static string LogFileName
        {
            get
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = string.Format("{0}Log{1}", System.AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = string.Format("{0}{1}.log", path, DateTime.Now.ToString("yyyy.MM.dd"));
                    dateTime = DateTime.Now;
                }
                else
                {
                    if (dateTime.Date != DateTime.Now.Date)
                    {
                        fileName = string.Format("{0}{1}.log", path, DateTime.Now.ToString("yyyy.MM.dd"));
                        dateTime = DateTime.Now;
                    }
                }
                return fileName;
            }
        }

        public static void WriteLog(Exception ex)
        {
            if (ex == null) return;
            lock (lockWriteLog)
            {
                CreateLogStream();
                Console.WriteLine("{0} Source {1}: {2} \r\n(\r\n    {3}\r\n)\n", new object[] { DateTime.Now, ex.Source, ex.Message, ex.StackTrace });
                if (textWriter != null)
                    textWriter.Flush();
            }
        }

        public static void WriteLog(string sMessage)
        {
            if (string.IsNullOrEmpty(sMessage)) return;
            lock (lockWriteLog)
            {
                CreateLogStream();
                Console.WriteLine("{0} {1}", DateTime.Now, sMessage);
                if (textWriter != null)
                    textWriter.Flush();
            }
        }

        private static void CreateLogStream()
        {
            bool isCreate = false;
            if (fileStream == null)
                isCreate = true;
            else if (dateTime.Date != DateTime.Now.Date)
            {
                fileStream.Close();
                fileStream = null;
                isCreate = true;
            }
            if (isCreate)
            {
                if (File.Exists(LogFileName))
                    fileStream = File.Open(LogFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                else
                    fileStream = File.Open(LogFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                textWriter = new StreamWriter(fileStream, Encoding.UTF8);
                Console.SetOut(textWriter);
                Console.SetError(textWriter);
            }
        }
    }
}
