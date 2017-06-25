using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;
using Log4Simple.Repository;
using Log4Simple.Util;

namespace Log4Simple.Config
{
    [AttributeUsage(AttributeTargets.Assembly)]
    [Serializable]
    public class XmlConfiguratorAttribute :ConfiguratorAttribute
    {

        public XmlConfiguratorAttribute()
        {
            
        }
        
        public string ConfigFile
        {
            get;
            set;
        }

        public string ConfigFileExtension
        {
            get;
            set;
        }

        public bool Watch
        {
            get;
            set;
        }

        public override void Configure(System.Reflection.Assembly sourceassembly, ILoggerRepository targetrepository)
        {
            IList configmessage = new ArrayList();
            string appbasedir = null;
            try
            {
                appbasedir = AppDomain.CurrentDomain.BaseDirectory;
            }
            catch
            { }

            if (appbasedir != null)
            {
                ConfigureFromFile(sourceassembly, targetrepository);
            }
            else
                throw new Exception("文件目录不存在！");
        }


        private void ConfigureFromFile(Assembly sourceassembly, ILoggerRepository targetrepository)
        {
            string fullpath2configfile = null;
            if (ConfigFile == null || ConfigFile.Length == 0)
            {
                if (ConfigFileExtension == null || ConfigFileExtension.Length == 0)
                {
                    try
                    {
                        fullpath2configfile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    }
                    catch
                    {

                    }
                }
                else
                {
                    if (ConfigFileExtension[0] != '.')
                    {
                        ConfigFileExtension = "." + ConfigFileExtension;
                    }
                    string appbasedir = null;
                    try
                    {
                        appbasedir = AppDomain.CurrentDomain.BaseDirectory;
                    }
                    catch
                    {
                    }
                    if (appbasedir != null)
                        fullpath2configfile = Path.Combine(appbasedir, System.IO.Path.GetFileName(sourceassembly.Location) + ConfigFileExtension);
                }
            }
            else
            {
                string appbasedir = null;
                try
                {
                    appbasedir = AppDomain.CurrentDomain.BaseDirectory;
                }
                catch
                { }


                if (!string.IsNullOrEmpty(ConfigFile) && string.IsNullOrEmpty(Path.GetExtension(ConfigFile))  && !string.IsNullOrEmpty(ConfigFileExtension))
                {
                    if (ConfigFileExtension.Length > 0 && ConfigFileExtension[0] != '.')
                        ConfigFileExtension = '.' + ConfigFileExtension;

                    ConfigFile += ConfigFileExtension;
                }

               


                if (appbasedir != null)
                {
                    if (File.Exists(ConfigFile))
                    {
                        fullpath2configfile = ConfigFile;
                    }                    
                    else
                        fullpath2configfile = Path.Combine(appbasedir, ConfigFile);
                }
                else
                {
                    fullpath2configfile = ConfigFile;
                }
            }

            if (fullpath2configfile != null)
                ConfigureFromFile(targetrepository, new FileInfo(fullpath2configfile));
            else
                LogLog.Error(typeof(XmlConfiguratorAttribute), "未找到配置文件");
        }


        private void ConfigureFromFile(ILoggerRepository targetRepository, FileInfo configFile)
        {
            XmlConfigurator.Configure(targetRepository, configFile);
        }
    }
}
