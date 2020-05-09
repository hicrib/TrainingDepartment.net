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
    public partial class InstructorMain : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Fill_OJTI_Reports_Grid();
            }
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
                ojtireports_tbl.Visible = false;
                return;
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
            Response.Redirect("~/Reports/ShowReport.aspx?ID=" + id);

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

        protected void btn_create_report_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Reports/CreateReport.aspx");
        }

        protected void btn_my_training_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/TraineeMain.aspx");
        }

        protected void btn_create_trainingfolde_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SysAdmin/CreateTrainingFolder.aspx");
        }
    }
}