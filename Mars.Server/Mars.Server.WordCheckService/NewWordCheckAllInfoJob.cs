using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Mars.Server.BLL.Comments;
using Mars.Server.BLL.Systems;
using Quartz;
using System.Data;
using System.Data.SqlClient;

namespace Mars.Server.WordCheckService
{
    public class NewWordCheckAllInfoJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(WordCheckJob));

        public void Execute(IJobExecutionContext context)
        {
            BCtrl_WordCheck wcBll = new BCtrl_WordCheck();
            Dictionary<int,string> wordList = wcBll.GetNewWordStringList();

            if (wordList != null)
            {
                List<string> wl = wordList.Values.ToList<string>();
                //获取需要检测的内容
                BCtrl_Comment bll = new BCtrl_Comment();

                try
                {
                    //构建两个table 一个成功，一个失败
                    //利用sqlbukcopy入库，然后执行批量更新数据
                    DataTable returnTable = new Common().GetDataTable();

                    SqlDataReader reader = bll.GetWillCheckCommentReader(true);
                    while (reader.Read())
                    {
                        DataRow drn = returnTable.NewRow();
                        drn["ID"] = int.Parse(reader["ID"].ToString());
                        drn["ContentType"] = int.Parse(reader["ContentType"].ToString());

                        string ws = "";
                        foreach (string word in wl)
                        {
                            if (reader["Content"].ToString().Contains(word))
                            {
                                ws += word + ",";
                            }
                        }
                        ws = ws.TrimEnd(',');
                        if (string.IsNullOrEmpty(ws))
                        {
                            drn["CheckTypeID"] = 1;
                        }
                        else
                        {
                            drn["CheckTypeID"] = -1;
                        }
                        drn["WordsInfo"] = ws;

                        returnTable.Rows.Add(drn);
                    }
                    reader.Close();

                    if (bll.WriteBack(returnTable))
                    {
                        bll.UpdateNewWordState(wordList);
                    }

                }
                catch (Exception ex)
                {
                    Utils.LogEntity log = new Utils.LogEntity()
                    {
                        LogContent = ex.Message,
                        LogMeta = "",
                        ExInfo = "",
                        LogTime = DateTime.Now,
                        LogTitle = "敏感词检测异常",
                        LogTypeID = 5,
                        UnqiueID = new Guid(),
                        UserID = 0
                    };
                    Utils.LogUtil.WriteLog(log);
                }

            }
        }
    }
}
