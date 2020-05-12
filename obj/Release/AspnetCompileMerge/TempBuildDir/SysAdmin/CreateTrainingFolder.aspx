<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="CreateTrainingFolder.aspx.cs" Inherits="AviaTrain.SysAdmin.CreateTrainingFolder1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
         #createuser_tbl {
            border: 2px solid #a52a2a	;
            border-collapse: collapse;
            width: 1000px;
        }

            #createuser_tbl td {
                padding: 5px;
                font-weight: bold;
                font-size: medium;
                height : 30px;
                width : 33%
            }

        #ContentPlaceHolder1_lbl_availability {
            color: #a52a2a	 !important;
            font-weight: bold;
            width : 100%;
            text-align:center;
        }
        #ContentPlaceHolder1_btn_chkavailable {
            width : 100%;
            background-color : #a52a2a	;
            font :bold;
            color : white;
        }

        .dropdown {
            width: 100%;
            font-weight: bold;
            border: 1px solid #a52a2a	;
            height : 30px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Always" ChildrenAsTriggers="true">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <div>
                    <table id="createuser_tbl" style="">
                        <tr>
                            <td>Trainee : </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddl_all_trainees" runat="server" CssClass="dropdown"></asp:DropDownList></td>
                            <td></td>

                        </tr>
                        <tr>
                            <td>Position :  </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddl_position" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddl_position_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="-" Text="---"></asp:ListItem>
                                    <asp:ListItem Value="TWR" Text="Tower"></asp:ListItem>
                                    <asp:ListItem Value="ACC" Text="ACC"></asp:ListItem>
                                    <asp:ListItem Value="APP" Text="APP"></asp:ListItem>
                                </asp:DropDownList></td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddl_sector" CssClass="dropdown" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Button ID="btn_chkavailable" runat="server" Text="Check Available" OnClick="btn_chkavailable_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                 <asp:Label ID="lbl_availability" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                 <asp:Label ID="lbl_createresult" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:DropDownList ID="ddl_steps" runat="server" CssClass="dropdown" Visible="false" ></asp:DropDownList>
                            </td>
                            <td> <asp:Label ID="lbl_totalhours" runat="server" Visible="false" Text="Total Hours :" ></asp:Label> 
                                <asp:TextBox ID="txt_totalhours" runat="server"  placeholder="Hours on Sector ex: 000:00" Visible="false" ></asp:TextBox>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btn_start_tree" runat="server" CssClass="dropdown" OnClick="btn_start_tree_Click" Text="Start Folder" Visible="false"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
