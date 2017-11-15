using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace MyTestExt.ConsoleApp
{
    public class ServiceControllerTest
    {
        public static void Test()
        {
            ServiceController sc = new ServiceController("SAP360WindowsServiceTest1");
            try
            {
                TimeSpan ts = TimeSpan.FromSeconds(20);

                //if (sc.Status != ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.StartPending)
                //{
                //    sc.Start();
                //    sc.WaitForStatus(ServiceControllerStatus.Running, ts);
                //}
                //else if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending)
                //{
                //    sc.Stop();
                //    sc.WaitForStatus(ServiceControllerStatus.Stopped, ts);
                //}

                var filePath = GetWindowsServiceInstallPath("SAP360WindowsServiceTest1");
                var filePath22 = GetWindowsServiceInstallPath("SAP360WindowsServiceTest22");
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        /// <summary>
        /// 获取服务安装路径
        /// </summary>
        /// <param name="ServiceName"></param>
        /// <returns></returns>
        public static string GetWindowsServiceInstallPath(string ServiceName)
        {
            string key = @"SYSTEM\CurrentControlSet\Services\" + ServiceName;
            string path = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(key).GetValue("ImagePath").ToString();
            //替换掉双引号   
            path = path.Replace("\"", string.Empty);
            FileInfo fi = new FileInfo(path);
            return fi.Directory.ToString();
        }
    }
}
