using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Mars.Server.Utils
{
   public class WebMaster
    {
        public static string webroot = System.Web.VirtualPathUtility.ToAbsolute("~/");
        //物理路径
        public static string localroot = HttpContext.Current.Server.MapPath("~/");

        public static string WebRoot = VirtualPathUtility.ToAbsolute("~/");

        private static Regex R_RemoveQuery = new Regex(@".*?(\?.*)?", RegexOptions.Compiled);
        public static string GetStaticResourceUrl(string rootRelativePath)
        {
            if (HttpRuntime.Cache[rootRelativePath] == null)
            {
                string abs_url = HostingEnvironment.MapPath(R_RemoveQuery.Replace(rootRelativePath, ""));
                string resourceUrl = VirtualPathUtility.ToAbsolute(rootRelativePath);
                if (System.IO.File.Exists(abs_url))
                {
                    DateTime dt = System.IO.File.GetLastWriteTime(abs_url);
                    string rt = string.Format("{0}{1}_ver={2}", resourceUrl, (resourceUrl.Contains("?") ? "&" : "?"), dt.Ticks);
                    HttpRuntime.Cache.Insert(rootRelativePath, rt, new System.Web.Caching.CacheDependency(abs_url));
                    return rt;
                }
                else
                {
                    return resourceUrl;
                }
            }
            else
                return HttpRuntime.Cache[rootRelativePath] as string;
        }

        #region 配置文件参数
        /// <summary>
       /// 获取Mars文章域名
       /// </summary>
        public static string ArticleDomain = System.Configuration.ConfigurationManager.AppSettings["ArticleDomain"];

        /// <summary>
        /// 获取Mars域名
        /// </summary>
        public static string Domain = System.Configuration.ConfigurationManager.AppSettings["Domain"];

        /// <summary>
        /// 获取图片服务器ID
        /// </summary>
        public static int ImageServerID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CurrentImageServerID"]);

       /// <summary>
       /// 文章轮播图片数量
       /// </summary>
        public static int ArticlePageAdvertNum = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageAdvertNum"]);

         /// <summary>
       /// 文章发现轮播图片数量
       /// </summary>
        public static int ArticlePageCarouselNum = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageCarouselNum"]);
       

       /// <summary>
        /// 上传书单文件大小
       /// </summary>
        public static string UploadBookListSize = System.Configuration.ConfigurationManager.AppSettings["UploadBookListSize"];

        
        /// <summary>
        /// 将原有图片转换成jpg 图片变小
        /// </summary>
        public static string ConvertJpg="?imageView2/0/interlace/1/format/jpg";


        public static string UserAnonymousHeader = System.Configuration.ConfigurationManager.AppSettings["UserAnonymousHeader"];
        public static string UserDefaultHeader = System.Configuration.ConfigurationManager.AppSettings["UserDefaultHeader"];
        #endregion

        /// <summary>
        /// HTTP页面状态码设置
        /// </summary>
        /// <param name="code"></param>
        public static void EndPage(int code=404)
        {
            HttpContext.Current.Response.StatusCode = code;
            HttpContext.Current.Response.End();
        }
    }
}
