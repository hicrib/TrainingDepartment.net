using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AviaTrain.App_Code;
using System.Data;

namespace AviaTrain.Reports
{
    public partial class LevelObjectives : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserSession user = (UserSession)Session["usersession"];
                if (user.isOnlyTrainee)
                    RedirectWithCode("UNAUTHORIZED");



                Write_Page_Header_Low("LEVEL OBJECTIVES");

                ddl_trainee.DataSource = DB_System.get_ALL_trainees();
                ddl_trainee.DataTextField = "NAME";
                ddl_trainee.DataValueField = "ID";
                ddl_trainee.DataBind();

            }
        }

        protected void ddl_trainee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_trainee.SelectedValue == "0")
            {
                pnl_grid.Visible = false;
                ddl_position.Visible = false;
                return;
            }


            DataTable steps = DB_Reports.get_ONGOING_steps(ddl_trainee.SelectedValue);

            if (steps == null || steps.Rows.Count == 0)
            {
                lbl_pageresult.Visible = true;
                lbl_pageresult.Text = "User has no ONGOING training";

                ddl_position.Visible = false;
                pnl_grid.Visible = false;
                return;
            }


            if (steps.Rows.Count > 1)
            {
                lbl_pageresult.Visible = true;
                lbl_pageresult.Text = "User has MULTIPLE ONGOING training, Choose Sector";
                pnl_grid.Visible = false;
                ddl_position.Visible = true;

                ddl_position.Items.Clear();
                ddl_position.Items.Add("-");

                foreach (DataRow row in steps.Rows)
                {
                    if (row["SECTOR"].ToString() == "")
                        ddl_position.Items.Add(new ListItem("TWR - " + row["PHASE"].ToString(), row["PHASE"].ToString()));
                    else
                        ddl_position.Items.Add(new ListItem(
                                                row["POSITION"].ToString() + "-" + row["SECTOR"].ToString(), row["SECTOR"].ToString()
                                                            ));
                }
                return;
            }

            //one ongoing step
            Fill_Grid(steps.Rows[0]);
        }

        protected void ddl_position_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_position.SelectedValue == "-")
            {
                pnl_grid.Visible = false;
                return;
            }

            DataTable dt = DB_Reports.get_ONGOING_step(ddl_trainee.SelectedValue, ddl_position.SelectedValue);
            if (dt != null && dt.Rows.Count == 1)
                Fill_Grid(dt.Rows[0]);
        }


        public void publishError(DataRow row)
        {
            string errormessage = "User's current level doesn't have defined Level Objectives : ";
            lbl_pageresult.Text = errormessage + row["POSITION"].ToString() + " - " + row["SECTOR"].ToString() + "  : " + row["PHASE"].ToString();
            lbl_pageresult.Visible = true;
            pnl_grid.Visible = false;
            return;
        }
        protected void Fill_Grid(DataRow row)
        {


            //there are some levels without objectives
            if ( (new string[2] { "ACC", "APP" }).Contains( row["POSITION"].ToString())
                 && (  (row["PHASE"].ToString() == "ASSIST" && row["SECTOR"].ToString() == "")
                       ||
                       (row["PHASE"].ToString() == "OJT_ASSIST" )
                    )
               )
            {
                publishError(row);
                return;
            }

            if (row["PHASE"].ToString() == "PRELEVEL1" || (row["PHASE"].ToString() == "OJT_LEVEL3PLUS"))
            {
                publishError(row);
                return;
            }

            lbl_tableheader.Text = row["POSITION"].ToString() + " - " + row["SECTOR"].ToString() + "  : " + row["PHASE"].ToString() + " Level Objectives";
            lbl_tableheader.Visible = true;

            //just one ongoing step
            lbl_pageresult.Visible = false;
            lbl_pageresult.Text = "";
            pnl_grid.Visible = true;

            DataTable dt = DB_Reports.get_Level_Objectives(ddl_trainee.SelectedValue, row["SECTOR"].ToString(), row["PHASE"].ToString());
            DataTable copy = dt.Copy();

            string categ = "";
            int insertcount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (categ != dt.Rows[i]["CATEGORY"].ToString())
                {
                    DataRow ro = copy.NewRow();
                    ro["ID"] = "0";
                    ro["CATEGORY"] = dt.Rows[i]["CATEGORY"].ToString();
                    copy.Rows.InsertAt(ro, i + insertcount);
                    insertcount++;
                    categ = dt.Rows[i]["CATEGORY"].ToString();
                }
            }

            grid_objectives.DataSource = copy;
            grid_objectives.DataBind();
        }

        protected void grid_objectives_PreRender(object sender, EventArgs e)
        {
            //merge cells with below
            for (int rowIndex = grid_objectives.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = grid_objectives.Rows[rowIndex];
                GridViewRow previousRow = grid_objectives.Rows[rowIndex + 1];

                for (int i = 0; i < row.Cells.Count - 4; i++) //-2 because dont-touch last cells
                {
                    if (row.Cells[i].Text == previousRow.Cells[i].Text && row.Cells[0].Text != "0")
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        row.Cells[i].CssClass = "mergedCell";
                        previousRow.Cells[i].Visible = false;
                    }
                }
            }

            //make headers for category
            for (int i = 0; i < grid_objectives.Rows.Count; i++)
            {
                GridViewRow row = grid_objectives.Rows[i];
                row.Cells[0].Visible = false; //OBJECTIVEID
                row.Cells[1].Visible = false; //CATEGORY
                row.Cells[5].Visible = false; //FORMID

                if ("0" == row.Cells[0].Text) //HEADER ROW
                {
                    row.Cells[2].Text = row.Cells[1].Text; // Move Category to a visible cell
                    row.Cells[2].ColumnSpan = 4;
                    row.Cells[2].CssClass = "CATEGORYROW";
                    row.Cells[3].Visible = false;
                    row.Cells[4].Visible = false;
                    row.Cells[5].Visible = false;
                    row.Cells[6].Visible = false;
                }
                else //normal row
                {
                    row.Cells[4].CssClass = "byCell";
                    //if (row.Cells[4].Text != "&nbsp;") //if already signed, dont show button
                    //    row.Cells[5].Style.Add("opacity", "0");
                }
            }
        }

        protected void grid_objectives_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SIGN")
            {
                GridViewRow selectedRow = ((GridViewRow)(((ImageButton)e.CommandSource).NamingContainer));

                string arg = (string)e.CommandArgument;

                string by = selectedRow.Cells[4].Text;
                string objectiveid = arg.Split(',')[1];
                string formid = arg.Split(',')[2];

                UserSession user = (UserSession)Session["usersession"];
                if (by == user.initial)
                {
                    //unsign
                    if (DB_Reports.sign_unsign_objective(ddl_trainee.SelectedValue, formid, objectiveid, sign: "0"))
                    {
                        selectedRow.Cells[4].Text = "";
                        if (selectedRow.Cells[6].HasControls())
                        {
                            ImageButton btn = (ImageButton)selectedRow.Cells[6].FindControl("btn_sign");
                            if (btn != null)
                                btn.ImageUrl = "~/images/sign.png";
                        }
                    }
                }
                else
                {
                    //sign
                    if (DB_Reports.sign_unsign_objective(ddl_trainee.SelectedValue, formid, objectiveid, sign: "1"))
                    {
                        selectedRow.Cells[4].Text = ((UserSession)Session["usersession"]).initial;
                        if (selectedRow.Cells[6].HasControls())
                        {
                            ImageButton btn = (ImageButton)selectedRow.Cells[6].FindControl("btn_sign");
                            if (btn != null)
                                btn.ImageUrl = "~/images/delete.png";
                        }

                    }
                }

            }
        }

        protected bool show_sign(string by)
        {
            if (by == "")
                return true;

            UserSession user = (UserSession)Session["usersession"];
            return by == user.initial;
        }

        protected string show_url(string by)
        {
            if (by == "")
                return "~/images/sign.png";

            return "~/images/delete.png";
        }
    }
}