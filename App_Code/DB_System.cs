using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AviaTrain.App_Code
{

    public static class DB_System
    {
        public static string con_str_hosting = ConfigurationManager.ConnectionStrings["local_dbconn"].ConnectionString;
        public static string con_str = ConfigurationManager.ConnectionStrings["dbconn_hosting"].ConnectionString;

        public static void log_pages(string employeeid, string page)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @"INSERT INTO LOG_PAGES VALUES (@EMPLOYEEID, @PAGE , convert(varchar, getutcdate(), 20) ) ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@PAGE", SqlDbType.NVarChar).Value = page;
                    command.CommandType = CommandType.Text;
                    int rows = command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
        }

        //returns null if invalid
        //returns user's id if valid
        public static string isLoginValid(string username, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                 "SELECT [EMPLOYEEID] FROM dbo.[USERS] WHERE [EMPLOYEEID] = @EMPLOYEEID AND [PASSWORD] = @PASSWORD AND ISACTIVE=1", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = username;
                        command.Parameters.Add("@PASSWORD", SqlDbType.NVarChar).Value = password;
                        command.CommandType = CommandType.Text;
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
                return e.Message;
            }

            return null;
        }

        public static string getUserInfo(string employeeid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                 "SELECT FIRSTNAME + ' ' + SURNAME AS 'NAME' FROM USERS WHERE ISACTIVE=1 AND EMPLOYEEID =" + employeeid, connection))
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

        // returns null if there is a problem in connection
        // returns 0 rows if theres no role or privilege
        // [ROLEID],  [ROLENAME],  [PRIVID], [PRIVNAME]
        public static DataTable get_ALL_Privileges_of_Person(string employeeid)
        {
            if (String.IsNullOrEmpty(employeeid))
                return null;

            DataTable dt = new DataTable();


            //getting roles and privileges of the person first in a DataTable
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT 
                                UR.ROLEID [ROLEID], rdef.[NAME] [ROLENAME], rdef.EXPLANATION [ROLEEXPLANATION] 
                                    FROM DBO.USER_ROLES ur
                                    JOIN DBO.ROLE_DEFINITION rdef ON ur.ROLEID = rdef.ID
                                    WHERE UR.EMPLOYEEID =" + employeeid, connection))
                    {
                        connection.Open();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);

                        if (dt == null || dt.Rows.Count == 0)
                            return null;

                        return dt;
                    }
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }


            return null;
        }



        public static DataTable get_APP_ACC_Positions()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("SHORT");
            dt.Columns.Add("LONG");
            dt.Rows.Add(new object[] { "NRH", "ACC-NRH" });
            dt.Rows.Add(new object[] { "NRL", "ACC-NRL" });
            dt.Rows.Add(new object[] { "SRL", "ACC-SRL" });
            dt.Rows.Add(new object[] { "SRH", "ACC-SRH" });
            dt.Rows.Add(new object[] { "ARN", "APP-ARN" });
            dt.Rows.Add(new object[] { "ARS", "APP-ARS" });
            dt.Rows.Add(new object[] { "BRN", "APP-BRN" });
            dt.Rows.Add(new object[] { "BRS", "APP-BRS" });
            dt.Rows.Add(new object[] { "KR", "APP-KR" });

            return dt;
        }

        public static DataTable get_Positions()
        {

            //TODO : HERE WE CAN MAKE SOME BUSINESS LOGIC FOR TRAINING TREE. WHO CAN or SHOULD TRAIN FOR PARTICULAR POSITIONS
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT '-' AS CODE  
                                    UNION
                                    SELECT DISTINCT POSITION AS CODE FROM POSITON_SECTOR", connection))
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

        public static DataTable get_Sectors(string position)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT '-' AS [CODE]    
                                    UNION  
                                    SELECT DISTINCT EXTRA AS [CODE] FROM POSITON_SECTOR WHERE POSITION = @POSITION", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
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

        public static DataTable get_Sectors()
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT '-' AS [CODE]    
                                    UNION  
                                    SELECT DISTINCT POSITION + '-' + SECTOR AS [CODE] FROM POSITON_SECTOR ", connection))
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


        public static DataTable get_Sectors_withpos(string position)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @" SELECT '-' AS [CODE]    
                                    UNION  
                                    SELECT DISTINCT [DESCRIPTION] AS [CODE] FROM POSITON_SECTOR WHERE POSITION = @POSITION", connection))
                    {
                        connection.Open();
                        command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
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

        public static bool add_role_to_user(string employeeid, string roleid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @" IF NOT EXISTS (SELECT TOP 1 ROLEID FROM USER_ROLES WHERE EMPLOYEEID = @EMPLOYEEID AND ROLEID = @ROLEID)
                                BEGIN
	                                INSERT INTO USER_ROLES VALUES (@EMPLOYEEID , @ROLEID )
                                END", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@ROLEID", SqlDbType.Int).Value = roleid;
                    command.CommandType = CommandType.Text;
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                        return true;

                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }
        public static bool remove_role_from_user(string employeeid, string roleid)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @" DELETE FROM USER_ROLES WHERE EMPLOYEEID = @EMPLOYEEID AND ROLEID = @ROLEID ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@ROLEID", SqlDbType.Int).Value = roleid;
                    command.CommandType = CommandType.Text;
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                        return true;
                        
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }


        public static DataTable get_ALL_Roles()
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"
                                select [ID], NAME , EXPLANATION  from ROLE_DEFINITION ORDER BY NAME", connection))
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

        public static DataTable get_ALL_OJTI_LCE_EXAMINER()
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT '-' AS [ID], ' --- ' AS [NAME]
                                  UNION
                                  SELECT DISTINCT
                                            U.[EMPLOYEEID] AS [ID], U.INITIAL + ' - ' + U.FIRSTNAME +' '+ U.SURNAME AS [NAME] 
                                  FROM USER_ROLES UR
                                  JOIN ROLE_DEFINITION RDEF  ON UR.ROLEID=RDEF.ID
                                  JOIN USERS U ON UR.EMPLOYEEID = U.EMPLOYEEID
                                  WHERE (RDEF.[NAME]='OJTI' OR RDEF.[NAME]='LCE' OR RDEF.[NAME]='EXAMINER') AND U.ISACTIVE=1 
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

        public static DataTable get_ALL_trainees()
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                {
                    using (SqlCommand command = new SqlCommand(
                                @"SELECT '-' AS [ID], ' --- ' AS [NAME]
                                  UNION
                                  SELECT DISTINCT
                                            U.[EMPLOYEEID] AS [ID], U.INITIAL + ' - ' + U.FIRSTNAME +' '+ U.SURNAME AS [NAME] 
                                  FROM USER_ROLES UR
                                  JOIN ROLE_DEFINITION RDEF  ON UR.ROLEID=RDEF.ID
                                  JOIN USERS U ON UR.EMPLOYEEID = U.EMPLOYEEID
                                  WHERE RDEF.[NAME] = 'TRAINEE' AND U.ISACTIVE=1 ", connection))
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

        public static DataTable get_ALL_Users(bool with_empty = true ,  bool isactive = true)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @"" + 
                            (with_empty ?
                            @"SELECT '-' AS [ID], ' --- ' AS [NAME]
                            UNION" : "" ) +
                         @" SELECT EMPLOYEEID AS ID ,  INITIAL + ' - ' + FIRSTNAME +' '+ SURNAME AS [NAME] 
                            FROM USERS " + (isactive ? " WHERE ISACTIVE = 1 " : " ") +
                            "ORDER BY NAME"
                                   , connection))
                {
                    connection.Open();
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

        public static bool createUser_withRoles(Dictionary<string, string> userinfo, List<string> roles)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                            @"INSERT INTO USERS VALUES ( @EMPLOYEEID, @NAME, @SURNAME, @PASSWORD, @INITIAL ,1) ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = userinfo["employeeid"];
                    command.Parameters.Add("@NAME", SqlDbType.NVarChar).Value = userinfo["firstname"];
                    command.Parameters.Add("@SURNAME", SqlDbType.NVarChar).Value = userinfo["surname"]; ;
                    command.Parameters.Add("@PASSWORD", SqlDbType.NVarChar).Value = userinfo["password"]; ;
                    command.Parameters.Add("@INITIAL", SqlDbType.NVarChar).Value = userinfo["initial"]; ;

                    command.CommandType = CommandType.Text;
                    int rows = command.ExecuteNonQuery();

                    if (rows == 0)
                        return false;

                    connection.Close();

                    foreach (string roleid in roles)
                    {
                        using (SqlCommand cmd = new SqlCommand(@" INSERT INTO USER_ROLES VALUES ( @EMPLOYEEID, @ROLEID) ", connection))
                        {
                            connection.Open();
                            cmd.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = userinfo["employeeid"];
                            cmd.Parameters.Add("@ROLEID", SqlDbType.NVarChar).Value = roleid;
                            cmd.CommandType = CommandType.Text;
                            rows = cmd.ExecuteNonQuery();
                            //todo : error check is to be implemented
                            if (rows == 0)
                                return false;
                            connection.Close();
                        }
                    }
                    return true;

                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            return false;
        }




        public static string get_MER(string employeeid, string position, string sector, string phase)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                             @"DECLARE @MER INT = ( SELECT MER FROM MER_DEFAULT WHERE POSITION = @POSITION AND SECTOR=@SECTOR AND PHASE = @PHASE )

                                    IF @MER = NULL
                                    BEGIN
                                    SET @MER = (SELECT MER FROM MER_USER WHERE EMPLOYEEID=@EMPLOYEEID AND POSITION = @POSITION AND SECTOR=@SECTOR AND PHASE = @PHASE  )
                                    END

                                    SELECT ISNULL(@MER,0)", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                    command.Parameters.Add("@PHASE", SqlDbType.NVarChar).Value = phase;
                    command.CommandType = CommandType.Text;

                    string result = Convert.ToString(command.ExecuteScalar());
                    if (String.IsNullOrWhiteSpace(result))
                        return null;
                    else
                        return result;

                }

            }
            catch (Exception e)
            {
                string err = e.Message;
                return e.Message;
            }
        }

        public static string get_TOTALHOURS(string employeeid, string position, string sector, string phase)
        {
            //todo : now, it's getting cumulative total_hours of the sector,  starting from ASSIST training, PREOJT, OJT Levels

            // gets the last report and take the total_hours field
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                             @"
                                DECLARE @REPORTID INT = 
                                (
	                                SELECT MAX(UTF.REPORTID)
	                                 FROM USER_TRAINING_FOLDER UTF
	                                 JOIN TRAINING_TREE_STEPS TTS ON TTS.ID = UTF.STEPID
	                                 WHERE UTF.EMPLOYEEID = @EMPLOYEEID
		                                   AND TTS.POSITION = @POSITION 
		                                   AND ( TTS.SECTOR = @SECTOR OR ISNULL(TTS.SECTOR, '') = '' )
		                                   AND UTF.STATUS = 'REPORT'
		                                   AND (LEFT(UTF.STATUS, 5 ) != 'RECOM')
                                )

                                DECLARE @REPORTTYPE INT = (SELECT [TYPE] FROM REPORTS_META WHERE ID = @REPORTID )

                                IF @REPORTTYPE = 1 
                                BEGIN 
	                                SELECT TOTAL_HOURS FROM REPORT_TR_ARE_APP_RAD WHERE ID = @REPORTID
                                END
                                ELSE IF @REPORTTYPE = 2
                                BEGIN
	                                SELECT TOTAL_HOURS FROM REPORT_DAILYTR_ASS_TWR WHERE ID = @REPORTID
                                END
                                ELSE IF @REPORTTYPE = 3
                                BEGIN
	                                SELECT TOTAL_HOURS FROM REPORT_DAILYTR_ASS_RAD WHERE ID = @REPORTID
                                END
                                ELSE IF @REPORTTYPE = 4
                                BEGIN
	                                SELECT TOTAL_HOURS FROM REPORT_TOWERTR_GMC_ADC WHERE ID = @REPORTID
                                END
                                            ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = employeeid;
                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
                    command.CommandType = CommandType.Text;

                    string result = Convert.ToString(command.ExecuteScalar());
                    if (String.IsNullOrWhiteSpace(result))
                        return null;
                    else
                        return result;

                }

            }
            catch (Exception e)
            {
                string err = e.Message;
                return e.Message;
            }


            return "0";
        }




        public static DataTable get_ROLES_PAGES(string employeeid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                           @"SELECT DISTINCT PAGE_NAME
                                FROM ROLES_PAGES RP
                                JOIN ROLE_DEFINITION RDEF ON RDEF.NAME = RP.ROLE_NAME
                                JOIN USER_ROLES UR ON UR.ROLEID = RDEF.ID
                                WHERE UR.EMPLOYEEID = @USERID"
                                   , connection))
                {
                    connection.Open();
                    command.Parameters.Add(@"USERID", SqlDbType.Int).Value = employeeid;
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