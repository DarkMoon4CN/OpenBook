using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Mars.Server.Controls
{
    public class MyPlaceHolder : PlaceHolder
    {
        private string _Tag;

        public string Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }
    }
}
