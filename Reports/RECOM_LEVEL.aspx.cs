using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Reports
{
    public partial class RECOM_LEVEL : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;

            if (!IsPostBack)
            {
                string reportid = Convert.ToString(Request.QueryString["ReportID"]);
                if (!string.IsNullOrWhiteSpace(reportid))
                {
                    fill_View_Mode_as(reportid);
                }
                else
                {
                    Fill_Default_Page_Elements(); //normal mode, filling form
                }
            }
        }

        protected void fill_View_Mode_as(string reportid)
        {
            UserSession user = (UserSession)Session["usersession"];

            string relation = DB_Reports.get_Relation_to_Report(reportid, user.employeeid);

            if (relation == "trainee")
                relation = "trainee";
            else if (user.isAdmin)
                relation = "sysadmin";
            else if (user.has_ROLENAME("TRN_DEPARTMENT_SIGNER"))
                relation = "TRN_DEPARTMENT_SIGNER";
            else if (user.isOJTI || user.isExaminer || user.isLCE)
                relation = "instructor";
            else
                relation = DB_Reports.get_Relation_to_Report(reportid, user.employeeid); //creater_ojti / trainee , nobody

            if (relation == "nobody")
                RedirectWithCode("UNAUTHORIZED!");

            if (ddl_trainee.SelectedValue == user.employeeid)
                relation = "trainee"; //we will force it for department member's own training (or ojti's)

            lbl_viewmode.Text = relation;


            //todo: first fill all elements
            Dictionary<string, DataTable> li = DB_Reports.pull_RECOM_LEVEL(reportid);

            if (li == null)
                Response.Redirect("~/Pages/UserMain.aspx?Code=4&ID=" + reportid);

            lbl_reportnumber.Text = reportid;

            DataTable meta = li["meta"];
            DataTable form = li["form"];


            ddl_trainee.Items.Add(new ListItem(meta.Rows[0]["TRAINEE_NAME"].ToString(), meta.Rows[0]["TRAINEE_ID"].ToString()));
            ddl_ojtis.Items.Add(new ListItem(meta.Rows[0]["CREATER_NAME"].ToString(), meta.Rows[0]["CREATER"].ToString()));

            ddl_Level.SelectedValue = form.Rows[0]["LEVEL"].ToString();

            ddl_sectors.Items.Add(form.Rows[0]["POSITION"].ToString() + "-" + form.Rows[0]["SECTOR"].ToString());

            txt_totalhours.Text = form.Rows[0]["TOTAL_HOURS"].ToString();

            chk_MER.Checked = form.Rows[0]["MER_MET"].ToString() == "True";
            chk_folder.Checked = form.Rows[0]["FOLDER_COMPLETE"].ToString() == "True";
            chk_objectives.Checked = form.Rows[0]["OBJECTIVES_SIGNED"].ToString() == "True";
            chk_reading.Checked = form.Rows[0]["READING_SIGNED"].ToString() == "True";

            if (meta.Rows[0]["OJTI_SIGNED"].ToString() == "True")
            {
                img_ojtisign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_ojtis.SelectedValue)["SIGNATURE"].ToString();
                img_ojtisign.Visible = true;
                btn_ojtisign.Visible = false;
                lbl_ojtisigned.Text = "1";
            }
            //bring trainee sing if signed
            if (meta.Rows[0]["TRAINEE_SIGNED"].ToString() == "True")
            {
                img_traineesign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_trainee.SelectedValue)["SIGNATURE"].ToString();
                img_traineesign.Visible = true;
                btn_traineesign.Visible = false;
                lbl_traineesigned.Text = "1";
            }
            if (form.Rows[0]["DEPARTMENT_SIGNED"].ToString() == "True")
            {
                img_departmentsign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(form.Rows[0]["DEPARTMENT_EMPLOYEEID"].ToString())["SIGNATURE"].ToString();
                img_departmentsign.Visible = true;
                btn_departmentsign.Visible = false;
                lbl_departmentsigned.Text = "1";
            }


            txt_date.Text = form.Rows[0]["DATE"].ToString();
            txt_comments.Text = form.Rows[0]["COMMENTS"].ToString();

            //disable everything
            DisableControls(pnl_wrapper);
            btn_submit.Visible = false;

         

           


            // if not signed by trainee enable sign button
            if (relation == "trainee" && meta.Rows[0]["TRAINEE_SIGNED"].ToString() != "True")
            {
                //let them sign , let them comment
                btn_traineesign.Enabled = true;

                //let them submit
                btn_submit.Visible = true;
                btn_submit.Enabled = true;
            }
            if (relation == "TRN_DEPARTMENT_SIGNER" && form.Rows[0]["DEPARTMENT_SIGNED"].ToString() != "True")
            {
                btn_departmentsign.Visible = true;
                btn_departmentsign.Enabled = true;

                btn_submit.Visible = true;
                btn_submit.Enabled = true;
            }

        }


        protected void Fill_Default_Page_Elements()
        {
            DataTable trainees = DB_System.get_ALL_trainees();
            if (trainees != null)
            {
                ddl_trainee.DataSource = trainees;
                ddl_trainee.DataTextField = "NAME";
                ddl_trainee.DataValueField = "ID";
                ddl_trainee.DataBind();
            }

            DataTable ojtis = DB_System.get_ALL_OJTI_LCE_EXAMINER();
            if (ojtis != null)
            {
                ddl_ojtis.DataSource = ojtis;
                ddl_ojtis.DataTextField = "NAME";
                ddl_ojtis.DataValueField = "ID";
                ddl_ojtis.DataBind();
            }
            ddl_ojtis.SelectedValue = ((UserSession)Session["usersession"]).employeeid;
            //todo: some extra powerful positions may create for other people
            ddl_ojtis.Enabled = false;

            ddl_sectors.DataSource = DB_System.get_Sectors(); //pos-sec format
            ddl_sectors.DataValueField = "CODE";
            ddl_sectors.DataBind();

            txt_date.Text = DateTime.UtcNow.ToString("yyyy-MM-dd");
            //string today = DateTime.UtcNow.ToString("yyyy-MM-dd");
            //ddl_DAY.SelectedValue = today.Split('.')[0];
            //ddl_MONTH.SelectedValue = Convert.ToInt32(today.Split('.')[1]).ToString();
            //ddl_YEAR.SelectedValue = today.Split('.')[2];


            Dictionary<string, string> direct_dict = (Dictionary<string, string>)Session["direct_dictionary"];

            if (direct_dict != null && direct_dict.Count != 0)
                fill_from_CreateReport(direct_dict);
        }

        protected void fill_from_CreateReport(Dictionary<string, string> directed)
        {
            lbl_genid.Text = directed["genid"];
            lbl_stepid.Text = directed["stepid"];

            ddl_trainee.SelectedValue = directed["traineeid"];
            ddl_trainee.Enabled = false;

            ddl_ojtis.SelectedValue = directed["ojtiid"];
            ddl_ojtis.Enabled = false;

            ddl_sectors.SelectedValue = directed["position"] + "-" + directed["sector"];
            ddl_sectors.Enabled = false;

            UserSession user = (UserSession)Session["usersession"];
            if (user.employeeid != ddl_trainee.SelectedValue)
                btn_traineesign.Enabled = false;

            btn_departmentsign.Enabled = user.has_ROLENAME("TRN_DEPARTMENT_SIGNER");


            string name = directed["name"];
            if (name.Contains("LEVEL3"))
                ddl_Level.SelectedValue = "3";
            if (name.Contains("LEVEL2"))
                ddl_Level.SelectedValue = "2";
            if (name.Contains("LEVEL1"))
                ddl_Level.SelectedValue = "1";
            ddl_Level.Enabled = false;


            string user_mer = DB_Reports.get_MER_step(ddl_trainee.SelectedValue, directed["stepid"]);
            string user_totalhours = DB_Reports.get_TOTALHOURS(directed["traineeid"], directed["stepid"]);

            if(user_mer == "00:00")
            {
                lbl_MER.Text = "( MER : " + "UNDEFINED FOR USER" + ")";
                chk_MER.Checked = false;
                chk_MER.Enabled = false;
            }
            else
                lbl_MER.Text = "( MER : " + user_mer + ")";


            if (user_totalhours == "00:00" || Utility.isgreater_TimeFormat(user_mer, user_totalhours) > 0 )
            {
                chk_MER.Checked = false;
                chk_MER.Enabled = false;   
            }
               
            txt_totalhours.Text = user_totalhours;



            if(!DB_Reports.is_LevelObjectives_completed(ddl_trainee.SelectedValue, directed["sector"],directed["phase"]))
            {
                chk_objectives.Checked = false;
                chk_objectives.Enabled = false;
            }
            //ddl_positions.SelectedValue = directed["position"] + "-" + directed["sector"];
            //ddl_positions.Enabled = false;

            //todo: ojt-PREOJT-assess etc can be filled here as well
        }




        protected void btn_submit_Click(object sender, EventArgs e)
        {
            //update student comments and sign
            if (lbl_viewmode.Text == "trainee")
            {
                if (lbl_traineesigned.Text != "1")
                {
                    ClientMessage(lbl_pageresult, "Trainee must sign to submit the report", System.Drawing.Color.Red);
                    return;
                }

                if (DB_Reports.update_Sign_RECOM_LEVEL(lbl_reportnumber.Text, "trainee", ""))
                    SuccessWithCode("SUCCESS! Report Signed.");
                else
                    RedirectWithCode("Error: Try Again Later.");
            }

            if (lbl_viewmode.Text == "TRN_DEPARTMENT_SIGNER")
            {
                if (lbl_departmentsigned.Text != "1")
                {
                    ClientMessage(lbl_pageresult, "Training Department must sign to submit the report", System.Drawing.Color.Red);
                    return;
                }

                if (DB_Reports.update_Sign_RECOM_LEVEL(lbl_reportnumber.Text, "TRN_DEPARTMENT_SIGNER", ((UserSession)Session["usersession"]).employeeid))
                    SuccessWithCode("SUCCESS! Report Signed.");
                else
                    RedirectWithCode("Error: Try Again Later.");
            }

            // in normal first submit mode : make all checks regarding page elements
            bool all_good = Check_Page_Elements();
            if (!all_good)
                return;


            // we insert everything in db 
            string reportid = push_into_db();
            if (String.IsNullOrWhiteSpace(reportid))
            {
                //todo : error message
            }
            else
            {
                SuccessWithCode("SUCCESS : Report Created !");
            }
        }

        protected bool Check_Page_Elements()
        {
            if (ddl_Level.SelectedValue == "-" || ddl_trainee.SelectedValue == "0")
            {
                ClientMessage(lbl_pageresult, "Choose the Level before signing!", System.Drawing.Color.Red);
                return false;
            }


            if (ddl_trainee.SelectedValue == "-" || ddl_trainee.SelectedValue == "0")
            {
                ClientMessage(lbl_pageresult, "Choose the Trainee before signing!", System.Drawing.Color.Red);
                return false;
            }

            if (ddl_sectors.SelectedValue == "-")
            {
                ClientMessage(lbl_pageresult, "Choose Position!", System.Drawing.Color.Red);
                return false;
            }

            if (txt_totalhours.Text == "" || txt_totalhours.Text == "00:00")
            {
                ClientMessage(lbl_pageresult, "Choose Total Hours", System.Drawing.Color.Red);
                return false;
            }
            if (lbl_ojtisigned.Text != "1")
            {
                ClientMessage(lbl_pageresult, "OJTI must sign to submit the report", System.Drawing.Color.Red);
                return false;
            }

            if (!(chk_folder.Checked && chk_MER.Checked && chk_objectives.Checked && chk_reading.Checked))
            {
                ClientMessage(lbl_pageresult, "All requirements must be met and confirmed", System.Drawing.Color.Red);
                return false;
            }

            if (txt_date.Text == "")
            {
                ClientMessage(lbl_pageresult, "Date must be entered", System.Drawing.Color.Red);
                return false;
            }



            return true;
        }

        protected string push_into_db()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("TRAINEE_ID", ddl_trainee.SelectedValue);
            data.Add("POSITION", ddl_sectors.SelectedValue.Split('-')[0]);
            data.Add("SECTOR", ddl_sectors.SelectedValue.Split('-')[1]);
            data.Add("PHASE", "OJT");
            data.Add("LEVEL", ddl_Level.SelectedValue);
            data.Add("OJTI_ID", ddl_ojtis.SelectedValue);
            data.Add("OJTI_SIGNED", lbl_ojtisigned.Text);
            data.Add("TOTAL_HOURS", txt_totalhours.Text);
            data.Add("MER_MET", "1");// no submit unless this is checked
            data.Add("READING_SIGNED", "1");
            data.Add("OBJECTIVES_SIGNED", "1");
            data.Add("FOLDER_COMPLETE", "1");
            data.Add("TRAINEE_SIGNED", lbl_traineesigned.Text);
            data.Add("DATE", txt_date.Text);
            data.Add("DEPARTMENT_SIGNED", lbl_departmentsigned.Text);
            data.Add("DEPARTMENT_EMPLOYEEID", lbl_departmentsigned.Text == "1" ? ((UserSession)Session["usersession"]).employeeid : "");
            data.Add("COMMENTS", txt_comments.Text);

            data.Add("genid", lbl_genid.Text);
            data.Add("stepid", lbl_stepid.Text);

            string reportid = DB_Reports.push_RECOM_LEVEL(data);
            return reportid;
        }

        private void DisableControls(System.Web.UI.Control control)
        {
            foreach (System.Web.UI.Control c in control.Controls)
            {
                // Get the Enabled property by reflection.
                Type type = c.GetType();
                PropertyInfo prop = type.GetProperty("Enabled");

                // Set it to False to disable the control.
                if (prop != null)
                {
                    prop.SetValue(c, false, null);
                }

                // Recurse into child controls.
                if (c.Controls.Count > 0)
                {
                    this.DisableControls(c);
                }
            }

        }


        protected void btn_ojtisign_Click(object sender, EventArgs e)
        {
            img_ojtisign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_ojtis.SelectedValue)["SIGNATURE"].ToString();
            img_ojtisign.Visible = true;
            btn_ojtisign.Visible = false;
            lbl_ojtisigned.Text = "1";
        }

        protected void btn_traineesign_Click(object sender, EventArgs e)
        {
            img_traineesign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_trainee.SelectedValue)["SIGNATURE"].ToString();
            img_traineesign.Visible = true;
            btn_traineesign.Visible = false;
            lbl_traineesigned.Text = "1";
        }

        protected void btn_departmentsign_Click(object sender, EventArgs e)
        {
            UserSession user = (UserSession)Session["usersession"];
            img_departmentsign.ImageUrl = AzureCon.general_container_url + user.signature;
            img_departmentsign.Visible = true;
            btn_departmentsign.Visible = false;
            lbl_departmentsigned.Text = "1";
        }
    }
}