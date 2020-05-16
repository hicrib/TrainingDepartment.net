<%@ Page Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="ViewTrainingFolder.aspx.cs" Inherits="AviaTrain.SysAdmin.ViewTrainingFolder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #createuser_tbl {
            border: 2px solid #a52a2a;
            border-collapse: collapse;
            width: 1000px;
        }

            #createuser_tbl td {
                padding: 5px;
                font-weight: bold;
                font-size: medium;
                height: 30px;
                width: 140px;
            }

        .imgicon {
            max-width: 25px;
            max-height: 25px;
        }

        .grid_folder {
            width: 1000px;
            border: 2px solid #a52a2a;
            margin-top: 20px;
        }

            .grid_folder th {
                background-color: #beb8b8;
            }
             .grid_folder td {
               padding : 5px;
            }
            .grid_folder td:first-child {
                width: 30px;
            }

            .grid_folder td:nth-child(2) {
                width: 300px;
            }

        select {
            font-weight : bold;
            width : 100%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <div>
                <table id="createuser_tbl" style="">
                    <tbody>
                        <tr>
                            <td>Trainee : </td>
                            <td>
                                <asp:DropDownList ID="ddl_trainees" runat="server"></asp:DropDownList></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>Position </td>
                            <td>
                                <asp:DropDownList ID="ddl_positions" runat="server" OnSelectedIndexChanged="ddl_positions_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="-" Text=" --- "></asp:ListItem>
                                    <asp:ListItem Value="TWR" Text="Tower"></asp:ListItem>
                                    <asp:ListItem Value="APP"></asp:ListItem>
                                    <asp:ListItem Value="ACC"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_sector" runat="server"></asp:DropDownList></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_find_folder" runat="server" OnClick="btn_find_folder_Click" Text="Find" />
                            </td>
                            <td colspan="4"></td>
                        </tr>
                    </tbody>
                </table>
                <asp:GridView ID="grid_folder" AllowPaging="true" AllowSorting="true" CssClass="grid_folder" runat="server" Visible="false"
                    OnSelectedIndexChanged="grid_folder_SelectedIndexChanged" OnPageIndexChanging="grid_folder_PageIndexChanging" PageSize="5"
                    OnSorting="grid_folder_Sorting" OnRowDataBound="grid_folder_RowDataBound">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" FirstPageText="First" LastPageText="Last" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" ButtonType="Image" ControlStyle-CssClass="imgicon" SelectImageUrl="~/Images/view.png" SelectText="View" />
                    </Columns>
                </asp:GridView>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
