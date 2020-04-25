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
                    UserSession user = (UserSession)Session["usersession"];

                    string relation = DB_Reports.get_Relation_to_Report(reportid, user.employeeid);

                    //todo: Check if trainee will sign
                    switch (relation)
                    {
                        case "trainee":
                            fill_View_Mode_as(reportid, "trainee");
                            break;
                        case "sysadmin":
                            fill_View_Mode_as(reportid, "sysadmin");
                            break;
                        case "creater_ojti":
                            fill_View_Mode_as(reportid, "creater_ojti");
                            break;
                        default:
                            Response.Redirect("~/Pages/UserMain.aspx?Code=3&ID=" + reportid);
                            break;
                    }


                }
                else
                {
                    Fill_Default_Page_Elements(); //normal mode, filling form
                }
            }
        }

        protected void fill_View_Mode_as(string reportid, string mode)
        {
            //todo: first fill all elements
            Dictionary<string, DataTable> li = DB_Reports.pull_RECOM_LEVEL(reportid);

            if (li == null)
                Response.Redirect("~/Pages/UserMain.aspx?Code=4&ID=" + reportid);

            lbl_reportnumber.Text = reportid;

            DataTable meta = li["meta"];
            DataTable form = li["form"];
            lbl_viewmode.Text = "viewonly";


            ddl_trainee.Items.Add(new ListItem(meta.Rows[0]["TRAINEE_NAME"].ToString(), meta.Rows[0]["TRAINEE_ID"].ToString()));
            ddl_ojtis.Items.Add(new ListItem(meta.Rows[0]["CREATER_NAME"].ToString(), meta.Rows[0]["CREATER"].ToString()));

            

            //bring trainee sing if signed
            if (meta.Rows[0]["TRAINEE_SIGNED"].ToString() == "True")
                btn_traineesign_Click(new object(), new EventArgs());

            ddl_sectors.Items.Add(form.Rows[0]["POSITION"].ToString() + "-" + form.Rows[0]["SECTOR"].ToString());

            txt_totalhours.Text = form.Rows[0]["TOTAL_HOURS"].ToString();

            chk_MER.Checked = form.Rows[0]["MER_MET"].ToString() == "True";
            chk_folder.Checked = form.Rows[0]["FOLDER_COMPLETE"].ToString() == "True";
            chk_objectives.Checked = form.Rows[0]["OBJECTIVES_SIGNED"].ToString() == "True";
            chk_reading.Checked = form.Rows[0]["READING_SIGNED"].ToString() == "True";

            if (meta.Rows[0]["OJTI_SIGNED"].ToString() == "True")
                btn_ojtisign_Click(new object(), new EventArgs());

            string date = form.Rows[0]["DATE"].ToString();
            ddl_DAY.Items.Add(date.Split('.')[0]);
            ddl_MONTH.SelectedValue = date.Split('.')[1];
            ddl_YEAR.SelectedValue = date.Split('.')[2];

            txt_comments.Text = form.Rows[0]["COMMENTS"].ToString();

            //disable everything
            DisableControls(form1);
            btn_submit.Visible = false;

            //bring ojti sign but check just in case
            if (meta.Rows[0]["OJTI_SIGNED"].ToString() == "True")
            {
                btn_ojtisign_Click(new object(), new EventArgs());
            }

            //bring trainee sing if signed
            if (meta.Rows[0]["TRAINEE_SIGNED"].ToString() == "True")
            {
                btn_traineesign_Click(new object(), new EventArgs());
            }


            // if not signed by trainee enable sign button
            if (mode == "trainee" && meta.Rows[0]["TRAINEE_SIGNED"].ToString() != "True")
            {
                //let them sign , let them comment
                btn_traineesign.Enabled = true;

                //let them submit
                btn_submit.Visible = true;
                btn_submit.Enabled = true;

                //change mode to allow update in reports table when submit button clicked
                lbl_viewmode.Text = "trainee";
            }
            if (mode == "department" && meta.Rows[0]["DEPARTMENT_SIGNED"].ToString() != "True")
            {
                //TODO : departMENT SIGN BUTTON AND FUNCTION
                //btn_DEPARTMENTSIGN.Enabled = true;

                //let them submit
                btn_submit.Visible = true;
                btn_submit.Enabled = true;

                //change mode to allow update in reports table when submit button clicked
                lbl_viewmode.Text = "department";
            }

            //todo: disable and hide elements based on mode
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

            string today = DateTime.UtcNow.ToString("dd.MM.yyyy");
            ddl_DAY.SelectedValue = today.Split('.')[0];
            ddl_MONTH.SelectedValue = Convert.ToInt32(today.Split('.')[1]).ToString();
            ddl_YEAR.SelectedValue = today.Split('.')[2];

            lbl_MER.Text = "( MER : " + DB_System.get_MER(ddl_trainee.SelectedValue, ddl_sectors.Text.Split('-')[0], ddl_sectors.Text.Split('-')[1], ddl_Level.SelectedValue) + ")";

            Dictionary<string, string> direct_dict = (Dictionary<string, string>)Session["direct_dictionary"];

            if (direct_dict != null || direct_dict.Count != 0)
                fill_from_CreateReport(direct_dict);
        }

        protected void fill_from_CreateReport(Dictionary<string, string> directed)
        {
            lbl_genid.Text = directed["genid"];

            ddl_trainee.SelectedValue = directed["traineeid"];
            ddl_trainee.Enabled = false;

            ddl_ojtis.SelectedValue = directed["ojtiid"];
            ddl_ojtis.Enabled = false;

            ddl_sectors.SelectedValue = directed["position"] + "-" + directed["sector"];
            ddl_sectors.Enabled = false;

            
            string name = directed["name"];
            if (name.Contains("LEVEL3"))
                ddl_Level.SelectedValue = "3";
            if (name.Contains("LEVEL2"))
                ddl_Level.SelectedValue = "2";
            if (name.Contains("LEVEL1"))
                ddl_Level.SelectedValue = "1";
            ddl_Level.Enabled = false;


            txt_totalhours.Text = DB_System.get_TOTALHOURS(directed["traineeid"], directed["position"], directed["sector"], directed["phase"]);
            txt_totalhours.Enabled = false;

            //ddl_positions.SelectedValue = directed["position"] + "-" + directed["sector"];
            //ddl_positions.Enabled = false;

            //todo: ojt-preojt-assess etc can be filled here as well
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

                if (DB_Reports.update_Sign_RECOM_LEVEL(lbl_reportnumber.Text, "trainee" , "")) 
                {
                    Response.Redirect("~/Pages/UserMain.aspx?Code=5&ID=" + lbl_reportnumber.Text);
                }
                else
                {
                    Response.Redirect("~/Pages/UserMain.aspx?Code=0");
                }
            }

            if (lbl_viewmode.Text == "department")
            {
                if (lbl_departmentsigned.Text != "1")
                {
                    ClientMessage(lbl_pageresult, "Training Department must sign to submit the report", System.Drawing.Color.Red);
                    return;
                }

                if (DB_Reports.update_Sign_RECOM_LEVEL(lbl_reportnumber.Text, "department", ((UserSession)Session["usersession"]).employeeid  )) 
                {
                    Response.Redirect("~/Pages/UserMain.aspx?Code=5&ID=" + lbl_reportnumber.Text);
                }
                else
                {
                    Response.Redirect("~/Pages/UserMain.aspx?Code=0");
                }
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
                //todo: GO TO SOME SORT OF RESULT PAGE
                lbl_pageresult.Text = "Your Report is filed with Report Number : " + reportid;
                lbl_pageresult.Visible = true;
                lbl_reportnumber.Text = reportid;
                lbl_reportnumber.Visible = true;
                DisableControls(form1);

                Response.Redirect("~/Pages/UserMain.aspx?Code=1&ID=" + reportid);
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
            
            if (txt_totalhours.Text == "" || txt_totalhours.Text == "0" )
            {
                ClientMessage(lbl_pageresult, "Choose Total Hours", System.Drawing.Color.Red);
                return false;
            }
            if (lbl_ojtisigned.Text != "1")
            {
                ClientMessage(lbl_pageresult, "OJTI must sign to submit the report", System.Drawing.Color.Red);
                return false;
            }

            if(!(chk_folder.Checked && chk_MER.Checked && chk_objectives.Checked && chk_reading.Checked))
            {
                ClientMessage(lbl_pageresult, "All requirements must be met and confirmed", System.Drawing.Color.Red);
                return false;
            }

            

            return true;
        }

        protected string  push_into_db()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("TRAINEE_ID", ddl_trainee.SelectedValue);
            data.Add("POSITION", ddl_sectors.SelectedValue.Split('-')[0]);
            data.Add("SECTOR", ddl_sectors.SelectedValue.Split('-')[1]);
            data.Add("PHASE",  "OJT" );
            data.Add("LEVEL",  ddl_Level.SelectedValue );
            data.Add("OJTI_ID", ddl_ojtis.SelectedValue);
            data.Add("OJTI_SIGNED", lbl_ojtisigned.Text);
            data.Add("TOTAL_HOURS", txt_totalhours.Text);
            data.Add("MER_MET", "1");
            data.Add("READING_SIGNED", "1");
            data.Add("OBJECTIVES_SIGNED", "1");
            data.Add("FOLDER_COMPLETE", "1");
            data.Add("TRAINEE_SIGNED", lbl_traineesigned.Text);
            data.Add("DATE", ddl_DAY.SelectedValue +"."+ddl_MONTH.SelectedValue + "."+ddl_YEAR.SelectedValue);
            data.Add("DEPARTMENT_SIGNED",  lbl_departmentsigned.Text);
            data.Add("DEPARTMENT_EMPLOYEEID", lbl_departmentsigned.Text == "1" ? ((UserSession)Session["usersession"]).employeeid : ""      );
            data.Add("COMMENTS", txt_comments.Text);

            data.Add("genid", lbl_genid.Text);

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
            img_ojtisign.ImageUrl = "~/Signatures/sign_id_" + ddl_ojtis.SelectedValue + ".jpeg";
            img_ojtisign.Visible = true;
            btn_ojtisign.Visible = false;
            lbl_ojtisigned.Text = "1";
        }

        protected void btn_traineesign_Click(object sender, EventArgs e)
        {
            img_traineesign.ImageUrl = "~/Signatures/sign_id_" + ddl_ojtis.SelectedValue + ".jpeg";
            img_traineesign.Visible = true;
            btn_traineesign.Visible = false;
            lbl_traineesigned.Text = "1";
        }

    }
}