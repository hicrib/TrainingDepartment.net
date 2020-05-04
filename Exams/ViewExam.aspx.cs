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
    public partial class ViewExam : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            string examid = Convert.ToString(Request.QueryString["ExID"]);
            if (String.IsNullOrWhiteSpace(examid))
                RedirectWithCode("UNAUTHORIZED!");

            Fill_Details(examid);
        }


        protected void Fill_Details(string examid)
        {
            string result_html = "";
            string header_html = make_exam_header(DB_Exams.get_Exam_Name(examid: examid));
            result_html += header_html;


            // Q.ID, QUESTION, ANSWER , POINT
            DataTable exam_questions = DB_Exams.get_EXAM_QUESTIONS_by_examid(examid);

            foreach (DataRow q in exam_questions.Rows)
            {
                string q_html = "";


                //@TYPE as [TYPE] , Q , OPA , OPB, ISNULL(OPC,'') as [OPC], ISNULL(OPD,'') AS [OPD], ANSWER  
                //FILL : SAME AS DB
                DataRow q_definition = DB_Exams.get_ONE_question(q["Q.ID"].ToString()).Rows[0];

                //q_id, type, point, real_ans1_acc1 .... ,  user_ans1 ...


                if (q_definition["TYPE"].ToString() == "2" || q_definition["TYPE"].ToString() == "3" || q_definition["TYPE"].ToString() == "4")
                {
                    q_html = make_ops(q_definition, Convert.ToInt32(q_definition["TYPE"]), q);
                }
                else if (q_definition["TYPE"].ToString() == "FILL")
                {
                    q_html = make_FILL(q_definition, q);
                }

                result_html += q_html;
            }


            div_result_html.InnerHtml = result_html;

        }

        protected string make_ops(DataRow q_def, int opsnumber, DataRow q)
        {
            //@TYPE as [TYPE] , Q , OPA , OPB, ISNULL(OPC,'') as [OPC], ISNULL(OPD,'') AS [OPD], ANSWER 

            string real_ans1_acc1 = q_def["ANSWER"].ToString();


            string q_main = @"<table style='' class='ops_q_table' >
        <tr>
            <td colspan='3'>
              <span class='ops_q_head_span' > Q " + q["ORDERBY"].ToString() + @"  (" + q["POINT"].ToString() + @" pts.) </span>
            </td>
        </tr>
        <tr>
            <td colspan='3' style=''> <span class='ops_q_span' >
               " + q_def["Q"].ToString() + @"  </span>
            </td>
        </tr>
        <tr>
            <td colspan='3' class='ops_empty_row'>

            </td>
        </tr>       ";
            //</ table >"; SHOULD BE ADDED


            for (int i = 1; i <= opsnumber; i++)
            {
                string ops_tr_html = @"<tr>
            <td class='ops_image_td' >
               @SRC@
            </td>
            <td class='ops_ABCD_td'>
                @@ABCD@@
            </td>
            <td class='ops_ops_td'>
                <span> @@OPS@@</span>
            </td>
        </tr>";
                if (i == 1)
                {
                    if (real_ans1_acc1 == "A")
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                    else
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "");


                    ops_tr_html = ops_tr_html.Replace(" @@ABCD@@", "A -)");
                    ops_tr_html = ops_tr_html.Replace(" @@OPS@@", q_def["OPA"].ToString());
                }
                else if (i == 2)
                {
                    if (real_ans1_acc1 == "B")
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                    else
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "");

                    ops_tr_html = ops_tr_html.Replace(" @@ABCD@@", "B -)");
                    ops_tr_html = ops_tr_html.Replace(" @@OPS@@", q_def["OPB"].ToString());
                }
                else if (i == 3)
                {
                    if (real_ans1_acc1 == "C")
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                    else
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "");

                    ops_tr_html = ops_tr_html.Replace(" @@ABCD@@", "C -)");
                    ops_tr_html = ops_tr_html.Replace(" @@OPS@@", q_def["OPC"].ToString());
                }
                else if (i == 4)
                {
                    if (real_ans1_acc1 == "D")
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                    else
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "");

                    ops_tr_html = ops_tr_html.Replace(" @@ABCD@@", "D -)");
                    ops_tr_html = ops_tr_html.Replace(" @@OPS@@", q_def["OPD"].ToString());
                }

                q_main += ops_tr_html;
            }
            q_main += "</ table >";


            return q_main;
        }

        protected string make_FILL(DataRow q_def, DataRow q)
        {

            string q_html = @"<table class='fill_q_table'>
        <tr>
            <td class='fill_q_head_td' >
                Q " + q["ORDERBY"].ToString() + @"  (" + q["POINT"].ToString() + @" pts.)
            </td>
        </tr>
        <tr><td>";

            //text1
            q_html += "<span class='fill_text_span'> " + q_def["TEXT1"].ToString() + " </span>";

            //blank 1
            q_html += "<input type = 'text' id='blank1' value = '' disabled> ";

            q_html += @"<select id='ans_blank1'>";
            if (q_def["FILL1_ANS1"].ToString() != "")
                q_html += @" < option value = '" + q_def["FILL1_ANS1"].ToString() + @"' > " + q_def["FILL1_ANS1"].ToString() +@" </ option >";
            if (q_def["FILL1_ANS2"].ToString() != "")                                                                        
                q_html += @" < option value = '" + q_def["FILL1_ANS2"].ToString() + @"' >" + q_def["FILL1_ANS2"].ToString() + @" </ option >";
            if (q_def["FILL1_ANS3"].ToString() != "")                                                                        
                q_html += @" < option value = '" + q_def["FILL1_ANS3"].ToString() + @"' >" + q_def["FILL1_ANS3"].ToString() + @" </ option >";
            q_html += "</ select > ";


            //if there is text 2
            if (q_def["TEXT2"].ToString() != "")
                q_html += "<span class='fill_text_span'> " + q_def["TEXT2"].ToString() + " </span>";

            //if there is blank 2
            if (q_def["FILL2_ANS1"].ToString() + q_def["FILL2_ANS2"].ToString() + q_def["FILL2_ANS3"].ToString() != "")
            {
                q_html += @"<input type = 'text' id='blank2' value = '' disabled> ";
                q_html += @"<select id='ans_blank2'>";
                if (q_def["FILL2_ANS1"].ToString() != "")
                    q_html += @" < option value = '" + q_def["FILL2_ANS1"].ToString() + @"' >" + q_def["FILL2_ANS1"].ToString() + " @</ option >";
                if (q_def["FILL2_ANS2"].ToString() != "")
                    q_html += @" < option value = '" + q_def["FILL2_ANS2"].ToString() + @"' > " + q_def["FILL2_ANS2"].ToString() + @"</ option >";
                if (q_def["FILL2_ANS3"].ToString() != "")
                    q_html += @" < option value = '" + q_def["FILL2_ANS3"].ToString() + @"' >" + q_def["FILL2_ANS3"].ToString() + @" </ option >";
                q_html += "</ select > ";
            }


            //if there is text 3
            if (q_def["TEXT3"].ToString() != "")
                q_html += "<span class='fill_text_span'> " + q_def["TEXT3"].ToString() + " </span>";

            //if there is blank 3
            if (q_def["FILL3_ANS1"].ToString() + q_def["FILL3_ANS2"].ToString() + q_def["FILL3_ANS3"].ToString() != "")
            {
                q_html += "<input type = 'text' id='blank3' value = '' disabled> ";
                q_html += @"<select id='ans_blank3'>";
                if (q_def["FILL3_ANS1"].ToString() != "")
                    q_html += @" < option value = '" + q_def["FILL3_ANS1"].ToString() + @"' >" + q_def["FILL3_ANS1"].ToString() + @"</ option >";
                if (q_def["FILL3_ANS2"].ToString() != "")
                    q_html += @" < option value = '" + q_def["FILL3_ANS2"].ToString() + @"' > " + q_def["FILL3_ANS2"].ToString() + @"</ option >";
                if (q_def["FILL3_ANS3"].ToString() != "")
                    q_html += @" < option value = '" + q_def["FILL3_ANS3"].ToString() + @"' >" + q_def["FILL3_ANS3"].ToString() + @"</ option >";
                q_html += "</ select > ";
            }

            //if there is text 4
            if (q_def["TEXT4"].ToString() != "")
                q_html += "<span class='fill_text_span'> " + q_def["TEXT4"].ToString() + " </span>";


            q_html += @"</td></tr>
    </table>";


            return q_html;
        }

        protected string make_exam_header(string examname)
        {


            //make html
            string html = @"<table class='exam_header'>
        <tr>
            <th colspan='3'>" + examname + @"
            </th>
        </tr>
    </table>";

            //return
            return html;

        }


    }
}