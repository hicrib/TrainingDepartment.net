using AviaTrain.App_Code;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.SysAdmin
{
    public partial class CreateTrainingFolder1 : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("CREATE TRAINING FOLDER");

                Fill_Page_Elements();
            }
        }

        protected void Fill_Page_Elements()
        {
            DataTable all_users = DB_System.get_ALL_trainees();
            if (all_users != null)
            {
                ddl_all_trainees.DataSource = all_users;
                ddl_all_trainees.DataTextField = "NAME";
                ddl_all_trainees.DataValueField = "ID";
                ddl_all_trainees.DataBind();
            }
        }

        protected void ddl_position_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_position.SelectedValue == "-")
            {
                ddl_sector.DataSource = new DataTable();
                ddl_sector.DataTextField = "NAME";
                ddl_sector.DataValueField = "CODE";
                ddl_sector.DataBind();
                return;
            }

            ddl_sector.DataSource = DB_System.get_Sectors(ddl_position.SelectedValue);
            ddl_sector.DataValueField = "CODE";
            ddl_sector.DataBind();

        }

        protected void btn_chkavailable_Click(object sender, EventArgs e)
        {
            //Page Error Checks
            if (ddl_all_trainees.SelectedValue == "0" || ddl_all_trainees.SelectedValue == "-")
            {
                lbl_availability.Text = "Choose Trainee";
                lbl_availability.Visible = true; return;
            }
            if (ddl_position.SelectedValue == "0" || ddl_position.SelectedValue == "-")
            {
                lbl_availability.Text = "Choose Position";
                lbl_availability.Visible = true; return;
            }
            if (ddl_sector.SelectedValue == "0" || ddl_sector.SelectedValue == "-")
            {
                lbl_availability.Text = "Choose Sector";
                lbl_availability.Visible = true; return;
            }

            //todo : error if user already has a training folder for that position
            // if anything comes before the position do not allow.
            // this means tower gmc check will not pass for a trainee who has a Assist training in the system

            bool has_folder = DB_Reports.has_Prior_TrainingFolder(ddl_all_trainees.SelectedValue, ddl_position.SelectedValue, ddl_sector.SelectedValue);
            if (has_folder)
            {
                lbl_availability.Visible = true;
                lbl_availability.Text = "User Has Prior Folder";
                return;
            }


            //available

            //he shouldnt change after checking availibility check, because page is now dependant on these values
            ddl_all_trainees.Enabled = false;
            ddl_position.Enabled = false;
            ddl_sector.Enabled = false;


            btn_chkavailable.Visible = false;
            lbl_availability.Text = "Trainee Available. &nbsp; " +
                                     " Choose A Step to Start (for Initial Data Migration). &nbsp;" +
                                     "All steps BEFORE the chosen step will be regarded as completed.";
            lbl_availability.Visible = true;
            txt_totalhours.Visible = true;
            lbl_totalhours.Visible = true;

            //load steps for chosen position/sector

            ddl_steps.DataSource = DB_Reports.get_Training_Steps_of_Position(ddl_position.SelectedValue, ddl_sector.SelectedValue);
            ddl_steps.DataTextField = "DESCRIPTION";
            ddl_steps.DataValueField = "ID";
            ddl_steps.DataBind();

            //make list button visible for further actions
            ddl_steps.Visible = true;
            btn_start_tree.Visible = true;
        }

        protected void btn_start_tree_Click(object sender, EventArgs e)
        {
            if (txt_totalhours.Text == "")
            {
                lbl_createresult.Text = "Total Hours is necessary";
                lbl_createresult.Visible = true;
                return;
            }
            if(!Utility.check_TimeTextbox_format(txt_totalhours.Text))
            {
                lbl_createresult.Text = "Total Hours Format  :  00:00";
                lbl_createresult.Visible = true;
                return;
            }

            string res = DB_Reports.start_Training_Folder_Migrate(ddl_all_trainees.SelectedValue, ddl_position.SelectedValue, ddl_sector.SelectedValue, ddl_steps.SelectedValue);

            if (res == "")
            {
                if (DB_Reports.update_totalhours(ddl_all_trainees.SelectedValue, ddl_steps.SelectedValue, txt_totalhours.Text, "MIGRATION"))
                {
                    lbl_createresult.Text = "SUCCESS : Training Folder is created.";
                    lbl_createresult.Visible = true;
                    ddl_steps.Enabled = false;
                    txt_totalhours.Enabled = false;
                    btn_start_tree.Enabled = false;
                    return;
                }
            }

            lbl_createresult.Text = "Unexpected System Error : Contact System Administrators";
            lbl_createresult.Visible = true;
        }
    }
}