using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Exams
{
    public partial class Exam_MainGeneral : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            //clean-up
            Session["direct_dictionary"] = null;
            Session["grid_questions"] = null; 
            Session["chosen_questions"] = null; 
            Session["current_exam_questions"] = null; 
            Session["exam_result"] = null;
            Session["exam_result_details"] = null;


            //todo : if not instructor, if not system admin -> Trainee
            // or came here as Trainee
            UserSession user = (UserSession)Session["usersession"];

            if (user.isAdmin)
                Response.Redirect("~/SysAdmin/SysAdminMain.aspx");

            if (!IsPostBack)
            {
                fill_grid_assignments();
                fill_grid_completed();
            }
        }

        protected void fill_grid_assignments()
        {
            UserSession user = (UserSession)Session["usersession"];
            DataTable dt = DB_Trainings.get_Assignments_Open(user.employeeid);

            if (dt == null || dt.Rows.Count == 0)
                return;

            grid_assignments.DataSource = dt;
            grid_assignments.DataBind();
        }
        protected void fill_grid_completed()
        {
            UserSession user = (UserSession)Session["usersession"];
            DataTable dt = DB_Trainings.get_Assignments_Completed(user.employeeid);

            if (dt == null || dt.Rows.Count == 0)
                return;

            grid_completed.DataSource = dt;
            grid_completed.DataBind();
        }

        protected void grid_assignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //todo : implement
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_assignments.Rows[index];

            string assignid = selectedRow.Cells[5].Text;
            Response.Redirect("~/Exams/UserInExam.aspx?AsID=" + assignid);
        }

        protected void grid_assignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[5].Visible = false; // hide ASSIGN_ID
        }

        protected void grid_completed_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[4].Visible = false; // hide ASSIGN_ID
        }
    }
}