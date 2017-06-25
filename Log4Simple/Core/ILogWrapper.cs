using System;
using System.Collections.Generic;
using System.Text;

namespace Log4Simple.Core
{
    public interface ILogWrapper
    {
        ILog Log { get; set; }     
    }
}
