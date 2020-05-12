using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Trainings
{
    public partial class UserInTraining : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserSession user = (UserSession)Session["usersession"];

                string assignid = Convert.ToString(Request.QueryString["AsID"]);
                if (String.IsNullOrWhiteSpace(assignid))
                    RedirectWithCode("UNAUTHORIZED!");

                string stepid = Convert.ToString(Request.QueryString["StepID"]);
                if (String.IsNullOrWhiteSpace(stepid))
                    RedirectWithCode("UNAUTHORIZED!");

                if (!user.isAdmin)
                    if (!Is_My_Assignment(assignid))
                        RedirectWithCode("NOT ASIGNED TO CURRENT USER");

                //check time within limits and user finished etc
                string problem = DB_Trainings.is_DOABLE_Training_Assignment(assignid, user.employeeid);
                if (!user.isAdmin)
                    if (problem != "OK")
                        RedirectWithCode(problem);



                string trn = DB_Trainings.get_TrainingName_by_assignid(assignid);
                lbl_trnid.Text = trn.Split(',')[0];
                lbl_trn_name.Text = trn.Split(',')[1];
                Write_Page_Header_Low(lbl_trn_name.Text);

                lbl_assignid.Text = assignid;
                lbl_stepid.Text = stepid;

                lbl_nextstepid.Text = DB_Trainings.get_prev_next_first_last_STEPID(lbl_trnid.Text, lbl_stepid.Text, "next");
                lbl_prevstepid.Text = DB_Trainings.get_prev_next_first_last_STEPID(lbl_trnid.Text, lbl_stepid.Text, "prev");

                if (lbl_nextstepid.Text == "")
                {
                    btn_next_step.Visible = false;
                    btn_finish_Training.Visible = true;
                }
                if (lbl_prevstepid.Text == "")
                    btn_prev_step.Visible = false;

                DB_Trainings.update_Assignment(assignid, laststepid: stepid);

                fill_step();
            }
        }
        protected bool Is_My_Assignment(string assignid)
        {
            string traineeid = DB_Trainings.whose_Assignment(assignid);

            UserSession user = (UserSession)Session["usersession"];
            if (traineeid == user.employeeid)
                return true;

            return false;
        }
        protected void fill_step()
        {
            //ROW_NUMBER()  OVER (ORDER BY s.step_id) as ORDERBY, S.STEP_ID, S.STEPTYPE , S.TEXTHTML , ISNULL(SQ.Q_ID ,'') AS 'QID', s.extra as EXAMID
            DataTable dt = DB_Trainings.get_STEP_INFO_with_questions(lbl_trnid.Text, lbl_stepid.Text);
            Session["step_info_w_q"] = dt;

            if (dt == null || dt.Rows.Count == 0)
                return; //not supposed to happen

            //check if there are questions, if there are, hide btn_next_step
            DataRow[] temp = dt.Select("QID <> '0'");
            if (temp != null && temp.Length > 0)
            {
                btn_next_step.Visible = false;
                btn_finish_Training.Visible = false;
            }
            else //no questions , hide the questions tab
            {
            }


            lbl_stepid.Text = dt.Rows[0]["STEP_ID"].ToString();

            if (dt.Rows[0]["STEPTYPE"].ToString() != "EXAM_STEP")
            {
                div_content.InnerHtml = dt.Rows[0]["TEXTHTML"].ToString();
            }
            else
            {
                // if exam_step
                btn_trn_EXAM.Visible = true;
                btn_finish_Training.Visible = false;
            }

        }

        protected void btn_prev_step_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Trainings/UserInTraining.aspx?AsID=" + lbl_assignid.Text + "&StepID=" + lbl_prevstepid.Text);
        }

        protected void btn_next_step_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Trainings/UserInTraining.aspx?AsID=" + lbl_assignid.Text + "&StepID=" + lbl_nextstepid.Text);
        }







        protected void cleanup_question()
        {
            img_a.ImageUrl = "";
            img_b.ImageUrl = "";
            img_c.ImageUrl = "";
            img_d.ImageUrl = "";
            img_a.Visible = false;
            img_b.Visible = false;
            img_c.Visible = false;
            img_d.Visible = false;
            chk_a.Checked = false;
            chk_b.Checked = false;
            chk_c.Checked = false;
            chk_d.Checked = false;

            //make all options visible because previous question shouldnt affect
            row_a.Visible = true;
            row_b.Visible = true;
            row_c.Visible = true;
            row_d.Visible = true;

            fill_text1.Visible = false;
            fill_text2.Visible = false;
            fill_text3.Visible = false;
            fill_text4.Visible = false;

            fill_fill_1.Visible = false;
            fill_fill_2.Visible = false;
            fill_fill_3.Visible = false;

        }
        protected void fill_question(DataRow ro)
        {
            lbl_current_qid.Text = ro["QID"].ToString();

            DataTable q_tbl = DB_Exams.get_ONE_question(lbl_current_qid.Text);
            if (q_tbl.Rows[0]["TYPE"].ToString() == "FILL")
                fill_fill_question(q_tbl.Rows[0]);
            else
                fill_ops_question(q_tbl.Rows[0]);


            //hide-show prev-next question
            DataTable dt = (DataTable)Session["step_info_w_q"];
            btn_next_q.Visible = dt.Select("ORDERBY = " + (Convert.ToInt32(lbl_current_orderby.Text) + 1)).Length > 0;
            btn_prev_q.Visible = dt.Select("ORDERBY = " + (Convert.ToInt32(lbl_current_orderby.Text) - 1)).Length > 0;

            //if no next question
            if (!btn_next_q.Visible)
            {
                //but there is a next step
                if (lbl_nextstepid.Text != "")
                    btn_next_step.Visible = true;
                else
                    btn_finish_Training.Visible = true;
            }
            else
                btn_next_step.Visible = false;

      

        }
        protected void fill_ops_question(DataRow n_question)
        {
            lbl_q_type.Text = n_question["TYPE"].ToString();
            panel_ops.Visible = true;
            panel_fill.Visible = false;

            lbl_ops_question.Text = n_question["Q"].ToString();
            lbl_a.Text = n_question["OPA"].ToString();
            lbl_b.Text = n_question["OPB"].ToString();
            lbl_c.Text = n_question["OPC"].ToString();
            lbl_d.Text = n_question["OPD"].ToString();
            lbl_current_answers.Text = n_question["ANSWER"].ToString();


            if (n_question["TYPE"].ToString() == "3")
                row_d.Visible = false;

            if (n_question["TYPE"].ToString() == "2")
            {
                row_c.Visible = false;
                row_d.Visible = false;
            }
        }
        protected void fill_fill_question(DataRow n_question)
        {
            lbl_q_type.Text = "FILL";
            panel_fill.Visible = true;
            panel_ops.Visible = false;

            fill_text1.Text = n_question["TEXT1"].ToString();
            fill_text1.Visible = true;

            lbl_current_answers.Text = "((" + n_question["FILL1_ANS1"].ToString() + "|" +
                                        n_question["FILL1_ANS2"].ToString() + "|" +
                                        n_question["FILL1_ANS3"].ToString() + "))" + "-" +
                                        "((" + n_question["FILL2_ANS1"].ToString() + "|" +
                                        n_question["FILL2_ANS2"].ToString() + "|" +
                                        n_question["FILL2_ANS3"].ToString() + "))" + "-" +
                                        "((" + n_question["FILL3_ANS1"].ToString() + "|" +
                                        n_question["FILL3_ANS2"].ToString() + "|" +
                                        n_question["FILL3_ANS3"].ToString() + "))";

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
        protected void btn_next_q_Click(object sender, EventArgs e)
        {
            cleanup_question();

            lbl_current_orderby.Text = (Convert.ToInt32(lbl_current_orderby.Text) + 1).ToString();

            DataTable dt = (DataTable)Session["step_info_w_q"];
            if (dt == null || dt.Rows.Count == 0 || dt.Rows[0]["QID"].ToString() == "0")
                return;

            DataRow ro = dt.Select("ORDERBY = " + lbl_current_orderby.Text)[0];

            fill_question(ro);
        }
        protected void btn_prev_q_Click(object sender, EventArgs e)
        {
            cleanup_question();

            lbl_current_orderby.Text = (Convert.ToInt32(lbl_current_orderby.Text) - 1).ToString();

            DataTable dt = (DataTable)Session["step_info_w_q"];
            if (dt == null || dt.Rows.Count == 0 || dt.Rows[0]["QID"].ToString() == "0")
                return;

            DataRow ro = dt.Select("ORDERBY = " + lbl_current_orderby.Text)[0];
            fill_question(ro);
        }
        protected void btn_show_answer_Click(object sender, EventArgs e)
        {
            if (lbl_q_type.Text == "FILL")
            {
                //for 1st fill
                string correct1 = lbl_current_answers.Text.Split('-')[0].Trim('(').Trim('(').Trim(')').Trim(')');
                if (fill_fill_1.Text.Trim().ToUpper() == correct1.Split('|')[0].Trim().ToUpper()
                    || fill_fill_1.Text.Trim().ToUpper() == correct1.Split('|')[1].Trim().ToUpper()
                    || fill_fill_1.Text.Trim().ToUpper() == correct1.Split('|')[2].Trim().ToUpper())
                {
                    img_fill1.ImageUrl = "~/images/tick_green_small.png";
                    img_fill1.Visible = true;
                }
                else
                {
                    img_fill1.ImageUrl = "~/images/cross_red_small.png";
                    img_fill1.Visible = true;
                }
                //fill correct answers
                if (correct1.Split('|')[0].Trim().ToUpper() != "")
                    ddl_fill1.Items.Add(correct1.Split('|')[0].Trim().ToUpper());
                if (correct1.Split('|')[1].Trim().ToUpper() != "")
                    ddl_fill1.Items.Add(correct1.Split('|')[1].Trim().ToUpper());
                if (correct1.Split('|')[2].Trim().ToUpper() != "")
                    ddl_fill1.Items.Add(correct1.Split('|')[2].Trim().ToUpper());
                ddl_fill1.Visible = true;


                //check if there's a second answer
                string correct2 = lbl_current_answers.Text.Split('-')[1].Trim('(').Trim('(').Trim(')').Trim(')');
                if (correct2.Replace("|", "") != "")
                {
                    if (fill_fill_2.Text.Trim().ToUpper() == correct2.Split('|')[0].Trim().ToUpper()
                    || fill_fill_2.Text.Trim().ToUpper() == correct2.Split('|')[1].Trim().ToUpper()
                    || fill_fill_2.Text.Trim().ToUpper() == correct2.Split('|')[2].Trim().ToUpper())
                    {
                        img_fill2.ImageUrl = "~/images/tick_green_small.png";
                        img_fill2.Visible = true;
                    }
                    else
                    {
                        img_fill2.ImageUrl = "~/images/cross_red_small.png";
                        img_fill2.Visible = true;
                    }

                    //correct answers
                    if (correct2.Split('|')[0].Trim().ToUpper() != "")
                        ddl_fill2.Items.Add(correct2.Split('|')[0].Trim().ToUpper());
                    if (correct2.Split('|')[1].Trim().ToUpper() != "")
                        ddl_fill2.Items.Add(correct2.Split('|')[1].Trim().ToUpper());
                    if (correct2.Split('|')[2].Trim().ToUpper() != "")
                        ddl_fill2.Items.Add(correct2.Split('|')[2].Trim().ToUpper());
                    ddl_fill2.Visible = true;
                }

                //check if there's a third answer
                string correct3 = lbl_current_answers.Text.Split('-')[2].Trim('(').Trim('(').Trim(')').Trim(')');
                if (correct3.Replace("|", "") != "")
                {
                    if (fill_fill_3.Text.Trim().ToUpper() == correct3.Split('|')[0].Trim().ToUpper()
                    || fill_fill_3.Text.Trim().ToUpper() == correct3.Split('|')[1].Trim().ToUpper()
                    || fill_fill_3.Text.Trim().ToUpper() == correct3.Split('|')[2].Trim().ToUpper())
                    {
                        img_fill3.ImageUrl = "~/images/tick_green_small.png";
                        img_fill3.Visible = true;
                    }
                    else
                    {
                        img_fill3.ImageUrl = "~/images/cross_red_small.png";
                        img_fill3.Visible = true;
                    }

                    //correct answers
                    if (correct3.Split('|')[0].Trim().ToUpper() != "")
                        ddl_fill3.Items.Add(correct3.Split('|')[0].Trim().ToUpper());
                    if (correct3.Split('|')[1].Trim().ToUpper() != "")
                        ddl_fill3.Items.Add(correct3.Split('|')[1].Trim().ToUpper());
                    if (correct3.Split('|')[2].Trim().ToUpper() != "")
                        ddl_fill3.Items.Add(correct3.Split('|')[2].Trim().ToUpper());
                    ddl_fill3.Visible = true;
                }

            }
            else //ops
            {

                //check if user has done right
                if (chk_a.Checked)
                {
                    if (lbl_current_answers.Text == "A")
                    {
                        img_a.ImageUrl = "~/images/tick_green.png";
                        img_a.Visible = true;
                    }
                    else
                    {
                        img_a.ImageUrl = "~/images/cross_red.png";
                        img_a.Visible = true;
                    }
                }
                else if (chk_b.Checked)
                {
                    if (lbl_current_answers.Text == "B")
                    {
                        img_b.ImageUrl = "~/images/tick_green.png";
                        img_b.Visible = true;
                    }
                    else
                    {
                        img_b.ImageUrl = "~/images/cross_red.png";
                        img_b.Visible = true;
                    }
                }
                else if (chk_c.Checked)
                {
                    if (lbl_current_answers.Text == "C")
                    {
                        img_c.ImageUrl = "~/images/tick_green.png";
                        img_c.Visible = true;
                    }
                    else
                    {
                        img_c.ImageUrl = "~/images/cross_red.png";
                        img_c.Visible = true;
                    }
                }
                else if (chk_d.Checked)
                {
                    if (lbl_current_answers.Text == "D")
                    {
                        img_d.ImageUrl = "~/images/tick_green.png";
                        img_d.Visible = true;
                    }
                    else
                    {
                        img_d.ImageUrl = "~/images/cross_red.png";
                        img_d.Visible = true;
                    }
                }

                // show the correct answer
                if (lbl_current_answers.Text == "A")
                {
                    img_a.ImageUrl = "~/images/tick_green.png";
                    img_a.Visible = true;
                }
                else if (lbl_current_answers.Text == "B")
                {
                    img_b.ImageUrl = "~/images/tick_green.png";
                    img_b.Visible = true;
                }
                else if (lbl_current_answers.Text == "C")
                {
                    img_c.ImageUrl = "~/images/tick_green.png";
                    img_c.Visible = true;
                }
                else if (lbl_current_answers.Text == "D")
                {
                    img_d.ImageUrl = "~/images/tick_green.png";
                    img_d.Visible = true;
                }
            }
        }

        protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
        {
            int index = Int32.Parse(e.Item.Value);
            MultiView1.ActiveViewIndex = index;

            //questions
            if (index == 1)
            {
                //fill the first question if there is
                DataTable dt = (DataTable)Session["step_info_w_q"];
                if (dt == null || dt.Rows.Count == 0 || dt.Rows[0]["QID"].ToString() == "0")
                {
                    panel_fill.Visible = false;
                    panel_ops.Visible = false;
                    panel_q_btn.Visible = false;
                    return;
                }

                DataRow ro = dt.Select("ORDERBY = " + lbl_current_orderby.Text)[0];

                fill_question(ro);
            }

        }






        protected void btn_trn_EXAM_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["step_info_w_q"];
            UserSession user = (UserSession)Session["usersession"];
            //create assignmnet
            string examassignid = DB_Exams.push_EXAM_Assignment(dt.Rows[0]["EXAMID"].ToString(), user.employeeid, "1900-01-01", "2099-01-01",trn_assignid:lbl_assignid.Text);

            DB_Trainings.update_Assignment(lbl_assignid.Text, examassignid: examassignid);
            //redirect with special flag
            Session["from_training"] = lbl_assignid.Text;
            Response.Redirect("~/Exams/UserInExam.aspx?AsID=" + examassignid);
        }

        protected void btn_finish_Training_Click(object sender, EventArgs e)
        {
            //update status 
            DB_Trainings.update_Assignment(lbl_assignid.Text, status: "FINISHED", userfinish: "now");
            SuccessWithCode("TRAINING FINISHED !");
        }
    }
}