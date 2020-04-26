using AjaxControlToolkit;
using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Exams
{
    public partial class CreateQuestions : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserSession user = (UserSession)Session["usersession"];
                if (!(user.isAdmin || user.isExamAdmin))
                    RedirectWithCode("UNAUTHORIZED !");

                get_my_last_fill_grid();
            }
            else
            {


            }
        }

        protected void get_my_last_fill_grid()
        {
            grid_questions.DataSource = DB_Exams.get_Questions_MY_LAST_("20");
            grid_questions.DataBind();
        }

        protected void btn_fill_submit_Click(object sender, EventArgs e)
        {
            //check page Elements
            if (!Check_FILL_Elements())
                return;

            //push db
            if (!push_FILL_question())
            {
                FILL_page_result("DB Error : Try Again Later !");
                return;
            }


            //put in grid
            // Renew_Qestions_Grid_for_FILL();
            get_my_last_fill_grid();


            //clean-up
            txt_Text1.Text = "";
            txt_Text2.Text = "";
            txt_Text3.Text = "";
            txt_Text4.Text = "";
            fill_1_ans1.Text = "";
            fill_1_ans2.Text = "";
            fill_1_ans3.Text = "";
            fill_2_ans1.Text = "";
            fill_2_ans2.Text = "";
            fill_2_ans3.Text = "";
            fill_3_ans1.Text = "";
            fill_3_ans2.Text = "";
            fill_3_ans3.Text = "";
            FILL_page_result("Question Added.");
        }

        protected bool Check_FILL_Elements()
        {
            if (txt_Text1.Text == "")
            {
                FILL_page_result("Fill first textbox !");
                return false;
            }
            else if (fill_1_ans1.Text.Trim().ToUpper() == "" && fill_1_ans2.Text.Trim().ToUpper() == "" && fill_1_ans3.Text.Trim().ToUpper() == "")
            {
                FILL_page_result("Fill acceptable answers for blank 1 !");
                return false;
            }//ardısık 2-3
            else if (txt_Text2.Text != "" && txt_Text3.Text != "" && fill_2_ans1.Text.Trim().ToUpper() == "" && fill_2_ans2.Text.Trim().ToUpper() == "" && fill_2_ans3.Text.Trim().ToUpper() == "")
            {
                FILL_page_result("Fill acceptable answers for blank 2 !");
                return false;
            }
            else if (txt_Text3.Text == "" && txt_Text4.Text != "" && fill_3_ans1.Text.Trim().ToUpper() == "" && fill_3_ans2.Text.Trim().ToUpper() == "" && fill_3_ans3.Text.Trim().ToUpper() == "")
            {
                FILL_page_result("Fill acceptable answers for blank 3 !");
                return false;
            }//ardısık 3-4
            else if (txt_Text3.Text != "" && txt_Text4.Text != "" && fill_3_ans1.Text.Trim().ToUpper() == "" && fill_3_ans2.Text.Trim().ToUpper() == "" && fill_3_ans3.Text.Trim().ToUpper() == "")
            {
                FILL_page_result("Fill acceptable answers for blank 3 !");
                return false;
            }//2-4
            else if (txt_Text2.Text == "" && (txt_Text3.Text != "" || txt_Text4.Text != ""))
            {
                FILL_page_result("Second textbox is empty!");
                return false;
            }
            else if (txt_Text2.Text == "" && (fill_2_ans1.Text.Trim().ToUpper() != "" || fill_2_ans2.Text.Trim().ToUpper() != "" || fill_2_ans3.Text.Trim().ToUpper() != ""))
            {
                FILL_page_result("Check acceptable answer fields");
                return false;
            }
            else if (txt_Text2.Text == "" && (fill_3_ans1.Text.Trim().ToUpper() != "" || fill_3_ans2.Text.Trim().ToUpper() != "" || fill_3_ans3.Text.Trim().ToUpper() != ""))
            {
                FILL_page_result("Second textbox is empty but blank 2 is filled");
                return false;
            }
            else if (txt_Text3.Text == "" && (fill_3_ans1.Text.Trim().ToUpper() != "" || fill_3_ans2.Text.Trim().ToUpper() != "" || fill_3_ans3.Text.Trim().ToUpper() != ""))
            {
                FILL_page_result("Third textbox is empty but blank 3 is filled");
                return false;
            }



            return true;
        }
        protected void FILL_page_result(string error = "")
        {
            lbl_fill_result.Text = error == "" ? "Fill Necessary Fields!" : error;
            lbl_fill_result.Visible = true;
        }
        protected bool push_FILL_question()
        {
            bool pushed = DB_Exams.push_Question_FILL(ddl_sector.SelectedValue,
                txt_Text1.Text, fill_1_ans1.Text.Trim().ToUpper(), fill_1_ans2.Text.Trim().ToUpper(), fill_1_ans3.Text.Trim().ToUpper(),
                txt_Text2.Text, fill_2_ans1.Text.Trim().ToUpper(), fill_2_ans2.Text.Trim().ToUpper(), fill_2_ans3.Text.Trim().ToUpper(),
                txt_Text3.Text, fill_3_ans1.Text.Trim().ToUpper(), fill_3_ans2.Text.Trim().ToUpper(), fill_3_ans3.Text.Trim().ToUpper(),
                txt_Text4.Text
                );

            return pushed;
        }
        protected bool Renew_Qestions_Grid_for_FILL()
        {
            DataTable dt = (DataTable)Session["grid_questions"];
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("SECTOR");
                dt.Columns.Add("QUESTION");
                dt.Columns.Add("ANSWER");
            }

            string q = txt_Text1.Text + " ((BLANK)) " + txt_Text2.Text + " ((BLANK)) " + txt_Text3.Text + " ((BLANK)) " + txt_Text4.Text;
            q = q.Replace(" ((BLANK))  ((BLANK)) ", " ((BLANK)) ");
            q = q.Replace(" ((BLANK))  ((BLANK)) ", " ((BLANK)) ");

            string ans = "(( " + fill_1_ans1.Text + " , " + fill_1_ans2.Text + " , " + fill_1_ans3.Text + " ))" + " " +
                         "(( " + fill_2_ans1.Text + " , " + fill_2_ans2.Text + " , " + fill_2_ans3.Text + " ))" + " " +
                         "(( " + fill_3_ans1.Text + " , " + fill_3_ans2.Text + " , " + fill_3_ans3.Text + " ))" + " ";
            ans = ans.Replace("((  ,  ,  ))", "");
            ans = ans.Replace(" ,  , ", "");



            // QUESTION , ANSWER

            DataRow row = dt.NewRow();
            row["SECTOR"] = ddl_sector.SelectedValue;
            row["QUESTION"] = q;
            row["ANSWER"] = ans;
            dt.Rows.Add(row);

            Session["grid_questions"] = dt;
            grid_questions.DataSource = dt;
            grid_questions.DataBind();

            return true;
        }


        #region OPTION QUESTIONS
        protected void ddl_qtypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddl_qtypes.SelectedValue)
            {
                case "-":
                    pnl_fill.Visible = false;
                    pnl_2op.Visible = false;
                    txt_ops_a.Text = "";
                    txt_ops_b.Text = "";
                    txt_ops_c.Text = "";
                    txt_ops_d.Text = "";
                    break;

                case "2":
                    pnl_2op.Visible = true;
                    pnl_fill.Visible = false;
                    txt_ops_c.Visible = false;
                    chk_c.Visible = false;
                    txt_ops_d.Visible = false;
                    chk_d.Visible = false;
                    txt_ops_c.Text = "";
                    txt_ops_d.Text = "";
                    break;

                case "3":
                    pnl_2op.Visible = true;
                    pnl_fill.Visible = false;
                    txt_ops_c.Visible = true;
                    chk_c.Visible = true;
                    txt_ops_d.Visible = false;
                    chk_d.Visible = false;
                    txt_ops_d.Text = "";
                    break;

                case "4":
                    pnl_2op.Visible = true;
                    pnl_fill.Visible = false;
                    txt_ops_c.Visible = true;
                    chk_c.Visible = true;
                    txt_ops_d.Visible = true;
                    chk_d.Visible = true;
                    break;

                case "FILL":
                    pnl_2op.Visible = false;
                    pnl_fill.Visible = true;
                    break;

                default:
                    break;
            }
        }
        protected void btn_questionOPS_submit_Click(object sender, EventArgs e)
        {
            //check elements
            if (!Check_OPS_Elements())
                return;


            //write to DB
            if (!push_OPS_question())
            {
                OPS_page_result("DB Error : Try Again Later !");
                return;
            }


            //put in grid
            //bool in_grid = Renew_Qestions_Grid_for_OPS();
            get_my_last_fill_grid();

            //clean eveything

            txt_question_ops.Text = "";
            txt_ops_a.Text = "";
            txt_ops_b.Text = "";
            txt_ops_c.Text = "";
            txt_ops_d.Text = "";

            OPS_page_result("Question added !");

            //ddl_qtypes.SelectedValue = "-";

        }
        protected bool push_OPS_question()
        {
            string ans = "";
            if (chk_a.Checked)
                ans = "A";
            else if (chk_b.Checked)
                ans = "B";
            else if (chk_c.Checked)
                ans = "C";
            else if (chk_d.Checked)
                ans = "D";

            string ops = ddl_qtypes.SelectedValue;

            bool pushed = DB_Exams.push_Question_OPS(ddl_sector.SelectedValue, ops, txt_question_ops.Text, ans,
                                                txt_ops_a.Text,
                                                txt_ops_b.Text,
                                                (ops == "3" || ops == "4") ? txt_ops_c.Text : null,
                                                ops == "4" ? txt_ops_d.Text : null);

            return pushed;
        }
        protected bool Check_OPS_Elements()
        {

            if (ddl_qtypes.SelectedValue == "-")
            {
                OPS_page_result();
                return false;
            }
            else if (txt_question_ops.Text == "")
            {
                OPS_page_result();
                return false;
            }
            else if (txt_ops_a.Text == "" || txt_ops_b.Text == "")
            {
                OPS_page_result();
                return false;
            }
            else if (ddl_qtypes.SelectedValue == "3" && (txt_ops_a.Text == "" || txt_ops_b.Text == "" || txt_ops_c.Text == ""))
            {
                OPS_page_result();
                return false;
            }
            else if (ddl_qtypes.SelectedValue == "4" && (txt_ops_a.Text == "" || txt_ops_b.Text == "" || txt_ops_c.Text == "" || txt_ops_d.Text == ""))
            {
                OPS_page_result();
                return false;
            }//none checked
            else if (!chk_a.Checked && !chk_b.Checked && !chk_c.Checked && !chk_d.Checked)
            {
                OPS_page_result("Choose The Answer");
                return false;
            }//non-visible checked
            else if (ddl_qtypes.SelectedValue == "2" && (!chk_a.Checked && !chk_b.Checked))
            {
                OPS_page_result("Choose The Answer");
                return false;
            }//non-visible checked
            else if (ddl_qtypes.SelectedValue == "3" && (chk_d.Checked))
            {
                OPS_page_result("Choose The Answer");
                return false;
            }

            return true;
        }
        protected void OPS_page_result(string error = "")
        {
            lbl_ops_result.Text = error == "" ? "Fill all fields and Choose an Answer!" : error;
            lbl_ops_result.Visible = true;
        }
        protected bool Renew_Qestions_Grid_for_OPS()
        {
            DataTable dt = (DataTable)Session["grid_questions"];
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("SECTOR");
                dt.Columns.Add("QUESTION");
                dt.Columns.Add("ANSWER");
            }


            string ans = "";
            if (chk_a.Checked)
                ans = txt_ops_a.Text;
            else if (chk_b.Checked)
                ans = txt_ops_b.Text;
            else if (chk_c.Checked)
                ans = txt_ops_c.Text;
            else if (chk_d.Checked)
                ans = txt_ops_d.Text;


            // QUESTION , ANSWER

            DataRow row = dt.NewRow();
            row["SECTOR"] = ddl_sector.SelectedValue;
            row["QUESTION"] = txt_question_ops.Text;
            row["ANSWER"] = ans;
            dt.Rows.Add(row);

            Session["grid_questions"] = dt;
            grid_questions.DataSource = dt;
            grid_questions.DataBind();

            return true;
        }

        protected void ddl_sector_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        protected void chk_a_CheckedChanged1(object sender, EventArgs e)
        {
            //uncheck others
            string id = ((CheckBox)sender).ID;
            if (id == "chk_a")
            {
                if (chk_a.Checked)
                {
                    chk_b.Checked = false;
                    chk_c.Checked = false;
                    chk_d.Checked = false;
                }
                else
                {
                    chk_a.Checked = true;
                }
            }
            else if (id == "chk_b")
            {
                if (chk_b.Checked)
                {
                    chk_a.Checked = false;
                    chk_c.Checked = false;
                    chk_d.Checked = false;
                }
                else
                {
                    chk_b.Checked = true;
                }
            }
            else if (id == "chk_c")
            {
                if (chk_c.Checked)
                {
                    chk_a.Checked = false;
                    chk_b.Checked = false;
                    chk_d.Checked = false;
                }
                else
                {
                    chk_c.Checked = true;
                }
            }
            else if (id == "chk_d")
            {
                if (chk_d.Checked)
                {
                    chk_a.Checked = false;
                    chk_b.Checked = false;
                    chk_c.Checked = false;
                }
                else
                {
                    chk_d.Checked = true;
                }
            }
        }

        
        
        protected void grid_questions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Assuming that the buttonField is at 1st cell, if it is diferent then change the index number accordngly
                Button btnDelete = e.Row.Cells[0].Controls[0] as Button;
                if (btnDelete != null)
                {
                    btnDelete.Attributes.Add("onclick", "Remove_Info()");
                }
            }
        }

        protected void grid_questions_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            //only DELETE Command
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_questions.Rows[index];
            string q_id = selectedRow.Cells[1].Text.Trim(); //Q_ID

            bool deleted = DB_Exams.delete_question_unless_assigned(q_id);
            if(deleted)
                get_my_last_fill_grid();
            else
            {
                lbl_fill_result.Text = "Can't DELETE : Question belongs to an exam";
                lbl_ops_result.Text = "Can't DELETE : Question belongs to an exam";
                lbl_fill_result.Visible = true;
                lbl_ops_result.Visible = true;
            }
        }

        protected void grid_questions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}