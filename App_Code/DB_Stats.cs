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
, [HOURS], NOTRAINING,NOSHOW , POSITION
FROM REPORT_DAILYTR_ASS_RAD
WHERE convert(datetime, [DATE], 20) >= convert(datetime, @START, 20) 
	AND convert(datetime, [DATE], 20) <=   convert(datetime, @FINISH, 20)
	AND TRAINEE_ID = CASE WHEN  @TRAINEE = '0' THEN TRAINEE_ID ELSE @TRAINEE END
	AND 1 = CASE WHEN  @UNIT IN  ('','APP','ACC') AND POSITION_EXTRA LIKE '%' + @UNIT + '%' THEN 1
				 ELSE 0 END
	AND POSITION = CASE WHEN @SECTOR = '' THEN POSITION ELSE @SECTOR  END


UNION ALL

SELECT (SELECT INITIAL FROM USERS WHERE EMPLOYEEID = TRAINEE_ID ) AS 'INITIAL'
, [HOURS], NOTRAINING,NOSHOW  , POSITION
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
, [HOURS], NOTRAINING,NOSHOW  ,  POSITION
FROM REPORT_TR_ARE_APP_RAD
WHERE convert(datetime, [DATE], 20) >= convert(datetime, @START, 20) 
	AND convert(datetime, [DATE], 20) <=   convert(datetime, @FINISH, 20)
	AND TRAINEE_ID = CASE WHEN  @TRAINEE = '0' THEN TRAINEE_ID ELSE @TRAINEE END
	AND 1 = CASE WHEN  @UNIT IN  ('','APP','ACC') AND POSITION_EXTRA LIKE '%' + @UNIT + '%' THEN 1
				 ELSE 0 END
	AND POSITION = CASE WHEN @SECTOR = '' THEN POSITION ELSE @SECTOR END
	
UNION ALL

SELECT  (SELECT INITIAL FROM USERS WHERE EMPLOYEEID = TRAINEE_ID ) AS 'INITIAL'
, [HOURS], NOTRAINING,NOSHOW  , POSITION
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



    }
}