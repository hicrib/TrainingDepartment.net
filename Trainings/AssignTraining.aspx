<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="AssignTraining.aspx.cs" Inherits="AviaTrain.Trainings.AssignTraining" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .assign_tbl {
            width: 1000px;
            border-collapse: collapse;
            border : 2px solid #a52a2a;
        }

            .assign_tbl th {
                text-align: center;
                font-weight: bold;
                font-size: large;
                background-color: #a52a2a;
                color: white;
            }

            .assign_tbl td:first-child {
                width: 300px;
                padding: 5px;
                font-weight: bold;
            }

        .btn_submit {
            width: 50%;
            background-color: #a52a2a;
            color: white;
            font-size: medium;
            font-weight: bold;
            margin : 5px;
        }

        .errorbox {
            width: 100%;
            height: 50px;
            background-color: lightgray;
            color: black;
            font-size: x-large;
            border: 2px solid red;
            text-align: center;
            padding: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <table class="assign_tbl">
                <tr>
                    <td>TRAINING : </td>
                    <td>
                        <asp:DropDownList ID="ddl_trainings" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chk_timelimit" Text="Add Time Limit" AutoPostBack="true" runat="server" OnCheckedChanged="chk_timelimit_CheckedChanged" />
                    </td>
                    <td>
                        <asp:Panel ID="panel_times" runat="server" Visible="false">
                            <asp:TextBox ID="txt_starttime" runat="server" TextMode="Date"></asp:TextBox>
                            <asp:TextBox ID="txt_finishtime" runat="server" TextMode="Date"></asp:TextBox>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td ></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lbl_pageresult" CssClass="errorbox" runat="server" Visible="false"></asp:Label>
                    </td>
                    </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btn_submit" CssClass="btn_submit" runat="server" Text="ASSIGN" OnClick="btn_submit_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="height:30px;"></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <div style="vertical-align:middle;">
                            <asp:ListBox ID="list_allusers" runat="server" style="display:inline-block; vertical-align:top;" SelectionMode="Multiple" Height="300" Width="250"></asp:ListBox>

                            <asp:ImageButton ID="btn_assign" runat="server" ImageUrl="~/images/toright.png" OnClick="btn_assign_Click" />
                        </div>
                    </td>
                    <td>
                        <div style="vertical-align:middle; ">
                            <asp:ImageButton ID="btn_unassign" runat="server"  ImageUrl="~/images/toleft.png" OnClick="btn_unassign_Click" />

                            <asp:ListBox ID="list_chosens" runat="server" SelectionMode="Multiple" Height="300" Width="250"></asp:ListBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>












</asp:Content>
