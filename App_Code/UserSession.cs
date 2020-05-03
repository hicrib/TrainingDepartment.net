using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace AviaTrain.App_Code
{
    public class UserSession
    {
        public string employeeid;
        public string initial;
        public string name_surname;
        public string email;
        public string photo;
        public string signature;
        public bool isAdmin;
        public bool isOJTI;
        public bool isLCE;
        public bool isOnlyTrainee = true;
        public string problem;
        public bool isExamTrainee;
        public bool isExamAdmin;

        public DataRow info;

        DataTable role_priv;
        public DataTable roles_pages;

        public UserSession(string id)
        {
            employeeid = id;
            info = DB_System.getUserInfo(employeeid);
            initial = info["INITIAL"].ToString();
            name_surname = info["FIRSTNAME"].ToString() + " " + info["SURNAME"].ToString();
            email = info["EMAIL"].ToString();
            photo = info["PHOTO"].ToString();
            signature = info["SIGNATURE"].ToString();

            role_priv = DB_System.get_ALL_Privileges_of_Person(employeeid);
            isAdmin = Utility.isAdmin(role_priv);
            isOJTI = Utility.isOJTI(role_priv);
            isLCE = Utility.isLCE(role_priv);
            isExamTrainee = Utility.isEXAMTRAINEE(role_priv);
            isExamAdmin = Utility.isEXAM_ADMIN(role_priv);
            roles_pages = DB_System.get_ROLES_PAGES(employeeid);

            if (isAdmin || isLCE || isOJTI)
                isOnlyTrainee = false;

            if (role_priv == null || role_priv.Rows.Count == 0)
                problem = "role_priv is empty";

            if (roles_pages == null || roles_pages.Rows.Count == 0)
                problem = "roles_pages is empty";
        
        }

    }
}