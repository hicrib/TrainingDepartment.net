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
        public static string con_str_hosting = ConfigurationManager.ConnectionStrings["local_dbconn"].ConnectionString;
        public static string con_str = ConfigurationManager.ConnectionStrings["dbconn_hosting"].ConnectionString;

        public static bool NoShow()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(con_str))
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

    }
}