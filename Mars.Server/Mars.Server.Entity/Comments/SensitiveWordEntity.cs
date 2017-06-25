using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Comments
{
    [Serializable]
    public class SensitiveWordEntity
    {
        public SensitiveWordEntity()
        { }
        #region Model
        private int _swid;
        private string _sensitivewords = "";
        private int _statetypeid = 1;
        private bool _isneedrecheck = false;
        private int _createuserid = 0;
        private DateTime _createtime = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public int SWID
        {
            set { _swid = value; }
            get { return _swid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SensitiveWords
        {
            set { _sensitivewords = value; }
            get { return _sensitivewords; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StateTypeID
        {
            set { _statetypeid = value; }
            get { return _statetypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsNeedRecheck
        {
            set { _isneedrecheck = value; }
            get { return _isneedrecheck; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CreateUserID
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
        #endregion Model
    }
}
