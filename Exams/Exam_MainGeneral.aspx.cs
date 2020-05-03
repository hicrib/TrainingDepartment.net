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
            if (!IsPostBack)
            {
                //clean-up
                Session["direct_dictionary"] = null;
                Session["grid_questions"] = null;
                Session["chosen_questions"] = null;
                Session["current_exam_questions"] = null;
                Session["exam_result"] = null;
                Session["exam_result_details"] = null;


                fill_grid_examassignments();
                fill_grid_examcompleted();
                fill_grid_trainingassignments();
                fill_grid_completed_trainings();
            }
        }

        protected void fill_grid_examassignments()
        {
            UserSession user = (UserSession)Session["usersession"];
            DataTable dt = DB_Exams.get_Assignments_Open(user.employeeid);

            if (dt == null || dt.Rows.Count == 0)
                return;

            grid_examassignments.DataSource = dt;
            grid_examassignments.DataBind();
        }
        protected void grid_examassignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_examassignments.Rows[index];

            string assignid = selectedRow.Cells[5].Text;
            DB_Exams.update_Exam_Assignment(assignid, userstart: "now", status: "USER_STARTED");
            Response.Redirect("~/Exams/UserInExam.aspx?AsID=" + assignid);
        }
        protected void grid_examassignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[5].Visible = false; // hide ASSIGN_ID
        }
        
        

        
        protected void fill_grid_examcompleted()
        {
            UserSession user = (UserSession)Session["usersession"];
            DataTable dt = DB_Exams.get_Assignments_Completed(user.employeeid);

            if (dt == null || dt.Rows.Count == 0)
                return;

            grid_examcompleted.DataSource = dt;
            grid_examcompleted.DataBind();
        }
        protected void grid_examcompleted_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[4].Visible = false; // hide ASSIGN_ID
        }







        protected void fill_grid_trainingassignments()
        {
            UserSession user = (UserSession)Session["usersession"];
            DataTable dt = DB_Trainings.get_Assigned_Trainings_open(user.employeeid);

            if (dt == null || dt.Rows.Count == 0)
                return;

            grid_assigned_training.DataSource = dt;
            grid_assigned_training.DataBind();
        }
        protected void grid_assigned_training_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[5].Visible = false; // hide ASSIGN_ID
            e.Row.Cells[7].Visible = false; // hide LASTSTEPID
        }
        protected void grid_assigned_training_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_assigned_training.Rows[index];

            string assignid = selectedRow.Cells[5].Text;
            string laststepid = selectedRow.Cells[7].Text;

            DB_Trainings.update_Assignment(assignid, status: "USER_STARTED" , userstart:"now" );

            Response.Redirect("~/Trainings/UserInTraining.aspx?AsID=" + assignid + "&StepID=" + laststepid);
        }




        protected void fill_grid_completed_trainings()
        {
            UserSession user = (UserSession)Session["usersession"];
            DataTable dt = DB_Trainings.get_Completed_Trainings(user.employeeid);

            if (dt == null || dt.Rows.Count == 0)
                return;

            grid_completed_trainings.DataSource = dt;
            grid_completed_trainings.DataBind();

        }

    }
}