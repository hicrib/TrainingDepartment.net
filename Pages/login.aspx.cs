using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AviaTrain.App_Code;


namespace AviaTrain.Pages
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void login_btn_Click(object sender, EventArgs e)
        {
            string employeeid = DB_System.isLoginValid(username_txt.Text, password_txt.Text);
            result_lbl.Text = employeeid;
            if (String.IsNullOrWhiteSpace(employeeid))
            {
                //problem with login
                result_lbl.Text = "Failed Login!";
            }
            else
            {
                Session["employeeid"] = employeeid;
                UserSession user = new UserSession(employeeid);
                Session["usersession"] = user;

                //todo : burada doldurulması faydalı başka şeyler de olabilir.
                //todo : kişiye yapılacak bildirimler buradan öğrenilip yapılabilir.

                if(user.isExamAdmin)
                    Response.Redirect("~/Exams/Exam_MainAdmin.aspx");
                else if (user.isExamTrainee)
                    Response.Redirect("~/Exams/Exam_MainGeneral.aspx");
                else
                    Response.Redirect("~/Pages/UserMain.aspx");
            }


        }
    }
}