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
    public partial class ViewUserExamDetails : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "TrainingDepartment.Net User Exam Results";
            if (!IsPostBack)
            {
                string assignid = Convert.ToString(Request.QueryString["AsID"]);
                if (String.IsNullOrWhiteSpace(assignid))
                    RedirectWithCode("UNAUTHORIZED!");

                //if admin, examadmin, any trainee
                UserSession user = (UserSession)Session["usersession"];
                if (!(user.employeeid == DB_Exams.whose_Assignment(assignid) || user.isAdmin || user.isExamAdmin))
                    RedirectWithCode("UNAUTHORIZED!");


                Fill_Details(assignid);
            }

        }

        protected void Fill_Details(string assignid)
        {
            string result_html = "";

            //get_assignment information and results
            string header_html = make_exam_header(assignid);
            result_html += header_html;


            // EDQ.ORDERBY as 'Question' , EDQ.Q_ID , '0' as SOLVED
            DataTable exam_questions = DB_Exams.get_EXAM_QUESTIONS_by_assignid(assignid);

            DataTable user_answers = DB_Exams.get_ALL_USER_ANSWERS_of_ASSIGNMENT(assignid);



            foreach (DataRow q in exam_questions.Rows)
            {
                string q_html = "";

                //@TYPE as [TYPE] , Q , OPA , OPB, ISNULL(OPC,'') as [OPC], ISNULL(OPD,'') AS [OPD], ANSWER  
                //FILL : SAME AS DB
                DataRow q_definition = DB_Exams.get_ONE_question(q["Q_ID"].ToString()).Rows[0];

                //q_id, type, point, real_ans1_acc1 .... ,  user_ans1 ...

                DataRow answer = null;
                //this check is if the trainee didnt answer question
                DataRow[] temp = user_answers.Select("Q_ID = " + q["Q_ID"]);
                if (temp != null && temp.Length > 0) //not answered
                    answer = user_answers.Select("Q_ID = " + q["Q_ID"])[0];


                if (q_definition["TYPE"].ToString() == "2" || q_definition["TYPE"].ToString() == "3" || q_definition["TYPE"].ToString() == "4")
                {
                    q_html = make_ops(q["Question"].ToString(), q_definition, answer, Convert.ToInt32(q_definition["TYPE"]));
                }
                else if (q_definition["TYPE"].ToString() == "FILL")
                {
                    q_html = make_FILL(q["Question"].ToString(), q_definition, answer);
                }



                result_html += q_html;
            }


            div_result_html.InnerHtml = result_html;

        }

        protected string make_ops(string orderby, DataRow q_def, DataRow answer, int opsnumber)
        {
            string user_ans1 = answer == null ? "" : answer["user_ans1"].ToString().ToUpper();
            string real_ans1_acc1 = answer == null ? "" : answer["real_ans1_acc1"].ToString().ToUpper();
            bool correct = user_ans1 == real_ans1_acc1;


            string point = "";
            if (answer != null)
                point = answer["POINT"].ToString();

            string q_main = @"<table style='' class='ops_q_table' >
        <tr>
            <td colspan='3'>
              <span class='ops_q_head_span' > Q " + orderby + @"  (" + point + @" pts.) </span>
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
                    if (user_ans1 == "A")
                        if (correct)
                            ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                        else
                            ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/cross_red.png'/>");
                    else if (real_ans1_acc1 == "A")
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                    else
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "");


                    ops_tr_html = ops_tr_html.Replace(" @@ABCD@@", "A -)");
                    ops_tr_html = ops_tr_html.Replace(" @@OPS@@", q_def["OPA"].ToString());
                }
                else if (i == 2)
                {
                    if (user_ans1 == "B")
                        if (correct)
                            ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                        else
                            ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/cross_red.png'/>");
                    else if (real_ans1_acc1 == "B")
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                    else
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "");

                    ops_tr_html = ops_tr_html.Replace(" @@ABCD@@", "B -)");
                    ops_tr_html = ops_tr_html.Replace(" @@OPS@@", q_def["OPB"].ToString());
                }
                else if (i == 3)
                {
                    if (user_ans1 == "C")
                        if (correct)
                            ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                        else
                            ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/cross_red.png'/>");
                    else if (real_ans1_acc1 == "C")
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                    else
                        ops_tr_html = ops_tr_html.Replace("@SRC@", "");

                    ops_tr_html = ops_tr_html.Replace(" @@ABCD@@", "C -)");
                    ops_tr_html = ops_tr_html.Replace(" @@OPS@@", q_def["OPC"].ToString());
                }
                else if (i == 4)
                {
                    if (user_ans1 == "D")
                        if (correct)
                            ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/tick_green_small.png'/>");
                        else
                            ops_tr_html = ops_tr_html.Replace("@SRC@", "<img src='../images/cross_red.png'/>");
                    else if (real_ans1_acc1 == "D")
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

        protected string make_FILL(string orderby, DataRow q_def, DataRow answer)
        {
            string user_ans1 = answer == null ? "" : answer["user_ans1"].ToString();
            string user_ans2 = answer == null ? "" : answer["user_ans2"].ToString();
            string user_ans3 = answer == null ? "" : answer["user_ans3"].ToString();

            string real_ans1_acc1 = answer == null ? "" : answer["real_ans1_acc1"].ToString();
            string real_ans1_acc2 = answer == null ? "" : answer["real_ans1_acc2"].ToString();
            string real_ans1_acc3 = answer == null ? "" : answer["real_ans1_acc3"].ToString();

            string real_ans2_acc1 = answer == null ? "" : answer["real_ans2_acc1"].ToString();
            string real_ans2_acc2 = answer == null ? "" : answer["real_ans2_acc2"].ToString();
            string real_ans2_acc3 = answer == null ? "" : answer["real_ans2_acc3"].ToString();

            string real_ans3_acc1 = answer == null ? "" : answer["real_ans3_acc1"].ToString();
            string real_ans3_acc2 = answer == null ? "" : answer["real_ans3_acc2"].ToString();
            string real_ans3_acc3 = answer == null ? "" : answer["real_ans3_acc3"].ToString();


            string point = "";
            if (answer != null)
                point = answer["POINT"].ToString();

            string q_html = @"<table class='fill_q_table'>
        <tr>
            <td class='fill_q_head_td' >
                Question : " + orderby + @"  (" + point + @" pts.)
            </td>
        </tr>
        <tr><td>";

            q_html += "<span class='fill_text_span'> " + q_def["TEXT1"].ToString() + " </span>";



            //blank 1
            q_html += "<input type = 'text' id='blank1' value = '" + user_ans1.ToUpper() + "' disabled> ";

            //answer1 correct
            if (user_ans1.Trim() != "" && (user_ans1 == real_ans1_acc1 || user_ans1 == real_ans1_acc2 || user_ans1 == real_ans1_acc3))
                q_html += "<img src='../images/tick_green_small.png'/>";
            else
                q_html += "<img src='../images/cross_red_small.png'/>";




            //if there is text 2
            if (q_def["TEXT2"].ToString() != "")
                q_html += "<span class='fill_text_span'> " + q_def["TEXT2"].ToString() + " </span>";

            //if there is blank 2
            if (real_ans2_acc1 + real_ans2_acc2 + real_ans2_acc3 != "")
            {
                q_html += "<input type = 'text' id='blank2' value = '" + user_ans2.ToUpper() + "' disabled> ";

                //answer2 correct
                if (user_ans2.Trim() != "" && (user_ans2 == real_ans2_acc1 || user_ans2 == real_ans2_acc2 || user_ans2 == real_ans2_acc3))
                    q_html += "<img src='../images/tick_green_small.png'/>";
                else
                    q_html += "<img src='../images/cross_red_small.png'/>";


            }

            //if there is text 3
            if (q_def["TEXT3"].ToString() != "")
                q_html += "<span class='fill_text_span'> " + q_def["TEXT3"].ToString() + " </span>";

            //if there is blank 3
            if (real_ans3_acc1 + real_ans3_acc2 + real_ans3_acc3 != "")
            {

                q_html += "<input type = 'text' id='blank3' value = '" + user_ans3.ToUpper() + "' disabled> ";

                //answer2 correct
                if (user_ans3.Trim() != "" && (user_ans3 == real_ans3_acc1 || user_ans3 == real_ans3_acc2 || user_ans3 == real_ans3_acc3))
                    q_html += "<img src='../images/tick_green_small.png'/>";
                else
                    q_html += "<img src='../images/cross_red_small.png'/>";


            }

            //if there is text 4
            if (q_def["TEXT4"].ToString() != "")
                q_html += "<span class='fill_text_span'> " + q_def["TEXT4"].ToString() + " </span>";


            q_html += @"</td></tr>
    </table>";


            return q_html;
        }


        protected string make_exam_header(string assignid)
        {
            //get everything
            List<string> details = (List<string>)Session["exam_result_details"];

            if (details == null || details.Count == 0)
                return "";

            //make html
            string html = @"<table class='exam_header'>
        <tr>
            <th colspan='3'>" + details.ElementAt(0) + @"
            </th>
        </tr>
        <tr>
             <td>INITIALS - NAME</td>
            <td> " + details.ElementAt(2) + @"</td>
            <td></td>
        </tr>
        <tr>
            <td>DATE : </td>
            <td>" + details.ElementAt(3) + @"</td>
            <td></td>
        </tr>
        <tr>
            <td>RESULT : </td>
            <td>" + details.ElementAt(4) + @"</td>
            <td></td>
        </tr>
 <tr>
            <td>GRADE : </td>
            <td>" + details.ElementAt(5) + @" (" + details.ElementAt(1) + @"% required)</td>
            <td></td>
        </tr>
    </table>";

            //return
            return html;

        }
    }
}