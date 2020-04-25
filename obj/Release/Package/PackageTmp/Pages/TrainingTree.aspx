<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrainingTree.aspx.cs" Inherits="AviaTrain.Pages.TrainingTree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:GridView ID="grid_user_tree"   AllowPaging="true" AllowSorting="true" runat="server"
                                    OnSelectedIndexChanged="grid_user_tree_SelectedIndexChanged" OnPageIndexChanging="grid_user_tree_PageIndexChanging" PageSize="5"
                                    OnSorting="grid_ojti_reports_Sorting">
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First" LastPageText="Last" />
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/Images/view.png"  SelectText="View" />
                                    </Columns>
                                </asp:GridView>



        </div>
    </form>
</body>
</html>
