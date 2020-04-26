﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AviaTrain.App_Code;
using System.Data;

namespace AviaTrain.SysAdmin
{
    public partial class SysAdminMain : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["direct_dictionary"] = null; //clean-up
                Session["chosen_questions"] = null; //clean-up
                Session["exam_result_details"] = null;

                UserSession user = (UserSession)Session["usersession"];
                if (!user.isAdmin)
                    RedirectWithCode("UNAUTHORIZED !");
            }
        }

        protected void btn_createuser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SysAdmin/CreateUser.aspx");
        }

        protected void btn_create_TrainingFolder_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SysAdmin/CreateTrainingFolder.aspx");
        }

        protected void btn_View_Trainee_Folder_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SysAdmin/ViewTrainingFolder.aspx");
        }

        protected void btn_create_questions_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Exams/CreateQuestions.aspx");
        }

        protected void btn_create_exam_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Exams/CreateExam.aspx");
        }

        protected void btn_assign_exam_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Exams/AssignExam.aspx");
        }

        protected void btn_delete_exam_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Exams/DeleteExam.aspx");
        }

        protected void btn_view_exam_result_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Exams/ViewExamResults.aspx");
        }

        protected void btn_edit_roles_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SysAdmin/EditRoles.aspx");
        }

        protected void btn_create_training_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Trainings/CreateTraining.aspx");
        }
    }
}