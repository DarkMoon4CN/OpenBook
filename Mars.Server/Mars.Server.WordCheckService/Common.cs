using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.WordCheckService
{
    public class Common
    {
        public DataTable GetDataTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add(new DataColumn("ID", typeof(Int32)));
            table.Columns.Add(new DataColumn("ContentType", typeof(Int32)));
            table.Columns.Add(new DataColumn("CheckTypeID", typeof(Int32)));
            table.Columns.Add(new DataColumn("WordsInfo", typeof(String)));

            return table;
        }
    }
}
