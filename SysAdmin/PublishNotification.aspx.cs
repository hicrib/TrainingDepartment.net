using AjaxControlToolkit;
using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.SysAdmin
{
    public partial class PublishNotification : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("PUBLISH NOTIFICATION");
                Session["file1"] = null;
                Session["file2"] = null;
                Session["file3"] = null;
                Session["file4"] = null;
            }
        }

        protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_type.SelectedValue == "BROADCAST")
            {
                ddl_generic.Visible = false;
            }
            else if (ddl_type.SelectedValue == "POSITION")
            {
                ddl_generic.DataSource = DB_System.get_Positions();
                ddl_generic.DataValueField = "CODE";
                ddl_generic.DataBind();
                ddl_generic.Items.RemoveAt(0);
                ddl_generic.Visible = true;
            }
            else if (ddl_type.SelectedValue == "SECTOR")
            {
                ddl_generic.DataSource = DB_System.get_Sectors();
                ddl_generic.DataTextField = "CODE";
                ddl_generic.DataValueField = "SECT";
                ddl_generic.DataBind();
                ddl_generic.Items.RemoveAt(0);
                ddl_generic.Visible = true;
            }
            else if (ddl_type.SelectedValue == "ROLE")
            {
                ddl_generic.DataSource = DB_System.get_ALL_Roles();
                ddl_generic.DataValueField = "ID";
                ddl_generic.DataTextField = "NAME";
                ddl_generic.DataBind();
                ddl_generic.Visible = true;
            }

        }

        protected void fileupload_UploadStart(object sender, AjaxFileUploadStartEventArgs e)
        {
            lbl_pageresult.Text = "Wait until upload finishes";
            lbl_pageresult.Visible = true;
            btn_publish.Visible = false;
        }
        protected void fileupload_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            btn_publish.Visible = true;
            lbl_pageresult.Text = "";
            lbl_pageresult.Visible = false;

            string generatedFileName = Utility.getRandomFileName() + e.FileName.ToString();
            string fileNamewithadress = Server.MapPath("~/AzureBlobs/Uploads/") + generatedFileName;

            fileupload.SaveAs(fileNamewithadress);

            if (AzureCon.upload_ToBlob_fromFile(fileNamewithadress))
            {
                if (String.IsNullOrEmpty((string)Session["file1"]))
                {
                    Session["file1"] = AzureCon.general_container_url + generatedFileName;
                }
                else if (String.IsNullOrEmpty((string)Session["file2"]))
                {
                    Session["file2"] = AzureCon.general_container_url + generatedFileName;
                }
                else if (String.IsNullOrEmpty((string)Session["file3"]))
                {
                    Session["file3"] = AzureCon.general_container_url + generatedFileName;
                }
                else if (String.IsNullOrEmpty((string)Session["file4"]))
                {
                    Session["file4"] = AzureCon.general_container_url + generatedFileName;
                }
            }
        }

        protected void btn_publish_Click(object sender, EventArgs e)
        {
            if (txt_header.Text.Trim() == "")
            {
                lbl_pageresult.Text = "Header can't be empty";
                lbl_pageresult.Visible = true;
                return;
            }
            if (txt_message.Text.Trim() == "")
            {
                lbl_pageresult.Text = "Message can't be empty";
                lbl_pageresult.Visible = true;
                return;
            }

            List<string> filelist = new List<string>();
            filelist.Add(Convert.ToString(Session["file1"] as object));
            filelist.Add(Convert.ToString(Session["file2"] as object));
            filelist.Add(Convert.ToString(Session["file3"] as object));
            filelist.Add(Convert.ToString(Session["file4"] as object));

            if (DB_System.publish_notification(ddl_type.SelectedValue, ddl_generic.SelectedValue,
                                            txt_header.Text, txt_message.Text, filelist,
                                            txt_effective.Text, txt_expires.Text))
            {
                lbl_pageresult.Visible = true;
                lbl_pageresult.Text = "Success : Notification published!";
                Session["file1"] = null;
                Session["file2"] = null;
                Session["file3"] = null;
                Session["file4"] = null;
                txt_effective.Text = "";
                txt_expires.Text = "";
                txt_header.Text = "";
                txt_message.Text = "";
            }

        }


    }
}