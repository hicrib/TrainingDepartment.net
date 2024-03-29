﻿using System;
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



            //bring trainee sing if signed
            if (meta.Rows[0]["TRAINEE_SIGNED"].ToString() == "True")
            {
                img_sign_controller.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_trainee.SelectedValue)["SIGNATURE"].ToString();
                img_sign_controller.Visible = true;
                btn_sign_controller.Visible = false;
                lbl_recom_trainee_signed.Text = "1";
            }

            ddl_sectors.Items.Add(form.Rows[0]["POSITION"].ToString() + "-" + form.Rows[0]["SECTOR"].ToString());

            txt_totalhours.Text = form.Rows[0]["TOTAL_HOURS"].ToString();

            chk_MER.Checked = form.Rows[0]["MER_MET"].ToString() == "True";
            chk_folder.Checked = form.Rows[0]["FOLDER_COMPLETE"].ToString() == "True";
            chk_objectives.Checked = form.Rows[0]["OBJECTIVES_SIGNED"].ToString() == "True";
            chk_reading.Checked = form.Rows[0]["READING_SIGNED"].ToString() == "True";

            if (meta.Rows[0]["OJTI_SIGNED"].ToString() == "True")
                btn_ojtisign_Click(new object(), new EventArgs());

            txt_controller_date.Text = form.Rows[0]["DATE"].ToString();


            txt_comments.Text = form.Rows[0]["COMMENTS"].ToString();

            //disable everything
            DisableControls(pnl_wrapper);
            btn_Submit.Visible = false;

            //bring ojti sign but check just in case
            if (meta.Rows[0]["OJTI_SIGNED"].ToString() == "True")
            {
                img_ojtisign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_ojtis.SelectedValue)["SIGNATURE"].ToString();
                img_ojtisign.Visible = true;
                btn_ojtisign.Visible = false;
                lbl_recom_ojti_signed.Text = "1";
            }




            //if not signed by trainee enable sign button
            if (relation == "trainee" && meta.Rows[0]["TRAINEE_SIGNED"].ToString() != "True")
            {
                //let them sign , let them comment
                btn_sign_controller.Enabled = true;

                //let them submit
                btn_Submit.Visible = true;
                btn_Submit.Enabled = true;

                // change mode to allow update in reports table when submit button clicked
                lbl_viewmode.Text = "trainee";
            }
            if (relation == "TRN_DEPARTMENT_SIGNER" && meta.Rows[0]["DEPARTMENT_SIGNED"].ToString() != "True")
            {
                // 3 department signs required

                if (meta.Rows[0]["MEMBER1_ID"].ToString() != "")
                {
                    DataRow r = DB_System.getUserInfo(meta.Rows[0]["MEMBER1_ID"].ToString());
                    ddl_member1.Items.Add( new ListItem(r["INITIAL"].ToString() , meta.Rows[0]["MEMBER1_ID"].ToString()));
                    img_member1_sign.ImageUrl = AzureCon.general_container_url + r["SIGNATURE"].ToString();
                    lbl_member1_signed.Text = "1";
                    btn_member1.Visible = false;

                }
                else //hide others
                {
                    ddl_member1.Items.Add(new ListItem(user.initial, user.employeeid));
                    btn_member1.Enabled = true;
                    btn_member2.Enabled = false;
                    btn_member3.Enabled = false;
                    lbl_memberwho.Text = "1";
                }


                if (meta.Rows[0]["MEMBER2_ID"].ToString() != "")
                {
                    DataRow r = DB_System.getUserInfo(meta.Rows[0]["MEMBER2_ID"].ToString());
                    ddl_member2.Items.Add(new ListItem(r["INITIAL"].ToString(), meta.Rows[0]["MEMBER2_ID"].ToString()));
                    img_member2_sign.ImageUrl = AzureCon.general_container_url + r["SIGNATURE"].ToString();
                    lbl_member2_signed.Text = "1";
                    btn_member2.Visible = false;
                }
                else //hide others
                {
                    ddl_member2.Items.Add(new ListItem(user.initial, user.employeeid));
                    btn_member2.Enabled = true;
                    btn_member1.Enabled = false;
                    btn_member3.Enabled = false;
                    lbl_memberwho.Text = "2";
                }

                if (meta.Rows[0]["MEMBER3_ID"].ToString() != "")
                {
                    DataRow r = DB_System.getUserInfo(meta.Rows[0]["MEMBER3_ID"].ToString());
                    ddl_member3.Items.Add(new ListItem(r["INITIAL"].ToString(), meta.Rows[0]["MEMBER3_ID"].ToString()));
                    img_member3_sign.ImageUrl = AzureCon.general_container_url + r["SIGNATURE"].ToString();
                    lbl_member3_signed.Text = "1";
                    btn_member3.Visible = false;
                }
                else //hide others
                {
                    ddl_member3.Items.Add(new ListItem(user.initial, user.employeeid));
                    btn_member3.Enabled = true;
                    btn_member1.Enabled = false;
                    btn_member2.Enabled = false;
                    lbl_memberwho.Text = "3";
                }

                //let them submit
                btn_Submit.Visible = true;
                btn_Submit.Enabled = true;

                //change mode to allow update in reports table when submit button clicked
                lbl_viewmode.Text = "TRN_DEPARTMENT_SIGNER";
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


            Dictionary<string, string> direct_dict = (Dictionary<string, string>)Session["direct_dictionary"];

            if (direct_dict != null && direct_dict.Count != 0)
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

            txt_totalhours.Text = DB_Reports.get_TOTALHOURS(directed["traineeid"], directed["stepid"]);
            txt_totalhours.Enabled = false;

            string user_mer = DB_Reports.get_MER_sector(ddl_trainee.SelectedValue, directed["sector"]);
            string user_totalhours = DB_Reports.get_TOTALHOURS(directed["traineeid"], directed["stepid"]); //this gets all sector hours

            if (user_mer == "00:00")
            {
                lbl_MER.Text = "( MER : " + "UNDEFINED FOR USER" + ")";
                chk_MER.Checked = false;
                chk_MER.Enabled = false;
            }
            else
                lbl_MER.Text = "( MER : " + user_mer + ")";


            if (user_totalhours == "00:00" || Utility.isgreater_TimeFormat(user_mer, user_totalhours) > 0)
            {
                chk_MER.Checked = false;
                chk_MER.Enabled = false;
            }

            txt_totalhours.Text = user_totalhours;
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            //update student comments and sign
            if (lbl_viewmode.Text == "trainee")
            {
                if (DB_Reports.update_Sign_RECOM_CERTIF(lbl_reportnumber.Text, "trainee", ""))
                    SuccessWithCode("SUCCESS : REPORT SIGNED !");
                else
                    RedirectWithCode("Error: Try Again Later.");
            }
            else if (lbl_viewmode.Text == "reviewteam")
            {
                //todo: implement

                //if (DB_Reports.update_Sign_RECOM_LEVEL(lbl_reportnumber.Text, "department", ((UserSession)Session["usersession"]).employeeid))
                //    Response.Redirect("~/Pages/UserMain.aspx?Code=5&ID=" + lbl_reportnumber.Text);
                //else
                //    Response.Redirect("~/Pages/UserMain.aspx?Code=0");
            }
            else if (lbl_viewmode.Text == "TRN_DEPARTMENT_SIGNER")
            {
                //todo: implement
                bool done = false;
                if(lbl_membersigned.Text != "1")
                {
                    ClientMessage(lbl_pageresult, "DEPARTMENT MEMBER SHOULD SIGN BEFORE SUBMITTING", System.Drawing.Color.Red);
                    return;
                }

                if (lbl_memberwho.Text == "1")
                    done = DB_Reports.update_Sign_RECOM_CERTIF(lbl_reportnumber.Text, "MEMBER1", ddl_member1.SelectedValue);
                else if (lbl_memberwho.Text == "2")
                    done = DB_Reports.update_Sign_RECOM_CERTIF(lbl_reportnumber.Text, "MEMBER2", ddl_member2.SelectedValue);
                else if (lbl_memberwho.Text == "3")
                   done = DB_Reports.update_Sign_RECOM_CERTIF(lbl_reportnumber.Text, "MEMBER3", ddl_member3.SelectedValue);

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
                SuccessWithCode("SUCCESS : REPORT CREATED !");
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

            if (txt_controller_date.Text == "")
            {
                ClientMessage(lbl_pageresult, "Date must be entered", System.Drawing.Color.Red);
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

            data.Add("DATE", txt_controller_date.Text);

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

            img_member1_sign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_member1.SelectedValue)["SIGNATURE"].ToString();
            img_member1_sign.Visible = true;
            btn_member1.Visible = false;
            lbl_member1_signed.Text = "1";
            lbl_membersigned.Text = "1";
        }

        protected void btn_member2_Click(object sender, EventArgs e)
        {
            if (ddl_member2.SelectedValue == "-" || ddl_member2.SelectedValue == "0")
            {
                ClientMessage(lbl_pageresult, "Choose the Team Member 2 before signing.", System.Drawing.Color.Red);
                return;
            }

            img_member2_sign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_member2.SelectedValue)["SIGNATURE"].ToString();
            img_member2_sign.Visible = true;
            btn_member2.Visible = false;
            lbl_member2_signed.Text = "1";
            lbl_membersigned.Text = "1";

        }

        protected void btn_member3_Click(object sender, EventArgs e)
        {
            if (ddl_member3.SelectedValue == "-" || ddl_member3.SelectedValue == "0")
            {
                ClientMessage(lbl_pageresult, "Choose the Team Member 3 before signing.", System.Drawing.Color.Red);
                return;
            }

            img_member3_sign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_member3.SelectedValue)["SIGNATURE"].ToString();
            img_member3_sign.Visible = true;
            btn_member3.Visible = false;
            lbl_member3_signed.Text = "1";
            lbl_membersigned.Text = "1";
        }
    }
}
