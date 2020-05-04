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
    public partial class ViewExamResults : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("VIEW EXAM RESULTS");
                Fill_Exam_Names();
                Fill_Trainees();
            }


        }

        protected void Fill_Exam_Names()
        {
            //only active exams or not
            filter_examname.DataSource = DB_Exams.get_Exams(chk_active_exams.Checked);
            filter_examname.DataTextField = "NAME";
            filter_examname.DataValueField = "ID";
            filter_examname.DataBind();
        }
        protected void Fill_Trainees()
        {
            //only active exams or not
            filter_trainee.DataSource = DB_Exams.get_EXAM_TRAINEES(chk_active_trainee.Checked);
            filter_trainee.DataTextField = "NAME";
            filter_trainee.DataValueField = "ID";
            filter_trainee.DataBind();
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if (!Check_Page_Elements())
                return;

            string exam_id = "";
            if (filter_examname.SelectedValue == "0" || filter_examname.SelectedValue == "-")
                exam_id = "";
            else
                exam_id = filter_examname.SelectedValue;

            string traineeid = "";
            if (filter_trainee.SelectedValue == "0" || filter_trainee.SelectedValue == "-")
                traineeid = "";
            else
                traineeid = filter_trainee.SelectedValue;



            string start_date = "2000-01-01 00:00:01";
            //yyyy-mm-dd
            if (filter_start.Text != "")
                start_date = filter_start.Text + " 00:00:01";


            string finish_date = "2050-12-31 23:59:59";
            //yyyy-mm-dd
            if (filter_finish.Text != "")
                finish_date = filter_finish.Text + " 23:59:59";


            DataTable dt = DB_Exams.View_Exam_Results(exam_id, chk_active_exams.Checked,
                                                            start_date, finish_date, traineeid, chk_active_trainee.Checked,
                                                            filter_passed.SelectedValue,
                                                            filter_grd_start.Text, filter_grd_finish.Text);
            if (dt != null && dt.Rows.Count > 0)
            {
                grid_results.DataSource = dt;
                grid_results.DataBind();
                grid_results.Visible = true;
                lbl_result.Visible = false;
            }
            else
            {
                lbl_result.Text = "No Result Found";
                lbl_result.Visible = true;
                grid_results.Visible = false;
            }

        }

        protected bool Check_Page_Elements()
        {
            if (Convert.ToInt32(filter_grd_start.Text) > 100 || Convert.ToInt32(filter_grd_finish.Text) < 0
                || Convert.ToInt32(filter_grd_start.Text) > Convert.ToInt32(filter_grd_finish.Text)
                || filter_grd_start.Text == "" || filter_grd_start.Text == "")
            {
                lbl_result.Text = "Grade Between  should be 1-100";
                lbl_result.Visible = true;
                return false;
            }

            lbl_result.Text = filter_start.Text;
            lbl_result.Visible = true;

            return true;
        }


        protected void chk_active_exams_CheckedChanged(object sender, EventArgs e)
        {
            Fill_Exam_Names();
        }

        protected void chk_active_trainee_CheckedChanged(object sender, EventArgs e)
        {
            Fill_Trainees();
        }

        protected void grid_results_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 7)
            {
                e.Row.Cells[7].Visible = false; // hide EXAMID
                e.Row.Cells[8].Visible = false; // hide ASSIGNID
            }
        }

        protected void grid_results_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //go to details in a new tab
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_results.Rows[index];

            string assignid = selectedRow.Cells[8].Text.Trim(); //  ASSIGNID  
            List<string> details = new List<string>();
            details.Add(selectedRow.Cells[1].Text.Trim()); //name
            details.Add(selectedRow.Cells[2].Text.Trim()); //name
            details.Add(selectedRow.Cells[3].Text.Trim()); //name
            details.Add(selectedRow.Cells[4].Text.Trim()); //name
            details.Add(selectedRow.Cells[5].Text.Trim()); //name
            details.Add(selectedRow.Cells[6].Text.Trim()); //name

            Session["exam_result_details"] = details;

            string url = "ViewUserExamDetails.aspx?AsID=" + assignid;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "open('" + url + "') ;", true);
        }
    }
}