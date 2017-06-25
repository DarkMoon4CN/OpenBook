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
    [DefaultProperty("Src")]
    [ToolboxData("<{0}:OBScript runat=server></{0}:OBScript>")]
    public class OBScript : WebControl
    {
        private String _Src;
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Src
        {
            get
            {
                return _Src;
            }

            set
            {
                _Src = value;
            }
        }

        private bool _MinFileMode = false;
        [DefaultValue(false)]
        [Category("Appearance")]
        [Bindable(true)]
        [Localizable(true)]
        public bool MinFileMode
        {
            get { return _MinFileMode; }
            set { _MinFileMode = value; }
        }

        private ScriptTypes _ScriptType;
        [DefaultValue(ScriptTypes.Javascript)]
        [Category("Appearance")]
        [Bindable(true)]
        [Localizable(true)]
        public ScriptTypes ScriptType
        {
            get { return _ScriptType; }
            set { _ScriptType = value; }
        }
        //private string GetCachedUrl(string rootRelativePath)
        //{
        //    if (HttpRuntime.Cache[rootRelativePath] == null)    
        //    {
        //        string abs_url = HostingEnvironment.MapPath(rootRelativePath);
        //        string resourceUrl = VirtualPathUtility.ToAbsolute(rootRelativePath);
        //        DateTime dt= System.IO.File.GetLastWriteTime(abs_url);
        //        string rt=string.Format("{0}{1}_ver={2}",resourceUrl,(resourceUrl.Contains("?") ? "&":"?"),dt.Ticks);
        //        HttpRuntime.Cache.Insert(rootRelativePath, rt, new System.Web.Caching.CacheDependency(abs_url));
        //        return rt;
        //    }
        //    else
        //        return HttpRuntime.Cache[rootRelativePath] as string;
        //}
        protected override void Render(HtmlTextWriter writer)
        {

            if (this.DesignMode)
            {
                base.Render(writer);
                return;
            }
            else
            {

                string resourceUrl = WebMaster.GetStaticResourceUrl(
                        this.MinFileMode ?
                        string.Format("{0}.min.{1}", _Src.Replace((this.ScriptType == ScriptTypes.Javascript ? ".js" : ".css"), ""), (this.ScriptType == ScriptTypes.Javascript ? "js" : "css"))
                            : _Src);
                //GetCachedUrl(_Src);
                //if (resourceUrl.Contains("?"))
                //{
                //    resourceUrl = string.Format("{0}&_ver_={1}", resourceUrl, G.AppVersion);
                //}
                //else
                //{
                //    resourceUrl = string.Format("{0}?_ver_={1}", resourceUrl, G.AppVersion);
                //}

                if (this._ScriptType == ScriptTypes.Javascript)
                {
                    writer.WriteBeginTag("script");
                    writer.WriteAttribute("type", "text/javascript");
                    if (!string.IsNullOrEmpty(Src))
                    {
                        writer.WriteAttribute("src", resourceUrl);
                    }
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteEndTag("script");
                }
                else
                {
                    writer.WriteBeginTag("link");
                    writer.WriteAttribute("type", "text/css");
                    writer.WriteAttribute("rel", "Stylesheet");
                    if (!string.IsNullOrEmpty(Src))
                    {
                        writer.WriteAttribute("href", resourceUrl);
                    }
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteEndTag("link");
                }
            }
        }
    }

    public enum ScriptTypes
    {
        Javascript,
        StyleCss
    }
}
