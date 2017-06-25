using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp
{
    public partial class Common : System.Web.UI.MasterPage
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