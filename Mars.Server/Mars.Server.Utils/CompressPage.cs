using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Mars.Server.Utils
{
    /// <summary>
    /// a base class for all pages that want to compress the viewstate to inhert from
    /// created by benny
    /// </summary>
    public class CompressPage : System.Web.UI.Page
    {
        private int limiteLength = 1096;
        private static readonly Regex REGEX_BETWEEN_TAGS = new Regex(@">\s+<", RegexOptions.Compiled);
        private static readonly Regex REGEX_LINE_BREAKS = new Regex(@"(\n\s+)|(\r\s+)|(\t\s+)", RegexOptions.Compiled);
        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            #region
            System.IO.StringWriter sw = new StringWriter();
            LosFormatter lformat = new LosFormatter();
            lformat.Serialize(sw, viewState);

            string vString = sw.ToString();

            bool userzip = false;

            switch (VS_SaveMethod)
            {
                case ViewStateSaveMethod.Page:
                    {
                        if (vString.Length > limiteLength)
                        {
                            userzip = true;
                            byte[] mbytes = Compress(vString);
                            vString = System.Convert.ToBase64String(mbytes);
                        }
                        this.Page.ClientScript.RegisterHiddenField("__Benny_P", vString);
                        this.Page.ClientScript.RegisterHiddenField("__Benny_PZ", userzip.ToString().ToLower());
                    }
                    break;
                case ViewStateSaveMethod.Cache:
                    {
                        string key = Guid.NewGuid().ToString().Replace("-", "");
                        MyCache<string>.Insert(key, vString, HttpContext.Current.Session.Timeout * 60);
                        this.Page.ClientScript.RegisterHiddenField("__Benny_C", key);
                    }
                    break;
                case ViewStateSaveMethod.Session:
                    {
                        string key = Guid.NewGuid().ToString().Replace("-", "");
                        HttpContext.Current.Session[key] = vString;
                        this.Page.ClientScript.RegisterHiddenField("__Benny_S", key);
                    }
                    break;
            }

            #endregion
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            #region
            byte[] obytes = new byte[] { };

            switch (VS_SaveMethod)
            {
                case ViewStateSaveMethod.Page:
                    {
                        string zipvstate = this.Page.Request.Form.Get("__Benny_P");
                        string usezip_vstate = this.Page.Request.Form.Get("__Benny_PZ");
                        if (Equals(usezip_vstate, "true"))
                        {
                            obytes = DeCompress(zipvstate);
                        }
                        else
                        {
                            obytes = System.Convert.FromBase64String(zipvstate);
                        }
                    }
                    break;
                case ViewStateSaveMethod.Cache:
                    {
                        string key = this.Page.Request.Form.Get("__Benny_C");
                        try
                        {
                            obytes = System.Convert.FromBase64String(MyCache<string>.Get(key));
                        }
                        catch { obytes = new byte[] { }; }
                    }
                    break;
                case ViewStateSaveMethod.Session:
                    {
                        string key = this.Page.Request.Form.Get("__Benny_S");
                        try
                        {
                            obytes = System.Convert.FromBase64String(Session[key].ToString());
                        }
                        catch { obytes = new byte[] { }; }
                    }
                    break;
            }
            LosFormatter lformat = new LosFormatter();
            return lformat.Deserialize(System.Convert.ToBase64String(obytes));
            #endregion
        }

        private byte[] Compress(string viewStatebase)
        {
            #region
            byte[] pbytes = System.Convert.FromBase64String(viewStatebase);
            MemoryStream ms = new MemoryStream();

            GZipStream zipstream = new GZipStream(ms, CompressionMode.Compress);

            zipstream.Write(pbytes, 0, pbytes.Length);
            zipstream.Close();

            return ms.ToArray();
            #endregion
        }

        private byte[] DeCompress(string viewStateBase)
        {
            #region
            byte[] pbytes = System.Convert.FromBase64String(viewStateBase);
            int msize = 0;
            MemoryStream ms = new MemoryStream();
            ms.Write(pbytes, 0, pbytes.Length);
            ms.Position = 0;
            GZipStream zipstream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream temp = new MemoryStream();
            byte[] bufferbytes = new byte[4096];
            while (true)
            {
                msize = zipstream.Read(bufferbytes, 0, bufferbytes.Length);
                if (msize > 0)
                {
                    temp.Write(bufferbytes, 0, msize);
                }
                else
                {
                    break;
                }
            }
            zipstream.Close();
            return temp.ToArray();
            #endregion
        }

        /// <summary>
        /// whether or not trim the whitespace
        /// setting AppSettings  --key:TrimWhiteSpace
        /// </summary>
        private bool NeedTrimWhiteSpace
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["TrimWhiteSpace"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["TrimWhiteSpace"]);
            }
        }

        private ViewStateSaveMethod VS_SaveMethod
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ViewStateSaveMethod"]))
                {
                    return ViewStateSaveMethod.Page;
                }
                else
                {
                    object o = Enum.Parse(typeof(ViewStateSaveMethod), ConfigurationManager.AppSettings["ViewStateSaveMethod"], true);
                    return o == null ? ViewStateSaveMethod.Page : (ViewStateSaveMethod)o;
                }
            }
        }

        /// <summary>
        /// override the base render funtion to trim the whitespace
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (!NeedTrimWhiteSpace)
            {
                base.Render(writer);
            }
            else
            {
                using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
                {
                    base.Render(htmlwriter);
                    string html = htmlwriter.InnerWriter.ToString();

                    html = REGEX_BETWEEN_TAGS.Replace(html, "><");
                    html = REGEX_LINE_BREAKS.Replace(html, string.Empty);

                    writer.Write(html.Trim());
                }
            }
        }
    }

    public enum ViewStateSaveMethod
    {
        Page,
        Cache,
        Session
    }
}
