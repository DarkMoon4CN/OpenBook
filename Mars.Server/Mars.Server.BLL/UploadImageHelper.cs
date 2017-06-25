using Qiniu.Conf;
using Qiniu.FileOp;
using Qiniu.IO;
using Qiniu.RS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Utils;
using System.IO;
using Qiniu.RPC;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing;

namespace Mars.Server.BLL
{
    /// <summary>
    /// 图片管理类
    /// </summary>
    public class UploadImageHelper : IUploadImage
    {
        private static readonly IUploadImage upLoadBase = null;

        /// <summary>
        /// 七牛域名
        /// </summary>
        public string QiniuDomain
        {
            get
            {
                return upLoadBase.QiniuDomain;
            }
        }

        /// <summary>
        /// 根据七牛完整图片路径，转换为图片名
        /// </summary>
        /// <param name="qiniuImage"></param>
        /// <param name="isPrefix">是否带路径前缀 /</param>
        /// <returns></returns>
        public string ConvertImgkeyByUrl(string qiniuImage,bool isPrefix = false)
        {
            return upLoadBase.ConvertImgkeyByUrl(qiniuImage, isPrefix);
        }

        static UploadImageHelper()
        {
            upLoadBase = UploadImageHelperFactory.ExecuteUploadImageHelper();
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imgkey">七牛显示名称（保持唯一性）</param>
        /// <param name="imgPhysicalPath"></param>
        /// <returns></returns>
        public bool UpLoadImage(string imgkey, string imgPhysicalPath)
        {
            return upLoadBase.UpLoadImage(imgkey, imgPhysicalPath);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imgPhysicalPath"></param>
        /// <param name="imgkey">返回图片在七牛上的名称</param>
        /// <param name="isDelPhysicalPath">是否删除本地当前图片</param>
        /// <returns></returns>
        public bool UpLoadImage(string imgPhysicalPath, out string imgkey, bool isDelPhysicalPath = false)
        {
            return upLoadBase.UpLoadImage(imgPhysicalPath, out imgkey, isDelPhysicalPath);
        }

        private void DeleteLocalImage(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                try
                {
                    File.Delete(imagePath);
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                }
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="imgkey"></param>
        public bool DeleteImage(string imgkey)
        {
            return upLoadBase.DeleteImage(imgkey);
        }

        /// <summary>
        /// 根据七牛图片名称，生成水印图片
        /// </summary>
        /// <returns></returns>
        public bool GetImageWater(string imgkey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据七牛图片名称，生成水印图片
        /// </summary>
        /// <param name="imgkey"></param>
        /// <param name="dissolve"></param>
        /// <param name="enumMarkerGravity"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public string GetImageWater(string imgkey, int dissolve = 50, MarkerGravity enumMarkerGravity = MarkerGravity.Center, int dx = 10, int dy = 10)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imgkey"></param>
        /// <returns></returns>
        public string GetImageMogrify(string imgkey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imgkey"></param>
        /// <param name="thumbnail">如 !50x50r</param>
        /// <param name="gravity">如 center</param>
        /// <param name="crop">如 !50x50</param>
        /// <param name="quality">如 80</param>
        /// <param name="rotate">如 90</param>
        /// <param name="format"></param>
        /// <param name="autoOrient"></param>
        /// <returns></returns>
        public string GetImageMogrify(string imgkey, string thumbnail, string gravity, string crop, string quality, int rotate, string format, bool autoOrient)
        {
            throw new NotImplementedException();
        }

        #region 特有方法
        ///// <summary>
        ///// 图片处理
        ///// </summary>
        ///// <param name="MyFile"></param>
        ///// <param name="thumbnailPath"></param>
        ///// <param name="width"></param>
        ///// <param name="height"></param>
        ///// <param name="mode"></param>
        ///// <returns></returns>
        //public string MakeThumbnail(HttpPostedFile MyFile, string thumbnailPath, int width, int height, string mode)
        //{
        //    System.Drawing.Image originalImage = System.Drawing.Image.FromStream(MyFile.InputStream);

        //    int towidth = width;
        //    int toheight = height;

        //    int x = 0;
        //    int y = 0;
        //    int ow = originalImage.Width;
        //    int oh = originalImage.Height;

        //    switch (mode)
        //    {
        //        case "HW"://指定高宽缩放（可能变形）                
        //            break;
        //        case "W"://指定宽，高按比例                    
        //            toheight = originalImage.Height * width / originalImage.Width;
        //            break;
        //        case "H"://指定高，宽按比例
        //            towidth = originalImage.Width * height / originalImage.Height;
        //            break;
        //        case "Cut"://指定高宽裁减（不变形）                
        //            if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
        //            {
        //                oh = originalImage.Height;
        //                ow = originalImage.Height * towidth / toheight;
        //                y = 0;
        //                x = (originalImage.Width - ow) / 2;
        //            }
        //            else
        //            {
        //                ow = originalImage.Width;
        //                oh = originalImage.Width * height / towidth;
        //                x = 0;
        //                y = (originalImage.Height - oh) / 2;
        //            }
        //            break;
        //        default:
        //            break;
        //    }

        //    //新建一个bmp图片
        //    Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

        //    //新建一个画板
        //    Graphics g = System.Drawing.Graphics.FromImage(bitmap);

        //    //设置高质量插值法
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

        //    //设置高质量,低速度呈现平滑程度
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //    //清空画布并以透明背景色填充
        //    g.Clear(Color.Transparent);

        //    //在指定位置并且按指定大小绘制原图片的指定部分
        //    g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
        //        new Rectangle(x, y, ow, oh),
        //        GraphicsUnit.Pixel);


        //    string strGuid = System.Guid.NewGuid().ToString().ToUpper();
        //    thumbnailPath += strGuid + ".jpg";

        //    try
        //    {
        //        //以jpg格式保存缩略图
        //        bitmap.Save(HttpContext.Current.Server.MapPath(thumbnailPath), System.Drawing.Imaging.ImageFormat.Jpeg);//System.Drawing.Imaging.ImageFormat.Jpeg

        //    }
        //    catch (System.Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        originalImage.Dispose();
        //        bitmap.Dispose();
        //        g.Dispose();
        //    }
        //    return thumbnailPath;
        //}
        #endregion
    }

    /// <summary>
    /// 七牛图片上传帮助类
    /// </summary>
    internal class QiniuHelper : QiniuBase, IUploadImage
    {
        /// <summary>
        /// 七牛域名
        /// </summary>
        public string QiniuDomain
        {
            get
            {
                //string domain = QiniuBase.Domain.Trim().EndsWith("/") || QiniuBase.Domain.Trim().EndsWith(@"\") ? QiniuBase.Domain : QiniuBase.Domain + "/";
                return QiniuBase.Domain.Trim();
            }
        }

        /// <summary>
        /// 根据七牛完整图片路径，转换为图片名
        /// </summary>
        /// <param name="qiniuImage"></param>
        /// <param name="isPrefix">是否带前缀路径/</param>
        /// <returns></returns>
        public string ConvertImgkeyByUrl(string qiniuImage, bool isPrefix)
        {
            string regUrl = string.Empty;
            if (isPrefix)
            {
                regUrl = "^" + this.QiniuDomain + "(.*?)$";
            }
            else
            {
                regUrl = "^" + this.QiniuDomain + "/(.*?)$";
            }

            Regex regex = new Regex(regUrl, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match match = regex.Match(qiniuImage);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imgkey">七牛显示名称（保持唯一性）</param>
        /// <param name="imgPhysicalPath"></param>
        /// <returns></returns>
        public bool UpLoadImage(string imgkey, string imgPhysicalPath)
        {
            try
            {
                var policy = new PutPolicy(Bucket, 3600);
                string upToken = policy.Token();
                PutExtra extra = new PutExtra();
                extra.MimeType = "text/plain";
                extra.Crc32 = 123;
                extra.CheckCrc = CheckCrcType.CHECK;
                extra.Params = new System.Collections.Generic.Dictionary<string, string>();

                IOClient client = new IOClient();

                PutRet ret = client.PutFile(upToken, imgkey, imgPhysicalPath, extra);
                //client.PutFinished += new EventHandler<PutRet>((o,e)=>{               
                //});

                return ret.OK;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imgPhysicalPath"></param>
        /// <param name="imgkey">返回图片在七牛上的名称</param>
        /// <param name="isDelPhysicalPath">是否删除本地当前图片</param>
        /// <returns></returns>
        public bool UpLoadImage(string imgPhysicalPath, out string imgkey, bool isDelPhysicalPath = false)
        {
            imgkey = string.Empty;
            try
            {
                imgkey = GenerateQiniuKey;
                var policy = new PutPolicy(Bucket, 3600);
                string upToken = policy.Token();
                PutExtra extra = new PutExtra();
                extra.MimeType = "text/plain";
                extra.Crc32 = 123;
                extra.CheckCrc = CheckCrcType.CHECK;
                extra.Params = new System.Collections.Generic.Dictionary<string, string>();

                IOClient client = new IOClient();              

                client.PutFinished += new EventHandler<PutRet>((o, e) =>
                {
                    if (isDelPhysicalPath)
                    {
                        DeleteLocalImage(imgPhysicalPath);
                    }
                });
                PutRet ret = client.PutFile(upToken, imgkey, imgPhysicalPath, extra);
                imgkey = "/" + imgkey;

                return ret.OK;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        private void DeleteLocalImage(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                try
                {
                    File.Delete(imagePath);
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                }
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="imgkey"></param>
        public bool DeleteImage(string imgkey)
        {
            try
            {
                RSClient client = new RSClient();
                CallRet ret = client.Delete(new EntryPath(Bucket, imgkey));

                return ret.OK;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 根据七牛图片名称，生成水印图片
        /// </summary>
        /// <returns></returns>
        public bool GetImageWater(string imgkey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据七牛图片名称，生成水印图片
        /// </summary>
        /// <param name="imgkey"></param>
        /// <param name="dissolve"></param>
        /// <param name="enumMarkerGravity"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public string GetImageWater(string imgkey, int dissolve = 50, MarkerGravity enumMarkerGravity = MarkerGravity.Center, int dx = 10, int dy = 10)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imgkey"></param>
        /// <returns></returns>
        public string GetImageMogrify(string imgkey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imgkey"></param>
        /// <param name="thumbnail">如 !50x50r</param>
        /// <param name="gravity">如 center</param>
        /// <param name="crop">如 !50x50</param>
        /// <param name="quality">如 80</param>
        /// <param name="rotate">如 90</param>
        /// <param name="format"></param>
        /// <param name="autoOrient"></param>
        /// <returns></returns>
        public string GetImageMogrify(string imgkey, string thumbnail, string gravity, string crop, string quality, int rotate, string format, bool autoOrient)
        {
            throw new NotImplementedException();
        }
    }

    internal class UploadImageHelperFactory
    {
        public static IUploadImage ExecuteUploadImageHelper()
        {
            return new QiniuHelper();
        }
    }

    /// <summary>
    /// 七牛基类
    /// </summary>
    internal class QiniuBase
    {

        /// <summary>
        /// 七牛空间名称
        /// </summary>
        protected static readonly string Bucket;

        /// <summary>
        /// 空间名称域名
        /// </summary>
        protected static readonly string Domain;

        /// <summary>
        /// 存储在七牛的文件名
        /// </summary>
        protected string GenerateQiniuKey
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd") + "/" + Guid.NewGuid().ToString() + ".jpg";
            }
        }

        static QiniuBase()
        {
            Config.ACCESS_KEY = System.Configuration.ConfigurationManager.AppSettings["ACCESS_KEY"];
            Config.SECRET_KEY = System.Configuration.ConfigurationManager.AppSettings["SECRET_KEY"];
            Bucket = System.Configuration.ConfigurationManager.AppSettings["Bucket"];
            int serverID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CurrentImageServerID"]);
            Domain = new BCtrl_PictureServer().QueryPicServer(serverID).Domain;
        }

        public QiniuBase()
        {
        }
    }

    internal interface IUploadImage
    {
        /// <summary>
        /// 当前所用的七年域名（所建空间所对应的域名）
        /// </summary>
        string QiniuDomain { get; }

        /// <summary>
        /// 根据七牛完整图片路径，转换为图片名
        /// </summary>
        /// <param name="qiniuImage"></param>
        /// <param name="isPrefix">是否带路径前缀 /</param>
        /// <returns></returns>
        string ConvertImgkeyByUrl(string qiniuImage, bool isPrefix);

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imgkey">七牛显示名称（保持唯一性）</param>
        /// <param name="imgPhysicalPath"></param>
        /// <returns></returns>
        bool UpLoadImage(string imgkey, string imgPhysicalPath);

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imgPhysicalPath"></param>
        /// <param name="imgkey">返回图片在七牛上的名称</param>
        /// <returns></returns>
        bool UpLoadImage(string imgPhysicalPath, out string imgkey, bool isDelPhysicalPath);

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="imgkey"></param>
        bool DeleteImage(string imgkey);

        /// <summary>
        /// 根据七牛图片名称，生成水印图片
        /// </summary>
        /// <returns></returns>
        bool GetImageWater(string imgkey);

        /// <summary>
        /// 根据七牛图片名称，生成水印图片
        /// </summary>
        /// <param name="imgkey"></param>
        /// <param name="dissolve"></param>
        /// <param name="enumMarkerGravity"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        string GetImageWater(string imgkey, int dissolve, MarkerGravity enumMarkerGravity, int dx, int dy);

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imgkey"></param>
        /// <returns></returns>
        string GetImageMogrify(string imgkey);

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imgkey"></param>
        /// <param name="thumbnail">如 !50x50r</param>
        /// <param name="gravity">如 center</param>
        /// <param name="crop">如 !50x50</param>
        /// <param name="quality">如 80</param>
        /// <param name="rotate">如 90</param>
        /// <param name="format"></param>
        /// <param name="autoOrient"></param>
        /// <returns></returns>
        string GetImageMogrify(string imgkey, string thumbnail, string gravity, string crop, string quality, int rotate, string format, bool autoOrient);
    }
}
