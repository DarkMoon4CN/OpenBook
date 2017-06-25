using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class ActivitySearchEntity : EntityBase
    {
        /// <summary>
        /// 展场id
        /// </summary>
        public int ExhibitionID { get; set; }
        /// <summary>
        /// 展商名称或活动名称
        /// </summary>
        public string SearchName { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentID { get; set; }
    }
}
