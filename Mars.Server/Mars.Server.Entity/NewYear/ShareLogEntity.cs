using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.NewYear
{
    [Serializable]
    public class ShareLogEntity
    {
        public ShareLogEntity()
        { }
        #region Model
        private int _sid;
        private DateTime _createtime = DateTime.Now;
        private int _shareuserid = 0;
        private string _machinecode = "";
        private int _sharetypeid = 0;
        private string _systemname = "";
        private string _verson = "";
        private string _ipaddress = "";
        private string _exinfo = "";
        private int _coupletid = 0;
        private int _imageid = 0;
        private int _isview = 1;
        /// <summary>
        /// 
        /// </summary>
        public int SID
        {
            set { _sid = value; }
            get { return _sid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ShareUserID
        {
            set { _shareuserid = value; }
            get { return _shareuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MachineCode
        {
            set { _machinecode = value; }
            get { return _machinecode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ShareTypeID
        {
            set { _sharetypeid = value; }
            get { return _sharetypeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SystemName
        {
            set { _systemname = value; }
            get { return _systemname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Verson
        {
            set { _verson = value; }
            get { return _verson; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IPAddress
        {
            set { _ipaddress = value; }
            get { return _ipaddress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExInfo
        {
            set { _exinfo = value; }
            get { return _exinfo; }
        }
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
        public int ImageID
        {
            set { _imageid = value; }
            get { return _imageid; }
        }
        /// <summary>
        /// 是否打开查看，默认 1 查看，0 为发布
        /// </summary>
        public int IsView
        {
            set { _isview = value; }
            get { return _isview; }
        }
        #endregion Model
    }
}
