using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class SMSEntity
    {
        public int SmsID { get; set; }
        public string Moblie { get; set; }
        public string Customer { get; set; }
        public int IsSend { get; set; }
        public string Content { get; set; }
        public DateTime? SendTime { get; set; }
        public int SysUserID { get; set; }
        public string ModelKey { get; set; }
    }

    public class SMSModeEntity
    {

        /// <summary>
        /// 短信模板ID
        /// </summary>
        public int ModelID { get; set; }

        /// <summary>
        /// 短息模板类型ID
        /// </summary>
        public string ModelKey { get; set; }


        /// <summary>
        /// 短信模板内容
        /// </summary>
        public string ModelContent { get; set; }


        /// <summary>
        /// 模板创建者ID
        /// </summary>
        public string SysUserID { get; set; }


        /// <summary>
        /// 模板创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
