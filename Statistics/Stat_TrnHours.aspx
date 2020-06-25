<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="Stat_TrnHours.aspx.cs" Inherits="AviaTrain.Statistics.Stat_TrnHours" %>

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
            margin: 3px;
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
                padding: 0px;
                min-height: 25px;
            }

        .filters_tbl {
            width: 99%;
            border-collapse: collapse;
            align-content: center;
            border: 1px solid #a52a2a;
        }

            .filters_tbl td {
                font-weight: bold;
                align-content: center;
                text-align: center;
                padding : 5px;
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
                                <td>Trainee </td>
                                <td>Unit</td>
                                <td>Sector</td>
                                <td colspan="2">Between Dates</td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:DropDownList ID="filter_trainee" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="filter_unit" runat="server" AutoPostBack="true" OnSelectedIndexChanged="filter_unit_SelectedIndexChanged">
                                        <asp:ListItem Value="-" Text="-"></asp:ListItem>
                                        <asp:ListItem Value="TWR"></asp:ListItem>
                                        <asp:ListItem Value="ACC"></asp:ListItem>
                                        <asp:ListItem Value="APP"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="filter_sector" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox TextMode="Date" ID="filter_start" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox TextMode="Date" ID="filter_finish" runat="server"></asp:TextBox>
                                </td>
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
                            <tr>
                                <td colspan="5">
                                    <asp:Button ID="btn_export" OnClientClick="DownloadExcel()" CssClass="btn_search" Style="float: right;" runat="server" Text="Export To Excel" OnClick="btn_export_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table class="exam_results_tbl">
                <tr>
                    <th><asp:Label ID="lbl_gridname" runat="server" ></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grid_trnhours" runat="server" CssClass="grid_results">
                            <%--AllowPaging="true" AllowSorting="true"--%>
                            <%--OnSelectedIndexChanged="grid_trnhours_SelectedIndexChanged" OnSorting="grid_trnhours_Sorting"--%>
                            <%--<PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />--%>
                            <Columns>
                                <%--<asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="iconimg" ButtonType="Image" SelectImageUrl="~/Images/view.png" SelectText="View" />--%>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>






</asp:Content>
