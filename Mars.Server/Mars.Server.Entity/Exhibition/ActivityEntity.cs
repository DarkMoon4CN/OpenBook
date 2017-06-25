using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class ActivityEntity
    {
        public ActivityEntity() { }
        #region Model
        private int _activityid;
        private int _parentid = 0;
        private int _exhibitionid = 0;
        private int _exhibitorid = 0;
        private string _activitytitle = "";
        private DateTime _activitystarttime = DateTime.Now;
        private DateTime _activityendtime = DateTime.Now;
        private string _activitylocation = "";
        private string _activityhostunit = "";
        private string _activityabstract = "";
        private string _activityguest = "";
        private int _statetypeid = 0;
        private int _activityorder = 1000;
        private int _activitytypeid = 0;
        private string _createuserid = "";
        private DateTime _createtime = DateTime.Now;

        private List<ActivityEntity> _childrenActivityList;
        public int ExhibitionID { get; set; }

        /// <summary>
        /// 主键，自增
        /// </summary>
        public int ActivityID
        {
            set { _activityid = value; }
            get { return _activityid; }
        }
        /// <summary>
        /// 父活动ID，主活动为0
        /// </summary>
        public int ParentID
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 展商ID
        ///   
        /// </summary>
        public int ExhibitorID
        {
            set { _exhibitorid = value; }
            get { return _exhibitorid; }
        }
        /// <summary>
        /// 活动标题
        /// </summary>
        public string ActivityTitle
        {
            set { _activitytitle = value; }
            get { return _activitytitle; }
        }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime ActivityStartTime
        {
            set { _activitystarttime = value; }
            get { return _activitystarttime; }
        }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime ActivityEndTime
        {
            set { _activityendtime = value; }
            get { return _activityendtime; }
        }
        /// <summary>
        /// 活动位置
        /// </summary>
        public string ActivityLocation
        {
            set { _activitylocation = value; }
            get { return _activitylocation; }
        }
        /// <summary>
        /// 活动主办方
        /// </summary>
        public string ActivityHostUnit
        {
            set { _activityhostunit = value; }
            get { return _activityhostunit; }
        }
        /// <summary>
        /// 活动简介
        /// </summary>
        public string ActivityAbstract
        {
            set { _activityabstract = value; }
            get { return _activityabstract; }
        }
        /// <summary>
        /// 活动嘉宾
        /// </summary>
        public string ActivityGuest
        {
            set { _activityguest = value; }
            get { return _activityguest; }
        }
        /// <summary>
        /// 状态ID
        /// </summary>
        public int StateTypeID
        {
            set { _statetypeid = value; }
            get { return _statetypeid; }
        }
        /// <summary>
        /// 活动排序
        /// </summary>
        public int ActivityOrder
        {
            set { _activityorder = value; }
            get { return _activityorder; }
        }
        /// <summary>
        /// 活动类型ID
        /// </summary>
        public int ActivityTypeID
        {
            set { _activitytypeid = value; }
            get { return _activitytypeid; }
        }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        public List<ActivityEntity> ChildrenActivityList
        {
            get
            {
                return _childrenActivityList;
            }

            set
            {
                _childrenActivityList = value;
            }
        }

        public int Exhibitionid
        {
            get
            {
                return _exhibitionid;
            }

            set
            {
                _exhibitionid = value;
            }
        }
        #endregion Model
    }
}
