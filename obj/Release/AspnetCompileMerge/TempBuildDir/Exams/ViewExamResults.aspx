<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="ViewExamResults.aspx.cs" Inherits="AviaTrain.Exams.ViewExamResults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function DownloadExcel() {
            var downloadFrame = document.createElement("IFRAME");

            if (downloadFrame != null) {
                downloadFrame.setAttribute("src", '../Pages/DownloadExcel.aspx');
                downloadFrame.style.width = "0px";
                downloadFrame.style.height = "0px";
                document.body.appendChild(downloadFrame);
            }
        }
    </script>
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
                                <th colspan="8">FILTERS
                                </th>
                            </tr>
                            <tr>
                                <td colspan="8" style="height: 10px;"></td>
                            </tr>
                            <tr>
                                <td>Exam Name</td>
                                <td>Sector</td>
                                <td colspan="2">Between Dates</td>
                                <td>Trainee</td>
                                <td>Passed</td>
                                <td colspan="2">Grade Between </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:DropDownList ID="filter_examname" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                     <asp:DropDownList ID="ddl_examsector"  Font-Bold="true" runat="server" >
                                                    <asp:ListItem Value="-" Text=" - "></asp:ListItem>
                                                    <asp:ListItem Value="GEN" Text=" General "></asp:ListItem>
                                                    <asp:ListItem Value="LATSI"></asp:ListItem>
                                                    <asp:ListItem Value="AIP"></asp:ListItem>
                                                    <asp:ListItem Value="TWR" Text=" Tower General "></asp:ListItem>
                                                    <asp:ListItem Value="ACC" Text=" ACC General "></asp:ListItem>
                                                    <asp:ListItem Value="APP" Text=" APP General "></asp:ListItem>
                                                    <asp:ListItem Value="ACC-NR"></asp:ListItem>
                                                    <asp:ListItem Value="ACC-SR"></asp:ListItem>
                                                    <asp:ListItem Value="ACC-CR"></asp:ListItem>
                                                    <asp:ListItem Value="APP-AR"></asp:ListItem>
                                                    <asp:ListItem Value="APP-BR"></asp:ListItem>
                                                    <asp:ListItem Value="APP-KR"></asp:ListItem>
                                                    <asp:ListItem Value="TWR-GMC"></asp:ListItem>
                                                    <asp:ListItem Value="TWR-ADC"></asp:ListItem>
                                                </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox TextMode="Date" ID="filter_start" Width="140" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox TextMode="Date" ID="filter_finish" Width="140" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="filter_trainee" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="filter_passed" runat="server" AutoPostBack="true" OnSelectedIndexChanged="filter_passed_SelectedIndexChanged">
                                        <asp:ListItem Value="ALL"></asp:ListItem>
                                        <asp:ListItem Value="PASSED"></asp:ListItem>
                                        <asp:ListItem Value="FAILED"></asp:ListItem>
                                        <asp:ListItem Value="NOSHOW"></asp:ListItem>
                                        <asp:ListItem Value="UNFINISHED"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="filter_grd_start" Width="30" Style="" runat="server" Text="0" Enabled="false" TextMode="Number"></asp:TextBox>
                                    <span style="float: right;"></span>
                                </td>
                                <td>

                                    <asp:TextBox ID="filter_grd_finish" Width="40" Style="float: left;" Text="100" Enabled="false" runat="server" TextMode="Number"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Only Active 
                                    <asp:CheckBox ID="chk_active_exams" OnCheckedChanged="chk_active_exams_CheckedChanged" AutoPostBack="true" runat="server" Checked="true" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>Only Active 
                                    <asp:CheckBox ID="chk_active_trainee" OnCheckedChanged="chk_active_trainee_CheckedChanged" AutoPostBack="true" runat="server" Checked="true" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="8" style="height: 10px;"></td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Label ID="lbl_result" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Button ID="btn_search" CssClass="btn_search" runat="server" Text="SEARCH" OnClick="btn_search_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Button ID="btn_export" OnClientClick="DownloadExcel()" CssClass="btn_search" Style="float: right;" runat="server" Text="Export To Excel" />
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
                        <asp:GridView ID="grid_results" runat="server" CssClass="grid_results" OnRowCommand="grid_results_RowCommand" OnRowDataBound="grid_results_RowDataBound">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" CommandName="GO" ImageUrl="~/images/exam.png" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
