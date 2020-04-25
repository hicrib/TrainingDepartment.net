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
    public partial class EditRoles : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserSession user = (UserSession)Session["usersession"];

                if (!user.isAdmin)
                    RedirectWithCode("UNAUTHORIZED !");

                //fill users
                ddl_users.DataSource = DB_System.get_ALL_Users();
                ddl_users.DataTextField = "NAME";
                ddl_users.DataValueField = "ID";
                ddl_users.DataBind();

                fill_grid_all_roles();
                fill_grid_user_roles();
            }
        }

        protected void fill_grid_all_roles()
        {
            grid_all_roles.DataSource = DB_System.get_ALL_Roles();
            grid_all_roles.DataBind();
        }

        protected void fill_grid_user_roles(string employeeid = "")
        {     
            DataTable dt = DB_System.get_ALL_Privileges_of_Person(employeeid);

            if (dt == null || dt.Rows.Count == 0)
            {
                dt = new DataTable();
                dt.Columns.Add("ROLEID");
                dt.Columns.Add("ROLENAME");
                dt.Columns.Add("ROLEEXPLANATION");
            }

            grid_user_roles.DataSource = dt;
            grid_user_roles.DataBind();
            
        }

        protected void ddl_users_SelectedIndexChanged(object sender, EventArgs e)
        {
            fill_grid_user_roles(ddl_users.SelectedValue);
        }

        protected void grid_user_roles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // COMMAND  :REMOVE
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_user_roles.Rows[index];

            string roleid = selectedRow.Cells[1].Text.Trim(); //  ID  of role

            bool removed = DB_System.remove_role_from_user(ddl_users.SelectedValue, roleid);
            if (removed)
            {
                grid_user_roles.DeleteRow(index);
                grid_user_roles.DataBind();
                fill_grid_user_roles(ddl_users.SelectedValue);
            }
               //
        }

        protected void grid_all_roles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //ADD
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_all_roles.Rows[index];

            string role_id = selectedRow.Cells[1].Text.Trim(); // ID of role to add

            bool added = DB_System.add_role_to_user(ddl_users.SelectedValue, role_id);

            if(added)
                fill_grid_user_roles(ddl_users.SelectedValue);
        }

        protected void grid_user_roles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void grid_user_roles_DeleteItem(int id)
        {

        }

        protected void grid_user_roles_RowDeleting1(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}