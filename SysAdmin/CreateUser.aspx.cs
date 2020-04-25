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
    public partial class CreateUser1 : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserSession user = (UserSession)Session["usersession"];

                if (!user.isAdmin)
                    RedirectWithCode("UNAUTHORIZED !");


                DataTable roles = DB_System.get_ALL_Roles();
                if (roles != null && roles.Rows.Count != 0)
                {
                    ddl_role1.DataSource = roles;
                    ddl_role1.DataTextField = "NAME";
                    ddl_role1.DataValueField = "ID";
                    ddl_role1.DataBind();

                    ddl_role2.DataSource = roles;
                    ddl_role2.DataTextField = "NAME";
                    ddl_role2.DataValueField = "ID";
                    ddl_role2.DataBind();

                    ddl_role3.DataSource = roles;
                    ddl_role3.DataTextField = "NAME";
                    ddl_role3.DataValueField = "ID";
                    ddl_role3.DataBind();
                }
            }
        }

        protected void btn_adduser_Click(object sender, EventArgs e)
        {
            //check page elements
            if (txt_employeeid.Text == "" || txt_firstName.Text == "" || txt_surName.Text == "" || txt_password.Text == "" || txt_initial.Text == "")
            {
                lbl_result_adduser.Text = "Error : Fill Mandatory Fields";
                return;
            }

            if (txt_initial.Text.Length > 3)
            {
                lbl_result_adduser.Text = "Initials cant be more than 3 characters";
                return;
            }

            Dictionary<string, string> userinfo = new Dictionary<string, string>()
            {
                { "employeeid", txt_employeeid.Text },
                { "firstname", txt_firstName.Text  },
                { "surname",  txt_surName.Text },
                { "password",  txt_password.Text },
                    {"initial", txt_initial.Text.ToUpper() }
             };

            List<string> roles = new List<string>();

            roles.Add("4"); //it means EVERYBODY ROLE
            roles.Add("2"); // Everybody is also a TRAINEE because you can't create sysadmin

            if (ddl_role1.SelectedValue != "0" && ddl_role1.SelectedValue != "2" && ddl_role1.SelectedValue != "4")
                roles.Add(ddl_role1.SelectedValue);

            if (ddl_role2.Visible && ddl_role2.SelectedValue != "0" && !roles.Contains(ddl_role2.SelectedValue))
            {
                if (ddl_role2.SelectedValue != "2" && ddl_role2.SelectedValue != "4")
                    roles.Add(ddl_role2.SelectedValue);
            }


            if (ddl_role3.Visible && ddl_role3.SelectedValue != "0" && !roles.Contains(ddl_role3.SelectedValue))
            {
                if (ddl_role3.SelectedValue != "2" && ddl_role3.SelectedValue != "4")
                    roles.Add(ddl_role3.SelectedValue);
            }


            if (roles.Count < 2) //because everybody has EVERYBODY ROLE and TRAINEE ROLE
            {
                lbl_result_adduser.Text = "Error : User needs at least 1 ROLE";
                return;
            }



            bool created = DB_System.createUser_withRoles(userinfo, roles);

            if (created)
                lbl_result_adduser.Text = "User Added with Roles";
            else
                lbl_result_adduser.Text = "EmployeeID and Initial must be unique";
        }

        protected void ddl_role1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DropDownList)sender).ID == "ddl_role1")
            {
                if (ddl_role1.SelectedValue == "0")
                {
                    ddl_role2.Visible = false;
                    ddl_role3.Visible = false;
                    return;
                }

                ddl_role2.Visible = true;
                return;
            }

            if (((DropDownList)sender).ID == "ddl_role2")
            {
                if (ddl_role2.SelectedValue == "0")
                {
                    ddl_role3.Visible = false;
                    return;
                }

                ddl_role3.Visible = true;
                return;
            }
        }

    }
}