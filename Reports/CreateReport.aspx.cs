using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Reports
{
    public partial class CreateReport : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable trainees = DB_System.get_ALL_trainees();
                if (trainees != null)
                {
                    ddl_trainees.DataSource = trainees;
                    ddl_trainees.DataTextField = "NAME";
                    ddl_trainees.DataValueField = "ID";
                    ddl_trainees.DataBind();
                }

                DataTable pozs = DB_System.get_Positions();
                if (pozs != null)
                {
                    ddl_positions.DataSource = pozs;
                    ddl_positions.DataValueField = "CODE";
                    ddl_positions.DataBind();
                }
            }
        }
        protected void ddl_positions_SelectedIndexChanged(object sender, EventArgs e)
        {
            //hide so that user cant click on createreport button
            grid_folder.Visible = false;

            if (ddl_positions.SelectedValue == "-")
            {
                ddl_sectors.DataSource = new DataTable();
                ddl_sectors.DataValueField = "CODE";
                ddl_sectors.DataBind();
                return;
            }
            else
            {
                ddl_sectors.DataSource = DB_System.get_Sectors(ddl_positions.SelectedValue);
                ddl_sectors.DataValueField = "CODE";
                ddl_sectors.DataBind();
            }
        }
        protected void ddl_sectors_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid_folder.Visible = false;
        }

        protected void btn_findfolder_Click(object sender, EventArgs e)
        {
            //TODO : bring the folder 
            DataTable folder = DB_Reports.get_Training_Folder(ddl_trainees.SelectedValue, ddl_positions.SelectedValue, ddl_sectors.SelectedValue);
            if (folder == null)
            {
                lbl_findresult.Text = "System Error : Try Again Later";
                lbl_findresult.Visible = true;
                btn_createFolder.Visible = false;
            }
            else if( folder.Rows.Count == 0)
            {
                lbl_findresult.Text = "Trainee has no Training Folder for Position. It should be created first";
                lbl_findresult.Visible = true;
                btn_createFolder.Visible = true;
            }
             
            else
            {
                grid_folder.DataSource = folder;
                grid_folder.DataBind();
                grid_folder.Visible = true;
                lbl_findresult.Visible = false;
                btn_createFolder.Visible = false;
            }


        }


        protected void grid_folder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //todo : here we can show the report in RptNum columns. Also maybe filename



            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                //if status isn't ongoing, dont show the create button
                if (e.Row.Cells[3].Text != "ONGOING")
                {
                    e.Row.Cells[0].Enabled = false; //disable the Create buttons is not ONGOING STEPS (finished, migration etc (todo: else?)
                    e.Row.Cells[0].CssClass = "hidden-cell";
                    e.Row.Cells[1].Enabled = false; //disable the Create buttons is not ONGOING STEPS (finished, migration etc (todo: else?)
                    e.Row.Cells[1].CssClass = "hidden-cell";
                }
                else
                {
                    e.Row.Cells[0].Attributes["src"] = "../Images/create.png";
                }


                //hide 
                e.Row.Cells[7].Visible = false; //the genid
                e.Row.Cells[8].Visible = false; // PHASE  : PRELEVEL1, OJT_LEVEL1 , ASSIST etc...
                e.Row.Cells[9].Visible = false; //NAME  : APP_BR_ASSIST , TWR_GMC_LEVEL3PLUS etc...



                //e.Row.Cells[0].Attributes["style"] = "cursor:pointer";
                //e.Row.Cells[0].Attributes["onclick"] = "location.href='/Pages/ProjectEdit.aspx?ID='";
            }
        }
        
        protected void grid_folder_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_folder.Rows[index];
            string genid = selectedRow.Cells[7].Text; //  genid
            string phase = selectedRow.Cells[8].Text; // PHASE  : PRELEVEL1, OJT_LEVEL1 , ASSIST etc...
            string name = selectedRow.Cells[9].Text; //NAME  : APP_BR_ASSIST , TWR_GMC_LEVEL3PLUS etc...


            Dictionary<string, string> direct_dictionary = new Dictionary<string, string>();
            direct_dictionary.Add("genid", genid);
            direct_dictionary.Add("traineeid", ddl_trainees.SelectedValue);
            direct_dictionary.Add("sector", ddl_sectors.SelectedValue);
            direct_dictionary.Add("position", ddl_positions.SelectedValue);
            direct_dictionary.Add("phase", phase);    // PHASE  : PRELEVEL1, OJT_LEVEL1 , ASSIST etc...
            direct_dictionary.Add("name", name);    //NAME  : APP_BR_ASSIST , TWR_GMC_LEVEL3PLUS etc...

            UserSession user = (UserSession)Session["usersession"];
            direct_dictionary.Add("ojtiid", user.employeeid);

            Session["direct_dictionary"] = direct_dictionary;



            if (e.CommandName == "Training")
            {
                if (ddl_positions.SelectedValue == "TWR")
                {
                    if (ddl_sectors.SelectedValue == "ASSIST")
                        Response.Redirect("~/Reports/DAILYTR_ASS_TWR.aspx?Code=GENID");
                    else
                        Response.Redirect("~/Reports/TOWERTR_GMC_ADC.aspx?Code=GENID");
                }
                else
                {
                    if (selectedRow.Cells[3].Text.Contains("ASSIST"))
                        Response.Redirect("~/Reports/DAILYTR_ASS_RAD.aspx?Code=GENID");
                    else
                        Response.Redirect("~/Reports/TR_ARE_APP_RAD.aspx?Code=GENID");

                }
            }
            else if (e.CommandName == "Recommend")
            {
                
                if (Recommendable(direct_dictionary))
                    Response.Redirect("~/Reports/RECOM_LEVEL.aspx?Code=GENID");
                else
                    lbl_findresult.Text = "User can't be recommended"; //todo: give some reason
            }

        }


        protected bool Recommendable(Dictionary<string, string> direct_dictionary)
        {
            //todo : implement
            return true;
        }


        protected void btn_createFolder_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SysAdmin/CreateTrainingFolder.aspx");
        }

    }
}