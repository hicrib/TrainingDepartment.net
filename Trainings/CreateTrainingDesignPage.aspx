﻿<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="CreateTrainingDesignPage.aspx.cs" Inherits="AviaTrain.Trainings.CreateTrainingDesignPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Scripts/trumbowyg/trumbowyg.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/Trumbowyg/2.20.0/plugins/table/ui/trumbowyg.table.min.css">
    <style>
        .modalBackgroundempty {
            background-color: lightgray;
            filter: alpha(opacity=50);
            opacity: 0.2;
        }

        .modalPopupempty {
            background-color: lightgray;
            width: 100%;
            height: 100%;
        }

            .modalPopupempty .body {
                min-height: 50px;
                line-height: 30px;
                text-align: center;
                padding: 5px;
                height: 99%;
            }

        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.2;
        }

        .modalPopup {
            background-color: lightgray;
            width: 90%;
            border: 3px solid #a52a2a;
            height: 99%;
        }

            .modalPopup .header {
                background-color: #2FBDF1;
                height: 99%;
                color: White;
                line-height: 30px;
                text-align: center;
                font-weight: bold;
            }

            .modalPopup .body {
                min-height: 50px;
                line-height: 30px;
                text-align: center;
                padding: 5px;
                height: 99%;
            }

            .modalPopup .footer {
                padding: 3px;
            }

            .modalPopup .button {
                height: 23px;
                color: White;
                line-height: 23px;
                text-align: center;
                font-weight: bold;
                cursor: pointer;
                background-color: #a52a2a;
                border: 1px solid black;
            }

            .modalPopup td {
                text-align: left;
            }

        .page_design_table_uppers {
            width: 1200px;
        }

        .btn_belows {
            background-color: gray;
            color: black;
            ;
            border: 1px solid black;
            font-size: medium;
            font-weight: bold;
            height: 23px;
            width: 110px;
        }

        .btn_prev {
            float: left;
        }

        .btn_forw {
            float: right;
        }

        .btn_create {
            margin: auto;
            background-color: #a52a2a !important;
            width: 125px !important;
            font-size: small;
        }

        .btn_finish {
            background-color: #a52a2a;
            color: white;
            width: 250px !important;
            height: 30px !important;
            text-align: center;
            float: right;
            margin: 10px;
            margin-left: auto;
            margin-right: auto;
            font-size: large !important;
        }

        .grid_navigate {
            border-collapse: collapse;
            border: 2px solid black;
            margin-top: 20px;
        }

            .grid_navigate input[type="image" ] {
                max-width: 20px;
            }

            .grid_navigate td:first-child img {
                max-width: 20px;
                max-height: 20px;
            }

        .right_buttons_tbl {
            border-collapse: collapse;
            padding: 5px;
            border: 0px solid #a52a2a;
            height: 480px;
        }

            .right_buttons_tbl td {
                padding-bottom: 0px;
            }
    </style>
    <script type="text/javascript">  
        function validate() {
            var doc = document.getElementById('txt_upper');
            if (doc.value.length == 0) {
                alert('Please Enter data in Richtextbox');
                return false;
            }
        }
        function TransferDivContent() {

            var div = document.getElementById("ContentPlaceHolder1_upper_div").innerHTML;
            document.getElementById("txt_upper_div_holder").value = div;

            ShowEmptyModal();
        }

        $(document).ready(function () {

        });

        function ShowModalPopup() {
            $("#iFramePersonal").attr('src', "../Exams/CreateQuestions.aspx");
            $find("mpe").show();
            return false;
        }
        function ShowModalPopup2() {
            $("#iFramePersonal2").attr('src', "../Exams/ChooseQuestions_Mini.aspx");
            $find("mpe2").show();
            return false;
        }
        function ShowEmptyModal() {
            $find("mpeempty").show();
            return true;
        }
        function ConfirmDelete() {
            if (confirm('Are you sure you want to Delete?')) {
                return true;
            } else {
                return false;
            }
        }
    </script>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div style="display: inline-block; float: left; width: 40px;">
            <table class="page_design_table_uppers">
                <tr style="vertical-align: top;">
                    <td>
                        <div style="float: left; width: 1000px; height: 480px !important;">
                            <div id="upper_div" style="height: 500px; background-color: white; border: 1px solid #a52a2a;" runat="server"></div>
                        </div>
                        <asp:TextBox ID="txt_upper_div_holder" runat="server" Style="display: none;" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td>
                        <table class="right_buttons_tbl">
                            <tr>
                                <td rowspan="10" style="vertical-align: top;">
                                    <div style="overflow-y: scroll; height: 550px;">
                                        <asp:GridView ID="grid_navigate" CssClass="grid_navigate" runat="server" OnRowDataBound="grid_navigate_RowDataBound" OnRowCommand="grid_navigate_RowCommand">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Image" ImageUrl="~/images/view.png" CommandName="GO" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="margin-left: auto; margin-right: auto; max-height: 400px; max-width: 400px;">
                                        <asp:Image ID="img1" runat="server" Style="display: none; max-height: 400px !important; max-width: 400px !important;" />
                                    </div>
                                    <div>
                                        You can use this address to post images 
                                         <br />
                                        <asp:FileUpload ID="file_upload" runat="server" accept=".png,.jpg,.jpeg,.gif" />
                                        <asp:Button runat="server" ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" />
                                        <div style="max-width: 150px;">
                                            <asp:Label ID="lbl_step_image1" runat="server" Visible="true"></asp:Label>
                                        </div>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Button ID="btn_create_question" CssClass="btn_create btn_belows" runat="server" OnClientClick="return ShowModalPopup()" Style="margin: auto;" Text="Create Question" OnClick="btn_create_question_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_chose_question" CssClass="btn_create btn_belows" runat="server" OnClientClick="return ShowModalPopup2()" Style="margin: auto;" Text="Choose Questions" OnClick="btn_chose_question_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_prev" runat="server" CssClass="btn_belows btn_prev" OnClientClick="TransferDivContent()" Style="float: left;" Text="Save & Prev." OnClick="btn_prev_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_next" runat="server" CssClass="btn_belows btn_forw" OnClientClick="TransferDivContent()" Style="float: right; margin-left: 10px !important;" Text="Save & Next" OnClick="btn_next_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="insertunable" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_delete_this_page" runat="server" CssClass="btn_belows" Text="Delete Page" OnClientClick="ConfirmDelete()" OnClick="btn_delete_this_page_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_insertpage_after" runat="server" CssClass="btn_belows btn_forw" Text="Insert After" OnClientClick="TransferDivContent()" OnClick="btn_insertpage_after_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding-bottom: 0px !important;">
                                    <asp:Button ID="btn_finish" runat="server" CssClass="btn_belows btn_finish" Text="Finish Designing Training" OnClientClick="TransferDivContent(); " OnClick="btn_finish_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </div>
        <div style="display: inline-block; float: right;">
        </div>
    </div>



    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mpShow" BehaviorID="mpe" runat="server" PopupControlID="pnlPopUp" X="10" Y="0"
        TargetControlID="lnkFake" BackgroundCssClass="modalBackground" CancelControlID="btnClosePopup">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlPopUp" runat="server" CssClass="modalPopup" Style="display: none">
        <div class="body">

            <asp:ImageButton ID="btnClosePopup" runat="server" Style="float: right;" ImageUrl="~/images/cross_red.png" />
            <iframe id="iFramePersonal" src="about:blank" style='height: 99%; width: 95%;'></iframe>
        </div>
    </asp:Panel>


    <asp:LinkButton ID="lnkFake2" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mpShow2" BehaviorID="mpe2" runat="server" PopupControlID="pnlPopUp2" X="10" Y="0"
        TargetControlID="lnkFake2" BackgroundCssClass="modalBackground" CancelControlID="btnClosePopup2">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlPopUp2" runat="server" CssClass="modalPopup" Style="display: none">
        <div class="body">

            <asp:ImageButton ID="btnClosePopup2" runat="server" Style="float: right;" ImageUrl="~/images/cross_red.png" />
            <iframe id="iFramePersonal2" src="about:blank" style='height: 99%; width: 95%;'></iframe>
        </div>
    </asp:Panel>


    <asp:LinkButton ID="lnkFakeempty" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mpshowempty" BehaviorID="mpeempty" runat="server" PopupControlID="pnlPopUpempty" X="0" Y="0"
        TargetControlID="lnkFakeempty" BackgroundCssClass="modalBackgroundempty" CancelControlID="btnClosePopupempty">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlPopUpempty" runat="server" CssClass="modalPopupempty" Style="display: none">
        <div class="body">
            <asp:ImageButton ID="btnClosePopupempty" runat="server" Style="display: block; margin: auto; margin: auto; width: 10%; vertical-align: middle;" ImageUrl="~/images/wait.gif" />
        </div>
    </asp:Panel>








    <asp:Label ID="lbl_trn_id" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_step_orderid" runat="server" Visible="false" Text="0"></asp:Label>
    <asp:Label ID="lbl_step_db_id" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_trnname" runat="server" Visible="false"></asp:Label>

    <asp:Label ID="lbl_editable" ClientIDMode="Static" runat="server" Text="1" Style="display: none;"></asp:Label>



    <div id="___editor"></div>


    <!-- Import jQuery -->
    <script src="//ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="js/vendor/jquery-3.3.1.min.js"><\/script>')</script>

    <!-- Import Trumbowyg -->
    <script src="../Scripts/trumbowyg/trumbowyg.min.js"></script>

    <!-- Import Trumbowyg plugins... -->
    <script src="../Scripts/trumbowyg/plugins/upload/trumbowyg.upload.js"></script>
    <script src="../Scripts/trumbowyg/plugins/colors/trumbowyg.colors.js"></script>
    <script src="../Scripts/trumbowyg/plugins/fontsize/trumbowyg.fontsize.js"></script>
    <script src="../Scripts/trumbowyg/plugins/fontfamily/trumbowyg.fontfamily.js"></script>
    <script src="../Scripts/trumbowyg/plugins/history/trumbowyg.history.js"></script>
    <script src="../Scripts/trumbowyg/plugins/noembed/trumbowyg.noembed.js"></script>
    <script src="../Scripts/trumbowyg/plugins/pasteimage/trumbowyg.pasteimage.js"></script>
    <script src="../Scripts/trumbowyg/plugins/table/trumbowyg.table.js"></script>

    <script src="../Scripts/trumbowyg/plugins/resizimg/trumbowyg.resizimg.js"></script>


    <script>
        //$.trumbowyg.svgPath = '~\Scripts\trumbowyg\ui\icons.svg';
        if ($('#lbl_editable').text() == "0") {
           
        }
        else {
            $('#ContentPlaceHolder1_upper_div')
                .trumbowyg({
                    svgPath: '/Scripts/trumbowyg/ui/icons.svg',// or a path like '/assets/my-custom-path/icons.svg',
                    btnsDef: {
                        // Create a new dropdown
                        image: {
                            dropdown: ['insertImage', 'noembed'],
                            ico: 'insertImage'
                        }
                    },
                    // Redefine the button pane
                    btns: [
                        ['viewHTML'],
                        ['historyUndo', 'historyRedo'],
                        ['fontfamily'],
                        ['foreColor', 'backColor'],
                        ['table'],
                        ['formatting'],
                        ['fontsize'],
                        ['strong', 'em', 'del'],
                        ['link'],
                        ['image'], // Our fresh created dropdown
                        ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
                        ['unorderedList', 'orderedList'],
                        ['horizontalRule'],
                        ['fullscreen']
                    ],
                    plugins: {
                        // Add imagur parameters to upload plugin for demo purposes
                        upload: {
                            serverPath: '~/images/',
                            fileFieldName: 'image',
                            headers: {
                                'Authorization': 'Client-ID xxxxxxxxxxxx'
                            },
                            urlPropertyName: 'data.link'
                        }
                    }
                });
        }
    </script>
</asp:Content>
