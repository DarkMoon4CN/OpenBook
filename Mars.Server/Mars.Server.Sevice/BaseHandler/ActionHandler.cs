using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mars.Server.Sevice.BaseHandler
{
    public class ActionHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return false; }
        }

        private string _MethordKey;

        public string MethordKey
        {
            get { return _MethordKey; }
            set { _MethordKey = value; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(HandlerReflector.ExcuteHandler(context, _MethordKey));
        }
    }
}
