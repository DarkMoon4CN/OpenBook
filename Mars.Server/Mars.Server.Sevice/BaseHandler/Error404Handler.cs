using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mars.Server.Sevice.BaseHandler
{
    public class Error404Handler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpContext.Current.Response.StatusCode = 404;
            HttpContext.Current.Response.End();
        }
    }
}
