using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AviaTrain.App_Code
{
    public static class DB_ScheduledTasks
    {
        public static bool NoShow()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(@"
UPDATE EXM_EXAM_ASSIGNMENT
SET [STATUS] = 'NOSHOW'
WHERE [STATUS] <> 'NOSHOW' AND ISNULL(USER_FINISH, '') = '' AND CONVERT(DATETIME, GETUTCDATE(), 20) > CONVERT(DATETIME, SCHEDULE_FINISH,20)
				                  

UPDATE TRN_ASSIGNMENT
SET [STATUS]= 'NOSHOW'
WHERE [STATUS] <> 'NOSHOW' AND ISNULL(USER_FINISH, '') = '' AND CONVERT(DATETIME, GETUTCDATE(), 20) > CONVERT(DATETIME, SCHEDULE_FINISH,20)
", connection))
                {
                    connection.Open();
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


        public static bool NoSign_24hour(string traineeid = "", string instructorid = "", string sysadminid = "")
        {
            string add = "";
            if (sysadminid != "")
                add = "";
            else if (traineeid != "")
                add += " AND TRAINEE_ID = " + traineeid;
            else if (instructorid != "")
                add += " AND CREATER = " + instructorid;

            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(@"
UPDATE REPORT_RECOM_CERTIF
SET TRAINEE_SIGNED = 1  , TRAINEE_SIGN_DATE = CONVERT(VARCHAR, GETUTCDATE(),20)
WHERE TRAINEE_SIGNED = 0 
AND 
dateADD(DAY, 1, CONVERT(DATETIME,  [DATE], 20))  <  CONVERT(DATETIME, GETUTCDATE(), 20)

UPDATE REPORT_RECOM_LEVEL
SET TRAINEE_SIGNED = 1  
WHERE TRAINEE_SIGNED = 0 
AND 
dateADD(DAY, 1, CONVERT(DATETIME,  [DATE], 20))  <  CONVERT(DATETIME, GETUTCDATE(), 20)

UPDATE REPORTS_META
SET TRAINEE_SIGNED = 1
WHERE TRAINEE_SIGNED = 0 
AND 
dateADD(DAY, 1, CONVERT(DATETIME,  CREATE_TIME, 20))  <  CONVERT(DATETIME, GETUTCDATE(), 20)" + add, connection))
                {
                    connection.Open();
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

    }
}