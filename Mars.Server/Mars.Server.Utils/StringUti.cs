using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mars.Server.Utils
{
    /// <summary>
    /// 常用字符串操作
    /// 编写时间：2006年9月27日   
    /// </summary>
    public class StringUti
    {

        public static string Divide(string s, string total, bool ispercent)
        {
            if (string.IsNullOrEmpty(total))
            {
                return "--";
            }
            else
            {
                int t = total.ToInt();
                return t > 0 ? (s.ToInt() * 1d / t).ToString((ispercent ? "p" : "f2")) : "--";
            }
        }



        ///<summary>
        ///截取对应长度的字符串
        ///</summary>
        ///<param name="str">要截取的字符串</param>
        ///<param name="len">截取的长度</param>
        public static string GetSubString(string str, int len)
        {
            #region
            return len < str.Length ? str.Substring(0, len) : str;
            #endregion
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="c">截取多少位</param>
        public static string SubStr(string s, int c)
        {
            if (string.IsNullOrWhiteSpace(s))
                return string.Empty;
            if (c < s.Length)
                return s.Substring(0, c) + "...";
            return s;
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetSubString(string obj, int len, string word)
        {
            if (!String.IsNullOrEmpty(obj))
                return obj.Length >= len ? obj.Substring(0, len - 2) + word : obj;
            else
                return "-";
        }

        ///<summary>
        ///简单清理输入字符串
        ///</summary>
        ///<param name="str">需要清理的字符串</param>
        ///<param name="maxlength">允许的字符串的最大长度</param>
        public static string CleanString(string str, int maxlength)
        {
            #region
            StringBuilder tempstr = new StringBuilder();
            if (str != null || str != string.Empty)
            {
                str = str.Trim();
                //str = (str.Length > maxlength ? str.Substring(0, maxlength) : str);
                if (str.Length > maxlength)
                    str = str.Substring(0, maxlength);
                for (int i = 0; i < str.Length; i++)
                {
                    switch (str[i])
                    {
                        case '"':
                            tempstr.Append("&quot;");
                            break;
                        case '<':
                            tempstr.Append("&lt;");
                            break;
                        case '>':
                            tempstr.Append("&gt;");
                            break;
                        default:
                            tempstr.Append(str[i]);
                            break;
                    }
                }
                tempstr.Replace("'", "''").Replace("&nbsp;", " ").Replace("%", "%").Replace(",", "，").Replace("script", " ").Replace(".js", " ").Replace("\n", "<br>").Replace("-", " ");
            }
            return tempstr.ToString();
            #endregion
        }
        public static string CleanString(string str)
        {
            return str.Replace("'", "''").Replace("%", "%").Replace(",", "，").Replace("script", " ").Replace(".js", " ");
        }
        public static string SuperCleanStr(string str)
        {
            #region
            str = str.Trim();
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            str = Regex.Replace(str, "[\\s]{2,}", " "); //替换多余空格
            str = Regex.Replace(str, "(<[b|B][r|R]/*>)+", "\n"); //代替 <br/>
            str = Regex.Replace(str, "(\\s*&[N|n][B|b][S|s][P|p];\\s*)+", " ");//代替&nbsp;
            str = Regex.Replace(str, "<(.|\\n)*?>", string.Empty);
            str = Regex.Replace(str, "</[.\\n]*?>", string.Empty);
            str = str.Replace("'", "''");

            return str;
            #endregion
        }
        ///<summary>
        ///安全清理输入的字符串
        ///</summary>
        ///<param name="str">需要清理的字符串</param>
        ///<param name="len">限定字符串长度</param>
        public static string SuperCleanStr(string str, int len)
        {
            #region
            str = str.Trim();
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            if (str.Length > len)
                str = str.Substring(0, len);
            str = Regex.Replace(str, "[\\s]{2,}", " "); //替换多余空格
            str = Regex.Replace(str, "(<[b|B][r|R]/*>)+", "\n"); //代替 <br/>
            str = Regex.Replace(str, "(\\s*&[N|n][B|b][S|s][P|p];\\s*)+", " ");//代替&nbsp;
            str = Regex.Replace(str, "<(.|\\n)*?>", string.Empty);
            str = Regex.Replace(str, "</[.\\n]*?>", string.Empty);
            str = str.Replace("'", "''");

            return str;
            #endregion
        }
        public static string TextToHtml(string txtStr)
        {
            return txtStr.Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r", "").Replace("\n", "<br />");
        }

        public static string GetSubStrWithoutHtml(string src, int len)
        {
            string str = RemoveHtml(src.Replace("&nbsp;", " "));
            return len < str.Length ? str.Substring(0, len) + "..." : str;
        }

        public static string RemoveHtml(string src)
        {
            string regimg = @"<img\s+[^>]*\s*src\s*=\s*([']?)(?<url>\S+)'?[^>]*>";
            string temp = Regex.Replace(src, regimg, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            string regexstr = @"<[^>]*>";
            return Regex.Replace(temp, regexstr, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// 去除字符串中的html代码
        /// </summary>
        /// <param name="strhtml"></param>
        /// <returns></returns>
        public static string StripHtml(string strhtml)
        {
            string stroutput = strhtml;
            Regex regex = new Regex(@"<[^>]+>|</[^>]+>");
            stroutput = regex.Replace(stroutput, "");
            return stroutput;
        }

        private static string REGEX_ISNUM = @"^\d+$";
        public static bool IsNum(string s)
        {
            return new Regex(REGEX_ISNUM).IsMatch(s);
        }
        public static int GetInt(string str)
        {
            int i = 0;
            if (!int.TryParse(str, out i))
                return -1;
            else
                return i;
        }
        //ConvertCHToEN
        //http://www.cnblogs.com/sxally/archive/2008/12/11/1352827.html


        public static Dictionary<string, string> SplitToDictionary(string str)
        {
            try
            {
                string[] strs = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length > 0)
                {
                    Dictionary<string, string> _t = new Dictionary<string, string>();
                    foreach (string s in strs)
                    {
                        string[] _ss = s.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        if (_ss.Length == 2)
                        {
                            _t.Add(_ss[0], _ss[1]);
                        }
                    }
                    return _t;
                }
                else
                    return new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new Dictionary<string, string>();
            }
        }
        public static string CombineFromDictionary(Dictionary<string, string> tar)
        {
            if (tar == null)
                return "";
            StringBuilder sbstr = new StringBuilder();
            foreach (KeyValuePair<string, string> v in tar)
            {
                sbstr.Append(v.Key + "_" + v.Value + ",");
            }
            return sbstr.ToString();
        }

        public static string DoubleToPercent(string doublestr)
        {
            double d = 0.0;
            if (double.TryParse(doublestr, out d))
            {
                return d.ToString("p");
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 截取double类型小数点后cnt位
        /// </summary>
        /// <param name="doublestr"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public static string TrimDouble(string doublestr, int cnt)
        {
            double d = 0.0;
            if (double.TryParse(doublestr, out d))
            {
                return string.Format("{0:N" + cnt.ToString() + "}", d);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Encode(string src)
        {
            try
            {
                string encoded = Convert.ToBase64String(Encoding.GetEncoding("gb2312").GetBytes(src));
                encoded = encoded.Replace("/", "_").Replace("+", "-");
                return encoded;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return src;
            }
        }
        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Decode(string src)
        {
            try
            {
                string value = src.Replace("_", "/").Replace("-", "+");
                byte[] buffer = Convert.FromBase64String(value);
                return Encoding.GetEncoding("gb2312").GetString(buffer);
            }
            catch
            {
                //LogUtil.WriteLog(ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 将字符床进行unicode编码
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string ToUnicode(string sourceString)
        {
            StringBuilder sb = new StringBuilder();
            char[] src = sourceString.ToCharArray();
            for (int i = 0; i < src.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
                if (bytes.Length > 1 && bytes[1] > 0)
                {
                    sb.Append(@"\u");
                    sb.Append(bytes[1].ToString("x2"));
                    sb.Append(bytes[0].ToString("x2"));
                }
                else
                {
                    sb.Append(src[i].ToString());
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="Length">字符串长度</param>
        /// <param name="Seed">随机函数种子值</param>
        /// <returns>指定长度的随机字符串</returns>
        public static string RndString(int Length, params int[] Seed)
        {
            string strSep = ",";
            char[] chrSep = strSep.ToCharArray();

            //这里定义字符集
            string strChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            // + ",A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";

            string[] aryChar = strChar.Split(chrSep, strChar.Length);

            string strRandom = string.Empty;
            Random Rnd;
            if (Seed != null && Seed.Length > 0)
            {
                Rnd = new Random(Seed[0]);
            }
            else
            {
                Rnd = new Random();
            }
            //生成随机字符串
            for (int i = 0; i < Length; i++)
            {
                strRandom += aryChar[Rnd.Next(aryChar.Length)];
            }
            return strRandom;
        }

        public static List<int> SplitToListINT(string src)
        {
            string[] parts = src.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> data = new List<int>();
            foreach (string s in parts)
            {
                data.Add(s.ToInt());
            }
            return data;
        }

        public static string CombineListINT(List<int> src)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int s in src)
            {
                sb.AppendFormat("{0}|", s);
            }
            if (!String.IsNullOrEmpty(sb.ToString()))
                return sb.ToString().TrimEnd(new char[] { '|' });
            else
                return
                    sb.ToString();
        }
    }

    public static class StringEx
    {
        public static string ToUnicodeString(this string src)
        {
            return StringUti.ToUnicode(src);
        }

        public static string ToJSON_Error_String(this string msg)
        {
            return string.Format("{{\"state\":0,\"msg\":\"{0}\"}}", msg).ToUnicodeString();
        }

        public static string ToJSON_OK_String(this string msg)
        {
            return string.Format("{{\"state\":1,\"msg\":\"{0}\"}}", msg).ToUnicodeString();
        }

        public static string FormatString(this string msg, params object[] values)
        {
            return string.Format(msg, values);
        }

        public static int ToInt(this string src)
        {
            try
            {
                if (string.IsNullOrEmpty(src))
                    return 0;
                return int.Parse(src);
            }
            catch
            {
                return 0;
            }
        }

        public static string UrlDecode(this string src)
        {
            if (!string.IsNullOrEmpty(src))
            {
                return System.Web.HttpUtility.UrlDecode(src);
            }
            return string.Empty;
        }

        public static List<int> ToIntList(this string[] src)
        {
            List<int> evt = new List<int>();
            foreach (string s in src)
            {
                evt.Add(s.ToInt());
            }
            return evt;
        }

        public static DateTime ToDateTime(this string src)
        {
            try
            {
                return DateTime.Parse(src);
            }
            catch { return DateTime.MinValue; }
        }

        public static string JoinBy<T>(this List<T> src, string spliter)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < src.Count; i++)
            {
                sb.Append(src[i].ToString());
                if (i < src.Count - 1)
                {
                    sb.Append(spliter);
                }
            }
            return sb.ToString();
        }
    }

  
}
