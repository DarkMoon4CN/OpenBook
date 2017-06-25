using Mars.Server.Sevice.BaseHandler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    /// <summary>
    /// 测试AJAX Controller
    /// </summary>
    [AjaxController]
    public class TestController
    {
        [AjaxHandlerAction]
        public string QueryTitleById(HttpContext context)
        {
            int id = int.Parse(context.Request["id"]);
            List<dynamic> dList = new List<dynamic>();
            if (id == 25)
            {
                dList.Add(new { name = "张三", desc = "这是张三，他的爱好广泛" });
            }
            else
            {
                dList.Add(new { name = "王五", desc = "他没什么爱好" });
            }
            return JsonConvert.SerializeObject(dList).ToString();
        }

        public string GET()
        {
            return JsonConvert.SerializeObject("123");
        }
    }
}
