<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="InstructorsMain.aspx.cs" Inherits="AviaTrain.Pages.InstructorsMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /*FOLLOWING ARE FOR MULTIVIEW */
        .tabs {
            position: relative;
            margin-top: 3px;
            z-index: 2;
            width: 1000px;
        }

        .tab {
            border: 2px solid #a52a2a;
            background-color: lightgray;
            background-repeat: repeat-x;
            color: White;
            width: 150px;
            font-weight: bold;
            font-size: large;
            padding: 10px;
            text-align: center;
        }

        .selected {
            background-color: #a52a2a;
            background-repeat: repeat-x;
            color: black;
        }

        .tabcontents {
            border: 1px solid #a52a2a;
            padding: 3px;
            width: 993px;
            height: 93%;
            background-color: #e1e1e1;
        }
        /*FINISHED :   FOR MULTIVIEW ACC-APP-TOWER*/

        .instructorButton {
            width: 100%;
            padding: 10px;
            margin: 10px;
            background-color: #46a2e8;
            font-weight: bold;
            font-size: medium;
            color: white;
            border: 1px solid #a52a2a;
        }

        .tabcontents {
            border: 1px solid #a52a2a;
            padding: 3px;
            width: 993px;
            height: 93%;
            background-color: #e1e1e1;
        }


        .ojtireports_tbl {
            border-collapse: collapse;
            height: 100px;
            width: 100%;
            border: 2px solid #a52a2a;
            margin: 0px !important;
        }

            .ojtireports_tbl * {
                padding: 5px;
                align-self: center;
                text-align: center;
            }

            .ojtireports_tbl td:nth-child(3) {
                padding: 5px;
                text-align: left;
            }

            .ojtireports_tbl td {
                padding: 5px;
                margin: 0px;
            }

        .grid_table {
            width: 99%;
            margin: 1px !important;
        }

        .examadmin_tbl {
            width: 100%;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid #a52a2a;
        }
            .examadmin_tbl td {
                padding: 5px;
                height: 33px;
                width: 200px;
                text-align : center;
            }

       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Conditional" ChildrenAsTriggers="true" style="margin-left: 3px;">
        <ContentTemplate>
            <asp:Menu ID="jobsMenu" Orientation="Horizontal" StaticMenuItemStyle-CssClass="tab"
                StaticSelectedStyle-CssClass="selected" CssClass="tabs" runat="server"
                OnMenuItemClick="jobsMenu_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Reports" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="My Training" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Training Department" Value="2"></asp:MenuItem>
                    <asp:MenuItem Text="System" Value="3"></asp:MenuItem>
                    <asp:MenuItem Text="Statistics" Value="4"></asp:MenuItem>
                    <asp:MenuItem Text="Exam Admin" Value="5"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <div class="tabcontents">
                <asp:MultiView ID="multiview1" ActiveViewIndex="0" runat="server">

                    <asp:View ID="view_reports" runat="server">
                        <div style="margin: 20px 0px 20px 0px;">
                            <asp:LinkButton ID="btn_create_report" runat="server" Text="Create Report" PostBackUrl="~/Reports/CreateReport.aspx" CssClass="instructorButton" />
                            <asp:LinkButton ID="btn_create_trainingfolder" runat="server" Text="Create Training Folder" PostBackUrl="~/SysAdmin/CreateTrainingFolder.aspx" CssClass="instructorButton" />
                            <asp:LinkButton ID="btn_levelobjectives" runat="server" Text="Sign Level Objectives" PostBackUrl="~/Reports/LevelObjectives.aspx" CssClass="instructorButton" />
                            <asp:LinkButton ID="btn_viewtrainingfolder" runat="server" Text="View Training Folders" PostBackUrl="~/SysAdmin/ViewTrainingFolder.aspx" CssClass="instructorButton" />
                        </div>
                        <table class="ojtireports_tbl">
                            <tr>
                                <th style="border: 3px solid #a52a2a; background-color: #a52a2a; color: white; font-weight: bold; align-content: center;">REPORTS I CREATED
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="grid_ojti_reports" AllowPaging="true" AllowSorting="true" runat="server" CssClass="grid_table"
                                        OnSelectedIndexChanged="grid_ojti_reports_SelectedIndexChanged" OnPageIndexChanging="grid_ojti_reports_PageIndexChanging" PageSize="10"
                                        OnSorting="grid_ojti_reports_Sorting">
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last" />
                                        <Columns>
                                            <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="iconimg" ButtonType="Image" SelectImageUrl="~/Images/view.png" SelectText="View" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>

                    </asp:View>

                    <asp:View ID="view_mytraining" runat="server"></asp:View>

                    <asp:View ID="view_department" runat="server">
                        <asp:GridView ID="grid_department" runat="server" AllowPaging="true" AllowSorting="true"
                            OnSelectedIndexChanged="grid_department_SelectedIndexChanged" OnSorting="grid_department_Sorting">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="iconimg" ButtonType="Image" SelectImageUrl="~/Images/view.png" SelectText="View" />
                            </Columns>
                        </asp:GridView>

                    </asp:View>

                    <asp:View ID="view_system" runat="server"></asp:View>


                    <asp:View ID="view_stats" runat="server">
                        <asp:LinkButton ID="lnk_trnhours" runat="server" Text="Training Hrs." PostBackUrl="~/Statistics/Stat_TrnHours.aspx" CssClass="instructorButton" />
                        <asp:LinkButton ID="lnk_workhours" runat="server" Text="Working Hrs." PostBackUrl="~/Statistics/Stat_WorkHours.aspx" CssClass="instructorButton" />
                    </asp:View>

                    <asp:View ID="view_examadmin" runat="server">
                        <table class="examadmin_tbl">
                            <tr>
                                <td rowspan="2">
                                    <asp:LinkButton ID="btn_create_questions" runat="server" PostBackUrl="~/Exams/CreateQuestions.aspx" CssClass="instructorButton" Text="Create/Delete Questions" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="btn_create_exam" runat="server" PostBackUrl="~/Exams/CreateExam.aspx" CssClass="instructorButton" Text="Create Exam" />                               
                                </td>
                                <td rowspan="2">
                                    <asp:LinkButton ID="btn_assign_exam" runat="server" PostBackUrl="~/Exams/AssignExam.aspx" CssClass="instructorButton" Text="Assign Exam to Trainee" />
                                </td >
                                <td rowspan="2">
                                    <asp:LinkButton ID="btn_view_exam_result" runat="server" PostBackUrl="~/Exams/ViewExamResults.aspx" CssClass="instructorButton" Text="View Exam Results" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                         <asp:LinkButton ID="btn_delete_exam" runat="server" PostBackUrl="~/Exams/DeleteExam.aspx" CssClass="instructorButton" Text="Delete Exam" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>





</asp:Content>
