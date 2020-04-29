using AviaTrain.App_Code;
using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Trainings
{
    public partial class CreateTrainingFinish : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // querystring T , S(last_step_id must create new one"

            if (!IsPostBack)
            { 
                string t = Convert.ToString(Request.QueryString["T"]);
                string s = Convert.ToString(Request.QueryString["S"]);
                if (t == "" || s == "")
                    RedirectWithCode("UNAUTHORIZED!");

                lbl_trnid.Text = t;
                lbl_prev_stepid.Text = s;

                fill_exams();

            }


        }
        protected void fill_exams()
        {
            ddl_exams.DataSource = DB_Exams.get_Exams();
            ddl_exams.DataTextField = "NAME";
            ddl_exams.DataValueField = "ID";
            ddl_exams.DataBind();
        }

        protected void chk_addexam_CheckedChanged(object sender, EventArgs e)
        {
            panel_add_exam.Visible = chk_addexam.Checked;
        }

        protected void btn_update_exams_Click(object sender, EventArgs e)
        {
            fill_exams();
        }

        protected void btn_finish_training_Click(object sender, EventArgs e)
        {

            if (chk_addexam.Checked)
            {
                //check page elements
                if (ddl_exams.SelectedValue == "0" || ddl_exams.SelectedValue == "" || ddl_exams.SelectedValue == "-")
                {
                    lbl_pageresult.Text = "You should choose an exam";
                    lbl_pageresult.Visible = true;
                    return;
                }


                // TODO : IF COMES HERE wıth url link AFTER AN ERROR, 2 EXAM_STEP IS CREATED. 
                //get new step 
                string stepid = DB_Trainings.create_NEXT_STEP(lbl_trnid.Text);

                //make status=EXAM_STEP , EXTRA=EXAMID
                bool step_updated = DB_Trainings.update_STEP(stepid, "EXAM_STEP", "", "", "", ddl_exams.SelectedValue, "");

                if (!step_updated)
                {
                    lbl_pageresult.Text = "System Error : Exam can't be added (Your Training is NOT LOST. You can continue later.";
                    lbl_pageresult.Visible = true;
                    return;
                }
            }
            UserSession user = (UserSession)Session["usersession"];
            bool ok = DB_Trainings.update_TRAINING_DEF(lbl_trnid.Text, user.employeeid, "now", "DESIGN_FINISHED", "", "1");

            if (!ok)
            {
                lbl_pageresult.Text = "System Error : Your Training is NOT LOST. You can continue later.";
                lbl_pageresult.Visible = true;
                return;
            }
            else
                SuccessWithCode("SUCCESS : Training Created" + (chk_addexam.Checked ? " with Exam !" : "!" ));

        }


    }
}
