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

        .errorbox{

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
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lbl_pageresult" CssClass="errorbox" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <asp:GridView ID="grid_folder"  CssClass="grid_folder" runat="server" Visible="false" 
                     OnRowCommand="grid_folder_RowCommand"
                     OnRowDataBound="grid_folder_RowDataBound">
                    <Columns>
                        <asp:ButtonField CommandName="GO"  ButtonType="Image" ControlStyle-CssClass="imgicon" ImageUrl="~/Images/view.png"  />
                    </Columns>
                </asp:GridView>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
