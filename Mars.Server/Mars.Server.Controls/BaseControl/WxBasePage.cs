using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Runtime.Serialization.Json;
using System.Xml;

namespace Mars.Server.Controls.BaseControl
{
    public class WxBasePage : System.Web.UI.Page
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        private string _ts;
        protected string ts { get { return this._ts; } }
        /// <summary>
        /// 获取签名随机串
        /// </summary>
        private string _ns;
        protected string ns { get { return this._ns; } }
        /// <summary>
        /// 获取签名
        /// </summary>
        private string _sign;
        protected string sign { get { return this._sign; } }

        protected string title { get; set; }
        protected string desc { get; set; }
        protected string link { get; set; }
        protected string imgUrl { get; set; }


        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        public string CheckSignature(string urlPath="")
        {
            string tmpStr = "";
            string noncestr = "gemmy" + new Random().Next(1, 1000) + "W";
            _ns = noncestr.Trim();
            string jsapi_ticket = GetJsapi_ticket();
            string timestamp = ConvertDateTimeInt(DateTime.Now).ToString();
            _ts = timestamp.Trim();
            
            string url = string.IsNullOrEmpty(urlPath)? Request.Url.AbsoluteUri.ToString().Trim(): urlPath.Trim();
            //string url = WebMaster.Domain + urlPath;
            SortedList<string, string> SLString = new SortedList<string, string>();
            SLString.Add("noncestr", noncestr);
            SLString.Add("url", url);
            SLString.Add("timestamp", timestamp);
            SLString.Add("jsapi_ticket", jsapi_ticket);
            foreach (KeyValuePair<string, string> des in SLString)  //返回的是KeyValuePair，在学习的时候尽量少用var，起码要知道返回的是什么
            {
                tmpStr += des.Key + "=" + des.Value + "&";
            }
            if (tmpStr.Length > 0)
                tmpStr = tmpStr.Substring(0, tmpStr.Length - 1);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            _sign = tmpStr.ToLower().Trim();
            return tmpStr.ToLower().Trim();
        }

        /// <summary>  
        /// DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>Unix时间戳格式</returns>  
        public static int ConvertDateTimeInt(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 获取微信Token
        /// </summary>
        /// <returns></returns>
        public static string GetJsapi_ticket()
        {
            string ACCESS_TOKEN = IsExistAccess_Token();
            string Jsapi_str = HttpGet("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + ACCESS_TOKEN + "&type=jsapi");
            Jsapi_ticket obj = JsonObj<Jsapi_ticket>.FromJson(Jsapi_str);
            return obj.ticket;
        }

        /// <summary>
        /// http请求
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>0
        public static string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        /// <summary>
        /// 获取微信Token
        /// </summary>
        /// <returns></returns>
        //public static string GetToken()
        //{
        //    try
        //    {
        //        string getAccessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        //        string respText = "";
        //        //获取appid和appsercret
        //        string wechat_appid = "wx14b829fae5b27750";
        //        string wechat_appsecret = "b316ef7d7c1c4bd9dad8141e252f6da7";
        //        //获取josn数据
        //        string url = string.Format(getAccessTokenUrl, wechat_appid, wechat_appsecret);

        //        string key = WebKeys.AccessTokenCacheKey;
        //        AccessToken access = MyCache<AccessToken>.Get(key);
        //        if (access == null)
        //        {
        //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //            using (Stream resStream = response.GetResponseStream())
        //            {
        //                StreamReader reader = new StreamReader(resStream, Encoding.Default);
        //                respText = reader.ReadToEnd();
        //                resStream.Close();
        //            }
        //            JavaScriptSerializer Jss = new JavaScriptSerializer();
        //            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
        //            //通过键access_token获取值
        //            access = new AccessToken();
        //            access.accessToken = respDic["access_token"].ToString();
        //            access.lastGetTicketTime = DateTime.Now;
        //            MyCache<AccessToken>.Insert(key, access, 1400);
        //        }

        //        return access.accessToken;
        //        #region
        //        ////ticket 缓存7200秒
        //        //if (System.Web.HttpContext.Current.Session["jsapi_ticket"] == null)
        //        //{
        //        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        //    using (Stream resStream = response.GetResponseStream())
        //        //    {
        //        //        StreamReader reader = new StreamReader(resStream, Encoding.Default);
        //        //        respText = reader.ReadToEnd();
        //        //        resStream.Close();
        //        //    }
        //        //    JavaScriptSerializer Jss = new JavaScriptSerializer();
        //        //    Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
        //        //    //通过键access_token获取值
        //        //    accessToken = respDic["access_token"].ToString();
        //        //    System.Web.HttpContext.Current.Session["jsapi_ticket"] = accessToken;
        //        //    System.Web.HttpContext.Current.Session.Timeout = 7200;
        //        //    return accessToken;
        //        //}
        //        //else
        //        //{
        //        //    accessToken = System.Web.HttpContext.Current.Session["jsapi_ticket"].ToString();
        //        //    return accessToken;
        //        //}
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtil.WriteLog(ex);
        //        return null;
        //    }
        //}

        public static Access_token GetAccess_token()
        {
            string appid = System.Configuration.ConfigurationManager.AppSettings["weixinappid"];
            string secret = System.Configuration.ConfigurationManager.AppSettings["weixincode"];
            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;
            Access_token mode = new Access_token();

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

                string content = reader.ReadToEnd();
                //Response.Write(content);
                //在这里对Access_token 赋值
                Access_token token = new Access_token();
                token = JsonHelper.ParseFromJson<Access_token>(content);
                mode.access_token = token.access_token;
                mode.expires_in = token.expires_in;
            }
            return mode;
        }

        /// <summary>
        /// 根据当前日期 判断Access_Token 是否超期  如果超期返回新的Access_Token   否则返回之前的Access_Token
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string IsExistAccess_Token()
        {

            string Token = string.Empty;
            DateTime YouXRQ;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径
            
            string filepath = System.Web.HttpContext.Current.Server.MapPath("/XMLWXACCESS.xml");

            StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
            YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText);

            if (DateTime.Now > YouXRQ)
            {
                DateTime _youxrq = DateTime.Now;
                Access_token mode = GetAccess_token();
                if (mode.access_token != null && mode.expires_in != null)
                {
                    xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = mode.access_token;
                    _youxrq = _youxrq.AddSeconds(int.Parse(mode.expires_in));
                    xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText = _youxrq.ToString();
                    xml.Save(filepath);
                    Token = mode.access_token;
                }
            }
            return Token;
            //string _access_token = IsExistAccess_Token();
        }

        public class Jsapi_ticket
        {
            public string errcode { get; set; }
            public string errmsg { get; set; }
            public string ticket { get; set; }
            public string expires_in { get; set; }
        }

        public class AccessToken {
            /// <summary>
            /// api认证
            /// </summary>
            public string accessToken { get; set; }
            /// <summary>
            /// 最后获取ticket时间
            /// </summary>
            public DateTime lastGetTicketTime { get; set; }
        }
    }

    /// <summary>
    ///Access_token 的摘要说明
    /// </summary>
    public class Access_token
    {
        public Access_token()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        string _access_token;
        string _expires_in;

        /// <summary>
        /// 获取到的凭证 
        /// </summary>
        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public string expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
    }

    public class JsonHelper
    {
        /// <summary>
        /// 生成Json格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetJson<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
            }
        }
        /// <summary>
        /// 获取Json的Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="szJson"></param>
        /// <returns></returns>
        public static T ParseFromJson<T>(string szJson)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
