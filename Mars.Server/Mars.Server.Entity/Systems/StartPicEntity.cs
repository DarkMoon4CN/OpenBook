using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Systems
{
    [Serializable]
    public class StartPicEntity
    {
        public StartPicEntity()
        { }
        #region Model
        private int _pictureid;
        private DateTime? _starttime;
        private DateTime? _endtime;
        private bool _isdefault = false;
        private bool _isoutdate = false;
        /// <summary>
        /// 图片路径
        /// </summary>
        public string PicURL { get; set; }
        /// <summary>
        /// 访问路径
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PictureID
        {
            set { _pictureid = value; }
            get { return _pictureid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? StartTime
        {
            set { _starttime = value; }
            get { return _starttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDefault
        {
            set { _isdefault = value; }
            get { return _isdefault; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsOutdate
        {
            set { _isoutdate = value; }
            get { return _isoutdate; }
        }
        #endregion Model
    }
}
