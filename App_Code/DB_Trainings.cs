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
                            VALUES ( @NAME , @SECTOR , @EFFECTIVE  , @USERID , CONVERT(VARCHAR , GETUTCDATE(), 20 ), 1)

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
                    command.Parameters.Add("@STEP_TYPE", SqlDbType.VarChar).Value = steptype ;
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
                using (SqlCommand command = new SqlCommand( insert, connection))
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
                            VALUES( @TRN_ID , null , null , null )

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
                using (SqlCommand command = new SqlCommand(select , connection))
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

    }
}