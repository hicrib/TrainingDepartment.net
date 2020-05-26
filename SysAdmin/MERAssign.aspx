<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="MERAssign.aspx.cs" Inherits="AviaTrain.SysAdmin.MERAssign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tbl_defaultMers {
            border-collapse: collapse;
            width: 1000px;
            margin-bottom: 40px;
            font-weight: bold;
            border: 2px solid #a52a2a;
        }



        .grid_MERS {
            border-collapse: collapse;
            width: 700px;
            margin: auto;
        }

            .grid_MERS td {
                padding: 5px;
            }

        .errorbox {
            width: 500px;
            height: 25px;
            background-color: lightgray;
            color: black;
            font-size: medium;
            border: 1px solid #a52a2a;
            text-align: center;
            padding: 5px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>

            <div class="tabcontents">

                <table class="tbl_defaultMers">
                    <tr>
                        <td style="text-align: center;">Trainee</td>
                        <td style="text-align: center;">Position</td>
                        <td style="text-align: center;">Sector</td>
                        <td style="text-align: center;">Step</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:DropDownList ID="ddl_trainee" runat="server" Width="150" AutoPostBack="true" OnSelectedIndexChanged="ddl_trainee_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td style="text-align: center;">
                            <asp:DropDownList ID="ddl_position" runat="server" Width="150" AutoPostBack="true" OnSelectedIndexChanged="ddl_position_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td style="text-align: center;">
                            <asp:DropDownList ID="ddl_sector" runat="server" Width="150" AutoPostBack="true" OnSelectedIndexChanged="ddl_sector_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td style="text-align: center;">
                            <asp:DropDownList ID="ddl_steps" runat="server" Width="200"></asp:DropDownList>
                        </td>
                        <td style="text-align: center;">
                            <asp:Button ID="btn_findMERs" runat="server" Text="Find" OnClick="btn_findMERs_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="height: 40px; text-align: center;">
                            <asp:Label ID="lbl_pageresult" runat="server" CssClass="errorbox" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="text-align: center;">
                            <asp:TextBox ID="txt_comments" runat="server" Width="300" PlaceHolder="Optional comments for altering user information" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div style="text-align: center; width: 1000px;">
                    <asp:GridView ID="grid_defaultMERs" runat="server" CssClass="grid_MERS" OnRowDataBound="grid_defaultMERs_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="MER">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_MER" Width="50" runat="server" AutoPostBack="true" OnTextChanged="txt_MER_TextChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
