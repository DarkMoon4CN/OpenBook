using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Sevice.BaseHandler
{
   public class CustomAttributeDefine
    {
    }

    /// <summary>
    /// 自义Http 处理程序类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AjaxControllerAttribute : Attribute
    {
    }

    /// <summary>
    /// 自定义Http Handler处理程序特性标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxHandlerActionAttribute : Attribute
    {
    }
}
