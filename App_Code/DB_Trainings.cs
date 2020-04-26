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
        public static string push_Training_Info_Get_Ids(string name, string sector, string effective )
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];

            try
            {

                using (SqlConnection connection = new SqlConnection(con_str))
                using (SqlCommand command = new SqlCommand(
                        @"  INSERT INTO TRN_TRAINING_DEF 
                            VALUES ( @NAME , @SECTOR , @EFFECTIVE  , @USERID , CONVERT(VARCHAR , GETUTCDATE(), 20 ), 1)

                            DECLARE @TRN_ID INT = (SELECT SCOPE_IDENTITY())


                            INSERT INTO TRN_STEP 
                            VALUES( @TRN_ID , null , null , null )

                            SELECT CONVERT(VARCHAR ,@TRN_ID) + ',' + CONVERT(VARCHAR, SCOPE_IDENTITY())  ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@NAME", SqlDbType.VarChar).Value = name;
                    command.Parameters.Add("@SECTOR", SqlDbType.VarChar).Value = sector;
                    command.Parameters.Add("@EFFECTIVE", SqlDbType.VarChar).Value = effective;
                    command.Parameters.Add("@USERID", SqlDbType.VarChar).Value = user.employeeid;
                    command.CommandType = CommandType.Text;

                    string trn_step_id = Convert.ToString(command.ExecuteScalar());
                    return trn_step_id;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return "";
        }

    }
}