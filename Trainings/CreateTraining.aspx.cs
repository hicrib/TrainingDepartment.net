using System;
using System.Collections.Generic;
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

            }

        }

        protected void btn_create_trn_Click(object sender, EventArgs e)
        {
            if (txt_examname.Text.Trim() == "")
            {
                lbl_info_error.Text = "Training Name is empty";
                lbl_info_error.Visible = true;
                return;
            }
            if (txt_effective.Text.Trim() == "")
            {//todo : this doesnt work properly
                lbl_info_error.Text = "Effective Date is empty";
                lbl_info_error.Visible = true;
                return;
            }


            // push_db
            string trnstepid = DB_Trainings.push_Training_Info_Get_Ids(txt_examname.Text, ddl_sectors.SelectedValue, txt_effective.Text);

            if (trnstepid == "")
            {
                lbl_info_error.Text = "Training Cant be stored. Try Again Later";
                lbl_info_error.Visible = true;
                return;
            }
            else
            {   //go to next step
                Response.Redirect("~/Trainings/CreateTrainingDesignPage.aspx?T=" + trnstepid.Split(',')[0] + "&S=" + trnstepid.Split(',')[1]);
            }
        }

    }
}