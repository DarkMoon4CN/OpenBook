using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class ExhibitionEmailEntity
    {
        public int BookListCustomerID { get; set; }
        public int ExhibitionID { get; set; }
        public int SendTypeID { get; set; }
        public string CustomerID {get;set; }
        public string CustomerToken { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? SendTime { get; set; }
    }
}
