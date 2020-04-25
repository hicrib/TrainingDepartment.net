using AviaTrain.App_Code;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Exams
{
    public partial class UserInExam : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                UserSession user = (UserSession)Session["usersession"];

                string assignid = Convert.ToString(Request.QueryString["AsID"]);
                if (String.IsNullOrWhiteSpace(assignid))
                    RedirectWithCode("UNAUTHORIZED!");

                lbl_assignid.Text = assignid;
                lbl_examname.Text = DB_Trainings.get_Exam_Name_by_assignid(assignid);

                if (!user.isAdmin)
                    if (!Is_My_Assignment(assignid))
                        RedirectWithCode("NOT ASIGNED TO CURRENT USER");

                //check time within limits and user finished etc
                string problem = DB_Trainings.is_DOABLE_Exam_Assignment(assignid, user.employeeid);
                if (!user.isAdmin)
                    if (problem != "OK")
                        RedirectWithCode(problem);




                DB_Trainings.update_Exam_Assignment_USERSTART(assignid);
                DB_Trainings.update_Exam_Assignment_STATUS(assignid, "USER_STARTED");


                Fill_grid_questions_map();

                lbl_current_orderby.Text = "0"; //it'll become 1 when filling for next
                Fill_Next_Question();
            }
        }

        protected bool Is_My_Assignment(string assignid)
        {
            string traineeid = DB_Trainings.whose_Assignment(lbl_assignid.Text);

            UserSession user = (UserSession)Session["usersession"];
            if (traineeid == user.employeeid)
                return true;

            return false;
        }


        protected string get_next_qid()
        {
            DataTable dt = (DataTable)Session["current_exam_questions"];

            //maybe there is no next, return 0
            if (lbl_total_questions.Text == lbl_current_orderby.Text)
                return "0";

            string nextorderby = (Convert.ToInt32(lbl_current_orderby.Text) + 1).ToString();

            DataRow[] dr = dt.Select("Question = " + nextorderby);
            return dr[0]["Q_ID"].ToString();
        }

        protected string get_orderby_of_qid(string qid)
        {
            DataTable dt = (DataTable)Session["current_exam_questions"];

            DataRow[] dr = dt.Select("Q_ID = " + qid);
            return dr[0]["Question"].ToString();
        }

        protected void Fill_grid_questions_map()
        {
            DataTable dt = DB_Trainings.get_EXAM_QUESTIONS_by_assignid(lbl_assignid.Text);
            if (dt == null || dt.Rows.Count == 0)
                RedirectWithCode();

            grid_questions_map.DataSource = dt;
            grid_questions_map.DataBind();

            Session["current_exam_questions"] = dt;
            lbl_total_questions.Text = dt.Rows.Count.ToString();
        }

        protected void grid_questions_map_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //todo
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_questions_map.Rows[index];

            //get q_id of the desired question
            string qid = selectedRow.Cells[2].Text;


            //push
            bool pushed = push_answer();

            if (pushed)
                mark_question_solved();

            //cleanup page 
            Clean_up_page();


            //show next question
            Fill_Next_Question(qid);


            //bring answer if solved before
            string solved = selectedRow.Cells[3].Text;
            if (solved == "1")
            {
                DataTable dt = DB_Trainings.get_USER_ANSWER_of_Question(lbl_assignid.Text, qid);
                if (dt == null || dt.Rows.Count == 0)
                    return;

                string q_type = dt.Rows[0]["TYPE"].ToString();
                if (q_type == "FILL")
                {
                    fill_fill_1.Text = dt.Rows[0]["ANSWER1"].ToString();
                    fill_fill_2.Text = dt.Rows[0]["ANSWER2"].ToString();
                    fill_fill_3.Text = dt.Rows[0]["ANSWER3"].ToString();
                }
                else if (q_type == "2" || q_type == "3" || q_type == "4")
                {
                    chk_a.Checked = (dt.Rows[0]["ANSWER1"].ToString()) == "A";
                    chk_b.Checked = (dt.Rows[0]["ANSWER1"].ToString()) == "B";
                    chk_c.Checked = (dt.Rows[0]["ANSWER1"].ToString()) == "C";
                    chk_d.Checked = (dt.Rows[0]["ANSWER1"].ToString()) == "D";
                }
            }

        }

        protected void grid_questions_map_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            e.Row.Cells[2].Visible = false; // hide Q_ID
            e.Row.Cells[3].Visible = false; // hide SOLVED

            if (e.Row.Cells[3].Text == "1")
                e.Row.BackColor = System.Drawing.Color.LightGreen;

        }


        protected void Fill_Next_Question(string next_qid = "")
        {
            if (next_qid == "")
                next_qid = get_next_qid();

            //there is no next, don't do anything. Hide Next button
            if (next_qid == "0")
            {
                btn_save_n_next.Visible = false;
                return;
            }
            else
                btn_save_n_next.Visible = true;



            DataTable dt = DB_Trainings.get_ONE_question(next_qid);

            if (dt == null || dt.Rows.Count == 0)
                return; //why what?

            DataRow n_question = dt.Rows[0];

            if (n_question["TYPE"].ToString() == "FILL")
            {
                panel_fill.Visible = true;
                panel_ops.Visible = false;

                fill_text1.Text = n_question["TEXT1"].ToString();
                fill_text1.Visible = true;

                // answers for blank 1, show textbox
                if ("" != n_question["FILL1_ANS1"].ToString() + n_question["FILL1_ANS2"].ToString() + n_question["FILL1_ANS3"].ToString())
                    fill_fill_1.Visible = true;

                if (n_question["TEXT2"].ToString() != "")
                {
                    fill_text2.Text = n_question["TEXT2"].ToString();
                    fill_text2.Visible = true;
                }

                // answers for blank 1, show textbox
                if ("" != n_question["FILL2_ANS1"].ToString() + n_question["FILL2_ANS2"].ToString() + n_question["FILL2_ANS3"].ToString())
                    fill_fill_2.Visible = true;

                if (n_question["TEXT3"].ToString() != "")
                {
                    fill_text3.Text = n_question["TEXT3"].ToString();
                    fill_text3.Visible = true;
                }

                // answers for blank 1, show textbox
                if ("" != n_question["FILL3_ANS1"].ToString() + n_question["FILL3_ANS2"].ToString() + n_question["FILL3_ANS3"].ToString())
                    fill_fill_3.Visible = true;

                if (n_question["TEXT4"].ToString() != "")
                {
                    fill_text4.Text = n_question["TEXT4"].ToString();
                    fill_text4.Visible = true;
                }

            }
            else if (n_question["TYPE"].ToString() == "2" || n_question["TYPE"].ToString() == "3" || n_question["TYPE"].ToString() == "4")
            {
                panel_ops.Visible = true;
                panel_fill.Visible = false;

                lbl_ops_question.Text = n_question["Q"].ToString();
                lbl_a.Text = n_question["OPA"].ToString();
                lbl_b.Text = n_question["OPB"].ToString();
                lbl_c.Text = n_question["OPC"].ToString();
                lbl_d.Text = n_question["OPD"].ToString();

                if (n_question["TYPE"].ToString() == "3")
                    row_d.Visible = false;

                if (n_question["TYPE"].ToString() == "2")
                {
                    row_c.Visible = false;
                    row_d.Visible = false;
                }
            }


            //fill the necessary parameters about page for the new question
            lbl_q_type.Text = n_question["TYPE"].ToString();
            lbl_current_orderby.Text = get_orderby_of_qid(next_qid);
            lbl_current_qid.Text = next_qid;
        }


        protected void chk_a_CheckedChanged(object sender, EventArgs e)
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







        protected void btn_save_n_next_Click(object sender, EventArgs e)
        {
            bool pushed = false;

            // save the answer into db
            pushed = push_answer();

            if (pushed)
                mark_question_solved();

            Clean_up_page();

            //fire next_question fill
            Fill_Next_Question();
        }

        protected void mark_question_solved()
        {
            DataTable dt = (DataTable)Session["current_exam_questions"];

            foreach (DataRow item in dt.Rows)
            {
                if (item["Question"].ToString() == lbl_current_orderby.Text)
                    item["SOLVED"] = "1";

                //does it update like this?
            }
            Session["current_exam_questions"] = dt;
            grid_questions_map.DataSource = dt;
            grid_questions_map.DataBind();
        }

        protected bool push_answer()
        {

            bool pushed = false;

            if (lbl_q_type.Text == "FILL")
            {

                if (fill_fill_1.Text.Trim() == "" && fill_fill_2.Text.Trim() == "" && fill_fill_3.Text.Trim() == "")
                    pushed = false;
                else
                    pushed = DB_Trainings.push_Exam_Answer(lbl_assignid.Text, lbl_current_qid.Text, fill_fill_1.Text.Trim().ToUpper(), fill_fill_2.Text.Trim().ToUpper(), fill_fill_3.Text.Trim().ToUpper());

            }
            else if (lbl_q_type.Text == "2" || lbl_q_type.Text == "3" || lbl_q_type.Text == "4")
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

                if (ans != "")
                    pushed = DB_Trainings.push_Exam_Answer(lbl_assignid.Text, lbl_current_qid.Text, ans, "", "");
                else
                    pushed = false;
            }

            return pushed;
        }
        protected void Clean_up_page()
        {

            //clean-up
            panel_fill.Visible = false;
            fill_text1.Text = "";
            fill_text2.Text = "";
            fill_text3.Text = "";
            fill_text4.Text = "";
            fill_text1.Visible = false;
            fill_text2.Visible = false;
            fill_text3.Visible = false;
            fill_text4.Visible = false;
            fill_fill_1.Text = "";
            fill_fill_2.Text = "";
            fill_fill_3.Text = "";
            fill_fill_1.Visible = false;
            fill_fill_2.Visible = false;
            fill_fill_3.Visible = false;

            panel_ops.Visible = false;
            lbl_ops_question.Text = "";
            lbl_a.Text = "";
            lbl_b.Text = "";
            lbl_c.Text = "";
            lbl_d.Text = "";
            row_c.Visible = true;
            row_d.Visible = true;
            chk_a.Checked = false;
            chk_b.Checked = false;
            chk_c.Checked = false;
            chk_d.Checked = false;
        }





        protected void btn_finish_exam_Click(object sender, EventArgs e)
        {
            //todo : implement

            //get db :   q_id, q_type , q_point -- userANS1,ANS2, ANS3,  --REAL ANS1, ANS2,ANS3
            DataTable questions = DB_Trainings.get_ALL_USER_ANSWERS_of_ASSIGNMENT(lbl_assignid.Text);

            float total_exam_points = 0;
            float total_user_points = 0;

            foreach (DataRow row in questions.Rows)
            {
                string q_id = row["Q_ID"].ToString();
                string q_type = row["TYPE"].ToString();
                float q_point = Convert.ToInt32(row["POINT"]);

                string user_ans1 = row["user_ans1"].ToString().ToUpper(); //ops uses only this
                string user_ans2 = row["user_ans2"].ToString().ToUpper();
                string user_ans3 = row["user_ans3"].ToString().ToUpper();

                string real_ans1_acc1 = row["real_ans1_acc1"].ToString().ToUpper(); //ops uses only this
                string real_ans1_acc2 = row["real_ans1_acc2"].ToString().ToUpper();
                string real_ans1_acc3 = row["real_ans1_acc3"].ToString().ToUpper();

                string real_ans2_acc1 = row["real_ans2_acc1"].ToString().ToUpper();
                string real_ans2_acc2 = row["real_ans2_acc2"].ToString().ToUpper();
                string real_ans2_acc3 = row["real_ans2_acc3"].ToString().ToUpper();

                string real_ans3_acc1 = row["real_ans3_acc1"].ToString().ToUpper();
                string real_ans3_acc2 = row["real_ans3_acc2"].ToString().ToUpper();
                string real_ans3_acc3 = row["real_ans3_acc3"].ToString().ToUpper();

                total_exam_points += q_point; //maybe total isn't 100

                if (q_type == "2" || q_type == "3" || q_type == "4")
                {
                    if (real_ans1_acc1 == user_ans1)
                        total_user_points += q_point;
                }
                else if (q_type == "FILL")
                {
                    //howmany blanks to be filled in question definition
                    int howmany_blank = 0;
                    if (real_ans1_acc1 + real_ans1_acc2 + real_ans1_acc3 != "")
                        howmany_blank++;
                    if (real_ans2_acc1 + real_ans2_acc2 + real_ans2_acc3 != "")
                        howmany_blank++;
                    if (real_ans3_acc1 + real_ans3_acc2 + real_ans3_acc3 != "")
                        howmany_blank++;

                    if (howmany_blank > 0)
                        if (user_ans1 == real_ans1_acc1 || user_ans1 == real_ans1_acc2 || user_ans1 == real_ans1_acc3)
                            total_user_points += (q_point / (float)howmany_blank);
                    if (howmany_blank > 1)
                        if (user_ans2 == real_ans2_acc1 || user_ans2 == real_ans2_acc2 || user_ans2 == real_ans2_acc3)
                            total_user_points += (q_point / (float)howmany_blank);
                    if (howmany_blank > 2)
                        if (user_ans3 == real_ans3_acc1 || user_ans3 == real_ans3_acc2 || user_ans3 == real_ans3_acc3)
                            total_user_points += (q_point / (float)howmany_blank);
                }
            }

            float grade = ((total_user_points / total_exam_points) * 100);

            string dummy = (lbl_examname.Text.Split('%')[0]);
            string passpercent = dummy.Substring(dummy.Length - 2, 2);
            string passfail = "";
            if (Convert.ToDecimal(passpercent) > Convert.ToDecimal(grade))
                passfail = "FAILED";
            else
                passfail = "PASSED";


                if (!DB_Trainings.update_Exam_Assignment_STATUS(lbl_assignid.Text, passfail))
                RedirectWithCode("System Error : Exam Status can't be updated");

            if (!DB_Trainings.update_Exam_Assignment_USERFINISH(lbl_assignid.Text))
                RedirectWithCode("System Error : Exam Finish time can't be updated");

            if (!DB_Trainings.update_Exam_Assignment_GRADE(lbl_assignid.Text, grade.ToString("0.00")))
                RedirectWithCode("System Error : Exam grade can't be updated");

            //send the results to result page
            Dictionary<string, string> exam_result = new Dictionary<string, string>();
            exam_result.Add("assignid", lbl_assignid.Text);
            exam_result.Add("total_exam_points", Convert.ToInt32(total_exam_points).ToString());
            exam_result.Add("total_user_points", Convert.ToInt32(total_user_points).ToString());
            exam_result.Add("grade", grade.ToString("0.00"));
            exam_result.Add("exam_name", lbl_examname.Text);

            Session["exam_result"] = exam_result;
            Response.Redirect("~/Exams/UserExamResult.aspx");

        }





    }
}