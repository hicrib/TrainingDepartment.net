using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Exams
{
    public partial class EditQuestions : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("EDIT QUESTIONS"); 
                ddl_by.DataSource = DB_System.get_ALL_Users(true, false);
                ddl_by.DataTextField = "NAME";
                ddl_by.DataValueField = "ID";
                ddl_by.DataBind();

            }
        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            //fill grid
            Fill_Grid();
        }
        protected void Fill_Grid()
        {
            grid_questions.DataSource = DB_Exams.get_questions_withAnswer(sector: ddl_sector.SelectedValue == "ALL" ? "" : ddl_sector.SelectedValue,
                                                                            creater: ddl_by.SelectedValue == "-" ? "" : ddl_by.SelectedValue,
                                                                            txt_qtext.Text ,chk_active.Checked);
            grid_questions.DataBind();
        }


        protected void grid_questions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 3)
                e.Row.Cells[2].Visible = false; // hide ISACTIVE
        }

        protected void grid_questions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EDIT_Q")
                return;

            //new question will be edited. Clean 
            lbl_page_result.Text = "";
            lbl_page_result.Visible = false;
            btn_push_question.Visible = true;

            //get q_info
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_questions.Rows[index];
            

            //highlight selected
            foreach (GridViewRow item in grid_questions.Rows)
                item.BackColor = System.Drawing.Color.Empty;
            selectedRow.BackColor = System.Drawing.Color.LightGreen;

            //fill this for using in other functions
            lbl_edit_qid.Text = selectedRow.Cells[3].Text.Trim();


            //get the question and options , blanks etc
            DataTable dt = DB_Exams.get_ONE_question(lbl_edit_qid.Text);
            if (dt == null || dt.Rows.Count == 0)
                return;
            DataRow q_info = dt.Rows[0];

            if (q_info["TYPE"].ToString() == "FILL")
            {
                lbl_edit_qtype.Text = "FILL";

                pnl_fill.Visible = true;
                pnl_2op.Visible = false;

                txt_Text1.Text = q_info["TEXT1"].ToString();
                txt_Text2.Text = q_info["TEXT2"].ToString();
                txt_Text3.Text = q_info["TEXT3"].ToString();
                txt_Text4.Text = q_info["TEXT4"].ToString();

                fill_1_ans1.Text = q_info["FILL1_ANS1"].ToString();
                fill_1_ans2.Text = q_info["FILL1_ANS2"].ToString();
                fill_1_ans3.Text = q_info["FILL1_ANS3"].ToString();
                fill_2_ans1.Text = q_info["FILL2_ANS1"].ToString();
                fill_2_ans2.Text = q_info["FILL2_ANS2"].ToString();
                fill_2_ans3.Text = q_info["FILL2_ANS3"].ToString();
                fill_3_ans1.Text = q_info["FILL3_ANS1"].ToString();
                fill_3_ans2.Text = q_info["FILL3_ANS2"].ToString();
                fill_3_ans3.Text = q_info["FILL3_ANS3"].ToString();
            }
            else
            {
                lbl_edit_qtype.Text = q_info["TYPE"].ToString();

                pnl_fill.Visible = false;
                pnl_2op.Visible = true;
                //option Visibility
                row__a.Visible = true;
                row__b.Visible = true;
                row__c.Visible = true;
                row__d.Visible = true;

                txt_question_ops.Text = q_info["Q"].ToString();
                txt_ops_a.Text = q_info["OPA"].ToString();
                txt_ops_b.Text = q_info["OPB"].ToString();
                txt_ops_c.Text = q_info["OPC"].ToString();
                txt_ops_d.Text = q_info["OPD"].ToString();

                if (lbl_edit_qtype.Text == "2")
                {
                    row__c.Visible = false;
                    row__d.Visible = false;
                }
                else if (lbl_edit_qtype.Text == "3")
                    row__d.Visible = false;

                string ans = q_info["ANSWER"].ToString();
                chk_a.Checked = ans == "A";
                chk_b.Checked = ans == "B";
                chk_c.Checked = ans == "C";
                chk_d.Checked = ans == "D";
            }
        }

        protected void grid_questions_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grid_questions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Gridview'in PageIndexChanging Eventında grid'in PageIndex'ine seçilen
            // sayfanın numarası atanır.
            grid_questions.PageIndex = e.NewPageIndex;

            Fill_Grid();
        }

        protected void chk_b_CheckedChanged(object sender, EventArgs e)
        {
            foreach (CheckBox item in new CheckBox[] { chk_a, chk_b, chk_c, chk_d })
            {
                item.Checked = false;
                if (item.ID == ((CheckBox)sender).ID)
                    item.Checked = true;
            }
        }

        protected void btn_push_question_Click(object sender, EventArgs e)
        {
            if (lbl_edit_qid.Text == "" || lbl_edit_qtype.Text == "")
            {
                lbl_page_result.Visible = true;
                lbl_page_result.Text = "First Choose a question and edit";
                return;
            }




            bool pushed = false;
            //write question
            if (lbl_edit_qtype.Text == "FILL")
            {
                pushed = DB_Exams.update_Question_FILL(lbl_edit_qid.Text,
                txt_Text1.Text, fill_1_ans1.Text.Trim().ToUpper(), fill_1_ans2.Text.Trim().ToUpper(), fill_1_ans3.Text.Trim().ToUpper(),
                txt_Text2.Text, fill_2_ans1.Text.Trim().ToUpper(), fill_2_ans2.Text.Trim().ToUpper(), fill_2_ans3.Text.Trim().ToUpper(),
                txt_Text3.Text, fill_3_ans1.Text.Trim().ToUpper(), fill_3_ans2.Text.Trim().ToUpper(), fill_3_ans3.Text.Trim().ToUpper(),
                txt_Text4.Text
                );
            }
            else
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

                string ops = lbl_edit_qtype.Text;

                pushed = DB_Exams.update_Question_OPS(lbl_edit_qid.Text, txt_question_ops.Text, ans,
                                                    txt_ops_a.Text,
                                                    txt_ops_b.Text,
                                                    (ops == "3" || ops == "4") ? txt_ops_c.Text : null,
                                                    ops == "4" ? txt_ops_d.Text : null);
            }

            if (pushed)
            {
                lbl_page_result.Visible = true;
                lbl_page_result.Text = "Success : Question Saved!";
                btn_push_question.Visible = false;

                //cleanup
                pnl_2op.Visible = false;
                pnl_fill.Visible = false;
                lbl_edit_qid.Text = "";
                lbl_edit_qtype.Text = "";
            }
            else
            {
                lbl_page_result.Visible = true;
                lbl_page_result.Text = "Error : Something went wrong. Try again later.";
            }

            Fill_Grid();

            //refill table
        }

        protected void chk_q_active_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk_qActive;
            HiddenField IdHiddenField;

            chk_qActive = (CheckBox)sender;

            IdHiddenField = (HiddenField)chk_qActive.Parent.FindControl("QID_hiddenfield");

            //deletes or undeletes
            bool ok = DB_Exams.delete_question_unless_assigned(IdHiddenField.Value, undelete: ((CheckBox)sender).Checked);

            if(!ok)
            {
                ((CheckBox)sender).Checked = !((CheckBox)sender).Checked;
                ((CheckBox)sender).Enabled = false;
            }
        }

       
    }
}