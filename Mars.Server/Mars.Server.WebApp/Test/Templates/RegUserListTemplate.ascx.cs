using Mars.Server.Controls;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Test.Templates
{
    public partial class RegUserListTemplate : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region 查询条件
        private string _strLoginName;
        /// <summary>
        /// 登录名
        /// </summary>
        [TemplateProperty("loginname", RequestMethod.Get)]
        public string StrLoginName
        {
            get { return _strLoginName; }
            set { _strLoginName = value; }
        }

        private string _strCompanyName;
        /// <summary>
        /// 公司名
        /// </summary>
        [TemplateProperty("companyname", RequestMethod.Get)]
        public string StrCompanyName
        {
            get { return _strCompanyName; }
            set { _strCompanyName = value; }
        }

        private string _strUserType;
        /// <summary>
        /// 公司名
        /// </summary>
        [TemplateProperty("usertype", RequestMethod.Get)]
        public string StrUserType
        {
            get { return _strUserType; }
            set { _strUserType = value; }
        }

        private string _strUserT;
        /// <summary>
        /// 公司名
        /// </summary>
        [TemplateProperty("usert", RequestMethod.Get)]
        public string StrUserT
        {
            get { return _strUserT; }
            set { _strUserT = value; }
        }

        private string _strIsOb;
        /// <summary>
        /// 公司名
        /// </summary>
        [TemplateProperty("isob", RequestMethod.Get)]
        public string StrIsOb
        {
            get { return _strIsOb; }
            set { _strIsOb = value; }
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
            DataTable dt = new DataTable();
            dt.Columns.Add("User_ID", typeof(int));
            dt.Columns.Add("LoginName", typeof(string));
            dt.Columns.Add("UserName", typeof(string));
            dt.Columns.Add("CompanyName", typeof(string));
            dt.Columns.Add("UserIdentityDesc", typeof(string));
            dt.Columns.Add("Mobile", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("RegisterDate", typeof(string));

            for (int i = 1; i <= 50;i++ )
            {
                DataRow row = dt.NewRow();
                row["User_Id"] = i;
                row["LoginName"] = "loginanme" + i;
                row["UserName"] = "张三" + i;
                row["CompanyName"] = "剑神" + i;
                row["UserIdentityDesc"] = "剑神" + i;
                row["Mobile"] = "13738421980" ;
                row["Phone"] = "7842123";
                row["RegisterDate"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                dt.Rows.Add(row);
            }

            base.TotalRecords = 50;
            return dt;               
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
                    message = GetMessage(false),
                    list = dt
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

        public override bool ValidQueryCondition()
        {
            List<KeyValuePair<string, Th>> thList = new List<KeyValuePair<string, Th>>();

            foreach (Control c in mphRegUserList.Controls)
            {
                if (c is Th)
                {
                    Th th = c as Th;
                    thList.Add(new KeyValuePair<string, Th>(th.BindingFiled, th));
                }
            }
            thList.Sort(delegate(KeyValuePair<string, Th> s1, KeyValuePair<string, Th> s2)
            {
                return ((int)((Th)s1.Value).OutPutOrder).CompareTo(((int)((Th)s2.Value).OutPutOrder));
            });

            foreach (KeyValuePair<string, Th> pair in thList)
            {
                this.OutputFiledDic.Add(((Th)pair.Value).BindingFiled, ((Th)pair.Value).Text.ToLower().Replace("<br/>", ""));
            }
            return base.ValidQueryCondition();
        }

        public override void HandleTemplate()
        {
            this.mphRegUserList.Visible = true;
            List<KeyValuePair<string, Th>> thList = new List<KeyValuePair<string, Th>>();

            foreach (Control c in mphRegUserList.Controls)
            {
                if (c is Th)
                {
                    Th th = c as Th;
                    thList.Add(new KeyValuePair<string, Th>(th.BindingFiled, th));
                }
            }
            thList.Sort(delegate(KeyValuePair<string, Th> s1, KeyValuePair<string, Th> s2)
            {
                return ((int)((Th)s1.Value).OutPutOrder).CompareTo(((int)((Th)s2.Value).OutPutOrder));
            });

            foreach (KeyValuePair<string, Th> pair in thList)
            {
                this.OutputFiledDic.Add(((Th)pair.Value).BindingFiled, ((Th)pair.Value).Text.ToLower().Replace("<br/>", ""));
            }
        }

        public override void ExportToExcel()
        {
            throw new NotImplementedException();

            //HttpResponse Res = HttpContext.Current.Response;

            //DataSource = QueryDataAllPages();
            //Dictionary<string, string> tmp = new Dictionary<string, string>();

            //if (DataSource != null)
            //{
            //    foreach (DataColumn dc in DataSource.Columns)
            //    {
            //        tmp.Add(dc.ColumnName, dc.ColumnName);
            //    }
            //}

            //string tmpMessage = "";

            //tmpMessage = "查询结果：共有 " + DataSource.Rows.Count.ToString() + " 个待审批客户";

            //this.Visible = false;
            //Res.Clear();

            //if (DataSource.Rows.Count < 65536)
            //{
            //    using (MemoryStream ms = Utils.ExcelHelper.CreateExcel2003Stream(DataSource, tmp, tmpMessage))
            //    {
            //        Res.ContentType = "application/vnd.ms-excel";
            //        Res.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            //        Res.BinaryWrite(ms.ToArray());
            //    }
            //}
            //else if (DataSource.Rows.Count < 1048576)
            //{
            //    Res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Res.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            //    Res.BinaryWrite(Utils.ExcelHelper.CreateExcel2007Stream(DataSource, tmp));
            //}
            //else
            //{
            //    //todo..
            //}
        }
        #endregion
    }
}