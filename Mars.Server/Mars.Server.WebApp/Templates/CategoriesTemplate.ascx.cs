using Mars.Server.BLL;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Templates
{
    public partial class CategoriesTemplate : Mars.Server.Controls.TemplateBase
    {
        #region 查询条件 
        [TemplateProperty("Categoriesname", RequestMethod.Get)]
        public string _StrCategoriesName { get; set; }

         [TemplateProperty("ParentCalendarID", RequestMethod.Get)]
        public int _StrParentCalendarID { get; set; }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
              
        }
        protected override System.Data.DataTable QueryDataPerPage()
        {
            return GetQueryData(false);
        } 
        private DataTable GetQueryData(bool isDownload)
        { 
            BCtrl_Categories bll = new BCtrl_Categories();
            int totalcnt = 0; 
            CategoriesSearchEntity entity = new CategoriesSearchEntity();
            
            entity.PageSize = base.PageSize;
            entity.PageIndex = base.PageIndex;
            if (!string.IsNullOrEmpty(_StrCategoriesName))
            {
                entity.CalendarTypeName = _StrCategoriesName;
            }
            if (_StrParentCalendarID > 0)
            {
                entity.ParentCalendarTypeID = _StrParentCalendarID;
            }
            else
            {
                entity.ParentCalendarTypeID = -1;
            }
            
            entity.UseDBPagination = !isDownload;
            entity.OrderfieldType = OrderFieldType.Asc;
            DataTable table = bll.QueryCategoriesTable(entity, out totalcnt);
            base.TotalRecords = totalcnt; 
            return table;  
        }

    }
}