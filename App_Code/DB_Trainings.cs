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
                              LAST_MODIFY=  CASE WHEN '' = @LAST_MODIFY THEN LAST_MODIFY ELSE @LAST_MODIFY END , 
                              LAST_MODIFY_DATE= CASE WHEN '' = @LAST_MODIFY_DATE THEN LAST_MODIFY_DATE ELSE  CONVERT(VARCHAR , GETUTCDATE(), 20 ) END  , 
                              STATUS=    CASE WHEN '' = @STATUS THEN STATUS ELSE @STATUS END , 
                              EXTRA=     CASE WHEN '' = @EXTRA THEN EXTRA ELSE @EXTRA END " +
                                (isactive != "" ? " , ISACTIVE= " + isactive : "") +
                      " WHERE ID=@TRNID ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@TRNID", SqlDbType.Int).Value = trnid;
                    command.Parameters.Add("@LAST_MODIFY", SqlDbType.Int).Value = last_modify;
                    command.Parameters.Add("@LAST_MODIFY_DATE", SqlDbType.NVarChar).Value = last_modif_date;
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

        public static bool Assign_Training(string trnid, string start, string finish, DataTable users)
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];

            string insert = @"DECLARE @EXAMID INT = (SELECT EXTRA FROM TRN_STEP WHERE TRN_ID = @TRNID AND STEPTYPE ='EXAM_STEP')";

            start = start == "" ? "2000-01-01" : start;
            finish = finish == "" ? "2099-01-01" : finish;
            foreach (DataRow row in users.Rows)
            {
                insert += @"
                        INSERT INTO TRN_ASSIGNMENT 
                        VALUES (@TRNID, " + row["ID"].ToString() + " , 'ASSIGNED', '" + start + "', '" + finish + "' , NULL,NULL,NULL,@EXAMID, @BY, CONVERT(VARCHAR , GETUTCDATE(), 20 ),1 )";
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
                insert += "INSERT INTO TRN_STEP_QUESTIONS VALUES (@STEPID , " + q_id + ")";

            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(insert, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
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
        public static string get_prev_next_STEPID(string trn_id, string current_stepid, bool next = true)
        {
            string select = "";
            try
            {
                if (next)
                    select = "SELECT TOP 1 ISNULL(STEP_ID,'') FROM TRN_STEP WHERE TRN_ID =@TRN_ID AND STEP_ID > @STEP_ID ORDER BY STEP_ID ASC";
                else
                    select = "SELECT TOP 1 ISNULL(STEP_ID,'') FROM TRN_STEP WHERE TRN_ID =@TRN_ID AND STEP_ID < @STEP_ID ORDER BY STEP_ID DESC";

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(select, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@TRN_ID", SqlDbType.Int).Value = trn_id;
                    command.Parameters.Add("@STEP_ID", SqlDbType.Int).Value = current_stepid;
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

        public static DataTable get_STEP_questions_withAnswers(string stepid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
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
                                        ASS.STATUS AS 'Status'
                                FROM TRN_ASSIGNMENT ASS 
                                JOIN TRN_TRAINING_DEF DEF ON ASS.TRNID = DEF.ID 
                                WHERE  ASS.STATUS = 'ASSIGNED' 
                                AND ASS.ISACTIVE = 1 AND DEF.ISACTIVE = 1 AND ASS.USERID = @USERID
                                AND convert(datetime, DEF.EFFECTIVE, 20) <= convert(datetime, GETUTCDATE(), 20)
                                AND  convert(datetime, ASS.SCHEDULE_FINISH, 20) >= 
							                                ( 
							                                CASE WHEN ASS.SCHEDULE_FINISH <> '' THEN  convert(datetime, getutcdate(), 104)
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
    }
}