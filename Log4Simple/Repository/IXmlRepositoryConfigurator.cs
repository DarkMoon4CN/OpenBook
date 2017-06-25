using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Simple.Repository
{
    public interface IXmlRepositoryConfigurator
    {
        void Configure(System.Xml.XmlElement element);
    }
}
