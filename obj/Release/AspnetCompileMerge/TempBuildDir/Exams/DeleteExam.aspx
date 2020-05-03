<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="DeleteExam.aspx.cs" Inherits="AviaTrain.Exams.DeleteExam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function confirmation_DELETE() {
            if (confirm('Are you sure you want to DELETE ? This will delete uncompleted Trainee Assignments of this exams!')) {
                return true;
            } else {
                return false;
            }
        }
    </script>
    <style>
        .tbl_exam_info {
            width: 1000px;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid #a52a2a	;
        }

            .tbl_exam_info th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: white;
                background-color: #a52a2a	;
            }

            .tbl_exam_info td {
                text-align: center;
                width: 500px;
                padding: 5px;
                min-height: 25px;
            }

        .grid_exam_questions th {
            background-color: lightgray;
            color: black;
        }
        .grid_exam_questions td  {
            font-weight : bold;
            font-size : small;
        }
        .grid_exam_questions td:first-child {
            width : 20px;
        }
        .grid_exam_questions td:nth-child(2) {
            width : 55%;
            text-align:left;
        }
        .grid_exam_questions td:last-child {
            width : 40%;
            text-align:left;
        }

        .btn_submit {
            width: 100%;
            float: left;
            font-weight: bold;
            border: 1px solid #a52a2a	;
            background-color: #a52a2a	;
            color: white;
            height: 25px;
        }

        .errorbox {
            width: 100%;
            height: 50px;
            background-color: lightgray;
            color: black;
            font-size: large;
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

            <table class="tbl_exam_info">
                <tr>
                    <th colspan="2">DELETE EXAM
                    </th>
                </tr>
                <tr>
                    <td colspan="2" style="height: 30px;"></td>
                </tr>
                <tr>
                    <td style="font-weight:bold; font-size:large;">Exam Name : 
                        <asp:DropDownList ID="ddl_exams" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_exams_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btn_delete" runat="server" Text="Delete Exam" Enabled="false" CssClass="btn_submit" OnClientClick="return confirmation_DELETE();" OnClick="btn_delete_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 40px;"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lbl_result" runat="server" CssClass="errorbox" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 40px;"></td>
                </tr>

                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grid_exam_questions" runat="server" CssClass="grid_exam_questions"></asp:GridView>

                    </td>
                </tr>

            </table>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
