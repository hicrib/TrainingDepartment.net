using AviaTrain.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AviaTrain.Pages
{
    public partial class UserFileSystem : MasterPage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserSession user = (UserSession)Session["usersession"];
                //get user's main folder content
                DataRow dr = DB_FileSys.get_UserMain(user.employeeid);
                if (dr == null)
                    return; //this shouldn't happen as it creates if there is none

                TreeNode mainnode = new TreeNode(dr["NAME"].ToString(), dr["ID"].ToString());
                tree.Nodes.Add(mainnode);

            }

        }


        protected void tree_SelectedNodeChanged(object sender, EventArgs e)
        {
            string selectedid = tree.SelectedValue.ToString();

            DataTable dt = DB_FileSys.get_ChildNodes(selectedid);
            if (dt == null) //no child
                return;

            tree.SelectedNode.ChildNodes.Clear();

            //find the child folders - add to tree
            DataRow[] c_folders = dt.Select("FILEID = 0 ");
            if (c_folders != null && c_folders.Length > 0)
            {
                foreach (DataRow row in c_folders)
                {
                    TreeNode n = new TreeNode(row["NAME"].ToString(), row["ID"].ToString());
                    tree.SelectedNode.ChildNodes.Add(n);
                }
            }

            DataRow[] c_files = dt.Select("FILEID <> 0");
            if (c_files != null && c_files.Length > 0)
            {
                foreach (DataRow row in c_files)
                {
                    TreeNode n = new TreeNode(row["NAME"].ToString(), row["ID"].ToString());
                    n.NavigateUrl = AzureCon.general_container_url + row["ADDRESS"].ToString();
                    n.Target = "_blank";
                    tree.SelectedNode.ChildNodes.Add(n);

                    //todo: show on the right
                }
            }
            tree.SelectedNode.Expand();

        }
    }
}