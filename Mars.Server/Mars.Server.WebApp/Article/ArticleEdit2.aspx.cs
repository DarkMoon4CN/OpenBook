using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.Utils;
using Mars.Server.BLL;
using Mars.Server.Entity;
using System.Data;

namespace Mars.Server.WebApp.Article
{
    public partial class ArticleEdit2 : System.Web.UI.Page
    {
        BCtrl_EventItem bllEventItem = new BCtrl_EventItem();
        BCtrl_Zone bllZone = new BCtrl_Zone();
        BCtrl_CalendarType bllCalendarType = new BCtrl_CalendarType();

        int eventItemID;
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["pid"], out eventItemID);
            if (!Page.IsPostBack)
            {
                InitData();

                if (eventItemID > 0)
                {
                    //绑定数据
                    BindData();
                }
                else
                {
                    WebMaster.EndPage();
                }
            }
        }

        private void InitData()
        {
            this.hidFun.Value = Master.fun.ToString();
        }

        private void BindData()
        {
            this.hidPid.Value = eventItemID.ToString();
            EventItemViewEntity entity = bllEventItem.QueryViewEntity(eventItemID);
            if (entity != null)
            {   
                List<CalendarTypeEntity> firstTypeList = bllCalendarType.QueryFirstCalendarType();
                List<CalendarTypeEntity> secondTypeList = bllCalendarType.QuerySecondCalendarType(entity.FirstTypeID);
                List<ZoneEntity> zoneList = bllZone.QueryZoneProvince();

                foreach (CalendarTypeEntity item in firstTypeList)
                {
                    this.selFirstType.Items.Add(new ListItem(item.CalendarTypeName, item.CalendarTypeID.ToString()));
                }
                this.selFirstType.Value = entity.FirstTypeID.ToString();

                foreach (CalendarTypeEntity item in secondTypeList)
                {
                    this.selSecondType.Items.Add(new ListItem(item.CalendarTypeName, item.CalendarTypeID.ToString()));
                }
                this.selSecondType.Value = entity.CalendarTypeID.ToString();

                if (entity.CalendarTypeID == 0 && entity.FirstTypeID == 0)
                {
                    this.chkSubpage.Checked = true;
                    this.selFirstType.Disabled = true;
                    this.selSecondType.Disabled = true;
                    this.chkSingleGroup.Disabled = false;
                }

                this.chkSingleGroup.Checked = entity.IsSingleGroupState;
             
                this.hidPublichAreaID.Value =  bllZone.ConvertZoneIDs(entity.PublishAreaID);

                this.txtActivePlace.Value = entity.ActivePlace;

                this.txtTitle.Value = entity.Title;
                this.txtContent.Value = entity.Content;

                if (!string.IsNullOrEmpty(entity.PicturePath))
                {
                    this.hidImgurl.Value = entity.Domain + entity.PicturePath;
                    this.imgLogo.Src = entity.Domain + entity.PicturePath;
                    this.btnImage.Value = "删除";
                    this.btnImage.Attributes.Add("datatype", "del");
                }              

                if (!string.IsNullOrEmpty(entity.ThemePicturePath))
                {
                    this.hidThemeImgurl.Value = entity.Domain + entity.ThemePicturePath;
                    this.imgThemeLogo.Src = entity.Domain + entity.ThemePicturePath;
                    this.btnThemeImage.Value = "删除";
                    this.btnThemeImage.Attributes.Add("datatype", "del");
                }             

                this.txtHtml.InnerHtml = entity.Html;

                this.hidPublishTime.Value = DateTime.Parse(entity.PublishTime).ToString("yyyy-MM-dd HH:mm:ss");
                this.txtPublishSource.Value = entity.PublishSource;
                this.lblUrl.InnerText = entity.Url;

                this.chkRecommend.Checked = entity.Recommend == 1 ? true:false;
                this.chkAdvert.Checked = entity.Advert == 1? true:false;
                this.chkDiscoverAdvert.Checked = entity.DiscoverAdvert == 1 ? true : false;

                this.txtAdsEndTime.Value = entity.AdsEndTime.Year == 4000 ? "" : entity.AdsEndTime.ToString("yyyy-MM-dd HH:mm:ss");
                this.txtDiscoverAdsEndTime.Value = entity.DiscoverAdsEndTime.Year == 4000 ? "" : entity.DiscoverAdsEndTime.ToString("yyyy-MM-dd HH:mm:ss");
                this.txtAdvertOrder.Value =entity.AdvertOrder.ToString();
                this.hidBookListPath.Value = entity.BookListPath;

                this.hidFestivalID.Value = entity.FestivalID.ToString() != "00000000-0000-0000-0000-000000000000" ? entity.FestivalID.ToString():"";
                
                //if (entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss") == entity.StartTime.ToString("yyyy-MM-dd") + " 00:00:00"
                //    && entity.EndTime.ToString("yyyy-MM-dd HH:mm:ss") == entity.EndTime.ToString("yyyy-MM-dd") + " 23:59:59")
                //{
                //    this.hidStartTime.Value = "4000-01-01 00:00:00";
                //    this.hidEndTime.Value = "4000-01-01 00:00:00";
                //    this.hidallday.Value = entity.StartTime.ToString("yyyy-MM-dd");
                //}
                //else
                {
                    this.hidStartTime.Value = entity.StartTime.ToString("yyyy-MM-dd HH:mm");
                    this.hidEndTime.Value = entity.EndTime.ToString("yyyy-MM-dd HH:mm");
                }

                if (!string.IsNullOrEmpty(entity.CarouselPicturePath))
                {
                    this.hidCarouselImgurl.Value = entity.Domain + entity.CarouselPicturePath;
                    this.imgCarouselLogo.Src = entity.Domain + entity.CarouselPicturePath;
                    this.btnCarouselImage.Value = "删除";
                    this.btnCarouselImage.Attributes.Add("datatype", "del");
                }
                
                if ( entity.EventItemFlag == 1)
                {
                    this.chkFestArticle.Checked = true;
                    this.txtStartTime2.Value = entity.StartTime2.ToString("yyyy-MM-dd HH:mm:ss");
                    this.txtEndTime2.Value = entity.EndTime2.ToString("yyyy-MM-dd HH:mm:ss");
                    this.txtTitle2.Value = entity.Title2;
                }
                else
                {
                    this.chkFestArticle.Checked = false;
                    this.txtStartTime2.Attributes.Add("disabled","disabled");
                    this.txtEndTime2.Attributes.Add("disabled","disabled");
                    this.txtTitle2.Attributes.Add("disabled","disabled");
                }
            }
            else
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "articleEdit2", "<script>bootbox.dialog(\"数据传输错误!\", [{\"label\": \"OK\",\"class\": \"btn-small btn-primary\",callback: function () { window.location.href = 'ArticleManager.aspx?fun="+ Master.fun +"';}}]);</script>");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "articleEdit2", "<script>alert('数据传输错误！');</script>");
            }
        }

    }
}