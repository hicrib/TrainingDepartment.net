using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.IO;
using Microsoft.Ajax.Utilities;

namespace AviaTrain
{
    public partial class MasterPage : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {

            if (!IsPostBack)
            {
                Page.MaintainScrollPositionOnPostBack = true;

                UserSession user = (UserSession)Session["usersession"];
                if (user == null)
                    Response.Redirect("~/Pages/login.aspx");


                RoleControlForPage();

                DB_System.log_pages(user.employeeid, HttpContext.Current.Request.Url.AbsoluteUri);
            }



            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public virtual void ClientMessage(Label sender, string message, System.Drawing.Color color)
        {
            sender.Text = message;
            sender.Visible = true;
        }

        protected void RedirectWithCode(string message = "System Error : Try Again Later")
        {
            Response.Redirect("~/Pages/ErrorPage.aspx?Code=" + message);//todo : error message for authorization
        }

        protected void SuccessWithCode(string message = "SUCCESS !")
        {
            Response.Redirect("~/Pages/SuccessPage.aspx?Code=" + message);
        }


        protected void RoleControlForPage()
        {
            UserSession user = (UserSession)Session["usersession"];

            //has every right 
            if (user.isAdmin)
                return;


            string page = (Page.AppRelativeVirtualPath.Split('.')[0]).Split('/')[2];

            DataRow[] alloweds = user.roles_pages.Select("PAGE_NAME = '" + page + "'");
            if (alloweds != null && alloweds.Length > 0)
                return; //allowed for this page
            else
                RedirectWithCode("UNAUTHORIZED !");

        }

    }
}