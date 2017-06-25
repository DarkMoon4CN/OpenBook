using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity
{
    public class CategoriesEntity
    {
        public int CalendarTypeID { get; set; }
        public int ParentCalendarTypeID { get; set; }
        public string CalendarTypeName { get; set; }
        public int CalendarTypeKind { get; set; }
        public string Descripition { get; set; }
        public bool Dismiss { get; set; }
        public int PictureID { get; set; }
         
    } 
        public class CategoriesSearchEntity : EntityBase
        {
            public int CalendarTypeID { get; set; }
            public string CalendarTypeName { get; set; }
            public int ParentCalendarTypeID { get; set; } 
        }
}
