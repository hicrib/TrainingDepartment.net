using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace AviaTrain.App_Code
{

    //ALL THE SMALL NEEDS
    public static class Utility
    {
        public static Dictionary<int, string> MessageCodes = new Dictionary<int, string>
        {
            { 0 , "SYSTEM FAULT. TRY AGAIN LATER."},
            { 1 , "REPORT STORED SUCCESFULLY. REPORT NUMBER : "},
             { 2 , "YOU DON'T HAVE VIEWING PRIVILEGE. CONTACT SYSTEM ADMIN. "},
             { 3 , "YOU DON'T HAVE VIEWING PRIVILEGE FOR REPORT NUMBER : "},
             { 4 , "UNEXPECTED ERROR VIEWING REPORT NUMBER : "},
             { 5 , "TRAINEE SIGNED AND SUCCESSFULLY STORED. REPORT NUMBER : "},
            { 6, "TRAINEE FOLDER HAS STARTED, FOR TRAINEE : " }
        };


        public static bool isOJTI(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return false;

                DataRow[] o = dt.Select("ROLENAME = 'OJTI'");
                if (o.Length > 0)
                    return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        public static bool isLCE(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return false;

                DataRow[] o = dt.Select("ROLENAME = 'LCE'");
                if (o.Length > 0)
                    return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        public static bool isEXAMTRAINEE(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return false;

                DataRow[] o = dt.Select("ROLENAME = 'EXAM_TRAINEE'");
                if (o.Length > 0)
                    return true;
            }
            catch (Exception)
            {
            }

            return false;
        }
        public static bool isEXAM_ADMIN(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return false;

                DataRow[] o = dt.Select("ROLENAME = 'EXAM_ADMIN'");
                if (o.Length > 0)
                    return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        //TODO : IMPLEMENT ET
        public static bool isOJTI_forTHIS_report(DataTable dt, string reportid)
        {
            return true;
        }

        public static bool isAdmin(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                    return false;

                DataRow[] o = dt.Select("ROLENAME = 'SYSADMIN'");
                if (o.Length > 0)
                    return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        public static string getRandomFileName()
        {
            string date = System.DateTime.UtcNow.ToString("yyyyMMdd_HHmmss" + "_" + (new Random().Next(100)).ToString() + "_");
            return date;
        }

        public static string GetSelectedRadioButtonValue(List<RadioButton> lis)
        {
            foreach (RadioButton rad in lis)
            {
                if (rad.Checked)
                {
                    string id = rad.ID.ToLower();
                    if (id.EndsWith("na"))
                        return "NA";

                    if (id.EndsWith("s"))
                        return "S";

                    if (id.EndsWith("ni"))
                        return "NI";

                    return id.Last<char>().ToString().ToLower();
                }

            }

            // If none of the RadioButton controls is checked, return empty
            return "";
        }
        public static RadioButton FindRadioButtonToSelect(List<RadioButton> lis, string answer)
        {
            foreach (RadioButton rad in lis)
            {
                if (answer.ToLower() == "a" && rad.ID.ToLower().EndsWith("na"))
                    continue;

                if (rad.ID.ToLower().EndsWith(answer.ToLower()))
                    return rad;
            }
            return null;
        }


        public static List<RadioButton> GetRadiobuttonsbyGroupname(Control rootControl, string groupname)
        {
            List<RadioButton> radio_list = new List<RadioButton>();

            foreach (Control currentControl in rootControl.Controls)
            {
                if (currentControl == null)
                    continue;
                if (currentControl is RadioButton)
                {
                    if (((RadioButton)currentControl).GroupName.ToLower() == groupname.ToLower())
                    {
                        radio_list.Add((RadioButton)currentControl);
                        continue;
                    }
                }

            }
            return radio_list;
        }


        public static Byte[] PdfSharpConvert(String html)
        {
            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }

        public static string getpagehtml(string request_url_to_string)
        {
            WebClient myClient = new WebClient();
            string myPageHTML = null;
            byte[] requestHTML;
            // Gets the url of the page
            //string currentPageUrl = Request.Url.ToString();
            string currentPageUrl = request_url_to_string;

            UTF8Encoding utf8 = new UTF8Encoding();

            // by setting currentPageUrl to www.yahoo.com it will fetch the source (html) 
            // of the yahoo.com and put it in the myPageHTML variable. 

            // currentPageUrl = "http://www.yahoo.com"; 

            requestHTML = myClient.DownloadData(currentPageUrl);

            myPageHTML = utf8.GetString(requestHTML);

            //Response.Write(myPageHTML);
            return myPageHTML;
        }




        public static List<ListItem> remove_sectors(ListItemCollection collection, string sec)
        {
            List<ListItem> c = new List<ListItem>();
            c.Add(new ListItem("-"));

            foreach (ListItem item in collection)
            {
                string long_ = item.Text;
                if (long_.Contains(sec))
                    c.Add(item);
            }

            return c;
        }


        public static string getExtension(string filename)
        {
            string[] arr = filename.Split('.');
            return filename.Split('.')[arr.Length - 1];
        }

        public static bool deleteFile(string path, string filename)
        {
            try
            {
                // path = "~/images/" ; will work
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(path + filename)))
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(path + filename));
                    return true;
                }
            }
            catch (Exception)
            {
                //file in use
            }
            return false;
        }




        public static bool check_TimeTextbox_format(string text)
        {
            if (Regex.IsMatch(text, @"[0-9]+:[0-9][0-9]")
                || Regex.IsMatch(text, @"[0-9]+:[0-9]"))
                return true;

            return false;
        }
        public static string add_TimeFormat(string first, string second)
        {

            int hours = Convert.ToInt32(first.Split(':')[0]) + Convert.ToInt32(second.Split(':')[0]);
            int minutes = Convert.ToInt32(first.Split(':')[1]) + Convert.ToInt32(second.Split(':')[1]);
            if(minutes >=60)
            {
                hours++;
                minutes -= 60;
            }
            
            return hours.ToString() + ":" + (minutes < 10 ? "0" : "") + minutes.ToString() ;
        }


       public static string last_part(string aString , char delimiter)
        {
            string[] arr = aString.Split(delimiter);

            return arr.ElementAt(arr.Length - 1);
        }
    }
}