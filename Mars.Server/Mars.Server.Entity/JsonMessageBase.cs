using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class JsonMessageBase
    {
        public string Msg { get; set; }
        //1-success,0-failed
        public int Status { get; set; }

        public string Tag { get; set; }
        private DateTime _ServerTime = DateTime.Now;

        public DateTime ServerTime
        {
            get { return _ServerTime; }
            set { _ServerTime = value; }
        }
    }

    public class JsonMessageBase<T> : JsonMessageBase
    {
        public T Value { get; set; }
    }

    public class JsonMessageBase<T1, T2> : JsonMessageBase<T1>
    {
        public T2 Value2 { get; set; }
    }

    public class JsonMessageBase<T1, T2, T3> : JsonMessageBase<T1,T2>
    {
        public T3 Value3 { get; set; }
    }
    public class JsonMessageBase<T1, T2, T3,T4> : JsonMessageBase<T1,T2,T3>
    {   
        public T4 Value4 { get; set; }
    }
}
