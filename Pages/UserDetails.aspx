<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="AviaTrain.Pages.UserDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body, table, tr, td, th {
            border-collapse: collapse;
            margin: 3px;
            padding: 0px;
        }

        /*FOLLOWING ARE FOR MULTIVIEW */
        .tabs {
            position: relative;
            margin-top: 3px;
            z-index: 2;
            width: 700px;
        }

        .tab {
            border: 2px solid #a52a2a;
            background-color: lightgray;
            background-repeat: repeat-x;
            color: White;
            width: 100px;
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
            border: 1px solid #a52a2a;
            padding: 3px;
            width: 993px;
            height: 93%;
            background-color: #e1e1e1;
        }
        /*FINISHED :   FOR MULTIVIEW ACC-APP-TOWER*/




        /*FOLLOWING ARE FOR ACCORDEON - RIGHT PANES CERTIFICATES*/
        .headerCssClass {
            background-image: url('../images/expand.png');
            background-repeat: no-repeat;
            background-position: center center;
            background-size: 15px;
            background-color: #cccccc;
            color: #716e6e;
            border: 1px solid black;
            padding: 4px;
            font-weight: bold;
        }

        .headerSelectedCss {
            background-color: #a52a2a;
            color: white;
            border: 1px solid black;
            padding: 4px;
            font-weight: bold;
        }

        .contentCssClass {
            background-color: #e1e1e1;
            color: black;
            border: 1px solid black;
            padding: 5px;
        }

            .contentCssClass img {
                display: block;
                margin-left: auto;
                margin-right: auto;
                max-width: 130px;
                max-height: 130px;
            }

        /*FINISHED:  ARE FOR ACCORDEON - RIGHT PANES CERTIFICATES*/



        .folder_headerCssClass {
            background-image: url('../images/expand.png');
            background-repeat: no-repeat;
            background-position: center;
            background-color: #cccccc;
            color: #716e6e;
            border: 1px solid black;
            padding: 4px;
            font-weight: bold;
        }

        .folder_headerSelectedCss {
            background-color: #a52a2a;
            color: white;
            border: 1px solid black;
            padding: 4px;
            font-weight: bold;
        }

        .folder_contentCssClass {
            background-color: #e1e1e1;
            color: black;
            border: 1px solid black;
            padding: 5px;
        }

        .ajaxfileuploadCss {
        }

        .ajax__fileupload_topFileStatus {
            display: none !important;
        }






        .userinfo_tbl {
            margin-top: 8px;
            width: 1000px;
            height: 530px;
            border-collapse: collapse;
            border: 2px solid #a52a2a;
        }

            .userinfo_tbl td {
            }

        .certificates_tbl {
            height: 100%;
            width: 100%;
            border: 1px solid #a52a2a;
            background-color: lightgray;
            border-collapse: collapse;
        }

            .certificates_tbl td {
                border: 1px solid black;
                font-size: small;
                height: 40px;
            }

        .folder_grids {
            width: 70%;
            margin-left: 50px;
            font-size: small;
            font-weight: bold;
        }

            .folder_grids td {
                padding: 5px;
            }

        .list_roles {
            height: 95%;
            width: 95%;
            padding: 2%;
            margin: 1%;
        }

        .treeview_css {
            max-width: 300px;
            padding: 0px;
            margin: 0px;
            overflow : auto;
            overflow-x: visible;
            overflow-y: visible;
            height: 400px;
        }

            .treeview_css img {
                width: 15px;
                height: 15px;
                margin-right: 2px;
            }

        .treeNode {
            transition: all .2s;
            padding: 0px;
            text-align: center;
            margin: 0;
            text-decoration: none !important;
            color: black;
            font-size: 14px;
            background-size: 20px;
        }

        .rootNode {
            font-size: 14px;
            color: black;
            background-size: 20px;
        }

        .leafNode {
            font-size: 14px;
            padding: 0px;
            color: black;
            background-size: 20px;
        }

        .selectNode {
            font-weight: bold;
            color: #a52a2a;
            font-size: 14px;
            background-size: 20px;
        }


        .newfiile_tbl {
            border-collapse: collapse;
            border: 1px solid #a52a2a;
            background-color: #f6d9f4;
            min-width: 660px;
        }

            .newfiile_tbl td {
                font-weight: bold;
                text-align: center;
                height: 30px;
            }

            .gridwrapper{
                text-align:center;
                width:99%;
                
            }
            .foldername{
                font-weight : bold;
                font-size : x-large;
                border : 2px solid black;
                padding : 0px 50px 0px 50px;
            }
            .grid_userfiles{width : 100%;
                            margin : 30px 0 0 50px;
                            margin : auto;
                            border : 2px solid #a52a2a;
            }
            .grid_userfiles td{ padding : 3px 3px 3px 10px;}
            .grid_userfiles td:first-child {width : 40px;}
            .grid_userfiles td:nth-child(2) {text-align:left !important;}
    </style>
    <script type="text/javascript">  
        function uploadcomplete() {
            alert("Successfully Uploaded!");
            __doPostBack('ContentPlaceHolder1_btn_hidden_refresh', "");
        }
        function uploaderror() {
            alert("sonme error occured while uploading file!");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_hidden_refresh" />
            <asp:PostBackTrigger ControlID="btn_uploadnewfile" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="panel_selectuser" runat="server" Visible="false">
                <asp:Label ID="lbl_user" runat="server" Text="User"></asp:Label>
                <asp:DropDownList ID="ddl_user" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_user_SelectedIndexChanged"></asp:DropDownList>
            </asp:Panel>

            <table class="userinfo_tbl">
                <tr>
                    <%--<td style="width: 300px; padding: 1px !important; vertical-align: top;"></td>--%>

                    <td style="width: 1000px; border-left: none !important; border-right: none !important;">
                        <asp:Menu ID="Menu1" Orientation="Horizontal" StaticMenuItemStyle-CssClass="tab"
                            StaticSelectedStyle-CssClass="selected" CssClass="tabs" runat="server"
                            OnMenuItemClick="Menu1_MenuItemClick">
                            <Items>
                                <asp:MenuItem Text="User Info" Value="0" Selected="true"></asp:MenuItem>
                                <asp:MenuItem Text="User Files" Value="1"></asp:MenuItem>
                                <asp:MenuItem Text="ACC" Value="2"></asp:MenuItem>
                                <asp:MenuItem Text="APP" Value="3"></asp:MenuItem>
                                <asp:MenuItem Text="TOWER" Value="4"></asp:MenuItem>

                            </Items>
                        </asp:Menu>
                        <div class="tabcontents">
                            <asp:MultiView ID="multiview1" ActiveViewIndex="0" runat="server">
                                <asp:View ID="view1" runat="server">
                                    <ajaxToolkit:Accordion ID="Accordion1" runat="server" HeaderCssClass="headerCssClass" ContentCssClass="contentCssClass"
                                        HeaderSelectedCssClass="headerSelectedCss" FadeTransitions="false" TransitionDuration="100"
                                        AutoSize="Fill" Height="400" Style="height: 100% !important;" SelectedIndex="0">
                                        <Panes>
                                            <ajaxToolkit:AccordionPane ID="AccordionPane11" runat="server">
                                                <Header>Info
                                        </Header>
                                                <Content>
                                                    UserID :
                                        <asp:Label ID="lbl_userid" runat="server"></asp:Label>
                                                    <br />
                                                    Name :
                                        <asp:Label ID="lbl_Name" runat="server"></asp:Label>
                                                    <br />
                                                    Initial :
                                        <asp:Label ID="lbl_initial" runat="server"></asp:Label>
                                                    <br />
                                                    Email :
                                        <asp:Label ID="lbl_email" runat="server"></asp:Label>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="AccordionPane9" runat="server">
                                                <Header>Password Change
                                        </Header>
                                                <Content>
                                                    <asp:TextBox ID="txt_pass1" runat="server" TextMode="Password"></asp:TextBox>
                                                    <asp:TextBox ID="txt_pass2" runat="server" TextMode="Password" PlaceHolder="Confirm"></asp:TextBox>
                                                    <asp:Button ID="btn_changepassword" runat="server" Text="Change!" OnClick="btn_changepassword_Click" />
                                                    <br />
                                                    <asp:Label ID="lbl_passchange" runat="server" Style="font-size: small;"></asp:Label>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="AccordionPane10" runat="server">
                                                <Header>Email
                                        </Header>
                                                <Content>
                                                    <asp:TextBox ID="txt_email" runat="server" ></asp:TextBox>
                                                    <asp:Button ID="btn_update_email" runat="server" Text="Save" OnClick="btn_update_email_Click" />
                                                    <asp:Image ID="img_email_result" runat="server" style="display:inline;" Visible="false" />
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_roles" runat="server" Visible="false">
                                                <Header>User Roles
                                        </Header>
                                                <Content>
                                                    <asp:ListBox ID="list_roles" runat="server" Enabled="false" CssClass="list_roles"></asp:ListBox>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_userphoto" runat="server">
                                                <Header>User photo 
                                        </Header>
                                                <Content>
                                                    <asp:Image ID="img_userphoto" runat="server" />
                                                    <ajaxToolkit:AjaxFileUpload ID="fileupload_photo" runat="server"
                                                        AllowedFileTypes="jpg,jpeg,png" CssClass="ajaxfileuploadCss" MaxFileSize="2000"
                                                        MaximumNumberOfFiles="1" OnUploadComplete="fileupload_photo_UploadComplete"
                                                        OnClientUploadComplete="uploadcomplete" OnClientUploadError="uploaderror" />
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                                                <Header>User Signature</Header>
                                                <Content>
                                                    <asp:Image ID="img_signature" runat="server" />
                                                    <ajaxToolkit:AjaxFileUpload ID="fileupload_signature" runat="server"
                                                        AllowedFileTypes="jpg,jpeg,png" CssClass="ajaxfileuploadCss" MaxFileSize="1000"
                                                        MaximumNumberOfFiles="1" OnUploadComplete="fileupload_photo_UploadComplete"
                                                        OnClientUploadComplete="uploadcomplete" OnClientUploadError="uploaderror" />
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <%--<ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                                    <Header>Certificate 1 - Academy</Header>
                                    <Content>
                                        <asp:Image ID="img_cert_academy" runat="server" />
                                        <ajaxToolkit:AjaxFileUpload ID="fileupload_cert_academy" runat="server"
                                            AllowedFileTypes="jpg,jpeg,png" CssClass="ajaxfileuploadCss" MaxFileSize="1000"
                                            MaximumNumberOfFiles="1" OnUploadComplete="fileupload_photo_UploadComplete"
                                            OnClientUploadComplete="uploadcomplete" OnClientUploadError="uploaderror" />
                                    </Content>
                                </ajaxToolkit:AccordionPane>
                                <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server">
                                    <Header>Certificate 2 - OJT Course</Header>
                                    <Content>
                                        <asp:Image ID="img_ojtcourse" runat="server" />
                                        <ajaxToolkit:AjaxFileUpload ID="fileupload_ojtcourse" runat="server"
                                            AllowedFileTypes="jpg,jpeg,png" CssClass="ajaxfileuploadCss" MaxFileSize="1000"
                                            MaximumNumberOfFiles="1" OnUploadComplete="fileupload_photo_UploadComplete"
                                            OnClientUploadComplete="uploadcomplete" OnClientUploadError="uploaderror" />
                                    </Content>
                                </ajaxToolkit:AccordionPane>
                                <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
                                    <Header>Sup. Course Certificate</Header>
                                    <Content>
                                        <asp:Image ID="img_supcourse" runat="server" />
                                        <ajaxToolkit:AjaxFileUpload ID="fileupload_supcourse" runat="server"
                                            AllowedFileTypes="jpg,jpeg,png" CssClass="ajaxfileuploadCss" MaxFileSize="1000"
                                            MaximumNumberOfFiles="1" OnUploadComplete="fileupload_photo_UploadComplete"
                                            OnClientUploadComplete="uploadcomplete" OnClientUploadError="uploaderror" />
                                    </Content>
                                </ajaxToolkit:AccordionPane>
                                <ajaxToolkit:AccordionPane ID="AccordionPane5" runat="server">
                                    <Header>ECT Certificate</Header>
                                    <Content>
                                        <asp:Image ID="img_ECT" runat="server" />
                                        <ajaxToolkit:AjaxFileUpload ID="fileupload_ECT" runat="server"
                                            AllowedFileTypes="jpg,jpeg,png" CssClass="ajaxfileuploadCss" MaxFileSize="1000"
                                            MaximumNumberOfFiles="1" OnUploadComplete="fileupload_photo_UploadComplete"
                                            OnClientUploadComplete="uploadcomplete" OnClientUploadError="uploaderror" />
                                    </Content>
                                </ajaxToolkit:AccordionPane>
                                <ajaxToolkit:AccordionPane ID="AccordionPane6" runat="server">
                                    <Header>Training Certificate 1</Header>
                                    <Content>
                                        <asp:Image ID="img_trnCert_1" runat="server" />
                                        <ajaxToolkit:AjaxFileUpload ID="fileupload_trnCert_1" runat="server"
                                            AllowedFileTypes="jpg,jpeg,png" CssClass="ajaxfileuploadCss" MaxFileSize="1000"
                                            MaximumNumberOfFiles="1" OnUploadComplete="fileupload_photo_UploadComplete"
                                            OnClientUploadComplete="uploadcomplete" OnClientUploadError="uploaderror" />
                                    </Content>
                                </ajaxToolkit:AccordionPane>
                                <ajaxToolkit:AccordionPane ID="AccordionPane7" runat="server">
                                    <Header>Equipment Test</Header>
                                    <Content>
                                        <asp:Image ID="img_equiptest" runat="server" />
                                        <ajaxToolkit:AjaxFileUpload ID="fileupload_equiptest" runat="server"
                                            AllowedFileTypes="jpg,jpeg,png" CssClass="ajaxfileuploadCss" MaxFileSize="1000"
                                            MaximumNumberOfFiles="1" OnUploadComplete="fileupload_photo_UploadComplete"
                                            OnClientUploadComplete="uploadcomplete" OnClientUploadError="uploaderror" />
                                    </Content>
                                </ajaxToolkit:AccordionPane>
                                <ajaxToolkit:AccordionPane ID="AccordionPane8" runat="server">
                                    <Header>OJT Permit</Header>
                                    <Content>
                                        <asp:Image ID="img_OJTPermit" runat="server" />
                                        <ajaxToolkit:AjaxFileUpload ID="fileupload_OJTPermit" runat="server"
                                            AllowedFileTypes="jpg,jpeg,png" CssClass="ajaxfileuploadCss" MaxFileSize="1000"
                                            MaximumNumberOfFiles="1" OnUploadComplete="fileupload_photo_UploadComplete"
                                            OnClientUploadComplete="uploadcomplete" OnClientUploadError="uploaderror" />
                                    </Content>
                                </ajaxToolkit:AccordionPane>--%>
                                        </Panes>
                                    </ajaxToolkit:Accordion>
                                </asp:View>

                                <asp:View ID="view_files" runat="server">
                                    <div style="width: 100%">
                                        <div style="display: inline-block; float: left; width: 30%">
                                            <asp:Panel ID="pnl_fileops" runat="server" Style="float: right; position: relative; top: 0px; left: 0px;">
                                                <asp:ImageButton ID="icon_newfolder" Style="margin: 5px;" ToolTip="Create Folder Here" runat="server" ImageUrl="~/images/newfolder.png" CssClass="iconimg" OnClick="icon_newfolder_Click" />
                                                <asp:ImageButton ID="icon_newfile" Style="margin: 5px;" ToolTip="Upload New User File Here" runat="server" ImageUrl="~/images/newfile.png" CssClass="iconimg" OnClick="icon_newfile_Click" />
                                            </asp:Panel>

                                            <asp:TreeView ID="tree" runat="server" OnSelectedNodeChanged="tree_SelectedNodeChanged1"
                                                CssClass="treeview_css"
                                                ExpandDepth="FullyExpand"
                                                NodeStyle-CssClass="treeNode"
                                                RootNodeStyle-CssClass="rootNode"
                                                LeafNodeStyle-CssClass="leafNode"
                                                SelectedNodeStyle-CssClass="selectNode"
                                                NodeIndent="16"
                                                PathSeparator="/"
                                                ShowLines="true">
                                            </asp:TreeView>
                                        </div>


                                        <asp:Panel ID="pnl_viewfolder" runat="server" Style="display: inline-block; float: right; width: 70%">
                                           
                                            <asp:Panel ID="pnl_createfolder" runat="server" Visible="false">
                                                <asp:TextBox ID="txt_newfoldername" runat="server"></asp:TextBox>
                                                <asp:Button ID="btn_newFolder" runat="server" Text="Create Folder" OnClick="btn_newFolder_Click"
                                                    Style="width: 100px; height: 30px; background-color: #a52a2a; font-weight: bold; color: white;"/>
                                                <br />
                                            </asp:Panel>


                                            <asp:Panel ID="pnl_createFile" runat="server" Visible="false">
                                                <table class="newfiile_tbl">
                                                    <tr>
                                                        <td>File Type</td>
                                                        <td>
                                                            <asp:Label ID="lbl_issue" runat="server" Visible="false" Text="Date of Issue"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_expir" runat="server" Visible="false" Text="Expiration Date"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_rolespec" runat="server" Visible="false" Text="Visible for"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_filetypes" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddl_filetypes_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="pnl_issuedate" runat="server" Visible="false">

                                                                <asp:TextBox ID="txt_issuedate" runat="server" TextMode="Date"></asp:TextBox>
                                                            </asp:Panel>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="pnl_expires" runat="server" Visible="false">

                                                                <asp:TextBox ID="txt_expires" runat="server" TextMode="Date"></asp:TextBox>
                                                            </asp:Panel>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="pnl_rolespec" runat="server" Visible="false">

                                                                <asp:ListBox ID="list_rolespec" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:FileUpload ID="uploadnewfile" runat="server" accept=".png,.jpg,.jpeg,.gif" />
                                                            <asp:Button runat="server" ID="btn_uploadnewfile" Text="Upload" OnClick="btn_uploadnewfile_Click" />
                                                            <asp:Label ID="cloudfilename" runat="server" Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txt_newfilename" Enabled="false" Visible="false" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:Button ID="btn_createNewFile" runat="server" Visible="false" Text="Create" OnClick="btn_createNewFile_Click"
                                                                Style="width: 80px; height: 30px; background-color: #a52a2a; font-weight: bold; color: white;" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>


                                            <asp:Panel ID="pnl_showcontent" runat="server" CssClass="gridwrapper">
                                                <asp:Label ID="lbl_foldername" runat="server" CssClass="foldername" ></asp:Label>
                                                <asp:GridView ID="grid_userfiles" runat="server" AutoGenerateColumns="false" BorderStyle="None" ShowHeader="false"
                                                    GridLines="Horizontal" AlternatingRowStyle-BackColor="#cccccc" CssClass="grid_userfiles"
                                                    OnRowDataBound="grid_userfiles_RowDataBound" OnRowCommand="grid_userfiles_RowCommand"
                                                    OnRowDeleting="grid_userfiles_RowDeleting">
                                                    <Columns>
                                                         <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btn_go"   runat="server"
                                                                         ImageUrl='<%# go_url( Eval("FILEID").ToString() )%>'
                                                                    Visible='<%# Eval("Issued").ToString()  != "Issued"  %>'
                                                                    CommandName="GO" CssClass="iconimg" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" />
                                                        <asp:BoundField DataField="NAME" /> 
                                                        <asp:BoundField DataField="FILEID" />
                                                        <asp:BoundField DataField="ADDRESS" />
                                                        <asp:BoundField DataField="Issued" />
                                                        <asp:BoundField DataField="Expires" />
                                                        <asp:BoundField DataField="Roles" />
                                                        <asp:BoundField DataField="Type" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btn_delete"   runat="server"
                                                                         ImageUrl="~/images/delete.png"
                                                                    Visible='<%# show_delete( Eval("FILEID").ToString() , Eval("Issued").ToString() )%>'
                                                                    CommandName="DEL" CssClass="iconimg" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </div>
                                </asp:View>

                                <asp:View ID="view_ACC" runat="server">
                                    <ajaxToolkit:Accordion ID="accordion_ACC" runat="server" HeaderCssClass="folder_headerCssClass" ContentCssClass="folder_contentCssClass"
                                        HeaderSelectedCssClass="folder_headerSelectedCss" FadeTransitions="true" TransitionDuration="100"
                                        AutoSize="None" Height="300" Style="height: 100% !important;" RequireOpenedPane="false" SelectedIndex="-1">
                                        <Panes>
                                            <ajaxToolkit:AccordionPane ID="pane_acc_FDO" runat="server">
                                                <Header>FDO & PreOJT
                                                </Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_ACC_fdo" runat="server" CssClass="folder_grids"></asp:GridView>

                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_acc_ASSIST" runat="server">
                                                <Header>Assist Training</Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_ACC_ASSIST" runat="server" CssClass="folder_grids"></asp:GridView>

                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_acc_NR" runat="server">
                                                <Header>NR </Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_ACC_NR" runat="server" CssClass="folder_grids"></asp:GridView>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_acc_SR" runat="server">
                                                <Header>SR</Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_ACC_SR" runat="server" CssClass="folder_grids"></asp:GridView>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_acc_CR" runat="server">
                                                <Header>CR</Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_ACC_CR" runat="server" CssClass="folder_grids"></asp:GridView>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                        </Panes>
                                    </ajaxToolkit:Accordion>
                                </asp:View>


                                <asp:View ID="view_APP" runat="server">
                                    <ajaxToolkit:Accordion ID="accordion_APP" runat="server" HeaderCssClass="folder_headerCssClass" ContentCssClass="folder_contentCssClass"
                                        HeaderSelectedCssClass="folder_headerSelectedCss" FadeTransitions="true" TransitionDuration="100"
                                        AutoSize="None" Height="300" Style="height: 100% !important;" RequireOpenedPane="false" SelectedIndex="-1">
                                        <Panes>
                                            <ajaxToolkit:AccordionPane ID="pane_app_FDO" runat="server">
                                                <Header>FDO & PreOJT
                                                </Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_APP_fdo" runat="server" CssClass="folder_grids"></asp:GridView>

                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_app_ASSIST" runat="server">
                                                <Header>Assist Training</Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_APP_ASSIST" runat="server" CssClass="folder_grids"></asp:GridView>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_app_AR" runat="server">
                                                <Header>AR </Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_APP_AR" runat="server" CssClass="folder_grids"></asp:GridView>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_app_BR" runat="server">
                                                <Header>BR</Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_APP_BR" runat="server" CssClass="folder_grids"></asp:GridView>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_app_KR" runat="server">
                                                <Header>KR</Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_APP_KR" runat="server" CssClass="folder_grids"></asp:GridView>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                        </Panes>
                                    </ajaxToolkit:Accordion>

                                </asp:View>


                                <asp:View ID="view_TOWER" runat="server">
                                    <ajaxToolkit:Accordion ID="accordion_TWR" runat="server" HeaderCssClass="folder_headerCssClass" ContentCssClass="folder_contentCssClass"
                                        HeaderSelectedCssClass="folder_headerSelectedCss" FadeTransitions="true" TransitionDuration="100"
                                        AutoSize="None" Height="300" Style="height: 100% !important;" RequireOpenedPane="false" SelectedIndex="-1">
                                        <Panes>
                                            <ajaxToolkit:AccordionPane ID="pane_TWR_ASSIST" runat="server">
                                                <Header>PreOJT & Assist Training</Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_TWR_ASSIST" runat="server" CssClass="folder_grids"></asp:GridView>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_TWR_GMC" runat="server">
                                                <Header>GMC</Header>
                                                <Content>
                                                    <asp:GridView ID="grid_TWR_GMC" runat="server" ShowHeader="false" CssClass="folder_grids"></asp:GridView>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                            <ajaxToolkit:AccordionPane ID="pane_TWR_ADC" runat="server">
                                                <Header>ADC</Header>
                                                <Content>
                                                    <asp:GridView ShowHeader="false" ID="grid_TWR_ADC" runat="server" CssClass="folder_grids"></asp:GridView>
                                                </Content>
                                            </ajaxToolkit:AccordionPane>
                                        </Panes>
                                    </ajaxToolkit:Accordion>
                                </asp:View>


                            </asp:MultiView>

                        </div>
                    </td>
                </tr>

            </table>

            <asp:Button ID="btn_hidden_refresh" runat="server" Style="display: none;" Text=" sssssssssssssssssssss" OnClick="btn_hidden_refresh_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
