<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="FileTypeDefinition.aspx.cs" Inherits="AviaTrain.SysAdmin.FileTypeDefinition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .main_tbl {
            width: 1000px;
            border-collapse: collapse;
            border: 2px solid #a52a2a;
        }

            .main_tbl td {
                width: 25%;
                padding: 5px;
                text-align: center;
                height: 25px;
            }

        .grid_types {
            width: 600px;
            border: 1px solid #a52a2a;
            display: block;
            margin: auto;
        }

            .grid_types td {
                padding: 5px;
            }
            .grid_types th {
                background-color : gray;
            }

                .grid_types td:first-child {
                    width: 300px;
                    text-align : left !important;
                }

                .grid_types td:nth-child(2) {
                    width: 120px;
                }

                .grid_types td:nth-child(3) {
                    width: 100px;
                }

                .grid_types td:nth-child(4) {
                    width: 100px;
                }

        .errorbox {
            width: 600px;
            height: 25px;
            background-color: lightgray;
            color: black;
            font-size: medium;
            border: 1px solid #a52a2a;
            text-align: center;
            padding: 5px;
            float: right !important;
        }

        .btn_submit {
            width: 50%;
            background-color: #a52a2a;
            font-size: large;
            font-weight: bold;
            color: white;
            height: 30px;
        }
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Always" ChildrenAsTriggers="true">
        <ContentTemplate>
            <table class="main_tbl">
                <tr>
                    <td>File Type Name</td>
                    <td>Issue Date Required</td>
                    <td>Expiration Required</td>
                    <td>Role Specific </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt_filetype" Style="width: 100%;" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_issue" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_expiration" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_rolespec" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lbl_pageresult" runat="server" CssClass="errorbox" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btn_submit" CssClass="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click" />
                    </td>
                </tr>
            </table>
            <div style="width: 1000px; text-align: center;">
                <asp:GridView ID="grid_types" runat="server" CssClass="grid_types"></asp:GridView>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
