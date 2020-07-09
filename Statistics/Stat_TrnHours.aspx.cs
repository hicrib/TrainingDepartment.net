using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Statistics
{
    public partial class Stat_TrnHours : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("TOTAL TRAINING HOURS");

                filter_trainee.DataSource = DB_System.get_ALL_Users(true,false);
                filter_trainee.DataTextField = "NAME";
                filter_trainee.DataValueField = "ID";
                filter_trainee.DataBind();

            }
        }

        //protected void grid_trnhours_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        //protected void grid_trnhours_Sorting(object sender, GridViewSortEventArgs e)
        //{

        //}

        //public SortDirection direction
        //{
        //    get
        //    {
        //        if (ViewState["directionState"] == null)
        //        {
        //            ViewState["directionState"] = SortDirection.Ascending;
        //        }
        //        return (SortDirection)ViewState["directionState"];
        //    }
        //    set
        //    {
        //        ViewState["directionState"] = value;
        //    }
        //}


        protected void filter_unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter_sector.Items.Clear();

            if (filter_unit.SelectedValue == "TWR")
            {
                filter_sector.Items.Add("-");
                filter_sector.Items.Add("ADC");
                filter_sector.Items.Add("GMC");
            }
            if (filter_unit.SelectedValue == "APP")
            {
                filter_sector.Items.Add("-");
                filter_sector.Items.Add("AR");
                filter_sector.Items.Add("BR");
                filter_sector.Items.Add("KR");
            }
            if (filter_unit.SelectedValue == "ACC")
            {
                filter_sector.Items.Add("-");
                filter_sector.Items.Add("NR");
                filter_sector.Items.Add("CR");
                filter_sector.Items.Add("SR");
            }
        }

        protected void btn_export_Click(object sender, EventArgs e)
        {

        }

        protected void btn_search_Click(object sender, EventArgs e)
        {
            if(filter_start.Text == "" || filter_finish.Text == "")
            {
                lbl_result.Text = "Choose Dates";
                lbl_result.Visible = true;
                return;
            } 
            else if(filter_trainee.SelectedValue == "0" && filter_unit.SelectedValue == "-" && filter_sector.SelectedValue == "-")
            {
                // if nothing is selected
            }

           
            string unit = filter_unit.SelectedValue == "-" ? "" : filter_unit.SelectedValue; 
            string sector = filter_sector.SelectedValue == "-" ? "" : filter_sector.SelectedValue;

            DataTable dt =  DB_Stats.get_TrainingHours(filter_start.Text, filter_finish.Text, filter_trainee.SelectedValue, unit, sector);
            if (dt == null || dt.Rows.Count == 0)
            {
                lbl_result.Text = "No Results";
                lbl_result.Visible = true;
                return;
            }

            lbl_result.Text = "";
            lbl_result.Visible = false;

            //calculate times // INITIAL, HOURS, NOTRAINING, NOSHOW, POSITION
            DataTable result_table = new DataTable();
            result_table.Columns.Add("Initial");
            result_table.Columns.Add("Unit");
            result_table.Columns.Add("Sector");
            result_table.Columns.Add("Scheduled");
            result_table.Columns.Add("Actual");
            result_table.Columns.Add("No Show");
            result_table.Columns.Add("No Trn. Value");

            List<string> completeds = new List<string>();
            
            foreach (DataRow row in dt.Rows)
            {
                if(completeds.Contains (row["INITIAL"].ToString() +"-"+row["POSITION"].ToString()))
                    continue;

                completeds.Add (row["INITIAL"].ToString() + "-" + row["POSITION"].ToString());

                string sched_total = "00:00";
                string noshow_total = "00:00";
                string notrn_total = "00:00";
                string actual = "00:00";

                DataRow[] results = dt.Select("INITIAL = '"+ row["INITIAL"].ToString() + "' AND POSITION = '"+ row["POSITION"].ToString() + "' ");
                foreach (DataRow sec_row in results)
                {
                    if (sec_row["NOTRAINING"].ToString() == "False" && sec_row["NOSHOW"].ToString() == "False")
                    {
                        actual = Utility.add_TimeFormat(actual, sec_row["HOURS"].ToString());
                        sched_total = Utility.add_TimeFormat(sched_total, sec_row["HOURS"].ToString());
                    }
                    else
                    {

                        string off = sec_row["TIMEOFF_SCH"].ToString();
                        if (Utility.isgreater_TimeFormat(sec_row["TIMEON_SCH"].ToString(), sec_row["TIMEOFF_SCH"].ToString()) == 1)
                            off = Utility.add_TimeFormat("24:00", sec_row["TIMEOFF_SCH"].ToString());

                        string sched = Utility.subtract_TimeFormat(sec_row["TIMEON_SCH"].ToString(), off);

                        sched_total = Utility.add_TimeFormat(sched_total, sched);

                        if (sec_row["NOTRAINING"].ToString() == "True")
                            notrn_total = Utility.add_TimeFormat(notrn_total, sched);

                        if (sec_row["NOSHOW"].ToString() == "True")
                            noshow_total = Utility.add_TimeFormat(noshow_total, sched);
                    }
                }

                string u = "";
                if (row["POSITION"].ToString().Contains("TWR"))
                    u = "TWR";
                else if (new string[3] { "AR", "BR", "KR" }.Contains(row["POSITION"].ToString()))
                    u = "APP";
                else if (new string[3] { "CR", "NR", "SR" }.Contains(row["POSITION"].ToString()))
                    u = "ACC";


                DataRow r = result_table.NewRow();
                r["Initial"] = row["INITIAL"].ToString();
                r["Unit"] = u;
                r["Sector"] = row["POSITION"].ToString();
                r["Scheduled"] = sched_total;
                r["Actual"] = actual;
                r["No Show"] = noshow_total;
                r["No Trn. Value"] = notrn_total;
                result_table.Rows.Add(r);
            }

            grid_trnhours.DataSource = result_table;
            grid_trnhours.DataBind();
            lbl_gridname.Text = filter_start.Text + " - " + filter_finish.Text;

            Session["excel_file"] = result_table;
            Session["excel_file_excludeColumns"] = new Dictionary<string, string> {   };

        }
    }
}