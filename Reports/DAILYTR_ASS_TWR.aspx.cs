﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AviaTrain.App_Code;
using System.Data;

namespace AviaTrain.Reports
{
    public partial class DAILYTR_ASS_TWR : MasterPage
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

                    if (user.isAdmin)
                        relation = "sysadmin";

                    // Check if trainee will sign OR there is privilege to view
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
                    fill_Default_Page_Elements(); //normal mode, filling form
                }

                Page.Form.Attributes.Add("enctype", "multipart/form-data");
            }
        }

        protected void fill_View_Mode_as(string reportid, string mode)
        {
            //todo: first fill all elements
            Dictionary<string, DataTable> li = DB_Reports.pull_DAILYTR_ASS_TWR(reportid);

            if (li == null)
                Response.Redirect("~/Pages/UserMain.aspx?Code=4&ID=" + reportid);

            lbl_reportnumber.Text = reportid;

            DataTable meta = li["meta"];
            DataTable form = li["form"];
            DataTable skills = li["skills"];
            lbl_viewmode.Text = "viewonly";


            ddl_trainees.Items.Add(new ListItem(meta.Rows[0]["TRAINEE_NAME"].ToString(), meta.Rows[0]["TRAINEE_ID"].ToString()));
            ddl_ojtis.Items.Add(new ListItem(meta.Rows[0]["CREATER_NAME"].ToString(), meta.Rows[0]["CREATER"].ToString()));

            chk_OJT.Checked = form.Rows[0]["CHK_OJT"].ToString() == "True";
            chk_Ass.Checked = form.Rows[0]["CHK_ASS"].ToString() == "True";

            string date = form.Rows[0]["DATE"].ToString();
            txt_date.Text = date;
            //ddl_DAY.Items.Add(date.Split('.')[0]);
            //ddl_MONTH.SelectedValue = date.Split('.')[1];
            //ddl_YEAR.SelectedValue = date.Split('.')[2];


            ddl_positions.Items.Add("ASSIST");
            ddl_positions.SelectedValue = form.Rows[0]["POSITION"].ToString();

            txt_timeon_sch.Text = form.Rows[0]["TIMEON_SCH"].ToString();
            txt_timeoff_sch.Text = form.Rows[0]["TIMEOFF_SCH"].ToString();

            txt_timeon_act.Text = form.Rows[0]["TIMEON_ACT"].ToString();
            txt_timeoff_act.Text = form.Rows[0]["TIMEOFF_ACT"].ToString();

            chk_den_L.Checked = form.Rows[0]["TRAF_DENS"].ToString().Contains("L");
            chk_den_M.Checked = form.Rows[0]["TRAF_DENS"].ToString().Contains("M");
            chk_den_H.Checked = form.Rows[0]["TRAF_DENS"].ToString().Contains("H");

            chk_comp_L.Checked = form.Rows[0]["COMPLEXITY"].ToString().Contains("L");
            chk_comp_M.Checked = form.Rows[0]["COMPLEXITY"].ToString().Contains("M");
            chk_comp_H.Checked = form.Rows[0]["COMPLEXITY"].ToString().Contains("H");

            txt_hours.Text = form.Rows[0]["HOURS"].ToString();
            txt_totalhours.Text = form.Rows[0]["TOTAL_HOURS"].ToString();


            img_file.ImageUrl = AzureCon.general_container_url + form.Rows[0]["PREBRIEF_COMMENTS_FILENAME"].ToString();
            if (form.Rows[0]["PREBRIEF_COMMENTS_FILENAME"].ToString() != "")
                img_file.Visible = true;
            txt_prebrief_comment.Text = form.Rows[0]["PREBRIEF_COMMENTS"].ToString();
            txt_prebrief_comment.Visible = true;
            file_prebrief_comment.Visible = false;
            UploadButton.Visible = false;

            txt_additionalcomments.Text = form.Rows[0]["ADDITIONAL_COMMENTS"].ToString();
            //txt_notes.Text = form.Rows[0]["NOTES"].ToString();
            txt_studentcomments.Text = form.Rows[0]["STUDENT_COMMENTS"].ToString();

            foreach (DataRow ro in skills.Rows)
            {
                string groupname = "gr" + ro["CATEG_SKILL"].ToString(); //gr1A, gr3B
                string answer = ro["RESULT"].ToString();

                List<RadioButton> answer_radios = Utility.GetRadiobuttonsbyGroupname(evaluation_panel, groupname);
                RadioButton rad = Utility.FindRadioButtonToSelect(answer_radios, answer);
                if (rad != null)
                    rad.Checked = true;
            }

            //disable everything
            DisableControls(pnl_wrapper);
            btn_submit.Visible = false;

            //bring ojti sign but check just in case
            if (meta.Rows[0]["OJTI_SIGNED"].ToString() == "True")
            {
                btn_sign_ojti_Click(new object(), new EventArgs());
            }

            //bring trainee sing if signed
            if (meta.Rows[0]["TRAINEE_SIGNED"].ToString() == "True")
            {
                img_traineesign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_trainees.SelectedValue)["SIGNATURE"].ToString();
                img_traineesign.Visible = true;
                btn_sign_trainee.Visible = false;
                lbl_trainee_signed.Text = "1";
            }


            // if not signed by trainee enable sign button
            if (mode == "trainee" && meta.Rows[0]["TRAINEE_SIGNED"].ToString() != "True")
            {

                //let them sign , let them comment
                btn_sign_trainee.Enabled = true;

                if (txt_studentcomments.Text == "")
                    txt_studentcomments.Enabled = true;

                //let them submit
                btn_submit.Visible = true;
                btn_submit.Enabled = true;

                //change mode to allow update in reports table when submit button clicked
                lbl_viewmode.Text = "trainee";
            }







            //todo: disable and hide elements based on mode
        }

        protected void fill_Default_Page_Elements()
        {
            //POSITION field fill
            
            ddl_positions.Items.Add("TWR-ASSIST");

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

            DataTable trainees = DB_System.get_ALL_trainees();
            if (trainees != null)
            {
                ddl_trainees.DataSource = trainees;
                ddl_trainees.DataTextField = "NAME";
                ddl_trainees.DataValueField = "ID";
                ddl_trainees.DataBind();
            }
        }

        
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (file_prebrief_comment.HasFile)
            {
                try
                {
                    string filename = Utility.getRandomFileName();
                    string newfilename = filename + "_" + file_prebrief_comment.PostedFile.FileName;
                    string file_address = Server.MapPath("~/AzureBlobs/Uploads/") + filename + "_" + file_prebrief_comment.PostedFile.FileName;
                    file_prebrief_comment.SaveAs(file_address);

                    //will be pushed to db
                    uploadedfilename.Text = newfilename;

                    //show the image , no need to read it from azure now
                    img_file.Visible = true;
                    img_file.ImageUrl = "~/AzureBlobs/Uploads/" + filename + "_" + file_prebrief_comment.PostedFile.FileName;

                    if (!AzureCon.upload_ToBlob_fromFile(file_address))
                    {
                        //todo : what to do when error
                        throw new Exception();
                    }
                }
                catch (Exception ex)
                {
                    statuslabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }

        protected void rad_prebrief_comment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rad_prebrief_comment.SelectedValue == "File")
            {
                file_prebrief_comment.Visible = true;
                txt_prebrief_comment.Visible = false;
                statuslabel.Visible = true;
                UploadButton.Visible = true;
            }
            else
            {
                file_prebrief_comment.Visible = false;
                txt_prebrief_comment.Visible = true;
                statuslabel.Visible = false;
                UploadButton.Visible = false;
            }


        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            //update student comments and sign
            if (lbl_viewmode.Text == "trainee")
            {
                if (lbl_trainee_signed.Text != "1")
                {
                    ClientMessage(lbl_pageresult, "Trainee must sign to submit the report", System.Drawing.Color.Red);
                    return;
                }

                if (DB_Reports.update_TraineeSigned(lbl_reportnumber.Text, txt_studentcomments.Text, "2")) //signed, commented and reporttype:2
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
                SuccessWithCode("SUCCESS : REPORT CREATED !");
            }

        }

        protected bool Check_Page_Elements()
        {
            if (ddl_trainees.SelectedValue == "-" || ddl_trainees.SelectedValue == "0")
            {
                ClientMessage(lbl_pageresult, "Choose the Trainee before signing!", System.Drawing.Color.Red);
                return false;
            }
            if (!chk_OJT.Checked && !chk_Ass.Checked)
            {
                ClientMessage(lbl_pageresult, "Choose the type of training!", System.Drawing.Color.Red);
                return false;
            }
            if (ddl_positions.SelectedValue == "-")
            {
                ClientMessage(lbl_pageresult, "Choose Position!", System.Drawing.Color.Red);
                return false;
            }
            if (txt_date.Text == "")
            {
                ClientMessage(lbl_pageresult, "Choose Date", System.Drawing.Color.Red);
                return false;
            }
            if (txt_timeon_act.Text == "" || txt_timeoff_act.Text == "")
            {
                ClientMessage(lbl_pageresult, "Choose Actual TIME ON/OFF!", System.Drawing.Color.Red);
                return false;
            }
            if (txt_timeon_sch.Text == "" || txt_timeoff_sch.Text == "")
            {
                ClientMessage(lbl_pageresult, "Choose Scheduled TIME ON/OFF!", System.Drawing.Color.Red);
                return false;
            }
            if ((!chk_comp_L.Checked && !chk_comp_M.Checked && !chk_comp_H.Checked)
                || (!chk_den_L.Checked && !chk_den_M.Checked && !chk_den_H.Checked))
            {
                ClientMessage(lbl_pageresult, "Choose Traffic Density and Complexity", System.Drawing.Color.Red);
                return false;
            }
            if (txt_hours.Text == "")
            {
                ClientMessage(lbl_pageresult, "Choose Hours", System.Drawing.Color.Red);
                return false;
            }
            if (txt_totalhours.Text == "")
            {
                ClientMessage(lbl_pageresult, "Choose Total Hours", System.Drawing.Color.Red);
                return false;
            }
            if (lbl_ojti_signed.Text != "1")
            {
                ClientMessage(lbl_pageresult, "OJTI must sign to submit the report", System.Drawing.Color.Red);
                return false;
            }

            return true;
        }

        protected string push_into_db()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("TRAINEE_ID", ddl_trainees.SelectedValue);
            data.Add("OJTI_ID", ddl_ojtis.SelectedValue);
            data.Add("CHK_OJT", chk_OJT.Checked ? "1" : "0");
            data.Add("CHK_ASS", chk_Ass.Checked ? "1" : "0");


            data.Add("OJTI_SIGNED", lbl_ojti_signed.Text == "1" ? "1" : "0");
            data.Add("TRAINEE_SIGNED", lbl_trainee_signed.Text == "1" ? "1" : "0");

            //string date = ddl_DAY.SelectedValue + "." + ddl_MONTH.SelectedValue + "." + ddl_YEAR.SelectedValue;
            data.Add("DATE", txt_date.Text);
            data.Add("POSITION", ddl_positions.SelectedValue);

            data.Add("TIMEON_SCH", txt_timeon_sch.Text);
            data.Add("TIMEOFF_SCH", txt_timeoff_sch.Text);

            data.Add("TIMEON_ACT", txt_timeon_act.Text);
            data.Add("TIMEOFF_ACT", txt_timeoff_act.Text);

            string density = (chk_den_L.Checked ? "L" : "") + (chk_den_M.Checked ? "M" : "") + (chk_den_H.Checked ? "H" : "");
            data.Add("TRAF_DENS", density);

            string complexity = (chk_comp_L.Checked ? "L" : "") + (chk_comp_M.Checked ? "M" : "") + (chk_comp_H.Checked ? "H" : "");
            data.Add("COMPLEXITY", complexity);

            txt_timeon_act_TextChanged(new object(), new EventArgs());
            data.Add("HOURS", txt_hours.Text);

            data.Add("TOTAL_HOURS", Utility.add_TimeFormat(txt_totalhours.Text, txt_hours.Text));

            data.Add("PREBRIEF_COMMENTS_FILENAME", uploadedfilename.Text);
            data.Add("PREBRIEF_COMMENTS", txt_prebrief_comment.Text);
            //data.Add("NOTES", txt_notes.Text);
            data.Add("ADDITIONAL_COMMENTS", txt_additionalcomments.Text);
            data.Add("STUDENT_COMMENTS", txt_studentcomments.Text);

            data.Add("1A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr1A")));
            data.Add("1B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr1B")));
            data.Add("1C", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr1C")));
            data.Add("1D", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr1D")));
            data.Add("1E", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr1E")));
            data.Add("1F", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr1F")));

            data.Add("2A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr2A")));
            data.Add("2B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr2B")));
            data.Add("2C", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr2C")));
            data.Add("2D", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr2D")));

            data.Add("3A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr3A")));
            data.Add("3B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr3B")));
            data.Add("3C", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr3C")));

            data.Add("4A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr4A")));
            data.Add("4B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr4B")));
            data.Add("4C", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr4C")));

            data.Add("5A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5A")));
            data.Add("5B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5B")));
            data.Add("5C", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5C")));
            data.Add("5D", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5D")));
            data.Add("5E", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5E")));

            data.Add("6A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr6A")));
            data.Add("6B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr6B")));

            data.Add("STEPID", lbl_STEPID.Text);

            string reportid = DB_Reports.push_Training_Report("2",data);
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


        protected void btn_sign_trainee_Click(object sender, EventArgs e)
        {
            if (ddl_trainees.SelectedValue == "-" || ddl_trainees.SelectedValue == "0")
            {
                ClientMessage(lbl_pageresult, "Choose the Trainee before signing.", System.Drawing.Color.Red);
                return;
            }
            UserSession user = (UserSession)Session["usersession"];
            if (user.employeeid != ddl_trainees.SelectedValue)
            {
                ClientMessage(lbl_pageresult, "Only Trainee can sign for trainee", System.Drawing.Color.Red);
                return;
            }

            img_traineesign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_trainees.SelectedValue)["SIGNATURE"].ToString();
            img_traineesign.Visible = true;
            btn_sign_trainee.Visible = false;
            lbl_trainee_signed.Text = "1";
        }

        protected void btn_sign_ojti_Click(object sender, EventArgs e)
        {
            img_ojtisign.ImageUrl = AzureCon.general_container_url + DB_System.getUserInfo(ddl_ojtis.SelectedValue)["SIGNATURE"].ToString();
            img_ojtisign.Visible = true;
            btn_sign_ojti.Visible = false;
            lbl_ojti_signed.Text = "1";
        }

        protected void txt_timeon_act_TextChanged(object sender, EventArgs e)
        {
            if (txt_timeon_act.Text == "" || txt_timeoff_act.Text == "")
                return;

            txt_hours.Text = Utility.subtract_TimeFormat(txt_timeon_act.Text, txt_timeoff_act.Text);
        }
    }
}
