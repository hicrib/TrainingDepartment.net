using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AviaTrain.App_Code;

namespace AviaTrain.Reports
{
    public partial class RECOM_CERTIF : MasterPage
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
                btn_sign_controller_Click(new object(), new EventArgs());

            ddl_sectors.Items.Add(form.Rows[0]["POSITION"].ToString() + "-" + form.Rows[0]["SECTOR"].ToString());

            txt_totalhours.Text = form.Rows[0]["TOTAL_HOURS"].ToString();

            chk_MER.Checked = form.Rows[0]["MER_MET"].ToString() == "True";
            chk_folder.Checked = form.Rows[0]["FOLDER_COMPLETE"].ToString() == "True";
            chk_objectives.Checked = form.Rows[0]["OBJECTIVES_SIGNED"].ToString() == "True";
            chk_reading.Checked = form.Rows[0]["READING_SIGNED"].ToString() == "True";

            if (meta.Rows[0]["OJTI_SIGNED"].ToString() == "True")
                btn_ojtisign_Click(new object(), new EventArgs());

            string date = form.Rows[0]["DATE"].ToString();
            ddl_DAY_controller.Items.Add(date.Split('.')[0]);
            ddl_MONTH_controller.SelectedValue = date.Split('.')[1];
            ddl_YEAR_controller.SelectedValue = date.Split('.')[2];

            txt_comments.Text = form.Rows[0]["COMMENTS"].ToString();

            //disable everything
            DisableControls(form1);
            btn_Submit.Visible = false;

            //bring ojti sign but check just in case
            if (meta.Rows[0]["OJTI_SIGNED"].ToString() == "True")
            {
                btn_ojtisign_Click(new object(), new EventArgs());
            }

            //bring trainee sing if signed
            if (meta.Rows[0]["TRAINEE_SIGNED"].ToString() == "True")
            {
                btn_sign_controller_Click(new object(), new EventArgs());
            }


            //if not signed by trainee enable sign button
            if (mode == "trainee" && meta.Rows[0]["TRAINEE_SIGNED"].ToString() != "True")
            {
                //let them sign , let them comment
                btn_sign_controller.Enabled = true;

                //let them submit
                btn_Submit.Visible = true;
                btn_Submit.Enabled = true;

                // change mode to allow update in reports table when submit button clicked
                lbl_viewmode.Text = "trainee";
            }
            if (mode == "department" && meta.Rows[0]["DEPARTMENT_SIGNED"].ToString() != "True")
            {
                //TODO: departMENT SIGN BUTTON AND FUNCTION
                //btn_DEPARTMENTSIGN.Enabled = true;

                //let them submit
                btn_Submit.Visible = true;
                btn_Submit.Enabled = true;

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

                ddl_member1.DataSource = trainees;
                ddl_member1.DataTextField = "NAME";
                ddl_member1.DataValueField = "ID";
                ddl_member1.DataBind();
                ddl_member2.DataSource = trainees;
                ddl_member2.DataTextField = "NAME";
                ddl_member2.DataValueField = "ID";
                ddl_member2.DataBind();
                ddl_member3.DataSource = trainees;
                ddl_member3.DataTextField = "NAME";
                ddl_member3.DataValueField = "ID";
                ddl_member3.DataBind();
            }
            ddl_ojtis.SelectedValue = ((UserSession)Session["usersession"]).employeeid;
            //todo: some extra powerful positions may create for other people
            ddl_ojtis.Enabled = false;

            ddl_sectors.DataSource = DB_System.get_Sectors(); //pos-sec format
            ddl_sectors.DataValueField = "CODE";
            ddl_sectors.DataBind();

            string today = DateTime.UtcNow.ToString("yyyy-MM-dd");
            ddl_DAY_controller.SelectedValue = today.Split('.')[0];
            ddl_MONTH_controller.SelectedValue = Convert.ToInt32(today.Split('.')[1]).ToString();
            ddl_YEAR_controller.SelectedValue = today.Split('.')[2];

            lbl_MER.Text = "( MER : " + DB_System.get_MER(ddl_trainee.SelectedValue, ddl_sectors.Text.Split('-')[0], ddl_sectors.Text.Split('-')[1], "") + ")";//ddl_level vardı burada

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

            txt_totalhours.Text = DB_System.get_TOTALHOURS(directed["traineeid"], directed["position"], directed["sector"], directed["phase"]);
            txt_totalhours.Enabled = false;
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            //update student comments and sign
            if (lbl_viewmode.Text == "trainee")
            {
                if (lbl_controller_sign.Text != "1")
                {
                    ClientMessage(lbl_pageresult, "Trainee must sign to submit the report", System.Drawing.Color.Red);
                    return;
                }

                if (DB_Reports.update_Sign_RECOM_LEVEL(lbl_reportnumber.Text, "trainee", ""))
                {
                    Response.Redirect("~/Pages/UserMain.aspx?Code=5&ID=" + lbl_reportnumber.Text);
                }
                else
                {
                    Response.Redirect("~/Pages/UserMain.aspx?Code=0");
                }
            }
            else if (lbl_viewmode.Text == "reviewteam")
            {

                //todo: implement

                if (DB_Reports.update_Sign_RECOM_LEVEL(lbl_reportnumber.Text, "department", ((UserSession)Session["usersession"]).employeeid))
                {
                    Response.Redirect("~/Pages/UserMain.aspx?Code=5&ID=" + lbl_reportnumber.Text);
                }
                else
                {
                    Response.Redirect("~/Pages/UserMain.aspx?Code=0");
                }
            }
            else if (lbl_viewmode.Text == "reviewteam")
            {
                //todo: implement
            }


            //in normal first submit mode : make all checks regarding page elements
            bool all_good = Check_Page_Elements();
            if (!all_good)
                return;


            //we insert everything in db
            string reportid = push_into_db();
            if (String.IsNullOrWhiteSpace(reportid))
            {
                //todo: error message
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

            if (txt_totalhours.Text == "" || txt_totalhours.Text == "0")
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

            if (!rad_YES.Checked && !rad_NO.Checked)
            {
                ClientMessage(lbl_pageresult, "Review Team Approval must be chosen", System.Drawing.Color.Red);
                return false;
            }

            //  todo: review team approval ???
            //todo : team member approval??



            return true;
        }

        protected string push_into_db()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("TRAINEE_ID", ddl_trainee.SelectedValue);

            data.Add("POSITION", ddl_sectors.SelectedValue.Split('-')[0]);
            data.Add("SECTOR", ddl_sectors.SelectedValue.Split('-')[1]);
            data.Add("PHASE", "OJT");

            data.Add("OJTI_ID", ddl_ojtis.SelectedValue);
            data.Add("OJTI_SIGNED", lbl_ojtisigned.Text);

            data.Add("TOTAL_HOURS", txt_totalhours.Text);
            data.Add("MER_MET", "1");
            data.Add("READING_SIGNED", "1");
            data.Add("OBJECTIVES_SIGNED", "1");
            data.Add("FOLDER_COMPLETE", "1");

            data.Add("TRAINEE_SIGNED", lbl_recom_trainee_signed.Text);
            data.Add("TRAINEE_SIGN_DATE", lbl_recom_trainee_signed.Text == "1" ? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") : "");

            data.Add("DATE", ddl_DAY_controller.SelectedValue + "." + ddl_MONTH_controller.SelectedValue + "." + ddl_YEAR_controller.SelectedValue);

            data.Add("REVIEW_TEAM_APPROVAL", rad_NO.Checked ? "0" : "1");
            data.Add("REVIEW_TEAM_APPROVAL_SIGN_DATE", rad_NO.Checked ? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") : "");

            data.Add("COMMENTS", txt_comments.Text);

            data.Add("MEMBER1_ID", "");
            data.Add("MEMBER1_SIGNED", "");
            data.Add("MEMBER1_SIGN_DATE", "");

            data.Add("MEMBER2_ID", "");
            data.Add("MEMBER2_SIGNED", "");
            data.Add("MEMBER2_SIGN_DATE", "");

            data.Add("MEMBER3_ID", "");
            data.Add("MEMBER3_SIGNED", "");
            data.Add("MEMBER3_SIGN_DATE", "");

            data.Add("genid", lbl_genid.Text);

            string reportid = DB_Reports.push_RECOM_CERTIF(data);
            return reportid;
        }


        private void DisableControls(System.Web.UI.Control control)
        {
            foreach (System.Web.UI.Control c in control.Controls)
            {
                //Get the Enabled property by reflection.
                Type type = c.GetType();
                PropertyInfo prop = type.GetProperty("Enabled");

                //Set it to False to disable the control.
                if (prop != null)
                {
                    prop.SetValue(c, false, null);
                }

                //Recurse into child controls.
                if (c.Controls.Count > 0)
                {
                    this.DisableControls(c);
                }
            }

        }


        protected void btn_ojtisign_Click(object sender, EventArgs e)
        {
            if (ddl_ojtis.SelectedValue == "-" || ddl_ojtis.SelectedValue == "0")
            {
                ClientMessage(lbl_pageresult, "Choose the Recommending OJTI before signing.", System.Drawing.Color.Red);
                return;
            }

            img_ojtisign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_ojtis.SelectedValue)["SIGNATURE"].ToString();
            img_ojtisign.Visible = true;
            btn_ojtisign.Visible = false;
            lbl_recom_ojti_signed.Text = "1";
        }

        protected void btn_sign_controller_Click(object sender, EventArgs e)
        {
            img_sign_controller.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_trainee.SelectedValue)["SIGNATURE"].ToString();
            img_sign_controller.Visible = true;
            btn_sign_controller.Visible = false;
            lbl_recom_trainee_signed.Text = "1";
        }

        protected void btn_member1_Click(object sender, EventArgs e)
        {
            if (ddl_member1.SelectedValue == "-" || ddl_member1.SelectedValue == "0")
            {
                ClientMessage(lbl_pageresult, "Choose the Team Member 1 before signing.", System.Drawing.Color.Red);
                return;
            }

            img_member1_sign.ImageUrl = "~/Signatures/sign_id_" + ddl_member1.SelectedValue + ".jpeg";
            img_member1_sign.Visible = true;
            btn_member1.Visible = false;
            lbl_member1_signed.Text = "1";
        }

        protected void btn_member2_Click(object sender, EventArgs e)
        {
            if (ddl_member2.SelectedValue == "-" || ddl_member2.SelectedValue == "0")
            {
                ClientMessage(lbl_pageresult, "Choose the Team Member 2 before signing.", System.Drawing.Color.Red);
                return;
            }

            img_member2_sign.ImageUrl = "~/Signatures/sign_id_" + ddl_member2.SelectedValue + ".jpeg";
            img_member2_sign.Visible = true;
            btn_member2.Visible = false;
            lbl_member2_signed.Text = "1";

        }

        protected void btn_member3_Click(object sender, EventArgs e)
        {
            if (ddl_member3.SelectedValue == "-" || ddl_member3.SelectedValue == "0")
            {
                ClientMessage(lbl_pageresult, "Choose the Team Member 3 before signing.", System.Drawing.Color.Red);
                return;
            }

            img_member3_sign.ImageUrl = "~/Signatures/sign_id_" + ddl_member3.SelectedValue + ".jpeg";
            img_member3_sign.Visible = true;
            btn_member3.Visible = false;
            lbl_member3_signed.Text = "1";
        }
    }
}
