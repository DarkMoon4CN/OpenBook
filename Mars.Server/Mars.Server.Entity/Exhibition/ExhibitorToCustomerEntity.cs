using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class ExhibitorToCustomerEntity
    {
        public ExhibitorToCustomerEntity() { }

        /// <summary>
        /// 主键，展商id
        /// </summary>
        public int ExhibitorID{get;set;}
        /// <summary>
        /// 展会id
        /// </summary>
        public int ExhibitionID { get; set; }
        /// <summary>
        /// 展商名称
        /// </summary>
        public string ExhibitorName { get; set; }
        /// <summary>
        /// 展商拼音
        /// </summary>
        public string ExhibitorPinYin { get; set; } 
        /// <summary>
        /// 是否存在书单
        /// </summary>
        public bool IsHadBookList { get; set; }
        /// <summary>
        /// 展商展位
        /// </summary>
        public List<ExhibitorToCustomerLocationEntity> ExhibitorLocationList { get; set; }

    }
}
