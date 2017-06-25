using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.Entity.NewYear
{
    [Serializable]
    public class FuImageEntity
    {
        public FuImageEntity()
        { }
        #region Model
        private int _imageid;
        private string _imageurl = "";
        /// <summary>
        /// 
        /// </summary>
        public int ImageID
        {
            set { _imageid = value; }
            get { return _imageid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl
        {
            set { _imageurl = value; }
            get { return _imageurl; }
        }
        #endregion Model
    }
}
