using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.BLL
{
    public class SmsMananger
    {
        static CCPRestSDK.CCPRestSDK api;
        static string _SmsTemplateID;
        static int _ExpriedMin;
        static BCtrl_Auth authobj = new BCtrl_Auth();
        static SmsMananger()
        {
             api = new CCPRestSDK.CCPRestSDK();
             bool isInit = api.init(GetConfig("SmsServerIP"), GetConfig("SmsServerPort"));
             api.setAccount(GetConfig("SmsServerID"), GetConfig("SmsServerToken"));
             api.setAppId(GetConfig("SmsServerAppID"));
             _SmsTemplateID = GetConfig("SmsTemplateID");
             _ExpriedMin = int.Parse(GetConfig("SmsExpriedMin"));
             if (!isInit)
             {
                 throw new Exception("短信平台初始化失败");
             }
        }
        static string CodesString = "1234567890";
        public static bool SendCode(string phone,out string msg)
        {
            msg = "发送成功";
            Random r=new Random(DateTime.Now.Millisecond);
            StringBuilder code = new StringBuilder();
            for (int i = 0; i < 5; i++)
            { 
                code.Append(CodesString[r.Next(CodesString.Length)]);
            }
            string[] arrCont = { code.ToString(), _ExpriedMin.ToString() };

            if (authobj.AddSmsCodeToDB(phone, arrCont[0], _ExpriedMin))
            {
                Dictionary<string, object> retData = api.SendTemplateSMS(phone, _SmsTemplateID, arrCont);
                if (retData["statusCode"].ToString() == "000000")
                {
                    return true;
                }
                else
                {
                    msg = retData["statusMsg"].ToString();
                    return false;
                }
            }
            else
            {
                msg = "验证码写入数据库失败";
                return false;
            }

        }

        /// <summary>
        /// 发送返回发送状态
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SendContent(string phone, string modelKey,string username,string msg)
        {
            string[] arrCont = { username, msg };
            Dictionary<string, object> retData = api.SendTemplateSMS(phone, modelKey, arrCont);
            if (retData["statusCode"].ToString() == "000000")
            {
                return true;
            }
            else
            {
                msg = retData["statusMsg"].ToString();
                return false;
            }
        }

        static string GetConfig(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        public static bool BindPhone(string phone, string code, int userid,string pwd,out string msg)
        {
            return authobj.BindPhone(userid, phone, code,pwd,  out msg);
        }
    }
}
