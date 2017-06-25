using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class AdvertisementEntity
    {
        public AdvertisementEntity() { }
        #region Model
        private int _advertisementid;
        private int _exhibitionid = 0;
        private string _advertisementurl = "";
        private string _advertisementtitle;
        private int _advertisementorder;
        private int _statetypeid;
        private string _createuserid;
        private DateTime _createtime;
        /// <summary>
        /// 主键，自增
        /// </summary>
        public int AdvertisementID
        {
            set { _advertisementid = value; }
            get { return _advertisementid; }
        }
        /// <summary>
        /// 展场id
        /// </summary>
        public int ExhibitionID
        {
            set { _exhibitionid = value; }
            get { return _exhibitionid; }
        }
        /// <summary>
        /// 广告对应url
        /// </summary>
        public string AdvertisementUrl
        {
            set { _advertisementurl = value; }
            get { return _advertisementurl; }
        }
        /// <summary>
        /// 广告标题
        /// </summary>
        public string AdvertisementTitle
        {
            set { _advertisementtitle = value; }
            get { return _advertisementtitle; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int AdvertisementOrder
        {
            set { _advertisementorder = value; }
            get { return _advertisementorder; }
        }
        /// <summary>
        /// 通用状态
        /// </summary>
        public int StateTypeID
        {
            set { _statetypeid = value; }
            get { return _statetypeid; }
        }
        /// <summary>
        /// 创建人id
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
        #endregion Model

    }
}
