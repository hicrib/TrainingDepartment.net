<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="UserNotifications.aspx.cs" Inherits="AviaTrain.Pages.UserNotifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .main_tbl {
            width: 1000px;
            border-collapse: collapse;
            display: inline-block;
            float: left;
            width: 500px;
        }

        .notif_tbl {
            width: 100%;
            display: inline-block;
            border-collapse: collapse;
            border: 2px solid indianred;
        }

            .notif_tbl td {
                border: 1px solid indianred;
                width : 500px;
            }

        .grid_notif {
            width: 100%;
        }
        .grid_notif th{
           background-color : lightgray;
        }

        .grid_notif td {
            padding : 5px;
        }

        .iconimg {
            max-width: 25px;
            max-height: 25px;
        }

        .header {
            font-weight: bold;
            font-size: large;
            margin: 5px;
            height: 30px;
            width: 100%;
        }

        .message {
            height: 300px;
        }

        .files {
            margin-left : 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="main_tbl">
        <tr>
            <td>
                <asp:GridView ID="grid_notif" runat="server" CssClass="grid_notif"
                    OnRowDataBound="grid_notif_RowDataBound" AllowPaging="true" AllowSorting="true"
                    OnPageIndexChanging="grid_notif_PageIndexChanging" PageSize="10"
                    OnSorting="grid_notif_Sorting" OnRowCommand="grid_notif_RowCommand">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" ImageUrl="~/images/view.png" CommandName="VIEWNOTIF" ControlStyle-CssClass="iconimg" />
                    </Columns>
                </asp:GridView>

            </td>
        </tr>
    </table>
    <asp:Panel ID="pnl_show_notification" runat="server" Visible="false" style="width:494px; display:inline-block;">
        <table class="notif_tbl">
            <tr>
                <td style="height:30px; padding: 5px; text-align:center;"
                    <asp:Label ID="lbl_header" CssClass="header"  runat="server"></asp:Label>

                </td>
            </tr>
            <tr>
                <td style="height: 300px; padding: 5px; text-align: justify;">
                    <asp:Label ID="lbl_message" CssClass="message" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-weight:bold ; ">FILES : 
                    <asp:LinkButton ID="lnk_file1" runat="server" CssClass="files" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="lnk_file2" runat="server" CssClass="files" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="lnk_file3" runat="server" CssClass="files" Visible="false"></asp:LinkButton>
                    <asp:LinkButton ID="lnk_file4" runat="server" CssClass="files" Visible="false"></asp:LinkButton>
                </td>
        </table>
    </asp:Panel>






</asp:Content>
