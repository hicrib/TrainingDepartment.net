﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using AviaTrain.App_Code;

namespace AviaTrain.Masters
{
    public partial class ExamsMaster : System.Web.UI.MasterPage
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

                if(user.notif > 0)
                {
                    lbl_notifnumber.Visible = true;
                    lbl_notifnumber.Text = user.notif.ToString();
                }
            }
        }



        protected void btn_log_out_Click(object sender, ImageClickEventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
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

        protected void btn_notifications_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pages/UserNotifications.aspx");
        }

        protected void btn_checkin_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Pages/PositionLog.aspx");
        }
    }
}