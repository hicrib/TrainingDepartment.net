using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AviaTrain.App_Code
{
    public static class DB_Stats
    {

        public static DataTable get_TrainingHours(string start, string finish, string trainee = "0", string unit = "", string sector = "")
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                            @"  
 
SELECT  (SELECT INITIAL FROM USERS WHERE EMPLOYEEID = TRAINEE_ID ) AS 'INITIAL'
, [HOURS], NOTRAINING,NOSHOW , POSITION , TIMEON_SCH, TIMEOFF_SCH
FROM REPORT_DAILYTR_ASS_RAD
WHERE convert(datetime, [DATE], 20) >= convert(datetime, @START, 20) 
	AND convert(datetime, [DATE], 20) <=   convert(datetime, @FINISH, 20)
	AND TRAINEE_ID = CASE WHEN  @TRAINEE = '0' THEN TRAINEE_ID ELSE @TRAINEE END
	AND 1 = CASE WHEN  @UNIT IN  ('','APP','ACC') AND POSITION_EXTRA LIKE '%' + @UNIT + '%' THEN 1
				 ELSE 0 END
	AND POSITION = CASE WHEN @SECTOR = '' THEN POSITION ELSE @SECTOR  END


UNION ALL

SELECT (SELECT INITIAL FROM USERS WHERE EMPLOYEEID = TRAINEE_ID ) AS 'INITIAL'
, [HOURS], NOTRAINING,NOSHOW  , POSITION , TIMEON_SCH, TIMEOFF_SCH
FROM REPORT_DAILYTR_ASS_TWR
WHERE convert(datetime, [DATE], 20) >= convert(datetime, @START, 20) 
	AND convert(datetime, [DATE], 20) <=   convert(datetime, @FINISH, 20)
	AND TRAINEE_ID = CASE WHEN  @TRAINEE = '0' THEN TRAINEE_ID ELSE @TRAINEE END
	AND 1 = CASE WHEN @UNIT IN  ('','TWR') THEN 1
				 ELSE 0 END
	AND 1 = CASE WHEN @SECTOR = '' THEN 1 
				 WHEN @SECTOR = 'ASSIST' THEN 1
				 ELSE 0  END

UNION ALL

SELECT  (SELECT INITIAL FROM USERS WHERE EMPLOYEEID = TRAINEE_ID ) AS 'INITIAL'
, [HOURS], NOTRAINING,NOSHOW  ,  POSITION , TIMEON_SCH, TIMEOFF_SCH
FROM REPORT_TR_ARE_APP_RAD
WHERE convert(datetime, [DATE], 20) >= convert(datetime, @START, 20) 
	AND convert(datetime, [DATE], 20) <=   convert(datetime, @FINISH, 20)
	AND TRAINEE_ID = CASE WHEN  @TRAINEE = '0' THEN TRAINEE_ID ELSE @TRAINEE END
	AND 1 = CASE WHEN  @UNIT IN  ('','APP','ACC') AND POSITION_EXTRA LIKE '%' + @UNIT + '%' THEN 1
				 ELSE 0 END
	AND POSITION = CASE WHEN @SECTOR = '' THEN POSITION ELSE @SECTOR END
	
UNION ALL

SELECT  (SELECT INITIAL FROM USERS WHERE EMPLOYEEID = TRAINEE_ID ) AS 'INITIAL'
, [HOURS], NOTRAINING,NOSHOW  , POSITION , TIMEON_SCH, TIMEOFF_SCH
FROM REPORT_TOWERTR_GMC_ADC
WHERE convert(datetime, [DATE], 20) >= convert(datetime, @START, 20) 
	AND convert(datetime, [DATE], 20) <=   convert(datetime, @FINISH, 20)
	AND TRAINEE_ID = CASE WHEN  @TRAINEE = '0' THEN TRAINEE_ID ELSE @TRAINEE END
	AND 1 = CASE WHEN @UNIT IN  ('','TWR') THEN 1
				 ELSE 0 END
	AND 1 = CASE WHEN @SECTOR = '' THEN 1 
				 WHEN POSITION LIKE '%'+ @SECTOR + '%' THEN 1
				 ELSE 0  END

ORDER BY INITIAL
                                ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@START", SqlDbType.NVarChar).Value = start;
                    command.Parameters.Add("@FINISH", SqlDbType.NVarChar).Value = finish;

                    command.Parameters.Add("@TRAINEE", SqlDbType.Int).Value = trainee;
                    command.Parameters.Add("@UNIT", SqlDbType.NVarChar).Value = unit;
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
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


        public static bool login_positionlog( string position, string sector, string userid, string date, string timeon, string trainee )
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                    @"INSERT INTO POSITION_LOG 
                        VALUES ( CONVERT(VARCHAR, GETUTCDATE(),20) 
		                        , @POSITION 
		                        , @SECTOR
		                        , @CONTROLLER
		                        , @DATEON 
		                        , @TIMEON 
		                        , NULL
		                        , @TRAINEE 
		                        , NULL
		                        , NULL
		                         )", connection)) // NULLS : tımeoff, cob, duration (those make sense in an update clause)
                {
                    connection.Open();
                    command.Parameters.Add("@POSITION", SqlDbType.VarChar).Value = position;
                    command.Parameters.Add("@SECTOR", SqlDbType.VarChar).Value = sector;
                    command.Parameters.Add("@CONTROLLER", SqlDbType.Int).Value = userid;
                    command.Parameters.Add("@DATEON", SqlDbType.VarChar).Value = date;
                    command.Parameters.Add("@TIMEON", SqlDbType.VarChar).Value = timeon;
                    command.Parameters.Add("@TRAINEE", SqlDbType.Int).Value = trainee == "" ? (object)DBNull.Value : trainee;
                    command.CommandType = CommandType.Text;

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }

            //something went wrong
            return false;
        }

        public static DataTable get_positionlog_page(string position, string sector)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                    @"  SELECT * FROM (
                            SELECT TOP 10 ID, DATEON , TIMEON , U.INITIAL AS 'CONTROLLER' , 
                            CASE WHEN ISNULL(TRAINEE,'') = '' THEN '' ELSE (SELECT INITIAL FROM USERS WHERE EMPLOYEEID = TRAINEE) END AS 'TRAINEE'
                            FROM POSITION_LOG L
                            JOIN USERS U ON U.EMPLOYEEID = L.CONTROLLER
                            WHERE POSITION = @POSITION AND SECTOR = @SECTOR 
                            ORDER BY ID  DESC
                         ) AS A 
                         ORDER BY ID ASC  ", connection))
                {
                    connection.Open();

                    command.Parameters.Add("@POSITION", SqlDbType.NVarChar).Value = position;
                    command.Parameters.Add("@SECTOR", SqlDbType.NVarChar).Value = sector;
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