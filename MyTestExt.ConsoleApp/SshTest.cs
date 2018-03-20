using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Renci.SshNet;

namespace MyTestExt.ConsoleApp
{
    public class SshTest
    {

        public static void Test()
        {
            //var monitor = FdfsMonitor.Get("192.168.1.205", "root", "pass1");

            //var info = ServerInfo.Get("58.67.219.73", "root", "sap360E206");

            var aa = GetResult("58.67.219.102", "root", "sap360E206", "pwd");

        }

        /// <summary>
        /// 获取命令执行返回
        /// </summary>
        public static string GetResult(string host, string name, string pwd, string strCmd)
        {
            var client = new SshClient(host, name, pwd);
            client.Connect();
            var cmd = client.CreateCommand(strCmd);
            var res = cmd.Execute();
            //var res2 = cmd.Result;
            client.Disconnect();
            client.Dispose();

            return res;
        }

    }

    public class FdfsMonitor
    {
        const string str_cmd = @"/usr/bin/fdfs_monitor /etc/fdfs/client.conf";
        const string pGroup = @" Group (?<GroupInfo>.*?)EndGroup";
        const string pGroupInfo = @"group name = (?<group_name>.*?)\n.*?"
            + @"disk total space = (?<disk_total_space>\d+) MB.*?"
            + @"disk free space = (?<disk_free_space>\d+) MB.*?"
            + @"storage server count = (?<storage_server_count>\d+?).*?"
            + @"active server count = (?<active_server_count>\d+?).*?"
            + @"storage server port = (?<storage_server_port>\d+)";
        const string pStorage = @"Storage .*?"
            + @"ip_addr = (?<Ip_addr>.*?)\s+"
            + @"\b(?<Status>\w+)\b.*?"
            + @"total storage = (?<Total_storage>\d*) MB.*?"
            + @"free storage = (?<Free_storage>\d*) MB.*?"
            + @"total_upload_count = (?<Total_upload_count>\d*).*?"
            + @"last_heart_beat_time = (?<Last_heart_beat_time>.*?)\n.*?"
            + @"last_source_update = (?<Last_source_update>.*?)\n.*?"
            + @"last_sync_update = (?<Last_sync_update>.*?)\n.*?"
            + @"last_synced_timestamp = (?<Last_synced_timestamp>.*?)\n.*?"
            + @"EndStorage";

        #region Properties
        List<StorageGroup> _StorageGroups = new List<StorageGroup>();

        /// <summary>
        /// 主键
        /// </summary>
        public string Uid = Guid.NewGuid().ToString();

        /// <summary>
        /// 监控时间
        /// </summary>
        public string RunTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 存储服务器组
        /// </summary>
        public List<StorageGroup> StorageGroups
        {
            get { return _StorageGroups; }
            set
            {
                _StorageGroups.Clear();
                if (value != null)
                    _StorageGroups.AddRange(value);
            }
        }

        /// <summary>
        /// 服务器信息相关
        /// </summary>
        public Dictionary<string, ServerInfo> DictServerInfo = new Dictionary<string, ServerInfo>();

        #endregion

        /// <summary>
        /// 解析监控字符串
        /// </summary>
        public static FdfsMonitor Get(string ip_addr, string name, string pwd)
        {
            var res = SshTest.GetResult(ip_addr, name, pwd, str_cmd); ;
            if (string.IsNullOrWhiteSpace(res)) return null;

            FdfsMonitor fdfs = new FdfsMonitor();
            //分组进行提取解析 Groups
            res = res.Replace("Group", "EndGroup Group") + "EndGroup";    //每组形成一个 Group...End Group 的闭环            
            Regex rGroup = new Regex(pGroup, RegexOptions.Singleline);
            foreach (Match mGroup in rGroup.Matches(res))
            {
                string strGroup = mGroup.Groups["GroupInfo"].Value;

                //解析组内存储服务器 Storage
                strGroup = strGroup.Replace("Storage", "EndStorage Storage") + "EndStorage"; //每个服务器形成一个 Storage...End Storage 的闭环
                var rStorage = new Regex(pStorage, RegexOptions.Singleline);
                var storages = new List<StorageServer>();
                foreach (Match mStorage in rStorage.Matches(strGroup))
                {
                    var storage = new StorageServer
                    {
                        Ip_addr = mStorage.Groups["Ip_addr"].Value,
                        Status = mStorage.Groups["Status"].Value,
                        Total_storage = int.Parse(mStorage.Groups["Total_storage"].Value),
                        Free_storage = int.Parse(mStorage.Groups["Free_storage"].Value),
                        Total_upload_count = int.Parse(mStorage.Groups["Total_upload_count"].Value),
                        Last_heart_beat_time = mStorage.Groups["Last_heart_beat_time"].Value,
                        Last_source_update = mStorage.Groups["Last_source_update"].Value,
                        Last_sync_update = mStorage.Groups["Last_sync_update"].Value,
                        Last_synced_timestamp = mStorage.Groups["Last_synced_timestamp"].Value
                    };
                    storages.Add(storage);
                }

                //组的信息 GroupInfo
                Regex rGroupInfo = new Regex(pGroupInfo, RegexOptions.Singleline);
                Match mGroupInfo = rGroupInfo.Match(strGroup);
                var storageGroup = new StorageGroup
                {
                    Group_name = mGroupInfo.Groups["group_name"].Value,
                    Disk_total_space = int.Parse(mGroupInfo.Groups["disk_total_space"].Value),
                    Disk_free_space = int.Parse(mGroupInfo.Groups["disk_free_space"].Value),
                    Storage_server_count = int.Parse(mGroupInfo.Groups["storage_server_count"].Value),
                    Active_server_count = int.Parse(mGroupInfo.Groups["active_server_count"].Value),
                    Storage_server_port = int.Parse(mGroupInfo.Groups["storage_server_port"].Value),
                    StorageServers = storages
                };

                fdfs.StorageGroups.Add(storageGroup);
            }

            return fdfs;
        }

    }

    /// <summary>
    /// 存储服务器组
    /// </summary>
    public class StorageGroup
    {
        List<StorageServer> _StorageServers = new List<StorageServer>();

        /// <summary>
        /// 名称
        /// </summary>
        public string Group_name { get; set; }

        /// <summary>
        /// 存储组总空间
        /// </summary>
        public int Disk_total_space { get; set; }

        /// <summary>
        /// 存储组可用空间
        /// </summary>
        public int Disk_free_space { get; set; }

        /// <summary>
        /// 组内服务器总数
        /// </summary>
        public int Storage_server_count { get; set; }

        /// <summary>
        /// 组内“活动中”的服务器总数
        /// </summary>
        public int Active_server_count { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Storage_server_port { get; set; }

        /// <summary>
        /// 组内服务器列表
        /// </summary>
        public List<StorageServer> StorageServers
        {
            get { return _StorageServers; }
            set
            {
                _StorageServers.Clear();
                if (value != null)
                    _StorageServers.AddRange(value);
            }
        }

    }

    /// <summary>
    /// 存储服务器
    /// </summary>
    public class StorageServer
    {
        /// <summary>
        /// Ip_addr 地址
        /// </summary>
        public string Ip_addr { get; set; }

        /// <summary>
        /// 服务器状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 总存储空间（MB）
        /// </summary>
        public int Total_storage { get; set; }

        /// <summary>
        /// 可用存储空间（MB）
        /// </summary>
        public int Free_storage { get; set; }

        /// <summary>
        /// 存储文件数
        /// </summary>
        public int Total_upload_count { get; set; }

        /// <summary>
        /// 服务器状态更新时间
        /// </summary>
        public string Last_heart_beat_time { get; set; }

        /// <summary>
        /// 最新客户端更新时间
        /// </summary>
        public string Last_source_update { get; set; }

        /// <summary>
        /// 最新服务器同步时间
        /// </summary>        
        public string Last_sync_update { get; set; }

        /// <summary>
        /// 最新服务器延迟（秒）
        /// </summary>
        public string Last_synced_timestamp { get; set; }

    }

    /// <summary>
    /// 服务器信息
    /// </summary>
    public class ServerInfo
    {
        const string str_cmd = @"ps -eLf |grep -Ec fdfs ; "
            + @"ss -ant |grep -Ec '22122|36004' ; "   //CentOS 7, 'ss' replace 'netstat'
            + @"ss -ant |grep -Ec '23000|8080'  ; "
            + @"top -b -n 1 |head -15 ; ";
        const string p_cmd = @"(?<Fdfs_process_count>.*?)\n"
            + @"(?<Fdfs_netstat_count>.*?)\n"
            + @"(?<Fdfs_access_count>.*?)\n.*?"
            + @"(?<Detail>"
            + @"up (?<Running_Day>.*?),  \d+ users,  load average: (?<Load_average>.*?)"
            + @"\n.*?(?<Tasks_running>\d+) running,.*?"
            + @"(?<Cpu_us>\d*\.*\d*) us,.*?(?<Cpu_id>\d*\.*\d*) id,.*?"
            + @"(?<Mem_total>\d*) total,.*?(?<Mem_free>\d*) free,"
            + @".*)";  //end.Detail

        #region Properties

        /// <summary>
        /// Ip_addr 地址
        /// </summary>
        public string Ip_addr { get; set; }

        /// <summary>
        /// 运行时间
        /// </summary>
        public string Running_Day { get; set; }

        /// <summary>
        /// 平均负载
        /// </summary>
        public string Load_average { get; set; }

        /// <summary>
        /// 正在运行进程
        /// </summary>
        public string Tasks_running { get; set; }

        /// <summary>
        /// 占用 Cpu
        /// </summary>
        public string Cpu_us { get; set; }

        /// <summary>
        /// 空闲 Cpu
        /// </summary>
        public string Cpu_id { get; set; }

        /// <summary>
        /// 总内存（MB）
        /// </summary>
        public double Mem_total { get; set; }

        /// <summary>
        /// 空闲内存（MB）
        /// </summary>
        public double Mem_free { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Fdfs 进程统计
        /// </summary>
        public string Fdfs_process_count { get; set; }

        /// <summary>
        /// Fdfs “更新操作链接"统计
        /// </summary>
        public string Fdfs_netstat_count { get; set; }

        /// <summary>
        /// Fdfs “访问操作链接”统计
        /// </summary>
        public string Fdfs_access_count { get; set; }

        #endregion


        public static ServerInfo Get(string ip_addr, string name, string pwd)
        {
            var res = SshTest.GetResult(ip_addr, name, pwd, str_cmd);
            var r_cmd = new Regex(p_cmd, RegexOptions.Singleline);
            var m_cmd = r_cmd.Match(res);
            var info = new ServerInfo
            {
                Ip_addr = ip_addr,
                Running_Day = m_cmd.Groups["Running_Day"].Value,
                Load_average = m_cmd.Groups["Load_average"].Value,
                Tasks_running = m_cmd.Groups["Tasks_running"].Value,
                Cpu_us = m_cmd.Groups["Cpu_us"].Value,
                Cpu_id = m_cmd.Groups["Cpu_id"].Value,
                Mem_total = Math.Round(double.Parse(m_cmd.Groups["Mem_total"].Value) / 1024, 2),
                Mem_free = Math.Round(double.Parse(m_cmd.Groups["Mem_free"].Value) / 1024, 2),
                Detail = m_cmd.Groups["Detail"].Value,
                Fdfs_process_count = m_cmd.Groups["Fdfs_process_count"].Value,
                Fdfs_netstat_count = m_cmd.Groups["Fdfs_netstat_count"].Value,
                Fdfs_access_count = m_cmd.Groups["Fdfs_access_count"].Value,
            };

            return info;
        }

    }
}
