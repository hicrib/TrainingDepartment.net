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
        public static string push_TR_ARE_APP_RAD(Dictionary<string, string> data)
        {
            string reportid = "";


            // push into  REPORTS_META
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                 @"INSERT INTO REPORTS_META VALUES('1', '" + data["OJTI_ID"] + "', convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , " + data["OJTI_SIGNED"] + ", " + data["TRAINEE_SIGNED"] + " , " + data["TRAINEE_ID"] + ")   ; SELECT SCOPE_IDENTITY()", connection))
                    {
                        connection.Open();
                        reportid = Convert.ToString(command.ExecuteScalar());
                        if (String.IsNullOrWhiteSpace(reportid))
                            return "";

                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }


            // push into REPORT_TR_ARE_APP_RAD
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {

                    string insertstr = @"INSERT INTO REPORT_TR_ARE_APP_RAD
                                               ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_PREOJT],[CHK_SIM],[CHK_LVLASS],[CHK_PROGASS],[CHK_COCASS],[CHK_REMASS],[CHK_OTS],[DATE],[POSITION],[POSITION_EXTRA],[TIMEON],[TIMEOFF],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[NOTES],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS] )
                                        VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_PREOJT,@CHK_SIM,@CHK_LVLASS,@CHK_PROGASS,@CHK_COCASS,@CHK_REMASS,@CHK_OTS,@DATE,@POSITION,@POSITION_EXTRA,@TIMEON,@TIMEOFF,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@NOTES,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS)";
                    using (SqlCommand command = new SqlCommand(insertstr, connection))
                    {
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = reportid;
                        command.Parameters.Add("@OJTI_ID", SqlDbType.Int).Value = data["OJTI_ID"];
                        command.Parameters.Add("@TRAINEE_ID", SqlDbType.Int).Value = data["TRAINEE_ID"];
                        command.Parameters.Add("@CHK_OJT", SqlDbType.Bit).Value = data["CHK_OJT"] == "1";
                        command.Parameters.Add("@CHK_PREOJT", SqlDbType.Bit).Value = data["CHK_PREOJT"] == "1";
                        command.Parameters.Add("@CHK_SIM", SqlDbType.Bit).Value = data["CHK_SIM"] == "1";
                        command.Parameters.Add("@CHK_LVLASS", SqlDbType.Bit).Value = data["CHK_LVLASS"] == "1";
                        command.Parameters.Add("@CHK_PROGASS", SqlDbType.Bit).Value = data["CHK_PROGASS"] == "1";
                        command.Parameters.Add("@CHK_COCASS", SqlDbType.Bit).Value = data["CHK_COCASS"] == "1";
                        command.Parameters.Add("@CHK_REMASS", SqlDbType.Bit).Value = data["CHK_REMASS"] == "1";
                        command.Parameters.Add("@CHK_OTS", SqlDbType.Bit).Value = data["CHK_OTS"] == "1";
                        command.Parameters.Add("@DATE", SqlDbType.VarChar).Value = data["DATE"];
                        command.Parameters.Add("@POSITION", SqlDbType.VarChar).Value = data["POSITION"];
                        command.Parameters.Add("@POSITION_EXTRA", SqlDbType.VarChar).Value = data["POSITION_EXTRA"];
                        command.Parameters.Add("@TIMEON", SqlDbType.VarChar).Value = data["TIMEON"];
                        command.Parameters.Add("@TIMEOFF", SqlDbType.VarChar).Value = data["TIMEOFF"];
                        command.Parameters.Add("@TRAF_DENS", SqlDbType.VarChar).Value = data["TRAF_DENS"];
                        command.Parameters.Add("@COMPLEXITY", SqlDbType.VarChar).Value = data["COMPLEXITY"];
                        command.Parameters.Add("@HOURS", SqlDbType.VarChar).Value = data["HOURS"];
                        command.Parameters.Add("@TOTAL_HOURS", SqlDbType.VarChar).Value = data["TOTAL_HOURS"];
                        command.Parameters.Add("@PREBRIEF_COMMENTS_FILENAME", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS_FILENAME"];
                        command.Parameters.Add("@PREBRIEF_COMMENTS", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS"];
                        command.Parameters.Add("@NOTES", SqlDbType.NVarChar).Value = data["NOTES"];
                        command.Parameters.Add("@ADDITIONAL_COMMENTS", SqlDbType.NVarChar).Value = data["ADDITIONAL_COMMENTS"];
                        command.Parameters.Add("@STUDENT_COMMENTS", SqlDbType.NVarChar).Value = data["STUDENT_COMMENTS"];

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
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }


            // push into CRITICALSKILL_RESULTS
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    //////////////////////////////////////////////////////////////////////
                    ///// REPORT TYPE 1 IS GIVEN, BE CAREFUL WHEN COPY/PASTE
                    string insert = @"with A as    
                                        (
                                            @replaceme@
                                        )
                                        INSERT INTO CRITICALSKILL_RESULTS
                                        SELECT " + reportid + @", RSC.ID , A.RES FROM REPORT_SKILL_CATEGORIES RSC
                                        JOIN A   ON RSC.CATEG_SKILL = A.CATEG
                                        WHERE RSC.REPORTTYPE = 1";

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


                    using (SqlCommand command = new SqlCommand(
                                 insert, connection))
                    {
                        connection.Open();
                        int rows = command.ExecuteNonQuery();

                        // if no rows are affected, rollback REPORTS_META, rollback REPORT_TR_ARE_APP_RAD  and RETURN FALSE
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
                                        }
                                        using (SqlCommand com = new SqlCommand(
                                                     @"DELETE FROM REPORT_TR_ARE_APP_RAD WHERE ID = " + reportid, connection))
                                        {
                                            con.Open();
                                            com.ExecuteNonQuery();
                                        }
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
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }
            //reaching here means everything went fine
            if (!push_UserTrainingFolder(reportid, data["genid"]))
                return "";
            return reportid;
        }

        public static string push_TOWERTR_GMC_ADC(Dictionary<string, string> data)
        {
            string reportid = "";


            // push into  REPORTS_META
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                             @"INSERT INTO REPORTS_META VALUES('4', '" + data["OJTI_ID"] + "', convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , " + data["OJTI_SIGNED"] + ", " + data["TRAINEE_SIGNED"] + " , " + data["TRAINEE_ID"] + ")   ; SELECT SCOPE_IDENTITY()", connection))
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


            //todo: push into step 

            // push into REPORT_TOWERTR_GMC_ADC
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {

                    string insertstr = @"INSERT INTO REPORT_TOWERTR_GMC_ADC
                                               ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_PREOJT],[CHK_SIM],[CHK_LVLASS],[CHK_PROGASS],[CHK_COCASS],[CHK_REMASS],[CHK_OTS],[DATE],[POSITION],[TIMEON],[TIMEOFF],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS] )
                                        VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_PREOJT,@CHK_SIM,@CHK_LVLASS,@CHK_PROGASS,@CHK_COCASS,@CHK_REMASS,@CHK_OTS,@DATE,@POSITION,@TIMEON,@TIMEOFF,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS)";
                    using (SqlCommand command = new SqlCommand(insertstr, connection))
                    {
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = reportid;
                        command.Parameters.Add("@OJTI_ID", SqlDbType.Int).Value = data["OJTI_ID"];
                        command.Parameters.Add("@TRAINEE_ID", SqlDbType.Int).Value = data["TRAINEE_ID"];
                        command.Parameters.Add("@CHK_OJT", SqlDbType.Bit).Value = data["CHK_OJT"] == "1";
                        command.Parameters.Add("@CHK_PREOJT", SqlDbType.Bit).Value = data["CHK_PREOJT"] == "1";
                        command.Parameters.Add("@CHK_SIM", SqlDbType.Bit).Value = data["CHK_SIM"] == "1";
                        command.Parameters.Add("@CHK_LVLASS", SqlDbType.Bit).Value = data["CHK_LVLASS"] == "1";
                        command.Parameters.Add("@CHK_PROGASS", SqlDbType.Bit).Value = data["CHK_PROGASS"] == "1";
                        command.Parameters.Add("@CHK_COCASS", SqlDbType.Bit).Value = data["CHK_COCASS"] == "1";
                        command.Parameters.Add("@CHK_REMASS", SqlDbType.Bit).Value = data["CHK_REMASS"] == "1";
                        command.Parameters.Add("@CHK_OTS", SqlDbType.Bit).Value = data["CHK_OTS"] == "1";
                        command.Parameters.Add("@DATE", SqlDbType.VarChar).Value = data["DATE"];
                        command.Parameters.Add("@POSITION", SqlDbType.VarChar).Value = data["POSITION"];
                        command.Parameters.Add("@TIMEON", SqlDbType.VarChar).Value = data["TIMEON"];
                        command.Parameters.Add("@TIMEOFF", SqlDbType.VarChar).Value = data["TIMEOFF"];
                        command.Parameters.Add("@TRAF_DENS", SqlDbType.VarChar).Value = data["TRAF_DENS"];
                        command.Parameters.Add("@COMPLEXITY", SqlDbType.VarChar).Value = data["COMPLEXITY"];
                        command.Parameters.Add("@HOURS", SqlDbType.VarChar).Value = data["HOURS"];
                        command.Parameters.Add("@TOTAL_HOURS", SqlDbType.VarChar).Value = data["TOTAL_HOURS"];
                        command.Parameters.Add("@PREBRIEF_COMMENTS_FILENAME", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS_FILENAME"];
                        command.Parameters.Add("@PREBRIEF_COMMENTS", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS"];
                        // command.Parameters.Add("@NOTES", SqlDbType.NVarChar).Value = data["NOTES"];
                        command.Parameters.Add("@ADDITIONAL_COMMENTS", SqlDbType.NVarChar).Value = data["ADDITIONAL_COMMENTS"];
                        command.Parameters.Add("@STUDENT_COMMENTS", SqlDbType.NVarChar).Value = data["STUDENT_COMMENTS"];

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
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }


            //push into CRITICALSKILL_RESULTS
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    //////////////////////////////////////////////////////////////////////
                    ///// REPORT TYPE 4 IS GIVEN, BE CAREFUL WHEN COPY/PASTE
                    string insert = @"with A as    
                                        (
                                            @replaceme@
                                        )
                                        INSERT INTO CRITICALSKILL_RESULTS
                                        SELECT " + reportid + @", RSC.ID , A.RES FROM REPORT_SKILL_CATEGORIES RSC
                                        JOIN A   ON RSC.CATEG_SKILL = A.CATEG
                                        WHERE RSC.REPORTTYPE = 4";

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


                    using (SqlCommand command = new SqlCommand(
                                 insert, connection))
                    {
                        connection.Open();
                        int rows = command.ExecuteNonQuery();

                        // if no rows are affected, rollback REPORTS_META, rollback REPORT_TOWERTR_GMC_ADC  and RETURN FALSE
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
                                        }
                                        using (SqlCommand com = new SqlCommand(
                                                     @"DELETE FROM REPORT_TOWERTR_GMC_ADC WHERE ID = " + reportid, connection))
                                        {
                                            con.Open();
                                            com.ExecuteNonQuery();
                                        }
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
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }

            //reaching here means everything went fine
            if (!push_UserTrainingFolder(reportid, data["genid"]))
                return "";

            return reportid;
        }

        public static string push_DAILYTR_ASS_RAD(Dictionary<string, string> data)
        {
            string reportid = "";


            //todo: push into  REPORTS_META
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                 @"INSERT INTO REPORTS_META VALUES('3', '" + data["OJTI_ID"] + "', convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , " + data["OJTI_SIGNED"] + ", " + data["TRAINEE_SIGNED"] + " , " + data["TRAINEE_ID"] + ")   ; SELECT SCOPE_IDENTITY()", connection))
                    {
                        connection.Open();
                        reportid = Convert.ToString(command.ExecuteScalar());
                        if (String.IsNullOrWhiteSpace(reportid))
                            return "";

                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }


            // push into REPORT_DAILYTR_ASS_RAD
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {

                    string insertstr = @"INSERT INTO REPORT_DAILYTR_ASS_RAD
                                               ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_ASS],[DATE],[POSITION],[POSITION_EXTRA],[TIMEON],[TIMEOFF],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS] )
                                        VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_ASS,@DATE,@POSITION,@POSITION_EXTRA,@TIMEON,@TIMEOFF,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS)";
                    using (SqlCommand command = new SqlCommand(insertstr, connection))
                    {
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = reportid;
                        command.Parameters.Add("@OJTI_ID", SqlDbType.Int).Value = data["OJTI_ID"];
                        command.Parameters.Add("@TRAINEE_ID", SqlDbType.Int).Value = data["TRAINEE_ID"];
                        command.Parameters.Add("@CHK_OJT", SqlDbType.Bit).Value = data["CHK_OJT"] == "1";
                        command.Parameters.Add("@CHK_ASS", SqlDbType.Bit).Value = data["CHK_ASS"] == "1";
                        command.Parameters.Add("@DATE", SqlDbType.VarChar).Value = data["DATE"];
                        command.Parameters.Add("@POSITION", SqlDbType.VarChar).Value = data["POSITION"];
                        command.Parameters.Add("@POSITION_EXTRA", SqlDbType.VarChar).Value = data["POSITION_EXTRA"];
                        command.Parameters.Add("@TIMEON", SqlDbType.VarChar).Value = data["TIMEON"];
                        command.Parameters.Add("@TIMEOFF", SqlDbType.VarChar).Value = data["TIMEOFF"];
                        command.Parameters.Add("@TRAF_DENS", SqlDbType.VarChar).Value = data["TRAF_DENS"];
                        command.Parameters.Add("@COMPLEXITY", SqlDbType.VarChar).Value = data["COMPLEXITY"];
                        command.Parameters.Add("@HOURS", SqlDbType.VarChar).Value = data["HOURS"];
                        command.Parameters.Add("@TOTAL_HOURS", SqlDbType.VarChar).Value = data["TOTAL_HOURS"];
                        command.Parameters.Add("@PREBRIEF_COMMENTS_FILENAME", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS_FILENAME"];
                        command.Parameters.Add("@PREBRIEF_COMMENTS", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS"];
                        command.Parameters.Add("@ADDITIONAL_COMMENTS", SqlDbType.NVarChar).Value = data["ADDITIONAL_COMMENTS"];
                        command.Parameters.Add("@STUDENT_COMMENTS", SqlDbType.NVarChar).Value = data["STUDENT_COMMENTS"];

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
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }


            //push into CRITICALSKILL_RESULTS
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    //////////////////////////////////////////////////////////////////////
                    ///// REPORT TYPE 4 IS GIVEN, BE CAREFUL WHEN COPY/PASTE
                    string insert = @"with A as    
                                        (
                                            @replaceme@
                                        )
                                        INSERT INTO CRITICALSKILL_RESULTS
                                        SELECT " + reportid + @", RSC.ID , A.RES FROM REPORT_SKILL_CATEGORIES RSC
                                        JOIN A   ON RSC.CATEG_SKILL = A.CATEG
                                        WHERE RSC.REPORTTYPE = 3";

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


                    using (SqlCommand command = new SqlCommand(
                                 insert, connection))
                    {
                        connection.Open();
                        int rows = command.ExecuteNonQuery();

                        // if no rows are affected, rollback REPORTS_META, rollback REPORT_TOWERTR_GMC_ADC  and RETURN FALSE
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
                                        }
                                        using (SqlCommand com = new SqlCommand(
                                                     @"DELETE FROM REPORT_DAILYTR_ASS_RAD WHERE ID = " + reportid, connection))
                                        {
                                            con.Open();
                                            com.ExecuteNonQuery();
                                        }
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
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }

            //reaching here means everything went fine
            if (!push_UserTrainingFolder(reportid, data["genid"]))
                return "";
            return reportid;
        }

        public static string push_DAILYTR_ASS_TWR(Dictionary<string, string> data)
        {
            string reportid = "";


            //todo: push into  REPORTS_META
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                 @"INSERT INTO REPORTS_META VALUES('2', '" + data["OJTI_ID"] + "', convert(varchar, getutcdate(), 20) ,  'OJTISUBMIT' , " + data["OJTI_SIGNED"] + ", " + data["TRAINEE_SIGNED"] + " , " + data["TRAINEE_ID"] + ")   ; SELECT SCOPE_IDENTITY()", connection))
                    {
                        connection.Open();
                        reportid = Convert.ToString(command.ExecuteScalar());
                        if (String.IsNullOrWhiteSpace(reportid))
                            return "";

                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }


            // push into REPORT_DAILYTR_ASS_RAD
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {

                    string insertstr = @"INSERT INTO REPORT_DAILYTR_ASS_TWR
                                               ([ID],[OJTI_ID],[TRAINEE_ID],[CHK_OJT],[CHK_ASS],[DATE],[POSITION],[TIMEON],[TIMEOFF],[TRAF_DENS],[COMPLEXITY],[HOURS],[TOTAL_HOURS],[PREBRIEF_COMMENTS_FILENAME],[PREBRIEF_COMMENTS],[ADDITIONAL_COMMENTS],[STUDENT_COMMENTS] )
                                        VALUES (@ID,@OJTI_ID,@TRAINEE_ID,@CHK_OJT,@CHK_ASS,@DATE,@POSITION,@TIMEON,@TIMEOFF,@TRAF_DENS,@COMPLEXITY,@HOURS,@TOTAL_HOURS,@PREBRIEF_COMMENTS_FILENAME,@PREBRIEF_COMMENTS,@ADDITIONAL_COMMENTS,@STUDENT_COMMENTS)";
                    using (SqlCommand command = new SqlCommand(insertstr, connection))
                    {
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = reportid;
                        command.Parameters.Add("@OJTI_ID", SqlDbType.Int).Value = data["OJTI_ID"];
                        command.Parameters.Add("@TRAINEE_ID", SqlDbType.Int).Value = data["TRAINEE_ID"];
                        command.Parameters.Add("@CHK_OJT", SqlDbType.Bit).Value = data["CHK_OJT"] == "1";
                        command.Parameters.Add("@CHK_ASS", SqlDbType.Bit).Value = data["CHK_ASS"] == "1";
                        command.Parameters.Add("@DATE", SqlDbType.VarChar).Value = data["DATE"];
                        command.Parameters.Add("@POSITION", SqlDbType.VarChar).Value = data["POSITION"];
                        command.Parameters.Add("@TIMEON", SqlDbType.VarChar).Value = data["TIMEON"];
                        command.Parameters.Add("@TIMEOFF", SqlDbType.VarChar).Value = data["TIMEOFF"];
                        command.Parameters.Add("@TRAF_DENS", SqlDbType.VarChar).Value = data["TRAF_DENS"];
                        command.Parameters.Add("@COMPLEXITY", SqlDbType.VarChar).Value = data["COMPLEXITY"];
                        command.Parameters.Add("@HOURS", SqlDbType.VarChar).Value = data["HOURS"];
                        command.Parameters.Add("@TOTAL_HOURS", SqlDbType.VarChar).Value = data["TOTAL_HOURS"];
                        command.Parameters.Add("@PREBRIEF_COMMENTS_FILENAME", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS_FILENAME"];
                        command.Parameters.Add("@PREBRIEF_COMMENTS", SqlDbType.NVarChar).Value = data["PREBRIEF_COMMENTS"];
                        command.Parameters.Add("@ADDITIONAL_COMMENTS", SqlDbType.NVarChar).Value = data["ADDITIONAL_COMMENTS"];
                        command.Parameters.Add("@STUDENT_COMMENTS", SqlDbType.NVarChar).Value = data["STUDENT_COMMENTS"];

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
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }


            //push into CRITICALSKILL_RESULTS
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    //////////////////////////////////////////////////////////////////////
                    ///// REPORT TYPE 4 IS GIVEN, BE CAREFUL WHEN COPY/PASTE
                    string insert = @"with A as    
                                        (
                                            @replaceme@
                                        )
                                        INSERT INTO CRITICALSKILL_RESULTS
                                        SELECT " + reportid + @", RSC.ID , A.RES FROM REPORT_SKILL_CATEGORIES RSC
                                        JOIN A   ON RSC.CATEG_SKILL = A.CATEG
                                        WHERE RSC.REPORTTYPE = 2";

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


                    using (SqlCommand command = new SqlCommand(
                                 insert, connection))
                    {
                        connection.Open();
                        int rows = command.ExecuteNonQuery();

                        // if no rows are affected, rollback REPORTS_META, rollback REPORT_TOWERTR_GMC_ADC  and RETURN FALSE
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
                                        }
                                        using (SqlCommand com = new SqlCommand(
                                                     @"DELETE FROM REPORT_DAILYTR_ASS_TWR WHERE ID = " + reportid, connection))
                                        {
                                            con.Open();
                                            com.ExecuteNonQuery();
                                        }
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
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return "";
            }

            //reaching here means everything went fine
            if (!push_UserTrainingFolder(reportid, data["genid"]))
                return "";

            return reportid;
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
FROM TRAINING_TREE_STEPS WHERE POSITION = @POSITION AND SECTOR =@SECTOR ORDER BY ID ASC ", connection))
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
                {
                    using (SqlCommand command = new SqlCommand(
                                @"  SELECT TTS.DESCRIPTION , UTF.STATUS , UTF.CREATED_TIME, ISNULL(UTF.REPORTID,'') AS [Rpt.Num.] , ISNULL(UTF.FILENAME,'') AS [FileName]
		                                , UTF.GENID, TTS.PHASE , TTS.[NAME]
                                        FROM USER_TRAINING_FOLDER UTF
                                        JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID
                                        WHERE UTF.EMPLOYEEID = @EMPLOYEEID 
	                                          AND TTS.POSITION = @POSITION 
	                                          AND ( TTS.SECTOR = @SECTOR OR ISNULL(TTS.SECTOR, '') = '' )
                                        ORDER BY TTS.ID DESC, UTF.GENID DESC", connection))
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

            //first add FDO step, it is always inserted
            //using (SqlConnection connection = new SqlConnection(con_str))
            //using (SqlCommand command = new SqlCommand(
            //        @" DECLARE @FDOID INT = (SELECT ID FROM TRAINING_TREE_STEPS WHERE POSITION ='APP' AND PHASE = 'FDO')
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
                //insert all steps before chosen step in the sector  as MIGRATION
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                                 @"  INSERT INTO USER_TRAINING_FOLDER (STEPID , EMPLOYEEID, CREATED_TIME, [STATUS] , REPORTID , [FILENAME] , COMMENTS)
                                    SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'MIGRATION' , NULL, NULL, NULL FROM TRAINING_TREE_STEPS TTS WHERE TTS.POSITION = 'APP' AND TTS.SECTOR = @SECTOR AND TTS.ID < @STEPID   ", connection))
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
                                    SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'ONGOING' , NULL, NULL, NULL FROM TRAINING_TREE_STEPS TTS WHERE TTS.POSITION = 'APP' AND TTS.SECTOR = @SECTOR AND  TTS.ID = @STEPID ", connection))
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

            ////first add FDO step, it is always inserted
            //using (SqlConnection connection = new SqlConnection(con_str))
            //using (SqlCommand command = new SqlCommand(
            //        @" DECLARE @FDOID INT = (SELECT ID FROM TRAINING_TREE_STEPS WHERE POSITION ='ACC' AND PHASE = 'FDO')
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


            if (sector == "GMC" || sector == "ADC")
            {
                // add ASIST as MIGRATION

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @" DECLARE @ASSISTID INT = (SELECT ID FROM TRAINING_TREE_STEPS WHERE POSITION ='TWR' AND SECTOR = 'ASSIST')
                                INSERT INTO USER_TRAINING_FOLDER VALUES (@ASSISTID , @EMPLOYEEID , convert(varchar, getutcdate(), 20) , 'MIGRATION' , NULL, NULL, NULL)  ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.CommandType = CommandType.Text;

                    if (command.ExecuteNonQuery() != 1)
                        return false; //something went wrong
                }
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
                                    SELECT TTS.ID , @EMPLOYEEID, convert(varchar, getutcdate(), 20), 'ONGOING' , NULL, NULL, NULL FROM TRAINING_TREE_STEPS TTS WHERE TTS.POSITION = 'TWR' AND TTS.SECTOR = @SECTOR AND  TTS.ID = @STEPID ", connection))
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


    }
}