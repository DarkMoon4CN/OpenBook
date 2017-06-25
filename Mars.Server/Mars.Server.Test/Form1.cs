using Mars.Server.Entity;
using Mars.Server.Test.Core;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mars.Server.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        AuthClient client = new AuthClient();
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.Text = "登录中";
            string loginname = textBox2.Text.Trim();
            string pwd = textBox3.Text.Trim();
            new Thread(() => {
                string sessionid = string.Empty;
                string json= client.TryLogin(loginname, pwd,out sessionid);
                this.Invoke(new MethodInvoker(() => {
                    textBox1.Text = json;
                    label7.Text = string.Format("会话SessionID:{0}",sessionid);
                    button1.Text = "登录";
                    if (string.IsNullOrEmpty(sessionid))
                    {
                        button1.Enabled = false;
                    }
                }));
            }).Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button2.Text = "执行中";
            string modulename = textBox4.Text.Trim();
            string action = textBox5.Text.Trim();

            if (modulename.ToLower() == "pic" && action.ToLower() == "upload")
            {
                if (comboBox1.SelectedIndex==1)
                {
                    MessageBox.Show("此接口不支持Get请求，切换成Post后重试");
                    button2.Text = "执行";
                    button2.Enabled = true;
                    return;
                }
                using (OpenFileDialog of = new OpenFileDialog())
                {
                    of.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.gif";
                    if (of.ShowDialog() == DialogResult.OK)
                    {
                        string localfilename = of.FileName;
                        new Thread(() =>
                        {
                            string json = client.UploadFile(modulename, action, localfilename);
                            this.Invoke(new MethodInvoker(() =>
                            {
                                textBox1.Text = json;
                                button2.Text = "执行";
                                button2.Enabled = true;
                            }));
                        }).Start();
                    }
                    else
                    {
                        button2.Text = "执行";
                        button2.Enabled = true;
                    }
                }
            }
            else
            {
                string data = textBox6.Text.Trim();
                int httpmethod = comboBox1.SelectedIndex;
                new Thread(() =>
                {
                    string json = client.DoAction(modulename, action, data, httpmethod);
                    this.Invoke(new MethodInvoker(() =>
                    {
                        textBox1.Text = json;
                        button2.Text = "执行";
                        button2.Enabled = true;
                    }));
                }).Start();
            }

          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text= MD5.Fun_MD5(textBox6.Text);
            //textBox1.Text = JsonObj<A>.ToJson(new A() { NameA = "aa", BObj = new B() {  NameB="bbb"} });
        }

        static string CodesString = "123456789abcdedfhijkmpqrstuvwxyz";
        public static string SendCode(string phone)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            StringBuilder code = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                code.Append(CodesString[r.Next(CodesString.Length)]);
            }
            return code.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FeedbackEntity fb = new FeedbackEntity();
            fb.Content = "test";
            fb.ContactMethod = "";
            textBox6.Text = JsonObj<FeedbackEntity>.ToJson(fb).ToLower();
            return;
            //textBox6.Text = SendCode("");
            //return;
            List<EventItemEntity> items = new List<EventItemEntity>();
            items.Add(new EventItemEntity()
            {
                CalendarTypeID = 1,
                Content = "test",
                CreateTime = DateTime.Now,
                EventItemGUID = Guid.NewGuid(),
                EventItemState = 1,
                //EventTypeID = 1,
                Remark = "remark",
                RepeatTypeID = 0,
                StartTime = new DateTime(2015, 7, 9),
                Title = "test title",
                UserID = 1,
                Locate = "20,345"
                ,
                Tags = new List<string>() { "tag1", "tag2" }
                ,
                Reminder = new ReminderEntity() { ReminderPreTime = 1, ReminderGUID = Guid.NewGuid() }
                ,
                Pics = new List<int>() { 234,12,24}
            });
            CalendarTypeEntity cc = new CalendarTypeEntity() { CalendarTypeID = 100, CalendarTypeName = "aaaaaaa" };
            Dictionary<CalendarTypeEntity, List<EventItemEntity>> dd = new Dictionary<CalendarTypeEntity, List<EventItemEntity>>();
            dd.Add(cc, items);
            textBox6.Text = JsonObj<Dictionary<CalendarTypeEntity, List<EventItemEntity>>>.ToJson(dd).ToLower();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btndelUser_Click(object sender, EventArgs e)
        {
            string json = client.DoAction("User", "ClearUser", "{'DelKey':'001001011011'}", 1);
            textBox1.Text = json;
        }
    }

    public class A
    {
        public string NameA { get; set; }
        public B BObj { get; set; }
    }
    public class B
    {
        public string NameB { get; set; }
    }
}
