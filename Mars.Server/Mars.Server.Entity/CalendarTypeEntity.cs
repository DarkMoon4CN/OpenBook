using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class CalendarTypeEntity
    {
        #region Model
        private int _isFavor;

        public int IsFavor
        {
            get { return _isFavor; }
            set
            {
                if (value > 0)
                {
                    _isFavor = 1;
                }
                else
                    _isFavor = value;
            }
        }

        private int _calendartypeid;
        /// <summary>
        /// 
        /// </summary>
        public int CalendarTypeID
        {
            set { _calendartypeid = value; }
            get { return _calendartypeid; }
        }
        private int _parentcalendartypeid;
        /// <summary>
        /// 
        /// </summary>
        public int ParentCalendarTypeID
        {
            set { _parentcalendartypeid = value; }
            get { return _parentcalendartypeid; }
        }
        private string _calendartypename;
        /// <summary>
        /// 
        /// </summary>
        public string CalendarTypeName
        {
            set { _calendartypename = value; }
            get { return _calendartypename; }
        }
        private int _calendartypekind;
        /// <summary>
        /// 
        /// </summary>
        public int CalendarTypeKind
        {
            set { _calendartypekind = value; }
            get { return _calendartypekind; }
        }
        private string _descripition;
        /// <summary>
        /// 
        /// </summary>
        public string Descripition
        {
            set { _descripition = value; }
            get { return _descripition; }
        }
        private string _dismiss;
        /// <summary>
        /// 
        /// </summary>
        public string Dismiss
        {
            set { _dismiss = value; }
            get { return _dismiss; }
        }

        public int PictureID { get; set; }

        public string PicUrl
        {
            get
            {
                return string.IsNullOrEmpty(Domain) ? string.Empty : string.Concat(Domain, PicturePath);
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public string Domain { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string PicturePath { get; set; }

        public List<EventItemEntity> Events { get; set; }
        #endregion Model
    }
}
