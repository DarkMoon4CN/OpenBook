using Mars.Server.BLL.NewYear;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity.NewYear;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.NewYear
{
    public partial class preview : WxBasePage
    {
        protected int cid = 28;//默认开卷日历春联
        protected int imgType = 1;
        protected CoupletGroupEntity item;
        protected FuImageEntity imageItem;
        protected string wordstyle = "cssword";
        protected string wordcss = "";
        protected string worddraw = "";
        protected string shareurl = "";
        protected string lastpage = "";
        protected bool IsView = false;
        protected string shareCount = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["c"]))
            {
                int.TryParse(Request.QueryString["c"], out cid);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["i"]))
            {
                int.TryParse(Request.QueryString["i"], out imgType);
            }
            if (Request.UrlReferrer == null)
            {
                lastpage = "";
                IsView = true;
            }
            else
            {
                lastpage = Request.UrlReferrer.ToString();
                if (Request.UrlReferrer.ToString().ToLower().IndexOf("index") <0)
                {
                    IsView = true;
                }
            }
            if (!IsPostBack)
            {
                InitPageData();
                shareurl = Request.Url.AbsoluteUri.ToString().Trim(); //WebMaster.Domain + "newyear/view.aspx" + Request.Url.Query.ToString();
                this.CheckSignature(shareurl);
            }
        }

        private void InitPageData()
        {
            BCtrl_Couplet bll = new BCtrl_Couplet();
            item = bll.GetCoupletGroupEntity(cid);
            imageItem = bll.GetFuImageEntity(imgType);
            if (item == null)
            {
                item = new CoupletGroupEntity()
                {
                    CoupletID = 28,
                    CoupletTypeID = 1,
                    UpCouplet = "开年大吉书卷飘香",
                    DownCouplet = "日子红火历久弥新",
                    HorizontalCouplet = "开卷日历"
                };
            }
            else
            {
                if (item.CoupletID > 29)
                {
                    //同步
                    //wordcss = "<script type=\"text/javascript\" src=\"http://api.youziku.com/webfont/FastJS/yzk_CE1B5D52F855A6C4\"></script>";
                    //异步
                    //wordcss = "<script type=\"text/javascript\" src=\"http://youzikuwebfont.oss-cn-beijing.aliyuncs.com/api.lib.min.js\"></script>";
                    wordstyle = "write_text";
                    //同步
                    //worddraw = "<script type=\"text/javascript\">$youziku.load(\".csswordw\",\"ac5dc40c8ca34e608ca8ed12d7a1aa4f\",\"jdxingkaifan\");$youziku.draw();</script>";
                    //异步
                    //worddraw = "<script type=\"text/javascript\">$youzikuapi.asyncLoad(\"http://api.youziku.com/webfont/FastJS/yzk_CE1B5D52F855A6C4\", function () { $youziku.load(\".csswordw\", \"ac5dc40c8ca34e608ca8ed12d7a1aa4f\", \"jdxingkaifan\");$youziku.draw(); }) ;</script>";
                }
            }
            if (imageItem == null)
            {
                imageItem = new FuImageEntity()
                {
                    ImageID = 1,
                    ImageUrl = "images/newyear/happiness.png"
                };
            }
            shareCount = bll.GetShareCount().ToString();
        }
    }
}