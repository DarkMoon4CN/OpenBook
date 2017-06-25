using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.NewYear
{
    public class CoupletEntity
    {
        public CoupletEntity()
        { }
        #region Model
        private int _coupletid;
        private int _coupletcontenttypeid;
        private string _coupletcontent;
        private int _orderby = 1000;
        private int _statetypeid = 1;
        public int CoupletTypeID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CoupletID
        {
            set { _coupletid = value; }
            get { return _coupletid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CoupletContentTypeID
        {
            set { _coupletcontenttypeid = value; }
            get { return _coupletcontenttypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CoupletContent
        {
            set { _coupletcontent = value; }
            get { return _coupletcontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OrderBy
        {
            set { _orderby = value; }
            get { return _orderby; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StateTypeID
        {
            set { _statetypeid = value; }
            get { return _statetypeid; }
        }
        #endregion Model
    }

    public class CoupletGroupEntity {
        public int CoupletID
        {
            get; set;
        }

        public string UpCouplet { get; set; }
        public string DownCouplet { get; set; }
        public string HorizontalCouplet { get; set; }

        public int CoupletTypeID { get; set; }
    }
}
