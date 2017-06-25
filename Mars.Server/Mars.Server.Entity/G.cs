using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Mars.Server.Entity
{
    public class G
    {
        public static string JSON_ERROR_STATE_STRING = "{{\"state\":0,\"msg\":\"{0}\"}}";
        public static string JSON_OK_STATE_STRING = "{{\"state\":1,\"msg\":\"{0}\"}}";
        public static string JSON_PMSERROR_STATE_STRING = "{{\"state\":-1,\"msg\":\"{0}\"}}";
        public static string JSON_OTHER_STATE_STRING = "{{\"state\":{0},\"msg\":\"{1}\"}}";
    }
}
