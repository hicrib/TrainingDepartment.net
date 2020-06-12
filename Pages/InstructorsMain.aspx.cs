using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Pages
{
    public partial class InstructorsMain : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //rollere gore menu itemları kaldır
                UserSession user = (UserSession)Session["usersession"];
                if (!user.has_ROLENAME("TRN_DEPARTMENT_SIGNER"))
                {
                    jobsMenu.Items.Remove(jobsMenu.FindItem("2"));
                }

                Fill_OJTI_Reports_Grid();
            }
        }

        protected void jobsMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            int index = Int32.Parse(e.Item.Value);
            if (index == 1) //my training
                Response.Redirect("~/Pages/TraineeMain.aspx");
            else if (index == 0)
                Fill_OJTI_Reports_Grid();
            else if (index == 2) //department
                Fill_Grid_Department();


            multiview1.ActiveViewIndex = index;
        }
        protected void grid_ojti_reports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid_ojti_reports.PageIndex = e.NewPageIndex;
            Fill_OJTI_Reports_Grid();
        }

        protected void Fill_OJTI_Reports_Grid()
        {
            DataTable dt = new DataTable();
            UserSession user = (UserSession)Session["usersession"];
            dt = DB_Reports.get_myCreatedReports(user.employeeid);
            if (dt == null || dt.Rows.Count == 0)
            {
                grid_ojti_reports.Visible = false;
            }

            grid_ojti_reports.DataSource = dt;
            grid_ojti_reports.DataBind();

        }
        protected void grid_ojti_reports_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }
        protected void grid_ojti_reports_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow SelectedRow = grid_ojti_reports.SelectedRow;
            string id = SelectedRow.Cells[1].Text;
            string url = "/Reports/ShowReport.aspx?ID=" + id;

            String js = "window.open('" + url + "', '_blank');";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Open " + url, js, true);
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
        protected void grid_ojti_reports_Sorting(object sender, GridViewSortEventArgs e)
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
            UserSession user = (UserSession)Session["usersession"];
            DataView sortedView = new DataView(DB_Reports.get_myCreatedReports(user.employeeid));
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            Session["SortedView"] = sortedView;
            grid_ojti_reports.DataSource = sortedView;
            grid_ojti_reports.DataBind();
        }


        protected void Fill_Grid_Department()
        {
            grid_department.DataSource = DB_Reports.get_Department_Reports();
            grid_department.DataBind();
        }

        protected void grid_department_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow SelectedRow = grid_department.SelectedRow;
            string id = SelectedRow.Cells[1].Text;
            string url = "/Reports/ShowReport.aspx?ID=" + id;

            String js = "window.open('" + url + "', '_blank');";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Open " + url, js, true);
        }

        protected void grid_department_Sorting(object sender, GridViewSortEventArgs e)
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

            DataView sortedView = new DataView(DB_Reports.get_Department_Reports ());
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            Session["SortedView"] = sortedView;
            grid_department.DataSource = sortedView;
            grid_department.DataBind();
        }
    }
}