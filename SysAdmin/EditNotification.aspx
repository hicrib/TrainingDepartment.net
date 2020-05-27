<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="EditNotification.aspx.cs" Inherits="AviaTrain.SysAdmin.EditNotification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .grid_notifications {
            display: inline-block;
            float: left;
            min-width: 500px;
            max-width: 550px;
            border : none;
        }
        .grid_notifications td{
            padding : 5px;
            font-size : small;
        }
        .grid_notifications td:nth-child(3){
            min-width:150px;
            text-wrap : normal;
            
        }

        .tbl_main {
            display: inline-block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="min-width: 1000px;">


        <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
            </Triggers>
            <ContentTemplate>

                <asp:GridView ID="grid_notifications" runat="server" AutoGenerateColumns="false" CssClass="grid_notifications"
                    AllowPaging="true" AllowSorting="true" 
                    OnSorting="grid_notifications_Sorting" OnPageIndexChanging="grid_notifications_PageIndexChanging" PageSize="10"
                    OnRowDataBound="grid_notifications_RowDataBound" OnRowCommand="grid_notifications_RowCommand">
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First" LastPageText="Last" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btn_go" runat="server" ImageUrl="~/images/view.png" CommandName="GO"
                                    CommandArgument='<%#Convert.ToString(Eval("ID")) %>' CssClass="iconimg" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" SortExpression="ID" HeaderText="ID" />
                        <asp:BoundField DataField="HEADER" SortExpression="HEADER" HeaderText="HEADER" />
                        <asp:BoundField DataField="EFFECTIVE" SortExpression="EFFECTIVE" HeaderText="EFFECTIVE" />
                        <asp:BoundField DataField="EXPIRED" SortExpression="EXPIRED" HeaderText="EXPIRES" />
                        <asp:BoundField DataField="ISACTIVE" SortExpression="ISACTIVE" HeaderText="ISACTIVE" />
                    </Columns>
                </asp:GridView>

                <asp:Label ID="lbl_notifid" runat="server" Visible="false"></asp:Label>


                <table class="tbl_main">
                    <tr>
                        <td>Type</td>
                        <td>
                            <asp:DropDownList ID="ddl_type" runat="server" AutoPostBack="true" Enabled="false">
                                <asp:ListItem Value="BROADCAST"></asp:ListItem>
                                <asp:ListItem Value="POSITION"></asp:ListItem>
                                <asp:ListItem Value="SECTOR"></asp:ListItem>
                                <asp:ListItem Value="ROLE"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_generic" runat="server" Enabled="false"></asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Effective / Expires</td>
                        <td>
                            <asp:TextBox ID="txt_effective" runat="server" TextMode="Date"></asp:TextBox></td>

                        <td>
                            <asp:TextBox ID="txt_expires" runat="server" TextMode="Date"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            Active : 
                            <asp:CheckBox ID="chk_active" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 20px; border-bottom: 1px solid #a52a2a;"></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center;">HEADER
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:TextBox ID="txt_header" runat="server" Style="width: 99%;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center;">MESSAGE
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:TextBox ID="txt_message" runat="server" Height="150" TextMode="MultiLine" Style="width: 99%; "></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <div>
                                <asp:LinkButton ID="lnk_file1" runat="server" CssClass="files" Visible="false"></asp:LinkButton>
                                <asp:ImageButton ID="del_file1" runat="server" ImageUrl="~/images/delete.png" CssClass="imgicon" Visible="false" OnClick="del_file_Click"/>
                            </div>
                            <div>
                                <asp:LinkButton ID="lnk_file2" runat="server" CssClass="files" Visible="false"></asp:LinkButton>
                                <asp:ImageButton ID="del_file2" runat="server" ImageUrl="~/images/delete.png" CssClass="imgicon" Visible="false" OnClick="del_file_Click" />
                            </div>
                            <div>
                                <asp:LinkButton ID="lnk_file3" runat="server" CssClass="files" Visible="false"></asp:LinkButton>
                                <asp:ImageButton ID="del_file3" runat="server" ImageUrl="~/images/delete.png" CssClass="imgicon" Visible="false" OnClick="del_file_Click"/>
                            </div>
                            <div>
                                <asp:LinkButton ID="lnk_file4" runat="server" CssClass="files" Visible="false"></asp:LinkButton>
                                <asp:ImageButton ID="del_file4" runat="server" ImageUrl="~/images/delete.png" CssClass="imgicon" Visible="false" OnClick="del_file_Click" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lbl_pageresult" runat="server" Visible="false" Style="font-size: medium; font-weight: bold; color: #a52a2a;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btn_publish" runat="server" OnClick="btn_publish_Click" Visible="false"
                                Style="width: 99%; background-color: #a52a2a; font-size: medium; font-weight: bold; color: white;" Text="SAVE" />
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
