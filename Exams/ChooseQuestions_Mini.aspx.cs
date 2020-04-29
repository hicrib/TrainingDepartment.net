using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Exams
{
    public partial class ChooseQuestions_Mini : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                DataTable dt = (DataTable)Session["chosen_questions_training"];
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("ID");
                    dt.Columns.Add("SECTOR");
                    dt.Columns.Add("QUESTION");
                    dt.Columns.Add("Answer");
                }

                Fill_Grid_AllQuestions();
                Fill_grid_Chosen_Questions();
            }
            else
            {
            }
        }


        protected DataTable Fill_Grid_AllQuestions(string sector = "GEN")
        {
            DataTable dt = DB_Exams.get_ALL_questions_sector_withAnswer(sector);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("SECTOR");
                dt.Columns.Add("QUESTION");
                dt.Columns.Add("Answer");
            }

            grid_all_questions.DataSource = dt;
            grid_all_questions.DataBind();

            //binds but also returns table
            return dt;
        }

        protected void grid_all_questions_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grid_all_questions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_all_questions.Rows[index];

            if (e.CommandName == "ADD")
            {
                selectedRow.BackColor = System.Drawing.Color.LightGreen;

                DataTable dt = (DataTable)Session["chosen_questions_training"];

                //add if ALREADY not exist
                DataRow[] result = dt.Select("ID = '" + selectedRow.Cells[2].Text.Trim() + "'");
                if (result == null || result.Length == 0)
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = selectedRow.Cells[2].Text.Trim();
                    row["SECTOR"] = selectedRow.Cells[3].Text.Trim();
                    row["QUESTION"] = selectedRow.Cells[4].Text.Trim();
                    row["Answer"] = selectedRow.Cells[5].Text.Trim();
                    dt.Rows.Add(row);
                    Session["chosen_questions_training"] = dt;
                }
            }
            else if (e.CommandName == "REMOVE")
            {
                selectedRow.BackColor = System.Drawing.Color.Empty;

                DataTable dt = (DataTable)Session["chosen_questions_training"];

                //remove that row containing selected id
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] result = dt.Select("ID = '" + selectedRow.Cells[2].Text.Trim() + "'");
                    foreach (DataRow row in result)
                    {
                        dt.Rows.Remove(row);
                    }
                    Session["chosen_questions_training"] = dt;
                }
            }

            //update chosen
            Fill_grid_Chosen_Questions();
        }

        protected void grid_all_questions_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grid_all_questions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Gridview'in PageIndexChanging Eventında grid'in PageIndex'ine seçilen
            // sayfanın numarası atanır.
            grid_all_questions.PageIndex = e.NewPageIndex;

            // Tekrar kayıtların gridview'e aktarılması sağlanır.
            grid_all_questions.DataSource = DB_Exams.get_ALL_questions_sector_withAnswer(ddl_sector.SelectedValue);
            grid_all_questions.DataBind();
        }


        protected void ddl_sector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Grid_AllQuestions(ddl_sector.SelectedValue);
        }

        protected void btn_check_questions_Click(object sender, EventArgs e)
        {

        }

        protected void Fill_grid_Chosen_Questions()
        {
            DataTable dt = (DataTable)Session["chosen_questions_training"];
            if (dt != null && dt.Rows.Count > 0)
            {
                grid_chosens.DataSource = dt;
                grid_chosens.DataBind();
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("SECTOR");
                dt.Columns.Add("QUESTION");
                dt.Columns.Add("Answer");

                Session["chosen_questions_training"] = dt;

                grid_chosens.DataSource = dt;
                grid_chosens.DataBind();
            }
        }

        protected void grid_chosens_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = grid_chosens.Rows[index];

            //only REMOVE command 

            DataTable dt = (DataTable)Session["chosen_questions_training"];

            //remove that row containing selected id
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] result = dt.Select("ID = '" + selectedRow.Cells[1].Text.Trim() + "'");
                foreach (DataRow row in result)
                {
                    dt.Rows.Remove(row);
                }


                Session["chosen_questions_training"] = dt;
                Fill_grid_Chosen_Questions();
            }
        }

    }
}