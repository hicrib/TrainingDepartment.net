using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Masters
{
    public partial class MainMaster : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Fill_UserInfo();
        }


        protected void Fill_UserInfo()
        {
            UserSession user = (UserSession)Session["usersession"];

            //todo: details can be added
            lbl_userinfo.Text = user.name_surname;
        }

        protected void btn_logout_Click1(object sender, ImageClickEventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Pages/Login.aspx");
        }

        protected void btn_main_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pages/UserMain.aspx");
        }
    }
}