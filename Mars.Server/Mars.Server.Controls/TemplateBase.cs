using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mars.Server.Utils;
using System.IO;
using System.Text.RegularExpressions;

namespace Mars.Server.Controls
{
    public abstract class TemplateBase : UserControlBase
    {
        private string regparas = "fun=(.*)&|fun=(.*)";
        public int fun
        {
            get
            {
                string queryparas = HttpContext.Current.Request.UrlReferrer.Query;
                int result = 0;
                if (!string.IsNullOrEmpty(queryparas))
                {
                    Match maresult = Regex.Match(queryparas, regparas, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    if (maresult.Success)
                    {
                        if (maresult.Groups[1].Value != "")
                        {
                            int.TryParse(maresult.Groups[1].Value, out result);
                        }
                        else
                        {
                            int.TryParse(maresult.Groups[2].Value, out result);
                        }
                    }
                }

                return result;
            }
        }

        protected string QueryString(string key)
        {
            string q = HttpContext.Current.Request.Url.Query.ToLower();
            int start = q.IndexOf(key.ToLower() + "=") + key.Length + 1;
            if (start > -1)
            {
                int end = q.IndexOf("&", start);
                if (end > -1)
                {
                    return q.Substring(start, (end - start));
                }
                else
                {
                    return q.Substring(start);
                }
            }
            else
                return null;
        }

        public virtual void HandleTemplate()
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (ProcessMode)
                {
                    case TemplateProcessMode.DataQuery:
                        RenderJsonData();
                        break;
                    case TemplateProcessMode.Template:
                        {
                            HandleTemplate();
#if DEBUG

#else
                            Response.Cache.SetExpires(DateTime.Now.AddDays(7));
#endif
                        }
                        break;
                    default:
                        break;
                }
            }
            base.OnLoad(e);
        }

        private int processMode = 0;

        /// <summary>
        /// 模版职能模式
        /// </summary>
        [TemplateProperty(Key = "__mode", Method = RequestMethod.Auto)]
        public int ProcessMode_INT
        {
            set { processMode = value; }
        }

        public TemplateProcessMode ProcessMode
        {
            get { return (TemplateProcessMode)processMode; }
        }

        private int _PageIndex;
        /// <summary>
        /// 页码，从1开始计数，防止前端恶意查询，最大值限制99999项。
        /// </summary>
        [TemplateProperty(Key = "_pageIndex", Method = RequestMethod.Auto)]
        public int PageIndex
        {
            get { return _PageIndex; }
            set
            {
                _PageIndex = value <= 99999 ? value + 1 : 99999;
            }
        }

        private int _PageSize;
        /// <summary>
        /// 每页记录数，,防止前端恶意查询，最大值限制500项。
        /// </summary>
        [TemplateProperty(Key = "_pageSize", Method = RequestMethod.Auto)]
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value <= 500 ? value : 500; ; }
        }

        private string _OutputFiledInfo;
        /// <summary>
        /// 导出字段明细（使用逗号分割）
        /// </summary>
        [TemplateProperty(Key = "_fd", Method = RequestMethod.Auto)]
        public string OutputFiledInfo
        {
            get { return _OutputFiledInfo; }
            set { _OutputFiledInfo = value; }
        }

        private Dictionary<string, string> _OutputFiledDic = new Dictionary<string, string>();
        /// <summary>
        /// 导出字段明细，包含绑定字段和显示名称
        /// </summary>
        public Dictionary<string, string> OutputFiledDic
        {
            get { return _OutputFiledDic; }
            set { _OutputFiledDic = value; }
        }

        private int _OrderFieldIndex = 0;

        /// <summary>
        /// 排序索引
        /// </summary>
        [TemplateProperty(Key = "_orderindex", Method = RequestMethod.Auto)]
        public int OrderFieldIndex
        {
            get { return _OrderFieldIndex; }
            set { _OrderFieldIndex = value; }
        }


        private int _OrderFieldType = 0;

        [TemplateProperty(Key = "_ordertype", Method = RequestMethod.Auto)]
        public int OrderFieldType_INT
        {
            set { _OrderFieldType = value; }
        }

        public OrderFieldType OrderType
        {
            get { return (OrderFieldType)_OrderFieldType; }
        }



        private DataTable _DataSource;
        /// <summary>
        /// 数据源
        /// </summary>
        public DataTable DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }

        private int _TotalRecords = -1;
        /// <summary>
        /// 数据源总记录数
        /// </summary>
        public int TotalRecords
        {
            get { return _TotalRecords; }
            set { _TotalRecords = value; }
        }

        private int _OutputCnt = -1;

        public int OutputCnt
        {
            get { return _OutputCnt; }
            set { _OutputCnt = value; }
        }

        private string _Message = "";
        /// <summary>
        /// 数据源备注
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        /// <summary>
        ///  获取数据源/分页
        /// </summary>
        /// <returns></returns>
        protected virtual DataTable QueryDataPerPage()
        {
            return _DataSource;
        }

        protected virtual DataTable QueryDataAllPages()
        {
            return _DataSource;
        }

        /// <summary>
        /// 根据数据源生产json数据
        /// </summary>  
        protected virtual void RenderJsonData()
        {
            if (ValidQueryCondition())
            {
                this.Visible = false;
                Response.Clear();
                DataTable dt = QueryDataPerPage();
                var data = new
                {
                    cnt = TotalRecords,
                    outputcnt = OutputCnt,
                    orderindex = this.OrderFieldIndex,
                    ordertype = (int)(this.OrderType),
                    message = this.Message,
                    list = dt
                };
                Response.Write(Mars.Server.Utils.StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
            else
            {
                var data = new
                {
                    cnt = 0,
                    message = "暂无权限",
                    list = new DataTable()
                };
                Response.Write(Mars.Server.Utils.StringUti.ToUnicode(JsonObj<object>.ToJsonString(data)));
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        public virtual void ExportToExcel()
        {
            #region 暂时不用注销
            //HttpResponse Res = HttpContext.Current.Response;
            //_DataSource = QueryDataAllPages();

            //this.Visible = false;
            //Res.Clear();

            //if (OutputFiledDic.Count == 0)
            //{
            //    if (!string.IsNullOrEmpty(this.OutputFiledInfo))
            //    {
            //        string[] cls = this.OutputFiledInfo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //        foreach (string c in cls)
            //        {
            //            OutputFiledDic.Add(c, c);
            //        }
            //    }
            //    else
            //    {
            //        foreach (DataColumn c in _DataSource.Columns)
            //        {
            //            OutputFiledDic.Add(c.ColumnName, c.ColumnName);
            //        }
            //    }
            //}

            //if (_DataSource.Rows.Count < 65536)
            //{
            //    using (MemoryStream ms = Avengers.Utils.ExcelHelper.CreateExcel2003Stream(_DataSource, OutputFiledDic, this.Message))
            //    {
            //        Res.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            //        Res.BinaryWrite(ms.ToArray());
            //    }
            //}
            //else if (_DataSource.Rows.Count < 1048576)
            //{
            //    Res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Res.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            //    Res.BinaryWrite(Avengers.Utils.ExcelHelper.CreateExcel2007Stream(_DataSource, OutputFiledDic));
            //}
            //else
            //{
            //    //todo..
            //}
            #endregion

            throw new NotImplementedException();
        }


        public virtual bool ValidQueryCondition() { return true; }

    }

    public enum TemplateProcessMode
    {
        Template = 0,
        DataQuery = 1,
        DataExport2003_2007 = 2
    }
}
