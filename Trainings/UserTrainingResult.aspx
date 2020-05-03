<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="UserTrainingResult.aspx.cs" Inherits="AviaTrain.Trainings.UserTrainingResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tbl_result {
            width: 1000px;
            border-collapse: collapse;
            border: 3px solid #a52a2a;
        }

            .tbl_result th {
                background-color: #a52a2a;
                color: white;
                text-align: center;
                font-size: large;
                font-weight: bold;
            }

            .tbl_result td {
                height: 40px;
                font-size: medium;
                font-weight: bold;
                text-align: left;
                padding: 10px;
            }

                .tbl_result td:first-child {
                    width: 100px;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tbl_result">
        <tr>
            <th colspan="2">
                <asp:Label ID="lbl_exam_name" runat="server"></asp:Label>
            </th>
        </tr>
        <tr>
            <td>
                <asp:Label ID="label2" runat="server" Text="Trainee : "></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_trainee_name" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="label1" runat="server" Text="Result : "></asp:Label></td>
            <td>
                <asp:Image ID="img_result" runat="server" />
                <asp:Label ID="lbl_result" runat="server"></asp:Label>

            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
        </tr>
    </table>
</asp:Content>
