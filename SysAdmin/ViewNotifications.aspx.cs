using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.SysAdmin
{
    public partial class ViewNotifications : MasterPage
    {
        protected  new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddl_users.DataSource = DB_System.get_ALL_Users(with_empty: true, isactive: false);
                ddl_users.DataTextField = "NAME";
                ddl_users.DataValueField = "ID";
                ddl_users.DataBind();

                ddl_notifications.DataSource = DB_System.get_notification(isactive: false);
                ddl_notifications.DataTextField = "HEADER";
                ddl_notifications.DataValueField = "ID";
                ddl_notifications.DataBind();

                ddl_notifications.Items.Insert(0, new ListItem("-", "0"));
            }
        }

        protected void btn_find_Click(object sender, EventArgs e)
        {
            grid_results.DataSource = DB_System.get_usernotification_views(ddl_notifications.SelectedValue, ddl_users.SelectedValue);
            grid_results.DataBind();
        }
    }
}