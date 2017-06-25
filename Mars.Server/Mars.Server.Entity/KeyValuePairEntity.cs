using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class KeyValuePairEntity
    {
        public string K { get; set; }
        public string V { get; set; }
    }
    public class KeyValuePairEntity<T>:KeyValuePairEntity
    {
        public T T1 { get; set; }  
    }
}
