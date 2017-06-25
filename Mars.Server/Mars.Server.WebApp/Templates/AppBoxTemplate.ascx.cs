using Mars.Server.BLL;
using Mars.Server.Controls;
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
    public partial class AppBoxTemplate : TemplateBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


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
            DataTable table = new DataTable();
            int count=0;
            OperationResult<DataTable> result =
                BCtrl_DialogMessages.Instance.DialogMessages_GetList(base.PageIndex, PageSize, " StartTime DESC ", "", out count);

            string shortDate = DateTime.Now.ToString("yyyy-MM-dd");
            string startTime = shortDate + " 00:00:00";
            string endTime = shortDate + " 23:59:59";
            string strWhere = " AND StartTime   BETWEEN  '" + startTime + "' ";
            strWhere += " AND  '" + endTime + "'   ORDER BY  StartTime  DESC ";
            OperationResult<IList<DialogMessagesEntity>> result2 =
                                    BCtrl_DialogMessages.Instance.DialogMessages_GetWhere(strWhere);
            int mid = 0;
            if (result2.AppendData != null && result2.AppendData.Count != 0)
            {
                mid = result2.AppendData[0].MessageID;
            }

            table = result.AppendData;
            table.Columns.Add("Remarks", typeof(string));
            table.Columns.Add("ILink", typeof(string));
            table.Columns.Add("ALink", typeof(string));
            table.Columns.Add("Type",typeof(string));
            
            foreach (DataRow row in table.Rows)
            {
                string contentFlag = StringUti.SubStr(row["Contents"].ToString(), 30);
                row["Contents"] = contentFlag;

                int type = row["StartType"].ToInt();

                if (type == 1)
                {
                    row["Type"] = "当天弹出一次";
                }
                else 
                {
                    row["Type"] = "当天启动时弹出";
                }
                

                string imageLinkFlag = StringUti.SubStr(row["ImageLink"].ToString(), 30);
                row["ILink"] = imageLinkFlag;

                string articleLinkFlag = StringUti.SubStr(row["ArticleLink"].ToString(), 30);
                row["ALink"] = articleLinkFlag;
                int id = row["MessageID"].ToInt();
                DateTime stime = row["StartTime"].ToDateTime();
                if (id == mid)
                {
                    row["Remarks"] = "正在使用";
                }
                else 
                {
                    if (stime > DateTime.Now)
                    {
                        row["Remarks"] = "预约";
                    }
                    else 
                    {
                        row["Remarks"] = "已过期";
                    }
                }
            }

            for (int i = 0; i < table.Rows.Count; i++)
            {
                    int id = table.Rows[i]["MessageID"].ToInt();
                    if (id == mid)
                    {
                        DataRow dr = table.NewRow();
                        dr.ItemArray = table.Rows[0].ItemArray;
                        table.Rows[0].ItemArray = table.Rows[i].ItemArray;
                        table.Rows[i].ItemArray = dr.ItemArray;
                        break;
                    }
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
            this.AppBoxList.Visible = true;
        }

        public override void ExportToExcel()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}