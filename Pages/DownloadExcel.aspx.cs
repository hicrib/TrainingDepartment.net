using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;

namespace AviaTrain.Pages
{
    public partial class DownloadExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt =  (DataTable)Session["excel_file"];
            Dictionary<string,string> excludes = (Dictionary<string,string>)Session["excel_file_excludeColumns"];
            if(dt!=null && excludes != null)
            {

                string attachment = "attachment; filename=Export_"+DateTime.UtcNow.Date.ToString("yyyy_MM_dd")+".xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (excludes.Keys.Contains(dc.ColumnName))
                        continue;
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        if (excludes.Values.Contains(i.ToString()))
                            continue;

                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }

                // cleanup
                Session["excel_file"] = null;
                Session["excel_file_excludeColumns"] = null;
                
                Response.End();

            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }
    }
}