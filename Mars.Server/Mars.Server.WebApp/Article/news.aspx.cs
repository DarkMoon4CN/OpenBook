using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.Utils;
using Mars.Server.BLL;
using Mars.Server.Entity;
using System.Web.Security;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
using Mars.Server.Controls.BaseControl;

namespace Mars.Server.WebApp.Article
{
    public partial class news : WxBasePage
    {
        public string moblieShareHeader = string.Empty;
        public string moblieShareFooter = string.Empty;
        public string prefix = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Guid eventItemGUID;
                //判定是否是分享页
                string id = Request["pid"];
                string pid = string.Empty;
                if (id.IndexOf("_") != -1)
                {
                    string[] urlSplit = id.Split('_');
                    if(urlSplit[0]=="1")
                    {
                        prefix="1_";
                        moblieShareHeader += "<header class='app-logo-box' style='display: none;' id='divMoblieShareHeader'>";
                        moblieShareHeader += "<div class='logo-box article-content'>";
                        moblieShareHeader += "<figure class='logo'>";
                        moblieShareHeader += "<img src='/images/app-logo.png' alt='开卷日历logo'/>";
                        moblieShareHeader += "</figure>";
                        moblieShareHeader += "<div class='app-open'>";
                        moblieShareHeader += "<a href='http://www.kjrili.com/app/download.html'>立即打开</a>";
                        moblieShareHeader += "</div>";
                        moblieShareHeader += "</div>";
                        moblieShareHeader += "</header>"; 

                        moblieShareFooter += " <div class='app-btn more' id='divMoblieShareFooter' style='display: none;'>";
                        moblieShareFooter += "<span></span><a href='http://www.kjrili.com/app/download.html' class='more-btn'><img src='/images/share_icon.png'/>";
                        moblieShareFooter += "打开开卷日历,看更多精彩内容</a> ";
                        moblieShareFooter += "</div> ";
                    }
                    pid = urlSplit[1];
                }
                else 
                {
                    pid = id;
                }
                if (Guid.TryParse(pid, out eventItemGUID))
                {
                    this.hidGlobalGUID.Value = eventItemGUID.ToString();
                    BCtrl_EventItem bll = new BCtrl_EventItem();

                    EventItemViewEntity entity = bll.QueryViewEntity(eventItemGUID);


                    title = entity.Title;
                    if (title.Length > 25)
                    {
                        title = title.Substring(0, 25) + "...";
                    }
                    Page.Title = entity.Title;
                    desc = entity.Content;
                    imgUrl =entity.Domain+entity.PicturePath + "?imageView2/0/interlace/1/format/jpg";
                    //设置带有下载头的url
                    link = Request.Url.Host + Request.RawUrl;
                    CheckSignature(link);
                    this.hidNewsPath.Value = WebMaster.ArticleDomain + eventItemGUID.ToString() + ".html";
                }  
                else
                {
                    WebMaster.EndPage();
                }
            }
        }
    }
}