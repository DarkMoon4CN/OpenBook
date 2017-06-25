using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class ExhibitorLocationEntity
    {
        public ExhibitorLocationEntity() { }
        #region Model
        private int _exhibitorlocationid;
        private int _exhibitorid = 0;
        private string _exhibitorlocation = "";
        private int _statetypeid = 0;
        private int _exhibitiorlocationorder = 1000;
        private string _createuserid = "";
        private DateTime _createtime = DateTime.Now;
        /// <summary>
        /// 主键
        /// </summary>
        public int ExhibitorLocationID
        {
            set { _exhibitorlocationid = value; }
            get { return _exhibitorlocationid; }
        }
        /// <summary>
        /// 展商ID
        /// </summary>
        public int ExhibitorID
        {
            set { _exhibitorid = value; }
            get { return _exhibitorid; }
        }
        /// <summary>
        /// 展位位置
        /// </summary>
        public string ExhibitorLocation
        {
            set { _exhibitorlocation = value; }
            get { return _exhibitorlocation; }
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
        /// 展位位置排序
        /// </summary>
        public int ExhibitiorLocationOrder
        {
            set { _exhibitiorlocationorder = value; }
            get { return _exhibitiorlocationorder; }
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
