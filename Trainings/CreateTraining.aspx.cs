using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AviaTrain.App_Code;
using Microsoft.Ajax.Utilities;

namespace AviaTrain.Trainings
{
    public partial class CreateTraining : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string trnid = Convert.ToString(Request.QueryString["ID"]);
                string trnname = Convert.ToString(Request.QueryString["NAME"]);

                if (String.IsNullOrWhiteSpace(trnid) || String.IsNullOrWhiteSpace(trnname))
                    Write_Page_Header_Low("CREATE TRAINING PACKAGE");
                else
                {
                    lbl_trnid.Text = trnid;
                    btn_create_trn.Visible = false;
                    btn_saveas.Visible = true;
                    Write_Page_Header_Low("SAVE AS : " + trnname);
                }


                
            }

        }
        protected bool check_page_elements()
        {
            if (txt_trainingname.Text.Trim() == "")
            {
                lbl_info_error.Text = "Training Name is empty";
                lbl_info_error.Visible = true;
                return false;
            }
            if (txt_effective.Text.Trim() == "")
            {//todo : this doesnt work properly
                lbl_info_error.Text = "Effective Date is empty";
                lbl_info_error.Visible = true;
                return false;
            }
            DataTable dt = DB_Trainings.get_TrainingNames(false);
            if (dt.Select("NAME = '" + txt_trainingname.Text.Trim() + "'").Length > 0)
            {
                lbl_info_error.Text = "Training Name is already taken.";
                lbl_info_error.Visible = true;
                return false;
            }

            return true;
        }

        protected void btn_create_trn_Click(object sender, EventArgs e)
        {
            if (!check_page_elements())
                return;

            // push_db
            string trnstepid = DB_Trainings.push_Training_Info_Get_Ids(txt_trainingname.Text, ddl_sectors.SelectedValue, txt_effective.Text);

            if (trnstepid == "")
            {
                lbl_info_error.Text = "Training Cant be stored. Try Again Later";
                lbl_info_error.Visible = true;
                return;
            }
            else
            {   //go to next step
                Response.Redirect("~/Trainings/CreateTrainingDesignPage.aspx?N=" + txt_trainingname.Text + "&T=" + trnstepid.Split(',')[0] + "&S=" + trnstepid.Split(',')[1]);
            }
        }

        protected void btn_saveas_Click(object sender, EventArgs e)
        {
            if (!check_page_elements())
                return;

            bool ok = DB_Trainings.saveAs_Training(lbl_trnid.Text, txt_trainingname.Text,  ddl_sectors.SelectedValue, txt_effective.Text);

            if (ok)
                SuccessWithCode(txt_trainingname.Text + " is saved.");

        }
    }
}