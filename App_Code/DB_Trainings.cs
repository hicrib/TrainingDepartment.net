using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace AviaTrain.App_Code
{
    public class DB_Trainings
    {

        public static string con_str_hosting = ConfigurationManager.ConnectionStrings["local_dbconn"].ConnectionString;
        public static string con_str = ConfigurationManager.ConnectionStrings["dbconn_hosting"].ConnectionString;


        //return trn_id, stepid
        public static string push_Training_Info_Get_Ids(string name, string sector, string effective)
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];

            try
            {

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @"  INSERT INTO TRN_TRAINING_DEF 
                            VALUES ( @NAME , @SECTOR , @EFFECTIVE  , @USERID , CONVERT(VARCHAR , GETUTCDATE(), 20 ),'DESIGN_STARTED' ,NULL,1)

                            SELECT SCOPE_IDENTITY()        ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@NAME", SqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@SECTOR", SqlDbType.VarChar).Value = sector;
                    command.Parameters.Add("@EFFECTIVE", SqlDbType.VarChar).Value = effective;
                    command.Parameters.Add("@USERID", SqlDbType.VarChar).Value = user.employeeid;
                    command.CommandType = CommandType.Text;

                    string trn_id = Convert.ToString(command.ExecuteScalar());
                    if (trn_id != "")
                    {
                        string stepid = create_NEXT_STEP(trn_id);
                        if (stepid != "")
                            return trn_id + "," + stepid;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }

        public static bool update_TRAINING_DEF(string trnid, string last_modify = "", string last_modif_date = "", string status = "", string extra = "", string isactive = "")
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                     @"UPDATE TRN_TRAINING_DEF 
                       SET    
" + (last_modify == "" ? "" : "LAST_MODIFY=  CASE WHEN '' = @LAST_MODIFY THEN LAST_MODIFY ELSE @LAST_MODIFY END , ") +
                           @" LAST_MODIFY_DATE= CASE WHEN '' = @LAST_MODIFY_DATE THEN LAST_MODIFY_DATE ELSE  CONVERT(VARCHAR , GETUTCDATE(), 20 ) END  , 
                              STATUS=    CASE WHEN '' = @STATUS THEN STATUS ELSE @STATUS END , 
                              EXTRA=     CASE WHEN '' = @EXTRA THEN EXTRA ELSE @EXTRA END " +
                                (isactive != "" ? " , ISACTIVE= " + isactive : "") +
                      " WHERE ID=@TRNID ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@TRNID", SqlDbType.Int).Value = trnid;
                    command.Parameters.Add("@LAST_MODIFY_DATE", SqlDbType.NVarChar).Value = last_modif_date;
                    command.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = status;
                    command.Parameters.Add("@EXTRA", SqlDbType.NVarChar).Value = extra;
                    if (last_modify != "")
                        command.Parameters.Add("@LAST_MODIFY", SqlDbType.Int).Value = user.employeeid;

                    command.CommandType = CommandType.Text;

                    if (Convert.ToInt32(command.ExecuteNonQuery()) > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }

        public static bool Assign_Training(string trnid, string start, string finish, DataTable users)
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];

            string insert = @"DECLARE @EXAMID INT = (SELECT EXTRA FROM TRN_STEP WHERE TRN_ID = @TRNID AND STEPTYPE ='EXAM_STEP')
                              DECLARE @LASTSTEPID INT = (SELECT TOP 1 STEP_ID FROM TRN_STEP WHERE TRN_ID = @TRNID ORDER BY STEP_ID ASC)    
                            ";


            start = start == "" ? "2000-01-01" : start;
            finish = finish == "" ? "2099-01-01" : finish;
            foreach (DataRow row in users.Rows)
            {
                insert += @"
                        INSERT INTO TRN_ASSIGNMENT 
                        VALUES (@TRNID, " + row["ID"].ToString() + " , 'ASSIGNED', '" + start + "', '" + finish + "' , NULL,NULL,@LASTSTEPID,@EXAMID, @BY, CONVERT(VARCHAR , GETUTCDATE(), 20 ),1 , NULL)";
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(insert, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@TRNID", SqlDbType.Int).Value = trnid;
                    command.Parameters.Add("@BY", SqlDbType.Int).Value = user.employeeid;
                    command.CommandType = CommandType.Text;

                    if (Convert.ToInt32(command.ExecuteNonQuery()) > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }

        public static bool update_Assignment(string assignid, string laststepid = "0", string status = "", string userstart = "", string userfinish = "",string examassignid="0")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @" UPDATE TRN_ASSIGNMENT 
                            SET LASTSTEPID = CASE WHEN @LASTSTEPID = '0' THEN LASTSTEPID ELSE @LASTSTEPID END ,
	                            [STATUS] = CASE WHEN @STATUS = '' THEN [STATUS] ELSE @STATUS END,
                                USER_START = CASE WHEN @USERSTART = '' THEN USER_START ELSE CONVERT(VARCHAR , GETUTCDATE(), 20 ) END,
                                USER_FINISH = CASE WHEN @USERFINISH = '' THEN USER_FINISH ELSE CONVERT(VARCHAR , GETUTCDATE(), 20 ) END,
                                EXAM_ASSIGNID = CASE WHEN @EXAM_ASSIGNID = 0 THEN EXAM_ASSIGNID ELSE @EXAM_ASSIGNID END
                            WHERE ASSIGNID = @ASSIGNID", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@LASTSTEPID", SqlDbType.Int).Value = laststepid;
                    command.Parameters.Add("@ASSIGNID", SqlDbType.Int).Value = assignid;
                    command.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = status;
                    command.Parameters.Add("@USERSTART", SqlDbType.NVarChar).Value = userstart;
                    command.Parameters.Add("@USERFINISH", SqlDbType.NVarChar).Value = userfinish;
                    command.Parameters.Add("@EXAM_ASSIGNID", SqlDbType.Int).Value = examassignid;
                    command.CommandType = CommandType.Text;

                    if (Convert.ToInt32(command.ExecuteNonQuery()) > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }

        public static bool push_Step(string trnid, string stepid, string steptype, string texthtml, string fileaddress)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @"  UPDATE TRN_STEP SET STEPTYPE = @STEP_TYPE , TEXTHTML = @TEXTHTML, FILEADRESS = @FILEADRESS 
                            WHERE TRN_ID = @TRN_ID AND STEP_ID = @STEP_ID  ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@STEP_TYPE", SqlDbType.VarChar).Value = steptype;
                    command.Parameters.Add("@TEXTHTML", SqlDbType.VarChar).Value = texthtml;
                    command.Parameters.Add("@FILEADRESS", SqlDbType.VarChar).Value = fileaddress;
                    command.Parameters.Add("@TRN_ID", SqlDbType.Int).Value = trnid;
                    command.Parameters.Add("@STEP_ID", SqlDbType.Int).Value = stepid;
                    command.CommandType = CommandType.Text;

                    if (Convert.ToInt32(command.ExecuteNonQuery()) > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }

        public static bool push_Step_Questions(string stepid, List<string> q_ids)
        {
            string insert = "";
            foreach (string q_id in q_ids)
                insert += @"
                     IF NOT EXISTS (SELECT TOP 1 * FROM TRN_STEP_QUESTIONS WHERE STEP_ID = @STEPID AND Q_ID =" + q_id + @"  )
                     BEGIN
                          INSERT INTO TRN_STEP_QUESTIONS VALUES (@STEPID , " + q_id + @")
                     END ";

            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(insert, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.CommandType = CommandType.Text;

                    //TODO  : IF EXISTS CLAUSE PREVENTS ROWS_AFFECTED VALUE TO BE > 0
                    //FIND A SOLUTION FOR ERROR CHECK
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }

        public static string create_NEXT_STEP(string trnid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @" INSERT INTO TRN_STEP 
                            VALUES( @TRN_ID , null , null , null, NULL , NULL, 1 )

                            SELECT SCOPE_IDENTITY()  ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@TRN_ID", SqlDbType.Int).Value = trnid;
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

        public static bool update_STEP(string stepid, string steptype = "", string texthtml = "", string fileadress = "", string status = "", string extra = "", string isactive = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                     @"UPDATE TRN_STEP 
                             SET     STEPTYPE = CASE WHEN '' = @STEPTYPE THEN STEPTYPE ELSE @STEPTYPE END , 
                                     TEXTHTML=  CASE WHEN '' = @TEXTHTML THEN TEXTHTML ELSE @TEXTHTML END , 
                                     FILEADRESS=CASE WHEN '' = @FILEADRESS THEN FILEADRESS ELSE @FILEADRESS END , 
                                     STATUS=    CASE WHEN '' = @STATUS THEN STATUS ELSE @STATUS END , 
                                     EXTRA=     CASE WHEN '' = @EXTRA THEN EXTRA ELSE @EXTRA END  " +
                                   (isactive != "" ? " , ISACTIVE= " + isactive : "") +
                "            WHERE STEP_ID = @STEPID", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.Parameters.Add("@STEPTYPE", SqlDbType.NVarChar).Value = steptype;
                    command.Parameters.Add("@TEXTHTML", SqlDbType.NVarChar).Value = texthtml;
                    command.Parameters.Add("@FILEADRESS", SqlDbType.NVarChar).Value = fileadress;
                    command.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = status;
                    command.Parameters.Add("@EXTRA", SqlDbType.NVarChar).Value = extra;
                    command.CommandType = CommandType.Text;

                    if (Convert.ToInt32(command.ExecuteNonQuery()) > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }
        public static string get_prev_next_first_last_STEPID(string trn_id, string current_stepid, string which)
        {
            string select = "";
            try
            {
                if (which == "next")
                    select = "SELECT TOP 1 ISNULL(STEP_ID,'') FROM TRN_STEP WHERE TRN_ID =@TRN_ID AND STEP_ID > @STEP_ID ORDER BY STEP_ID ASC";
                else if (which == "prev")
                    select = "SELECT TOP 1 ISNULL(STEP_ID,'') FROM TRN_STEP WHERE TRN_ID =@TRN_ID AND STEP_ID < @STEP_ID ORDER BY STEP_ID DESC";
                else if (which == "last")
                    select = "SELECT TOP 1 STEP_ID FROM TRN_STEP WHERE TRN_ID = @TRN_ID and STEPTYPE='TRN'  ORDER BY STEP_ID DESC";
                else if (which == "first")
                    select = "SELECT TOP 1 STEP_ID FROM TRN_STEP WHERE TRN_ID = @TRN_ID and STEPTYPE='TRN'  ORDER BY STEP_ID ASC";

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(select, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@TRN_ID", SqlDbType.Int).Value = trn_id;
                    if (which == "next" || which == "prev")
                        command.Parameters.Add("@STEP_ID", SqlDbType.Int).Value = current_stepid;
                    command.CommandType = CommandType.Text;

                    return Convert.ToString(command.ExecuteScalar() as object); //so that it returns empty
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }

        //ROW_NUMBER()  OVER (ORDER BY s.step_id) as 'ORDERBY',  S.STEP_ID, S.STEPTYPE , S.TEXTHTML , ISNULL(SQ.Q_ID ,'') AS 'QID'
        public static DataTable get_STEP_INFO_with_questions(string trnid, string stepid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @"      SELECT ROW_NUMBER() OVER (ORDER BY s.step_id) as 'ORDERBY', 
                                            S.STEP_ID, S.STEPTYPE , S.TEXTHTML , ISNULL(SQ.Q_ID ,'0') AS 'QID' , ISNULL(S.EXTRA,'0') AS EXAMID
                                    FROM TRN_STEP S
                                    LEFT JOIN TRN_STEP_QUESTIONS SQ ON SQ.STEP_ID = S.STEP_ID
                                    WHERE S.TRN_ID = @TRNID AND S.STEP_ID = @STEPID 
                               ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@TRNID", SqlDbType.Int).Value = trnid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
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

        public static DataTable get_STEP_info(string stepid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT * FROM TRN_STEP WHERE STEP_ID = @STEPID", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
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

        //TBL.ID, ISNULL(TBL.SECTOR,'') AS SECTOR , QUESTION, Answer
        public static DataTable get_STEP_questions_withAnswers(string stepid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @"
                                SELECT TBL.ID, ISNULL(TBL.SECTOR,'') AS SECTOR , QUESTION, Answer FROM
                                (
                                   SELECT OPS.ID, Q AS QUESTION , EQ.SECTOR ,
                                   CASE WHEN OPS.ANSWER = 'A' THEN OPS.OPA
                                        WHEN OPS.ANSWER = 'B' THEN OPS.OPB
                                        WHEN OPS.ANSWER = 'C' THEN OPS.OPC
                                        WHEN OPS.ANSWER = 'D' THEN OPS.OPD END AS 'Answer'
                                   FROM EXM_QUESTIONS_OPS ops
                                   JOIN EXM_QUESTIONS EQ ON EQ.ID = OPS.ID AND EQ.ISACTIVE=1
                                   JOIN TRN_STEP_QUESTIONS TSQ ON TSQ.Q_ID = EQ.ID AND TSQ.STEP_ID = @STEPID
                                   UNION
                                   SELECT FILL.ID ,
                                       TEXT1 + ' ((BLANK)) ' + ISNULL(TEXT2,'') + ' ((BLANK)) ' + ISNULL(TEXT3,'') + ' ((BLANK)) ' + ISNULL(TEXT4,'') AS QUESTION,
                                       EQ.SECTOR ,
	                                   '((' + 
                                       FILL1_ANS1 +','+FILL1_ANS2+','+FILL1_ANS3+')) - ((' + 
                                       FILL2_ANS1 +','+FILL2_ANS2+','+FILL2_ANS3+')) - ((' +
                                       FILL3_ANS1 +','+FILL3_ANS2+','+FILL3_ANS3+'))' AS 'Answer'
                                   FROM EXM_QUESTIONS_FILL FILL
                                   JOIN EXM_QUESTIONS EQ ON EQ.ID = FILL.ID AND EQ.ISACTIVE=1
                                   JOIN TRN_STEP_QUESTIONS TSQ ON TSQ.Q_ID = EQ.ID AND TSQ.STEP_ID = @STEPID
                                ) TBL
                                ORDER BY TBL.ID DESC", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
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

        public static DataTable get_Assigned_Trainings_open(string employeeid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @" 
                                SELECT	DEF.NAME AS 'Training' ,
		                                ASS.SCHEDULE_START as 'Starts',
		                                ASS.SCHEDULE_FINISH AS 'Finishes',
		                                CASE WHEN EXAMID IS NULL THEN 'No' ELSE 'Yes' END AS 'Exam?' ,
		                                ASS.ASSIGNID,
                                        ASS.STATUS AS 'Status',
                                        ASS.LASTSTEPID 
                                FROM TRN_ASSIGNMENT ASS 
                                JOIN TRN_TRAINING_DEF DEF ON ASS.TRNID = DEF.ID 
                                WHERE ( ASS.STATUS = 'ASSIGNED' OR ASS.STATUS = 'USER_STARTED' )
                                AND ASS.ISACTIVE = 1 AND DEF.ISACTIVE = 1 AND ASS.USERID = @USERID
                                AND convert(datetime, DEF.EFFECTIVE, 20) <= convert(datetime, GETUTCDATE(), 20)
                                AND  convert(datetime, ASS.SCHEDULE_FINISH, 20) >= 
							                                ( 
							                                CASE WHEN ASS.SCHEDULE_FINISH <> '' THEN  convert(datetime, getutcdate(), 20)
							                                ELSE  convert(datetime, ASS.SCHEDULE_FINISH, 20) END
							                                )
                                AND  convert(datetime, ASS.SCHEDULE_START, 20) <= 
							                                ( 
							                                CASE WHEN ASS.SCHEDULE_START <> '' THEN  convert(datetime, getutcdate(), 20)
							                                ELSE  convert(datetime, ASS.SCHEDULE_START, 20) END
							                                ) 
                               ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = employeeid;
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

        public static DataTable get_TrainingNames()
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" 
                                SELECT 0 AS ID, '---' AS [NAME]
                                UNION 
                                SELECT ID , NAME FROM TRN_TRAINING_DEF WHERE ISACTIVE = 1 ORDER BY NAME 
                               ", connection))
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

        public static string get_TrainingName_by_assignid(string assignid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" 
                                SELECT CONVERT(VARCHAR , DEF.ID) + ',' + DEF.NAME
                                FROM TRN_ASSIGNMENT ASS
                                JOIN TRN_TRAINING_DEF DEF ON DEF.ID = ASS.TRNID
                                WHERE ASS.ASSIGNID = @ASSIGNID
                               ", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@ASSIGNID", SqlDbType.Int).Value = assignid;

                        return Convert.ToString(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return "";

        }

        public static DataTable get_training_Designs()
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" 
                                SELECT 
	                                CASE WHEN TDEF.ISACTIVE = 1 THEN 'Yes' Else 'No' END AS 'ACTIVE',
	                                NAME , 
	                                STATUS , 
	                                SECTOR , 
	                                EFFECTIVE , 
	                                U.FIRSTNAME + ' ' + U.SURNAME AS LAST_MODIFY,
	                                LAST_MODIFY_DATE,
	                                CASE WHEN EXISTS (SELECT TOP 1 * FROM TRN_STEP WHERE STEPTYPE='EXAM_STEP' AND TRN_ID=TDEF.ID AND ISNULL(EXTRA,'') <> '' )
					                                THEN 'Yes' ELSE 'No' END AS 'EXAM',
	                                TDEF.ID AS 'TRNID' , 
	                                (SELECT TOP 1 STEP_ID FROM TRN_STEP WHERE TRN_ID = TDEF.ID ORDER BY STEP_ID DESC) AS 'LASTSTEPID',
                                    (SELECT TOP 1 STEP_ID FROM TRN_STEP WHERE TRN_ID = TDEF.ID ORDER BY STEP_ID ASC) AS 'FIRSTSTEP'
                                from TRN_TRAINING_DEF TDEF 
                                JOIN USERS U ON U.EMPLOYEEID = TDEF.LAST_MODIFY
                               ", connection))
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


        public static string whose_Assignment(string assign_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                    @"  SELECT  USERID FROM TRN_ASSIGNMENT WHERE ASSIGNID = @ASSIGN_ID ", connection))
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
        public static string is_DOABLE_Training_Assignment(string assignid, string traineeid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                                SELECT 
                                CASE 
	                                WHEN [STATUS] = 'FAILED' OR [STATUS] = 'PASSED'
			                                THEN 'TRAINING ALREADY FINISHED'
	                                WHEN convert(datetime, getutcdate(), 20) < convert(datetime, SCHEDULE_START, 20) 
			                                THEN 'TRAINING SCHEDULED TO A LATER DATE'
	                                 WHEN convert(datetime, getutcdate(), 20) > convert(datetime, SCHEDULE_FINISH, 20)
			                                THEN 'TRAINING TIME IS EXPIRED'
	                                 WHEN USERID <> @TRAINEE_ID
			                                THEN 'NOT ASSIGNED TO THIS USER' 
	                                 ELSE 'OK' END AS PROBLEM
		
                                FROM TRN_ASSIGNMENT WHERE ASSIGNID = @ASSIGN_ID
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

        public static DataTable get_Completed_Trainings(string userid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" 
                               SELECT DEF.NAME , ASS.STATUS , ASS.USER_START , ASS.USER_FINISH , 
		                                CASE WHEN ISNULL(ASS.EXAMID,'') = '' THEN 'No' 
			                                 ELSE (SELECT EA.GRADE + ' ' + EA.STATUS FROM EXM_EXAM_ASSIGNMENT EA WHERE EA.ASSIGN_ID = ASS.EXAM_ASSIGNID )
			                                 END AS 'EXAM'
                                FROM TRN_ASSIGNMENT ASS
                                JOIN TRN_TRAINING_DEF DEF ON DEF.ID = ASS.TRNID 
                                WHERE ASS.USERID = @USERID  AND  ASS.STATUS IN ('PASSED','FAILED', 'FINISHED')
                                order by ass.USER_FINISH desc
                               ", connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@USERID", userid);
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
    }
}