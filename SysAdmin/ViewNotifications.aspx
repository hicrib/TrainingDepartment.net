<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="ViewNotifications.aspx.cs" Inherits="AviaTrain.SysAdmin.ViewNotifications" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .grid_results{
            min-width : 500px;
        }
        .grid_results td {
            padding : 5px;
        }
        .grid_results td:nth-child(2) {
            min-width : 300px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin : 3px; border : 2px solid #a52a2a; width:986px; padding : 5px;">
    User : 
    <asp:DropDownList ID="ddl_users" runat="server" style="width:200px; height:30px;"></asp:DropDownList>
    <div style="display:inline-block; border:none; width : 50px;"></div>
    Notification : 
    <asp:DropDownList ID="ddl_notifications" runat="server" style="width:400px; height:30px;" ></asp:DropDownList>
    <asp:Button ID="btn_find" runat="server" Text="Find" OnClick="btn_find_Click" Height="30" Width="70" />

    <br />
    <br />

    <asp:GridView runat="server" ID="grid_results"  CssClass="grid_results"></asp:GridView>
        </div>
</asp:Content>
