<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="PositionLog.aspx.cs" Inherits="AviaTrain.Pages.PositionLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tabs {
            position: relative;
            margin-top: 3px;
            z-index: 2;
            width: 1000px;
        }

        .tab {
            border: 2px solid #a52a2a;
            background-color: lightgray;
            background-repeat: repeat-x;
            color: White;
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

        .login_tbl {
            width: 600px;
            border: 1px solid #a52a2a;
            margin: 3px 0px 3px 0px;
        }

            .login_tbl td:first-child {
                width: 50%;
                font-weight: bold;
            }

            .login_tbl td:nth-child(3) {
                width: 30%;
            }

            .login_tbl td:nth-child(3) {
                width: 20%;
            }

        .errorbox {
            height: 30px;
            background-color: lightgray;
            color: black;
            font-size: large;
            border: 1px solid red;
            text-align: center;
            padding: 5px;
        }

        .submitbtn {
            width: 70px;
            font-weight: bold;
            background-color: #a52a2a;
            color: white;
        }

        .gridlog {
            float: right;
            width: 100%;
            padding: 5px;
            background-color: white;
            font-size: small;
            font-weight: bold;
            border: 2px solid black;
        }

            .gridlog td {
                text-align: center;
            }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Conditional" ChildrenAsTriggers="true" style="margin-left: 3px;">
        <ContentTemplate>
            <asp:Menu ID="menu_logs" Orientation="Horizontal" StaticMenuItemStyle-CssClass="tab"
                StaticSelectedStyle-CssClass="selected" CssClass="tabs" runat="server"
                OnMenuItemClick="menu_logs_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Login" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="CoB" Value="1"></asp:MenuItem>
                </Items>
            </asp:Menu>
            <div class="tabcontents">
                <asp:MultiView ID="multiview1" ActiveViewIndex="0" runat="server">
                    <asp:View ID="view_positionlogin" runat="server">

                        <table style="width: 1000px; margin-left: 0px !important;">
                            <tr>
                                <td>
                                    <table class="login_tbl">
                                        <tr>
                                            <td>Position :  </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_position" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddl_position_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="-" Text="---"></asp:ListItem>
                                                    <asp:ListItem Value="TWR" Text="Tower"></asp:ListItem>
                                                    <asp:ListItem Value="ACC" Text="ACC"></asp:ListItem>
                                                    <asp:ListItem Value="APP" Text="APP"></asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td>
                                                <asp:DropDownList ID="ddl_sector" CssClass="dropdown" OnSelectedIndexChanged="ddl_sector_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Time : </td>
                                            <td>
                                                <asp:TextBox ID="txt_dateon" runat="server" TextMode="Date"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_timeon" runat="server" TextMode="Time"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 30px;"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Are you current?
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rad_current" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Are you fit to work?
                                            </td>

                                            <td>
                                                <asp:RadioButtonList ID="rad_fit" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Did you read the latest TOI/POI/NOTAMs?
                                            </td>

                                            <td>
                                                <asp:RadioButtonList ID="rad_reading" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Are you working solo?
                                            </td>

                                            <td>
                                                <asp:RadioButtonList ID="rad_solo" runat="server" OnSelectedIndexChanged="rad_solo_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center;">
                                                <asp:Label ID="lbl_trainee" runat="server" Text="Trainee : " Visible="false"></asp:Label>
                                                <asp:DropDownList ID="ddl_trainee" Style="float: right;" runat="server" Visible="false"></asp:DropDownList>
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 30px;"></td>
                                        </tr>
                                        <tr>

                                            <td colspan="2" style="text-align: center;">
                                                <asp:Label ID="lbl_pageresult" CssClass="errorbox" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_login" CssClass="submitbtn" runat="server" OnClick="btn_login_Click" Text="LOGIN" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:GridView ID="grid_log" runat="server" OnRowDataBound="grid_log_RowDataBound" CssClass="gridlog" Visible="false"></asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:View>

                    <asp:View ID="view_CoB" runat="server">
                        <table style="width: 1000px; margin-left: 0px !important;">
                            <tr>
                                <td>
                                    <table class="login_tbl">
                                        <tr>
                                            <td>Position :  </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_CoBposition" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddl_CoBposition_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="-" Text="---"></asp:ListItem>
                                                    <asp:ListItem Value="TWR" Text="Tower"></asp:ListItem>
                                                    <asp:ListItem Value="ACC" Text="ACC"></asp:ListItem>
                                                    <asp:ListItem Value="APP" Text="APP"></asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td>
                                                <asp:DropDownList ID="ddl_CoBsector" CssClass="dropdown" OnSelectedIndexChanged="ddl_CoBsector_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>CoB Time : </td>
                                            <td>
                                                <asp:TextBox ID="txt_CoBdate" runat="server" TextMode="Date"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_CoBtime" runat="server" TextMode="Time"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height: 30px;"></td>
                                        </tr>
                                        <tr>

                                            <td colspan="2" style="text-align: center;">
                                                <asp:Label ID="lbl_CoBresult" CssClass="errorbox" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_CoB" CssClass="submitbtn" runat="server" OnClick="btn_CoB_Click" Text="CoB" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:Label ID="lbl_cobid" runat="server" Visible="false"></asp:Label>
                                    <asp:GridView ID="grid_COB" runat="server" OnRowDataBound="grid_COB_RowDataBound" CssClass="gridlog" Visible="false"></asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
