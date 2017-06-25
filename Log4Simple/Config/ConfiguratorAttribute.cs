using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Log4Simple.Repository;

namespace Log4Simple.Config
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public abstract class ConfiguratorAttribute :Attribute
    {
        public abstract void Configure(Assembly sourceassembly, ILoggerRepository targetrepository);
    }
}
