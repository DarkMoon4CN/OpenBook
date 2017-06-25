using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class SignBookEntity
    {
        public int SignID { get; set; }
        public string Customer { get; set; }
        public string Moblie { get; set; }
        public string Company { get; set; }

        public string Department { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public string LuckyNumber { get; set; }
        public int IsSign { get; set; }
        public int IsRegister { get; set; }
        public string Remarks { get; set; }
        public string CustomerKey { get; set; }
        public string SalesName { get; set; }
        public string SignURL { get; set; }
        public DateTime CreateTime { get; set; }
        public string SalesDepartment { get; set; }
        
    }
}
