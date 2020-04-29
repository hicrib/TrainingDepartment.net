<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="AssignTraining.aspx.cs" Inherits="AviaTrain.Trainings.AssignTraining" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:DropDownList ID="ddl_trainings" runat="server"></asp:DropDownList>



    <asp:CheckBox ID="chk_timelimit" Text="Add Time Limit" AutoPostBack="true" runat="server" OnCheckedChanged="chk_timelimit_CheckedChanged" />
    <asp:Panel ID="panel_times" runat="server" Visible="false">
        <asp:TextBox ID="txt_starttime" runat="server" TextMode="Date"></asp:TextBox>
        <asp:TextBox ID="txt_finishtime" runat="server" TextMode="Date"></asp:TextBox>
    </asp:Panel>




    <asp:ListBox ID="list_allusers" runat="server" SelectionMode="Multiple" Height="300"   ></asp:ListBox>

    <asp:ImageButton ID="btn_assign" runat="server" ImageUrl="~/images/toright.png" OnClick="btn_assign_Click" />
    <asp:ImageButton ID="btn_unassign" runat="server" ImageUrl="~/images/toleft.png" OnClick="btn_unassign_Click" />

    <asp:ListBox ID="list_chosens" runat="server"  SelectionMode="Multiple" Height="300" ></asp:ListBox>

</asp:Content>
