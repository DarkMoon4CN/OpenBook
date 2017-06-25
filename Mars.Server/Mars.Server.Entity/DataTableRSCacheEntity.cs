using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    [Serializable]
    public class DataTableRSCacheEntity
    {
        private DataTable _Source;

        public DataTable Source
        {
            get { return _Source; }
            set { _Source = value; }
        }
        private int _TotalCnt;

        public int TotalCnt
        {
            get { return _TotalCnt; }
            set { _TotalCnt = value; }
        }
    }
}
