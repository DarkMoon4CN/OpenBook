using Mars.Server.BLL.Exhibition;
using Mars.Server.Controls.BaseControl;
using Mars.Server.Entity.Exhibition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.Server.WebApp.Exhibition
{
    public partial class _ExhibitionEdit : BasePage
    {
        protected ExhibitionEntity item = new ExhibitionEntity();
        protected int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["id"], out id);
            if (!IsPostBack)
            {
                this.hid_eid.Value = id.ToString();
                if (id > 0)
                {
                    InitExhibition();
                }
            }
        }

        private void InitExhibition()
        {
            BCtrl_Exhibition bll = new BCtrl_Exhibition();
            item = bll.GetEntity(id);
            if (item != null)
            {
                this.txt_exhibitionname.Value = item.ExhibitionTitle;
                this.txt_exhibitionlocation.Value = item.ExhibitionLocation;
                this.txt_starttime.Value = item.ExhibitionStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                this.txt_endTime.Value = item.ExhibitionEndTime.ToString("yyyy-MM-dd HH:mm:ss");
                this.txt_exhibitionaddress.Value = item.ExhibitionAddress;
                this.txt_exhibitiontraffic.Value = item.ExhibitionTraffic;
                this.txt_exhibitionabstract.Value = item.ExhibitionAbstract;
                this.txt_exhibitionabout.Value = item.ExhibitionAbout;
                this.txt_exhibitionbooklistdesc.Value = item.ExhibitionBookListDesc;
                //this.txt_advertisementtitle.Value = item.AdvertisementList.ToString();
                if (item.AdvertisementList != null)
                {
                    string tmploc = "";
                    foreach (AdvertisementEntity adv in item.AdvertisementList)
                    {
                        tmploc += adv.AdvertisementTitle + "\n";
                    }
                    this.txt_advertisementtitle.Value = tmploc;
                }

                this.chk_statetype.Checked = item.StateTypeID == 1;
                this.chk_ispublish.Checked = item.IsPublish;
                this.chk_isdownloadbooklist.Checked = item.IsDownloadBookList;

                this.txt_booklistdownloadurl.Value = item.BookListDownloadUrl;

                this.hid_eid.Value = item.ExhibitionID.ToString();
                this.hid_imgurl.Value = item.ExhibitionLogoUrl;
            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            StringBuilder strJs = new StringBuilder();
            strJs.Append("<script type='text/javascript'>");
            strJs.Append("bootbox.alert('保存失败');");
            strJs.Append("</script>");

            try
            {
                #region 填充
                int eid = 0;
                int.TryParse(this.hid_eid.Value, out eid);
                ExhibitionEntity item = new ExhibitionEntity() {
                    ExhibitionID = eid,
                    ExhibitionTitle = this.txt_exhibitionname.Value,
                    ExhibitionLocation = this.txt_exhibitionlocation.Value,
                    ExhibitionStartTime = Convert.ToDateTime(this.txt_starttime.Value),
                    ExhibitionEndTime = Convert.ToDateTime(this.txt_endTime.Value),
                    ExhibitionAddress = this.txt_exhibitionaddress.Value,
                    ExhibitionTraffic = this.txt_exhibitiontraffic.Value,
                    ExhibitionAbstract = this.txt_exhibitionabstract.Value,
                    ExhibitionAbout = this.txt_exhibitionabout.Value,
                    ExhibitionBookListDesc = this.txt_exhibitionbooklistdesc.Value,
                    BookListDownloadUrl = this.txt_booklistdownloadurl.Value,
                    CreateTime = DateTime.Now,
                    CreateUserID = this.CurrentAdmin.Sys_UserID,
                    ExhibitionOrder = 1000,
                    StateTypeID = this.chk_statetype.Checked ? 1 : 0,
                    IsPublish = this.chk_ispublish.Checked,
                    IsDownloadBookList = this.chk_isdownloadbooklist.Checked
                };
                item.AdvertisementList = new List<AdvertisementEntity>();

                string[] elocs = this.txt_advertisementtitle.Value.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (elocs != null)
                {
                    foreach (string loc in elocs)
                    {
                        AdvertisementEntity locEntity = new AdvertisementEntity()
                        {
                            ExhibitionID = eid,
                            CreateTime = DateTime.Now,
                            CreateUserID = this.CurrentAdmin.Sys_UserID,
                            AdvertisementOrder = 1000,
                            AdvertisementTitle = loc,
                            AdvertisementUrl = "",
                            StateTypeID = this.chk_statetype.Checked ? 1 : 0
                        };
                        item.AdvertisementList.Add(locEntity);
                    }
                }
                #endregion

                if (this.txt_imageurl.HasFile)
                {
                    string fileName = txt_imageurl.FileName;
                    string tempPath = Server.MapPath("~/UploadFile/ExhibitionLogo/");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }
                    fileName = System.IO.Path.GetFileName(fileName); //获取文件名（不带路径）
                    string currFileExtension = System.IO.Path.GetExtension(fileName);//获取文件的扩展名
                    string currFilePath = tempPath + fileName;       //获取上传后的文件路径 记录到前面声明的全局变量
                    this.txt_imageurl.SaveAs(currFilePath);
                    item.ExhibitionLogoUrl = "/UploadFile/ExhibitionLogo/"+ fileName;
                }
                else
                {
                    item.ExhibitionLogoUrl = hid_imgurl.Value;
                }

                BCtrl_Exhibition bll = new BCtrl_Exhibition();

                if (bll.Save(item))
                {
                    strJs = strJs.Clear();
                    strJs.Append("<script type='text/javascript'>");
                    strJs.Append("bootbox.alert('保存成功', function () {");
                    strJs.Append(@"if (window.parent != undefined) {
                                    window.parent.TObj('tmpExhibitionList')._prmsData.ts = new Date().getTime();
                                    window.parent.TObj('tmpExhibitionList').S();
                                    window.parent.asyncbox.close('exhibition');
                                        }
                                    })");
                    strJs.Append("</script>");
                }
            }
            catch (Exception ex)
            {
                Utils.LogUtil.WriteLog(ex);
                strJs = strJs.Clear();
                strJs.Append("<script type='text/javascript'>");
                strJs.Append("bootbox.alert('保存失败');");
                strJs.Append("</script>");
            }

            ClientScript.RegisterStartupScript(this.GetType(), "LoadExhibitionScript", strJs.ToString());
        }
    }

}