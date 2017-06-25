namespace Mars.Server.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    /// <summary>
    /// 程序路径
    /// </summary>
    public class AppPath
    {
        static AppPath()
        {
            //if (!Directory.Exists(PluginFolder))
            //{
            //    Directory.CreateDirectory(PluginFolder);
            //}

            if (!Directory.Exists(TempFolder))
            {
                Directory.CreateDirectory(TempFolder);
            }

            //if (!Directory.Exists(RawDataFolder))
            //{
            //    Directory.CreateDirectory(RawDataFolder);
            //}

        }

        
        public static string App_Root = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        /// <summary>
        /// 日志路径
        /// </summary>
        public static string LogFolder = Path.Combine(App_Root.Substring(0, App_Root.LastIndexOf(@"\") + 1), "Logs");

        /// <summary>
        /// 日志路径
        /// </summary>  
        public static string TempFolder = Path.Combine(App_Root.Substring(0, App_Root.LastIndexOf(@"\") + 1), "Temp");

        public static string PluginFolder = Path.Combine(App_Root.Substring(0, App_Root.LastIndexOf(@"\") + 1), "Plugins");

        /// <summary>
        /// 中间数据根目录
        /// </summary>
        public static string RawDataFolder = Path.Combine(App_Root.Substring(0, App_Root.LastIndexOf(@"\") + 1), "RawData");



        public static string HostName = System.Net.Dns.GetHostName();
    }
}

