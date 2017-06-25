using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class UserEventItemEntity
    {
        #region Model

        private string _title;
        private int _eventitemid;
        private int _repeattypeid;
        private DateTime? _starttime = new DateTime(4000, 1, 1);
        private DateTime? _endtime = new DateTime(4000, 1, 1);
        //private int _startmonth;
        //private int _startday;
        //private int _startweek;
        //private int _starthour;
        //private int _startminutes;
        
        public int EventItemID
        {
            set { _eventitemid = value; }
            get { return _eventitemid; }
        }
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        public Guid EventItemGUID { get; set; }
        public int UserID { get; set; }
        public bool StartTimeNeedConvertLunar { get; set; }
        public int CalendarTypeID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
       
        /// <summary>
        ///  开始时间
        /// </summary>
        public DateTime? StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }

        public DateTime? EditTime { get; set; }


        /// <summary>
        /// 0：默认（不重复） 1：天 2：周 3：月 4：年
        /// </summary>
        public int RepeatTypeID
        {
            set { _repeattypeid = value; }
            get { return _repeattypeid; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public int StartYear
        {
            get { return StartTime.Value.Year; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public int StartMonth
        {
            get { return StartTime.Value.Month; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public int StartDay
        {
            get { return StartTime.Value.Day; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public int StartWeek
        {
            get { return (int)(StartTime.Value.DayOfWeek); }
        }

        [Newtonsoft.Json.JsonIgnore]
        public int StartHour
        {
            get { return StartTime.Value.Hour; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public int StartMinutes
        {
            get { return StartTime.Value.Minute; }
        }


        public int EventItemState { get; set; }
        public string Remark { get; set; }
        public string Locate { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public bool Finished { get; set; }
        public int BitTags { get; set; }
        public string Tag { get; set; }
        public string CalendarTypeName { get; set; }
        public List<string> Tags { get; set; }
        public List<ReminderEntity> Reminders { get; set; }
        public List<int> Pics { get; set; }
        #endregion Model
    }
}
