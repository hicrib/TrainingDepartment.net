<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="LevelObjectives.aspx.cs" Inherits="AviaTrain.Reports.LevelObjectives" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .errorbox {
            width: 500px;
            height: 35px;
            background-color: lightgray;
            color: black;
            font-size: medium;
            border: 1px solid #a52a2a;
            text-align: center;
            padding: 5px;
        }

        .CATEGORYROW {
            text-align: center;
            background-color: gray;
            font-size: large !important;
            font-weight: bold;
            height: 30px;
        }

        .grid_objectives {
            width: 988px;
            background-color: #ffffff;
        }

            .grid_objectives td {
                padding : 2px 0px 2px 5px;
                font-size: medium;
            }

        .mergedCell {
            font-weight: bold !important;
            font-size: large !important;
        }

        .byCell {
            width: 100px;
        }


        .main_tbl {
            width: 1000px;
            border: 1px solid #a52a2a;
        }

            .main_tbl td {
                width: 333px;
                height: 35px;
                padding: 5px;
            }

        .grid_wrapper {
            border-collapse: collapse;
            border : 2px solid #a52a2a;
        }

            .grid_wrapper th {
                background-color: #a52a2a;
                color: white;
                text-align: center;
                font-weight: bold;
                font-size: large;
            }
    </style>
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="uppanel" UpdateMode="Always">
        <ContentTemplate>

            <table class="main_tbl">
                <tr>
                    <td colspan="3" style="text-align: center; font-weight: bold;">Trainee  :<asp:DropDownList ID="ddl_trainee" runat="server" Width="200" Height="35" AutoPostBack="true" OnSelectedIndexChanged="ddl_trainee_SelectedIndexChanged"></asp:DropDownList>
                        <asp:DropDownList ID="ddl_position" runat="server" Width="200" Height="35" AutoPostBack="true" OnSelectedIndexChanged="ddl_position_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: center;">
                        <asp:Label ID="lbl_pageresult" runat="server" CssClass="errorbox" Visible="false"></asp:Label></td>
                </tr>
            </table>


            <asp:Panel ID="pnl_grid" Visible="false" runat="server">

                <table class="grid_wrapper">
                    <tr>
                        <th>
                            <asp:Label ID="lbl_tableheader" runat="server"></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grid_objectives" runat="server" CssClass="grid_objectives" ShowHeader="false" AutoGenerateColumns="false"
                                OnPreRender="grid_objectives_PreRender" OnRowCommand="grid_objectives_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="ID" />
                                    <asp:BoundField DataField="CATEGORY" />
                                    <asp:BoundField DataField="HEADER" />
                                    <asp:BoundField DataField="OBJECTIVE" />
                                    <asp:BoundField DataField="BY" />
                                    <asp:BoundField DataField="FORMID" />
                                    <%--<asp:ButtonField ButtonType="Image" Visible='<%# Eval("BY") != "&nbsp;" %>' ImageUrl="~/images/sign.png" CommandName="SIGN" ControlStyle-CssClass="iconimg" />--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_sign" Text="sign" Visible='<%# show_sign( Eval("BY").ToString() )%>' runat="server"
                                                ImageUrl='<%# show_url( Eval("BY").ToString() )%>'
                                                CommandName="SIGN" CommandArgument='<%#Eval("BY")+","+Eval("ID") +","+Eval("FORMID") %>' CssClass="iconimg" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>


        </ContentTemplate>
    </asp:UpdatePanel>





</asp:Content>
