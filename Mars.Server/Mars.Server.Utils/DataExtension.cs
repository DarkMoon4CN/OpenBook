using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data
{
    public static class DataExtension
    {
        public static bool IsNotNULL(this object obj)
        {
            return DBNull.Value != obj;
        }

        public static short ToShort(this object obj)
        {
            if (obj.IsNotNULL())
            {
                return short.Parse(obj.ToString());
            }
            return 0;
        }
        public static int ToInt(this object obj)
        {
            if (obj.IsNotNULL())
            {
                return int.Parse(obj.ToString());
            }
            return 0;
        }
        public static DateTime ToDateTime(this object obj)
        {
            if (obj.IsNotNULL())
            {
                return DateTime.Parse(obj.ToString());
            }
            return DateTime.Parse("1999-01-01");
        }
        public static decimal ToDecimal(this object obj)
        {
            if (obj.IsNotNULL())
                return decimal.Parse(obj.ToString());
            return 0;
        }
    }
}