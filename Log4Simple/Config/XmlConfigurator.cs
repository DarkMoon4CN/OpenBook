using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Log4Simple.Repository;

namespace Log4Simple.Config
{
    public class XmlConfigurator
    {

        static private void InternalConfigureFromXml(ILoggerRepository repository, XmlElement element)
        {
            if (repository != null && element != null)
            {
                IXmlRepositoryConfigurator confrepository = repository as IXmlRepositoryConfigurator;
                if (confrepository != null)
                {
                    XmlDocument newdoc = new XmlDocument();                    
                    XmlElement newelement = (XmlElement)newdoc.AppendChild(newdoc.ImportNode(element, true));
                    confrepository.Configure(newelement);
                }
            }
        }

        static private void InternalConfigure(ILoggerRepository repository, Stream configstream)
        {
            if (configstream == null)
                return;
            XmlDocument doc = new XmlDocument();
        
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;
            XmlReader xmlreader = XmlReader.Create(configstream, settings);

            doc.Load(xmlreader);

            XmlNodeList configNodeList = doc.GetElementsByTagName("log4simple");

            InternalConfigureFromXml(repository, configNodeList[0] as XmlElement);
        }

        static public void Configure(ILoggerRepository repository,FileInfo configfile)
        {
            if (configfile == null)
            {
                throw new ArgumentNullException("Configure called with null 'configFile' parameter");
            }
            else
            {
                if (File.Exists(configfile.FullName))
                {
                    FileStream fs = null;
                    for(int retry = 5; --retry >= 0;)
                    {
                        try
                        {
                            fs = configfile.Open(FileMode.Open,FileAccess.Read,FileShare.Read);
                            break;
                        }
                        catch(IOException ex)
                        {
                            if (retry == 0)
                            {
                                throw new Exception("Failed to open XML config file [" + configfile.Name + "]", ex);
                            }
                            System.Threading.Thread.Sleep(150);
                        }
                    }
                    if (fs != null)
                    {
                        try
                        {
                            InternalConfigure(repository, fs);
                        }
                        finally
                        {
                            fs.Close();
                            fs.Dispose();
                        }
                    }

                }

            }            
        }
    }
}
