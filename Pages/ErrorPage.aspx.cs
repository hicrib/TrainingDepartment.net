using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Exams
{
    public partial class ErrorPage : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            
            string code = Convert.ToString(Request.QueryString["Code"]);

            lbl_error.Text = code;
            

        }
    }
}