using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Topshelf;

namespace Mars.Server.WordCheckService
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            HostFactory.Run(x =>
            {
                x.UseLog4Net();

                x.Service<ServiceRunner>();

                x.SetDescription("开卷日历：评论模块敏感词检测服务");
                x.SetDisplayName("Mars.WordCheckService");
                x.SetServiceName("Mars.WordCheckService");

                x.EnablePauseAndContinue();
            });
        }
    }
}
