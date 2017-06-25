using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class EventItemEntity
    {
        #region Model
        public bool StartTimeNeedConvertLunar { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public DateTime PublishTime { get; set; }
        public Guid FromEventItemGUID { get; set; }
        public string Url { get; set; }
        public int Recommend { get; set; }
        public string Remark { get; set; }
        public string Locate { get; set; }
        public int UserID { get; set; }
        public int CalendarTypeID { get; set; }
        public string CalendarTypeName { get; set; }
        /// <summary>
        /// 状态 0删除 1正常
        /// </summary>
        public int EventItemState { get; set; }
        private int _eventitemid;
        /// <summary>
        /// 
        /// </summary>
        public int EventItemID
        {
            set { _eventitemid = value; }
            get { return _eventitemid; }
        }
        private string _title;
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }

        /// <summary>
        /// 节日标题
        /// </summary>
        public string Title2 { get; set; }

        private string _content;
        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        private DateTime? _starttime = new DateTime(4000, 1, 1);
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        private DateTime? _endtime = new DateTime(4000, 1, 1);
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        private DateTime _createtime = DateTime.Now;

        private DateTime? _StartTime2 = new DateTime(4000, 1, 1);
        /// <summary>
        /// 节日开始时间
        /// </summary>
        public DateTime? StartTime2
        {
            get { return _StartTime2; }
            set { _StartTime2 = value; }
        }

        private DateTime? _EndTime2 = new DateTime(4000, 1, 1);
        /// <summary>
        /// 节日结束时间
        /// </summary>
        public DateTime? EndTime2
        {
            get { return _EndTime2; }
            set { _EndTime2 = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        //private int _eventtypeid;
        ///// <summary>
        ///// 
        ///// </summary>
        //public int EventTypeID
        //{
        //    set { _eventtypeid = value; }
        //    get { return _eventtypeid; }
        //}
        private int _repeattypeid;
        /// <summary>
        /// 
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
        private int _startmonth;
        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int StartMonth
        {
            get { return StartTime.Value.Month; }
        }
        private int _startday;
        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int StartDay
        {
            get { return StartTime.Value.Day; }
        }
        private int _startweek;
        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int StartWeek
        {
            get { return (int)(StartTime.Value.DayOfWeek); }
        }
        private int _starthour;
        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int StartHour
        {
            get { return StartTime.Value.Hour; }
        }
        private int _startminutes;
        /// <summary>
        /// 
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int StartMinutes
        {
            get { return StartTime.Value.Minute; }
        }

        public Guid EventItemGUID { get; set; }

        public List<string> Tags { get; set; }

        [Obsolete]
        public ReminderEntity Reminder { get; set; }

        public List<ReminderEntity> Reminders { get; set; }

        public List<int> Pics { get; set; }

        public string Tag { get; set; }
        public bool Finished { get; set; }

        public string PicUrl
        {
            get
            {
                return string.IsNullOrEmpty(Domain) ? string.Empty : string.Concat(Domain, PicturePath);
            }
        }
        [Newtonsoft.Json.JsonIgnore]
        public string Domain { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string PicturePath { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int PictureID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int Advert { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int AdvertOrder { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int PublishAreaID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string Html { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int CreateOpID { get; set; }

        /// <summary>
        /// 主题图
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int ThemePictureID { get; set; }

        /// <summary>
        /// 轮播图
        /// </summary>
        public int CarouselPictureID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string PublishSource { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string ActivePlace { get; set; }

        /// <summary>
        /// 书单地址
        /// </summary>
        public string BookListPath { get; set; }

        /// <summary>
        /// 是否可报名 0否 1是
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool ActiveApply { get; set; }

        /// <summary>
        /// 0普通文章 1节日文章
        /// </summary>
        public int EventItemFlag { get; set; }

        public int BitTags { get; set; }

        /// <summary>
        /// 辅助字段 是否单篇成组 0否 1是
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int Singlegroup { get; set; }

        /// <summary>
        /// 节日类型ID
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public Guid FestivalID { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCnt { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeCnt { get; set; }
        /// <summary>
        /// 首页轮播结束时间
        /// </summary>
        public DateTime AdsEndTime { get; set; }
        /// <summary>
        /// 发现频道结束时间
        /// </summary>
        public DateTime DiscoverAdsEndTime { get; set; }
        #endregion Model
    }

    public class EventItemSearchEntity : EntityBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 二级分类
        /// </summary>
        public int SecondTypeID { get; set; }

        /// <summary>
        /// 一级分类
        /// </summary>
        public int FirstTypeID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 指定日期
        /// </summary>
        public DateTime? DayTime { get; set; }
        /// <summary>
        /// 推荐 0否 1是
        /// </summary>
        public int Recommend { get; set; }

        private int _Advert = -1;
        /// <summary>
        /// 首页轮播 -1忽略 0否 1是 
        /// </summary>
        public int Advert { 
            get { return _Advert; }
            set { _Advert = value; }
        }

        private int _DiscoverAdvert = -1;
        /// <summary>
        /// 发现轮播 -1忽略 0否 1是
        /// </summary>
        public int DiscoverAdvert {
            get { return _DiscoverAdvert; }
            set { _DiscoverAdvert = value; }
        }

        /// <summary>
        /// 操作员
        /// </summary>
        public int CreateOpID { get; set; }

        /// <summary>
        /// 是否开启轮播排序
        /// </summary>
        public bool IsEnableAdvertOrder { get; set; }

        private int _SubarticleState = -1;
        /// <summary>
        /// 子文章状态  -1不操作  0否 1是
        /// </summary>
        public int SubarticleState
        {
            get { return _SubarticleState; }
            set { _SubarticleState = value; }
        }

        private int _SingleGroup = -1;
        /// <summary>
        /// 单篇成组 0否 1是
        /// </summary>
        public int SingleGroup {
            get { return _SingleGroup; }
            set { _SingleGroup = value; }
        }

    }

    /// <summary>
    /// 视图对应实体
    /// </summary>
    public class EventItemViewEntity
    {
        [Newtonsoft.Json.JsonIgnore]
        public int EventItemID { get; set; }

        public Guid EventItemGUID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 二级分类ID
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int CalendarTypeID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string SecondTypeName { get; set; }

        /// <summary>
        /// 一级分类ID
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int FirstTypeID { get; set; }

        public string FirstTypName { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int PublishAreaID { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string ZoneName { get; set; }

        public string Html { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int CreateOpID { get; set; }

        public int BrowseCnt { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int EventItemState { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string Url { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int Recommend { get; set; }

        public string PublishTime { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int PictureID { get; set; }

        public string Domain { get; set; }

        public string PicturePath { get; set; }

        /// <summary>
        /// 发现图片轮播状态
        /// </summary>
        public int DiscoverAdvert { get; set; }

        /// <summary>
        /// 封面图片轮播项状态
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int Advert { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int AdvertOrder { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int ThemePictureID { get; set; }

        /// <summary>
        /// 主题图片路径
        /// </summary>
        public string ThemePicturePath { get; set; }

        public string PublishSource { get; set; }

        /// <summary>
        /// 活动地点
        /// </summary>
        public string ActivePlace { get; set; }

        /// <summary>
        /// 活动时间 
        /// </summary>
        public string ActiveTimeDesc { get; set; }

        /// <summary>
        /// 书单地址
        /// </summary>
        public string BookListPath { get; set; }

        /// <summary>
        /// 是否可报名 0否 1是
        /// </summary>      
        public bool ActiveApply { get; set; }

        /// <summary>
        /// 节日标题
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string Title2 { get; set; }

        /// <summary>
        /// 节日开始时间
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public DateTime StartTime2 { get; set; }

        /// <summary>
        /// 节日结束时间
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public DateTime EndTime2 { get; set; }

        /// <summary>
        /// 轮播图片ID
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int CarouselPictureID { get; set; }

        /// <summary>
        /// 轮播图片路径
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string CarouselPicturePath { get; set; }

        /// <summary>
        /// 0普通文章 1节日文章
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public int EventItemFlag { get; set; }

        /// <summary>
        /// 是否单篇文章成组
        /// </summary>
        public bool IsSingleGroupState { get; set; }

        /// <summary>
        /// 节日文章ID
        /// </summary>
        public Guid FestivalID { get; set; }

        public DateTime AdsEndTime { get; set; }
        public DateTime DiscoverAdsEndTime { get; set; }
    }

    /// <summary>
    /// 各平台请求是返回的数据载体
    /// </summary>
    public class EventItemAsyncPullRequest 
    {
        public Guid EventItemGUID { get; set; }

        public DateTime? EditTime { get; set; }
    }
}
