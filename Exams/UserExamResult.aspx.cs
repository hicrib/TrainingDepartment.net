﻿using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Exams
{
    public partial class UserExamResult : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Dictionary<string, string> exam_result = (Dictionary<string, string>)Session["exam_result"];
                if (exam_result == null || exam_result.Count() == 0)
                    RedirectWithCode("Not Authorized to View This Page");

                lbl_exam_name.Text = exam_result["exam_name"];
                UserSession user = (UserSession)Session["usersession"];
                lbl_trainee_name.Text = user.name_surname;

                Write_Page_Header_Low(lbl_exam_name.Text + " Result");

                lbl_result.Text = exam_result["grade"] + "%";

                string dummy = (lbl_exam_name.Text.Split('%')[0]);
                string passpercent = dummy.Substring(dummy.Length - 2, 2);

                if (Convert.ToDecimal(passpercent) > Convert.ToDecimal(exam_result["grade"]))
                    img_result.ImageUrl = "~/images/fail.png";
                else
                    img_result.ImageUrl = "~/images/pass.png";

            }
        }
    }
}