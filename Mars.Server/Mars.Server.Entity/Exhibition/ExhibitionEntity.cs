using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class ExhibitionEntity
    {
        public ExhibitionEntity() {}
        #region Model
        private int _exhibitionid;
        private string _exhibitiontitle = "";
        private string _exhibitionlogourl = "";
        private DateTime _exhibitionstarttime = DateTime.Now;
        private DateTime _exhibitionendtime = DateTime.Now;
        private string _exhibitionaddress = "";
        private string _exhibitiontraffic = "";
        private string _exhibitionlocation = "";
        private string _exhibitionabstract = "";
        private string _exhibitionabout = "";
        private int _exhibitionorder = 1000;
        private string _exhibitionbooklistdesc = "";
        private int _statetypeid = 0;
        private bool _ispublish = false;
        private string _booklistdownloadurl = "";
        private bool _isdownloadbooklist = false;
        private string _createuserid = "";
        private DateTime _createtime = DateTime.Now;
        private List<AdvertisementEntity> _AdvertisementList;
        /// <summary>
        /// 展会ID，主键，自增
        /// </summary>
        public int ExhibitionID
        {
            set { _exhibitionid = value; }
            get { return _exhibitionid; }
        }
        /// <summary>
        /// 展会名称
        /// </summary>
        public string ExhibitionTitle
        {
            set { _exhibitiontitle = value; }
            get { return _exhibitiontitle; }
        }
        /// <summary>
        /// 展会Logo的地址(扩展字段)
        /// </summary>
        public string ExhibitionLogoUrl
        {
            set { _exhibitionlogourl = value; }
            get { return _exhibitionlogourl; }
        }
        /// <summary>
        /// 展会开始时间
        /// </summary>
        public DateTime ExhibitionStartTime
        {
            set { _exhibitionstarttime = value; }
            get { return _exhibitionstarttime; }
        }
        /// <summary>
        /// 展会结束时间
        /// </summary>
        public DateTime ExhibitionEndTime
        {
            set { _exhibitionendtime = value; }
            get { return _exhibitionendtime; }
        }
        /// <summary>
        /// 展会地址
        /// </summary>
        public string ExhibitionAddress
        {
            set { _exhibitionaddress = value; }
            get { return _exhibitionaddress; }
        }
        /// <summary>
        /// 展会通勤方式
        /// </summary>
        public string ExhibitionTraffic
        {
            set { _exhibitiontraffic = value; }
            get { return _exhibitiontraffic; }
        }
        /// <summary>
        /// 展会位置存贮地图位置(扩展字段)
        /// </summary>
        public string ExhibitionLocation
        {
            set { _exhibitionlocation = value; }
            get { return _exhibitionlocation; }
        }
        /// <summary>
        /// 展会简介
        /// </summary>
        public string ExhibitionAbstract
        {
            set { _exhibitionabstract = value; }
            get { return _exhibitionabstract; }
        }
        /// <summary>
        /// 关于展会信息
        /// </summary>
        public string ExhibitionAbout
        {
            set { _exhibitionabout = value; }
            get { return _exhibitionabout; }
        }
        /// <summary>
        /// 展会排序
        /// </summary>
        public int ExhibitionOrder
        {
            set { _exhibitionorder = value; }
            get { return _exhibitionorder; }
        }
        /// <summary>
        /// 获取书单编辑描述
        /// </summary>
        public string ExhibitionBookListDesc
        {
            set { _exhibitionbooklistdesc = value; }
            get { return _exhibitionbooklistdesc; }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public int StateTypeID
        {
            set { _statetypeid = value; }
            get { return _statetypeid; }
        }
        /// <summary>
        /// 是否发布，有发布记录即可在app显示
        /// </summary>
        public bool IsPublish
        {
            set { _ispublish = value; }
            get { return _ispublish; }
        }
        /// <summary>
        /// 书单下载地址
        /// </summary>
        public string BookListDownloadUrl
        {
            set { _booklistdownloadurl = value; }
            get { return _booklistdownloadurl; }
        }
        /// <summary>
        /// 是否开启书单现在功能
        /// </summary>
        public bool IsDownloadBookList
        {
            set { _isdownloadbooklist = value; }
            get { return _isdownloadbooklist; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        public List<AdvertisementEntity> AdvertisementList
        {
            get
            {
                return _AdvertisementList;
            }

            set
            {
                _AdvertisementList = value;
            }
        }
        #endregion Model
    }
}
