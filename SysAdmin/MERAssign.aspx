<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="MERAssign.aspx.cs" Inherits="AviaTrain.SysAdmin.MERAssign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tabs {
            position: relative;
            top: 1px;
            z-index: 2;
            width: 500px;
        }

        .tab {
            border: 2px solid #a52a2a;
            background-color: lightgray;
            background-repeat: repeat-x;
            color: White;
            padding: 2px 10px;
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
            border: 2px solid #a52a2a;
            padding: 10px;
            width: 975px;
            height: 500px;
            background-color: #e1e1e1;
        }

        .tbl_defaultMers {
            border-collapse : collapse;
            width:700px;
            margin-bottom : 40px;
            font-weight :bold;
        }
        .grid_MERS{
            border-collapse : collapse;
            width : 700px;
        }
        .grid_MERS td{
            padding : 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
            <asp:PostBackTrigger ControlID="Menu1" />
        </Triggers>
        <ContentTemplate>

            <asp:Menu ID="Menu1" Orientation="Horizontal" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selected" CssClass="tabs" runat="server"
                OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="DEFAULT MERs" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="USER MERs" Value="1"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <div class="tabcontents">
                <asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
                    <asp:View ID="view_defaultMERs" runat="server">
                        <table class="tbl_defaultMers">
                            <tr>
                                <td>Position</td>
                                <td>Sector</td>
                                <td>Step</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddl_position" runat="server" Width="150" AutoPostBack="true" OnSelectedIndexChanged="ddl_position_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_sector" runat="server" Width="150" AutoPostBack="true" OnSelectedIndexChanged="ddl_sector_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_steps" runat="server" Width="200"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_findMERs" runat="server" Text="Find" OnClick="btn_findMERs_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="height:40px;"></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:GridView ID="grid_defaultMERs" runat="server" CssClass="grid_MERS" OnRowCommand="grid_defaultMERs_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="MER">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_MER" Width="30" runat="server" AutoPostBack="true" OnTextChanged="txt_MER_TextChanged" TextMode="Number" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                </td>
                            </tr>
                        </table>


                    </asp:View>


                    <asp:View ID="view_userMERs" runat="server">
                         --Coming Soon--
                    </asp:View>

                </asp:MultiView>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
