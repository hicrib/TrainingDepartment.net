﻿using AviaTrain.App_Code;
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


                //todo : if not instructor, if not system admin -> Trainee
                // or came here as Trainee
                UserSession user = (UserSession)Session["usersession"];

                if (user.isAdmin)
                    Response.Redirect("~/SysAdmin/SysAdminMain.aspx");


                fill_grid_examassignments();
                fill_grid_examcompleted();
                fill_grid_trainingassignments();
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
        protected void fill_grid_examcompleted()
        {
            UserSession user = (UserSession)Session["usersession"];
            DataTable dt = DB_Exams.get_Assignments_Completed(user.employeeid);

            if (dt == null || dt.Rows.Count == 0)
                return;

            grid_examcompleted.DataSource = dt;
            grid_examcompleted.DataBind();
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

        protected void grid_examassignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_examassignments.Rows[index];

            string assignid = selectedRow.Cells[5].Text;
            Response.Redirect("~/Exams/UserInExam.aspx?AsID=" + assignid);
        }

        protected void grid_examassignments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[5].Visible = false; // hide ASSIGN_ID
        }

        protected void grid_examcompleted_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[4].Visible = false; // hide ASSIGN_ID
        }


        protected void grid_assigned_training_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[5].Visible = false; // hide ASSIGN_ID
        }

        protected void grid_assigned_training_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_assigned_training.Rows[index];

            string assignid = selectedRow.Cells[5].Text;
            Response.Redirect("~/Trainings/UserInTraining.aspx?AsID=" + assignid);
        }
    }
}