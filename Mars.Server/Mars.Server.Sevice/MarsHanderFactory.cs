using Mars.Server.Sevice.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Mars.Server.Sevice
{
    /// <summary>
    /// 自定义处理程序工厂
    /// </summary>
    public class MarsHanderFactory : IHttpHandlerFactory
    {
        private static Regex R_Url = new Regex(@"Handlers/(.*?)/(.*?)\.ashx", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            Match mc = R_Url.Match(url);

            if (mc.Success)
            {
                return new ActionHandler() { MethordKey = string.Format("{0}.{1}", mc.Groups[1].Value, mc.Groups[2].Value) };
            }
            else
            {
                return new Error404Handler();
            }
        }

        public void ReleaseHandler(IHttpHandler handler)
        {            
        }
    }
}
