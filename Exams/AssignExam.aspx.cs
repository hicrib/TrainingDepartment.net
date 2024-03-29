﻿using AviaTrain.App_Code;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Exams
{
    public partial class AssignExam : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("ASSIGN EXAM TO USER");

                DataTable exams = DB_Exams.get_Exams();
                if (exams != null)
                {
                    ddl_exams.DataSource = exams;
                    ddl_exams.DataTextField = "NAME";
                    ddl_exams.DataValueField = "ID";
                    ddl_exams.DataBind();
                }

                DataTable dt = DB_Exams.get_EXAM_TRAINEES();
                if (dt != null)
                {
                    ddl_trainee.DataSource = dt;
                    ddl_trainee.DataTextField = "NAME";
                    ddl_trainee.DataValueField = "ID";
                    ddl_trainee.DataBind();
                }

            }
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            //check page elements
            if (!Check_Page_Elements())
                return;


            //push db
            if (!push_assignment())
            {
                Page_Result("System Error : Try Again Later");
                return;
            }


            //show result
            Page_Result("SUCCESS :  '" + ddl_exams.SelectedItem.Text + "' assigned to Trainee : " + ddl_trainee.SelectedItem.Text);

            //clean-up
            ddl_exams.SelectedValue = "0";
            ddl_trainee.SelectedValue = "0";
        }

        protected bool push_assignment()
        {
            string start = Calendar_start.SelectedDate.ToString("yyyy-MM-dd");
            string finish = Calendar_finish.SelectedDate.ToString("yyyy-MM-dd");

            string assignid = DB_Exams.push_EXAM_Assignment(ddl_exams.SelectedValue, ddl_trainee.SelectedValue, start, finish);

            return assignid != "";
        }

        protected bool Check_Page_Elements()
        {
            if (ddl_exams.SelectedValue == "-"
                 || ddl_exams.SelectedValue == "0" || ddl_exams.SelectedValue == "")
            {
                Page_Result("Choose Exam");
                return false;
            }
            if (ddl_trainee.SelectedValue == "-" || ddl_trainee.SelectedValue == "" || ddl_trainee.SelectedValue == "0")
            {
                Page_Result("Choose Trainee");
                return false;
            }

            //todo : date isleri

            if (DateTime.Compare(Calendar_start.SelectedDate.Date, DateTime.UtcNow.Date) < 0)
            {
                Page_Result("Can't schedule to past (UTC time)");
                return false;
            }

            if (DateTime.Compare(Calendar_finish.SelectedDate.Date, DateTime.UtcNow.Date) < 0)
            {
                Page_Result("Can't schedule to past (UTC time)");
                return false;
            }
            if (DateTime.Compare(Calendar_finish.SelectedDate.Date, Calendar_start.SelectedDate.Date) < 0)
            {
                Page_Result("Finish date must be later (UTC time)");
                return false;
            }

            return true;
        }

        protected void Page_Result(string message)
        {
            lbl_pageresult.Text = message;
            lbl_pageresult.Visible = true;
        }

        protected void Calendar_finish_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date < Calendar_start.SelectedDate.Date || e.Day.Date < DateTime.UtcNow.Date)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
        }

        protected void Calendar_start_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date < DateTime.UtcNow.Date)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.White;
            }
        }

        protected void ddl_exams_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_show_examformat.Visible = !(new string[3] { "-", "", "0" }).Contains(ddl_exams.SelectedValue);

            DataTable dt = DB_Exams.get_EXAM_QUESTIONS_by_examid(ddl_exams.SelectedValue);
            if (dt == null || dt.Rows.Count == 0)
                return;

            grid_questions.DataSource = dt;
            grid_questions.DataBind();

        }

        protected void btn_show_examformat_Click(object sender, EventArgs e)
        {
            string url = "ViewExam.aspx?ExID=" + ddl_exams.SelectedValue;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "open('" + url + "') ;", true);
        }

        protected void grid_questions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 4)
                e.Row.Cells[4].Visible = false; //ORDERBY column
        }
    }
}