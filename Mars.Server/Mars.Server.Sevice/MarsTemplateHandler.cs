using Mars.Server.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Mars.Server.Sevice
{
    public class MarsTemplateHandler : IHttpHandler, IRequiresSessionState
    {
        private static readonly Regex REGEX_BETWEEN_TAGS = new Regex(@">\s+<", RegexOptions.Compiled);
        private static readonly Regex REGEX_LINE_BREAKS = new Regex(@"(\n\s+)|(\r\s+)|(\t\s+)", RegexOptions.Compiled);

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string req_path = context.Request.AppRelativeCurrentExecutionFilePath;
            TemplateManager<TemplateBase> tm = new TemplateManager<TemplateBase>();
            TemplateBase tb = tm.LoadTemplate(req_path.Replace(".tt", ".ascx"));
            TemplatePropertyManager.SetProperties(tb, context);

            int mode = -1;
            if (int.TryParse(context.Request.QueryString["__mode"], out mode) && mode == 2) //二进制导出
            {
                if (tb.ValidQueryCondition())
                {
                    tb.ExportToExcel();
                }
            }
            else
            {
                string html = tm.RenderTemplate(tb);
                html = REGEX_BETWEEN_TAGS.Replace(html, "><");
                html = REGEX_LINE_BREAKS.Replace(html, string.Empty);
                context.Response.Clear();
                context.Response.Write(html);
            }
        }
    }
}
