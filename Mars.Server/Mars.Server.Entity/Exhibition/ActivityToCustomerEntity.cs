using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class ActivityToCustomerEntity
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ActivityToCustomerEntity() { }

        /// <summary>
        /// 活动id
        /// </summary>
        public int ActivityID { get; set; }
        /// <summary>
        /// 展商id
        /// </summary>
        public int ExhibitorID { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityTitle { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime ActivityStartTime { get; set; }
        /// <summary>
        /// 活动结束时间 
        /// </summary>
        public DateTime ActivityEndTime { get; set; }
        /// <summary>
        /// 格式化后的活动时间
        /// </summary>
        public string FormatActivityTime { get; set; }
        /// <summary>
        /// 活动地点
        /// </summary>
        public string ActivityLocation { get; set; }
        /// <summary>
        /// 活动主办方
        /// </summary>
        public string ActivityHostUnit { get; set; }
        /// <summary>
        /// 活动简介
        /// </summary>
        public string ActivityAbstract { get; set; }
        /// <summary>
        /// 活动嘉宾
        /// </summary>
        public string ActivityGuest { get; set; }
        /// <summary>
        /// 展商名称
        /// </summary>
        public string ExhibitorName { get; set; }
        /// <summary>
        /// 展商拼音
        /// </summary>
        public string ExhibitorPinYin { get; set; }
        /// <summary>
        /// 子活动列表
        /// </summary>
        public List<ActivityToCustomerEntity> SubactivityList { get; set; }
        /// <summary>
        /// 格式化输出活动开始时间
        /// </summary>
        public string FormatActivityStartTime { get; set; }
        /// <summary>
        /// 格式化输出活动结束时间
        /// </summary>
        public string FormatActivityEndTiem { get; set; }

        /// <summary>
        /// 活动是否结束
        /// </summary>
        public bool ActivityIsEnd { get; set; }
    }
}
