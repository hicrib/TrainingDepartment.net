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

        #region PUSH REPORT
        public static void rollback_push_report(string reportid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Con_Str.current))
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

                

                string result = data["ASSESS_PASSED"] == null ? "REPORT" : ( data["ASSESS_PASSED"].Contains("PASSED")  ? "Assessment Passed" : "Assessment Failed");
                if (!push_UserTrainingFolder(reportid, data["genid"], status: result))
                {
                    rollback_push_report(reportid);
                    return "";
                }

                //todo : there is no rollback of usertrainingfolder when this fails. Rest is rolledback
                if (!update_totalhours(data["TRAINEE_ID"], data["stepid"], data["TOTAL_HOURS"], "REPORT:" + reportid))
                {
                    rollback_push_report(reportid);
                    return "";
                }

                //if it was an assessment, do everything here
                //if failed, nothing to do
                if (data["ASSESS_PASSED"].Contains("PASSED"))
                {
                    //no rollback, report is valid but next step&complete should be manual

                    if (reporttype == "3" || reporttype == "2")  //it's assist
                    {
                        complete_Step(data["genid"]);

                        //twr assist doesnt create next step, app-acc creates next step
                        if (data["POSITION"] != "TWR-ASSIST")
                            create_NEXT_STEP(data["genid"], data["TRAINEE_ID"]);

                    }
                    else if (reporttype == "1" || reporttype == "4") //GMC/ADC,  APP, ACC LEVELS
                    {
                        //if it's a Remedial, Progress Assessment -> don't complete the step
                        if (data.ContainsKey("CHK_LVLASS") && data["CHK_LVLASS"] == "1")
                        {
                            complete_Step(data["genid"]);
                            create_NEXT_STEP(data["genid"], data["TRAINEE_ID"]);
                        }
                    }

                }

            }
            catch (Exception e)
            {

                string mes = e.Message;
            }

            return reportid;
        }

        public static bool create_NEXT_STEP(string genid, string employeeid)
        {
            //level3 plus isn't an issue because chk_lvl_assesment is disabled 
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                      @"DECLARE @COMPLETEDSTEP INT = ( SELECT STEPID FROM USER_TRAINING_FOLDER WHERE GENID= @GENID)

                        DECLARE @NEWSTEP INT = (SELECT TOP 1 ID FROM TRAINING_TREE_STEPS WHERE ID > @COMPLETEDSTEP ORDER BY ID ASC)

                        INSERT INTO USER_TRAINING_FOLDER
                        SELECT TTS.ID , @USERID, CONVERT(varchar,GETUTCDATE(),20) , 'ONGOING',NULL,NULL,NULL 
                        FROM TRAINING_TREE_STEPS  TTS
                        WHERE TTS.ID = @NEWSTEP

                        INSERT INTO USER_TOTALHOURS
                        SELECT USERID, @NEWSTEP, TOTALHOURS, 'StepCreated',CONVERT(varchar,GETUTCDATE(),20), '',0  FROM USER_TOTALHOURS WHERE USERID=@USERID AND STEPID=@COMPLETEDSTEP


", connection))
                {
                    connection.Open();

                    command.Parameters.Add("@GENID", SqlDbType.Int).Value = genid;
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = employeeid;
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

                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
            using (SqlConnection connection = new SqlConnection(Con_Str.current))
            using (SqlCommand command = new SqlCommand(
                         @"INSERT INTO REPORTS_META VALUES('1', @OJTI_ID, convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , 1, @TRAINEE_SIGNED , @TRAINEE_ID )
                           DECLARE @ID INT = ( SELECT SCOPE_IDENTITY() )

                         INSERT INTO REPORT_TR_ARE_APP_RAD
                                ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_PREOJT],[CHK_SIM],[CHK_LVLASS],[CHK_PROGASS],[CHK_COCASS],[CHK_REMASS],[CHK_OTS],[DATE],[POSITION],[POSITION_EXTRA],[TIMEON_SCH],[TIMEOFF_SCH],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[NOTES],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS],TIMEON_ACT,TIMEOFF_ACT,NOSHOW,NOTRAINING, ASSESS_PASSED )
                         VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_PREOJT,@CHK_SIM,@CHK_LVLASS,@CHK_PROGASS,@CHK_COCASS,@CHK_REMASS,@CHK_OTS,@DATE,@POSITION,@POSITION_EXTRA,@TIMEON_SCH,@TIMEOFF_SCH,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@NOTES,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS,@TIMEON_ACT, @TIMEOFF_ACT , @NOSHOW,@NOTRAINING, @ASSESS_PASSED)

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
                    command.Parameters.Add("@CHK_PREOJT", SqlDbType.Bit).Value = data["CHK_PREOJT"] == "1";
                    command.Parameters.Add("@CHK_SIM", SqlDbType.Bit).Value = data["CHK_SIM"] == "1";
                    command.Parameters.Add("@CHK_LVLASS", SqlDbType.Bit).Value = data["CHK_LVLASS"] == "1";
                    command.Parameters.Add("@CHK_PROGASS", SqlDbType.Bit).Value = data["CHK_PROGASS"] == "1";
                    command.Parameters.Add("@CHK_COCASS", SqlDbType.Bit).Value = data["CHK_COCASS"] == "1";
                    command.Parameters.Add("@CHK_REMASS", SqlDbType.Bit).Value = data["CHK_REMASS"] == "1";
                    command.Parameters.Add("@CHK_OTS", SqlDbType.Bit).Value = data["CHK_OTS"] == "1";
                    command.Parameters.Add("@NOSHOW", SqlDbType.Bit).Value = data["NOSHOW"] == "1";
                    command.Parameters.Add("@NOTRAINING", SqlDbType.Bit).Value = data["NOTRAINING"] == "1";
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
                    command.Parameters.Add("@ASSESS_PASSED", SqlDbType.VarChar).Value = data["ASSESS_PASSED"] == null ? (object)DBNull.Value : data["ASSESS_PASSED"] ;

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
                    return "";
                }

                return reportid;
            }
        }

        public static string push_TOWERTR_GMC_ADC(Dictionary<string, string> data)
        {
            string reportid = "";

            // push into  REPORTS_META
            using (SqlConnection connection = new SqlConnection(Con_Str.current))
            using (SqlCommand command = new SqlCommand(
@"INSERT INTO REPORTS_META VALUES('4', @OJTI_ID, convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , 1, @TRAINEE_SIGNED , @TRAINEE_ID )
  DECLARE @ID INT = ( SELECT SCOPE_IDENTITY() )

INSERT INTO REPORT_TOWERTR_GMC_ADC
        ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_PREOJT],[CHK_SIM],[CHK_LVLASS],[CHK_PROGASS],[CHK_COCASS],[CHK_REMASS],[CHK_OTS],[DATE],[POSITION],[TIMEON_SCH],[TIMEOFF_SCH],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS],TIMEON_ACT,TIMEOFF_ACT,NOSHOW,NOTRAINING,ASSESS_PASSED )
VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_PREOJT,@CHK_SIM,@CHK_LVLASS,@CHK_PROGASS,@CHK_COCASS,@CHK_REMASS,@CHK_OTS,@DATE,@POSITION,@TIMEON_SCH,@TIMEOFF_SCH,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS,@TIMEON_ACT, @TIMEOFF_ACT,@NOSHOW,@NOTRAINING,@ASSESS_PASSED)

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
                    command.Parameters.Add("@CHK_PREOJT", SqlDbType.Bit).Value = data["CHK_PREOJT"] == "1";
                    command.Parameters.Add("@CHK_SIM", SqlDbType.Bit).Value = data["CHK_SIM"] == "1";
                    command.Parameters.Add("@CHK_LVLASS", SqlDbType.Bit).Value = data["CHK_LVLASS"] == "1";
                    command.Parameters.Add("@CHK_PROGASS", SqlDbType.Bit).Value = data["CHK_PROGASS"] == "1";
                    command.Parameters.Add("@CHK_COCASS", SqlDbType.Bit).Value = data["CHK_COCASS"] == "1";
                    command.Parameters.Add("@CHK_REMASS", SqlDbType.Bit).Value = data["CHK_REMASS"] == "1";
                    command.Parameters.Add("@CHK_OTS", SqlDbType.Bit).Value = data["CHK_OTS"] == "1";
                    command.Parameters.Add("@NOSHOW", SqlDbType.Bit).Value = data["NOSHOW"] == "1";
                    command.Parameters.Add("@NOTRAINING", SqlDbType.Bit).Value = data["NOTRAINING"] == "1";
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
                    command.Parameters.Add("@ASSESS_PASSED", SqlDbType.Bit).Value = data["ASSESS_PASSED"] == null ? (object)DBNull.Value : data["ASSESS_PASSED"] ;


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

                    return "";

                }

                return reportid;
            }
        }

        public static string push_DAILYTR_ASS_RAD(Dictionary<string, string> data)
        {
            string reportid = "";


            //todo: push into  REPORTS_META

            using (SqlConnection connection = new SqlConnection(Con_Str.current))
            using (SqlCommand command = new SqlCommand(
         @"
INSERT INTO REPORTS_META VALUES('3', @OJTI_ID, convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , 1, @TRAINEE_SIGNED , @TRAINEE_ID )
DECLARE @ID INT = ( SELECT SCOPE_IDENTITY() )

INSERT INTO REPORT_DAILYTR_ASS_RAD
       ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_ASS],[DATE],[POSITION],[POSITION_EXTRA],[TIMEON_SCH],[TIMEOFF_SCH],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS],TIMEON_ACT, TIMEOFF_ACT,NOSHOW,NOTRAINING, ASSESS_PASSED )
VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_ASS,@DATE,@POSITION,@POSITION_EXTRA,@TIMEON_SCH,@TIMEOFF_SCH,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS, @TIMEON_ACT, @TIMEOFF_ACT,@NOSHOW,@NOTRAINING, @ASSESS_PASSED)

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
                    command.Parameters.Add("@NOSHOW", SqlDbType.Bit).Value = data["NOSHOW"] == "1";
                    command.Parameters.Add("@NOTRAINING", SqlDbType.Bit).Value = data["NOTRAINING"] == "1";
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
                    command.Parameters.Add("@ASSESS_PASSED", SqlDbType.Bit).Value = data["CHK_ASS"] == "0" ? (object)DBNull.Value : true;

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
                    return "";

                }

                return reportid;

            }

        }

        public static string push_DAILYTR_ASS_TWR(Dictionary<string, string> data)
        {
            string reportid = "";


            //todo: push into  REPORTS_META

            using (SqlConnection connection = new SqlConnection(Con_Str.current))
            using (SqlCommand command = new SqlCommand(
@"
INSERT INTO REPORTS_META VALUES('2', @OJTI_ID, convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , 1, @TRAINEE_SIGNED , @TRAINEE_ID )
DECLARE @ID INT = ( SELECT SCOPE_IDENTITY() )

INSERT INTO REPORT_DAILYTR_ASS_TWR
       ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_ASS],[DATE],[POSITION],[TIMEON_SCH],[TIMEOFF_SCH],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS], TIMEON_ACT, TIMEOFF_ACT ,NOSHOW,NOTRAINING,ASSESS_PASSED)
VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_ASS,@DATE,@POSITION,@TIMEON_SCH,@TIMEOFF_SCH,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS, @TIMEON_ACT, @TIMEOFF_ACT,@NOSHOW,@NOTRAINING, @ASSESS_PASSED)

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
                    command.Parameters.Add("@NOSHOW", SqlDbType.Bit).Value = data["NOSHOW"] == "1";
                    command.Parameters.Add("@NOTRAINING", SqlDbType.Bit).Value = data["NOTRAINING"] == "1";
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
                    command.Parameters.Add("@ASSESS_PASSED", SqlDbType.Bit).Value = data["CHK_ASS"] == "0" ? (object)DBNull.Value : true;


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
                    return "";
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                                using (SqlConnection con = new SqlConnection(Con_Str.current))
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



            string status = data["DEPARTMENT_SIGNED"] == "1" ? "RECOMMENDED_FOR_LEVEL" : "PENDING_APPROVAL";
            //reaching here means everything went fine
            if (!push_UserTrainingFolder(reportid, data["genid"], status: status))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                            using (SqlConnection con = new SqlConnection(Con_Str.current))
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
            if (!push_UserTrainingFolder(reportid, data["genid"], status: "RECOMMENDED_FOR_CERTIFICATION"))
                return "";

            return reportid;
        }


        public static bool push_UserTrainingFolder(string reportid, string genid, string status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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


        public static bool complete_Step(string genid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                             @"DECLARE @STEPID INT = (SELECT STEPID FROM USER_TRAINING_FOLDER WHERE GENID=@GENID)

                                UPDATE USER_TRAINING_FOLDER
                                SET STATUS = 'COMPLETED'
                                WHERE STEPID = @STEPID AND STATUS = 'ONGOING'", connection))
                {
                    connection.Open();

                    command.Parameters.Add("@GENID", SqlDbType.Int).Value = genid;
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                rpt_tablename = "REPORT_TOWERTR_GMC_ADC";
            else if (type == "5")
                rpt_tablename = "REPORT_RECOM_LEVEL";

            string updatetbl = "";
            if (Convert.ToInt32(type) <= 4)
                updatetbl = "UPDATE " + rpt_tablename + " SET STUDENT_COMMENTS = @STUDENTCOMMENT WHERE ID = @REPORTID ";
            else if (type == "5")
                updatetbl = "UPDATE REPORT_RECOM_LEVEL SET TRAINEE_SIGNED = 1 WHERE ID=@REPORTID)";

            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                         @" UPDATE REPORTS_META SET TRAINEE_SIGNED = 1 , STATUS = 'TRAINEESIGNED' WHERE ID = @REPORTID ;"
                            + updatetbl, connection))
                {
                    connection.Open();
                    command.Parameters.Add("@REPORTID", SqlDbType.Int).Value = reportid;
                    if (type != "5")
                        command.Parameters.Add("@STUDENTCOMMENT", SqlDbType.NVarChar).Value = studentcomment;

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

        public static bool update_Sign_RECOM_LEVEL(string reportid, string whosigns, string signerid)
        {
            string update = "";

            if (whosigns == "trainee")
                update = @"UPDATE REPORTS_META SET TRAINEE_SIGNED = 1 , STATUS = 'TRAINEESIGNED' WHERE ID = @REPORTID ;
                           UPDATE  REPORT_RECOM_LEVEL SET TRAINEE_SIGNED = 1 WHERE ID = @REPORTID";
            else if (whosigns == "TRN_DEPARTMENT_SIGNER")
                update = @"UPDATE REPORTS_META SET  STATUS = 'DEPARTMENTSIGNED' WHERE ID = @REPORTID ;
                           UPDATE  REPORT_RECOM_LEVEL SET DEPARTMENT_SIGNED = 1 , DEPARTMENT_EMPLOYEEID = " + signerid + @" WHERE ID = @REPORTID  ; 
                           UPDATE USER_TRAINING_FOLDER SET STATUS = 'RECOMMENDED_FOR_LEVEL' WHERE REPORTID = @REPORTID ";

            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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

        public static bool update_Sign_RECOM_CERTIF(string reportid, string whosigns, string signerid)
        {
            string update = "";

            if (whosigns == "trainee")
                update = @"UPDATE REPORTS_META SET TRAINEE_SIGNED = 1 , STATUS = 'TRAINEESIGNED' WHERE ID = @REPORTID ;
                           UPDATE  REPORT_RECOM_CERTIF SET TRAINEE_SIGNED = 1 WHERE ID = @REPORTID";
            else if (whosigns == "MEMBER1")
                update = @"UPDATE REPORTS_META SET  STATUS = 'MEMBER1SIGNED' WHERE ID = @REPORTID ;
                           UPDATE  REPORT_RECOM_CERTIF SET MEMBER1_SIGNED_SIGNED = 1 , MEMBER1_ID = " + signerid + @" , MEMBER1_SIGN_DATE = CONVERT(VARCHAR, GETUTCDATE(),20)  WHERE ID = @REPORTID  ; ";
            else if (whosigns == "MEMBER2")
                update = @"UPDATE REPORTS_META SET  STATUS = 'MEMBER2SIGNED' WHERE ID = @REPORTID ;
                           UPDATE  REPORT_RECOM_CERTIF SET MEMBER2_SIGNED_SIGNED = 1 , MEMBER2_ID = " + signerid + @" , MEMBER2_SIGN_DATE = CONVERT(VARCHAR, GETUTCDATE(),20)  WHERE ID = @REPORTID  ; ";
            else if (whosigns == "MEMBER3")
                update = @"UPDATE REPORTS_META SET  STATUS = 'MEMBER3SIGNED' WHERE ID = @REPORTID ;
                           UPDATE  REPORT_RECOM_CERTIF SET MEMBER3_SIGNED_SIGNED = 1 , MEMBER3_ID = " + signerid + @" , MEMBER3_SIGN_DATE = CONVERT(VARCHAR, GETUTCDATE(),20)  WHERE ID = @REPORTID  ; 
                        UPDATE USER_TRAINING_FOLDER SET STATUS = 'DEPARTMENTSIGN_COMPLETE' WHERE REPORTID = @REPORTID";

            //REVIEW TEAM APPROVAL DURUMUNDA ASAGISI : 
            // UPDATE USER_TRAINING_FOLDER SET STATUS = 'RECOMMENDED_FOR_CERTIF' WHERE REPORTID = @REPORTID ";



            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                return get_TrainingSteps_TWR(sector);
            else if (position == "ACC" || position == "APP")
                return get_TrainingSteps_ACC_APP(position, sector);

            return null;
        }
        public static DataTable get_TrainingSteps_ACC_APP(string position, string sector)
        {

            DataTable result_sector = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(@"
                        SELECT [ID] , [DESCRIPTION] 
                        FROM TRAINING_TREE_STEPS 
                        WHERE POSITION = @POSITION AND SECTOR =@SECTOR ORDER BY ID ASC", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                            @"  SELECT TTS.DESCRIPTION , UTF.STATUS , UTF.CREATED_TIME, ISNULL(UTF.REPORTID,'') AS [Rpt.Num.] , ISNULL(UTF.FILENAME,'') AS [FileName]
		                                , UTF.GENID, TTS.PHASE , TTS.[NAME] , UTF.STEPID
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                {
                    using (SqlCommand command = new SqlCommand(
                              @" 
                                SELECT TOP 1 UTF.GENID FROM USER_TRAINING_FOLDER UTF
                                JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID 
                                WHERE UTF.EMPLOYEEID = @EMPLOYEEID 
                                        AND TTS.POSITION = @POSITION
                                        AND 1 = CASE WHEN @SECTOR = 'ASSIST' AND TTS.PHASE = @SECTOR THEN 1
			                                         WHEN TTS.SECTOR = @SECTOR THEN 1
			 ELSE 0 END", connection))
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


        public static string start_FDO(string employeeid, string position)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                        @" IF NOT EXISTS (
                                            SELECT TOP 1 TTS.ID  
                                            FROM USER_TRAINING_FOLDER UTF
                                            JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID
                                            WHERE UTF.EMPLOYEEID = @EMPLOYEEID 
                                                 AND TTS.POSITION = @POSITION 
                                                 AND ISNULL(TTS.SECTOR,'') = '' 
                                                 AND TTS.PHASE IN ('FDO')
                                          )
                            BEGIN
                                INSERT INTO USER_TRAINING_FOLDER 
                                SELECT TTS.ID, @EMPLOYEEID, CONVERT(VARCHAR, GETUTCDATE(),20),'MIGRATION', NULL, NULL,NULL
                                FROM TRAINING_TREE_STEPS TTS 
                                WHERE TTS.POSITION = @POSITION
                                        AND ISNULL(TTS.SECTOR,'') = '' 
                                        AND TTS.PHASE IN ('FDO')
                            END", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                    return "";
                }
            }
            catch (Exception e)
            {
                string mes = e.Message;
            }
            return "fdo";
        }

        public static string start_PREOJT(string employeeid, string position, string status = "MIGRATION")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                        @" IF NOT EXISTS (
                                            SELECT TOP 1 TTS.ID  
                                            FROM USER_TRAINING_FOLDER UTF
                                            JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID
                                            WHERE UTF.EMPLOYEEID = @EMPLOYEEID 
                                                 AND TTS.POSITION = @POSITION 
                                                 AND ISNULL(TTS.SECTOR,'') = '' 
                                                 AND TTS.PHASE IN ('PREOJT')
                                          )
                            BEGIN
                                INSERT INTO USER_TRAINING_FOLDER 
                                SELECT TTS.ID, @EMPLOYEEID, CONVERT(VARCHAR, GETUTCDATE(),20),@STATUS, NULL, NULL,NULL
                                FROM TRAINING_TREE_STEPS TTS 
                                WHERE TTS.POSITION = @POSITION
                                        AND ISNULL(TTS.SECTOR,'') = '' 
                                        AND TTS.PHASE IN ('PREOJT')
                            END", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
                    command.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = status;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                    return "";
                }
            }
            catch (Exception e)
            {
                string mes = e.Message;
            }
            return "preojt";
        }
        public static string start_POSITION_ASSIST(string employeeid, string position, string status = "MIGRATION")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                        @" IF NOT EXISTS (
                                            SELECT TOP 1 TTS.ID  
                                            FROM USER_TRAINING_FOLDER UTF
                                            JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID
                                            WHERE UTF.EMPLOYEEID = @EMPLOYEEID 
                                                 AND TTS.POSITION = @POSITION 
                                                 AND ISNULL(TTS.SECTOR,'') = '' 
                                                 AND TTS.PHASE IN ('ASSIST')
                                          )
                            BEGIN
                                INSERT INTO USER_TRAINING_FOLDER 
                                SELECT TTS.ID, @EMPLOYEEID, CONVERT(VARCHAR, GETUTCDATE(),20),@STATUS, NULL, NULL,NULL
                                FROM TRAINING_TREE_STEPS TTS 
                                WHERE TTS.POSITION = @POSITION
                                        AND ISNULL(TTS.SECTOR,'') = '' 
                                        AND TTS.PHASE IN ('ASSIST')
                            END", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
                    command.Parameters.Add("@STATUS", SqlDbType.NVarChar).Value = status;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                    return "";
                }
            }
            catch (Exception e)
            {
                string mes = e.Message;
            }
            return "posassist";
        }

        public static string start_SECTOR_MIGRATION(string employeeid, string position, string sector, string startstepid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                                 @" INSERT INTO USER_TRAINING_FOLDER (STEPID , EMPLOYEEID, CREATED_TIME, [STATUS] , REPORTID , [FILENAME] , COMMENTS)
                                    SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'MIGRATION' , NULL, NULL, NULL 
                                    FROM TRAINING_TREE_STEPS TTS 
                                    WHERE TTS.POSITION = @POSITION 
                                          AND TTS.SECTOR = @SECTOR 
                                          AND TTS.ID < @STEPID   ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = startstepid;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                    return "";
                }
            }
            catch (Exception e)
            {
                string em = e.Message;
            }
            return "stepmigration";
        }

        public static string start_ONGOING_STEP(string employeeid, string stepid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                                 @" INSERT INTO USER_TRAINING_FOLDER (STEPID , EMPLOYEEID, CREATED_TIME, [STATUS] , REPORTID , [FILENAME] , COMMENTS)
                                    SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'ONGOING' , NULL, NULL, NULL 
                                    FROM TRAINING_TREE_STEPS TTS 
                                    WHERE TTS.ID = @STEPID ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.CommandType = CommandType.Text;

                    command.ExecuteNonQuery();
                    return "";
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return "ongoing";

        }

        public static string start_Training_Folder_Migrate(string employeeid, string position, string sector, string startstepid)
        {
            string ok = "";

            if (position == "TWR")
            {
                if (sector == "ASSIST") //exceptional case : TWR_ASSIST
                {
                    //tower doesnt have FDO
                    ok = start_PREOJT(employeeid, position, "MIGRATION") +
                         start_POSITION_ASSIST(employeeid, position, "ONGOING");
                }
                else
                {
                    ok = start_PREOJT(employeeid, position, "MIGRATION")
                     + start_POSITION_ASSIST(employeeid, position, "MIGRATION")
                     + start_SECTOR_MIGRATION(employeeid, position, sector, startstepid)
                     + start_ONGOING_STEP(employeeid, startstepid);
                }
            }
            else
            {
                ok = start_FDO(employeeid, position)
                 + start_POSITION_ASSIST(employeeid, position)
                 + start_PREOJT(employeeid, position)
                 + start_SECTOR_MIGRATION(employeeid, position, sector, startstepid)
                 + start_ONGOING_STEP(employeeid, startstepid);
            }

            return ok;
        }

        public static bool update_totalhours(string userid, string stepid, string totalhours, string lastupdate, string extra = "")
        {
            try
            {
                //insert all steps before chosen step in the sector  as MIGRATION
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                      @"IF EXISTS (SELECT TOP 1 TOTALHOURS FROM USER_TOTALHOURS WHERE USERID= @USERID AND STEPID = @STEPID)
                        BEGIN
		                        UPDATE USER_TOTALHOURS 
		                        SET TOTALHOURS = @TOTALHOURS ,
			                        LASTUPDATE = @LASTUPDATE ,
			                        LASTUPDATE_TIME = CONVERT(VARCHAR, GETUTCDATE(), 20 ),
			                        EXTRA = CASE WHEN @EXTRA = '' THEN EXTRA ELSE @EXTRA END,
                                    [BY] = @BY
		                        WHERE USERID=@USERID AND STEPID =@STEPID
                        END
                        ELSE
	                        INSERT INTO USER_TOTALHOURS 
	                        VALUES (@USERID, @STEPID, @TOTALHOURS , @LASTUPDATE, CONVERT(VARCHAR, GETUTCDATE(),20), @EXTRA, @BY)", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = userid;
                    command.Parameters.Add("@STEPID", SqlDbType.VarChar).Value = stepid;
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT DISTINCT TTS.ID, TTS.POSITION, TTS.SECTOR, TTS.PHASE, TTS.DESCRIPTION, 
		                                    UTF.STATUS
                                    FROM USER_TRAINING_FOLDER UTF
                                    JOIN TRAINING_TREE_STEPS TTS ON UTF.STEPID = TTS.ID
                                    WHERE EMPLOYEEID = @USERID AND UTF.STATUS NOT IN  ('REPORT', 'Assessment Passed','Assessment Failed')
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

        public static string get_TOTALHOURS(string trainee, string stepid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                             @" SELECT top 1 max(TOTALHOURS) AS 'TOTALHOURS' FROM USER_TOTALHOURS UTH
                                WHERE UTH.STEPID IN
	                                (	
		                                SELECT TTS.ID FROM TRAINING_TREE_STEPS TTS
		                                JOIN ( SELECT * FROM TRAINING_TREE_STEPS WHERE ID = @STEPID ) AS CURR
		                                    ON TTS.POSITION = CURR.POSITION AND 
                                               1 = CASE WHEN ISNULL(CURR.SECTOR,'' ) = '' AND CURR.NAME = TTS.NAME THEN 1
				                                        WHEN CURR.SECTOR = TTS.SECTOR AND TTS.PHASE <> 'OJT_ASSIST' THEN 1
				                                        ELSE 0 END
	                                ) 
                                    AND UTH.USERID = @USERID
                                    ", connection))
                {
                    // todo : it should change to a better solution but this should work
                    ///IMPORTANT
                    /// normally, we calculated total hours by selecting all ojtlevels and adding them below
                    /// now we are selecting max with the same step-selection
                    /// if sector is null: check stepname : for twr assist
                    /// if sector is not null but not OJT , check name for BR-AR-CR-SB-NR ASSISTS (THEY DONT COMBINE WTIH OJT LEVELS)
                    /// if sector is not null, and it is OJT LEVELS, select all the steps, get the max
                    connection.Open();
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = trainee;
                    command.Parameters.Add("@STEPID", SqlDbType.VarChar).Value = stepid;


                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable res = new DataTable();
                    da.Fill(res);

                    if (res == null || res.Rows.Count == 0)
                        return "00:00";

                    string total = "00:00"; // we are keeping this because there is one row anyways
                    foreach (DataRow item in res.Rows)
                        total = Utility.add_TimeFormat(total, item["TOTALHOURS"].ToString());

                    return total;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return "00:00";
        }

        public static string get_MER_sector(string employeeid, string sector)
        {
            //for sector it returns MER to complete LEVEL3PLUS of the sector, if none LEVEL3

            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
              @"DECLARE @LEVEL3PLUS INT = ( SELECT STEPID FROM TRAINING_TREE_STEPS 
							WHERE SECTOR = @SECTOR AND PHASE = 'OJT_LEVEL3PLUS')
                DECLARE @MER_LEVEL3PLUS VARCHAR(10) = ( SELECT ISNULL(MER,'') FROM MER_USER 
										                WHERE STEPID = @LEVEL3PLUS AND EMPLOYEEID = @EMPLOYEEID )

                IF @MER_LEVEL3PLUS = ''
                BEGIN
	                DECLARE @LEVEL3 INT = ( SELECT STEPID FROM TRAINING_TREE_STEPS 
							                WHERE SECTOR = @SECTOR AND PHASE = 'OJT_LEVEL3')
	                SELECT ISNULL(MER,'') FROM MER_USER WHERE STEPID = @LEVEL3 AND EMPLOYEEID = @EMPLOYEEID 
                END
                ELSE
	                SELECT @MER_LEVEL3PLUS
				", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;

                    command.CommandType = CommandType.Text;

                    return Convert.ToString(command.ExecuteScalar() as object);
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return "";
        }
        public static string get_MER_step(string employeeid, string stepid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                             @"SELECT MER FROM MER_USER  WHERE STEPID = @STEPID AND EMPLOYEEID = @EMPLOYEEID
				", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;

                    command.CommandType = CommandType.Text;

                    string res = Convert.ToString(command.ExecuteScalar() as object);
                    if (String.IsNullOrEmpty(res))
                        return "00:00";
                    else
                        return res;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return "";
        }

        public static DataTable get_MERs(string employeeid, string position = "", string sector = "", string stepid = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                            @"  SELECT ISNULL(MU.MER,'00:00') AS 'Current' , 
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

        public static bool update_MER(string stepid, string MER, string employeeid, string comments = "")
        {
            //TODO : EMPLOYEEID WOULDNT UPDATE SINCE THERE IS NO PRIOR RECORD. MUST INSERT FIRST
            UserSession user = (UserSession)HttpContext.Current.Session["usersession"];

            try
            {

                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                    command.Parameters.Add("@MER", SqlDbType.VarChar).Value = MER;
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


        //public static DataTable get_Level_Objectives(string employeeid, DataRow row)
        //{
        //    string position = row["POSITION"].ToString();
        //    string sector = row["SECTOR"].ToString();
        //    string phase = row["PHASE"].ToString();

        //    if (sector == "")
        //        sector = phase; // TWR ASSIST HAS TO BE HANDLED

        //    string type = "";
        //    if (sector == "GMC")
        //        type = "TWR_GMC";
        //    else if (sector == "ADC")
        //        type = "TWR_ADC";
        //    else if (sector == "ASSIST")
        //        type = "TWR_ASSIST";
        //    else if (new string[3] { "NR", "SR", "CR" }.Contains(sector))
        //        type = "ACC_" + sector;
        //    else if (new string[3] { "AR", "BR", "KR" }.Contains(sector))
        //        type = "APP_" + sector;

        //    string level = "";
        //    if (phase.Contains("LEVEL1"))
        //        level = "1";
        //    else if (phase.Contains("LEVEL2"))
        //        level = "2";
        //    else if (phase.Contains("LEVEL3"))
        //        level = "3";
        //    else if (sector == "ASSIST" && phase == "ASSIST") //TWR ASSIST
        //        level = "0";


        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(Con_Str.current))
        //        using (SqlCommand command = new SqlCommand(
        //         @" SELECT DEF.ID, DEF.CATEGORY , DEF.HEADER, DEF.OBJECTIVE ,
	       //              ISNULL(US.INITIAL,'') as 'By' , F.ID as 'FORMID'
        //         FROM LEVEL_OBJECTIVES_FORM F
        //         JOIN LEVEL_OBJECTIVES_DEF DEF ON DEF.FORMID = F.ID
        //         LEFT JOIN LEVEL_OBJECTIVES_USER U ON U.OBJECTIVEID = DEF.ID AND U.USERID = @USERID
        //         LEFT JOIN USERS US ON US.EMPLOYEEID = U.SIGNEDBY
        //         WHERE F.TYPE = @TYPE AND F.LEVELNUM = @LEVEL AND F.ISACTIVE = 1 AND DEF.ISACTIVE = 1", connection))
        //        {
        //            connection.Open();
        //            command.Parameters.Add("@USERID", SqlDbType.Int).Value = employeeid;
        //            command.Parameters.Add("@TYPE", SqlDbType.VarChar).Value = type;
        //            command.Parameters.Add("@LEVEL", SqlDbType.VarChar).Value = level;
        //            command.CommandType = CommandType.Text;

        //            SqlDataAdapter da = new SqlDataAdapter(command);
        //            DataTable res = new DataTable();
        //            da.Fill(res);

        //            if (res != null || res.Rows.Count > 0)
        //                return res;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        string err = e.Message;
        //    }
        //    return null;
        //}

        public static DataTable get_Level_Objectives(string employeeid, string sector, string phase)
        {
            //string position = row["POSITION"].ToString();
            //string sector = row["SECTOR"].ToString();
            //string phase = row["PHASE"].ToString();

            if (sector == "")
                sector = phase; // TWR ASSIST HAS TO BE HANDLED

            string type = "";
            if (sector == "GMC")
                type = "TWR_GMC";
            else if (sector == "ADC")
                type = "TWR_ADC";
            else if (sector == "ASSIST")
                type = "TWR_ASSIST";
            else if (new string[3] {  "NR", "SR", "CR" }.Contains(sector))
                type = "ACC_" + sector;
            else if (new string[3] { "AR", "BR", "KR" }.Contains(sector))
                type = "APP_" + sector;
            
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
        public static bool is_LevelObjectives_completed(string userid, string sector, string phase)
        {
            DataTable objectives = get_Level_Objectives(userid, sector, phase);
            if (objectives == null || objectives.Rows.Count == 0)
                return true; // todo : no objectives to sign or error? does it matter, because we call this func only when there should be objectives

            DataRow[] res = objectives.Select("BY = '' ");
            if (res == null || res.Length == 0)
                return true;

            return false;
        }

        public static DataTable get_ONGOING_step(string employeeid, string sector)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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

                using (SqlConnection connection = new SqlConnection(Con_Str.current))
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


        public static bool is_RECOMMENDED_forLevel(string employeeid, string stepid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                    @"  IF EXISTS ( SELECT STATUS FROM USER_TRAINING_FOLDER 
			                        WHERE EMPLOYEEID = @EMPLOYEEID AND STEPID = @STEPID
			                        AND STATUS = 'RECOMMENDED_FOR_LEVEL' )
	                        SELECT '1'
                        ELSE
	                        SELECT '0'
                     ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@STEPID", SqlDbType.Int).Value = stepid;

                    command.CommandType = CommandType.Text;

                    return Convert.ToString(command.ExecuteScalar()) == "1";
                }
            }
            catch (Exception e)
            {
                string me = e.Message;
            }
            return false;
        }

        public static DataTable get_Department_Reports()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                            @" SELECT 
	                                META.ID,
	                                CASE WHEN META.[TYPE] = 5 THEN 'LEVEL' WHEN META.[TYPE] = '6' THEN 'CERTIFICATION' END AS 'RECOMMENDED',
	                                META.[STATUS],
	                                (SELECT INITIAL + '-' + FIRSTNAME + ' ' + SURNAME FROM USERS WHERE EMPLOYEEID = META.TRAINEE_ID) AS 'TRAINEE',
	                                (SELECT INITIAL + '-' + FIRSTNAME + ' ' + SURNAME FROM USERS WHERE EMPLOYEEID = META.CREATER) AS 'CREATER',
	                                META.CREATE_TIME
                                FROM REPORTS_META META
                                LEFT JOIN REPORT_RECOM_LEVEL LEV ON LEV.ID = META.ID
                                LEFT JOIN REPORT_RECOM_CERTIF CER ON CER.ID = META.ID
                                WHERE META.[TYPE] IN (5,6)  ", connection))
                {
                    connection.Open();

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
            return new DataTable();
        }
    }
}