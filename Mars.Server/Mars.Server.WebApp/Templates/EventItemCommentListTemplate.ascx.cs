using Mars.Server.BLL.Comments;
using Mars.Server.Controls;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity;
using Mars.Server.Entity.Comments;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Templates
{
    public partial class EventItemCommentListTemplate : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region 查询条件
        /// <summary>
        /// 是否是分享 大于0就是分享
        /// </summary>
        [TemplateProperty("_isshare", RequestMethod.Get)]
        public int IsShare { get; set; }

        /// <summary>
        /// 文章id
        /// </summary>
        //[TemplateProperty("_eid", RequestMethod.Get)]
        //public int EventItemID { get; set; }


        /// <summary>
        /// 文章GUID
        /// </summary>
        [TemplateProperty("_eguid", RequestMethod.Get)]
        public string EventItemGUID { get; set; }

        /// <summary>
        /// 当前登录人
        /// </summary>
        [TemplateProperty("_uid", RequestMethod.Get)]
        public int UserID { get; set; }
        #endregion

        protected override System.Data.DataTable QueryDataPerPage()
        {
            return GetQueryData(false);
        }

        protected override System.Data.DataTable QueryDataAllPages()
        {
            return GetQueryData(true);
        }

        BCtrl_EventItemCommentSearch bll = new BCtrl_EventItemCommentSearch();
        EventItemCommentSearchEntity entity = new EventItemCommentSearchEntity();
        private DataTable GetQueryData(bool isDownload)
        {
            int totalcnt = 0;

            entity.EventItemGUID = EventItemGUID;
            //entity.EventItemID = EventItemID;
            entity.IsShare = IsShare;
            entity.UserID = UserID;

            entity.PageSize = base.PageSize;
            entity.PageIndex = base.PageIndex;
            entity.UseDBPagination = !isDownload;

            DataTable table = bll.QueryData(entity, out totalcnt);
            base.TotalRecords = totalcnt;

            return table;
        }

        #region 重写

        /// <summary>
        /// 重写
        /// </summary>
        protected override void RenderJsonData()
        {
            if (ValidQueryCondition())
            {
                this.Visible = false;
                Response.Clear();
                DataTable dt = QueryDataPerPage();

                var data = new
                {
                    cnt = TotalRecords,
                    pindex = this.PageIndex,
                    uid = UserID,
                    share = IsShare,
                    useranonymousheader = WebMaster.UserAnonymousHeader,
                    userdefaultheader = WebMaster.UserDefaultHeader,
                    showstyle = 2,
                    list = dt
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
            else
            {
                var data = new
                {
                    cnt = 0,
                    pindex = this.PageIndex,
                    uid = UserID,
                    share = IsShare,
                    useranonymousheader = WebMaster.UserAnonymousHeader,
                    userdefaultheader = WebMaster.UserDefaultHeader,
                    list = new DataTable()
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
        }

        #endregion
    }
}