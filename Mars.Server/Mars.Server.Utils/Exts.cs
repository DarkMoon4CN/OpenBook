using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Utils
{
    public static class Exts
    {
        public static string ToSafeString(this string src)
        {
            return src.Replace("'","").Replace("%","").Replace("-","");
        }
    }
}
