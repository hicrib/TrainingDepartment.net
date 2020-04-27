<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="SuccessPage.aspx.cs" Inherits="AviaTrain.Pages.SuccessPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 1000px; text-align: center;">
        <div style="display: block;">
            <asp:Image ID="img" runat="server" ImageUrl="~/Images/success.png" />
        </div>
         <asp:Label ID="lbl_result" runat="server" Style="font-weight: bold; font-size: xx-large;"></asp:Label>
    </div>

   
</asp:Content>
