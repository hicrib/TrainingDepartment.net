<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="PublishNotification.aspx.cs" Inherits="AviaTrain.SysAdmin.PublishNotification" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ajaxfileuploadCss {
            width: 500px;
        }

        .ajax__fileupload_topFileStatus {
            /*display: none !important;*/
            z-index: -1;
        }

        .tbl_main {
            border-collapse: collapse;
            border: 2px solid #a52a2a;
            width: 700px;
            display: inline-block;
            float: left;
        }

            .tbl_main td {
                width: 250px;
                font-weight: bold;
                min-height: 45px;
                padding: 5px;
            }

                .tbl_main td * {
                    min-height: 30px;
                    width: 99%;
                }

        .file_tbl {
            width: 330px;
            height: 500px;
        }

            .file_tbl td {
                vertical-align: middle;
            }
    </style>
    <script type="text/javascript">  
        function uploadcomplete() {
            alert("Successfully Uploaded!");
        }
        function uploaderror() {
            alert("sonme error occured while uploading file!");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <table class="tbl_main">
                <tr>
                    <td>Type</td>
                    <td>
                        <asp:DropDownList ID="ddl_type" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_type_SelectedIndexChanged">
                            <asp:ListItem Value="BROADCAST"></asp:ListItem>
                            <asp:ListItem Value="POSITION"></asp:ListItem>
                            <asp:ListItem Value="SECTOR"></asp:ListItem>
                            <asp:ListItem Value="ROLE"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_generic" Visible="false" runat="server"></asp:DropDownList>
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
                        <asp:TextBox ID="txt_message" runat="server" Height="150" TextMode="MultiLine" Style="width: 99%; resize: none;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lbl_pageresult" runat="server" Visible="false" Style="font-size: medium; font-weight: bold; color: #a52a2a;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Button ID="btn_publish" runat="server" OnClick="btn_publish_Click"
                            Style="width: 99%; background-color: #a52a2a; font-size: medium; font-weight: bold; color: white;" Text="PUBLISH" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="file_tbl">
        <tr>
            <td>
                <ajaxToolkit:AjaxFileUpload ID="fileupload" runat="server"
                    CssClass="ajaxfileuploadCss" MaxFileSize="19000"
                    MaximumNumberOfFiles="4" OnUploadComplete="fileupload_UploadComplete"
                    OnUploadStart="fileupload_UploadStart"
                    OnClientUploadComplete="uploadcomplete" OnClientUploadError="uploaderror" />
            </td>
        </tr>
    </table>

</asp:Content>
