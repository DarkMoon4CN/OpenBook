using Mars.Server.Controls.BaseControl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mars.Server.Controls
{
    /// <summary>
    /// 模版管理者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TemplateManager<T> : IDisposable where T : TemplateBase
    {
        private PageLite _p_holder;

        public T LoadTemplate(string path)
        {
            _p_holder = new PageLite();
            // string abs_path = HttpContext.Current.Server.MapPath(path);
            return _p_holder.LoadControl(path) as T;
        }

        public string RenderTemplate(T control)
        {
            StringWriter sw = new StringWriter();
            if (control != null)
            {
                this._p_holder.Controls.Add(control);
            }
            HttpContext.Current.Server.Execute(this._p_holder, sw, false);
            //StringBuilder sb = new StringBuilder("<p style=\"display: none\">");
            //sb.AppendFormat("<textarea id=\"{0}\" rows=\"0\" cols=\"0\"><!--", "mytemplate");
            //sb.Append(sw.ToString());
            //sb.Append("--></textarea></p>");
            return sw.ToString();
        }

        private bool IsDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool Diposing)
        {
            if (!IsDisposed)
            {
                if (Diposing)
                {              //清理托管资源
                    this._p_holder.Dispose();
                }
                //清理非托管资源 
            }
            IsDisposed = true;
        }
    }

    /// <summary>
    /// 自定义页面，充当Render模版的容器。
    /// </summary>
    public class PageLite : System.Web.UI.Page
    {
        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            //donothing...
        }
    }


    /// <summary>
    /// 模版属性负值管理者
    /// </summary>
    public class TemplatePropertyManager
    {
        public static void SetProperties(TemplateBase ctrl, HttpContext context)
        {
            if (ctrl == null)
            {
                return;
            }
            Dictionary<PropertyInfo, List<TemplatePropertyAttribute>> metadata = GetMetaData(ctrl.GetType());
            foreach (PropertyInfo property in metadata.Keys)
            {
                object value = GetValue(metadata[property], context) ?? GetDefaultValue(property);
                if (value != null)
                {
                    if (property.PropertyType == typeof(String))
                    {
                        property.SetValue(ctrl, HttpUtility.UrlDecode(Convert.ChangeType(value, property.PropertyType).ToString()), null);
                    }
                    else
                    {
                        property.SetValue(ctrl, Convert.ChangeType(value, property.PropertyType), null);
                    }
                }
            }
        }

        private static Dictionary<Type, Dictionary<PropertyInfo, List<TemplatePropertyAttribute>>> s_meta_cache =
            new Dictionary<Type, Dictionary<PropertyInfo, List<TemplatePropertyAttribute>>>();
        private static Dictionary<PropertyInfo, object> s_default_cache = new Dictionary<PropertyInfo, object>();
        private static object _lock = new object();

        private static Dictionary<PropertyInfo, List<TemplatePropertyAttribute>> GetMetaData(Type type)
        {
            if (!s_meta_cache.ContainsKey(type))
            {
                lock (_lock)
                {
                    if (!s_meta_cache.ContainsKey(type))
                    {
                        s_meta_cache[type] = LoadMetaData(type);
                    }
                }
            }
            return s_meta_cache[type];
        }

        private static Dictionary<PropertyInfo, List<TemplatePropertyAttribute>> LoadMetaData(Type type)
        {
            Dictionary<PropertyInfo, List<TemplatePropertyAttribute>> tar = new Dictionary<PropertyInfo, List<TemplatePropertyAttribute>>();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField);
            foreach (PropertyInfo p in properties)
            {
                TemplatePropertyAttribute[] attrs = p.GetCustomAttributes(typeof(TemplatePropertyAttribute), true) as TemplatePropertyAttribute[];
                if (attrs.Length > 0)
                {
                    tar[p] = new List<TemplatePropertyAttribute>(attrs);
                }
            }
            return tar;
        }

        private static object GetDefaultValue(PropertyInfo property)
        {
            if (!s_default_cache.ContainsKey(property))
            {
                lock (_lock)
                {
                    if (!s_default_cache.ContainsKey(property))
                    {
                        DefaultValueAttribute[] attr = property.GetCustomAttributes(typeof(DefaultValueAttribute), true) as DefaultValueAttribute[];
                        object value = attr.Length > 0 ? attr[0].Value : null;
                        s_default_cache[property] = value;
                    }
                }
            }
            return s_default_cache[property];
        }

        private static object GetValue(IEnumerable<TemplatePropertyAttribute> metadata, HttpContext context)
        {
            foreach (TemplatePropertyAttribute ua in metadata)
            {
                NameValueCollection nvc; //= (ua.Method == RequestMethod.Get) ? context.Request.QueryString : context.Request.Form;
                switch (ua.Method)
                {
                    case RequestMethod.Get:
                        nvc = context.Request.QueryString;
                        break;
                    case RequestMethod.Post:
                        nvc = context.Request.Form;
                        break;
                    default:
                        nvc = context.Request.Params;
                        break;
                }
                object value = nvc[ua.Key];
                if (value != null)
                    return value;
            }
            return null;
        }
    }
}
