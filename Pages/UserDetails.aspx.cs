using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using System.IO;
using AjaxControlToolkit;

namespace AviaTrain.Pages
{
    public partial class UserDetails : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //QueryString ID
                //if no id - > unauth
                //id != userid and not admin -> unauth


                Write_Page_Header_Low("USER DETAILS");

                fill_page();
            }
            else
            {
                UserSession user = (UserSession)Session["usersession"];
                UserSession uptodate = new UserSession(user.employeeid);
                Session["usersession"] = uptodate;
                fill_page();
            }




        }
        protected void fill_page()
        {
            UserSession u = (UserSession)Session["usersession"];
            txt_email.Text = u.email;
            img_userphoto.ImageUrl = AzureCon.general_container_url + u.photo;
            img_signature.ImageUrl = AzureCon.general_container_url + u.signature;
        }

        protected void btn_update_email_Click(object sender, EventArgs e)
        {
            img_email_result.Visible = true;

            if (txt_email.Text == "")
            {
                img_email_result.ImageUrl = "~/images/cross_red_small.png";

                return;
            }
            UserSession user = (UserSession)Session["usersession"];
            if (!DB_System.update_UserInfo(user.employeeid, email: txt_email.Text))
                img_email_result.ImageUrl = "~/images/cross_red_small.png";
            else
                img_email_result.ImageUrl = "~/images/tick_green_small.png";
        }

        protected void btn_changepassword_Click(object sender, EventArgs e)
        {
            if (txt_pass1.Text == "" || txt_pass2.Text == "" || txt_pass1.Text != txt_pass2.Text)
            {
                lbl_passchange.Style.Add("color", "red");
                lbl_passchange.Text = "Password should match";
                if (txt_pass1.Text.Length < 6)
                    lbl_passchange.Text = "Minimum 6 characters";
                return;
            }

            UserSession user = (UserSession)Session["usersession"];
            if (!DB_System.update_UserInfo(user.employeeid, password: txt_pass1.Text))
            {
                lbl_passchange.Style.Add("color", "green");
                lbl_passchange.Text = "Try again";
            }
            else
            {
                lbl_passchange.Style.Add("color", "red");
                lbl_passchange.Text = "Password changed";
            }

        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            int index = Int32.Parse(e.Item.Value);
            multiview1.ActiveViewIndex = index;
        }

        protected void fileupload_photo_UploadComplete(object sender, AjaxFileUploadEventArgs e)
        {
            string generatedFileName = Utility.getRandomFileName() + e.FileName.ToString();
            string fileNametoupload = Server.MapPath("~/AzureBlobs/Uploads/") + generatedFileName;
            
            AjaxFileUpload afu = (AjaxFileUpload)sender;
            afu.SaveAs(fileNametoupload);

            if (AzureCon.upload_ToBlob_fromFile(fileNametoupload))
            {
                UserSession user = (UserSession)Session["usersession"];

                //write db
                if (afu.ID == "fileupload_photo")
                {
                    if (DB_System.update_UserInfo(user.employeeid, photo: generatedFileName))
                        img_userphoto.ImageUrl = AzureCon.general_container_url + generatedFileName;

                }
                else if(afu.ID == "fileupload_signature")
                {
                    if (DB_System.update_UserInfo(user.employeeid, signature: generatedFileName))
                        img_signature.ImageUrl = AzureCon.general_container_url + generatedFileName;
                }
                else if (afu.ID == "fileupload_cert_academy")
                {
                    if(DB_System.update_User_Certificates(user.employeeid, academy: generatedFileName))
                        img_cert_academy.ImageUrl = AzureCon.general_container_url + generatedFileName;
                }
                else if (afu.ID == "fileupload_ojtcourse")
                {
                    if (DB_System.update_User_Certificates(user.employeeid, ojtcourse: generatedFileName))
                        img_ojtcourse.ImageUrl = AzureCon.general_container_url + generatedFileName;
                }
                else if (afu.ID == "fileupload_supcourse")
                {
                    if (DB_System.update_User_Certificates(user.employeeid, supcourse: generatedFileName))
                        img_supcourse.ImageUrl = AzureCon.general_container_url + generatedFileName;
                }
                else if (afu.ID == "fileupload_ECT")
                {
                    if (DB_System.update_User_Certificates(user.employeeid, ECT: generatedFileName))
                        img_ECT.ImageUrl = AzureCon.general_container_url + generatedFileName;
                }
                else if (afu.ID == "fileupload_trnCert_1")
                {
                    if (DB_System.update_User_Certificates(user.employeeid, training: generatedFileName))
                        img_trnCert_1.ImageUrl = AzureCon.general_container_url + generatedFileName;
                }
                else if (afu.ID == "fileupload_equiptest")
                {
                    if (DB_System.update_User_Certificates(user.employeeid, equiptest: generatedFileName))
                        img_equiptest.ImageUrl = AzureCon.general_container_url + generatedFileName;
                }
                else if (afu.ID == "fileupload_OJTPermit")
                {
                    if (DB_System.update_User_Certificates(user.employeeid, ojtpermit: generatedFileName))
                        img_OJTPermit.ImageUrl = AzureCon.general_container_url + generatedFileName;
                }
            }
        }

        


















        protected void btn_hidden_refresh_Click(object sender, EventArgs e)
        {

        }

    }
}