using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class ExhibitorToCustomerLocationEntity
    {
        public ExhibitorToCustomerLocationEntity() { }

        /// <summary>
        /// 主键，展商展位id
        /// </summary>
        public int ExhibitorLocationID { get; set; }

        /// <summary>
        /// 展商展位
        /// </summary>
        public string ExhibitorLocation { get; set; }

        /// <summary>
        /// 展商展位排序
        /// </summary>
        public int ExhibitiorLocationOrder { get; set; }
    }
}
