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
    public partial class DeleteExam : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("DELETE EXAM");
                DataTable exams = DB_Exams.get_Exams();
                if (exams != null)
                {
                    ddl_exams.DataSource = exams;
                    ddl_exams.DataTextField = "NAME";
                    ddl_exams.DataValueField = "ID";
                    ddl_exams.DataBind();
                }

            }
        }

        protected void ddl_exams_SelectedIndexChanged(object sender, EventArgs e)
        {
            string examid = ddl_exams.SelectedValue;

            if (examid == "" || examid == "-" || examid == "0")
            {
                grid_exam_questions.Visible = false;
                btn_delete.Enabled = false;
            }
            else
            {
                string status = DB_Exams.is_examid_assigned_and_open(examid);
                if (status != "")
                {
                    lbl_result.Text = "THIS EXAM IS ASSIGNED TO A USER ! Deleting will cause it to be unassigned! Or malfunctions if user is currently in exam";
                    lbl_result.Visible = true;
                }
                else
                {
                    lbl_result.Text = "";
                    lbl_result.Visible = false;
                }


                btn_delete.Enabled = true;
                DataTable dt = DB_Exams.get_EXAM_QUESTIONS_by_examid(examid);
                grid_exam_questions.DataSource = dt;
                grid_exam_questions.DataBind();
                grid_exam_questions.Visible = true;
            }
        }

        protected void btn_delete_Click(object sender, EventArgs e)
        {
            if (ddl_exams.SelectedValue == "" || ddl_exams.SelectedValue == "-" || ddl_exams.SelectedValue == "0")
            {
                return;
            }

            bool ok = DB_Exams.delete_EXAM(ddl_exams.SelectedValue);
            if (!ok)
            {
                lbl_result.Text = "System Error ! ";
                lbl_result.Visible = true;
            }
            else
            {
                lbl_result.Text = "Exam Deleted ";
                lbl_result.Visible = true;

                DataTable exams = DB_Exams.get_Exams();
                if (exams != null)
                {
                    ddl_exams.DataSource = exams;
                    ddl_exams.DataTextField = "NAME";
                    ddl_exams.DataValueField = "ID";
                    ddl_exams.DataBind();
                }
                grid_exam_questions.Visible = false;
            }

        }

        protected void grid_exam_questions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 4)
                e.Row.Cells[4].Visible = false;
        }
    }
}