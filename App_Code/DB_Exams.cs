using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web.UI.WebControls;

namespace AviaTrain.App_Code
{
    public static class DB_Exams
    {
        public static string con_str_hosting = ConfigurationManager.ConnectionStrings["local_dbconn"].ConnectionString;
        public static string con_str = ConfigurationManager.ConnectionStrings["dbconn_hosting"].ConnectionString;


        public static string push_Question_OPS(string sector, string op, string q, string answer, string a, string b, string c = null, string d = null)
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];

            try
            {

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @"  INSERT INTO EXM_QUESTIONS VALUES ( @TYPE , @SECTOR , 1 ," + user.employeeid + @" , convert(varchar, getutcdate(), 20) )
                            DECLARE @Q_ID INT = ( SELECT SCOPE_IDENTITY() )

                            INSERT INTO EXM_QUESTIONS_OPS 
                            VALUES ( @Q_ID , @OPS , @Q , @OPA , @OPB , @OPC, @OPD, @ANSWER  )  
                            select @Q_ID", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@TYPE", SqlDbType.VarChar).Value = op;
                    command.Parameters.Add("@SECTOR", SqlDbType.VarChar).Value = sector;
                    command.Parameters.Add("@OPS", SqlDbType.VarChar).Value = op;

                    command.Parameters.Add("@Q", SqlDbType.VarChar).Value = q;
                    command.Parameters.Add("@OPA", SqlDbType.VarChar).Value = a;
                    command.Parameters.Add("@OPB", SqlDbType.VarChar).Value = b;
                    command.Parameters.Add("@OPC", SqlDbType.VarChar).Value = c == null ? SqlString.Null : c;
                    command.Parameters.Add("@OPD", SqlDbType.VarChar).Value = d == null ? SqlString.Null : d;
                    command.Parameters.Add("@ANSWER", SqlDbType.VarChar).Value = answer;
                    command.CommandType = CommandType.Text;

                    string qid = Convert.ToString(command.ExecuteScalar());
                    if (qid != "")
                        return qid;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }

        public static string push_Question_FILL(string sector, string TEXT1, string FILL1_ANS1, string FILL1_ANS2, string FILL1_ANS3,
                                                             string TEXT2, string FILL2_ANS1, string FILL2_ANS2, string FILL2_ANS3,
                                                             string TEXT3, string FILL3_ANS1, string FILL3_ANS2, string FILL3_ANS3,
                                                             string TEXT4)
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];
            try
            {

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @"  INSERT INTO EXM_QUESTIONS VALUES ('FILL', @SECTOR ,1  ," + user.employeeid + @" , convert(varchar, getutcdate(), 20) )
                            DECLARE @Q_ID INT = (SELECT SCOPE_IDENTITY())

                            INSERT INTO EXM_QUESTIONS_FILL
                            VALUES (@Q_ID , @TEXT1, @FILL1_ANS1, @FILL1_ANS2, @FILL1_ANS3, @TEXT2, @FILL2_ANS1, @FILL2_ANS2, @FILL2_ANS3, @TEXT3, @FILL3_ANS1, @FILL3_ANS2, @FILL3_ANS3, @TEXT4)
                             select @Q_ID", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@SECTOR", SqlDbType.VarChar).Value = sector;
                    command.Parameters.Add("@TEXT1", SqlDbType.VarChar).Value = TEXT1;
                    command.Parameters.Add("@FILL1_ANS1", SqlDbType.VarChar).Value = FILL1_ANS1;
                    command.Parameters.Add("@FILL1_ANS2", SqlDbType.VarChar).Value = FILL1_ANS2;
                    command.Parameters.Add("@FILL1_ANS3", SqlDbType.VarChar).Value = FILL1_ANS3;
                    command.Parameters.Add("@TEXT2", SqlDbType.VarChar).Value = TEXT2;
                    command.Parameters.Add("@FILL2_ANS1", SqlDbType.VarChar).Value = FILL2_ANS1;
                    command.Parameters.Add("@FILL2_ANS2", SqlDbType.VarChar).Value = FILL2_ANS2;
                    command.Parameters.Add("@FILL2_ANS3", SqlDbType.VarChar).Value = FILL2_ANS3;
                    command.Parameters.Add("@TEXT3", SqlDbType.VarChar).Value = TEXT3;
                    command.Parameters.Add("@FILL3_ANS1", SqlDbType.VarChar).Value = FILL3_ANS1;
                    command.Parameters.Add("@FILL3_ANS2", SqlDbType.VarChar).Value = FILL3_ANS2;
                    command.Parameters.Add("@FILL3_ANS3", SqlDbType.VarChar).Value = FILL3_ANS3;
                    command.Parameters.Add("@TEXT4", SqlDbType.VarChar).Value = TEXT4;

                    command.CommandType = CommandType.Text;

                    string qid = Convert.ToString(command.ExecuteScalar());
                    if (qid != "")
                        return qid;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }

        public static bool push_EXAM_DEF(string name, string passpercent, Dictionary<string, string> questions)
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];

            string insert = @"INSERT INTO EXM_EXAM_DEFINITION VALUES ( @NAME, @PASSPERCENT, 1 , " + user.employeeid + @" , convert(varchar, getutcdate(), 20)   )
                                DECLARE @EXAMID INT = (SELECT SCOPE_IDENTITY())

                              ";
            int i = 1;
            foreach (KeyValuePair<string, string> pair in questions)
            {
                insert += @" INSERT INTO EXM_EXAM_DEF_QUESTIONS VALUES (@EXAMID, " + pair.Key + ", " + pair.Value + " , " + i.ToString() + @"  )  
                            ";
                i++;
            }


            try
            {

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(insert, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@NAME", SqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@PASSPERCENT", SqlDbType.Int).Value = passpercent;

                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return false;

        }

        public static string push_EXAM_Assignment(string examid, string traineeid, string schedule_start, string schedule_finish, string trn_assignid = "")
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];

            string insert = "";
            if (trn_assignid == "")
                insert = @"IF NOT EXISTS  (SELECT ASSIGN_ID FROM EXM_EXAM_ASSIGNMENT WHERE EXAM_ID = @EXAM_ID AND TRAINEE_ID = @TRAINEE_ID AND (STATUS = 'ASSIGNED' OR STATUS ='USER_STARTED') )
                        BEGIN
		                        INSERT INTO EXM_EXAM_ASSIGNMENT VALUES (@EXAM_ID, @TRAINEE_ID, 'ASSIGNED', @START, @FINISH, NULL, NULL, NULL, @BY , convert(varchar, getutcdate(), 20)  )  
                                SELECT SCOPE_IDENTITY() 
                        END
                        ELSE
                        BEGIN
		                         SELECT ASSIGN_ID FROM EXM_EXAM_ASSIGNMENT WHERE EXAM_ID = @EXAM_ID AND TRAINEE_ID = @TRAINEE_ID AND (STATUS = 'ASSIGNED' OR STATUS ='USER_STARTED')
                        END";
            else
            {//if it is a training exam
                insert = @"
 IF NOT EXISTS  (SELECT ASSIGN_ID FROM EXM_EXAM_ASSIGNMENT 
                 WHERE EXAM_ID = @EXAM_ID AND TRAINEE_ID = @TRAINEE_ID AND (STATUS = 'ASSIGNED_TRN' + @TRNASSIGNID OR STATUS ='USER_STARTED_TRN'+@TRNASSIGNID) 
                )
                        BEGIN
		                        INSERT INTO EXM_EXAM_ASSIGNMENT VALUES (@EXAM_ID, @TRAINEE_ID, 'ASSIGNED_TRN'+ @TRNASSIGNID , @START, @FINISH, NULL, NULL, NULL, @BY , convert(varchar, getutcdate(), 20)  )  
                                SELECT SCOPE_IDENTITY() 
                        END
                        ELSE
                        BEGIN
		                         SELECT ASSIGN_ID FROM EXM_EXAM_ASSIGNMENT WHERE EXAM_ID = @EXAM_ID AND TRAINEE_ID = @TRAINEE_ID AND (STATUS = 'ASSIGNED_TRN' + @TRNASSIGNID  OR STATUS ='USER_STARTED_TRN' + @TRNASSIGNID )
                        END";
            }


            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(insert, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EXAM_ID", SqlDbType.Int).Value = examid;
                    command.Parameters.Add("@TRAINEE_ID", SqlDbType.Int).Value = traineeid;
                    command.Parameters.Add("@START", SqlDbType.NVarChar).Value = schedule_start;
                    command.Parameters.Add("@FINISH", SqlDbType.NVarChar).Value = schedule_finish;
                    command.Parameters.Add("@BY", SqlDbType.Int).Value = user.employeeid;
                    if (trn_assignid != "")
                        command.Parameters.Add("@TRNASSIGNID", SqlDbType.VarChar).Value = trn_assignid;

                    command.CommandType = CommandType.Text;

                    string assignid = Convert.ToString(command.ExecuteScalar());
                    push_USERANSWERS_for_Assignment(assignid);

                    return assignid;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }

        public static bool push_USERANSWERS_for_Assignment(string assignid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                    INSERT INTO EXM_USER_ANSWERS
                        select ASS.ASSIGN_ID , EDEFQ.Q_ID, '','',''
                        from EXM_EXAM_DEF_QUESTIONS EDEFQ 
                        JOIN EXM_EXAM_ASSIGNMENT ASS ON ASS.EXAM_ID = ID
                        WHERE ASS.ASSIGN_ID = @ASSIGNID ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@ASSIGNID", SqlDbType.Int).Value = assignid;


                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return false;
        }

        public static bool push_Exam_Answer(string assignid, string qid, string ans1, string ans2, string ans3)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                     IF EXISTS (SELECT TOP 1 Q_ID  FROM  EXM_USER_ANSWERS  WHERE ASSIGN_ID = @ASSIGN_ID AND Q_ID = @Q_ID )
                        BEGIN
	                        UPDATE EXM_USER_ANSWERS SET ANSWER1 = @ANSWER1, ANSWER2 = @ANSWER2, ANSWER3 = @ANSWER3 WHERE ASSIGN_ID = @ASSIGN_ID AND Q_ID = @Q_ID 
                        END
                     ELSE
                        BEGIN
	                        INSERT INTO EXM_USER_ANSWERS VALUES (@ASSIGN_ID, @Q_ID, @ANSWER1, @ANSWER2, @ANSWER3) 
                        END ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@ASSIGN_ID", SqlDbType.Int).Value = assignid;
                    command.Parameters.Add("@Q_ID", SqlDbType.Int).Value = qid;
                    command.Parameters.Add("@ANSWER1", SqlDbType.NVarChar).Value = ans1;
                    command.Parameters.Add("@ANSWER2", SqlDbType.NVarChar).Value = ans2;
                    command.Parameters.Add("@ANSWER3", SqlDbType.NVarChar).Value = ans3;

                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return false;
        }

        public static bool update_Question_OPS(string qid, string q, string answer, string a, string b, string c = null, string d = null)
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];

            try
            {

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @" UPDATE EXM_QUESTIONS SET [BY] = @USERID , BY_TIME =convert(varchar, getutcdate(), 20)   WHERE ID= @QID

                           UPDATE EXM_QUESTIONS_OPS SET Q=@Q , OPA=@OPA, OPB=@OPB,OPC=@OPC , OPD=@OPD , ANSWER=@ANSWER  WHERE ID = @QID", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = user.employeeid;

                    command.Parameters.Add("@QID", SqlDbType.Int).Value = qid;
                    command.Parameters.Add("@Q", SqlDbType.VarChar).Value = q;
                    command.Parameters.Add("@OPA", SqlDbType.VarChar).Value = a;
                    command.Parameters.Add("@OPB", SqlDbType.VarChar).Value = b;
                    command.Parameters.Add("@OPC", SqlDbType.VarChar).Value = c == null ? SqlString.Null : c;
                    command.Parameters.Add("@OPD", SqlDbType.VarChar).Value = d == null ? SqlString.Null : d;
                    command.Parameters.Add("@ANSWER", SqlDbType.VarChar).Value = answer;
                    command.CommandType = CommandType.Text;

                    if (Convert.ToInt32(command.ExecuteNonQuery()) > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return false;
        }
        public static bool update_Question_FILL(string qid, string TEXT1, string FILL1_ANS1, string FILL1_ANS2, string FILL1_ANS3,
                                                              string TEXT2, string FILL2_ANS1, string FILL2_ANS2, string FILL2_ANS3,
                                                              string TEXT3, string FILL3_ANS1, string FILL3_ANS2, string FILL3_ANS3,
                                                              string TEXT4)
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];
            try
            {

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @"  UPDATE EXM_QUESTIONS SET [BY] = @USERID , BY_TIME =convert(varchar, getutcdate(), 20)   WHERE ID= @QID

                            UPDATE EXM_QUESTIONS_FILL
                            SET TEXT1        = @TEXT1     , 
                                FILL1_ANS1   = @FILL1_ANS1, 
                                FILL1_ANS2   = @FILL1_ANS2, 
                                FILL1_ANS3   = @FILL1_ANS3, 
                                TEXT2        = @TEXT2     , 
                                FILL2_ANS1   = @FILL2_ANS1, 
                                FILL2_ANS2   = @FILL2_ANS2, 
                                FILL2_ANS3   = @FILL2_ANS3, 
                                TEXT3        = @TEXT3     , 
                                FILL3_ANS1   = @FILL3_ANS1, 
                                FILL3_ANS2   = @FILL3_ANS2, 
                                FILL3_ANS3   = @FILL3_ANS3, 
                                TEXT4        = @TEXT4      
                            WHERE ID = @QID                          ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@QID", SqlDbType.Int).Value = qid;
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = user.employeeid;

                    command.Parameters.Add("@TEXT1", SqlDbType.VarChar).Value = TEXT1;
                    command.Parameters.Add("@FILL1_ANS1", SqlDbType.VarChar).Value = FILL1_ANS1;
                    command.Parameters.Add("@FILL1_ANS2", SqlDbType.VarChar).Value = FILL1_ANS2;
                    command.Parameters.Add("@FILL1_ANS3", SqlDbType.VarChar).Value = FILL1_ANS3;
                    command.Parameters.Add("@TEXT2", SqlDbType.VarChar).Value = TEXT2;
                    command.Parameters.Add("@FILL2_ANS1", SqlDbType.VarChar).Value = FILL2_ANS1;
                    command.Parameters.Add("@FILL2_ANS2", SqlDbType.VarChar).Value = FILL2_ANS2;
                    command.Parameters.Add("@FILL2_ANS3", SqlDbType.VarChar).Value = FILL2_ANS3;
                    command.Parameters.Add("@TEXT3", SqlDbType.VarChar).Value = TEXT3;
                    command.Parameters.Add("@FILL3_ANS1", SqlDbType.VarChar).Value = FILL3_ANS1;
                    command.Parameters.Add("@FILL3_ANS2", SqlDbType.VarChar).Value = FILL3_ANS2;
                    command.Parameters.Add("@FILL3_ANS3", SqlDbType.VarChar).Value = FILL3_ANS3;
                    command.Parameters.Add("@TEXT4", SqlDbType.VarChar).Value = TEXT4;

                    command.CommandType = CommandType.Text;

                    if (Convert.ToInt32(command.ExecuteNonQuery()) > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return false;
        }



        public static bool delete_EXAM(string examid)
        {
            // we removed this upon request: UPDATE EXM_EXAM_ASSIGNMENT SET STATUS='EXAM_DELETED' WHERE EXAM_ID=@EXAMID
            // todo : find another good way to handle 
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                               UPDATE EXM_EXAM_DEFINITION SET ISACTIVE=0 WHERE ID=@EXAMID  
                                ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EXAMID", SqlDbType.Int).Value = examid;
                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return false;
        }

        public static bool update_Exam_Assignment(string assignid, string status = "", string userstart = "", string userfinish = "", string grade = "" ,string trnassignid = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                    @" UPDATE EXM_EXAM_ASSIGNMENT 
                                        SET STATUS = CASE WHEN  @STATUS = '' THEN STATUS 
                                                          WHEN @TRNASSIGNID = ''  THEN @STATUS 
                                                          ELSE @STATUS + '_TRN' + @TRNASSIGNID END,
                                            USER_START = CASE WHEN @USERSTART = '' THEN USER_START ELSE CONVERT(VARCHAR, GETUTCDATE(),20) END,
                                            USER_FINISH = CASE WHEN @USERFINISH = '' THEN USER_FINISH ELSE CONVERT(VARCHAR, GETUTCDATE(),20) END,
                                            GRADE = CASE WHEN @GRADE = '' THEN GRADE ELSE @GRADE END
                        WHERE ASSIGN_ID = @ASSIGN_ID ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@ASSIGN_ID", SqlDbType.Int).Value = assignid;
                    command.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = status;
                    command.Parameters.Add("@USERSTART", SqlDbType.NVarChar).Value = userstart;
                    command.Parameters.Add("@USERFINISH", SqlDbType.NVarChar).Value = userfinish;
                    command.Parameters.Add("@GRADE", SqlDbType.NVarChar).Value = grade;
                    command.Parameters.Add("@TRNASSIGNID", SqlDbType.NVarChar).Value = trnassignid;

                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return false;
        }

        //delete question unless in an exam or in an trainingstep
        public static bool delete_question_unless_assigned(string qid, bool undelete = false)
        {

            string update = @"  declare @exist INT= (SELECT top 1 Q_ID  FROM EXM_EXAM_DEF_QUESTIONS where Q_ID = @QID)
                        declare @exist2  INT= (SELECT top 1 Q_ID  FROM TRN_STEP_QUESTIONS where Q_ID = @QID)

                        IF ISNULL(@exist,'') = '' AND ISNULL(@exist2, '') = ''
                        BEGIN
		                        UPDATE EXM_QUESTIONS SET ISACTIVE=0 WHERE ID= @QID
                        END";

            if (undelete)
                update = " UPDATE EXM_QUESTIONS SET ISACTIVE=1 WHERE ID= @QID";

            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(update, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@QID", SqlDbType.Int).Value = qid;

                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;

        }



        //gets all questions for exam creation
        public static DataTable get_ALL_questions()
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"
                                    SELECT EQ.ID, ISNULL(EQ.SECTOR,'') AS SECTOR , QUESTION FROM
                                     (
                                     SELECT ID, Q AS QUESTION FROM EXM_QUESTIONS_OPS
                                     UNION
                                     SELECT ID ,
                                         TEXT1 + ' ((BLANK)) ' + ISNULL(TEXT2,'') + ' ((BLANK)) ' + ISNULL(TEXT3,'') + ' ((BLANK)) ' + ISNULL(TEXT4,'') AS QUESTION
                                     FROM EXM_QUESTIONS_FILL
                                     ) TBL
                                     JOIN EXM_QUESTIONS EQ ON EQ.ID = TBL.ID 
                                     WHERE EQ.ISACTIVE=1
                                     ORDER BY TBL.ID DESC", connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }

        public static DataTable get_questions_withAnswer(string sector = "", string creater = "0", string text = "", bool onlyactive = true)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT TBL.ISACTIVE , TBL.ID, ISNULL(TBL.SECTOR,'') AS SECTOR , QUESTION , Answer 
                                FROM
                                (
                                   SELECT EQ.ISACTIVE , OPS.ID, EQ.SECTOR , Q AS QUESTION , 
                                   CASE WHEN OPS.ANSWER = 'A' THEN OPS.OPA
                                        WHEN OPS.ANSWER = 'B' THEN OPS.OPB
                                        WHEN OPS.ANSWER = 'C' THEN OPS.OPC
                                        WHEN OPS.ANSWER = 'D' THEN OPS.OPD END AS 'Answer'
                                   FROM EXM_QUESTIONS_OPS OPS
                                   JOIN EXM_QUESTIONS EQ ON EQ.ID = OPS.ID 
		                                AND  EQ.SECTOR = CASE WHEN @SECTOR = '' THEN EQ.SECTOR ELSE @SECTOR END
		                                AND  ( OPS.Q + OPS.OPA + OPS.OPB + ISNULL(OPS.OPC,'') + ISNULL(OPS.OPD,'') LIKE '%' + @TEXT + '%' )
		                                AND EQ.[BY] = CASE WHEN @BY = 0 THEN EQ.[BY] ELSE @BY END
                                   UNION
                                   SELECT EQ.ISACTIVE , FILL.ID , EQ.SECTOR ,
                                       TEXT1 + ' ((BLANK)) ' + ISNULL(TEXT2,'') + ' ((BLANK)) ' + ISNULL(TEXT3,'') + ' ((BLANK)) ' + ISNULL(TEXT4,'') AS 'QUESTION',
                                       '((' + 
                                       FILL1_ANS1 +','+FILL1_ANS2+','+FILL1_ANS3+')) - ((' + 
                                       FILL2_ANS1 +','+FILL2_ANS2+','+FILL2_ANS3+')) - ((' +
                                       FILL3_ANS1 +','+FILL3_ANS2+','+FILL3_ANS3+'))' AS 'Answer'
                                   FROM EXM_QUESTIONS_FILL FILL
                                   JOIN EXM_QUESTIONS EQ ON EQ.ID = FILL.ID 
		                                AND  EQ.SECTOR = CASE WHEN @SECTOR = '' THEN EQ.SECTOR ELSE @SECTOR END
		                                AND ( ISNULL(FILL.TEXT1,'') + ISNULL(FILL.TEXT2,'') +ISNULL(FILL.TEXT3,'') +ISNULL(FILL.TEXT4,'') 
			                                  + ISNULL(FILL.FILL1_ANS1,'') + ISNULL(FILL.FILL1_ANS2,'') + ISNULL(FILL.FILL1_ANS3,'')	
			                                  + ISNULL(FILL.FILL2_ANS1,'') + ISNULL(FILL.FILL2_ANS2,'') + ISNULL(FILL.FILL2_ANS3,'')
			                                  + ISNULL(FILL.FILL3_ANS1,'') + ISNULL(FILL.FILL3_ANS2,'') + ISNULL(FILL.FILL3_ANS3,'')
			                                     LIKE '%' + @TEXT + '%'
			                                )
                                        AND EQ.[BY] = CASE WHEN @BY = 0 THEN EQ.[BY] ELSE @BY END
                                ) TBL
                               " + (onlyactive ? "WHERE TBL.ISACTIVE = 1 " : "")
                                 + " ORDER BY TBL.ID DESC", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                        command.Parameters.Add("@BY", SqlDbType.Int).Value = creater;
                        command.Parameters.Add("@TEXT", SqlDbType.NVarChar).Value = text;
                        command.CommandType = CommandType.Text;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }



        //THIS FUNCTION DOESNT LOOK FOR ISACTIVE QUESTIONS, BRINGS THEM ALL
        //gets type and question definition without useranswer
        public static DataTable get_ONE_question(string id)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" DECLARE @TYPE VARCHAR(10) = ( SELECT [TYPE] FROM EXM_QUESTIONS WHERE ID = @Q_ID )
                                   
                                    IF @TYPE = '2' OR @TYPE = '3' OR @TYPE = '4'
                                    BEGIN
		                                    SELECT @TYPE as [TYPE] , Q , OPA , OPB, ISNULL(OPC,'') as [OPC], ISNULL(OPD,'') AS [OPD], ANSWER  
		                                    FROM EXM_QUESTIONS_OPS WHERE ID = @Q_ID
                                    END
                                    ELSE IF @TYPE = 'FILL'
                                    BEGIN
		                                    SELECT @TYPE as [TYPE], * FROM EXM_QUESTIONS_FILL WHERE ID  = @Q_ID
                                    END  ", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@Q_ID", SqlDbType.Int).Value = id;
                        command.CommandType = CommandType.Text;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }



        //gets exams for assignment creation
        public static DataTable get_Exams(bool onlyisactive = true)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT '0' as ID, ' --- ' as NAME
                            UNION
                                SELECT ID, [NAME] FROM EXM_EXAM_DEFINITION " + (onlyisactive ? " WHERE ISACTIVE = 1" : "") + " order by name", connection))
                    {
                        connection.Open();


                        command.CommandType = CommandType.Text;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;


        }

        //gets exam trainees for assignment creation
        public static DataTable get_EXAM_TRAINEES(bool onlyisactive = true)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT '0' AS [ID], ' --- ' AS [NAME]
                                  UNION
                                  SELECT DISTINCT
                                            U.[EMPLOYEEID] AS [ID], U.INITIAL + ' - ' + U.FIRSTNAME +' '+ U.SURNAME AS [NAME] 
                                  FROM USER_ROLES UR
                                  JOIN ROLE_DEFINITION RDEF  ON UR.ROLEID=RDEF.ID
                                  JOIN USERS U ON UR.EMPLOYEEID = U.EMPLOYEEID
                                  WHERE RDEF.[NAME] = 'EXAM_TRAINEE'" + (onlyisactive ? " AND U.ISACTIVE = 1" : ""), connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }





        //gets ORDERBY , id, SOLVED
        public static DataTable get_EXAM_QUESTIONS_by_assignid(string assignid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT EDQ.ORDERBY as 'Question' , EDQ.Q_ID , '0' as SOLVED
                                  FROM EXM_EXAM_ASSIGNMENT ASS
                                  JOIN EXM_EXAM_DEF_QUESTIONS EDQ ON ASS.EXAM_ID = EDQ.ID
                                  WHERE ASS.ASSIGN_ID = @ASSIGN_ID
                                  ORDER BY EDQ.ORDERBY ASC ", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@ASSIGN_ID", SqlDbType.Int).Value = assignid;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }


        // gets Q.ID, QUESTION, ANSWER IN A GRIDVIEW FORMAT 
        //CHECKS FOR ISACTIVE QUESTIONS 
        //DOESNT CHECK ISACTIVE EXAM
        public static DataTable get_EXAM_QUESTIONS_by_examid(string examid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT * FROM 
                                    (
                                            SELECT    OPS.ID AS 'Q.ID' ,  
                                             OPS.Q AS 'Question' ,
                                             CASE WHEN OPS.ANSWER = 'A' THEN OPS.OPA
                                                  WHEN OPS.ANSWER = 'B' THEN OPS.OPB
                                                  WHEN OPS.ANSWER = 'C' THEN OPS.OPC
                                                  WHEN OPS.ANSWER = 'D' THEN OPS.OPD END AS 'Answer',
                                                     DEFQ.POINT , DEFQ.ORDERBY
                                             FROM EXM_EXAM_DEF_QUESTIONS DEFQ
                                             JOIN EXM_QUESTIONS Q ON Q.ID = DEFQ.Q_ID AND (Q.TYPE='2' OR Q.TYPE='3' OR Q.TYPE='4' )
                                             JOIN EXM_QUESTIONS_OPS OPS ON OPS.ID = Q.ID
                                             WHERE DEFQ.ID = 44 AND  Q.ISACTIVE = 1
                                             UNION
                                             SELECT FILL.ID AS 'Q.ID' ,  
                                                     FILL.TEXT1 + ' ((blank)) ' + FILL.TEXT2 + ' ((blank)) '+ FILL.TEXT3 + ' ((blank)) ' + FILL.TEXT4  AS 'Question',
                                                     '((' + 
                                                     FILL1_ANS1 +','+FILL1_ANS2+','+FILL1_ANS3+')) - ((' + 
                                                     FILL2_ANS1 +','+FILL2_ANS2+','+FILL2_ANS3+')) - ((' +
                                                     FILL3_ANS1 +','+FILL3_ANS2+','+FILL3_ANS3+'))' AS 'Answer',
                                                     DEFQ.POINT , DEFQ.ORDERBY
                                             FROM EXM_EXAM_DEF_QUESTIONS DEFQ
                                             JOIN EXM_QUESTIONS Q ON Q.ID = DEFQ.Q_ID AND (Q.TYPE='FILL' )
                                             JOIN EXM_QUESTIONS_FILL FILL ON FILL.ID = Q.ID
                                             WHERE DEFQ.ID = 44 AND  Q.ISACTIVE = 1
                                     ) TBL
                                     ORDER BY TBL.ORDERBY

", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@EXAMID", SqlDbType.Int).Value = examid;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }

        public static DataTable get_Questions_MY_LAST_(string howmany = "15")
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT TOP " + howmany + @" * FROM (
                                                    SELECT  
                                                    OPS.ID AS 'ID' , Q.SECTOR, OPS.Q AS 'Question' , 
                                                    CASE WHEN OPS.ANSWER = 'A' THEN OPS.OPA
                                                         WHEN OPS.ANSWER = 'B' THEN OPS.OPB
                                                         WHEN OPS.ANSWER = 'C' THEN OPS.OPC
                                                         WHEN OPS.ANSWER = 'D' THEN OPS.OPD END AS 'Answer'
                                                    FROM EXM_QUESTIONS Q  
                                                    JOIN EXM_QUESTIONS_OPS OPS ON OPS.ID = Q.ID
                                                    WHERE Q.ISACTIVE = 1 AND (Q.TYPE='2' OR Q.TYPE='3' OR Q.TYPE='4' ) AND Q.[BY] = @USERID
                                                    UNION
                                                    SELECT 
                                                    FILL.ID AS 'ID' ,  Q.SECTOR,
                                                    FILL.TEXT1 + ' ((blank)) ' + FILL.TEXT2 + ' ((blank)) '+ FILL.TEXT3 + ' ((blank)) ' + FILL.TEXT4  AS 'Question',
                                                    '((' + 
                                                    FILL1_ANS1 +','+FILL1_ANS2+','+FILL1_ANS3+')) - ((' + 
                                                    FILL2_ANS1 +','+FILL2_ANS2+','+FILL2_ANS3+')) - ((' +
                                                    FILL3_ANS1 +','+FILL3_ANS2+','+FILL3_ANS3+'))' AS 'Answer'
                                                    FROM EXM_QUESTIONS Q 
                                                    JOIN EXM_QUESTIONS_FILL FILL ON FILL.ID = Q.ID
                                                    WHERE Q.ISACTIVE = 1 AND (Q.TYPE='FILL' ) AND Q.[BY] = @USERID
                                ) A
                                ORDER BY ID DESC
                                                    ", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@USERID", SqlDbType.Int).Value = user.employeeid;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }


        // DOESNT CARE EXAM ISACTIVE OR NOT
        //gets exam name for assignment creation
        public static string get_Exam_Name(string assignid = "0", string examid = "0")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                       IF @EXAMID = 0
                        BEGIN
                            SELECT DEF.[NAME] + '  (' + convert(varchar, DEF.PASSPERCENT ) + '% required)' 
                            FROM EXM_EXAM_DEFINITION DEF
                            JOIN EXM_EXAM_ASSIGNMENT EA ON EA.EXAM_ID = DEF.ID 
                            WHERE EA.ASSIGN_ID = @ASSIGN_ID
                        END
                        ELSE
                            SELECT DEF.[NAME] + '  (' + convert(varchar, DEF.PASSPERCENT ) + '% required)' 
                            FROM EXM_EXAM_DEFINITION DEF
                            WHERE DEF.ID = @EXAMID
                                                            ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@ASSIGN_ID", SqlDbType.Int).Value = assignid;
                    command.Parameters.Add("@EXAMID", SqlDbType.Int).Value = examid;
                    command.CommandType = CommandType.Text;

                    string res = Convert.ToString(command.ExecuteScalar());
                    return res;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }







        //THIS FUNCTION DOESNT LOOK FOR ISACTIVE QUESTIONS, BRINGS THEM ALL BECAUSE USER HAS ALREADY ANSWERED
        public static DataTable get_USER_ANSWER_of_Question(string assign_id, string q_id)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT 
                    EQ.[TYPE] AS [TYPE] , ISNULL(UA.ANSWER1,'') AS ANSWER1 , ISNULL(UA.ANSWER2,'') AS ANSWER2 , ISNULL(UA.ANSWER3,'') AS ANSWER3
                                    FROM EXM_USER_ANSWERS UA 
                                    JOIN EXM_QUESTIONS EQ ON EQ.ID = UA.Q_ID
                                    WHERE UA.Q_ID = @Q_ID AND UA.ASSIGN_ID = @ASSIGN_ID", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@ASSIGN_ID", SqlDbType.Int).Value = assign_id;
                        command.Parameters.Add("@Q_ID", SqlDbType.Int).Value = q_id;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }


        //THIS FUNCTION DOESNT LOOK FOR ISACTIVE QUESTIONS , BRINGS THEM ALL BECAUSE USER HAS ALREADY ANSWERED
        //THIS FUNCTION DOESNT LOOK FOR ISACTIVE EXAMS, BRINGS THEM ALL BECAUSE USER HAS ALREADY ANSWERED
        public static DataTable get_ALL_USER_ANSWERS_of_ASSIGNMENT(string assign_id)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" 
                                DECLARE @EXAM_ID INT = ( SELECT EXAM_ID FROM EXM_EXAM_ASSIGNMENT WHERE ASSIGN_ID = @ASSIGN_ID ) 

                                SELECT 
                                EDQ.Q_ID, Q.TYPE , EDQ.POINT, 
                                OPS.ANSWER AS 'real_ans1_acc1' , '' AS 'real_ans1_acc2' , '' AS 'real_ans1_acc3' ,
		                                '' AS 'real_ans2_acc1' , '' AS 'real_ans2_acc2' , '' AS 'real_ans2_acc3' ,
		                                '' AS 'real_ans3_acc1' , '' AS 'real_ans3_acc2' , '' AS 'real_ans3_acc3' ,
                                UA.ANSWER1 AS 'user_ans1' , '' AS 'user_ans2' , '' AS 'user_ans3' 
                                FROM EXM_EXAM_DEF_QUESTIONS EDQ
                                JOIN EXM_QUESTIONS Q ON EDQ.Q_ID = Q.ID
                                JOIN EXM_USER_ANSWERS UA ON EDQ.Q_ID = UA.Q_ID
                                JOIN EXM_QUESTIONS_OPS OPS ON OPS.ID = UA.Q_ID
                                WHERE EDQ.ID = @EXAM_ID AND ( Q.TYPE = '2' OR Q.TYPE = '3' OR Q.TYPE = '4')
                                AND UA.ASSIGN_ID = @ASSIGN_ID

                                UNION 

                                SELECT 
                                EDQ.Q_ID, Q.TYPE , EDQ.POINT, 
                                FILL.FILL1_ANS1 AS 'real_ans1_acc1' , FILL.FILL1_ANS2 AS 'real_ans1_acc2' , FILL.FILL1_ANS3 AS 'real_ans1_acc3' ,
                                FILL.FILL2_ANS1 AS 'real_ans2_acc1' , FILL.FILL2_ANS2 AS 'real_ans2_acc2' , FILL.FILL2_ANS3 AS 'real_ans2_acc3' ,
                                FILL.FILL3_ANS1 AS 'real_ans3_acc1' , FILL.FILL3_ANS2 AS 'real_ans3_acc2' , FILL.FILL3_ANS3 AS 'real_ans3_acc3' ,
                                UA.ANSWER1 AS 'user_ans1' ,
                                UA.ANSWER2 AS 'user_ans2' ,
                                UA.ANSWER3 AS 'user_ans3' 
                                FROM EXM_EXAM_DEF_QUESTIONS EDQ
                                JOIN EXM_QUESTIONS Q ON EDQ.Q_ID = Q.ID
                                JOIN EXM_USER_ANSWERS UA ON EDQ.Q_ID = UA.Q_ID
                                JOIN EXM_QUESTIONS_FILL FILL ON FILL.ID = UA.Q_ID
                                WHERE EDQ.ID = @EXAM_ID AND  Q.TYPE = 'FILL' 
                                AND UA.ASSIGN_ID = @ASSIGN_ID
                                ", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@ASSIGN_ID", SqlDbType.Int).Value = assign_id;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }





        public static string whose_Assignment(string assign_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                    @"  SELECT  TRAINEE_ID FROM EXM_EXAM_ASSIGNMENT WHERE ASSIGN_ID = @ASSIGN_ID ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@ASSIGN_ID", SqlDbType.Int).Value = assign_id;
                    command.CommandType = CommandType.Text;

                    return Convert.ToString(command.ExecuteScalar());

                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }

        public static string is_DOABLE_Exam_Assignment(string assignid, string traineeid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                                SELECT 
                                CASE 
	                                WHEN [STATUS] = 'FAILED' OR [STATUS] = 'PASSED'
			                                THEN 'EXAM ALREADY FINISHED'
	                                WHEN convert(datetime, getutcdate(), 20) < convert(datetime, SCHEDULE_START, 20) 
			                                THEN 'EXAM SCHEDULED TO A LATER DATE'
	                                 WHEN convert(datetime, getutcdate(), 20) > convert(datetime, SCHEDULE_FINISH, 20)
			                                THEN 'EXAM TIME IS EXPIRED'
	                                 WHEN TRAINEE_ID <> @TRAINEE_ID
			                                THEN 'NOT ASSIGNED TO THIS USER' 
	                                 ELSE 'OK' END AS PROBLEM
		
                                FROM EXM_EXAM_ASSIGNMENT WHERE ASSIGN_ID = @ASSIGN_ID
                      ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@ASSIGN_ID", SqlDbType.Int).Value = assignid;
                    command.Parameters.Add("@TRAINEE_ID", SqlDbType.Int).Value = traineeid;
                    command.CommandType = CommandType.Text;

                    return Convert.ToString(command.ExecuteScalar()); //expecting ok
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }






        //BRINGS NON-ISACTIVE EXAMS AS WELL BECAUSE TRAINEE ALREADY COMPLETED
        public static DataTable get_Assignments_Completed(string traineeid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" 
                                 SELECT DEF.NAME AS 'Exam Name' , ASS.STATUS as 'Status' , ASS.GRADE as 'Grade' , ASS.USER_FINISH as 'Time', ASS.ASSIGN_ID
                                 FROM EXM_EXAM_ASSIGNMENT ASS
                                 JOIN EXM_EXAM_DEFINITION DEF ON DEF.ID = ASS.EXAM_ID
                                 WHERE ASS.TRAINEE_ID = @TRAINEE_ID 
                                     AND ( ASS.STATUS = 'PASSED' OR ASS.STATUS = 'FAILED' OR ASS.STATUS = 'NOSHOW')
                                ", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@TRAINEE_ID", SqlDbType.Int).Value = traineeid;

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return null;

                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }

        //FOR ASSIGNMENTS WAITING TO BE COMPLETED, DOESNT BRING NON-ISACTIVE EXAMS
        public static DataTable get_Assignments_Open(string traineeid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @" 
                                  SELECT EDEF.NAME AS 'Exam Name' , 
                                      STATUS AS 'Status' ,
                                      ASS.SCHEDULE_START as 'Starts' , 
                                      ASS.SCHEDULE_FINISH as 'Finishes', 
                                      ASS.ASSIGN_ID 
                                   FROM EXM_EXAM_ASSIGNMENT ASS
                                   JOIN EXM_EXAM_DEFINITION EDEF ON EDEF.ID = ASS.EXAM_ID
                                   WHERE ASS.TRAINEE_ID = @TRAINEE_ID 
                                    AND ( ASS.STATUS = 'ASSIGNED' OR ASS.STATUS = 'USER_STARTED')
                                    AND EDEF.ISACTIVE=1 
                                    AND convert(datetime, getutcdate(), 20) <= convert(datetime, SCHEDULE_FINISH, 20)
                                    AND convert(datetime, getutcdate(), 20) >= convert(datetime, ASS.SCHEDULE_START, 20)
                                ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@TRAINEE_ID", SqlDbType.Int).Value = traineeid;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(res);

                    if (res == null || res.Rows.Count == 0)
                        return null;

                    return res;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }

        public static string is_examid_assigned_and_open(string examid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                               SELECT TOP 1 STATUS  FROM EXM_EXAM_ASSIGNMENT 
                                WHERE EXAM_ID = @EXAMID and [STATUS] = 'ASSIGNED' AND ISNULL(USER_START,'') = '' AND  
                                convert(datetime, getutcdate(), 20) < convert(datetime, SCHEDULE_FINISH, 20)  ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EXAMID", SqlDbType.Int).Value = examid;
                    command.CommandType = CommandType.Text;

                    return Convert.ToString(command.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }
        public static string get_Assignment_STATUS(string assignid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                               SELECT STATUS  FROM EXM_EXAM_ASSIGNMENT WHERE ASSIGN_ID = @ASSIGNID  ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@ASSIGNID", SqlDbType.Int).Value = assignid;
                    command.CommandType = CommandType.Text;

                    return Convert.ToString(command.ExecuteScalar());
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }






        public static DataTable View_Exam_Results(string examid, bool onlyactiveexam, string start_date, string finish_date,
                                                        string traineeid, bool onlyactivetrainee,
                                                        string passfailnoshow, string grade_start, string grade_finish)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @" 
SELECT 
EDEF.NAME AS 'Exam',
EDEF.PASSPERCENT as '% Req.', 
U.INITIAL + ' - ' + U.FIRSTNAME + ' ' + U.SURNAME as 'Trainee',
ASS.USER_FINISH as 'Time', 
ASS.STATUS as 'Result', 
ASS.GRADE as 'Grade',
ASS.EXAM_ID as 'EXAMID',
ASS.ASSIGN_ID as 'ASSIGNID'
FROM EXM_EXAM_ASSIGNMENT ASS
JOIN USERS U ON U.EMPLOYEEID = ASS.TRAINEE_ID              
JOIN EXM_EXAM_DEFINITION EDEF ON EDEF.ID = ASS.EXAM_ID     
WHERE 
ASS.[STATUS] NOT LIKE '%TRN%'
AND
ASS.EXAM_ID =  CASE  WHEN @USE_EXAMID = 1  THEN @EXAMID ELSE ASS.EXAM_ID END
AND
convert(datetime, ASS.USER_FINISH, 20)   >=  convert(datetime, @STARTDATE, 20) 
AND
convert(datetime, ASS.USER_FINISH, 20)   <= convert(datetime, @FINISHDATE, 20)  
AND
ASS.TRAINEE_ID = CASE WHEN @USE_TRAINEEID = 1 THEN @TRAINEEID ELSE ASS.TRAINEE_ID END
AND
ASS.STATUS = CASE WHEN @PASSFAILNOSHOW = 'ALL' THEN ASS.STATUS ELSE @PASSFAILNOSHOW END
AND 
CAST(ASS.GRADE AS DECIMAL(10, 3)) >= CAST(@GRADE_START AS DECIMAL(10, 3))
AND
CAST(ASS.GRADE AS DECIMAL(10, 3)) <= CAST(@GRADE_FINISH AS DECIMAL(10, 3))
AND 
EDEF.ISACTIVE = CASE WHEN @ONLYACTIVEEXAM = 1 THEN 1 ELSE EDEF.ISACTIVE END
AND
U.ISACTIVE = CASE WHEN @ONLYACTIVETRAINEE = 1 THEN 1 ELSE U.ISACTIVE END
                                ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@USE_EXAMID", SqlDbType.Int).Value = examid == "" ? 0 : 1;
                    command.Parameters.Add("@EXAMID", SqlDbType.Int).Value = examid == "" ? "123456" : examid;
                    command.Parameters.Add("@STARTDATE", SqlDbType.NVarChar).Value = start_date;
                    command.Parameters.Add("@FINISHDATE", SqlDbType.NVarChar).Value = finish_date;

                    command.Parameters.Add("@USE_TRAINEEID", SqlDbType.Int).Value = traineeid == "" ? 0 : 1;
                    command.Parameters.Add("@TRAINEEID", SqlDbType.Int).Value = traineeid == "" ? "123456" : traineeid;

                    command.Parameters.Add("@PASSFAILNOSHOW", SqlDbType.NVarChar).Value = passfailnoshow;

                    command.Parameters.Add("@GRADE_START", SqlDbType.NVarChar).Value = grade_start;
                    command.Parameters.Add("@GRADE_FINISH", SqlDbType.NVarChar).Value = grade_finish;

                    command.Parameters.Add("@ONLYACTIVEEXAM", SqlDbType.Int).Value = onlyactiveexam ? 1 : 0;
                    command.Parameters.Add("@ONLYACTIVETRAINEE", SqlDbType.Int).Value = onlyactivetrainee ? 1 : 0;

                    command.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(res);

                    if (res == null || res.Rows.Count == 0)
                        return null;

                    return res;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }





    }
}