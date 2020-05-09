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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("MER DEFINITION");

                ddl_position.DataSource = DB_System.get_Positions();
                ddl_position.DataValueField = "CODE";
                ddl_position.DataBind();

            }
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            int index = Int32.Parse(e.Item.Value);
            MultiView1.ActiveViewIndex = index;

            //questions
            if (index == 1)
            {
            }
        }

        protected void ddl_position_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_sector.DataSource = DB_System.get_Sectors(ddl_position.SelectedValue);
            ddl_sector.DataValueField = "CODE";
            ddl_sector.DataBind();
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
        }

        protected void btn_findMERs_Click(object sender, EventArgs e)
        {
            string pos = "";
            if (ddl_position.SelectedValue != "-")
                pos = ddl_position.SelectedValue;
            string sec = "";
            if (ddl_sector.SelectedValue != "-")
                sec = ddl_sector.SelectedValue;
            string step = "";
            if (ddl_steps.SelectedValue != "-")
                step = ddl_steps.SelectedValue;

            grid_defaultMERs.DataSource = DB_Reports.get_MERs(pos, sec, step);
            grid_defaultMERs.DataBind();
        }

        protected void grid_defaultMERs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_defaultMERs.Rows[index];

            //only REMOVE command 

            //Fill_grid_Chosen_Questions();

        }

        protected void txt_MER_TextChanged(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            string stepid = currentRow.Cells[5].Text;

            TextBox txt_merr = (TextBox)sender;

            if (DB_Reports.update_MER(stepid, txt_merr.Text))
            {
                currentRow.Cells[4].Text = txt_merr.Text;
            }

            //get chosen_questions
            //find q_id row
            //replace Point with q_point

            //DataTable dt = (DataTable)Session["chosen_questions"];
            //if (dt == null || dt.Rows.Count == 0)
            //    return;

            //foreach (DataRow ro in dt.Rows)
            //{
            //    if (ro["ID"].ToString() == q_id)
            //        ro["P"] = q_point;
            //}

            //Session["chosen_questions"] = dt;
            //Fill_grid_Chosen_Questions();
        }
    }
}