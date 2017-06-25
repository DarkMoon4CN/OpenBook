using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class ExhibitorSearchEntity:EntityBase
    {
        /// <summary>
        /// 展场id
        /// </summary>
        public int ExhibitionID { get; set; }
        /// <summary>
        /// 展商名称
        /// </summary>
        public string ExhibitorName { get; set; }
    }
}
