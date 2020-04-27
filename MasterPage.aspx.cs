using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.IO;

namespace AviaTrain
{
    public partial class MasterPage : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;

            UserSession user = (UserSession)Session["usersession"];
            if (user == null)
            {
                Response.Redirect("~/Pages/login.aspx");
            }

            DB_System.log_pages(user.employeeid, HttpContext.Current.Request.Url.AbsoluteUri);

            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;
        }

        public virtual void ClientMessage(Label sender , string message, System.Drawing.Color color)
        {
            sender.Text = message;
            sender.Visible = true;
        }

        protected void RedirectWithCode(string message = "System Error : Try Again Later")
        {
            Response.Redirect("~/Exams/Exam_ErrorPage.aspx?Code=" + message);//todo : error message for authorization
        }

        protected void SuccessWithCode(string message = "SUCCESS !")
        {
            Response.Redirect("~/Pages/SuccessPage.aspx?Code=" + message);
        }

    }
}