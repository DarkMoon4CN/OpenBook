
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mars.Server.Test.Core
{
    public class G
    {
        public static void Init(string appkey, string appScrect)
        {
            AppKey = appkey;
            AppScrect = appScrect;
        }
        public static string Version = "1.0";
        public static string AppKey = "Mars.Mobile";
        public static string AppScrect = "41578020073147cdb707f99dbb6eeb46";
        public static string Server = System.Configuration.ConfigurationManager.AppSettings["SparrowF.Server"];
        public static UserSessionEntity CurrentSession{get;set;}
        
    }
}
