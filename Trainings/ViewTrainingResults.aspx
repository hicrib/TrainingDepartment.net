<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="ViewTrainingResults.aspx.cs" Inherits="AviaTrain.Trainings.ViewTrainingResults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .exam_results_tbl {
            width: 1000px;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid #a52a2a;
        }

            .exam_results_tbl th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: white;
                background-color: #a52a2a;
            }

            .exam_results_tbl td {
                padding: 5px;
                min-height: 25px;
            }

        .filters_tbl {
            width: 100%;
            border-collapse: collapse;
            align-content: center;
            border: 1px solid #a52a2a;
        }

            .filters_tbl td {
                font-weight: bold;
                align-content: center;
                text-align: center;
            }

        .btn_search {
            background-color: #a52a2a;
            color: white;
            font-weight: bold;
            font-size: large;
            float: right !important;
        }

        .grid_results {
            width: 100%;
            border-collapse: collapse;
            border: 2px solid black;
        }

            .grid_results th {
                background-color: #d89191;
                color: black;
                font-size: large;
                margin: 20px;
                padding: 15px;
            }

            .grid_results td {
                font-weight: bold;
                font-size: small;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <table class="exam_results_tbl">
                <tr>
                    <td>
                        <table class="filters_tbl">
                            <tr>
                                <th colspan="5">FILTERS
                                </th>
                            </tr>
                            <tr>
                                <td colspan="5" style="height: 10px;"></td>
                            </tr>
                            <tr>
                                <td>Training </td>
                                <td>Trainee</td>
                                <td>Status</td>
                                <td colspan="2">Last Action<br />
                                    Between Dates</td>
                                <td>Exam </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:DropDownList ID="filter_training" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="filter_trainee" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="filter_status" runat="server">
                                        <asp:ListItem Value="ALL"></asp:ListItem>
                                        <asp:ListItem Value="ASSIGNED"></asp:ListItem>
                                        <asp:ListItem Value="USER_STARTED"></asp:ListItem>
                                        <asp:ListItem Value="FINISHED"></asp:ListItem>
                                        <asp:ListItem Value="PASSED"></asp:ListItem>
                                        <asp:ListItem Value="FAILED"></asp:ListItem>
                                        <asp:ListItem Value="NOSHOW"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox TextMode="Date" ID="filter_start" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox TextMode="Date" ID="filter_finish" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Only Active Trainings
                                    <asp:CheckBox ID="chk_active_training" OnCheckedChanged="chk_active_training_CheckedChanged" AutoPostBack="true" runat="server" Checked="true" />
                                </td>
                                <td></td>
                                <td></td>
                                <td>Only Active Trainees
                                    <asp:CheckBox ID="chk_active_trainee" OnCheckedChanged="chk_active_trainee_CheckedChanged" AutoPostBack="true" runat="server" Checked="true" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="5" style="height: 10px;"></td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lbl_result" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Button ID="btn_search" CssClass="btn_search" runat="server" Text="SEARCH" OnClick="btn_search_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <br />

            <table class="exam_results_tbl">
                <tr>
                    <th>RESULTS
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grid_results" runat="server" CssClass="grid_results" OnRowDataBound="grid_results_RowDataBound"
                            AllowPaging="true" AllowSorting="true" OnSelectedIndexChanged="grid_results_SelectedIndexChanged" 
                            OnPageIndexChanging="grid_results_PageIndexChanging" PageSize="30"
                            OnSorting="grid_results_Sorting">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />                            
                        </asp:GridView>
                    </td>
                </tr>
            </table>



        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
