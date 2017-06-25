using Mars.Server.BLL;
using Mars.Server.Sevice.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mars.Server.Sevice.Controller
{
    [AjaxController]
    public class CategoriesController : BaseController
    {

        [AjaxHandlerAction]
        public string IsUseableName(HttpContext context)  //判断分类是否可以添加
        {
            bool flg = false;
            string id = context.Request.Form["id"];
            string pid = context.Request.Form["pid"];
            string fname = context.Request.Form["fname"];
            if (!string.IsNullOrEmpty(fname) && !string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(id))
            {
                BCtrl_Categories cate = new BCtrl_Categories();
                flg = cate.IsUserable(int.Parse(pid), fname, int.Parse(id));
                if (flg)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }
        }

        [AjaxHandlerAction]
        public string DeleteCategories(HttpContext contex)
        {
            string id = contex.Request.Form["id"].ToString();
            string str = "{\"status\":0}";
            var strr = new { status = "0" };
            BCtrl_Categories bll = new BCtrl_Categories();
            if (bll.deleteCategories(id) == "0")
            {
                str = "{\"status\":0}";
            }
            else if (bll.deleteCategories(id) == "1")
            {
                str = "{\"status\":1}";
            }
            else
            {
                str = "{\"status\":2}";
            }
            return str;
        }
    }
}
