using Mars.Server.BLL;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity;
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
    public partial class FunctionsTemplate : Mars.Server.Controls.TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region 查询条件
        private string _StrFunctionName;
        /// <summary>
        /// 登录名
        /// </summary>
        [TemplateProperty("functionname", RequestMethod.Get)]
        public string StrFunctionName
        {
            get { return _StrFunctionName; }
            set { _StrFunctionName = value; }
        }


        #endregion

        protected override System.Data.DataTable QueryDataPerPage()
        {
            return GetQueryData(false);
        }

        protected override System.Data.DataTable QueryDataAllPages()
        {
            return GetQueryData(true);
        }

        private DataTable GetQueryData(bool isDownload)
        {
            BCtrl_Function bll = new BCtrl_Function();
            int totalcnt = 0;

            FunctionSearchEntity entity = new FunctionSearchEntity();
            entity.Function_Name = _StrFunctionName;
            entity.PageSize = base.PageSize;
            entity.PageIndex = base.PageIndex;
            entity.UseDBPagination = !isDownload;

            DataTable table = bll.QueryFunctionTable(entity, out totalcnt);
            base.TotalRecords = totalcnt;

            return table;
        }

        private List<FunctionEntity> GetQueryData()
        {
            BCtrl_Function bll = new BCtrl_Function();         

            FunctionSearchEntity entity = new FunctionSearchEntity();
            entity.Function_Name = _StrFunctionName;         
            entity.UseDBPagination = false;

            return bll.QueryFunctionListByGroup(entity);                
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
                //DataTable dt = QueryDataPerPage();

                List<FunctionEntity> dt = this.GetQueryData();
                var data = new
                {
                    cnt = TotalRecords,
                    list = dt,
                    fun = base.fun
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
            else
            {
                var data = new
                {
                    cnt = 0,
                    message = "暂无权限",
                    list = new DataTable()
                };
                Response.Write(StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
        }

        public override void HandleTemplate()
        {
            this.mphFunctionsList.Visible = true;
        }

        public override void ExportToExcel()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}