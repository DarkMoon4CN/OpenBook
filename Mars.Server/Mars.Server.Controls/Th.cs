using Mars.Server.Entity;
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
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Th runat=server></{0}:Th>")]
    public class Th : WebControl
    {
        private string _Text;
        [Bindable(true)]
        [Category("Th Settings")]
        [Description("表头显示名称")]
        public string Text
        {
            get { return string.Format("{0}{1}{2}", this.StartFixText, this._Text, this.EndFixText); }
            set { this._Text = value.ToUpper(); }
        }

        private string _EndFixText;
        [Bindable(true)]
        [Category("Th Settings")]
        [Description("表头显示名称后缀")]
        public string EndFixText
        {
            get { return _EndFixText; }
            set { _EndFixText = value; }
        }

        private string _StartFixText;
        [Bindable(true)]
        [Category("Th Settings")]
        [Description("表头显示名称前缀")]
        public string StartFixText
        {
            get { return _StartFixText; }
            set { _StartFixText = value; }
        }

        private int _OutPutOrder;
        [Bindable(true)]
        [Category("Th Settings")]
        [Description("导出时排序")]
        public int OutPutOrder
        {
            get { return _OutPutOrder; }
            set { _OutPutOrder = value; }
        }

        private string _BindingFiled;
        [Bindable(true)]
        [Category("Th Settings")]
        [Description("表头绑定字段名称")]
        public string BindingFiled
        {
            get { return _BindingFiled; }
            set { _BindingFiled = value; }
        }

        private bool _EnableAlink = false;
        [Bindable(true)]
        [Category("Th Settings")]
        [Description("表头绑定字段名称")]
        public bool EnableAlink
        {
            get { return _EnableAlink; }
            set { _EnableAlink = value; }
        }

        private string _CssStyle;
        [Bindable(true)]
        [Category("Th Settings")]
        [Description("扩展css样式")]
        public string CssStyle
        {
            get { return _CssStyle; }
            set { _CssStyle = value; }
        }

        private OrderByFileds _OrderIndex = OrderByFileds.Default;
        [Bindable(true)]
        [Category("Th Settings")]
        [Description("排序索引")]
        public OrderByFileds OrderIndex
        {
            get { return _OrderIndex; }
            set { _OrderIndex = value; }
        }


        private string _Parameters;
        [Bindable(true)]
        [Category("Th Settings")]
        [Description("自定义属性")]
        public string Parameters
        {
            get { return _Parameters; }
            set { _Parameters = value; }
        }


        private int _rowspan = 0;

        public int RowSpan
        {
            get { return _rowspan; }
            set { _rowspan = value; }
        }

        private int _colspan = 0;

        public int ColSpan
        {
            get { return _colspan; }
            set { _colspan = value; }
        }



        public override void RenderControl(HtmlTextWriter writer)
        {
            writer.WriteBeginTag("th");
            if (this.OrderIndex != OrderByFileds.Default)
            {
                writer.WriteAttribute("ox", ((int)this.OrderIndex).ToString());
            }

            if (this.RowSpan > 0)
            {
                writer.WriteAttribute("rowspan", this.RowSpan.ToString());
            }

            if (this.ColSpan > 0)
            {
                writer.WriteAttribute("colspan", this.ColSpan.ToString());
            }


            if (!string.IsNullOrEmpty(this.BindingFiled))
            {
                writer.WriteAttribute("idx", this.BindingFiled);
            }
            if (!string.IsNullOrEmpty(this.CssStyle))
            {
                writer.WriteAttribute("style", this.CssStyle);
            }

            if (!string.IsNullOrEmpty(this.CssClass.Trim()))
            {
                writer.WriteAttribute("class", this.CssClass);
            }

            if (!string.IsNullOrEmpty(Parameters))
            {
                string[] parts = this.Parameters.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sets in parts)
                {
                    string[] ps = sets.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (ps.Length == 2)
                    {
                        writer.WriteAttribute(ps[0], ps[1]);
                    }
                }
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            if (this.EnableAlink)
            {

                writer.WriteBeginTag("a");

                if (this.OrderIndex >= 0)
                {
                    writer.WriteAttribute("oc", "1");
                    //writer.WriteAttribute("class", "thName b");
                }
                //else
                //{
                //    writer.WriteAttribute("class", "thName");
                //}

                writer.WriteAttribute("href", "javascript:void(0)");



                writer.Write(HtmlTextWriter.TagRightChar);


                writer.Write(this._Text);

                //if (G._FiledsDic.ContainsKey(this.Text))
                //{
                //    writer.Write(G._FiledsDic[this.Text]);
                //}
                //else
                //{
                //    writer.Write(this.Text);
                //}
                writer.WriteEndTag("a");
            }
            else
            {
                writer.Write(this._Text);
            }
            writer.WriteEndTag("th");
        }
    }
}
