using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class JsonObj<T>
    {
        public static string ToJson(T obj)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings settings = new JsonSerializerSettings() {  NullValueHandling=NullValueHandling.Ignore};
            settings.Converters.Add(convert);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings);
            return json;
        }


        public static T FromJson(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                JsonSerializerSettings settings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
                settings.Converters.Add(convert);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(text, settings);
            }
            else
                return default(T);
        }

        public static object FromJson2(string text)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings settings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            settings.Converters.Add(convert);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(text, settings);
        }

        public static string ToJsonString(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

    }

    public class JsonObj
    {
        public static object FromJson(string text, Type t)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Newtonsoft.Json.JsonConvert.DeserializeObject(text, t, convert);
        }

        public static string ToJson(object o, Type t)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            
            settings.Converters.Add(convert);
            return Newtonsoft.Json.JsonConvert.SerializeObject(o, t, settings);
        }
    }

   
}
