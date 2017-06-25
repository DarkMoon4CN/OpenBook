using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.Exhibition
{
    [Serializable]
    public class SearchKeyWordEntity
    {
        /// <summary>
        /// 查询名称
        /// </summary>
        public string SearchName { get; set; }
        /// <summary>
        /// 查询拼音
        /// </summary>
        public string SearchPinYin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SearchType { get; set; }
    }
}
