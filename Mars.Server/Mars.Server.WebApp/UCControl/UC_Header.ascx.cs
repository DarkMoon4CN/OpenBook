using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.Server.Controls.BaseControl;
using System.Net;

namespace Mars.Server.WebApp.UCControl
{
    public partial class UC_Header : UserControlBase
    {
        public string title = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strHostName = Dns.GetHostName();
            System.Net.IPAddress[] addressList = Dns.GetHostEntry(strHostName).AddressList;

            if (addressList.Length > 1)
            {
                string serverHost = Request.Url.Host;
                if (addressList[0].ToString() == "192.168.10.15" || serverHost == "192.168.10.15" || serverHost == "localhost")
                {
                    title = "测试";
                }
                else
                {
                    title = "控制台";
                }
            }
        }
    }
}