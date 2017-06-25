using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class DialogMessagesEntity
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public int MessageID { get; set; }
        /// <summary>
        /// 图片链接地址
        /// </summary>
        public string ImageLink { get; set; }

        /// <summary>
        /// 手机类型  0.全部  1.Android  2.IOS
        /// </summary>
        public int MoblieType { get; set; }

        /// <summary>
        /// button显示的文本
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// 文章链接
        /// </summary>
        public string ArticleLink { get; set; }


        /// <summary>
        ///  主体 文本
        /// </summary>
        public string Contents { get; set; }


        /// <summary>
        /// 启动类型:
        /// 1.单次启动
        /// 2.使用 StartTime 与 EndTime 周期内启动  
        /// 3.使用 StartCount  可弹出几次
        /// </summary>
        public int StartType { get; set; }

        /// <summary>
        ///  周期内   开始时间
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public DateTime? StartTime { get; set; }


        /// <summary>
        /// 周期内   结束时间
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public DateTime? EndTime { get; set; }

        /// <summary>
        ///  可弹出几次
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public long? StartCount { get; set; }
    }
}
