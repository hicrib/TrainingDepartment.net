<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/MainsMaster.Master" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="AviaTrain.SysAdmin.CreateUser1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style>
        #createuser_tbl {
            border: 2px solid #b63838	;
            border-collapse: collapse;
        }

            #createuser_tbl td, th {
                padding: 5px;
            }

        input:not([type="image"]) {
            width: 100%;
        }
        

        #ContentPlaceHolder1_lbl_result_adduser {
            color : #b63838	;
            font : bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Always" ChildrenAsTriggers="true">
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_adduser" />
            </Triggers>
            <ContentTemplate>
                <table id="createuser_tbl" style="">
                    <thead>
                        <th colspan="5" style="border: 2px solid #b63838	; background-color: #b63838	; color: white; font-weight: bold; text-align: center;">CREATE USER</th>
                    </thead>
                    <thead>
                        <th>*Employee ID</th>
                        <th>*First Name</th>
                        <th>*Surname</th>
                        <th>*Password</th>
                        <th>*Initial</th>

                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <asp:TextBox ID="txt_employeeid" TextMode="Number" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_firstName" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_surName" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_password" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_initial" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" style="height: 20px;"></td>
                        </tr>
                        <thead>
                            <th></th>
                            <th>Role 1</th>
                            <th>Role 2</th>
                            <th>Role 3</th>
                            <th></th>
                        </thead>
                        <tr>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="ddl_role1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_role1_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_role2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_role1_SelectedIndexChanged" Visible="false"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_role3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_role1_SelectedIndexChanged" Visible="false"></asp:DropDownList>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="5" style="height: 20px;"></td>
                        </tr>
                         <tr>
                            <td colspan="5" style="height: 20px;">
                                <asp:Button ID="btn_adduser" runat="server" Text="Add User" OnClick="btn_adduser_Click" />
                            </td>
                        </tr>
                         <tr>
                            <td colspan="5" style="height: 20px;"> <asp:Label ID="lbl_result_adduser" runat="server" Text=""></asp:Label></td>
                        </tr>
                    </tbody>
                </table>
                
               
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
