<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="ViewTrainings.aspx.cs" Inherits="AviaTrain.Trainings.ViewTrainings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .grid_view_trainings {
            font-size: small;
            width: 1000px;
        }

            .grid_view_trainings th {
                background-color: #a52a2a;
                font-weight: bold;
            }

            .grid_view_trainings td {
                padding: 5px;
                font-weight: bold;
            }

                .grid_view_trainings td:first-child {
                    max-height: 25px;
                    max-width: 25px;
                }

                .grid_view_trainings td:nth-child(3), td:nth-child(6), td:nth-child(10) {
                    text-align: center;
                }

        .viewbutton {
            max-height: 25px;
            max-width: 25px;
        }
    </style>
    <script>
        function RefreshMe() {
            __doPostBack('ContentPlaceHolder1_btn_hidden_refresh', "");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>

            <asp:GridView ID="grid_trainings" runat="server" OnRowCommand="grid_trainings_RowCommand"
                AllowSorting="true" OnPageIndexChanging="grid_trainings_PageIndexChanging" PageSize="20"
                OnSorting="grid_trainings_Sorting"
                OnRowDataBound="grid_trainings_RowDataBound" CssClass="grid_view_trainings">
                <PagerSettings Mode="NumericFirstLast" PageButtonCount="6" FirstPageText="First" LastPageText="Last" />
                <Columns>
                    <asp:ButtonField CommandName="VIEWTRAINING" ButtonType="Image" ImageUrl="~/images/view.png" ControlStyle-CssClass="viewbutton" />
                    <asp:ButtonField CommandName="SAVEAS" ButtonType="Image" ImageUrl="~/images/saveas.png" ControlStyle-CssClass="viewbutton" HeaderText="SaveAs" />
                    <asp:ButtonField CommandName="INACTIVE" ButtonType="Image" ImageUrl="~/images/delete.png" ControlStyle-CssClass="deletebutton" HeaderText="Toggle" />
                </Columns>
            </asp:GridView>



            <asp:Button ID="btn_hidden_refresh" runat="server" style="display:none;;" Text ="aaa" OnClientClick="RefreshMe()" OnClick="btn_hidden_refresh_Click"/>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
