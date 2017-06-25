using System;
using Nancy.Hosting.Self;
using Mars.Server.App.Core;

namespace Mars.Server.App
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            AppServerDataInitializer.Init();
            var uri =
                new Uri(System.Configuration.ConfigurationManager.AppSettings["Server"]);

            using (var host = new NancyHost(uri))
            {
                host.Start();
                SessionCenter.Start();
                //SessionCenter.EmailTiming(); -- 邮件定时发送
                Console.WriteLine("Your application is running on " + uri);
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
                SessionCenter.Stop();
            }
        }
    }
}
