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
    public partial class CreateExam : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = (DataTable)Session["chosen_questions"];
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("P");
                    dt.Columns.Add("ID");
                    dt.Columns.Add("SECTOR");
                    dt.Columns.Add("QUESTION");
                    dt.Columns.Add("Answer");
                }



                Fill_Grid_AllQuestions();
                Fill_grid_Chosen_Questions();
            }
            else
            {
            }

        }

        private Control GetControlThatCausedPostBack(Page page)
        {
            //initialize a control and set it to null
            Control ctrl = null;

            //get the event target name and find the control
            string ctrlName = page.Request.Params.Get("__EVENTTARGET");
            if (!String.IsNullOrEmpty(ctrlName))
                ctrl = page.FindControl(ctrlName);

            //return the control to the calling method
            return ctrl;
        }

        protected DataTable Fill_Grid_AllQuestions(string sector = "GEN")
        {
            DataTable dt = DB_Exams.get_questions_withAnswer(sector:sector);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("SECTOR");
                dt.Columns.Add("QUESTION");
                dt.Columns.Add("Answer");
            }

            grid_all_questions.DataSource = dt;
            grid_all_questions.DataBind();

            //binds but also returns table
            return dt;
        }

        protected void grid_all_questions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 2)
                e.Row.Cells[2].Visible = false;
        }

        protected void grid_all_questions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_all_questions.Rows[index];

            if (e.CommandName == "ADD")
            {
                selectedRow.BackColor = System.Drawing.Color.LightGreen;

                DataTable dt = (DataTable)Session["chosen_questions"];

                //add if not exist
                DataRow[] result = dt.Select("ID = '" + selectedRow.Cells[3].Text.Trim() + "'");

                if (result == null || result.Length == 0)
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = selectedRow.Cells[3].Text.Trim();
                    row["SECTOR"] = selectedRow.Cells[4].Text.Trim();
                    row["QUESTION"] = selectedRow.Cells[5].Text.Trim();
                    row["Answer"] = selectedRow.Cells[6].Text.Trim();
                    dt.Rows.Add(row);
                    Session["chosen_questions"] = dt;
                }
            }
            else if (e.CommandName == "REMOVE")
            {
                selectedRow.BackColor = System.Drawing.Color.Empty;

                DataTable dt = (DataTable)Session["chosen_questions"];

                //remove that row containing selected id
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] result = dt.Select("ID = '" + selectedRow.Cells[3].Text.Trim() + "'");
                    foreach (DataRow row in result)
                    {
                        dt.Rows.Remove(row);
                    }
                    Session["chosen_questions"] = dt;
                }
            }

            //update chosen
            Fill_grid_Chosen_Questions();
        }

        protected void grid_all_questions_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grid_all_questions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Gridview'in PageIndexChanging Eventında grid'in PageIndex'ine seçilen
            // sayfanın numarası atanır.
            grid_all_questions.PageIndex = e.NewPageIndex;

            // Tekrar kayıtların gridview'e aktarılması sağlanır.
            grid_all_questions.DataSource = DB_Exams.get_questions_withAnswer(sector:ddl_sector.SelectedValue);
            grid_all_questions.DataBind();
        }

        protected void ddl_sector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Grid_AllQuestions(ddl_sector.SelectedValue);
        }

        protected void btn_check_questions_Click(object sender, EventArgs e)
        {

        }

        protected void Fill_grid_Chosen_Questions()
        {
            DataTable dt = (DataTable)Session["chosen_questions"];
            if (dt != null && dt.Rows.Count > 0)
            {
                grid_chosens.DataSource = dt;
                grid_chosens.DataBind();
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("P");
                dt.Columns.Add("ID");
                dt.Columns.Add("SECTOR");
                dt.Columns.Add("QUESTION");
                dt.Columns.Add("Answer");

                Session["chosen_questions"] = dt;

                grid_chosens.DataSource = dt;
                grid_chosens.DataBind();
            }
        }

        protected void grid_chosens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_chosens.Rows[index];

            //only REMOVE command 

            DataTable dt = (DataTable)Session["chosen_questions"];

            //remove that row containing selected id
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] result = dt.Select("ID = '" + selectedRow.Cells[3].Text.Trim() + "'");
                foreach (DataRow row in result)
                {
                    dt.Rows.Remove(row);
                }


                Session["chosen_questions"] = dt;
                Fill_grid_Chosen_Questions();
            }
        }



        protected void btn_submit_exam_Click(object sender, EventArgs e)
        {
            //string Ques = ((TextBox)grid_chosens.Rows[i].FindControl("txt_points")).Text;


            //check page Elements
            if (!Check_Page_Elements())
            {
                return;
            }

            //push db
            if (!push_Exam())
            {
                Page_Result("Name Should be Unique");
                return;
            }
            else
            {
                Page_Result("EXAM ADDED SUCCESFULLY");
            }

            //clean up with a SUCCESS MESSAGE
            Session["chosen_questions"] = null;
            txt_examname.Text = "";
            txt_passpercent.Text = "";
            Fill_Grid_AllQuestions();



        }

        protected bool Check_Page_Elements()
        {
            if (txt_examname.Text == "")
            {
                Page_Result("Enter an Exam Name");
                return false;
            }
            if (txt_passpercent.Text == "" || Convert.ToInt32(txt_passpercent.Text) > 100 || Convert.ToInt32(txt_passpercent.Text) < 1)
            {
                Page_Result("Pass Grade must be within 1-100");
                return false;
            }

            DataTable dt = (DataTable)Session["chosen_questions"];
            if (dt == null || dt.Rows.Count == 0)
            {
                Page_Result("Choose Questions ");
                return false;
            }
            DataRow[] rows = dt.Select("P <> ''");
            if (rows.Length != 0 && rows.Length != dt.Rows.Count)
            {
                Page_Result("Either Enter All Question Points or Leave Them All Blank ");
                return false;
            }

            //todo : what else ???


            return true;

        }

        protected void Page_Result(string message)
        {
            lbl_pageresult.Text = message;
            lbl_pageresult.Visible = true;
        }

        protected bool push_Exam()
        {
            string name = txt_examname.Text;
            string passpercent = txt_passpercent.Text;

            Dictionary<string, string> questions = new Dictionary<string, string>();

            DataTable dt = (DataTable)Session["chosen_questions"];
            if (dt == null || dt.Rows.Count == 0)
            {
                Page_Result("Chosen Questions empty!");
                return false;
            }


            bool manuel_points = false;
            DataRow[] rows = dt.Select("P <> ''");
            if (rows.Length > 0)
                manuel_points = true;

            if (manuel_points)
            {
                foreach (DataRow item in dt.Rows)
                {
                    questions.Add(item["ID"].ToString(), item["P"].ToString()) ;
                }
            }
            else
            {
                float q_count = dt.Rows.Count;
                float q_point_float = 100 / q_count;
                string q_point = q_point_float.ToString("0.00");

                foreach (DataRow item in dt.Rows)
                {
                    questions.Add(item["ID"].ToString(), q_point);
                }
            }


            bool ok = DB_Exams.push_EXAM_DEF(name, passpercent, questions);

            return ok;


            /* 
            foreach (DataRow row in dt.Rows)
            {
                string q_id = (string)row["ID"];

                string q_point = "";

                //string Ques = ((TextBox)grid_chosens.Rows[i].FindControl("txt_points")).Text;

                foreach (GridViewRow grid_row in grid_chosens.Rows)
                {
                    if(grid_row.Cells[3].Text == q_id)
                    {
                        q_point = ((TextBox)grid_row.FindControl("txt_points")).Text;
                    }
                }
            }
            */


            return false;
        }

        protected void txt_points_TextChanged(object sender, EventArgs e)
        {
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            string q_id = currentRow.Cells[3].Text;
            string q_point = ((TextBox)sender).Text;

            //get chosen_questions
            //find q_id row
            //replace Point with q_point

            DataTable dt = (DataTable)Session["chosen_questions"];
            if (dt == null || dt.Rows.Count == 0)
                return;

            foreach (DataRow ro in dt.Rows)
            {
                if (ro["ID"].ToString() == q_id)
                    ro["P"] = q_point;
            }

            Session["chosen_questions"] = dt;
            Fill_grid_Chosen_Questions();


            //TextBox txt = (TextBox)currentRow.FindControl("txt_points");
            //Int32 count = Convert.ToInt32(txt.Text);
            //txt.Text = Convert.ToString(count + 10);
        }


    }
}