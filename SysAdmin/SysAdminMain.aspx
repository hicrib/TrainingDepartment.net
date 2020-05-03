<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/MainsMaster.Master" AutoEventWireup="true" CodeBehind="SysAdminMain.aspx.cs" Inherits="AviaTrain.SysAdmin.SysAdminMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .admin_actions_tbl {
            width: 1000px;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid #a52a2a	;
        }

            .admin_actions_tbl th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: white;
                background-color: #a52a2a	;
            }

            .admin_actions_tbl td {
                padding: 5px;
                min-height: 25px;
                width : 200px;
            }

            .admin_buttons {
                width : 100%;
                background-color : dimgray;
                color:white;
                font-size: small;
                font-weight : bold;
                margin : 2px;
                max-width:160px;
                min-height: 30px;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="admin_actions_tbl">
        <tr>
            <th colspan="2">
                SYSTEM FUNCTIONS
            </th>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_createuser" runat="server" CssClass="admin_buttons" OnClick="btn_createuser_Click" Text="Create User" />
            </td>
            <td>
                <asp:Button ID="btn_edit_roles" runat="server" CssClass="admin_buttons" OnClick="btn_edit_roles_Click" Text="Edit User Roles" />
            </td>
        </tr>
    </table>

    <br />
    <br />

    <table class="admin_actions_tbl">
        <tr>
            <th colspan="4">
                EXAMS FUNCTIONS
            </th>
        </tr>
        <tr>
            <td>
                    <asp:Button ID="btn_create_questions" runat="server" CssClass="admin_buttons"  OnClick="btn_create_questions_Click" Text="Create/Delete Questions" />
                    <asp:Button ID="btn_edit_questions" runat="server" CssClass="admin_buttons"  OnClick="btn_edit_questions_Click" Text="Edit Questions" />
            </td>
            <td>
                    <asp:Button ID="btn_create_exam" runat="server" CssClass="admin_buttons"  OnClick="btn_create_exam_Click" Text="Create Exam" />
                <br />
                <asp:Button ID="btn_delete_exam" runat="server" CssClass="admin_buttons"   OnClick="btn_delete_exam_Click" Text="Delete Exam" />
            </td>
            <td>
                 <asp:Button ID="btn_assign_exam" runat="server"  CssClass="admin_buttons" OnClick="btn_assign_exam_Click" Text="Assign Exam to Trainee" />
            </td>
            <td>
                 <asp:Button ID="btn_view_exam_result" runat="server" CssClass="admin_buttons"  OnClick="btn_view_exam_result_Click" Text="View Exam Results" />
            </td>
        </tr>
    </table>

    <br />
    <br />

    <table class="admin_actions_tbl">
        <tr>
            <th colspan="4">
                TRAINING FUNCTIONS
            </th>
        </tr>
        <tr>
            <td>
                    <asp:Button ID="btn_create_training" runat="server" CssClass="admin_buttons"  OnClick="btn_create_training_Click" Text="Create Training" />
            </td>
            <td>
                         <asp:Button ID="btn_view_trainingdesigns" runat="server" CssClass="admin_buttons"  OnClick="btn_view_trainingdesigns_Click" Text="View Trainings" />
          
            </td>
            <td>
                    <asp:Button ID="btn_assign_training" runat="server" CssClass="admin_buttons"  OnClick="btn_assign_training_Click" Text="Assign Training" />
            </td>
        </tr>
    </table>

    <br />
    <br />

    <table class="admin_actions_tbl">
        <tr>
            <th colspan="2">
                TRAINING REPORTS FUNCTIONS
            </th>
        </tr>
        <tr>
            <td>
                    <asp:Button ID="btn_create_TrainingFolder" runat="server" CssClass="admin_buttons"  OnClick="btn_create_TrainingFolder_Click" Text="Create Training Folder" />
            </td>
            <td>
                    <asp:Button ID="btn_View_Trainee_Folder" runat="server" CssClass="admin_buttons" OnClick="btn_View_Trainee_Folder_Click" Text="View Trainee Folder" />
            </td>
        </tr>
    </table>



  
</asp:Content>
