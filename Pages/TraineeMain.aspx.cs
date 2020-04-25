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
    public partial class TraineeMain : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //todo: UserSession has employeeid but if somebody has the right to look other people's pages (like SysAdmin or Testing)
                // string-query might carry target employeeid. If there's enough privilege, set this parameter and everything else works
                lbl_traineeID.Text = ((UserSession)Session["usersession"]).employeeid;

                Fill_trainee_Reports_Grid();
            }
          
        }


        protected void grid_trainee_reports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid_trainee_reports.PageIndex = e.NewPageIndex;
            Fill_trainee_Reports_Grid();
        }

        protected void Fill_trainee_Reports_Grid()
        {
            DataTable dt = new DataTable();
            
            dt = DB_Reports.get_my_training_Reports(lbl_traineeID.Text);
            if (dt == null || dt.Rows.Count == 0)
            {
                traineeports_tbl.Visible = false;
                return;
            }

            grid_trainee_reports.DataSource = dt;
            grid_trainee_reports.DataBind();

        }
        protected void grid_trainee_reports_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }
        protected void grid_trainee_reports_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow SelectedRow = grid_trainee_reports.SelectedRow;
            string id = SelectedRow.Cells[1].Text;
            Response.Redirect("~/Reports/ShowReport.aspx?ID=" + id);

        }

        protected void grid_trainee_reports_Sorting(object sender, GridViewSortEventArgs e)
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
            DataView sortedView = new DataView(DB_Reports.get_my_training_Reports(user.employeeid));
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            Session["SortedView"] = sortedView;
            grid_trainee_reports.DataSource = sortedView;
            grid_trainee_reports.DataBind();
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
    }
}