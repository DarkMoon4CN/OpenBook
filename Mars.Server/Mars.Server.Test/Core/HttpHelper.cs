namespace Mars.Server.Test.Core
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Collections.Generic;
    using System.IO.Compression;
    using Mars.Server.Utils;
    public class HttpHelper
    {
        private CookieContainer _cc = new CookieContainer();
        private int _delayTime;
        private string _lastUrl = string.Empty;
        private WebProxy _proxy;
        private int _timeout = 60000;
        private int _tryTimes = 3;
        private string reqUserAgent = "Mars.Agent1.0";
        public string Upload(string url, string localfile, Dictionary<string, string> dic = null)
        {
            try
            {
                WebClient wc = new WebClient();
                if (dic != null)
                {
                    foreach (KeyValuePair<string, string> pair in dic)
                    {
                        wc.Headers[pair.Key] = pair.Value;
                    }
                }
                byte[] buffer = wc.UploadFile(url, localfile);
                return System.Text.Encoding.UTF8.GetString(buffer);
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return "{\"Status\":0,\"Msg\":\"网络异常! 无法连接远程服务器!\"}";
            }

        }
        private void DelaySomeTime()
        {
            if (this._delayTime > 0)
            {
                Random random = new Random();
                int millisecondsTimeout = (this._delayTime * 0x3e8) + random.Next(0x3e8);
                Thread.Sleep(millisecondsTimeout);
            }
        }

        public void Download(string url, string localfile)
        {
            new WebClient().DownloadFile(url, localfile);
        }

        public static string EncodePostData(string data)
        {
            return HttpUtility.UrlEncode(data);
        }

        public string Get(string url)
        {
            string str = this.Get(url, this._lastUrl);
            this._lastUrl = url;
            return str;
        }

        public string Get(string url, string referer)
        {
            return this.Get(url, referer, Encoding.Default);
        }


        public Stream GetImage(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.UserAgent = this.reqUserAgent;
            request.CookieContainer = this._cc;
            request.Method = "GET";
            request.Timeout = this._timeout;
            if ((this._proxy != null) && (this._proxy.Credentials != null))
            {
                request.UseDefaultCredentials = true;
            }
            
            request.Proxy = this._proxy;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.GetResponseStream();
        }

        public string Get(string url, string referer, Encoding encoding,Dictionary<string,string> dic=null)
        {
            int num = this._tryTimes;
            while (num-- > 0)
            {
                try
                {
                    this.DelaySomeTime();
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new Uri(url));
                    request.UserAgent = this.reqUserAgent;
                    request.CookieContainer = this._cc;
                    request.Referer = referer;
                    request.Method = "GET";
                    request.Timeout = this._timeout;
                    if ((this._proxy != null) && (this._proxy.Credentials != null))
                    {
                        request.UseDefaultCredentials = true;
                    }
                    request.Proxy = this._proxy;
                    request.Headers.Add("Accept-Encoding", "gzip");
                    if (dic != null)
                    {
                        foreach (KeyValuePair<string, string> pair in dic)
                        {
                            request.Headers[pair.Key] = pair.Value;
                        }
                    }

                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                    if (response.ContentEncoding == "gzip" && response.ContentType.Contains("application/json"))
                    {
                        var gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress, true);
                        var readerUnzipped = new StreamReader(gzip, encoding);
                        return readerUnzipped.ReadToEnd();
                    }
                    else
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream(), encoding);
                        return reader.ReadToEnd();
                    }
                    
                }
                catch (Exception exception)
                {
                    //LogUtil.WriteLog(string.Format("请求发生错误{0}:即将重试第{1}次,",exception.Message,num));
                    //throw new Exception(exception.Message);
                    LogUtil.WriteLog(exception);
                    //throw new Exception(exception.Message);
                }
            }
            LogUtil.WriteLog(string.Format("超过重试次数，请求资源:{0}失败.",url));
            return "{\"Status\":0,\"Msg\":\"网络异常! 无法连接远程服务器!\"}";
        }

        public string Post(string url, string content)
        {
            string str = this.Post(url, content, this._lastUrl);
            this._lastUrl = url;
            return str;
        }

        public string Post(string url, string content, string referer)
        {
            return this.Post(url, content, referer, Encoding.UTF8);
        }

        public string Post(string url, string content, string referer, Encoding encoding,Dictionary<string,string> dic=null)
        {
            int num = this._tryTimes;
            while (num-- > 0)
            {
                try
                {
                    this.DelaySomeTime();
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new Uri(url));
                    request.UserAgent = this.reqUserAgent;
                    request.CookieContainer = this._cc;
                    request.Referer = referer;
                    byte[] bytes = encoding.GetBytes(content);
                    request.Method = "POST";
                    request.Timeout = this._timeout;
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = bytes.Length;
                    if ((this._proxy != null) && (this._proxy.Credentials != null))
                    {
                        request.UseDefaultCredentials = true;
                    }
                    request.Proxy = this._proxy;
                    request.Headers.Add("Accept-Encoding", "gzip");
                    if (dic != null)
                    {
                        foreach (KeyValuePair<string, string> pair in dic)
                        {
                            request.Headers[pair.Key] = pair.Value;
                        }
                    }
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                    if (response.ContentEncoding == "gzip" && response.ContentType.Contains("application/json"))
                    {
                        var gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress, true);
                        var readerUnzipped = new StreamReader(gzip, encoding);
                        return readerUnzipped.ReadToEnd();
                    }
                    else
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream(), encoding);
                        return reader.ReadToEnd();
                    }
                }
                catch (Exception exception)
                {
                    LogUtil.WriteLog(exception);
                    //throw new Exception(exception.Message);
                    return "{\"Status\":0,\"Msg\":\"网络异常! 无法连接远程服务器!\"}";
                }
            }
            return string.Empty;
        }

        public void PostWithoutResponse(string url, string content)
        {
            this.PostWithoutResponse(url, content, this._lastUrl);
            this._lastUrl = url;
        }

        public void PostWithoutResponse(string url, string content, string referer)
        {
            int num = this._tryTimes;
            while (num-- > 0)
            {
                try
                {
                    this.DelaySomeTime();
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(new Uri(url));
                    request.UserAgent = this.reqUserAgent;
                    request.CookieContainer = this._cc;
                    request.Referer = "";
                    byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(content);
                    request.Method = "POST";
                    request.Timeout = this._timeout;
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = bytes.Length;
                    if ((this._proxy != null) && (this._proxy.Credentials != null))
                    {
                        request.UseDefaultCredentials = true;
                    }
                    request.Proxy = this._proxy;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                    break;
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
            }
        }

        public void SetDelayConnect(int delayTime)
        {
            this._delayTime = delayTime;
        }

        public void SetProxy(string server, int port, string username, string password)
        {
            if ((server != null) && (port > 0))
            {
                this._proxy = new WebProxy(server, port);
                if ((username != null) && (password != null))
                {
                    this._proxy.Credentials = new NetworkCredential(username, password);
                    this._proxy.BypassProxyOnLocal = true;
                }
            }
        }

        public void SetTimeOut(int timeout)
        {
            if (timeout > 0)
            {
                this._timeout = timeout;
            }
        }

        public void SetTryTimes(int times)
        {
            if (times > 0)
            {
                this._tryTimes = times;
            }
        }

        public bool CheckProxy(string server, int port)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri("http://www.baidu.com"));
                request.UserAgent = this.reqUserAgent;
                request.CookieContainer = this._cc;
                request.Method = "GET";
                request.Timeout = this._timeout/2;
                request.ReadWriteTimeout = _timeout/2;
                if ((this._proxy != null) && (this._proxy.Credentials != null))
                {
                    request.UseDefaultCredentials = true;
                }
                request.Proxy = this._proxy;
                request.GetResponse();
                request.Abort();
                return true;
            }
            catch {
                return false;
            }
        }

    }
}

