using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.SysAdmin
{
    public partial class FileTypeDefinition : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("FILE TYPE DEFINITION");
                fill_grid();
            }

        }

        protected void fill_grid ()
        {
            DataTable dt = DB_FileSys.get_filetypes();
            if (dt == null || dt.Rows.Count == 0)
                return;

            grid_types.DataSource = dt;
            grid_types.DataBind();
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if(txt_filetype.Text == "")
            {
                lbl_pageresult.Text = "File Type must have a name";
                lbl_pageresult.Visible = true;
                return;
            }

            if(DB_FileSys.create_filetype(txt_filetype.Text,chk_issue.Checked, chk_expiration.Checked,chk_rolespec.Checked))
            {
                lbl_pageresult.Text = "Success : File Type Created";
                lbl_pageresult.Visible = true;

                fill_grid();
                txt_filetype.Text = "";
                chk_issue.Checked = false;
                chk_expiration.Checked = false;
                chk_rolespec.Checked = false;
                return;
            }
            else
            {
                lbl_pageresult.Text = "File Type must be unique!";
                lbl_pageresult.Visible = true;
                return;
            }

        }
    }
}