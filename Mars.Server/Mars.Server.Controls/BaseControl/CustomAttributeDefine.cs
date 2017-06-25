using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Controls.BaseControl
{
   public class CustomAttributeDefine
    {
    }

   /// <summary>
   /// 模版自定义属性
   /// </summary>
   [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
   public class TemplatePropertyAttribute : Attribute
   {
       public TemplatePropertyAttribute() { }
       public TemplatePropertyAttribute(string key, RequestMethod method) { _key = key; _method = method; }
       public TemplatePropertyAttribute(string key) : this(key, RequestMethod.Get) { }

       private string _key;

       public string Key
       {
           get { return _key; }
           set { _key = value; }
       }
       private RequestMethod _method;

       public RequestMethod Method
       {
           get { return _method; }
           set { _method = value; }
       }
   }

   /// <summary>
   /// HTTP请求参数提交方式
   /// </summary>
   public enum RequestMethod
   {
       Get,
       Post,
       Auto
   }
}
