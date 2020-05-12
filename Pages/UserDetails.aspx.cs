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
using System.Data;

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

                ddl_user.DataSource = DB_System.get_ALL_trainees();
                ddl_user.DataTextField = "NAME";
                ddl_user.DataValueField = "ID";
                ddl_user.DataBind();

                UserSession user = (UserSession)Session["usersession"];
                ddl_user.SelectedValue = user.employeeid;

                if (user.isAdmin)
                    panel_selectuser.Visible = true;


                fill_page();
            }
            else
            {
                UserSession user = (UserSession)Session["usersession"];
                if (user.employeeid == ddl_user.SelectedValue)
                {
                    UserSession uptodate = new UserSession(ddl_user.SelectedValue);
                    Session["usersession"] = uptodate;
                }

                fill_page();
            }




        }
        protected void fill_page()
        {
            UserSession u = new UserSession(ddl_user.SelectedValue);
            txt_email.Text = u.email;
            img_userphoto.ImageUrl = AzureCon.general_container_url + u.photo;
            img_signature.ImageUrl = AzureCon.general_container_url + u.signature;


            Fill_Training_Folder();
        }

        protected void Fill_Training_Folder()
        {
            DataTable utf = DB_Reports.UserDetails_TrainingFolder(ddl_user.SelectedValue);

            if (utf == null || utf.Rows.Count == 0)
                return;

            //tower 
            DataRow[] twr_assist = utf.Select("POSITION = 'TWR' AND SECTOR = 'ASSIST'");
            if (twr_assist.Length > 0)
            {
                grid_TWR_ASSIST.DataSource = twr_assist.CopyToDataTable();
                grid_TWR_ASSIST.DataBind();
            }
            DataRow[] twr_gmc = utf.Select("POSITION = 'TWR' AND SECTOR = 'GMC'");
            if (twr_gmc.Length > 0)
            {
                grid_TWR_GMC.DataSource = twr_gmc.CopyToDataTable();
                grid_TWR_GMC.DataBind();
            }
            DataRow[] twr_adc = utf.Select("POSITION = 'TWR' AND SECTOR = 'ADC'");
            if (twr_adc.Length > 0)
            {
                grid_TWR_ADC.DataSource = twr_adc.CopyToDataTable();
                grid_TWR_ADC.DataBind();
            }


            //APP
            DataRow[] app_fdo = utf.Select("POSITION = 'APP' AND PHASE = 'FDO'");
            if (app_fdo.Length > 0)
            {
                grid_APP_fdo.DataSource = app_fdo.CopyToDataTable();
                grid_APP_fdo.DataBind();
            }
            DataRow[] app_assist = utf.Select("POSITION = 'APP' AND PHASE = 'ASSIST'");
            if (app_assist.Length > 0)
            {
                grid_APP_ASSIST.DataSource = app_assist.CopyToDataTable();
                grid_APP_ASSIST.DataBind();
            }
            DataRow[] app_AR = utf.Select("POSITION = 'APP' AND SECTOR = 'AR'");
            if (app_AR.Length > 0)
            {
                grid_APP_AR.DataSource = app_AR.CopyToDataTable();
                grid_APP_AR.DataBind();
            }
            DataRow[] app_BR = utf.Select("POSITION = 'APP' AND SECTOR = 'BR'");
            if (app_BR.Length > 0)
            {
                grid_APP_BR.DataSource = app_BR.CopyToDataTable();
                grid_APP_BR.DataBind();
            }
            DataRow[] app_KR = utf.Select("POSITION = 'APP' AND SECTOR = 'KR'");
            if (app_KR.Length > 0)
            {
                grid_APP_KR.DataSource = app_KR.CopyToDataTable();
                grid_APP_KR.DataBind();
            }


            //ACC
            DataRow[] acc_fdo = utf.Select("POSITION = 'ACC' AND PHASE = 'FDO'");
            if (acc_fdo.Length > 0)
            {
                grid_ACC_fdo.DataSource = acc_fdo.CopyToDataTable();
                grid_ACC_fdo.DataBind();
            }
            DataRow[] acc_assist = utf.Select("POSITION = 'ACC' AND PHASE = 'ASSIST'");
            if (acc_assist.Length > 0)
            {
                grid_ACC_ASSIST.DataSource = acc_assist.CopyToDataTable();
                grid_ACC_ASSIST.DataBind();
            }
            DataRow[] acc_SR = utf.Select("POSITION = 'ACC' AND SECTOR = 'SR'");
            if (acc_SR.Length > 0)
            {
                grid_ACC_SR.DataSource = acc_SR.CopyToDataTable();
                grid_ACC_SR.DataBind();
            }
            DataRow[] acc_NR = utf.Select("POSITION = 'ACC' AND SECTOR = 'NR'");
            if (acc_NR.Length > 0)
            {
                grid_ACC_NR.DataSource = acc_NR.CopyToDataTable();
                grid_ACC_NR.DataBind();
            }
            DataRow[] acc_CR = utf.Select("POSITION = 'ACC' AND SECTOR = 'CR'");
            if (acc_CR.Length > 0)
            {
                grid_ACC_CR.DataSource = acc_CR.CopyToDataTable();
                grid_ACC_CR.DataBind();
            }


        }

        protected void btn_update_email_Click(object sender, EventArgs e)
        {
            img_email_result.Visible = true;

            if (txt_email.Text == "")
            {
                img_email_result.ImageUrl = "~/images/cross_red_small.png";

                return;
            }

            if (!DB_System.update_UserInfo(ddl_user.SelectedValue, email: txt_email.Text))
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


            if (!DB_System.update_UserInfo(ddl_user.SelectedValue, password: txt_pass1.Text))
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
                else if (afu.ID == "fileupload_signature")
                {
                    if (DB_System.update_UserInfo(user.employeeid, signature: generatedFileName))
                        img_signature.ImageUrl = AzureCon.general_container_url + generatedFileName;
                }
                else if (afu.ID == "fileupload_cert_academy")
                {
                    if (DB_System.update_User_Certificates(user.employeeid, academy: generatedFileName))
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

        protected void ddl_user_SelectedIndexChanged(object sender, EventArgs e)
        {
            fill_page();
        }
    }
}