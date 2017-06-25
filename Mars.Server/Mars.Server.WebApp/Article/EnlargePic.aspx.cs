using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Article
{
    public partial class EnlargePic : System.Web.UI.Page
    {
        public int  index=0;
        public string  slideList=string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string st = Request["st"];
            string img = Request["img"];
            
            if (st != null) 
            {
                int.TryParse(st, out index);
            }
            if (img!=null &&img.IndexOf("_") != -1)
            {
                string[] imgSplit = img.Split('_');
                for (int i = 0; i < imgSplit.Length; i++)
                {
                    if (imgSplit[i].IndexOf(".jpg") != -1)
                    {
                        slideList += " <div class='swiper-slide'><figure> ";
                        slideList += " <img id='imgeIndex_"+i+"' src='" + imgSplit[i] + "' alt='' /> ";
                        slideList += " </figure> ";
                        slideList += " </div> ";
                    }
                }
            }
            
        }
    }
}