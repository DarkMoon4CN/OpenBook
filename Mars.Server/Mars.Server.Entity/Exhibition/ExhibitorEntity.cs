using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class ExhibitorEntity
    {
        public ExhibitorEntity(){}
        #region Model
        private int _exhibitorid;
        private int _exhibitionid = 0;
        private string _exhibitorname = "";
        private string _exhibitorpinyin = "";
        private int _obcustomerid = 0;
        private int _obcustomertypeid = 0;
        private bool _ishadbooklist = false;
        private int _statetypeid = 0;
        private string _createuserid = "";
        private DateTime _createtime = DateTime.Now;
        public List<ExhibitorLocationEntity> ExhibitorLocationList { get; set; }
        public int rowId { get; set; }
        /// <summary>
        /// 展商ID，主键，自增
        /// </summary>
        public int ExhibitorID
        {
            set { _exhibitorid = value; }
            get { return _exhibitorid; }
        }
        /// <summary>
        /// 展场ID
        /// </summary>
        public int ExhibitionID
        {
            set { _exhibitionid = value; }
            get { return _exhibitionid; }
        }
        /// <summary>
        /// 展商名称
        /// </summary>
        public string ExhibitorName
        {
            set { _exhibitorname = value; }
            get { return _exhibitorname; }
        }
        /// <summary>
        /// 展商拼音名称，自动添加根据展商名称生成
        /// </summary>
        public string ExhibitorPinYin
        {
            set { _exhibitorpinyin = value; }
            get { return _exhibitorpinyin; }
        }
        /// <summary>
        /// 对应开卷客户ID，组合OBCustomerTypeID进行关联
        /// </summary>
        public int OBCustomerID
        {
            set { _obcustomerid = value; }
            get { return _obcustomerid; }
        }
        /// <summary>
        /// 开卷客户类型ID
        /// </summary>
        public int OBCustomerTypeID
        {
            set { _obcustomertypeid = value; }
            get { return _obcustomertypeid; }
        }
        /// <summary>
        /// 是否存在书单
        /// </summary>
        public bool IsHadBookList
        {
            set { _ishadbooklist = value; }
            get { return _ishadbooklist; }
        }
        /// <summary>
        /// 状态类型ID
        /// </summary>
        public int StateTypeID
        {
            set { _statetypeid = value; }
            get { return _statetypeid; }
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
        #endregion Model
    }
}
