using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class FeedbackEntity
    {
        #region Model
        private int _feedbackid;
        /// <summary>
        /// 
        /// </summary>
        public int FeedbackID
        {
            set { _feedbackid = value; }
            get { return _feedbackid; }
        }
        private string _content;
        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        private int _userid;
        /// <summary>
        /// 
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        private DateTime _createtime;
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        private string   _ContactMethod;
        /// <summary>
        /// 
        /// </summary>
        public string ContactMethod
        {
            set { _ContactMethod = value; }
            get { return _ContactMethod; }
        }
        #endregion Model
    }
}
