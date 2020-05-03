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
    public partial class ViewTrainings : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("VIEW TRAININGS");

                fill_grid_trainings();
            }
        }

        protected void fill_grid_trainings()
        {
            grid_trainings.DataSource = DB_Trainings.get_training_Designs();
            grid_trainings.DataBind();
        }

        protected void grid_trainings_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName == "VIEWTRAINING")
            {
                //go to details in a new tab
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = grid_trainings.Rows[index];


                string trnid = selectedRow.Cells[10].Text.Trim(); //TRNID
                string laststepid = selectedRow.Cells[11].Text.Trim(); //LASTSTEPID
                string firststepid = selectedRow.Cells[12].Text.Trim(); //FIRSTSTEPID

                string url = "";
                if (selectedRow.Cells[3].Text.Trim() == "DESIGN_FINISHED")
                {
                    //SEND PASSIVE - FROM THE FIRST DESIGN STEP
                    Session["editable"] = false;
                    url = "CreateTrainingDesignPage.aspx?T=" + trnid + "&S=" + firststepid;
                }
                else
                {
                    Session["editable"] = true;
                    url = "CreateTrainingDesignPage.aspx?T=" + trnid + "&S=" + laststepid;
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "open('" + url + "') ;", true);
            }
            else if (e.CommandName == "INACTIVE")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = grid_trainings.Rows[index];

                string trnid = selectedRow.Cells[10].Text.Trim();
                bool active = selectedRow.Cells[2].Text.Trim() == "Yes";

                if (active)
                    DB_Trainings.update_TRAINING_DEF(trnid, isactive: "0");
                else
                    DB_Trainings.update_TRAINING_DEF(trnid, isactive: "1");


                selectedRow.Cells[2].Text = active ? "No" : "Yes"; 
            }
        }



        protected void grid_trainings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[10].Visible = false; // hide TRNID
            e.Row.Cells[11].Visible = false; // hide LASTSTEPID
            e.Row.Cells[12].Visible = false; // hide FIRSTSTEPID
        }



        protected void grid_trainings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid_trainings.PageIndex = e.NewPageIndex;
            fill_grid_trainings();
        }

        protected void grid_trainings_Sorting(object sender, GridViewSortEventArgs e)
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
            DataView sortedView = new DataView(DB_Trainings.get_training_Designs());
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            Session["SortedView"] = sortedView;
            grid_trainings.DataSource = sortedView;
            grid_trainings.DataBind();
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

        protected void btn_hidden_refresh_Click(object sender, EventArgs e)
        {

        }
    }
}