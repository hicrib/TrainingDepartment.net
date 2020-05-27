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
    public partial class EditNotification : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fill_grid();
                Session["file1"] = null;
                Session["file2"] = null;
                Session["file3"] = null;
                Session["file4"] = null;
            }
        }

        protected void fill_grid()
        {
            grid_notifications.DataSource = DB_System.get_notification(isactive: false);
            grid_notifications.DataBind();
        }

        protected void grid_notifications_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortingDirection = string.Empty;
            if (direction == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
                sortingDirection = "Desc";
            }
            else
            {
                direction = SortDirection.Ascending;
                sortingDirection = "Asc";
            }

            DataView sortedView = new DataView(DB_System.get_notification(isactive: false));
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            Session["SortedView"] = sortedView;
            grid_notifications.DataSource = sortedView;
            grid_notifications.DataBind();
        }

        protected void grid_notifications_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid_notifications.PageIndex = e.NewPageIndex;
            fill_grid();
        }

        public SortDirection direction
        {
            get
            {
                if (ViewState["directionState"] == null)
                    ViewState["directionState"] = SortDirection.Ascending;

                return (SortDirection)ViewState["directionState"];
            }
            set
            {
                ViewState["directionState"] = value;
            }
        }

        protected void grid_notifications_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grid_notifications_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "GO")
            {
                //GridViewRow selectedRow = grid_notifications.Rows[Convert.ToInt32()];
                lbl_notifid.Text = (string)e.CommandArgument;

                DataTable notif = DB_System.get_notification(notifid: lbl_notifid.Text, isactive: false);
                if (notif == null || notif.Rows.Count == 0)
                    return;

                btn_publish.Visible = true;

                lbl_pageresult.Text = "";
                lbl_pageresult.Visible = false;
                Session["file1"] = null;
                Session["file2"] = null;
                Session["file3"] = null;
                Session["file4"] = null;

                ddl_type.SelectedValue = notif.Rows[0]["TYPE"].ToString();
                ddl_generic.Items.Add(notif.Rows[0]["TO"].ToString());
                txt_header.Text = notif.Rows[0]["HEADER"].ToString();
                txt_message.Text = notif.Rows[0]["TEXT"].ToString();
                txt_effective.Text = notif.Rows[0]["EFFECTIVE"].ToString();
                txt_expires.Text = notif.Rows[0]["EXPIRED"].ToString();
                chk_active.Checked = notif.Rows[0]["ISACTIVE"].ToString() == "True";

                string file1 = notif.Rows[0]["FILE1"].ToString();
                string file2 = notif.Rows[0]["FILE2"].ToString();
                string file3 = notif.Rows[0]["FILE3"].ToString();
                string file4 = notif.Rows[0]["FILE4"].ToString();

                if (file1 != "")
                {
                    lnk_file1.Visible = true;
                    lnk_file1.Text = Utility.last_part(file1, "_x_");
                    lnk_file1.Attributes.Add("href", file1);
                    lnk_file1.Attributes.Add("target", "_blank");
                    del_file1.Visible = true;
                    Session["file1"] = file1;
                }
                else lnk_file1.Visible = false;

                if (file2 != "")
                {
                    lnk_file2.Visible = true;
                    lnk_file2.Text = Utility.last_part(file2, "_x_");
                    lnk_file2.Attributes.Add("href", file2);
                    lnk_file2.Attributes.Add("target", "_blank");
                    del_file2.Visible = true;
                    Session["file2"] = file2;
                }
                else lnk_file2.Visible = false;

                if (file3 != "")
                {
                    lnk_file3.Visible = true;
                    lnk_file3.Text = Utility.last_part(file3, "_x_");
                    lnk_file3.Attributes.Add("href", file3);
                    lnk_file3.Attributes.Add("target", "_blank");
                    del_file3.Visible = true;
                    Session["file3"] = file3;
                }
                else lnk_file3.Visible = false;

                if (file4 != "")
                {
                    lnk_file4.Visible = true;
                    lnk_file4.Text = Utility.last_part(file4, "_x_");
                    lnk_file4.Attributes.Add("href", file4);
                    lnk_file4.Attributes.Add("target", "_blank");
                    del_file4.Visible = true;
                    Session["file4"] = file1;
                }
                else lnk_file4.Visible = false;
            }
        }

        protected void btn_publish_Click(object sender, EventArgs e)
        {
            if (lbl_notifid.Text == "")
            {
                return;
            }
            if (txt_header.Text.Trim() == "")
            {
                lbl_pageresult.Text = "Header can't be empty";
                lbl_pageresult.Visible = true;
                return;
            }
            if (txt_message.Text.Trim() == "")
            {
                lbl_pageresult.Text = "Message can't be empty";
                lbl_pageresult.Visible = true;
                return;
            }

            List<string> filelist = new List<string>();
            filelist.Add(Convert.ToString(Session["file1"] as object));
            filelist.Add(Convert.ToString(Session["file2"] as object));
            filelist.Add(Convert.ToString(Session["file3"] as object));
            filelist.Add(Convert.ToString(Session["file4"] as object));


            if (DB_System.update_notification(lbl_notifid.Text,
                                           txt_header.Text, txt_message.Text, filelist,
                                           txt_effective.Text, txt_expires.Text, chk_active.Checked))
            {
                lbl_pageresult.Visible = true;
                lbl_pageresult.Text = "Success : Notification published!";

                lbl_notifid.Text = "";
                btn_publish.Visible = false ;

                Session["file1"] = null;
                Session["file2"] = null;
                Session["file3"] = null;
                Session["file4"] = null;
                txt_effective.Text = "";
                txt_expires.Text = "";
                txt_header.Text = "";
                txt_message.Text = "";
                chk_active.Checked = false;

                lnk_file1.Visible = false;
                lnk_file2.Visible = false;
                lnk_file3.Visible = false;
                lnk_file4.Visible = false;
                del_file1.Visible = false;
                del_file2.Visible = false;
                del_file3.Visible = false;
                del_file4.Visible = false;

                fill_grid();
            }
        }

        protected void del_file_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            if (btn.ID == "del_file1")
            {
                lnk_file1.Text = "";
                Session["file1"] = "";
                lnk_file1.Visible = false;
                del_file1.Visible = false;
            }
            else if (btn.ID == "del_file2")
            {
                lnk_file2.Text = "";
                Session["file2"] = "";
                lnk_file2.Visible = false;
                del_file2.Visible = false;
            }
            else if (btn.ID == "del_file3")
            {
                lnk_file3.Text = "";
                Session["file3"] = "";
                lnk_file3.Visible = false;
                del_file3.Visible = false;
            }
            else if (btn.ID == "del_file4")
            {
                lnk_file4.Text = "";
                Session["file4"] = "";
                lnk_file4.Visible = false;
                del_file4.Visible = false;
            }
        }
    }
}