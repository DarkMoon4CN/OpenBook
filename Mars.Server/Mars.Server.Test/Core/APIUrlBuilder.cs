using System;
using System.Collections.Generic;
using System.Text;

namespace Mars.Server.Test.Core
{
    public class APIUrlBuilder
    {
        public static string CreateUrl(string module,string method, string sessionid, string data, out Dictionary<string,string> dic,bool withtime=false)
        {   
            string tick=DateTime.Now.Ticks.ToString();
            dic = new Dictionary<string, string>();
            dic.Add("mars_appkey", G.AppKey);
            dic.Add("mars_sid", sessionid);
            dic.Add("mars_tick",tick );
            dic.Add("mars_version", G.Version);
            if (withtime)
            {
                dic.Add("mars_token", GetSign(System.Web.HttpUtility.UrlDecode(data,Encoding.UTF8), tick, G.AppKey, sessionid, G.Version, G.AppScrect));
            }
            else
            {
                dic.Add("mars_token", GetSign(data, tick, G.AppKey, sessionid, G.Version, G.AppScrect));
            }
            return string.Format("{0}/{1}/{2}",G.Server,module,method);
        }
            
        public static string CreateUrlWithGet(string module, string method, string sessionid, string data, out Dictionary<string, string> dic,bool withtime=false)
        {
            string tick = DateTime.Now.Ticks.ToString();
            dic = new Dictionary<string, string>();
            dic.Add("mars_appkey", G.AppKey);
            dic.Add("mars_sid", sessionid);
            dic.Add("mars_tick", tick);
            dic.Add("mars_version", G.Version);
            if (withtime)
            {
                dic.Add("mars_token", GetSign(System.Web.HttpUtility.UrlDecode(data, Encoding.UTF8), tick, G.AppKey, sessionid, G.Version, G.AppScrect));
            }
            else
            {
                dic.Add("mars_token", GetSign(data, tick, G.AppKey, sessionid, G.Version, G.AppScrect));
            }
           
            return string.Format("{0}/{1}/{2}?data={3}", G.Server, module, method, System.Web.HttpUtility.UrlEncode(data,Encoding.UTF8));
        }

        public static string fun_MD5(string str)
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        static string GetSign(string data, string tick, string appkey, string sesstionid, string version, string AppScrect)
        {
            try
            {
                string text1 = fun_MD5(string.IsNullOrEmpty(data) ? "" : data);
                string text2 = fun_MD5(string.Concat(new object[] {
                AppScrect,
                "&",text1,"&",tick,"&",version,"&",sesstionid,
            }));
                return text2;
            }
            catch
            {

                return string.Empty;
            }

        }

    }
}
