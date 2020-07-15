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
                Write_Page_Header_Low("POSITION LOG");
            }

        }

        protected void menu_logs_MenuItemClick(object sender, MenuEventArgs e)
        {
            int index = Int32.Parse(e.Item.Value);
            multiview1.ActiveViewIndex = index;

            ddl_position.ClearSelection();
            ddl_sector.ClearSelection();
            grid_log.Visible = false;
            ddl_CoBsector.ClearSelection();
            ddl_CoBposition.ClearSelection();
            grid_COB.Visible = false;
        }

        #region LOGIN
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

            fill_grid_log();
        }
        protected void fill_grid_log()
        {
            // fill grid 
            DataTable dt = DB_Stats.get_positionlog_page(ddl_position.SelectedValue, ddl_sector.SelectedValue);
            if (dt == null || dt.Rows.Count == 0)
            {
                grid_log.Visible = false;
                return;
            }
            else
            {
                grid_log.DataSource = dt;
                grid_log.DataBind();
                grid_log.Visible = true;
            }
        }
        protected void rad_solo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //fill ojtis
            if (rad_solo.SelectedValue == "0")
            {
                ddl_trainee.DataSource = DB_System.get_ALL_Users(with_empty: true, isactive: true);
                ddl_trainee.DataTextField = "NAME";
                ddl_trainee.DataValueField = "ID";
                ddl_trainee.DataBind();
            }

            ddl_trainee.Visible = rad_solo.SelectedValue == "0";
            lbl_trainee.Visible = rad_solo.SelectedValue == "0";
        }
        protected void btn_login_Click(object sender, EventArgs e)
        {
            //check elements
            if (ddl_position.SelectedValue == "-" || ddl_sector.SelectedValue == "-")
            {
                lbl_pageresult.Text = "Choose Sector";
                lbl_pageresult.Visible = true;
                return;
            }
            if (txt_timeon.Text == "" || txt_dateon.Text == "")
            {
                lbl_pageresult.Text = "Choose Date-Time to Login";
                lbl_pageresult.Visible = true;
                return;
            }
            if (rad_current.SelectedValue != "1" || rad_fit.SelectedValue != "1" || rad_reading.SelectedValue != "1")
            {
                lbl_pageresult.Text = "You must confirm the requirements";
                lbl_pageresult.Visible = true;
                return;
            }
            if (rad_solo.SelectedValue == "0" && (ddl_trainee.SelectedValue == "0" || ddl_trainee.SelectedValue == "-"))
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
                rad_solo.SelectedValue == "0" ? ddl_trainee.SelectedValue : "");

            if (ok)
            {
                fill_grid_log();

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
        protected void grid_log_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
                e.Row.Cells[0].Visible = false;
        }
        #endregion



        protected void ddl_CoBsector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_CoBsector.SelectedValue == "-")
            {
                grid_COB.Visible = false;
                return;
            }

            fill_grid_cob();
        }

        protected void fill_grid_cob()
        {
            // fill grid 
            DataTable dt = DB_Stats.get_positionlog_page(ddl_CoBposition.SelectedValue, ddl_CoBsector.SelectedValue);
            if (dt == null || dt.Rows.Count == 0)
            {
                grid_COB.Visible = false;
                lbl_cobid.Text = "";
                return;
            }
            else
            {
                grid_COB.DataSource = dt;
                grid_COB.DataBind();
                grid_COB.Visible = true;

                //find last entry id
                int maxid = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    int id = dr.Field<int>("ID");
                    maxid = Math.Max(maxid, id);
                }
                lbl_cobid.Text = maxid.ToString();
            }
        }

        protected void grid_COB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
                e.Row.Cells[0].Visible = false;
        }

        protected void ddl_CoBposition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_CoBposition.SelectedValue == "-")
            {
                ddl_CoBsector.DataSource = new DataTable();
                ddl_CoBsector.DataTextField = "NAME";
                ddl_CoBsector.DataValueField = "CODE";
                ddl_CoBsector.DataBind();
                return;
            }

            ddl_CoBsector.Items.Clear();
            foreach (DataRow r in DB_System.get_Sectors_withpos(ddl_CoBposition.SelectedValue).Rows)
            {
                if (r["CODE"].ToString() == "TWR-ASSIST")
                    continue;

                ddl_CoBsector.Items.Add(r["CODE"].ToString().Replace("ACC-", "").Replace("APP-", "").Replace("TWR-", ""));
            }
        }

        protected void btn_CoB_Click(object sender, EventArgs e)
        {
            if(ddl_CoBposition.SelectedValue == "-" || ddl_CoBsector.SelectedValue == "-")
            {
                lbl_CoBresult.Text = "Choose position and sector";
                lbl_CoBresult.Visible = true;
                return;
            }
            if(txt_CoBtime.Text == "" || txt_CoBdate.Text == "")
            {
                lbl_CoBresult.Text = "Choose CoB Time";
                lbl_CoBresult.Visible = true;
                return;
            }
            if(lbl_cobid.Text == "")
            {
                lbl_CoBresult.Text = "No Entry in the Position Log";
                lbl_CoBresult.Visible = true;
                return;
            }
            lbl_CoBresult.Visible = false;

            UserSession user = (UserSession)Session["usersession"];
            bool ok = DB_Stats.CoB_positionlog(lbl_cobid.Text, user.employeeid, txt_CoBdate.Text, txt_CoBtime.Text);
            if (ok)
            {
                fill_grid_cob();
                txt_CoBdate.Text = "";
                txt_CoBtime.Text = "";
            }
            else
            {
                lbl_CoBresult.Text = "System Error";
                lbl_CoBresult.Visible = true;
            }
        }
    }
}