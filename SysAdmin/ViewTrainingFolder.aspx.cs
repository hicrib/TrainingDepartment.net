using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AviaTrain.App_Code;
using System.Data;

namespace AviaTrain.SysAdmin
{
    public partial class ViewTrainingFolder : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("VIEW TRAINING FOLDER");

                Fill_Page_Elements();
            }
        }

        protected void Fill_Page_Elements()
        {
            DataTable all_users = DB_System.get_ALL_trainees();
            if (all_users != null)
            {
                ddl_trainees.DataSource = all_users;
                ddl_trainees.DataTextField = "NAME";
                ddl_trainees.DataValueField = "ID";
                ddl_trainees.DataBind();
            }
        }

        protected void ddl_positions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_positions.SelectedValue == "-")
            {
                ddl_sector.DataSource = new DataTable();
                ddl_sector.DataTextField = "DESCRIPTION";
                ddl_sector.DataValueField = "ID";
                ddl_sector.DataBind();
                return;
            }

            ddl_sector.DataSource = DB_System.get_Sectors(ddl_positions.SelectedValue);
            ddl_sector.DataValueField = "CODE";
            ddl_sector.DataTextField = "DESCRIPTION";
            ddl_sector.DataBind();
        }


        protected void Fill_Folder_Grid()
        {
            DataTable dt = new DataTable();

            dt = DB_Reports.get_Training_Folder(ddl_trainees.SelectedValue , ddl_positions.SelectedValue , ddl_sector.SelectedValue);
            if (dt == null || dt.Rows.Count == 0)
            {
                grid_folder.Visible = false;
                return;
            }
            grid_folder.Visible = true;
            grid_folder.DataSource = dt;
            grid_folder.DataBind();
        }
        protected void btn_find_folder_Click(object sender, EventArgs e)
        {
            Fill_Folder_Grid();
        }

        protected void grid_folder_SelectedIndexChanged(object sender, EventArgs e)
        {
            //todo: do somethings when row is selected
        }

        protected void grid_folder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid_folder.PageIndex = e.NewPageIndex;
            Fill_Folder_Grid();
        }

        public SortDirection direction
        {
            get
            {
                if (ViewState["directionState"] == null)
                {
                    ViewState["directionState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["directionState"];
            }
            set
            {
                ViewState["directionState"] = value;
            }
        }
        protected void grid_folder_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortingDirection = string.Empty;
            if (direction == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
                sortingDirection = "Desc";

            }
            else
            {
                direction = SortDirection.Ascending;
                sortingDirection = "Asc";
            }
            
            DataView sortedView = new DataView(DB_Reports.get_Training_Folder(ddl_trainees.SelectedValue, ddl_positions.SelectedValue, ddl_sector.SelectedValue));
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            Session["SortedView"] = sortedView;
            grid_folder.DataSource = sortedView;
            grid_folder.DataBind();
        }

        protected void grid_folder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.Cells.Count > 5)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;

                

                if (e.Row.Cells[2].Text == "REPORT" 
                    || e.Row.Cells[2].Text.ToLower().Contains("assessment")
                    || e.Row.Cells[2].Text.ToLower().Contains("recommended")
                    )
                    e.Row.Cells[1].Text = "";
                else
                    e.Row.Cells[0].Style.Add("opacity", "0");//todo burada findcontrols desek daha iyi. ornegi LEVELOBJECTIVES'de



                if (e.Row.Cells[2].Text != "ONGOING" && e.Row.Cells[2].Text != "COMPLETED" && e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Style.Add("font-size", "small");
                    e.Row.Cells[0].Style.Add("border", "none");
                    e.Row.Cells[1].Style.Add("border", "none");
                }
                else
                    e.Row.Style.Add("font-weight", "bold");

                if (e.Row.Cells[4].Text == "0")
                    e.Row.Cells[4].Text = "";
            }
        }

        protected void grid_folder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName == "GO")
            {
                GridViewRow selectedRow = grid_folder.Rows[Convert.ToInt32(e.CommandArgument)];
                string reportid = selectedRow.Cells[4].Text.ToString().Trim();
                string url = "/Reports/ShowReport.aspx?ID=" + reportid;
                

                String js = "window.open('"+url+"', '_blank');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Open " + url, js, true);
            }
           
        }
    }
}