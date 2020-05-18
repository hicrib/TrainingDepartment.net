using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.SysAdmin
{
    public partial class MERAssign : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("MER DEFINITION");

                ddl_trainee.DataSource = DB_System.get_ALL_trainees();
                ddl_trainee.DataTextField = "NAME";
                ddl_trainee.DataValueField = "ID";
                ddl_trainee.DataBind();

                ddl_position.DataSource = DB_System.get_Positions();
                ddl_position.DataValueField = "CODE";
                ddl_position.DataBind();

            }
        }

        protected void ddl_position_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_sector.DataSource = DB_System.get_Sectors(ddl_position.SelectedValue);
            ddl_sector.DataValueField = "CODE";
            ddl_sector.DataBind();

            grid_defaultMERs.Visible = false;
        }

        protected void ddl_sector_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_steps.DataSource = DB_Reports.get_Training_Steps_of_Position(ddl_position.SelectedValue, ddl_sector.SelectedValue);
            ddl_steps.DataTextField = "DESCRIPTION";
            ddl_steps.DataValueField = "ID";
            ddl_steps.DataBind();

            ListItem l = new ListItem();
            l.Value = "-";
            ddl_steps.Items.Insert(0, l);

            grid_defaultMERs.Visible = false;
        }

        protected void btn_findMERs_Click(object sender, EventArgs e)
        {
            
            if (ddl_trainee.SelectedValue == "0" || ddl_position.SelectedValue == "-" || ddl_sector.SelectedValue == "-")
            {
                lbl_pageresult.Text = "Choose Trainee / Position / Sector";
                lbl_pageresult.Visible = true;
                return;
            }
            
            string step = "";
            if (ddl_steps.SelectedValue != "-")
                step = ddl_steps.SelectedValue;

            grid_defaultMERs.DataSource = DB_Reports.get_MERs(ddl_trainee.SelectedValue, ddl_position.SelectedValue, ddl_sector.SelectedValue, step);
            grid_defaultMERs.DataBind();
            grid_defaultMERs.Visible = true;

        }


        protected void txt_MER_TextChanged(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            string stepid = currentRow.Cells[5].Text;

            TextBox txt_merr = (TextBox)sender;

           if(DB_Reports.update_MER(stepid,txt_merr.Text,ddl_trainee.SelectedValue,txt_comments.Text))
            {
                currentRow.Cells[1].Text = txt_merr.Text;
                currentRow.Style.Add("font-style", "italic");
                currentRow.Style.Add("color", "#d36767");
            }

        }

        protected void grid_defaultMERs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.Cells.Count > 3)
            {
                e.Row.Cells[5].Visible = false;
            }
        }

        protected void ddl_trainee_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid_defaultMERs.Visible = false;
        }
    }
}