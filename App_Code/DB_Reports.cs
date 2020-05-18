using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace AviaTrain.App_Code
{
    public class DB_Reports
    {

        public static string con_str_hosting = ConfigurationManager.ConnectionStrings["local_dbconn"].ConnectionString;
        public static string con_str = ConfigurationManager.ConnectionStrings["dbconn_hosting"].ConnectionString;


        #region PUSH REPORT
        public static void rollback_push_report(string reportid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(con_str))
                using (SqlCommand com = new SqlCommand(@" DELETE FROM REPORTS_META WHERE ID = @REPORTID

                                                          DELETE FROM REPORT_TR_ARE_APP_RAD WHERE ID = @REPORTID
                                                          DELETE FROM REPORT_DAILYTR_ASS_RAD WHERE ID = @REPORTID
                                                          DELETE FROM REPORT_DAILYTR_ASS_TWR WHERE ID = @REPORTID
                                                          DELETE FROM REPORT_TOWERTR_GMC_ADC WHERE ID = @REPORTID 

                                                          DELETE FROM CRITICALSKILL_RESULTS WHERE REPORTID = @REPORTID", con))
                {
                    con.Open();
                    com.Parameters.Add("@REPORTID", SqlDbType.Int).Value = reportid;
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {

            }

        }

        public static string push_Training_Report(string reporttype, Dictionary<string, string> data)
        {
            string reportid = "";

            try
            {


                if (reporttype == "1")
                    reportid = push_TR_ARE_APP_RAD(data);
                if (reporttype == "2")
                    reportid = push_DAILYTR_ASS_TWR(data);
                if (reporttype == "3")
                    reportid = push_DAILYTR_ASS_RAD(data);
                if (reporttype == "4")
                    reportid = push_TOWERTR_GMC_ADC(data);

                if (reportid == "")
                    return "";


                if (!push_criticalskill_results(reportid, reporttype, data))
                {
                    rollback_push_report(reportid);
                    return "";
                }


                if (!push_UserTrainingFolder(reportid, data["genid"]))
                {
                    rollback_push_report(reportid);
                    return "";
                }

                //todo : there is no rollback of usertrainingfolder when this fails. Rest is rolledback
                if (!update_totalhours(data["TRAINEE_ID"], data["POSITION"], data["TOTAL_HOURS"], "REPORT:" + reportid))
                {
                    rollback_push_report(reportid);
                    return "";
                }
            }
            catch (Exception e)
            {

                string mes = e.Message;
            }

            return reportid;
        }

        public static bool push_criticalskill_results(string reportid, string reportcategory, Dictionary<string, string> data)
        {
            try
            {
                string insert = @"with A as (
                                            @replaceme@
                                        )
                                        INSERT INTO CRITICALSKILL_RESULTS
                                        SELECT " + reportid + @", RSC.ID , A.RES FROM REPORT_SKILL_CATEGORIES RSC
                                        JOIN A   ON RSC.CATEG_SKILL = A.CATEG
                                        WHERE RSC.REPORTTYPE = " + reportcategory;

                //todo: empty radiobuttons might be regarded as N/A
                for (int i = 1; i < 11; i++)
                {
                    foreach (string st in new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S" })
                    {
                        string skill = i.ToString() + st;
                        if (data.ContainsKey(skill))
                        {
                            insert = insert.Replace("@replaceme@", @"SELECT '" + skill + "' as CATEG , '" + data[skill] + "' as RES   "
                                                           + " UNION @replaceme@");
                        }
                        else
                            break;// go to next skill category
                    }
                }
                //clean the union and @replaceme@
                insert = insert.Replace("UNION @replaceme@", "");

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(insert, connection))
                {
                    connection.Open();
                    if (command.ExecuteNonQuery() > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string debug = e.Message;
            }
            return false;
        }
        public static string push_TR_ARE_APP_RAD(Dictionary<string, string> data)
        {
            string reportid = "";

            // push into  REPORTS_META
            using (SqlConnection connection = new SqlConnection(con_str))
            using (SqlCommand command = new SqlCommand(
                         @"INSERT INTO REPORTS_META VALUES('1', @OJTI_ID, convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , 1, @TRAINEE_SIGNED , @TRAINEE_ID )
                           DECLARE @ID INT = ( SELECT SCOPE_IDENTITY() )

                         INSERT INTO REPORT_TR_ARE_APP_RAD
                                ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_PRELEVEL1],[CHK_SIM],[CHK_LVLASS],[CHK_PROGASS],[CHK_COCASS],[CHK_REMASS],[CHK_OTS],[DATE],[POSITION],[POSITION_EXTRA],[TIMEON_SCH],[TIMEOFF_SCH],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[NOTES],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS],TIMEON_ACT,TIMEOFF_ACT )
                         VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_PRELEVEL1,@CHK_SIM,@CHK_LVLASS,@CHK_PROGASS,@CHK_COCASS,@CHK_REMASS,@CHK_OTS,@DATE,@POSITION,@POSITION_EXTRA,@TIMEON_SCH,@TIMEOFF_SCH,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@NOTES,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS,@TIMEON_ACT, @TIMEOFF_ACT)

                            SELECT @ID", connection))
            {
                connection.Open();
                //SqlTransaction tran = connection.BeginTransaction();
                try
                {
                    command.Parameters.AddWithValue("@OJTI_ID", data["OJTI_ID"]);
                    command.Parameters.AddWithValue("@TRAINEE_SIGNED", data["TRAINEE_SIGNED"]);
                    command.Parameters.AddWithValue("@TRAINEE_ID", data["TRAINEE_ID"]);

                    command.Parameters.Add("@CHK_OJT", SqlDbType.Bit).Value = data["CHK_OJT"] == "1";
                    command.Parameters.Add("@CHK_PRELEVEL1", SqlDbType.Bit).Value = data["CHK_PRELEVEL1"] == "1";
                    command.Parameters.Add("@CHK_SIM", SqlDbType.Bit).Value = data["CHK_SIM"] == "1";
                    command.Parameters.Add("@CHK_LVLASS", SqlDbType.Bit).Value = data["CHK_LVLASS"] == "1";
                    command.Parameters.Add("@CHK_PROGASS", SqlDbType.Bit).Value = data["CHK_PROGASS"] == "1";
                    command.Parameters.Add("@CHK_COCASS", SqlDbType.Bit).Value = data["CHK_COCASS"] == "1";
                    command.Parameters.Add("@CHK_REMASS", SqlDbType.Bit).Value = data["CHK_REMASS"] == "1";
                    command.Parameters.Add("@CHK_OTS", SqlDbType.Bit).Value = data["CHK_OTS"] == "1";
                    command.Parameters.Add("@DATE", SqlDbType.VarChar).Value = data["DATE"];
                    command.Parameters.Add("@POSITION", SqlDbType.VarChar).Value = data["POSITION"];
                    command.Parameters.Add("@POSITION_EXTRA", SqlDbType.VarChar).Value = data["POSITION_EXTRA"];
                    command.Parameters.Add("@TIMEON_SCH", SqlDbType.VarChar).Value = data["TIMEON_SCH"];
                    command.Parameters.Add("@TIMEOFF_SCH", SqlDbType.VarChar).Value = data["TIMEOFF_SCH"];
                    command.Parameters.Add("@TRAF_DENS", SqlDbType.VarChar).Value = data["TRAF_DENS"];
                    command.Parameters.Add("@COMPLEXITY", SqlDbType.VarChar).Value = data["COMPLEXITY"];
                    command.Parameters.Add("@HOURS", SqlDbType.VarChar).Value = data["HOURS"];
                    command.Parameters.Add("@TOTAL_HOURS", SqlDbType.VarChar).Value = data["TOTAL_HOURS"];
                    command.Parameters.Add("@PREBRIEF_COMMENTS_FILENAME", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS_FILENAME"];
                    command.Parameters.Add("@PREBRIEF_COMMENTS", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS"];
                    command.Parameters.Add("@NOTES", SqlDbType.NVarChar).Value = data["NOTES"];
                    command.Parameters.Add("@ADDITIONAL_COMMENTS", SqlDbType.NVarChar).Value = data["ADDITIONAL_COMMENTS"];
                    command.Parameters.Add("@STUDENT_COMMENTS", SqlDbType.NVarChar).Value = data["STUDENT_COMMENTS"];
                    command.Parameters.Add("@TIMEON_ACT", SqlDbType.NVarChar).Value = data["TIMEON_ACT"];
                    command.Parameters.Add("@TIMEOFF_ACT", SqlDbType.NVarChar).Value = data["TIMEOFF_ACT"];

                    reportid = Convert.ToString(command.ExecuteScalar() as object);
                    //tran.Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        //tran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        string debug = exRollback.Message + "___" + e.Message;
                    }

                }

                return reportid;
            }
        }

        public static string push_TOWERTR_GMC_ADC(Dictionary<string, string> data)
        {
            string reportid = "";

            // push into  REPORTS_META
            using (SqlConnection connection = new SqlConnection(con_str))
            using (SqlCommand command = new SqlCommand(
@"INSERT INTO REPORTS_META VALUES('4', @OJTI_ID, convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , 1, @TRAINEE_SIGNED , @TRAINEE_ID )
  DECLARE @ID INT = ( SELECT SCOPE_IDENTITY() )

INSERT INTO REPORT_TOWERTR_GMC_ADC
        ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_PRELEVEL1],[CHK_SIM],[CHK_LVLASS],[CHK_PROGASS],[CHK_COCASS],[CHK_REMASS],[CHK_OTS],[DATE],[POSITION],[TIMEON_SCH],[TIMEOFF_SCH],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS],TIMEON_ACT,TIMEOFF_ACT )
VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_PRELEVEL1,@CHK_SIM,@CHK_LVLASS,@CHK_PROGASS,@CHK_COCASS,@CHK_REMASS,@CHK_OTS,@DATE,@POSITION,@TIMEON_SCH,@TIMEOFF_SCH,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS,@TIMEON_ACT, @TIMEOFF_ACT)

SELECT @ID
", connection))
            {
                connection.Open();
                //SqlTransaction tran = connection.BeginTransaction();
                try
                {
                    command.Parameters.AddWithValue("@OJTI_ID", data["OJTI_ID"]);
                    command.Parameters.AddWithValue("@TRAINEE_SIGNED", data["TRAINEE_SIGNED"]);
                    command.Parameters.AddWithValue("@TRAINEE_ID", data["TRAINEE_ID"]);

                    command.Parameters.Add("@CHK_OJT", SqlDbType.Bit).Value = data["CHK_OJT"] == "1";
                    command.Parameters.Add("@CHK_PRELEVEL1", SqlDbType.Bit).Value = data["CHK_PRELEVEL1"] == "1";
                    command.Parameters.Add("@CHK_SIM", SqlDbType.Bit).Value = data["CHK_SIM"] == "1";
                    command.Parameters.Add("@CHK_LVLASS", SqlDbType.Bit).Value = data["CHK_LVLASS"] == "1";
                    command.Parameters.Add("@CHK_PROGASS", SqlDbType.Bit).Value = data["CHK_PROGASS"] == "1";
                    command.Parameters.Add("@CHK_COCASS", SqlDbType.Bit).Value = data["CHK_COCASS"] == "1";
                    command.Parameters.Add("@CHK_REMASS", SqlDbType.Bit).Value = data["CHK_REMASS"] == "1";
                    command.Parameters.Add("@CHK_OTS", SqlDbType.Bit).Value = data["CHK_OTS"] == "1";
                    command.Parameters.Add("@DATE", SqlDbType.VarChar).Value = data["DATE"];
                    command.Parameters.Add("@POSITION", SqlDbType.VarChar).Value = data["POSITION"];
                    command.Parameters.Add("@TIMEON_SCH", SqlDbType.VarChar).Value = data["TIMEON_SCH"];
                    command.Parameters.Add("@TIMEOFF_SCH", SqlDbType.VarChar).Value = data["TIMEOFF_SCH"];
                    command.Parameters.Add("@TRAF_DENS", SqlDbType.VarChar).Value = data["TRAF_DENS"];
                    command.Parameters.Add("@COMPLEXITY", SqlDbType.VarChar).Value = data["COMPLEXITY"];
                    command.Parameters.Add("@HOURS", SqlDbType.VarChar).Value = data["HOURS"];
                    command.Parameters.Add("@TOTAL_HOURS", SqlDbType.VarChar).Value = data["TOTAL_HOURS"];
                    command.Parameters.Add("@PREBRIEF_COMMENTS_FILENAME", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS_FILENAME"];
                    command.Parameters.Add("@PREBRIEF_COMMENTS", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS"];
                    command.Parameters.Add("@ADDITIONAL_COMMENTS", SqlDbType.NVarChar).Value = data["ADDITIONAL_COMMENTS"];
                    command.Parameters.Add("@STUDENT_COMMENTS", SqlDbType.NVarChar).Value = data["STUDENT_COMMENTS"];
                    command.Parameters.Add("@TIMEON_ACT", SqlDbType.NVarChar).Value = data["TIMEON_ACT"];
                    command.Parameters.Add("@TIMEOFF_ACT", SqlDbType.NVarChar).Value = data["TIMEOFF_ACT"];


                    reportid = Convert.ToString(command.ExecuteScalar() as object);
                    //tran.Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        //tran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        string debug = exRollback.Message + "___" + e.Message;
                    }

                }

                return reportid;
            }
        }

        public static string push_DAILYTR_ASS_RAD(Dictionary<string, string> data)
        {
            string reportid = "";


            //todo: push into  REPORTS_META

            using (SqlConnection connection = new SqlConnection(con_str))
            using (SqlCommand command = new SqlCommand(
         @"
INSERT INTO REPORTS_META VALUES('3', @OJTI_ID, convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , 1, @TRAINEE_SIGNED , @TRAINEE_ID )
DECLARE @ID INT = ( SELECT SCOPE_IDENTITY() )

INSERT INTO REPORT_DAILYTR_ASS_RAD
       ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_ASS],[DATE],[POSITION],[POSITION_EXTRA],[TIMEON_SCH],[TIMEOFF_SCH],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS],TIMEON_ACT, TIMEOFF_ACT )
VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_ASS,@DATE,@POSITION,@POSITION_EXTRA,@TIMEON_SCH,@TIMEOFF_SCH,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS, @TIMEON_ACT, @TIMEOFF_ACT)

SELECT @ID
", connection))
            {
                connection.Open();
                //SqlTransaction tran = connection.BeginTransaction();
                try
                {
                    command.Parameters.AddWithValue("@OJTI_ID", data["OJTI_ID"]);
                    command.Parameters.AddWithValue("@TRAINEE_SIGNED", data["TRAINEE_SIGNED"]);
                    command.Parameters.AddWithValue("@TRAINEE_ID", data["TRAINEE_ID"]);

                    command.Parameters.Add("@CHK_OJT", SqlDbType.Bit).Value = data["CHK_OJT"] == "1";
                    command.Parameters.Add("@CHK_ASS", SqlDbType.Bit).Value = data["CHK_ASS"] == "1";
                    command.Parameters.Add("@DATE", SqlDbType.VarChar).Value = data["DATE"];
                    command.Parameters.Add("@POSITION", SqlDbType.VarChar).Value = data["POSITION"];
                    command.Parameters.Add("@POSITION_EXTRA", SqlDbType.VarChar).Value = data["POSITION_EXTRA"];
                    command.Parameters.Add("@TIMEON_SCH", SqlDbType.VarChar).Value = data["TIMEON_SCH"];
                    command.Parameters.Add("@TIMEOFF_SCH", SqlDbType.VarChar).Value = data["TIMEOFF_SCH"];
                    command.Parameters.Add("@TRAF_DENS", SqlDbType.VarChar).Value = data["TRAF_DENS"];
                    command.Parameters.Add("@COMPLEXITY", SqlDbType.VarChar).Value = data["COMPLEXITY"];
                    command.Parameters.Add("@HOURS", SqlDbType.VarChar).Value = data["HOURS"];
                    command.Parameters.Add("@TOTAL_HOURS", SqlDbType.VarChar).Value = data["TOTAL_HOURS"];
                    command.Parameters.Add("@PREBRIEF_COMMENTS_FILENAME", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS_FILENAME"];
                    command.Parameters.Add("@PREBRIEF_COMMENTS", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS"];
                    command.Parameters.Add("@ADDITIONAL_COMMENTS", SqlDbType.NVarChar).Value = data["ADDITIONAL_COMMENTS"];
                    command.Parameters.Add("@STUDENT_COMMENTS", SqlDbType.NVarChar).Value = data["STUDENT_COMMENTS"];
                    command.Parameters.Add("@TIMEON_ACT", SqlDbType.NVarChar).Value = data["TIMEON_ACT"];
                    command.Parameters.Add("@TIMEOFF_ACT", SqlDbType.NVarChar).Value = data["TIMEOFF_ACT"];

                    reportid = Convert.ToString(command.ExecuteScalar() as object);
                    //tran.Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        //tran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        string debug = exRollback.Message + "___" + e.Message;
                    }

                }

                return reportid;

            }

        }

        public static string push_DAILYTR_ASS_TWR(Dictionary<string, string> data)
        {
            string reportid = "";


            //todo: push into  REPORTS_META

            using (SqlConnection connection = new SqlConnection(con_str))
            using (SqlCommand command = new SqlCommand(
@"
INSERT INTO REPORTS_META VALUES('2', @OJTI_ID, convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , 1, @TRAINEE_SIGNED , @TRAINEE_ID )
DECLARE @ID INT = ( SELECT SCOPE_IDENTITY() )

INSERT INTO REPORT_DAILYTR_ASS_TWR
       ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_ASS],[DATE],[POSITION],[TIMEON_SCH],[TIMEOFF_SCH],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS], TIMEON_ACT, TIMEOFF_ACT )
VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_ASS,@DATE,@POSITION,@TIMEON_SCH,@TIMEOFF_SCH,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS, @TIMEON_ACT, @TIMEOFF_ACT)

SELECT @ID
", connection))
            {
                connection.Open();
                //SqlTransaction tran = connection.BeginTransaction();
                try
                {
                    command.Parameters.AddWithValue("@OJTI_ID", data["OJTI_ID"]);
                    command.Parameters.AddWithValue("@TRAINEE_SIGNED", data["TRAINEE_SIGNED"]);
                    command.Parameters.AddWithValue("@TRAINEE_ID", data["TRAINEE_ID"]);

                    command.Parameters.Add("@CHK_OJT", SqlDbType.Bit).Value = data["CHK_OJT"] == "1";
                    command.Parameters.Add("@CHK_ASS", SqlDbType.Bit).Value = data["CHK_ASS"] == "1";
                    command.Parameters.Add("@DATE", SqlDbType.VarChar).Value = data["DATE"];
                    command.Parameters.Add("@POSITION", SqlDbType.VarChar).Value = data["POSITION"];
                    command.Parameters.Add("@TIMEON_SCH", SqlDbType.VarChar).Value = data["TIMEON_SCH"];
                    command.Parameters.Add("@TIMEOFF_SCH", SqlDbType.VarChar).Value = data["TIMEOFF_SCH"];
                    command.Parameters.Add("@TRAF_DENS", SqlDbType.VarChar).Value = data["TRAF_DENS"];
                    command.Parameters.Add("@COMPLEXITY", SqlDbType.VarChar).Value = data["COMPLEXITY"];
                    command.Parameters.Add("@HOURS", SqlDbType.VarChar).Value = data["HOURS"];
                    command.Parameters.Add("@TOTAL_HOURS", SqlDbType.VarChar).Value = data["TOTAL_HOURS"];
                    command.Parameters.Add("@PREBRIEF_COMMENTS_FILENAME", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS_FILENAME"];
                    command.Parameters.Add("@PREBRIEF_COMMENTS", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS"];
                    command.Parameters.Add("@ADDITIONAL_COMMENTS", SqlDbType.NVarChar).Value = data["ADDITIONAL_COMMENTS"];
                    command.Parameters.Add("@STUDENT_COMMENTS", SqlDbType.NVarChar).Value = data["STUDENT_COMMENTS"];
                    command.Parameters.Add("@TIMEON_ACT", SqlDbType.NVarChar).Value = data["TIMEON_ACT"];
                    command.Parameters.Add("@TIMEOFF_ACT", SqlDbType.NVarChar).Value = data["TIMEOFF_ACT"];


                    reportid = Convert.ToString(command.ExecuteScalar() as object);
                    //tran.Commit();
                }
                catch (Exception e)
                {
                    try
                    {
                        //tran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        string debug = exRollback.Message + "___" + e.Message;
                    }

                }

                return reportid;

            }

        }

        public static string push_RECOM_LEVEL(Dictionary<string, string> data)
        {
            //todo : implement

            string reportid = "";


            //todo: push into  REPORTS_META
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                             @"INSERT INTO REPORTS_META VALUES('5', '" + data["OJTI_ID"] + "', convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , " + data["OJTI_SIGNED"] + ", " + data["TRAINEE_SIGNED"] + " , " + data["TRAINEE_ID"] + ")   ; SELECT SCOPE_IDENTITY()", connection))
                {
                    connection.Open();
                    reportid = Convert.ToString(command.ExecuteScalar());
                    if (String.IsNullOrWhiteSpace(reportid))
                        return "";
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }


            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                INSERT INTO REPORT_RECOM_LEVEL ( ID, EMPLOYEEID , POSITION, SECTOR, PHASE , LEVEL , OJTI_ID , OJTI_SIGNED , TOTAL_HOURS,MER_MET, READING_SIGNED,
								 OBJECTIVES_SIGNED, FOLDER_COMPLETE, TRAINEE_SIGNED , DATE , DEPARTMENT_SIGNED, DEPARTMENT_EMPLOYEEID,COMMENTS)
                VALUES 
								(@ID, @TRAINEE_ID , @POSITION, @SECTOR, @PHASE , @LEVEL , @OJTI_ID , @OJTI_SIGNED , @TOTAL_HOURS,@MER_MET, @READING_SIGNED,
								 @OBJECTIVES_SIGNED, @FOLDER_COMPLETE, @TRAINEE_SIGNED , @DATE , @DEPARTMENT_SIGNED, @DEPARTMENT_EMPLOYEEID  ,@COMMENTS)", connection))
                {
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = reportid;
                    command.Parameters.Add("@TRAINEE_ID", SqlDbType.Int).Value = data["TRAINEE_ID"];
                    command.Parameters.Add("@POSITION", SqlDbType.VarChar).Value = data["POSITION"];
                    command.Parameters.Add("@SECTOR", SqlDbType.VarChar).Value = data["SECTOR"];
                    command.Parameters.Add("@PHASE", SqlDbType.VarChar).Value = data["PHASE"];
                    command.Parameters.Add("@LEVEL", SqlDbType.Int).Value = data["LEVEL"];
                    command.Parameters.Add("@OJTI_ID", SqlDbType.Int).Value = data["OJTI_ID"];
                    command.Parameters.Add("@OJTI_SIGNED", SqlDbType.Bit).Value = data["OJTI_SIGNED"] == "1";
                    command.Parameters.Add("@TOTAL_HOURS", SqlDbType.VarChar).Value = data["TOTAL_HOURS"];
                    command.Parameters.Add("@MER_MET", SqlDbType.Bit).Value = data["MER_MET"] == "1";
                    command.Parameters.Add("@READING_SIGNED", SqlDbType.Bit).Value = data["READING_SIGNED"] == "1";
                    command.Parameters.Add("@OBJECTIVES_SIGNED", SqlDbType.Bit).Value = data["OBJECTIVES_SIGNED"] == "1";
                    command.Parameters.Add("@FOLDER_COMPLETE", SqlDbType.Bit).Value = data["FOLDER_COMPLETE"] == "1";
                    command.Parameters.Add("@TRAINEE_SIGNED", SqlDbType.Bit).Value = data["TRAINEE_SIGNED"] == "1";
                    command.Parameters.Add("@DATE", SqlDbType.VarChar).Value = data["DATE"];
                    command.Parameters.Add("@DEPARTMENT_SIGNED", SqlDbType.Bit).Value = data["DEPARTMENT_SIGNED"] == "1";
                    if (data["DEPARTMENT_EMPLOYEEID"] == "")
                        command.Parameters.AddWithValue("@DEPARTMENT_EMPLOYEEID", SqlDbType.Int).Value = SqlInt32.Null;
                    else
                        command.Parameters.AddWithValue("@DEPARTMENT_EMPLOYEEID", SqlDbType.Int).Value = data["DEPARTMENT_EMPLOYEEID"];
                    command.Parameters.Add("@COMMENTS", SqlDbType.NVarChar).Value = data["COMMENTS"];

                    connection.Open();
                    command.CommandType = CommandType.Text;
                    int rows = command.ExecuteNonQuery();

                    // if no rows are affected, rollback REPORTS_META and RETURN FALSE
                    if (rows == 0)
                    {
                        {
                            try
                            {
                                using (SqlConnection con = new SqlConnection(con_str))
                                {
                                    using (SqlCommand com = new SqlCommand(
                                                 @"DELETE FROM REPORTS_META WHERE ID = " + reportid, connection))
                                    {
                                        con.Open();
                                        com.ExecuteNonQuery();
                                        return "";
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                string err = e.Message;
                                return "";
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }

            //reaching here means everything went fine
            if (!push_UserTrainingFolder(reportid, data["genid"], status: "RECOMMENDED_FOR_LEVEL"))
                return "";

            return reportid;
        }

        public static string push_RECOM_CERTIF(Dictionary<string, string> data)
        {
            //todo : implement

            string reportid = "";


            //todo: push into  REPORTS_META
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                             @"INSERT INTO REPORTS_META VALUES('6', '" + data["OJTI_ID"] + "', convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , " + data["OJTI_SIGNED"] + ", " + data["TRAINEE_SIGNED"] + " , " + data["TRAINEE_ID"] + ")   ; SELECT SCOPE_IDENTITY()", connection))
                {
                    connection.Open();
                    reportid = Convert.ToString(command.ExecuteScalar());
                    if (String.IsNullOrWhiteSpace(reportid))
                        return "";
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }


            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"    
                    INSERT INTO REPORT_RECOM_CERTIF 
                    (   ID   ,TRAINEE_ID   ,POSITION   ,SECTOR   ,PHASE  ,OJTI_ID   ,OJTI_SIGNED   ,TOTAL_HOURS    ,MER_MET   ,READING_SIGNED   
                            ,OBJECTIVES_SIGNED   ,FOLDER_COMPLETE   ,TRAINEE_SIGNED   ,TRAINEE_SIGN_DATE  ,[DATE]   ,
                        REVIEW_TEAM_APPROVAL   ,[REVIEW_TEAM_APPROVAL_SIGN_DATE]   ,COMMENTS  ,MEMBER1_ID  ,MEMBER1_SIGNED   ,MEMBER1_SIGN_DATE  
                            ,MEMBER2_ID  ,MEMBER2_SIGNED   ,MEMBER2_SIGN_DATE  ,MEMBER3_ID  ,MEMBER3_SIGNED   ,MEMBER3_SIGN_DATE    )   
                    VALUES 
                    (
                         @ID   ,@TRAINEE_ID   ,@POSITION   ,@SECTOR   ,@PHASE  ,@OJTI_ID   ,@OJTI_SIGNED   ,@TOTAL_HOURS    ,@MER_MET   ,@READING_SIGNED 
                            ,@OBJECTIVES_SIGNED   ,@FOLDER_COMPLETE   ,@TRAINEE_SIGNED   ,@TRAINEE_SIGN_DATE  ,@[DATE]   ,
                         @REVIEW_TEAM_APPROVAL   ,@[REVIEW_TEAM_APPROVAL_SIGN_DATE]   ,@COMMENTS  ,@MEMBER1_ID  ,@MEMBER1_SIGNED   ,@MEMBER1_SIGN_DATE 
                            ,@MEMBER2_ID  ,@MEMBER2_SIGNED   ,@MEMBER2_SIGN_DATE  ,@MEMBER3_ID  ,@MEMBER3_SIGNED   ,@MEMBER3_SIGN_DATE  
                    )", connection))
                {
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = reportid;
                    command.Parameters.Add("@TRAINEE_ID", SqlDbType.Int).Value = data["TRAINEE_ID"];
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = data["POSITION"];
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = data["SECTOR"];
                    command.Parameters.Add("@PHASE", SqlDbType.NVarChar).Value = data["PHASE"];
                    command.Parameters.Add("@OJTI_ID", SqlDbType.Int).Value = data["OJTI_ID"];
                    command.Parameters.Add("@TOTAL_HOURS", SqlDbType.NVarChar).Value = data["TOTAL_HOURS"];
                    command.Parameters.Add("@COMMENTS", SqlDbType.NVarChar).Value = data["COMMENTS"];
                    command.Parameters.Add("@DATE", SqlDbType.NVarChar).Value = data["DATE"];

                    command.Parameters.Add("@OJTI_SIGNED", SqlDbType.Bit).Value = data["OJTI_SIGNED"] == "1";
                    command.Parameters.Add("@MER_MET", SqlDbType.Bit).Value = data["MER_MET"] == "1";
                    command.Parameters.Add("@READING_SIGNED", SqlDbType.Bit).Value = data["READING_SIGNED"] == "1";
                    command.Parameters.Add("@OBJECTIVES_SIGNED", SqlDbType.Bit).Value = data["OBJECTIVES_SIGNED"] == "1";
                    command.Parameters.Add("@FOLDER_COMPLETE", SqlDbType.Bit).Value = data["FOLDER_COMPLETE"] == "1";
                    command.Parameters.Add("@TRAINEE_SIGNED", SqlDbType.Bit).Value = data["TRAINEE_SIGNED"] == "1";
                    command.Parameters.Add("@REVIEW_TEAM_APPROVAL", SqlDbType.Bit).Value = data["REVIEW_TEAM_APPROVAL"] == "1";
                    command.Parameters.Add("@MEMBER1_SIGNED", SqlDbType.Bit).Value = data["MEMBER1_SIGNED"] == "1";
                    command.Parameters.Add("@MEMBER2_SIGNED", SqlDbType.Bit).Value = data["MEMBER2_SIGNED"] == "1";
                    command.Parameters.Add("@MEMBER3_SIGNED", SqlDbType.Bit).Value = data["MEMBER3_SIGNED"] == "1";

                    command.Parameters.Add("@MEMBER1_SIGN_DATE", SqlDbType.NVarChar).Value = SqlString.Null;
                    command.Parameters.Add("@MEMBER2_SIGN_DATE", SqlDbType.NVarChar).Value = SqlString.Null;
                    command.Parameters.Add("@MEMBER3_SIGN_DATE", SqlDbType.NVarChar).Value = SqlString.Null;



                    if (data["REVIEW_TEAM_APPROVAL_SIGN_DATE"] == "")
                        command.Parameters.AddWithValue("@REVIEW_TEAM_APPROVAL_SIGN_DATE", SqlDbType.NVarChar).Value = SqlString.Null;
                    else
                        command.Parameters.AddWithValue("@REVIEW_TEAM_APPROVAL_SIGN_DATE", SqlDbType.NVarChar).Value = data["REVIEW_TEAM_APPROVAL_SIGN_DATE"];

                    if (data["TRAINEE_SIGN_DATE"] == "")
                        command.Parameters.AddWithValue("@TRAINEE_SIGN_DATE", SqlDbType.NVarChar).Value = SqlString.Null;
                    else
                        command.Parameters.AddWithValue("@TRAINEE_SIGN_DATE", SqlDbType.NVarChar).Value = data["TRAINEE_SIGN_DATE"];

                    if (data["MEMBER1_ID"] == "")
                        command.Parameters.AddWithValue("@MEMBER1_ID", SqlDbType.Int).Value = SqlInt32.Null;
                    else
                        command.Parameters.AddWithValue("@MEMBER1_ID", SqlDbType.Int).Value = data["MEMBER1_ID"];

                    if (data["MEMBER2_ID"] == "")
                        command.Parameters.AddWithValue("@MEMBER2_ID", SqlDbType.Int).Value = SqlInt32.Null;
                    else
                        command.Parameters.AddWithValue("@MEMBER2_ID", SqlDbType.Int).Value = data["MEMBER2_ID"];

                    if (data["MEMBER3_ID"] == "")
                        command.Parameters.AddWithValue("@MEMBER3_ID", SqlDbType.Int).Value = SqlInt32.Null;
                    else
                        command.Parameters.AddWithValue("@MEMBER3_ID", SqlDbType.Int).Value = data["MEMBER3_ID"];



                    connection.Open();
                    command.CommandType = CommandType.Text;
                    int rows = command.ExecuteNonQuery();

                    // if no rows are affected, rollback REPORTS_META and RETURN FALSE
                    if (rows == 0)
                    {
                        try
                        {
                            using (SqlConnection con = new SqlConnection(con_str))
                            using (SqlCommand com = new SqlCommand(
                                         @"DELETE FROM REPORTS_META WHERE ID = " + reportid, connection))
                            {
                                con.Open();
                                com.ExecuteNonQuery();
                                return "";
                            }
                        }
                        catch (Exception e)
                        {
                            string err = e.Message;
                            return "";
                        }
                    }
                }

            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }

            //reaching here means everything went fine
            if (!push_UserTrainingFolder(reportid, data["genid"], status: "RECOMMENDED_FOR_LEVEL"))
                return "";

            return reportid;
        }


        public static bool push_UserTrainingFolder(string reportid, string genid, string status = "REPORT")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                             @"INSERT INTO USER_TRAINING_FOLDER (STEPID , EMPLOYEEID, CREATED_TIME , [STATUS] , REPORTID , [FILENAME] , COMMENTS)
                                SELECT STEPID, EMPLOYEEID , convert(varchar, getutcdate(), 20) , @STATUS , @REPORTID , NULL, NULL
                                FROM USER_TRAINING_FOLDER WHERE GENID = @GENID", connection))
                {
                    connection.Open();

                    command.Parameters.Add("@REPORTID", SqlDbType.Int).Value = reportid;
                    command.Parameters.Add("@GENID", SqlDbType.Int).Value = genid;
                    command.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = status;
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

        #endregion


        #region PULL REPORT

        public static Dictionary<string, DataTable> pull_TR_ARE_APP_RAD(string reportid)
        {
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();



            // get the meta information
            try
            {
                DataTable meta = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT	RM.CREATER, 
		                                    UO.INITIAL + '-'+UO.FIRSTNAME + ' ' + UO.SURNAME AS CREATER_NAME,
		                                    RM.TRAINEE_ID,
		                                    UT.INITIAL + '-' + UT.FIRSTNAME + ' ' + UT.SURNAME AS TRAINEE_NAME,
		                                    CREATE_TIME,
		                                    OJTI_SIGNED,
		                                    TRAINEE_SIGNED
                                    FROM REPORTS_META RM
                                    JOIN USERS UO ON UO.EMPLOYEEID = RM.CREATER 
                                    JOIN USERS UT ON UT.EMPLOYEEID = RM.TRAINEE_ID
                                    WHERE RM.ID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(meta);

                        if (meta == null || meta.Rows.Count == 0)
                            return null;

                        result.Add("meta", meta);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }


            //get the form information
            try
            {
                DataTable form = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT * FROM REPORT_TR_ARE_APP_RAD WHERE ID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(form);

                        if (form == null || form.Rows.Count == 0)
                            return null;

                        result.Add("form", form);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }


            //critical skill results
            try
            {
                DataTable skills = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT CAT.CATEG_SKILL , RES.RESULT 
                                    FROM CRITICALSKILL_RESULTS RES 
                                    JOIN REPORT_SKILL_CATEGORIES CAT ON CAT.ID = CATEG_SKILL_ID AND CAT.REPORTTYPE = 1
                                    WHERE RES.REPORTID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(skills);

                        // might return 0 rows, why not
                        if (skills == null)
                            return null;

                        result.Add("skills", skills);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }

            return result;
        }


        public static Dictionary<string, DataTable> pull_TOWERTR_GMC_ADC(string reportid)
        {
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();



            // get the meta information
            try
            {
                DataTable meta = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT	RM.CREATER, 
		                                    UO.INITIAL + '-'+UO.FIRSTNAME + ' ' + UO.SURNAME AS CREATER_NAME,
		                                    RM.TRAINEE_ID,
		                                    UT.INITIAL + '-' + UT.FIRSTNAME + ' ' + UT.SURNAME AS TRAINEE_NAME,
		                                    CREATE_TIME,
		                                    OJTI_SIGNED,
		                                    TRAINEE_SIGNED
                                    FROM REPORTS_META RM
                                    JOIN USERS UO ON UO.EMPLOYEEID = RM.CREATER 
                                    JOIN USERS UT ON UT.EMPLOYEEID = RM.TRAINEE_ID
                                    WHERE RM.ID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(meta);

                        if (meta == null || meta.Rows.Count == 0)
                            return null;

                        result.Add("meta", meta);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }


            //get the form information
            try
            {
                DataTable form = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT * FROM REPORT_TOWERTR_GMC_ADC WHERE ID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(form);

                        if (form == null || form.Rows.Count == 0)
                            return null;

                        result.Add("form", form);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }


            //critical skill results
            try
            {
                DataTable skills = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT CAT.CATEG_SKILL , RES.RESULT 
                                    FROM CRITICALSKILL_RESULTS RES 
                                    JOIN REPORT_SKILL_CATEGORIES CAT ON CAT.ID = CATEG_SKILL_ID AND CAT.REPORTTYPE = 4
                                    WHERE RES.REPORTID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(skills);

                        // might return 0 rows, why not
                        if (skills == null)
                            return null;

                        result.Add("skills", skills);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }

            return result;
        }


        public static Dictionary<string, DataTable> pull_DAILYTR_ASS_RAD(string reportid)
        {
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();

            // get the meta information
            try
            {
                DataTable meta = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT	RM.CREATER, 
		                                    UO.INITIAL + '-'+UO.FIRSTNAME + ' ' + UO.SURNAME AS CREATER_NAME,
		                                    RM.TRAINEE_ID,
		                                    UT.INITIAL + '-' + UT.FIRSTNAME + ' ' + UT.SURNAME AS TRAINEE_NAME,
		                                    CREATE_TIME,
		                                    OJTI_SIGNED,
		                                    TRAINEE_SIGNED
                                    FROM REPORTS_META RM
                                    JOIN USERS UO ON UO.EMPLOYEEID = RM.CREATER 
                                    JOIN USERS UT ON UT.EMPLOYEEID = RM.TRAINEE_ID
                                    WHERE RM.ID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(meta);

                        if (meta == null || meta.Rows.Count == 0)
                            return null;

                        result.Add("meta", meta);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }


            //get the form information
            try
            {
                DataTable form = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT * FROM REPORT_DAILYTR_ASS_RAD WHERE ID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(form);

                        if (form == null || form.Rows.Count == 0)
                            return null;

                        result.Add("form", form);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }


            //critical skill results
            try
            {
                DataTable skills = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT CAT.CATEG_SKILL , RES.RESULT 
                                    FROM CRITICALSKILL_RESULTS RES 
                                    JOIN REPORT_SKILL_CATEGORIES CAT ON CAT.ID = CATEG_SKILL_ID AND CAT.REPORTTYPE = 3
                                    WHERE RES.REPORTID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(skills);

                        // might return 0 rows, why not
                        if (skills == null)
                            return null;

                        result.Add("skills", skills);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }

            return result;
        }


        public static Dictionary<string, DataTable> pull_DAILYTR_ASS_TWR(string reportid)
        {
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();



            // get the meta information
            try
            {
                DataTable meta = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT	RM.CREATER, 
		                                    UO.INITIAL + '-'+UO.FIRSTNAME + ' ' + UO.SURNAME AS CREATER_NAME,
		                                    RM.TRAINEE_ID,
		                                    UT.INITIAL + '-' + UT.FIRSTNAME + ' ' + UT.SURNAME AS TRAINEE_NAME,
		                                    CREATE_TIME,
		                                    OJTI_SIGNED,
		                                    TRAINEE_SIGNED
                                    FROM REPORTS_META RM
                                    JOIN USERS UO ON UO.EMPLOYEEID = RM.CREATER 
                                    JOIN USERS UT ON UT.EMPLOYEEID = RM.TRAINEE_ID
                                    WHERE RM.ID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(meta);

                        if (meta == null || meta.Rows.Count == 0)
                            return null;

                        result.Add("meta", meta);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }


            //get the form information
            try
            {
                DataTable form = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT * FROM REPORT_DAILYTR_ASS_TWR WHERE ID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(form);

                        if (form == null || form.Rows.Count == 0)
                            return null;

                        result.Add("form", form);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }


            //critical skill results
            try
            {
                DataTable skills = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT CAT.CATEG_SKILL , RES.RESULT 
                                    FROM CRITICALSKILL_RESULTS RES 
                                    JOIN REPORT_SKILL_CATEGORIES CAT ON CAT.ID = CATEG_SKILL_ID AND CAT.REPORTTYPE = 2
                                    WHERE RES.REPORTID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(skills);

                        // might return 0 rows, why not
                        if (skills == null)
                            return null;

                        result.Add("skills", skills);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }

            return result;
        }

        public static Dictionary<string, DataTable> pull_RECOM_LEVEL(string reportid)
        {
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();

            // get the meta information
            try
            {
                DataTable meta = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT	RM.CREATER, 
		                                    UO.INITIAL + '-'+UO.FIRSTNAME + ' ' + UO.SURNAME AS CREATER_NAME,
		                                    RM.TRAINEE_ID,
		                                    UT.INITIAL + '-' + UT.FIRSTNAME + ' ' + UT.SURNAME AS TRAINEE_NAME,
		                                    CREATE_TIME,
		                                    OJTI_SIGNED,
		                                    TRAINEE_SIGNED
                                    FROM REPORTS_META RM
                                    JOIN USERS UO ON UO.EMPLOYEEID = RM.CREATER 
                                    JOIN USERS UT ON UT.EMPLOYEEID = RM.TRAINEE_ID
                                    WHERE RM.ID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(meta);

                        if (meta == null || meta.Rows.Count == 0)
                            return null;

                        result.Add("meta", meta);
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }


            //get the form information
            try
            {
                DataTable form = new DataTable();
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT * FROM REPORT_RECOM_LEVEL WHERE ID = " + reportid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(form);

                        if (form == null || form.Rows.Count == 0)
                            return null;

                        result.Add("form", form);

                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }



            return null;
        }

        #endregion



        public static bool update_TraineeSigned(string reportid, string studentcomment, string type)
        {

            string rpt_tablename = "";

            if (type == "1")
                rpt_tablename = "REPORT_TR_ARE_APP_RAD";
            if (type == "2")
                rpt_tablename = "REPORT_DAILYTR_ASS_TWR";
            if (type == "3")
                rpt_tablename = "REPORT_DAILYTR_ASS_RAD";
            else if (type == "4")
                rpt_tablename = "REPORT_TOWEETR_GMC_ADC";

            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" UPDATE REPORTS_META SET TRAINEE_SIGNED = 1 , STATUS = 'TRAINEESIGNED' WHERE ID = @REPORTID ;
                                        UPDATE " + rpt_tablename + " SET STUDENT_COMMENTS = @STUDENTCOMMENT WHERE ID = @REPORTID ", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@REPORTID", SqlDbType.Int).Value = reportid;
                        command.Parameters.Add("@STUDENTCOMMENT", SqlDbType.NVarChar).Value = studentcomment;

                        command.CommandType = CommandType.Text;
                        int rows = command.ExecuteNonQuery();

                        if (rows == 0)
                            return false;

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }

        public static bool update_Sign_RECOM_LEVEL(string reportid, string whosigns, string signerid)
        {
            string update = "";

            if (whosigns == "trainee")
                update = @"UPDATE REPORTS_META SET TRAINEE_SIGNED = 1 , STATUS = 'TRAINEESIGNED' WHERE ID = @REPORTID ;
                           UPDATE  REPORT_RECOM_LEVEL SET TRAINEE_SIGNED = 1 WHERE ID = @REPORTID";
            else if (whosigns == "department")
                update = @"UPDATE REPORTS_META SET TRAINEE_SIGNED = 1 , STATUS = 'TRAINEESIGNED' WHERE ID = @REPORTID ;
                           UPDATE  REPORT_RECOM_LEVEL SET DEPARTMENT_SIGNED = 1 , DEPARTMENT_EMPLOYEEID = " + signerid + " WHERE ID = @REPORTID";

            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(update, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@REPORTID", SqlDbType.Int).Value = reportid;
                    command.CommandType = CommandType.Text;
                    int rows = command.ExecuteNonQuery();

                    if (rows == 0)
                        return false;

                    return true;
                }

            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }




        public static DataTable get_myCreatedReports(string employeeid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT RM.ID AS 'Rpt No', RT.[NAME] AS 'Type' ,  
		                                    convert(varchar, CREATE_TIME, 20) AS 'Created' , 
		                                    [STATUS] AS 'Status', 
                                            U.FIRSTNAME + ' '  + U.SURNAME AS 'Trainee',
		                                    TRAINEE_SIGNED AS 'Tr. Sign' 
                                 FROM REPORTS_META RM
                                 JOIN REPORT_TYPES RT ON RM.[TYPE] = RT.ID
                                 JOIN USERS U ON U.EMPLOYEEID = RM.TRAINEE_ID
                                 WHERE RM.CREATER = " + employeeid +
                                " ORDER BY CREATED DESC ", connection))
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

        public static DataTable get_my_training_Reports(string employeeid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT	RM.ID ,
		                                    RT.NAME AS 'Type',
		                                    RM.CREATE_TIME AS 'Created',
		                                    UO.FIRSTNAME + ' ' + UO.SURNAME AS 'OJTI',
		                                    RM.STATUS AS 'Status',
		                                    RM.TRAINEE_SIGNED AS 'My Sign'
                                    FROM REPORTS_META RM
                                    JOIN USERS UO ON UO.EMPLOYEEID = RM.CREATER
                                    JOIN REPORT_TYPES RT ON RM.[TYPE] = RT.ID
                                    WHERE RM.TRAINEE_ID = " + employeeid +
                                                       " ORDER BY RM.CREATE_TIME DESC ", connection))
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


        public static string get_Report_Type(string reportid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                 @"SELECT RT.[TYPE] FROM REPORTS_META RM
                                        JOIN REPORT_TYPES RT ON RT.ID = RM.[TYPE]
                                        WHERE RM.ID = " + reportid, connection))
                    {
                        connection.Open();
                        string result = Convert.ToString(command.ExecuteScalar());
                        if (String.IsNullOrWhiteSpace(result))
                            return null;
                        else
                            return result;

                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return null;
        }

        public static string get_Relation_to_Report(string reportid, string employeeid)
        {
            string result = ""; //creator_ojti, trainee, nobody
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                 @" SELECT CASE
                                        WHEN CREATER = @employeeid THEN 'creater_ojti'
                                        WHEN TRAINEE_ID = @employeeid THEN 'trainee'
                                        ELSE 'nobody'
                                    END AS QuantityText from REPORTS_META 
                                    WHERE ID = @reportid AND ( CREATER = @employeeid OR TRAINEE_ID = @employeeid) ", connection))
                    {
                        command.Parameters.Add("@reportid", SqlDbType.Int).Value = reportid;
                        command.Parameters.Add("@employeeid", SqlDbType.Int).Value = employeeid;

                        connection.Open();
                        result = Convert.ToString(command.ExecuteScalar());
                        if (String.IsNullOrWhiteSpace(result))
                            return "nobody";
                        else
                            return result;

                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return "nobody";
        }




        public static DataTable get_Training_Steps_of_Position(string position, string sector)
        {

            DataTable result = new DataTable(); //ID, DESCRIPTION

            if (position == "TWR")
            {
                result = get_TrainingSteps_TWR(sector);
                return result;
            }
            else if (position == "ACC")
            {
                result = get_TrainingSteps_ACC(sector);
                return result;
            }
            else if (position == "APP")
            {
                result = get_TrainingSteps_APP(sector);
                return result;
            }

            //todo : add other posiitions

            return null;

        }
        public static DataTable get_TrainingSteps_APP(string sector)
        {

            DataTable result_sector = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
SELECT [ID] , [DESCRIPTION] 
FROM TRAINING_TREE_STEPS WHERE POSITION = @POSITION AND SECTOR =@SECTOR ORDER BY ID ASC ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = "APP";
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;

                    command.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(result_sector);

                    if (result_sector == null || result_sector.Rows.Count == 0)
                        return null;
                }

            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }
            //res_assist.Merge(result_sector);
            return result_sector;
        }
        public static DataTable get_TrainingSteps_ACC(string sector)
        {

            DataTable result_sector = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
SELECT [ID] , [DESCRIPTION] 
FROM TRAINING_TREE_STEPS WHERE POSITION = @POSITION AND SECTOR =@SECTOR ORDER BY ID ASC", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = "ACC";
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;

                    command.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(result_sector);

                    if (result_sector == null || result_sector.Rows.Count == 0)
                        return null;
                }

            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }
            //res_assist.Merge(result_sector);
            return result_sector;
        }
        public static DataTable get_TrainingSteps_TWR(string sector)
        {

            DataTable result_sector = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"  
                                SELECT [ID] , [DESCRIPTION] 
                                FROM TRAINING_TREE_STEPS 
                                WHERE POSITION = @POSITION 
                                AND 1 = CASE WHEN @POSITION = 'TWR' AND @SECTOR = 'ASSIST' AND PHASE = @SECTOR
						                                THEN 1
				                                   WHEN SECTOR = @SECTOR
						                                THEN 1
				                                   ELSE 0 END
                                ORDER BY ID ASC", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = "TWR";
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;

                    command.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(result_sector);

                    if (result_sector == null || result_sector.Rows.Count == 0)
                        return null;
                }

            }
            catch (Exception e)
            {
                string err = e.Message;
                return null;
            }
            //res_assist.Merge(result_sector);
            return result_sector;
        }







        //todo : update this
        public static DataTable get_Training_Folder(string employeeid, string position, string sector)
        {
            DataTable res = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @"  SELECT TTS.DESCRIPTION , UTF.STATUS , UTF.CREATED_TIME, ISNULL(UTF.REPORTID,'') AS [Rpt.Num.] , ISNULL(UTF.FILENAME,'') AS [FileName]
		                                , UTF.GENID, TTS.PHASE , TTS.[NAME]
                                        FROM USER_TRAINING_FOLDER UTF
                                        JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID
                                        WHERE UTF.EMPLOYEEID = @EMPLOYEEID 
	                                          AND TTS.POSITION = @POSITION 
	                                          AND 1 = CASE WHEN @POSITION = 'TWR' AND @SECTOR = 'ASSIST' AND TTS.PHASE = @SECTOR
						                                        THEN 1
				                                           WHEN TTS.SECTOR = @SECTOR
						                                        THEN 1
				                                           ELSE 0 END
                                        ORDER BY TTS.ID DESC, UTF.GENID ASC", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.NVarChar).Value = employeeid;
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;

                    command.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(res);

                    if (res == null || res.Rows.Count == 0)
                        return new DataTable();

                    return res;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }




        public static bool has_Prior_TrainingFolder(string employeeid, string position, string sector)
        {
            if (position == "TWR")
            {
                return has_Prior_TrainingFolder_TWR(employeeid, sector);
            }
            else if (position == "ACC")
            {
                return has_Prior_TrainingFolder_ACC(employeeid, sector);
            }
            else if (position == "APP")
            {
                return has_Prior_TrainingFolder_APP(employeeid, sector);
            }


            return false;
        }

        public static bool has_Prior_TrainingFolder_APP(string employeeid, string sector)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                              @"
                                SELECT TOP 1 UTF.GENID FROM USER_TRAINING_FOLDER UTF
                                JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID 
                                WHERE UTF.EMPLOYEEID = @EMPLOYEEID AND TTS.POSITION = @POSITION AND TTS.SECTOR = @SECTOR", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                        command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = "APP";
                        command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                        command.CommandType = CommandType.Text;
                        //genID is the return, maybe it will be used lated
                        if (String.IsNullOrEmpty(Convert.ToString(command.ExecuteScalar())))
                            return false; //meaning, no prior record for the sector

                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message; //error means, there is prior training folder, prevents further actions (until i fix whatever is wrong)
            }

            return true;
        }

        public static bool has_Prior_TrainingFolder_ACC(string employeeid, string sector)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                              @" 
                                SELECT TOP 1 UTF.GENID FROM USER_TRAINING_FOLDER UTF
                                JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID 
                                WHERE UTF.EMPLOYEEID = @EMPLOYEEID AND TTS.POSITION = @POSITION AND TTS.SECTOR = @SECTOR", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                        command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = "ACC";
                        command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                        command.CommandType = CommandType.Text;
                        //genID is the return, maybe it will be used lated
                        if (String.IsNullOrEmpty(Convert.ToString(command.ExecuteScalar())))
                            return false; //meaning, no prior record for the sector

                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message; //error means, there is prior training folder, prevents further actions (until i fix whatever is wrong)
            }

            return true;
        }

        public static bool has_Prior_TrainingFolder_TWR(string employeeid, string sector)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                              @" 
                                SELECT TOP 1 UTF.GENID FROM USER_TRAINING_FOLDER UTF
                                JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID 
                                WHERE UTF.EMPLOYEEID = @EMPLOYEEID AND TTS.POSITION = @POSITION AND TTS.SECTOR = @SECTOR", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                        command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = "TWR";
                        command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                        command.CommandType = CommandType.Text;
                        //genID is the return, maybe it will be used lated
                        if (String.IsNullOrEmpty(Convert.ToString(command.ExecuteScalar())))
                            return false; //meaning, no prior record for the sector

                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message; //error means, there is prior training folder, prevents further actions (until i fix whatever is wrong)
            }

            return true;
        }





        public static bool start_Training_Folder(string employeeid, string position, string sector, string stepid)
        {

            if (position == "TWR")
            {
                if (start_folder_TWR(employeeid, sector, stepid))
                    return true;
            }
            else if (position == "ACC")
            {
                if (start_folder_ACC(employeeid, sector, stepid))
                    return true;
            }
            else if (position == "APP")
            {
                if (start_folder_APP(employeeid, sector, stepid))
                    return true;
            }

            return false;
        }

        public static bool start_folder_APP(string employeeid, string sector, string stepid)
        {
            //ADD FDO AND ASSIST IF THEY DONT EXIST 
            // (BECAUSE EVERY SECTOR FOLDER CREATED SEPERATELY, FDO AND ASSIST IS CREATED ONCE)
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @" IF NOT EXISTS (
                                            SELECT TOP 1 TTS.ID  
                                            FROM USER_TRAINING_FOLDER UTF
                                            JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID
                                            WHERE UTF.EMPLOYEEID = @EMPLOYEEID 
                                                 AND TTS.POSITION = 'APP' 
                                                 AND ISNULL(TTS.SECTOR,'') = '' 
                                                 AND TTS.PHASE IN ('FDO', 'ASSIST')
                                          )
                            BEGIN
                                INSERT INTO USER_TRAINING_FOLDER 
                                SELECT TTS.ID, @EMPLOYEEID, CONVERT(VARCHAR, GETUTCDATE(),20),'MIGRATION', NULL, NULL,NULL
                                FROM TRAINING_TREE_STEPS TTS 
                                WHERE TTS.POSITION = 'APP' 
                                        AND ISNULL(TTS.SECTOR,'') = '' 
                                        AND TTS.PHASE IN ('FDO', 'ASSIST')
                            END", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                string mes = e.Message;
            }



            try
            {
                //insert all steps before chosen step in the sector  as MIGRATION
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                                 @" INSERT INTO USER_TRAINING_FOLDER (STEPID , EMPLOYEEID, CREATED_TIME, [STATUS] , REPORTID , [FILENAME] , COMMENTS)
                                    SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'MIGRATION' , NULL, NULL, NULL 
                                    FROM TRAINING_TREE_STEPS TTS 
                                    WHERE TTS.POSITION = 'APP' 
                                            AND TTS.SECTOR = @SECTOR 
                                            AND TTS.ID < @STEPID   ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                }

                // insert chosen step as ONGOING
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                                 @" INSERT INTO USER_TRAINING_FOLDER (STEPID , EMPLOYEEID, CREATED_TIME, [STATUS] , REPORTID , [FILENAME] , COMMENTS)
                                    SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'ONGOING' , NULL, NULL, NULL 
                                    FROM TRAINING_TREE_STEPS TTS 
                                    WHERE TTS.POSITION = 'APP' 
                                            AND TTS.SECTOR = @SECTOR 
                                            AND  TTS.ID = @STEPID ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() != 1)
                        return false; //something went wrong.
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return false;
            }

            return true;
        }

        public static bool start_folder_ACC(string employeeid, string sector, string stepid)
        {
            //ADD FDO AND ASSIST IF THEY DONT EXIST 
            // (BECAUSE EVERY SECTOR FOLDER CREATED SEPERATELY, FDO AND ASSIST IS CREATED ONCE)
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @" IF NOT EXISTS (
                                            SELECT TOP 1 TTS.ID  
                                            FROM USER_TRAINING_FOLDER UTF
                                            JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID
                                            WHERE UTF.EMPLOYEEID = @EMPLOYEEID 
                                                 AND TTS.POSITION = 'ACC' 
                                                 AND ISNULL(TTS.SECTOR,'') = '' 
                                                 AND TTS.PHASE IN ('FDO', 'ASSIST')
                                          )
                            BEGIN
                                INSERT INTO USER_TRAINING_FOLDER 
                                SELECT TTS.ID, @EMPLOYEEID, CONVERT(VARCHAR, GETUTCDATE(),20),'MIGRATION', NULL, NULL,NULL
                                FROM TRAINING_TREE_STEPS TTS 
                                WHERE TTS.POSITION = 'ACC' 
                                        AND ISNULL(TTS.SECTOR,'') = '' 
                                        AND TTS.PHASE IN ('FDO', 'ASSIST')
                            END", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                string mes = e.Message;
            }


            try
            {
                //insert all steps before chosen step in the sector  as MIGRATION
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                                 @"  INSERT INTO USER_TRAINING_FOLDER (STEPID , EMPLOYEEID, CREATED_TIME, [STATUS] , REPORTID , [FILENAME] , COMMENTS)
                                    SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'MIGRATION' , NULL, NULL, NULL FROM TRAINING_TREE_STEPS TTS WHERE TTS.POSITION = 'ACC' AND TTS.SECTOR = @SECTOR AND TTS.ID < @STEPID   ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                }

                // insert chosen step as ONGOING
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                                 @"  INSERT INTO USER_TRAINING_FOLDER (STEPID , EMPLOYEEID, CREATED_TIME, [STATUS] , REPORTID , [FILENAME] , COMMENTS)
                                    SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'ONGOING' , NULL, NULL, NULL FROM TRAINING_TREE_STEPS TTS WHERE TTS.POSITION = 'ACC' AND TTS.SECTOR = @SECTOR AND  TTS.ID = @STEPID ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() != 1)
                        return false; //something went wrong.
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return false;
            }

            return true;
        }

        public static bool start_folder_TWR(string employeeid, string sector, string stepid)
        {

            //first add FDO step, it is always inserted
            //using (SqlConnection connection = new SqlConnection(con_str))
            //using (SqlCommand command = new SqlCommand(
            //        @" DECLARE @FDOID INT = (SELECT ID FROM TRAINING_TREE_STEPS WHERE POSITION ='TWR' AND PHASE = 'FDO')
            //                INSERT INTO USER_TRAINING_FOLDER VALUES (@FDOID , @EMPLOYEEID , convert(varchar, getutcdate(), 20) , 'MIGRATION' , NULL, NULL, NULL) ", connection))
            //{
            //    connection.Open();
            //    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
            //    command.CommandType = CommandType.Text;
            //    if (command.ExecuteNonQuery() != 1)
            //        return false; //something went wrong
            //}

            try
            {
                if (sector == "GMC" || sector == "ADC")
                {
                    // add ASIST as MIGRATION

                    using (SqlConnection connection = new SqlConnection(con_str))
                    using (SqlCommand command = new SqlCommand(
                            @" DECLARE @ASSISTID INT = (SELECT ID FROM TRAINING_TREE_STEPS WHERE POSITION ='TWR' AND PHASE = 'ASSIST')
                                INSERT INTO USER_TRAINING_FOLDER VALUES (@ASSISTID , @EMPLOYEEID , convert(varchar, getutcdate(), 20) , 'MIGRATION' , NULL, NULL, NULL)  ", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                        command.CommandType = CommandType.Text;

                        if (command.ExecuteNonQuery() != 1)
                            return false; //something went wrong
                    }
                }
            }
            catch (Exception e)
            {
                string mes = e.Message;
            }




            try
            {
                //insert all steps before chosen step in the sector  as MIGRATION
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                                 @"  INSERT INTO USER_TRAINING_FOLDER (STEPID , EMPLOYEEID, CREATED_TIME, [STATUS] , REPORTID , [FILENAME] , COMMENTS)
                                    SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'MIGRATION' , NULL, NULL, NULL FROM TRAINING_TREE_STEPS TTS WHERE TTS.POSITION = 'TWR' AND TTS.SECTOR = @SECTOR AND TTS.ID < @STEPID   ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                }

                // insert chosen step as ONGOING
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                                 @"  INSERT INTO USER_TRAINING_FOLDER (STEPID , EMPLOYEEID, CREATED_TIME, [STATUS] , REPORTID , [FILENAME] , COMMENTS)
                                        SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'ONGOING' , NULL, NULL, NULL 
                                        FROM TRAINING_TREE_STEPS TTS 
                                        WHERE TTS.POSITION = 'TWR' AND  TTS.ID = @STEPID 
                                        AND 1 = CASE WHEN @SECTOR = 'ASSIST' AND TTS.PHASE = @SECTOR
                                                                THEN 1
                                                           WHEN TTS.SECTOR = @SECTOR
                                                                THEN 1
                                                           ELSE 0 END", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() != 1)
                        return false; //something went wrong.
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return false;
            }

            return true;
        }




        public static bool update_totalhours(string userid, string sector, string totalhours, string lastupdate, string extra = "")
        {
            try
            {
                //insert all steps before chosen step in the sector  as MIGRATION
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                      @"IF EXISTS (SELECT TOP 1 TOTALHOURS FROM USER_TOTALHOURS WHERE USERID= @USERID AND SECTOR = @SECTOR)
                        BEGIN
		                        UPDATE USER_TOTALHOURS 
		                        SET TOTALHOURS = @TOTALHOURS ,
			                        LASTUPDATE = @LASTUPDATE ,
			                        LASTUPDATE_TIME = CONVERT(VARCHAR, GETUTCDATE(), 20 ),
			                        EXTRA = CASE WHEN @EXTRA = '' THEN EXTRA ELSE @EXTRA END,
                                    [BY] = @BY
		                        WHERE USERID=@USERID AND SECTOR =@SECTOR
                        END
                        ELSE
	                        INSERT INTO USER_TOTALHOURS 
	                        VALUES (@USERID, @SECTOR, @TOTALHOURS , @LASTUPDATE, CONVERT(VARCHAR, GETUTCDATE(),20), @EXTRA, @BY)", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = userid;
                    command.Parameters.Add("@SECTOR", SqlDbType.VarChar).Value = sector;
                    command.Parameters.Add("@TOTALHOURS", SqlDbType.VarChar).Value = totalhours;
                    command.Parameters.Add("@LASTUPDATE", SqlDbType.VarChar).Value = lastupdate;
                    command.Parameters.Add("@EXTRA", SqlDbType.NVarChar).Value = extra;
                    UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];
                    command.Parameters.Add("@BY", SqlDbType.Int).Value = user.employeeid;
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




        public static DataTable UserDetails_TrainingFolder(string userid)
        {
            DataTable res = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT DISTINCT TTS.ID, TTS.POSITION, TTS.SECTOR, TTS.PHASE, TTS.DESCRIPTION, 
		                                    UTF.STATUS
                                    FROM USER_TRAINING_FOLDER UTF
                                    JOIN TRAINING_TREE_STEPS TTS ON UTF.STEPID = TTS.ID
                                    WHERE EMPLOYEEID = @USERID AND UTF.STATUS <> 'REPORT'
                                    ORDER BY TTS.ID ASC", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@USERID", SqlDbType.Int).Value = userid;

                        command.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(res);

                        if (res == null || res.Rows.Count == 0)
                            return new DataTable();

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

        public static string get_TOTALHOURS(string trainee, string sector)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                             @"  SELECT TOTALHOURS FROM USER_TOTALHOURS WHERE USERID = @TRAINEE AND SECTOR = @SECTOR
                                    ", connection))
                {
                    command.Parameters.Add("@TRAINEE", SqlDbType.Int).Value = trainee;
                    command.Parameters.Add("@SECTOR", SqlDbType.VarChar).Value = sector;

                    connection.Open();
                    string result = Convert.ToString(command.ExecuteScalar());
                    if (String.IsNullOrWhiteSpace(result))
                        return "00:00";
                    else
                        return result;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return "";
        }

        public static string get_MER(string stepid, string employeeid = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                             @"DECLARE @MER INT = (  SELECT UM.MER FROM TRAINING_TREE_STEPS TTS
					                                  JOIN MER_USER UM ON UM.POSITION = TTS.POSITION 
											                                AND ISNULL(UM.SECTOR,'') = ISNULL(TTS.SECTOR,'')
											                                AND TTS.PHASE = UM.PHASE
					                                  WHERE TTS.ID = @STEPID AND UM.EMPLOYEEID = @EMPLOYEEID
					                                )

                                IF ISNULL(@MER,0) = 0
                                BEGIN
		                                SELECT MD.MER FROM TRAINING_TREE_STEPS TTS
		                                JOIN MER_DEFAULT MD ON MD.POSITION = TTS.POSITION 
								                                AND ISNULL(MD.SECTOR,'') = ISNULL(TTS.SECTOR,'')
								                                AND TTS.PHASE = MD.PHASE
		                                WHERE TTS.ID = @STEPID
                                END
                                ELSE 
                                        SELECT @MER", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid == "" ? "0" : employeeid;

                    command.CommandType = CommandType.Text;

                    string result = Convert.ToString(command.ExecuteScalar() as object);
                    if (result == "")
                        return null;
                    else
                        return result;

                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }

        public static DataTable get_MERs(string employeeid, string position = "", string sector = "", string stepid = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @"  SELECT ISNULL(MU.MER,'0') AS 'Current' , 
                                     TTS.POSITION , TTS.SECTOR, TTS.DESCRIPTION , 
                                     TTS.ID
                                 FROM TRAINING_TREE_STEPS TTS
                                 LEFT JOIN MER_USER MU ON TTS.ID = MU.STEPID AND  MU.EMPLOYEEID = @EMPLOYEEID
                                 WHERE 
                                     TTS.ID = CASE WHEN @STEPID = 0 THEN TTS.ID ELSE @STEPID END
                                     AND 
                                     TTS.POSITION = CASE WHEN @POSITION = '' THEN TTS.POSITION ELSE @POSITION END
                                     AND 
                                     TTS.SECTOR = CASE WHEN @SECTOR = '' THEN TTS.SECTOR ELSE @SECTOR END", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid == "" ? "0" : stepid;
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;

                    command.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable res = new DataTable();
                    da.Fill(res);

                    if (res != null || res.Rows.Count > 0)
                        return res;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }

        public static bool update_MER(string stepid, string MER, string employeeid , string comments = "")
        {
            //TODO : EMPLOYEEID WOULDNT UPDATE SINCE THERE IS NO PRIOR RECORD. MUST INSERT FIRST
            UserSession user = (UserSession)HttpContext.Current.Session["usersession"];
            
            try
            {

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                        IF NOT EXISTS (SELECT * FROM MER_USER WHERE EMPLOYEEID = @EMPLOYEEID AND STEPID = @STEPID)
	                        INSERT INTO MER_USER 
	                        VALUES( @EMPLOYEEID, @STEPID , @MER, @ADMINID, CONVERT(VARCHAR,GETUTCDATE(),20),@COMMENTS)
                        ELSE
	                        UPDATE MER_USER
	                        SET MER=@MER , DEFINED_BY = @ADMINID, COMMENTS = @COMMENTS, DEFINED_TIME = CONVERT(VARCHAR,GETUTCDATE(),20)
	                        WHERE EMPLOYEEID = @EMPLOYEEID AND STEPID = @STEPID", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.Parameters.Add("@MER", SqlDbType.Int).Value = MER;
                    command.Parameters.Add("@ADMINID", SqlDbType.Int).Value = user.employeeid;
                    command.Parameters.Add("@COMMENTS", SqlDbType.VarChar).Value = comments;
                    
                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string me = e.Message;
            }
            return false;
        }


        public static DataTable get_Level_Objectives(string employeeid, DataRow row)
        {
            string position = row["POSITION"].ToString();
            string sector = row["SECTOR"].ToString();
            string phase = row["PHASE"].ToString();

            if (sector == "")
                sector = phase; // TWR ASSIST HAS TO BE HANDLED

            string type = "";
            if (sector == "GMC")
                type = "TWR_GMC";
            else if (sector == "ADC")
                type = "TWR_ADC";
            else if (sector == "ASSIST")
                type = "TWR_ASSIST";
            else if (new string[6] { "AR", "BR", "KR", "NR", "SR", "CR" }.Contains(sector))
                type = "AREA_APPROACH";

            string level = "";
            if (phase.Contains("LEVEL1"))
                level = "1";
            else if (phase.Contains("LEVEL2"))
                level = "2";
            else if (phase.Contains("LEVEL3"))
                level = "3";
            else if (sector == "ASSIST" && phase == "ASSIST") //TWR ASSIST
                level = "0";


            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                 @" SELECT DEF.ID, DEF.CATEGORY , DEF.HEADER, DEF.OBJECTIVE ,
	                     ISNULL(US.INITIAL,'') as 'By' , F.ID as 'FORMID'
                 FROM LEVEL_OBJECTIVES_FORM F
                 JOIN LEVEL_OBJECTIVES_DEF DEF ON DEF.FORMID = F.ID
                 LEFT JOIN LEVEL_OBJECTIVES_USER U ON U.OBJECTIVEID = DEF.ID AND U.USERID = @USERID
                 LEFT JOIN USERS US ON US.EMPLOYEEID = U.SIGNEDBY
                 WHERE F.TYPE = @TYPE AND F.LEVELNUM = @LEVEL AND F.ISACTIVE = 1 AND DEF.ISACTIVE = 1", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@TYPE", SqlDbType.VarChar).Value = type;
                    command.Parameters.Add("@LEVEL", SqlDbType.VarChar).Value = level;
                    command.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable res = new DataTable();
                    da.Fill(res);

                    if (res != null || res.Rows.Count > 0)
                        return res;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }

        public static DataTable get_ONGOING_step(string employeeid, string sector)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @" SELECT TTS.POSITION, TTS.SECTOR, TTS.PHASE, TTS.ID, UTF.GENID
                                FROM USER_TRAINING_FOLDER UTF
                                JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID
                                WHERE UTF.EMPLOYEEID = @EMPLOYEEID AND UTF.STATUS = 'ONGOING'
                                AND 1 = CASE WHEN @SECTOR = 'ASSIST' AND TTS.POSITION='TWR' AND TTS.PHASE = 'ASSIST' THEN 1
			                                 WHEN TTS.SECTOR = @SECTOR THEN 1
			                                 ELSE 0 END ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@SECTOR", SqlDbType.VarChar).Value = sector;
                    command.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable res = new DataTable();
                    da.Fill(res);

                    if (res != null || res.Rows.Count > 0)
                        return res;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }
        public static DataTable get_ONGOING_steps(string employeeid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @" SELECT TTS.POSITION, ISNULL(TTS.SECTOR,'') AS 'SECTOR', TTS.PHASE, TTS.ID, UTF.GENID
                                FROM USER_TRAINING_FOLDER UTF
                                JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID
                                WHERE UTF.EMPLOYEEID = @EMPLOYEEID AND UTF.STATUS = 'ONGOING'  ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable res = new DataTable();
                    da.Fill(res);

                    if (res != null || res.Rows.Count > 0)
                        return res;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }

        public static bool sign_unsign_objective(string employeeid, string formid, string objectiveid, string sign)
        {
            UserSession user = (UserSession)HttpContext.Current.Session["usersession"];

            try
            {

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(@"
                       DELETE FROM LEVEL_OBJECTIVES_USER WHERE USERID=@EMPLOYEEID AND FORMID=@FORMID AND OBJECTIVEID=@OBJID
                       IF @SIGN = 1
                            INSERT INTO LEVEL_OBJECTIVES_USER VALUES(@EMPLOYEEID, @FORMID,@OBJID,@OJTIID,CONVERT(VARCHAR,GETUTCDATE(),20) )", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@FORMID", SqlDbType.Int).Value = formid;
                    command.Parameters.Add("@OBJID", SqlDbType.Int).Value = objectiveid;
                    command.Parameters.Add("@OJTIID", SqlDbType.Int).Value = user.employeeid;
                    command.Parameters.Add("@SIGN", SqlDbType.Int).Value = sign;
                    

                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                string me = e.Message;
            }
            return false;
        }
    }
}