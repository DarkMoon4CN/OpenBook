using Mars.Server.BLL.Exhibition;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Exhibition
{
    public partial class _ActivityImport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitController();
            }
        }

        private void InitController()
        {
            BCtrl_Exhibition bll = new BCtrl_Exhibition();
            DataTable table = bll.GetExhibitionTable();
            if (table != null)
            {
                foreach (DataRow dr in table.Rows)
                {
                    this.sel_exhibition.Items.Add(new ListItem(dr["ExhibitionTitle"].ToString(), dr["ExhibitionID"].ToString()));
                }
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            StringBuilder strJs = new StringBuilder();
            strJs.Append("<script type='text/javascript'>");
            strJs.Append("bootbox.alert('导入失败');");
            strJs.Append("</script>");
            try
            {
                int eid = 0;
                int.TryParse(this.sel_exhibition.Value, out eid);
                if (this.file_activity.HasFile)
                {
                    string fileName = file_activity.FileName;
                    //string tempPath = AppDomain.CurrentDomain.BaseDirectory + "/UploadFile/" + "ExhibitorImport/";
                    string tempPath = Server.MapPath("~/UploadFile/ActivityImport/");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }
                    fileName = System.IO.Path.GetFileName(fileName); //获取文件名（不带路径）
                    string currFileExtension = System.IO.Path.GetExtension(fileName);//获取文件的扩展名
                    string currFilePath = tempPath + fileName;       //获取上传后的文件路径 记录到前面声明的全局变量
                    this.file_activity.SaveAs(currFilePath);


                    using (ExcelHelper excelHelper = new ExcelHelper(currFilePath))
                    {
                        DataTable table = excelHelper.ExcelToDataTable("Sheet1", true);
                        if (table != null)
                        {
                            BCtrl_Console_Activity bll = new BCtrl_Console_Activity();
                            if (bll.ImportActivitys(table, eid, this.CurrentAdmin.Sys_UserID))
                            {
                                strJs = strJs.Clear();
                                strJs.Append("<script type='text/javascript'>");
                                strJs.Append("bootbox.alert('导入成功', function () {");
                                strJs.Append(@"if (window.parent != undefined) {
                                    window.parent.TObj('tmpActivityList')._prmsData.ts = new Date().getTime();
                                    window.parent.TObj('tmpActivityList').S();
                                    window.parent.asyncbox.close('activityimport');
                                        }
                                    })");
                                strJs.Append("</script>");
                            }
                        }
                    }

                    if (File.Exists(currFilePath))
                    {
                        //如果存在则删除
                        File.Delete(currFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogUtil.WriteLog(ex);
                strJs = strJs.Clear();
                strJs.Append("<script type='text/javascript'>");
                strJs.Append("bootbox.alert('导入失败');");
                strJs.Append("</script>");
            }

            ClientScript.RegisterStartupScript(this.GetType(), "LoadPicScript", strJs.ToString());
        }
    }
}