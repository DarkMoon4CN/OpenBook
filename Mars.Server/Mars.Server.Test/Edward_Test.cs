using Mars.Server.BLL.Exhibition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mars.Server.Test
{
    public partial class Edward_Test : Form
    {
        public Edward_Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BCtrl_Exhibition bll = new BCtrl_Exhibition();
            MessageBox.Show(bll.IsPublished().ToString());
        }
    }
}
