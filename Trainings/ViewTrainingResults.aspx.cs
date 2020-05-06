using AviaTrain.App_Code;
using PdfSharp.Pdf.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;





namespace AviaTrain.Trainings
{
    public partial class ViewTrainingResults : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Write_Page_Header_Low("USER TRAINING RESULTS");

                Fill_Training_Names();
                Fill_Trainees();
            }
        }
        protected void Fill_Training_Names()
        {
            filter_training.DataSource = DB_Trainings.get_TrainingNames(chk_active_training.Checked);
            filter_training.DataTextField = "NAME";
            filter_training.DataValueField = "ID";
            filter_training.DataBind();

        }
        protected void Fill_Trainees()
        {
            filter_trainee.DataSource = DB_Exams.get_EXAM_TRAINEES(chk_active_trainee.Checked);
            filter_trainee.DataTextField = "NAME";
            filter_trainee.DataValueField = "ID";
            filter_trainee.DataBind();
        }
        protected void chk_active_training_CheckedChanged(object sender, EventArgs e)
        {
            Fill_Training_Names();
        }

        protected void chk_active_trainee_CheckedChanged(object sender, EventArgs e)
        {
            Fill_Trainees();
        }

        protected DataTable return_Filter_Result()
        {
            string training = "0";
            if (!(new string[2] { "-", "" }).Contains(filter_training.SelectedValue))
                training = filter_training.SelectedValue;

            string trainee = "0";
            if (!(new string[2] { "-", "" }).Contains(filter_trainee.SelectedValue))
                trainee = filter_trainee.SelectedValue;

            string status = filter_status.SelectedValue == "ALL" ? "" : filter_status.SelectedValue;

            string start_date = "2000-01-01 00:00:01";
            //yyyy-mm-dd
            if (filter_start.Text != "")
                start_date = filter_start.Text + " 00:00:01";


            string finish_date = "2050-12-31 23:59:59";
            //yyyy-mm-dd
            if (filter_finish.Text != "")
                finish_date = filter_finish.Text + " 23:59:59";

            return DB_Trainings.View_Training_Results(start_date, finish_date,
                                                               training, chk_active_training.Checked,
                                                               trainee, chk_active_trainee.Checked,
                                                               status);
        }
        protected void btn_search_Click(object sender, EventArgs e)
        {
            DataTable dt = return_Filter_Result();


            if (dt != null && dt.Rows.Count > 0)
            {
                grid_results.DataSource = dt;
                grid_results.DataBind();
                grid_results.Visible = true;
                lbl_result.Visible = false;

                //for excel export
                Session["excel_file"] = dt;
                Session["excel_file_excludeColumns"] = new Dictionary<string,string> { { "ID", "7" } , { "ASSIGNID", "8" } };
            }
            else
            {
                lbl_result.Text = "No Result Found";
                lbl_result.Visible = true;
                grid_results.Visible = false;
            }

        }

        protected void grid_results_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 8)
            {
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
            }
        }

        protected void grid_results_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void grid_results_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid_results.PageIndex = e.NewPageIndex;
            grid_results.DataSource = return_Filter_Result();
            grid_results.DataBind();
        }

        protected void grid_results_Sorting(object sender, GridViewSortEventArgs e)
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

            DataView sortedView = new DataView(return_Filter_Result());
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            Session["SortedView"] = sortedView;
            grid_results.DataSource = sortedView;
            grid_results.DataBind();

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

        protected void btn_export_Click(object sender, EventArgs e)
        {
        }
        
    }
}