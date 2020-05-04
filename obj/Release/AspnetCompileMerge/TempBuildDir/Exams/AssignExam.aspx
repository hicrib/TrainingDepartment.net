<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="AssignExam.aspx.cs" Inherits="AviaTrain.Exams.AssignExam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                padding: 5px;
                min-height: 25px;
            }

            .btn_submitexam {
            width: 100%;
            font-weight: bold;
            border: 1px solid #a52a2a	;
            background-color: #a52a2a	;
            color: white;
            height: 40px;
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

        .grid_questions {
            margin-top : 30px;
            width : 1000px;
            font-size : small;
        }
        .grid_questions th {
            background-color : darkgray;
            font-size : large !important;
        }
        .grid_questions td {
            padding : 5px;
        }
        .grid_questions td:nth-child(2) {
            width : 700px;
        }
        .grid_questions td:nth-child(3) {
            width : 300px;
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
                    <td>
                        <asp:Label ID="lbl_name" runat="server" Width="160" Font-Bold="true" Text="Exam Name : "></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_exams" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_exams_SelectedIndexChanged"  Width="400" Font-Bold="true"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_trainee" runat="server" Width="160" Font-Bold="true" Text="Trainee Name:"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_trainee" runat="server" Width="400" Font-Bold="true" ></asp:DropDownList>
                    </td>
                </tr>
               <%-- <tr>
                    <td>
                        <asp:Label ID="lbl_start" runat="server" Width="160" Font-Bold="true" Text="Scheduled Start (inclusive) : "></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_finish" runat="server" Width="160" Font-Bold="true" Text="Scheduled Finish (inclusive) : "></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Width="160" Font-Bold="true" Text="Start/End (inclusive) : "></asp:Label>
                    </td>
                    <td>
                        <asp:Calendar ID="Calendar_start" runat="server" OnDayRender="Calendar_start_DayRender"  SelectionMode="DayWeekMonth" ></asp:Calendar>
                    </td>
                    <td>
                        <asp:Calendar ID="Calendar_finish" runat="server" OnDayRender="Calendar_finish_DayRender" SelectionMode="DayWeekMonth" ></asp:Calendar>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lbl_pageresult" runat="server" Visible="false" CssClass="errorbox"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Button ID="btn_submit" runat="server" CssClass="btn_submitexam" Text="SUBMIT " OnClick="btn_submit_Click" />
                    </td>
                </tr>
            </table>

            <asp:GridView ID="grid_questions" runat="server" CssClass="grid_questions"></asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
