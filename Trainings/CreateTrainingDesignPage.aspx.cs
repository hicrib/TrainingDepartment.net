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
    public partial class CreateTrainingDesignPage : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            { 
                string trn_id = Convert.ToString(Request.QueryString["T"]);
                string step_id = Convert.ToString(Request.QueryString["S"]);
                if (String.IsNullOrWhiteSpace(trn_id) || (String.IsNullOrWhiteSpace(step_id)))
                    RedirectWithCode("UNAUTHORIZED");

                lbl_step_db_id.Text = step_id;
                lbl_trn_id.Text = trn_id;

                fill_page();
            }
        }

        protected void fill_page()
        {
            //fill texthtml
            DataTable dt = DB_Trainings.get_STEP_info(lbl_step_db_id.Text);
            if (dt == null || dt.Rows.Count == 0)
                return;

            upper_div.InnerHtml = dt.Rows[0]["TEXTHTML"].ToString();

            //fill Session["chosen_questions_training"]
            dt = DB_Trainings.get_STEP_questions_withAnswers(lbl_step_db_id.Text);
            if (dt == null || dt.Rows.Count == 0)
                Session["chosen_questions_training"] = null;

            Session["chosen_questions_training"] = dt;
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (file_upload.HasFile)
            {
                try
                {
                    //first delete if there is any
                    //if (lbl_step_image1.Text != "")
                    //    System.IO.File.Delete(Server.MapPath("~/Trainings/T_Images/") + lbl_step_image1.Text);

                    string filename = "training" + lbl_trn_id.Text + "_step" + lbl_step_db_id.Text + Utility.getRandomFileName();
                    string newfilename = filename + "_" + file_upload.PostedFile.FileName;
                    string file_address = Server.MapPath("~/Trainings/T_Images/") + filename + "_" + file_upload.PostedFile.FileName;
                    file_upload.SaveAs(file_address);

                    lbl_step_image1.Text = "http://trainingdepartment.net/images/" + newfilename;

                    //show the image
                    img1.Visible = true;
                    img1.ImageUrl = "~/Trainings/T_Images/" + newfilename;
                }
                catch (Exception ex)
                {
                    //statuslabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }



        protected void btn_prev_Click(object sender, EventArgs e)
        {
            //put everything in db , step and questions if there are
            bool pushed = push_step_in_db();
            if (!pushed)
                return; //Page Error

            string nextstepid = get_step_create_ifnone(next: false); //next

            if (nextstepid == "")
                return; //no prev step, do nothing
                        //cleanup
            Session["chosen_questions_training"] = null;

            go_to_training_page(nextstepid);
        }

        protected void btn_next_Click(object sender, EventArgs e)
        {
            //put everything in db , step and questions if there are
            bool pushed = push_step_in_db();
            if (!pushed)
                return; //Page Error 

            //cleanup
            Session["chosen_questions_training"] = null;

            string nextstepid = get_step_create_ifnone(next: true); //next

            go_to_training_page(nextstepid);
        }




        protected bool push_step_in_db()
        {
            //push_only_current_step
            //questions too

            if (DB_Trainings.push_Step(lbl_trn_id.Text, lbl_step_db_id.Text, "TRN", txt_upper_div_holder.Text, ""))
            {
                //establish step question list
                DataTable dt = (DataTable)Session["chosen_questions_training"];
                if (dt == null || dt.Rows.Count == 0)
                    return true; //no questions for step //todo: what if there is a fail

                List<string> q_list = new List<string>();
                foreach (DataRow row in dt.Rows)
                    q_list.Add(row["ID"].ToString());

                return DB_Trainings.push_Step_Questions(lbl_step_db_id.Text, q_list);
            }

            return false;
        }

        //creates if there is no next_step
        protected string get_step_create_ifnone(bool next)
        {
            if (next)
            {
                string stepid = DB_Trainings.get_prev_next_STEPID(lbl_trn_id.Text, lbl_step_db_id.Text, next);
                if (stepid == "")
                    return DB_Trainings.create_NEXT_STEP(lbl_trn_id.Text);
                else
                    return stepid;
            }

            //then, asking for previous, returns "" if no prev
            return DB_Trainings.get_prev_next_STEPID(lbl_trn_id.Text, lbl_step_db_id.Text, false);
        }

        protected void go_to_training_page(string stepid)
        {
            //can be next, previous
            Response.Redirect("~/Trainings/CreateTrainingDesignPage.aspx?T=" + lbl_trn_id.Text + "&S=" + stepid);
        }


        protected void btn_finish_Click(object sender, EventArgs e)
        {
            //put everything in db , step and questions if there are
            bool pushed = push_step_in_db();
            if (!pushed)
                return; //Page Error 

            //cleanup
            Session["chosen_questions_training"] = null;

            Response.Redirect("~/Trainings/CreateTrainingFinish.aspx?T=" + lbl_trn_id.Text + "&S=" + lbl_step_db_id.Text);
        }


        protected void btn_chose_question_Click(object sender, EventArgs e)
        {

        }

        protected void btn_create_question_Click(object sender, EventArgs e)
        {

        }


    }
}