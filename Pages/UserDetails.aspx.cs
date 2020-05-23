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
                {
                    panel_selectuser.Visible = true;
                    pane_roles.Visible = true;
                }

                fill_page();
            }
            else
            {
                //UserSession user = (UserSession)Session["usersession"];
                //if (user.employeeid == ddl_user.SelectedValue)
                //{
                //    UserSession uptodate = new UserSession(ddl_user.SelectedValue);
                //    Session["usersession"] = uptodate;

                //    fill_page();
                //}
            }

        }
        protected void fill_page()
        {
            if (ddl_user.SelectedValue == "0")
                return;

            UserSession u = new UserSession(ddl_user.SelectedValue);
            txt_email.Text = u.email;
            img_userphoto.ImageUrl = AzureCon.general_container_url + u.photo;
            img_signature.ImageUrl = AzureCon.general_container_url + u.signature;

            lbl_initial.Text = u.initial;
            lbl_Name.Text = u.name_surname;
            lbl_email.Text = u.email;
            lbl_userid.Text = u.employeeid;

            list_roles.DataSource = u.role_priv;
            list_roles.DataValueField = "ROLENAME";
            list_roles.DataBind();

            Fill_Training_Folder();


            tree.Nodes.Clear();
            DataRow dr = DB_FileSys.get_UserMain(ddl_user.SelectedValue);
            if (dr == null)
                return; //this shouldn't happen as it creates if there is none

            TreeNode mainnode = new TreeNode(dr["NAME"].ToString(), dr["ID"].ToString());
            tree.Nodes.Add(mainnode);
            tree.Nodes[0].Selected = true;
            tree_SelectedNodeChanged1(new object(), new EventArgs());
        }

        protected void Fill_Training_Folder()
        {
            DataTable utf = DB_Reports.UserDetails_TrainingFolder(ddl_user.SelectedValue);

            if (utf == null || utf.Rows.Count == 0)
                return;

            //tower 
            DataRow[] twr_assist = utf.Select("POSITION = 'TWR' AND SECTOR IS null ");
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
            DataRow[] app_fdo = utf.Select("POSITION = 'APP' AND  (PHASE = 'FDO' or PHASE = 'PREOJT' )");
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
            DataRow[] acc_fdo = utf.Select("POSITION = 'ACC' AND ( PHASE = 'FDO' or  PHASE = 'PREOJT' ) ");
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
                lbl_passchange.Visible = true;
                return;
            }
            if (txt_pass1.Text.Length < 6)
            {
                lbl_passchange.Text = "Minimum 6 characters";
                lbl_passchange.Visible = true;
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
                //write db
                if (afu.ID == "fileupload_photo")
                {
                    if (DB_System.update_UserInfo(ddl_user.SelectedValue, photo: generatedFileName))
                        img_userphoto.ImageUrl = AzureCon.general_container_url + generatedFileName;

                }
                else if (afu.ID == "fileupload_signature")
                {
                    if (DB_System.update_UserInfo(ddl_user.SelectedValue, signature: generatedFileName))
                        img_signature.ImageUrl = AzureCon.general_container_url + generatedFileName;
                }
                //else if (afu.ID == "fileupload_cert_academy")
                //{
                //    if (DB_System.update_User_Certificates(user.employeeid, academy: generatedFileName))
                //        img_cert_academy.ImageUrl = AzureCon.general_container_url + generatedFileName;
                //}
                //else if (afu.ID == "fileupload_ojtcourse")
                //{
                //    if (DB_System.update_User_Certificates(user.employeeid, ojtcourse: generatedFileName))
                //        img_ojtcourse.ImageUrl = AzureCon.general_container_url + generatedFileName;
                //}
                //else if (afu.ID == "fileupload_supcourse")
                //{
                //    if (DB_System.update_User_Certificates(user.employeeid, supcourse: generatedFileName))
                //        img_supcourse.ImageUrl = AzureCon.general_container_url + generatedFileName;
                //}
                //else if (afu.ID == "fileupload_ECT")
                //{
                //    if (DB_System.update_User_Certificates(user.employeeid, ECT: generatedFileName))
                //        img_ECT.ImageUrl = AzureCon.general_container_url + generatedFileName;
                //}
                //else if (afu.ID == "fileupload_trnCert_1")
                //{
                //    if (DB_System.update_User_Certificates(user.employeeid, training: generatedFileName))
                //        img_trnCert_1.ImageUrl = AzureCon.general_container_url + generatedFileName;
                //}
                //else if (afu.ID == "fileupload_equiptest")
                //{
                //    if (DB_System.update_User_Certificates(user.employeeid, equiptest: generatedFileName))
                //        img_equiptest.ImageUrl = AzureCon.general_container_url + generatedFileName;
                //}
                //else if (afu.ID == "fileupload_OJTPermit")
                //{
                //    if (DB_System.update_User_Certificates(user.employeeid, ojtpermit: generatedFileName))
                //        img_OJTPermit.ImageUrl = AzureCon.general_container_url + generatedFileName;
                //}
            }
        }








        protected void tree_SelectedNodeChanged1(object sender, EventArgs e)
        {

            DataTable gridtbl = new DataTable();
            gridtbl.Columns.Add("ID");
            gridtbl.Columns.Add("NAME");
            gridtbl.Columns.Add("FILEID");
            gridtbl.Columns.Add("ADDRESS");
            gridtbl.Columns.Add("Issued");
            gridtbl.Columns.Add("Expires");
            gridtbl.Columns.Add("Roles");
            gridtbl.Columns.Add("Type");

            lbl_foldername.Text = tree.SelectedNode.Text;

            DataTable dt = DB_FileSys.get_ChildNodes(tree.SelectedValue);
            if (dt == null) //no child
            {
                //just the refresh
                grid_userfiles.DataSource = gridtbl;
                grid_userfiles.DataBind();
                return;
            }

            tree.SelectedNode.ChildNodes.Clear();

            //find the child folders - add to tree
            DataRow[] c_folders = dt.Select("FILEID = 0 ");
            if (c_folders != null && c_folders.Length > 0)
            {
                foreach (DataRow row in c_folders)
                {
                    TreeNode n = new TreeNode(row["NAME"].ToString(), row["ID"].ToString());
                    n.ImageUrl = "~/images/folder.png";
                    tree.SelectedNode.ChildNodes.Add(n);

                    DataRow filerow = gridtbl.NewRow();
                    filerow["ID"] = row["ID"];
                    filerow["NAME"] = row["NAME"];
                    filerow["FILEID"] = row["FILEID"];
                    filerow["ADDRESS"] = row["ADDRESS"];
                    filerow["Issued"] = row["Issued"];
                    filerow["Expires"] = row["Expires"];
                    filerow["Roles"] = row["Roles"];
                    filerow["Type"] = row["Type"];
                    gridtbl.Rows.Add(filerow);
                }
            }

            DataRow[] c_files = dt.Select("FILEID <> 0");
            if (c_files != null && c_files.Length > 0)
            {
                //adding a header
                DataRow headerrow = gridtbl.NewRow();
                headerrow["ID"] = "HEADER";
                headerrow["NAME"] = "NAME";
                headerrow["FILEID"] = "HEADER";
                headerrow["ADDRESS"] = "";
                headerrow["Issued"] = "Issued";
                headerrow["Expires"] = "Expires";
                headerrow["Roles"] = "Roles";
                headerrow["Type"] = "Type";
                gridtbl.Rows.Add(headerrow);


                foreach (DataRow row in c_files)
                {
                    //TreeNode n = new TreeNode(row["NAME"].ToString(), row["ID"].ToString());
                    //n.NavigateUrl = AzureCon.general_container_url + row["ADDRESS"].ToString();
                    //n.Target = "_blank";
                    //n.ImageUrl = "~/images/view.png";
                    //tree.SelectedNode.ChildNodes.Add(n);

                    DataRow filerow = gridtbl.NewRow();
                    filerow["ID"] = row["ID"];
                    filerow["NAME"] = row["NAME"];
                    filerow["FILEID"] = row["FILEID"];
                    filerow["ADDRESS"] = row["ADDRESS"];
                    filerow["Issued"] = row["Issued"];
                    filerow["Expires"] = row["Expires"];
                    filerow["Roles"] = row["Roles"];
                    filerow["Type"] = row["Type"];
                    gridtbl.Rows.Add(filerow);
                }
            }
            tree.SelectedNode.Expand();

            grid_userfiles.DataSource = gridtbl;
            grid_userfiles.DataBind();

        }


        protected void btn_newFolder_Click(object sender, EventArgs e)
        {
            if (txt_newfoldername.Text == "")
                return;

            if (DB_FileSys.create_NewFolder(txt_newfoldername.Text, ddl_user.SelectedValue, tree.SelectedNode.Value))
            {
                //update the node
                tree_SelectedNodeChanged1(new object(), new EventArgs());
                pnl_createfolder.Visible = false;

            }
        }
        protected void icon_newfolder_Click(object sender, ImageClickEventArgs e)
        {
            pnl_createfolder.Visible = !pnl_createfolder.Visible; //toggle
            if (pnl_createfolder.Visible)
                pnl_createFile.Visible = false; //oppposite
        }
        protected void icon_newfile_Click(object sender, ImageClickEventArgs e)
        {
            pnl_createFile.Visible = !pnl_createFile.Visible;
            if (pnl_createFile.Visible)
                pnl_createfolder.Visible = false;

            ddl_filetypes.DataSource = DB_FileSys.get_filetypes();
            ddl_filetypes.DataTextField = "Name";
            ddl_filetypes.DataValueField = "ID";
            ddl_filetypes.DataBind();
            ddl_filetypes_SelectedIndexChanged(new object(), new EventArgs());
        }

        protected void ddl_filetypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = DB_FileSys.get_filetypes();
            if (dt == null || dt.Rows.Count == 0)
                return;

            DataRow type = dt.Select("ID = " + ddl_filetypes.SelectedValue)[0];

            pnl_issuedate.Visible = type["Issue Date"].ToString() == "True";
            lbl_issue.Visible = pnl_issuedate.Visible;


            pnl_expires.Visible = type["Expires"].ToString() == "True";
            lbl_expir.Visible = pnl_expires.Visible;

            if (type["Role Specific"].ToString() == "True")
            {
                pnl_rolespec.Visible = true;
                lbl_rolespec.Visible = true;
                list_rolespec.DataSource = DB_System.get_ALL_Roles();
                list_rolespec.DataTextField = "NAME";
                list_rolespec.DataValueField = "ID";
                list_rolespec.DataBind();
            }
            else
            {
                pnl_rolespec.Visible = false;
                lbl_rolespec.Visible = false;
            }

        }
        protected void btn_uploadnewfile_Click(object sender, EventArgs e)
        {
            if (uploadnewfile.HasFile)
            {
                try
                {
                    string filename = Utility.getRandomFileName();
                    string newfilename = filename + uploadnewfile.PostedFile.FileName;
                    string file_address = Server.MapPath("~/AzureBlobs/Uploads/") + newfilename;
                    uploadnewfile.SaveAs(file_address);

                    //will be pushed to db
                    cloudfilename.Text = newfilename; //new adress
                    txt_newfilename.Text = uploadnewfile.PostedFile.FileName; //file's friendly name 

                    btn_createNewFile.Visible = true;
                    txt_newfilename.Visible = true;

                    if (!AzureCon.upload_ToBlob_fromFile(file_address))
                    {
                        //todo : what to do when error
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    cloudfilename.Text = "Error Occured!";
                    cloudfilename.Visible = true;
                }
            }
        }
        protected void btn_createNewFile_Click(object sender, EventArgs e)
        {
            if (txt_newfilename.Text == "")
                return;
            if (pnl_issuedate.Visible && txt_issuedate.Text == "")
                return;
            if (pnl_expires.Visible && txt_expires.Text == "")
                return;
            if (pnl_rolespec.Visible && list_rolespec.GetSelectedIndices().Length == 0)
                return;
            if (cloudfilename.Text == "Error Occured!")
                return;

            string rolespec = "";
            if (pnl_rolespec.Visible)
            {
                foreach (int item in list_rolespec.GetSelectedIndices())
                {
                    rolespec += item.ToString() + ",";
                }
            }


            bool uploaded = DB_FileSys.upload_UserFile(ddl_user.SelectedValue, tree.SelectedValue, ddl_filetypes.SelectedValue, txt_newfilename.Text,
                                         cloudfilename.Text, txt_issuedate.Text, txt_expires.Text, rolespec);

            if (uploaded)
            {
                txt_newfilename.Text = "";
                txt_issuedate.Text = "";
                txt_expires.Text = "";
                list_rolespec.ClearSelection();
                cloudfilename.Text = "";
                pnl_expires.Visible = false;
                pnl_issuedate.Visible = false;
                pnl_rolespec.Visible = false;
                pnl_createFile.Visible = false;

                tree_SelectedNodeChanged1(new object(), new EventArgs());
            }

        }



        protected void grid_userfiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 3)
            {
                // BUTTON , ID, [NAME], 'FILEID' , 'ADDRESS', issued, expires, roles, type
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[7].Visible = false; //roles

                if (e.Row.Cells[1].Text == "HEADER")
                {
                    e.Row.Style.Add("font-weight", "bold");
                    e.Row.Style.Add("font-size", "small");
                    e.Row.Style.Add("text-align", "center");
                    e.Row.Style.Add("background-color", "#a52a2a");
                    e.Row.Style.Add("color", "white");
                }
            }
        }
        protected string go_url(string fileid)
        {
            if (fileid == "" || fileid == "0")
                return "~/images/folder.png";

            if (fileid == "HEADER")
                return "";

            return "~/images/view.png";
        }
        protected bool show_go(string issued)
        {
            if (issued.ToLower() == "issued")
                return false;

            return true;
        }
        protected bool show_delete(string fileid, string issued)
        {
            if (issued == "Issued")
                return false;

            if (fileid == "" || fileid == "0")
                return false;

            UserSession user = (UserSession)Session["usersession"];
            if (user.isAdmin)
                return true;

            return false;
        }

        protected void grid_userfiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GO")
            {
                GridViewRow selectedRow = ((GridViewRow)(((ImageButton)e.CommandSource).NamingContainer));

                if (selectedRow.Cells[3].Text == "0") // meaning folder
                {
                    //get filesystemid and select that node
                    string fs_id = selectedRow.Cells[1].Text;
                    TreeNode go_node = tree.FindNode(tree.SelectedNode.ValuePath + "/" + fs_id);
                    if (go_node == null)
                        return;

                    go_node.Select();
                    tree_SelectedNodeChanged1(new object(), new EventArgs());
                }
                else //it's a file 
                {
                    //get address , send new tab
                    string address = selectedRow.Cells[4].Text;
                    string url = AzureCon.general_container_url + address;

                    //Page.ClientScript.RegisterStartupScript( this.GetType(), "OpenWindow", "window.open('"+url+"','_blank');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "open('" + url + "') ;", true);
                }

            }
            else if (e.CommandName == "DEL")
            {
                GridViewRow selectedRow = ((GridViewRow)(((ImageButton)e.CommandSource).NamingContainer));
                string fs_id = selectedRow.Cells[1].Text;

                if (DB_FileSys.delete_File(fs_id))
                {
                    grid_userfiles.DeleteRow(selectedRow.RowIndex);

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

        protected void grid_userfiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            tree_SelectedNodeChanged1(new object(), new EventArgs());
        }
    }
}