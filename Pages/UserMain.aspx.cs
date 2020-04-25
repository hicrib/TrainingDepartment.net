using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AviaTrain.App_Code;
using System.Data;


namespace AviaTrain.Pages
{
    public partial class UserMain : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {

            Session["direct_dictionary"] = null; //clean-up
            Session["chosen_questions"] = null; //clean-up
            Session["exam_result_details"] = null;

            //todo : if not instructor, if not system admin -> Trainee
            // or came here as Trainee
            UserSession user = (UserSession)Session["usersession"];

            if (user.isOnlyTrainee)
                Response.Redirect("~/Pages/TraineeMain.aspx");

            if (user.isAdmin)
                Response.Redirect("~/SysAdmin/SysAdminMain.aspx");

            if (user.isOJTI || user.isLCE )
                Response.Redirect("~/Pages/InstructorMain.aspx");
        }
    }
}