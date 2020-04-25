using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Exams
{
    public partial class Exam_MainAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["direct_dictionary"] = null; //clean-up
            Session["chosen_questions"] = null; //clean-up
            Session["exam_result_details"] = null;
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
    }
}