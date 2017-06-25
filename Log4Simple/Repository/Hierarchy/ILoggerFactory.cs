using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Simple.Repository.Hierarchy
{
    public interface ILoggerFactory
    {        
        Logger CreateLogger(ILoggerRepository repository, string name);
    }
}
