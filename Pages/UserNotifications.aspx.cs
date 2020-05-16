using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Pages
{
    public partial class UserNotifications : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("MY NOTIFICATIONS");

                Fill_Grid_Notifs();
            }
        }

        protected void Fill_Grid_Notifs()
        {
            UserSession user = (UserSession)Session["usersession"];
            grid_notif.DataSource = DB_System.get_user_notifications(user.employeeid);
            grid_notif.DataBind();
        }

        protected void grid_notif_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VIEWNOTIF")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = grid_notif.Rows[index];

                selectedRow.BackColor = System.Drawing.Color.White;

                string notifid = selectedRow.Cells[3].Text;
                //view notif 
                DataRow notification = DB_System.get_notification(notifid);
                if (notification == null)
                    return;

                pnl_show_notification.Visible = true;
                lbl_header.Text = notification["HEADER"].ToString();
                lbl_message.Text = notification["TEXT"].ToString();

                string file1 = notification["FILE1"].ToString();
                string file2 = notification["FILE2"].ToString();
                string file3 = notification["FILE3"].ToString();
                string file4 = notification["FILE4"].ToString();

                if (file1 != "")
                {
                    lnk_file1.Visible = true;
                    lnk_file1.Text = Utility.last_part(file1, '_');
                    lnk_file1.Attributes.Add("href", file1);
                    lnk_file1.Attributes.Add("target", "_blank");
                }
                else lnk_file1.Visible = false;

                if (file2 != "")
                {
                    lnk_file2.Visible = true;
                    lnk_file2.Text = Utility.last_part(file2, '_');
                    lnk_file2.Attributes.Add("href", file2);
                    lnk_file2.Attributes.Add("target", "_blank");
                }
                else lnk_file2.Visible = false;

                if (file3 != "")
                {
                    lnk_file3.Visible = true;
                    lnk_file3.Text = Utility.last_part(file3, '_');
                    lnk_file3.Attributes.Add("href", file3);
                    lnk_file3.Attributes.Add("target", "_blank");
                }
                else lnk_file3.Visible = false;

                if (file4 != "")
                {
                    lnk_file4.Visible = true;
                    lnk_file4.Text = Utility.last_part(file4, '_');
                    lnk_file4.Attributes.Add("href", file4);
                    lnk_file4.Attributes.Add("target", "_blank");
                }
                else lnk_file4.Visible = false;

                //mark as seen
                if (selectedRow.Cells[2].Text == "-")
                {
                    bool ok = DB_System.update_user_notification(notifid, ((UserSession)Session["usersession"]).employeeid);
                    selectedRow.Cells[2].Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm");
                    UserSession user = (UserSession)Session["usersession"];
                    user.notif--;
                }
            }
        }















        protected void grid_notif_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 2)
            {
                e.Row.Cells[3].Visible = false; //ID of notification
                if (e.Row.Cells[2].Text == "-")
                    e.Row.Style.Add("font-weight", "bold");
            }

        }

        protected void grid_notif_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grid_notif_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            UserSession user = (UserSession)Session["usersession"];

            grid_notif.PageIndex = e.NewPageIndex;
            grid_notif.DataSource = DB_System.get_user_notifications(user.employeeid);
            grid_notif.DataBind();
        }

        protected void grid_notif_Sorting(object sender, GridViewSortEventArgs e)
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

            UserSession user = (UserSession)Session["usersession"];
            DataView sortedView = new DataView(DB_System.get_user_notifications(user.employeeid));
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            Session["SortedView"] = sortedView;
            grid_notif.DataSource = sortedView;
            grid_notif.DataBind();
        }

        public SortDirection direction
        {
            get
            {
                if (ViewState["directionState"] == null)
                {
                    ViewState["directionState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["directionState"];
            }
            set
            {
                ViewState["directionState"] = value;
            }
        }


    }
}