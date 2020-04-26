using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
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
                //admin kontrol
                UserSession user = (UserSession)Session["usersession"];

                if (!(user.isAdmin || user.isExamAdmin))
                    RedirectWithCode("UNAUTHORIZED");

                string trn_id = Convert.ToString(Request.QueryString["T"]);
                string step_id = Convert.ToString(Request.QueryString["S"]);
                if (String.IsNullOrWhiteSpace(trn_id) || (String.IsNullOrWhiteSpace(step_id)))
                    RedirectWithCode("UNAUTHORIZED");

                lbl_step_db_id.Text = step_id;
                lbl_trn_id.Text = trn_id;
            }
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
            // put everything in db, get previous stepid

            //get previous page content 
        }

       

        protected void btn_next_Click(object sender, EventArgs e)
        {
            //put everything in db , get next stepid
            bool pushed= push_step_in_db();

            string next_step = get_step("next");

            //clean session chosen questions
            if (!pushed)
                ;//error! ;

            go_to_training_page();

            //send it to next step design
        }

        protected void btn_finish_Click(object sender, EventArgs e)
        {
            //put everythin in db

            //clean session chosen questions

            //send to success page
        }

        protected void btn_chose_question_Click(object sender, EventArgs e)
        {

        }

        protected void btn_create_question_Click(object sender, EventArgs e)
        {

        }


        protected bool push_step_in_db()
        {
            //push_only_current_step
            //questions too

            return false;
        }
        protected string get_step(string next_prev)
        {
            if(next_prev == "next")
            {

            }
        }

        protected void go_to_training_page(string stepid)
        {
            //can be next, previous
        }

    }
}