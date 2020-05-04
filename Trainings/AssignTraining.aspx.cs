using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Trainings
{
    public partial class AssignTraining : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //admin kontrol
                ddl_trainings.DataSource = DB_Trainings.get_TrainingNames();
                ddl_trainings.DataTextField = "NAME";
                ddl_trainings.DataValueField = "ID";
                ddl_trainings.DataBind();

                list_allusers.DataSource = DB_System.get_ALL_Users(with_empty: false);
                list_allusers.DataTextField = "NAME";
                list_allusers.DataValueField = "ID";
                list_allusers.DataBind();

                Session["selected_users"] = null;
            }
        }

        protected void chk_timelimit_CheckedChanged(object sender, EventArgs e)
        {
            panel_times.Visible = chk_timelimit.Checked;
        }

        protected void btn_assign_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt = (DataTable)Session["selected_users"];
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("NAME");
            }

            foreach (int i in list_allusers.GetSelectedIndices())
            {
                DataRow[] d = dt.Select("ID = " + list_allusers.Items[i].Value);
                if (d == null || d.Length == 0)
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = list_allusers.Items[i].Value;
                    row["NAME"] = list_allusers.Items[i].Text;
                    dt.Rows.Add(row);
                }
            }
            Session["selected_users"] = dt;
            list_chosens.DataSource = dt;
            list_chosens.DataTextField = "NAME";
            list_chosens.DataValueField = "ID";
            list_chosens.DataBind();
        }

        protected void btn_unassign_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt = (DataTable)Session["selected_users"];

            foreach (int i in list_chosens.GetSelectedIndices())
            {
                DataRow[] d = dt.Select("ID = " + list_chosens.Items[i].Value);
                if (!(d == null || d.Length == 0))
                    dt.Rows.Remove(d[0]);
            }
            Session["selected_users"] = dt;
            list_chosens.DataSource = dt;
            list_chosens.DataTextField = "NAME";
            list_chosens.DataValueField = "ID";
            list_chosens.DataBind();
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if(ddl_trainings.SelectedValue == "0")
            {
                lbl_pageresult.Text = "Chose a Training";
                lbl_pageresult.Visible = true;
                return;
            }
            DataTable dt = (DataTable)Session["selected_users"];
            if (dt == null || dt.Rows.Count == 0)
            {
                lbl_pageresult.Text = "Chose Trainees to assign";
                lbl_pageresult.Visible = true;
                return;
            }
            if(chk_timelimit.Checked && (txt_finishtime.Text == "" || txt_starttime.Text == ""))
            {
                lbl_pageresult.Text = "Chose Start&Finish Time";
                lbl_pageresult.Visible = true;
                return;
            }


            bool ok = DB_Trainings.Assign_Training(ddl_trainings.SelectedValue, txt_starttime.Text, txt_finishtime.Text, (DataTable)Session["selected_users"]);

            if (ok)
            {
                lbl_pageresult.Text = "SUCCESS : Training is Assigned";
                lbl_pageresult.Visible = true;

                int[] itemsSelected = list_allusers.GetSelectedIndices();
                for (int i = 0; i < itemsSelected.Length; i++)
                    list_chosens.Items.RemoveAt(i);

                ddl_trainings.SelectedValue = "0";

                chk_timelimit.Checked = false;
                txt_finishtime.Text = "";
                txt_starttime.Text = "";
                Session["selected_users"] = null;
            }
        }
    }
}