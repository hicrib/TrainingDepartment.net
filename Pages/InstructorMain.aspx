<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/MainsMaster.Master" AutoEventWireup="true" CodeBehind="InstructorMain.aspx.cs" Inherits="AviaTrain.Pages.InstructorMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        table, tr, td {
            border-collapse: collapse;
        }

        .ojtireports_tbl {
            border-collapse: collapse;
            height: 100px;
            width: 100%;
            border: 2px solid #b63838	;
        }

        .ojtireports_tbl {
            width: 100%;
            border-collapse: collapse;
        }

            .ojtireports_tbl * {
                padding: 5px;
                align-self: center;
                text-align: center;
            }

            .ojtireports_tbl td:nth-child(2) {
                padding: 5px;
                text-align: center;
            }

        .instructorButton {
            width: 100%;
            background-color: #46a2e8;
            font-weight: bold;
            font-size: medium;
            color: white;
            border: 1px solid #b63838	;
        }

        #ContentPlaceHolder1_actionstbl td{
            padding:5px;
            margin:0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <br />
    <br />
    <asp:Table runat="server" ID="actionstbl" CssClass="ojtireports_tbl">
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
                <asp:Button ID="btn_my_training" runat="server" Text="My Training" OnClick="btn_my_training_Click" CssClass="instructorButton" />
            </asp:TableCell>
            <asp:TableCell ColumnSpan="2">
                <asp:Button ID="btn_create_report" runat="server" Text="Create Report" OnClick="btn_create_report_Click" CssClass="instructorButton" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <br />
    <asp:Table runat="server" ID="ojtireports_tbl" CssClass="ojtireports_tbl">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell ColumnSpan="4" Style="border: 3px solid #b63838	; background-color: #b63838	; color: white; font-weight: bold; align-content: center;">
                                                REPORTS I CREATED
            </asp:TableHeaderCell>

        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="4">
                <asp:GridView ID="grid_ojti_reports" AllowPaging="true" AllowSorting="true" runat="server"
                    OnSelectedIndexChanged="grid_ojti_reports_SelectedIndexChanged" OnPageIndexChanging="grid_ojti_reports_PageIndexChanging" PageSize="10"
                    OnSorting="grid_ojti_reports_Sorting">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" ButtonType="Image" SelectImageUrl="~/Images/view.png" SelectText="View" />
                    </Columns>
                </asp:GridView>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
