using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AviaTrain.App_Code
{
    public class DB_FileSys
    {
        public static DataRow get_UserMain(string user)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                            @"
IF NOT EXISTS (SELECT ID,[NAME] FROM USER_FILESYSTEM WHERE USERID=@USERID AND PARENT_ID IS NULL)
BEGIN
	INSERT INTO USER_FILESYSTEM VALUES (@USERID,'Main', null, null, @USERID, convert(varchar, getutcdate(),20) , 1 )
END

SELECT ID,[NAME] FROM USER_FILESYSTEM WHERE USERID=@USERID AND PARENT_ID IS NULL", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@USERID", user);
                    command.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(res);

                    if (res == null || res.Rows.Count == 0)
                        return null;

                    return res.Rows[0];
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
            }
            return null;
        }

        public static DataTable get_ChildNodes(string parentid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                            @"
                             SELECT UFS.ID, UFS.[NAME], ISNULL(UFS.FILEID,0) AS 'FILEID' ,
                                     ISNULL(F.ADDRES,'') AS 'ADDRESS',
		                             ISNULL(F.ISSUE_DATE, '' ) AS 'Issued',
		                             ISNULL(F.EXPIRATION_DATE, '' ) AS 'Expires',
		                             ISNULL(F.ROLE_SPECIFIC, '' ) AS 'Roles',
		                             ISNULL(FT.TYPENAME,'') AS 'Type'
                             FROM USER_FILESYSTEM UFS
                             LEFT JOIN USER_FILES F ON F.ID = UFS.FILEID AND F.ISACTIVE = 1
                             LEFT JOIN FILE_TYPES FT ON FT.ID = F.FILETYPE 
                             WHERE UFS.PARENT_ID = @PARENT AND UFS.ISACTIVE = 1", connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@PARENT", parentid);
                    command.CommandType = CommandType.Text;

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


        public static bool create_NewFolder(string foldername, string userid, string parentid)
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                            @"INSERT INTO USER_FILESYSTEM VALUES(@USERID,@NAME,NULL,@PARENTID, @CREATERID,CONVERT(varchar,GETUTCDATE(),20),1)", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = userid;
                    command.Parameters.Add("@NAME", SqlDbType.NVarChar).Value = foldername;
                    command.Parameters.Add("@PARENTID", SqlDbType.Int).Value = parentid;
                    command.Parameters.Add("@CREATERID", SqlDbType.Int).Value = user.employeeid;
                    command.CommandType = CommandType.Text;

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
            }
            return false;

        }

        public static bool delete_File(string fs_id )
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                          @"UPDATE USER_FILESYSTEM SET ISACTIVE = 0 WHERE ID = @ID

                            UPDATE USER_FILES SET ISACTIVE = 0
                            WHERE ID = (SELECT FILEID FROM USER_FILESYSTEM WHERE ID = @ID )", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = fs_id;
                    command.CommandType = CommandType.Text;

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        

        public static bool upload_UserFile(string userid, string parentid,
                                                    string filetypeid, string filename, string fileaddress, 
                                                    string issuedate="", string expirationdate="", string rolespec="" )
        {
            UserSession user = (UserSession)System.Web.HttpContext.Current.Session["usersession"];
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                                        @"INSERT INTO USER_FILES
                                          SELECT    @FILETYPE , @FILENAME, @FILEADRES, 1, 
	                                                CASE WHEN @ISSUE = '' THEN NULL ELSE @ISSUE END,
	                                                CASE WHEN @EXPIRES = '' THEN NULL ELSE @EXPIRES END,
	                                                CASE WHEN @ROLSPEC = '' THEN NULL ELSE @ROLSPEC END,
                                                    @USERID

                                        DECLARE @FILEID INT = (SELECT SCOPE_IDENTITY())

                                        INSERT INTO USER_FILESYSTEM
                                        SELECT @USERID , @FILENAME, @FILEID , @PARENTID, @BY, CONVERT(VARCHAR,GETUTCDATE(),20), 1
                                        ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@FILETYPE", SqlDbType.Int).Value = filetypeid;
                    command.Parameters.Add("@FILENAME", SqlDbType.NVarChar).Value = filename;
                    command.Parameters.Add("@FILEADRES", SqlDbType.NVarChar).Value = fileaddress;
                    command.Parameters.Add("@ISSUE", SqlDbType.NVarChar).Value = issuedate;
                    command.Parameters.Add("@EXPIRES", SqlDbType.NVarChar).Value = expirationdate;
                    command.Parameters.Add("@ROLSPEC", SqlDbType.NVarChar).Value = rolespec;
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = userid;
                    command.Parameters.Add("@PARENTID", SqlDbType.Int).Value = parentid;
                    command.Parameters.Add("@BY", SqlDbType.Int).Value = user.employeeid;
                    command.CommandType = CommandType.Text;

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception e )
            {
                string em = e.Message;
            }
            return false;

        }

        public static bool create_filetype(string name, bool issue, bool expire, bool rolespec)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                            @"INSERT INTO FILE_TYPES VALUES (@NAME , @ISSUE , @EXPIRE, @ROLESPEC) ", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@NAME", SqlDbType.NVarChar).Value = name;
                    command.Parameters.Add("@ISSUE", SqlDbType.Bit).Value = issue;
                    command.Parameters.Add("@EXPIRE", SqlDbType.Bit).Value = expire;
                    command.Parameters.Add("@ROLESPEC", SqlDbType.Bit).Value = rolespec;
                    command.CommandType = CommandType.Text;

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static DataTable get_filetypes()
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                            @" SELECT   TYPENAME AS 'Name' , ISSUE_DATE as 'Issue Date' , 
                                        EXPIRATION_DATE as 'Expires', ROLE_SPECIFIC as 'Role Specific', ID
                               FROM FILE_TYPES", connection))
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


        public static DataTable get_user_filetype(string userid, string typeid)
        {
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(Con_Str.current))
                using (SqlCommand command = new SqlCommand(
                            @"select F.* FROM  USER_FILES F 
                            WHERE F.FILETYPE = @TYPE AND F.USERID = @USERID AND F.ISACTIVE = 1", connection))
                {
                    connection.Open();
                    command.Parameters.Add("@TYPE", SqlDbType.Int).Value = typeid;
                    command.Parameters.Add("@USERID", SqlDbType.Int).Value = userid;

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(res);

                    return res;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}