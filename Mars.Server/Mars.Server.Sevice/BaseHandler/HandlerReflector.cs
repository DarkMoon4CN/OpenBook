using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mars.Server.Sevice.BaseHandler
{
    public delegate string AjaxDelegate(HttpContext context);

    public class HandlerReflector
    {
        static HandlerReflector()
        {
            LoadController();
        }

        // 用于从类型查找Action的反射标记
        private static readonly BindingFlags ActionBindingFlags =
            BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;

        public static Dictionary<string, AjaxDelegate> _AjaxDelegates = new Dictionary<string, AjaxDelegate>();


        public static void LoadController()
        {
            _AjaxDelegates.Clear();
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass)
                {
                    object[] attr = type.GetCustomAttributes(typeof(AjaxControllerAttribute), false);
                    if (attr != null && attr.Length > 0)
                    {
                        MethodInfo[] mf = type.GetMethods(ActionBindingFlags);
                        foreach (MethodInfo m in mf)
                        {
                            object[] m_attrs = m.GetCustomAttributes(typeof(AjaxHandlerActionAttribute), false);
                            if (m_attrs != null && m_attrs.Length > 0)
                            {
                                try
                                {
                                    AjaxDelegate ad = (AjaxDelegate)Delegate.CreateDelegate(typeof(AjaxDelegate), null, m);

                                    string key = string.Format("{0}.{1}", type.Name, m.Name);
                                    _AjaxDelegates.Add(key, ad);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }

        public static string ExcuteHandler(HttpContext context, string methodkey)
        {
            if (_AjaxDelegates.ContainsKey(methodkey))
            {
                AjaxDelegate ad = _AjaxDelegates[methodkey];
                return ad(context);
            }
            else
            {
                return string.Empty;
            }
        }

    }    
}
