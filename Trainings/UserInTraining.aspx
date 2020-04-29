<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="UserInTraining.aspx.cs" Inherits="AviaTrain.Trainings.UserInTraining" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .main_tbl {
            width : 100%;
            border-collapse : collapse;
        }
        .main_tbl td:first-child {
            width : 80%;
        }
        .main_tbl td:nth-child(2) {
            width: 20%;
            border : 1px solid indianred;
        }
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="main_tbl">
        <tr>
            <td>
                <asp:Panel ID="pnl_content" runat="server" Visible="false" ></asp:Panel>
                <asp:Panel ID="pnl_question" runat="server" Visible="false" ></asp:Panel>
            </td>
            <td>

            </td>
        </tr>
    </table>




    <asp:Label ID="lbl_trnid" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_stepid" runat="server" Visible="false"></asp:Label>
</asp:Content>
