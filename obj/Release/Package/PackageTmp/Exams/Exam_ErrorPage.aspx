<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="Exam_ErrorPage.aspx.cs" Inherits="AviaTrain.Exams.Exam_ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <asp:Image ID="img_error" runat="server" ImageUrl="~/images/error.png" />
    <br />
    <br />
    <asp:Label ID="lbl_error" style="font-size: x-large; font-weight:bold;" runat="server"></asp:Label>

</asp:Content>
