using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Reports
{
    public partial class ShowReport : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            string reportID = Convert.ToString(Request.QueryString["ID"]);
            if (String.IsNullOrWhiteSpace(reportID))
            {
                Response.Redirect("~/Pages/UserMain.aspx?Code=2&ID=" + reportID);

                //todo : here, instead of sending to UserMain, we can show a MESSAGE here and make a BUTTON to go to UserMain
            }
            else
            {
                string report_type = DB_Reports.get_Report_Type(reportID);
                if (string.IsNullOrWhiteSpace(report_type))
                    Response.Redirect("~/Pages/UserMain.aspx?Code=2&ID=" + reportID);

                Response.Redirect("~/Reports/" + report_type + ".aspx?ReportID=" + reportID);
            }
        }
    }
}