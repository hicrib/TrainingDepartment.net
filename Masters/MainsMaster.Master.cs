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
            if (!IsPostBack)
            {
                UserSession user = (UserSession)Session["usersession"];
                lbl_username.Text = user.initial + " - " + user.name_surname;
                lbl_userid.Text = user.employeeid;
                if (user.photo != "")
                    img_userphoto.ImageUrl = AzureCon.general_container_url + user.photo;
            }
        }

        protected void btn_log_out_Click(object sender, ImageClickEventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Pages/Login.aspx");
        }

        protected void btn_mainpage_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pages/UserMain.aspx");
        }

        protected void btn_user_details_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pages/UserDetails.aspx");
        }
    }
}