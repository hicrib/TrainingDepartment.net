<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/MainsMaster.Master" AutoEventWireup="true" CodeBehind="EditRoles.aspx.cs" Inherits="AviaTrain.SysAdmin.EditRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #createuser_tbl {
            border: 2px solid #a52a2a;
            border-collapse: collapse;
            width : 1000px;
        }

            #createuser_tbl td, th {
                padding: 5px;
            }
        .role_table {
            width : 475px;
            border-collapse : collapse;
            border : 1px solid black;
            vertical-align : top;
        }
        .role_table td {
            width : 100%;
        }
        .role_table th {
            background-color: lightgray;
        }
        .role_table td:first-child {
            width: 50px;
        }
         .role_table td:nth-child(2) {
            width: 70px;
            text-align : center;
        }
          .role_table td:nth-child(3) {
            font-weight : bold;
        }

    </style>

</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
    <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>


            <table id="createuser_tbl" style="">
                <thead>
                    <th colspan="3" style="border: 2px solid #a52a2a; background-color: #a52a2a; color: white; font-weight: bold; text-align: center;">EDIT USER ROLES</th>
                </thead>
                <tbody>
                    <tr>
                        <td colspan="3" style="text-align:center;">
                            <span style="font-weight:bold;"> System User : </span>
                            <asp:DropDownList ID="ddl_users" Height="30" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_users_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="width: 50px;"></td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <table class="role_table">
                                <tr>
                                    <th>USER ROLES</th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grid_user_roles" runat="server" CssClass="grid_user_roles" OnRowDeleting="grid_user_roles_RowDeleting1"
                                            OnRowCommand="grid_user_roles_RowCommand">
                                            <Columns>
                                                <asp:ButtonField ImageUrl="~/images/delete.png"  CommandName="DELETE" ButtonType="Image" Text="Remove" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>

                        </td>
                        <td   style="width: 50px;"></td>
                        <td>
                            <table class="role_table">
                                <tr>
                                    <th>ALL SYSTEM ROLES</th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grid_all_roles" runat="server" CssClass="grid_all_roles" OnRowCommand="grid_all_roles_RowCommand">
                                            <Columns>
                                                <asp:ButtonField ImageUrl="~/images/create.png" CommandName="ADD" ButtonType="Image" Text="Add" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
