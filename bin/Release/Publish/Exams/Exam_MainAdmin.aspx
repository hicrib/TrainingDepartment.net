<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="Exam_MainAdmin.aspx.cs" Inherits="AviaTrain.Exams.Exam_MainAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .admin_actions_tbl {
            width: 1000px;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid #a52a2a;
        }

            .admin_actions_tbl th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: white;
                background-color: #a52a2a;
            }

            .admin_actions_tbl td {
                padding: 5px;
                min-height: 25px;
                width: 200px;
            }

        .admin_buttons {
            width: 100%;
            background-color: dimgray;
            color: white;
            font-size: small;
            font-weight: bold;
            margin: 2px;
            max-width: 160px;
            min-height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <table class="admin_actions_tbl">
        <tr>
            <th colspan="4">EXAMS FUNCTIONS
            </th>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_create_questions" runat="server" CssClass="admin_buttons" OnClick="btn_create_questions_Click" Text="Create/Delete Questions" />
            </td>
            <td>
                <asp:Button ID="btn_create_exam" runat="server" CssClass="admin_buttons" OnClick="btn_create_exam_Click" Text="Create Exam" />
                <br />
                <asp:Button ID="btn_delete_exam" runat="server" CssClass="admin_buttons" OnClick="btn_delete_exam_Click" Text="Delete Exam" />
            </td>
            <td>
                <asp:Button ID="btn_assign_exam" runat="server" CssClass="admin_buttons" OnClick="btn_assign_exam_Click" Text="Assign Exam to Trainee" />
            </td>
            <td>
                <asp:Button ID="btn_view_exam_result" runat="server" CssClass="admin_buttons" OnClick="btn_view_exam_result_Click" Text="View Exam Results" />
            </td>
        </tr>
    </table>
</asp:Content>
