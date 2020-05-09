﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AviaTrain.App_Code;

namespace AviaTrain.Reports
{
    public partial class DAILYTR_ASS_RAD : MasterPage
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
            Dictionary<string, DataTable> li = DB_Reports.pull_DAILYTR_ASS_RAD(reportid);

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

            ddl_positions.Items.Add(new ListItem(form.Rows[0]["POSITION_EXTRA"].ToString(), form.Rows[0]["POSITION"].ToString()));
            //ddl_positions.SelectedValue = form.Rows[0]["POSITION"].ToString();

            txt_timeon.Text = form.Rows[0]["TIMEON"].ToString();
            txt_timeoff.Text = form.Rows[0]["TIMEOFF"].ToString();


            radio_density.SelectedValue = form.Rows[0]["TRAF_DENS"].ToString();
            radio_complexity.SelectedValue = form.Rows[0]["COMPLEXITY"].ToString();

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
            DisableControls(form1);
            btn_submit.Visible = false;

            //bring ojti sign but check just in case
            if (meta.Rows[0]["OJTI_SIGNED"].ToString() == "True")
            {
                btn_sign_ojti_Click(new object(), new EventArgs());
            }

            //bring trainee sing if signed
            if (meta.Rows[0]["TRAINEE_SIGNED"].ToString() == "True")
            {
                btn_sign_trainee_Click(new object(), new EventArgs());
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
            DataTable pozs = DB_System.get_APP_ACC_Positions();
            if (pozs != null)
            {
                ddl_positions.DataSource = pozs;
                ddl_positions.DataTextField = "LONG";
                ddl_positions.DataValueField = "SHORT";
                ddl_positions.DataBind();
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

            DataTable trainees = DB_System.get_ALL_trainees();
            if (trainees != null)
            {
                ddl_trainees.DataSource = trainees;
                ddl_trainees.DataTextField = "NAME";
                ddl_trainees.DataValueField = "ID";
                ddl_trainees.DataBind();
            }

            Dictionary<string, string> direct_dict = (Dictionary<string, string>)Session["direct_dictionary"];
            if (direct_dict != null || direct_dict.Count != 0)
                fill_from_CreateReport(direct_dict);
        }

        protected void fill_from_CreateReport(Dictionary<string, string> directed)
        {
            lbl_genid.Text = directed["genid"];

            ddl_trainees.SelectedValue = directed["traineeid"];
            ddl_trainees.Enabled = false;

            ddl_ojtis.SelectedValue = directed["ojtiid"];
            ddl_ojtis.Enabled = false;

            //remove the ones not containing sector
            List<ListItem> l = Utility.remove_sectors(ddl_positions.Items, directed["sector"]);
            ddl_positions.Items.Clear();
            foreach (ListItem item in l)
            {
                ddl_positions.Items.Add(item);
            }


            //ddl_positions.SelectedValue = directed["position"] + "-" + directed["sector"];
            //ddl_positions.Enabled = false;

            //todo: ojt-PRELEVEL1-assess etc can be filled here as well
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

            string pos = ddl_positions.SelectedValue;
            if (pos.Length == 3)
                pos = pos.Substring(0, 2);
            data.Add("POSITION", pos);
            data.Add("POSITION_EXTRA", ddl_positions.SelectedItem.Text);


            data.Add("TIMEON", txt_timeon.Text);
            data.Add("TIMEOFF", txt_timeoff.Text);

            data.Add("TRAF_DENS", radio_density.SelectedValue);
            data.Add("COMPLEXITY", radio_complexity.SelectedValue);
            data.Add("HOURS", txt_hours.Text);
            data.Add("TOTAL_HOURS", txt_totalhours.Text);

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
            data.Add("2E", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr2E")));

            data.Add("3A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr3A")));
            data.Add("3B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr3B")));
            data.Add("3C", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr3C")));
            data.Add("3D", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr3D")));
            data.Add("3E", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr3E")));

            data.Add("4A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr4A")));
            data.Add("4B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr4B")));
            data.Add("4C", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr4C")));
            data.Add("4D", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr4D")));

            data.Add("5A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5A")));
            data.Add("5B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5B")));
            data.Add("5C", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5C")));
            data.Add("5D", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5D")));
            data.Add("5E", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5E")));
            data.Add("5F", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr5F")));

            data.Add("6A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr6A")));
            data.Add("6B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr6B")));
            data.Add("6C", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr6C")));
            data.Add("6D", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr6D")));
            data.Add("6E", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr6E")));
            data.Add("6F", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr6F")));
            data.Add("6G", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr6G")));

            data.Add("7A", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr7A")));
            data.Add("7B", Utility.GetSelectedRadioButtonValue(Utility.GetRadiobuttonsbyGroupname(evaluation_panel, "gr7B")));

            if (lbl_genid.Text == "")
            {
                lbl_pageresult.Text = "genid empty?";
                lbl_pageresult.Visible = true;
                return "";
            }
            data.Add("genid", lbl_genid.Text);

            string reportid = DB_Reports.push_DAILYTR_ASS_RAD(data);
            return reportid;
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
                ClientMessage(lbl_pageresult, "Choose Date!", System.Drawing.Color.Red);
                return false;
            }
            if (txt_timeon.Text == "")
            {
                ClientMessage(lbl_pageresult, "Choose TIME ON!", System.Drawing.Color.Red);
                return false;
            }
            if (txt_timeoff.Text == "")
            {
                ClientMessage(lbl_pageresult, "Choose TIME OFF!", System.Drawing.Color.Red);
                return false;
            }
            if (radio_complexity.SelectedValue == "" || radio_density.SelectedValue == "")
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

                if (DB_Reports.update_TraineeSigned(lbl_reportnumber.Text, txt_studentcomments.Text, "3")) //signed, commented and reporttype:4
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


    }
}