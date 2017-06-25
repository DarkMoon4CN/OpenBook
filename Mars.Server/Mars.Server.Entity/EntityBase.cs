using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    [Serializable]
    public class EntityBase
    {
        public string ToJson(EntityBase obj)
        {
            return JsonObj<EntityBase>.ToJsonString(obj);
        }
        public EntityBase() { }

        private int _PageIndex = 1;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }
        private int _PageSize = 20;

        /// <summary>
        /// 页大小
        /// </summary>

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        private bool _UseDBPagination = true;

        /// <summary>
        /// 
        /// </summary>

        public bool UseDBPagination
        {
            get { return _UseDBPagination; }
            set { _UseDBPagination = value; }
        }

        private int _OrderFieldIndex = -1;

        public int OrderFieldIndex
        {
            get { return _OrderFieldIndex; }
            set { _OrderFieldIndex = value; }
        }

        private OrderFieldType orderfieldType = OrderFieldType.Desc;

        public OrderFieldType OrderfieldType
        {
            get { return orderfieldType; }
            set { orderfieldType = value; }
        }

        //private string _logMeta = "";
        ///// <summary>
        ///// 用户的浏览器信息
        ///// </summary>

        //public string LogMeta
        //{
        //    get { return _logMeta; }
        //    set { _logMeta = value; }
        //}

        //private string _logExInfo = "";
        ///// <summary>
        ///// 日志扩展信息
        ///// </summary>

        //public string LogExInfo
        //{
        //    get { return _logExInfo; }
        //    set { _logExInfo = value; }
        //}
    }
}
