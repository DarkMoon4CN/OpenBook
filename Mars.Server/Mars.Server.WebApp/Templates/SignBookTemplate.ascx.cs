using Mars.Server.BLL;
using Mars.Server.Controls;
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
    public partial class SignBookTemplate : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        #region 查询条件
        private string _key;
        /// <summary>
        /// key
        /// </summary>
        [TemplateProperty("key", RequestMethod.Get)]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _value;
        /// <summary>
        /// value
        /// </summary>
        [TemplateProperty("value", RequestMethod.Get)]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
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

           string strWhere = string.Empty;
           switch (Convert.ToInt32(Key)) 
           {
               case 1: strWhere = " Customer like '%" + Value + "%'  "; break;
               case 2: strWhere = " Company like '%" + Value + "%'  "; break;
               case 3: strWhere = " Moblie like '"+Value+"%'"; break;
               case 4: strWhere = " SalesName like '" + Value + "%'   "; break;
               case 5: strWhere = " IsSign =1   "; break;
               case 6: strWhere = " IsSign =0   "; break;
           }  

           int count=0;
           DataTable table=
               BCtrl_SignBook.Instance.SignBook_GetList(base.PageIndex, PageSize, " CreateTime DESC ", strWhere, out count);
           base.TotalRecords = count;
           table.Columns.Add("IsRegister", typeof(int));
           BCtrl_Users user = new BCtrl_Users();
           foreach (DataRow dr in table.Rows)
           {
               string moblie= dr["Moblie"].ToString();
               UserEntity entity= user.QueryUserInfo(moblie,true);
               if (entity == null || entity.UserID == 0)
               {
                   dr["IsRegister"] = 0;
               }
               else
               {
                   dr["IsRegister"] = 1;
               }
           }
           
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
                    //message = GetMessage(false),
                    list = dt,
                    //fun = base.fun
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

        /// <summary>
        /// 获得消息
        /// </summary>
        /// <returns></returns>
        private string GetMessage(bool isexport)
        {
            string tmpMessage = "";

            tmpMessage = "查询结果：共有 " + TotalRecords + " 个待审批客户";

            return tmpMessage;
        }

        public override void HandleTemplate()
        {
            this.SignBookList.Visible = true;
        }

        public override void ExportToExcel()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}