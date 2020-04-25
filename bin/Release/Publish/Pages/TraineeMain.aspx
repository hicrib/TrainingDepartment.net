<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/MainsMaster.Master" AutoEventWireup="true" CodeBehind="TraineeMain.aspx.cs" Inherits="AviaTrain.Pages.TraineeMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #ContentPlaceHolder1_traineeports_tbl {
            border-collapse: collapse;
            height: 100px;
            width: 100%;
            border: 2px solid #b63838	;
        }

         #ContentPlaceHolder1_grid_trainee_reports {
            width: 100%;
        }

            #ContentPlaceHolder1_grid_trainee_reports * {
                padding: 5px;
                align-self: center;
                text-align: center;
            }

            #ContentPlaceHolder1_grid_trainee_reports td:nth-child(2) {
                padding: 5px;
                text-align: center;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lbl_traineeID" runat="server" Visible="false"></asp:Label>
    <asp:Table runat="server" ID="traineeports_tbl">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell ColumnSpan="4" Style="border: 3px solid #b63838	; background-color: #b63838	; color: white; font-weight: bold; align-content: center;">
                                                MY TRAINING REPORTS
            </asp:TableHeaderCell>

        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:GridView ID="grid_trainee_reports" AutoGenerateSelectButton="true" AllowPaging="true" AllowSorting="true" runat="server"
                    OnSelectedIndexChanged="grid_trainee_reports_SelectedIndexChanged" OnPageIndexChanging="grid_trainee_reports_PageIndexChanging" PageSize="10"
                    OnSorting="grid_trainee_reports_Sorting">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last" />
                </asp:GridView>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
