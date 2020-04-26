<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTrainingFolder.aspx.cs" Inherits="AviaTrain.SysAdmin.ViewTrainingFolder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #createuser_tbl {
            border: 2px solid #b63838	;
            border-collapse: collapse;
            width: 700px;
        }

            #createuser_tbl td {
                padding: 5px;
                font-weight: bold;
                font-size: medium;
                height: 30px;
                width : 140px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
      
        <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Always" ChildrenAsTriggers="true">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <div>
                    <table id="createuser_tbl" style="">
                        <thead>
                            <th colspan="5" style="border: 2px solid #b63838	; background-color: #b63838	; color: white; font-weight: bold; text-align: center;">VIEW TRAINING FOLDER</th>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Trainee : </td>
                                <td>
                                    <asp:DropDownList ID="ddl_trainees" runat="server"></asp:DropDownList></td>
                                <td>Position </td>
                                <td>
                                    <asp:DropDownList ID="ddl_positions" runat="server" OnSelectedIndexChanged="ddl_positions_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="-" Text=" --- "></asp:ListItem>
                                        <asp:ListItem Value="TWR" Text="Tower"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_sector" runat="server"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_find_folder" runat="server" OnClick="btn_find_folder_Click" Text="Find" />
                                </td>
                                <td colspan="4"></td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:GridView ID="grid_folder"   AllowPaging="true" AllowSorting="true" runat="server" Visible="false"
                                    OnSelectedIndexChanged="grid_folder_SelectedIndexChanged" OnPageIndexChanging="grid_folder_PageIndexChanging" PageSize="5"
                                    OnSorting="grid_folder_Sorting">
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" FirstPageText="First" LastPageText="Last" />
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/Images/view.png"  SelectText="View" />
                                    </Columns>
                                </asp:GridView>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
