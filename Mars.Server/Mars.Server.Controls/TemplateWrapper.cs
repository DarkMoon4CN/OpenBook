using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.Controls
{
    [DefaultProperty("TemplateSrc")]
    [ToolboxData("<{0}:TemplateWrapper runat=server></{0}:TemplateWrapper>")]
    public class TemplateWrapper : WebControl
    {

        private string _SearchCheckFuc;
        //canSearch
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("定义搜索条件判断函数，返回true或者false，来判断是否执行查询")]
        public string SearchCheckFuc
        {
            get { return _SearchCheckFuc; }
            set { _SearchCheckFuc = value; }
        }

        private string _HandleRetDataFuc;
        //canSearch
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("定义搜索条件判断函数，返回true或者false，来判断是否执行查询")]
        public string HandleRetDataFuc
        {
            get { return _HandleRetDataFuc; }
            set { _HandleRetDataFuc = value; }
        }

        private bool _UseRequestCache = true;
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("是否使用浏览器IE的请求缓存")]
        public bool UseRequestCache
        {
            get { return _UseRequestCache; }
            set { _UseRequestCache = value; }
        }

        //handleRetData

        private string _AlternateTemplateUrls;
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("定义额外模版链接，通过数组ID_templates;获取")]
        public string AlternateTemplateUrls
        {
            get { return _AlternateTemplateUrls; }
            set { _AlternateTemplateUrls = value; }
        }

        #region 属性

        private string _AfterDataLoadedFunc;
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("数据加载完毕后执行的j自定义js方法")]
        public string AfterDataLoadedFunc
        {
            get { return _AfterDataLoadedFunc; }
            set { _AfterDataLoadedFunc = value; }
        }

        private string _OnSearchBtnClickingFunc;
        //onSearchButtonClicked
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("点击查询按钮前的j自定义js方法")]
        public string OnSearchBtnClickingFunc
        {
            get { return _OnSearchBtnClickingFunc; }
            set { _OnSearchBtnClickingFunc = value; }
        }


        private string _OnSearchBtnClickedFunc;
        //onSearchButtonClicked
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("点击查询按钮后的j自定义js方法")]
        public string OnSearchBtnClickedFunc
        {
            get { return _OnSearchBtnClickedFunc; }
            set { _OnSearchBtnClickedFunc = value; }
        }

        private string _InitParameterFunc;
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("重写参数赋值方法")]
        public string InitParameterFunc
        {
            get { return _InitParameterFunc; }
            set { _InitParameterFunc = value; }
        }


        private RequestMethod _HttpMethod = RequestMethod.Get;
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("数据提交方式：GET/POST")]
        public RequestMethod HttpMethod
        {
            get { return _HttpMethod; }
            set { _HttpMethod = value; }
        }

        private String _TemplateSrc;

        /// <summary>
        /// 模版链接 格式：（~/xxx.ascx）
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("模版链接 格式：（~/xxx.ascx）")]
        public string TemplateSrc
        {
            get
            {
                return _TemplateSrc;
            }

            set
            {
                _TemplateSrc = value;
            }
        }

        private String _DataSrc;
        /// <summary>
        /// 备用属性，暂时不用。
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("备用属性，暂时不用。")]
        public string DataSrc
        {
            get
            {
                return _DataSrc;
            }

            set
            {
                _DataSrc = value;
            }
        }

        private Boolean _EnablePagination = true;
        /// <summary>
        /// 是否开启分页功能
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue(true)]
        [Localizable(true)]
        [Description("是否开启分页功能")]
        public Boolean EnablePagination
        {
            get { return _EnablePagination; }
            set { _EnablePagination = value; }
        }

        private string _ParameterName = "_prms";
        /// <summary>
        /// 搜索中间变量名称 此变量为javascript中json类型。一般不需要修改。前台如果要对搜索中间变量进行修改，可以使用此变量。
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue("_prms")]
        [Localizable(true)]
        [Description("搜索中间变量名称 此变量为javascript中json类型。一般不需要修改。前台如果要对搜索中间变量进行修改，可以使用此变量。")]
        public string ParameterName
        {
            get { return _ParameterName; }
            set { _ParameterName = value; }
        }

        private int _PageSize = 50;
        /// <summary>
        /// 每页的记录数
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue(50)]
        [Localizable(true)]
        [Description("每页的记录数")]
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        private string _DownloadControlID;
        /// <summary>
        /// 下载按钮ID
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [Localizable(true)]
        [Description("下载按钮ID")]
        public string DownloadControlID
        {
            get { return _DownloadControlID; }
            set { _DownloadControlID = value; }
        }

        private string _DownloadALLControlID;
        [Bindable(true)]
        [Category("Template Settings")]
        [Localizable(true)]
        [Description("下载全部列按钮ID")]
        public string DownloadALLControlID
        {
            get { return _DownloadALLControlID; }
            set { _DownloadALLControlID = value; }
        }

        private bool _AutoLoadData = true;
        /// <summary>
        /// 页面第一次载入时，是否自动加载数据
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue(true)]
        [Localizable(true)]
        [Description("页面第一次载入时，是否自动加载数据")]
        public bool AutoLoadData
        {
            get { return _AutoLoadData; }
            set { _AutoLoadData = value; }
        }

        private bool _EnableColumnFilter = false;
        /// <summary>
        /// 针对表格模版，是否开启列筛选功能
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [DefaultValue(false)]
        [Localizable(true)]
        [Description("针对表格模版，是否开启列筛选功能")]
        public bool EnableColumnFilter
        {
            get { return _EnableColumnFilter; }
            set { _EnableColumnFilter = value; }
        }

        private string _SearchControlID;
        /// <summary>
        /// 搜索按钮ID
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [Localizable(true)]
        [Description("搜索按钮ID")]
        public string SearchControlID
        {
            get { return _SearchControlID; }
            set { _SearchControlID = value; }
        }


        private int _DefaultOrderIndex = 0;
        [Bindable(true)]
        [Category("Template Settings")]
        [Localizable(true)]
        [Description("默认排序索引")]
        public int DefaultOrderIndex
        {
            get { return _DefaultOrderIndex; }
            set { _DefaultOrderIndex = value; }
        }

        private OrderFieldType _OrderType = OrderFieldType.Desc;
        [Bindable(true)]
        [Category("Template Settings")]
        [Localizable(true)]
        [Description("默认排序方式：升序OR降序")]
        public OrderFieldType OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; }
        }


        private bool _DebugMode = false;
        /// <summary>
        /// 是否启用调试模式
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [Localizable(true)]
        [Description("是否启用调试模式")]
        public bool DebugMode
        {
            get { return _DebugMode; }
            set { _DebugMode = value; }
        }

        //hiddenCols
        private string _HiddenColunms;
        /// <summary>
        /// 默认需要隐藏的列，使用逗号分隔
        /// </summary>
        [Bindable(true)]
        [Category("Template Settings")]
        [Localizable(true)]
        [Description("默认需要隐藏的列，使用逗号分隔")]
        public string HiddenColunms
        {
            get { return _HiddenColunms; }
            set { _HiddenColunms = value; }
        }


        private PaginationTypes _PaginationType = PaginationTypes.Classic;

        [Bindable(true)]
        [Category("Template Settings")]
        [Localizable(true)]
        [Description("翻页类型")]
        public PaginationTypes PaginationType
        {
            get { return _PaginationType; }
            set { _PaginationType = value; }
        }
        #endregion

        private Dictionary<string, string> _PreloadParameters = new Dictionary<string, string>();
        /// <summary>
        /// 预先加载参数，此参数只能已编程方式赋值。
        /// </summary>
        public Dictionary<string, string> PreloadParameters
        {
            get { return _PreloadParameters; }
            set { _PreloadParameters = value; }
        }

        private string _PreloadParamters_JsonFormat;
        /// <summary>
        /// 预先加载参数，此参数只能已编程方式赋值。
        /// </summary>
        public string PreloadParamters_JsonFormat
        {
            get { return _PreloadParamters_JsonFormat; }
            set { _PreloadParamters_JsonFormat = value; }
        }

        /// <summary>
        /// 初始化搜索中间变量。
        /// </summary>
        /// <returns></returns>
        private string CreateInitScript()
        {
            StringBuilder sb = new StringBuilder();

            //输入额外定义的模版数组
            if (!string.IsNullOrEmpty(this.AlternateTemplateUrls))
            {
                string[] parts = this.AlternateTemplateUrls.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                sb.AppendFormat(" var {0}_templates=[", this.ID);
                sb.AppendFormat("\"{0}\"", WebMaster.GetStaticResourceUrl(TemplateSrc).Replace(".ascx", ".tt"));
                foreach (string s in parts)
                {
                    sb.AppendFormat(",\"{0}\"", WebMaster.GetStaticResourceUrl(s).Replace(".ascx", ".tt"));
                }
                sb.Append("];");
            }

            ParameterName = this.ID + "_" + ParameterName;

            sb.AppendFormat(" var obj_{0}; ", this.ID);
            sb.AppendFormat("var {0}={{}};", ParameterName);
            sb.AppendFormat("function __initprmsData_{0}() {{", this.ID);
            sb.AppendFormat(" obj_{0}= new smart();", this.ID);

#if DEBUG
            if (this.DebugMode)
            {
                sb.AppendFormat("obj_{0}.debug={1};", this.ID, this.DebugMode.ToString().ToLower());
            }
#endif
            sb.AppendFormat("obj_{0}.enableColunmFilter={1};", this.ID, this.EnableColumnFilter.ToString().ToLower());
            sb.AppendFormat("obj_{0}.useCache={1};", this.ID, this.UseRequestCache.ToString().ToLower());
            sb.AppendFormat("obj_{0}.paginationType={1};", this.ID, (int)this.PaginationType);
            if (this.HttpMethod == RequestMethod.Post)
            {
                sb.AppendFormat("obj_{0}.httpType={1};", this.ID, (int)this.HttpMethod);
            }

            if (!string.IsNullOrEmpty(AfterDataLoadedFunc))
            {
                sb.AppendFormat("obj_{0}.onDataLoaded={1};", this.ID, this.AfterDataLoadedFunc);
            }

            if (!string.IsNullOrEmpty(OnSearchBtnClickedFunc))
            {
                sb.AppendFormat("obj_{0}.onSearchButtonClicked={1};", this.ID, this.OnSearchBtnClickedFunc);
            }

            if (!string.IsNullOrEmpty(OnSearchBtnClickingFunc))
            {
                sb.AppendFormat("obj_{0}.onSearchButtonClicking={1};", this.ID, this.OnSearchBtnClickingFunc);
            }

            if (!string.IsNullOrEmpty(InitParameterFunc))
            {
                sb.AppendFormat("obj_{0}.initP={1};", this.ID, this.InitParameterFunc);
            }

            if (!string.IsNullOrEmpty(this.SearchCheckFuc))
            {
                sb.AppendFormat("obj_{0}.canSearch={1};", this.ID, this.SearchCheckFuc);
            }

            if (!string.IsNullOrEmpty(this.HandleRetDataFuc))
            {
                sb.AppendFormat("obj_{0}.handleRetData={1};", this.ID, this.HandleRetDataFuc);
            }

            if (!string.IsNullOrEmpty(this.HiddenColunms))
            {
                string[] parts = this.HiddenColunms.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    StringBuilder sbparts = new StringBuilder("[");
                    for (int i = 0; i < parts.Length; i++)
                    {
                        sbparts.AppendFormat("\"{0}\"", parts[i].ToLower());
                        if (i < parts.Length - 1)
                        {
                            sbparts.Append(",");
                        }
                    }
                    sbparts.Append("]");


                    sb.AppendFormat("obj_{0}._hiddenCols={1};", this.ID, sbparts.ToString());
                }
            }

            if (!string.IsNullOrEmpty(PreloadParamters_JsonFormat))
            {
                sb.AppendFormat("{0}={1};", ParameterName, PreloadParamters_JsonFormat);
            }

            sb.AppendFormat("{0}._orderindex={1};", ParameterName, this.DefaultOrderIndex);
            sb.AppendFormat("{0}._ordertype={1};", ParameterName, (int)this.OrderType);
            sb.AppendFormat("{0}.__mode = 1;", ParameterName);
            sb.AppendFormat("{0}._pageIndex = 0;", ParameterName);
            sb.AppendFormat("{0}._pageSize = {1};", ParameterName, PageSize);

            if (this._PreloadParameters.Count > 0)
            {
                foreach (KeyValuePair<string, string> p in this._PreloadParameters)
                {
                    sb.AppendFormat("{0}.{1} = '{2}';", ParameterName, p.Key, p.Value);
                }
            }

            sb.Append("}");
            return sb.ToString();
        }

        private string CreateLoadScript()
        {
            string templatepath = WebMaster.GetStaticResourceUrl(TemplateSrc).Replace(".ascx", ".tt"); // VirtualPathUtility.ToAbsolute(TemplateSrc).Replace(".ascx",".tt");
            StringBuilder sb = new StringBuilder();
            sb.Append("$(function () {");
            sb.AppendFormat(" __initprmsData_{0}(); obj_{0}.SetHolder(\"{0}\");", this.ID, this.ID);

            sb.AppendFormat("obj_{0}.setTemplate(\"{1}\");", this.ID, string.Format("{0}&__mode=0", templatepath));

            //构造smart搜索对象。
            //sb.Append("");
            sb.AppendFormat(" obj_{0}._dataUrl = \"{1}\";", this.ID, templatepath);
            sb.AppendFormat(" obj_{0}._prmsData = {1};", this.ID, ParameterName);


            if (AutoLoadData)
            {
                if (this._PreloadParameters.Count > 0 || !string.IsNullOrEmpty(this._PreloadParamters_JsonFormat))
                {
                    sb.AppendFormat(" obj_{0}.onSearchButtonClicked(obj_{0},true);obj_{0}.loadData(); ", this.ID);
                }
                else
                {
                    sb.AppendFormat(" obj_{0}.S(true); ", this.ID);
                }
            }

            if (!string.IsNullOrEmpty(DownloadControlID))
            {
                //初始化下载按钮点击事件
                //sb.AppendFormat(" $(\"#{0}\").click(function () ", DownloadControlID);
                //sb.Append("{ obj.download();  });");
                sb.AppendFormat("obj_{0}.setD(\"{1}\");", this.ID, DownloadControlID);
            }

            if (!string.IsNullOrEmpty(DownloadALLControlID))
            {
                //初始化下载按钮点击事件
                //sb.AppendFormat(" $(\"#{0}\").click(function () ", DownloadControlID);
                //sb.Append("{ obj.download();  });");
                sb.AppendFormat("obj_{0}.setDA(\"{1}\");", this.ID, DownloadALLControlID);
            }

            if (!string.IsNullOrEmpty(SearchControlID))
            {
                sb.AppendFormat("obj_{0}.setS(\"{1}\");", this.ID, SearchControlID);
            }

            sb.Append(" }) ");
            return sb.ToString();
        }

        public override void RenderControl(HtmlTextWriter writer)
        {

            if (this.DesignMode)
            {
                writer.Write(string.Format("<div style=\"background-color:#a4d955; padding:5px;\">Template:{0}</div>", this.TemplateSrc));
            }
            else
            {
                writer.WriteBeginTag("div");
                if (!string.IsNullOrEmpty(this.CssClass))
                {
                    writer.WriteAttribute("class", this.CssClass);
                }

                writer.WriteAttribute("id", this.ID);
                writer.Write(HtmlTextWriter.TagRightChar);
                if (this.AutoLoadData)
                {
                    writer.Write("<div class=\"dataloading\"><div>正在载入数据,请稍候...</div></div>");
                }
                writer.WriteEndTag("div");

                if (EnablePagination)
                {
                    writer.Write(string.Format("<div class=\"ph\"><div id=\"dp_{0}\" class=\"pagination\"></div></div>", this.ID));
                }

                writer.WriteBeginTag("script");
                writer.WriteAttribute("type", "text/javascript");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(CreateInitScript());
                writer.Write(CreateLoadScript());
                writer.WriteEndTag("script");
            }
        }
    }
}
