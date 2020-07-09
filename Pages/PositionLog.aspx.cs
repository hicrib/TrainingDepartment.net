using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Pages
{
    public partial class PositionLog : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }

        }

        protected void menu_logs_MenuItemClick(object sender, MenuEventArgs e)
        {
            int index = Int32.Parse(e.Item.Value);
            multiview1.ActiveViewIndex = index;
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            //check elements
            if(ddl_position.SelectedValue == "-" || ddl_sector.SelectedValue == "-")
            {
                lbl_pageresult.Text = "Choose Sector";
                lbl_pageresult.Visible = true;
                return;
            }
            if(txt_timeon.Text == "")
            {
                lbl_pageresult.Text = "Choose Time On";
                lbl_pageresult.Visible = true;
                return;
            }
            if(rad_current.SelectedValue != "1" || rad_fit.SelectedValue != "1" || rad_reading.SelectedValue != "1")
            {
                lbl_pageresult.Text = "You must confirm the requirements";
                lbl_pageresult.Visible = true;
                return;
            }
            if(rad_solo.SelectedValue == "0" && (ddl_trainee.SelectedValue == "0" || ddl_trainee.SelectedValue == "-" ))
            {
                lbl_pageresult.Text = "You must enter Trainee if you aren't solo";
                lbl_pageresult.Visible = true;
                return;
            }

            lbl_pageresult.Text = "";
            lbl_pageresult.Visible = false;



            UserSession user = (UserSession)Session["usersession"];
            bool ok = DB_Stats.login_positionlog(ddl_position.SelectedValue, ddl_sector.SelectedValue,
                user.employeeid, DateTime.UtcNow.Date.ToString("yyyy-MM-dd"), txt_timeon.Text,
                rad_solo.SelectedValue == "0" ? ddl_trainee.SelectedValue : "" );

            if (ok)
            {
                // fill grid 
                DataTable dt = DB_Stats.get_positionlog_page(ddl_position.SelectedValue, ddl_sector.SelectedValue);
                if (dt == null || dt.Rows.Count == 0)
                    return;

                grid_log.DataSource = dt;
                grid_log.DataBind();
                grid_log.Visible = true;

                //clean  to prevent multiple submission
                txt_timeon.Text = "";
                rad_current.SelectedValue = "0";
                rad_fit.SelectedValue = "0";
                rad_reading.SelectedValue = "0";
                rad_solo.SelectedValue = "1";
            }
            else
            {
                lbl_pageresult.Text = "System Error";
                lbl_pageresult.Visible = true;
            }


        }

        protected void rad_solo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //fill ojtis
            if(rad_solo.SelectedValue == "0")
            {
                ddl_trainee.DataSource = DB_System.get_ALL_Users(with_empty:true , isactive:true);
                ddl_trainee.DataTextField = "NAME";
                ddl_trainee.DataValueField = "ID";
                ddl_trainee.DataBind();
            }

            ddl_trainee.Visible = rad_solo.SelectedValue == "0";
            lbl_trainee.Visible = rad_solo.SelectedValue == "0";
        }

        protected void ddl_position_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_position.SelectedValue == "-")
            {
                ddl_sector.DataSource = new DataTable();
                ddl_sector.DataTextField = "NAME";
                ddl_sector.DataValueField = "CODE";
                ddl_sector.DataBind();
                return;
            }

            ddl_sector.Items.Clear();
            foreach(DataRow r in DB_System.get_Sectors_withpos(ddl_position.SelectedValue).Rows)
            {
                if (r["CODE"].ToString() == "TWR-ASSIST")
                    continue;

                ddl_sector.Items.Add(r["CODE"].ToString().Replace("ACC-", "").Replace("APP-", "").Replace("TWR-", ""));
            }
        }

        protected void ddl_sector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_sector.SelectedValue == "-")
            {
                grid_log.Visible = false;
                return;
            }

            // fill grid 
            DataTable dt = DB_Stats.get_positionlog_page(ddl_position.SelectedValue, ddl_sector.SelectedValue);
            if (dt == null || dt.Rows.Count == 0)
            {
                grid_log.Visible = false;
                return;
            }

            grid_log.DataSource = dt;
            grid_log.DataBind();
            grid_log.Visible = true;

        }

        protected void grid_log_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
                e.Row.Cells[0].Visible = false;
        }
    }
}