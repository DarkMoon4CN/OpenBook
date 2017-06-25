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
    public partial class SignSMSTemplate : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        #region 查询条件
        private string _hidType;
        /// <summary>
        /// key
        /// </summary>
        [TemplateProperty("hidType", RequestMethod.Get)]
        public string HidType
        {
            get { return _hidType; }
            set { _hidType = value; }
        }

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

            AdminSessionEntity ue = (AdminSessionEntity)Session[WebKeys.AdminSessionKey];
            if (ue != null)
            {
                strWhere = " SysUserID = " + ue.Sys_UserID + " ";
            }
            else 
            {
                return new DataTable();
            }
            if (HidType == "2")
            {
                strWhere += " AND IsSend=0 ";
            }
            int keyFlag=0;
            int.TryParse(Key, out keyFlag);
            switch (keyFlag) 
            {
                case 1: strWhere += " AND Customer like '%"+Value.Trim()+"%' "; break;
                case 2: strWhere += " AND Moblie like '%" + Value.Trim() + "%' "; break;
                case 3: strWhere += " AND Content like '%" + Value.Trim() + "%' "; break;
                case 4:
                    DateTime stime = DateTime.Parse(Value);
                    DateTime etime = DateTime.Parse(Value).AddDays(1).AddSeconds(-1);
                    strWhere += " AND SendTime >='" + stime + "' AND  SendTime<= '"+etime+"' ";
                    break;
            }
            int count = 0;
            DataTable table =
                BCtrl_SMS.Instance.SMS_GetList(base.PageIndex, PageSize, " SendTime DESC ", strWhere, out count);
           
            foreach (DataRow row in table.Rows)
            {
              string contentFlag = StringUti.SubStr(row["Content"].ToString(),30);
              row["Content"] = contentFlag;
            }
            base.TotalRecords = count;
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
            tmpMessage = "查询结果：共有 " + TotalRecords + " 个数据";
            return tmpMessage;
        }

        public override void HandleTemplate()
        {
            this.SignSMSList.Visible = true;
        }

        public override void ExportToExcel()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}