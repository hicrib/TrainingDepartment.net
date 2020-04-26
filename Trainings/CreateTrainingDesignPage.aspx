<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="CreateTrainingDesignPage.aspx.cs" Inherits="AviaTrain.Trainings.CreateTrainingDesignPage" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Scripts/trumbowyg/trumbowyg.min.css">
    <style>
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 95%;
            border: 3px solid #0DA9D0;
        }

            .modalPopup .header {
                background-color: #2FBDF1;
                height: 95%;
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
                height: 95%;
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
                background-color: indianred;
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
            width: 120px;
        }

        .btn_prev {
            float: left;
        }

        .btn_forw {
            float: right;
        }

        .btn_create {
            margin: auto;
            background-color: indianred !important;
            width: 150px !important;
        }

        .btn_finish {
            background-color: indianred;
            color: white;
            width: 300px !important;
            height: 30px !important;
            text-align: center;
            float: right;
            margin: 10px;
            margin-left: auto;
            margin-right: auto;
            font-size: large !important;
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
        }

        $(document).ready(function () {




        });

        function ShowModalPopup() {
            $find("mpe").show();
            return false;
        }
        function ShowModalPopup2() {
            $find("mpe2").show();
            return false;
        }

    </script>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="page_design_table_uppers">
        <tr>
            <td>
                <div style="float: left; width: 1000px; height: 500px !important;">
                    <asp:TextBox ID="txt_upper_div_holder" runat="server" Style="display: none;" ClientIDMode="Static"></asp:TextBox>
                    <div id="upper_div" style="height: 500px; border: 1px solid indianred;" runat="server"></div>
                </div>
            </td>
            <td>
                <div style="margin-left: auto; margin-right: auto; max-height: 400px; max-width: 400px;">
                    <asp:Image ID="img1" runat="server" Style="display: none; max-height: 400px !important; max-width: 400px !important;" />
                </div>
                <div>
                    <asp:FileUpload ID="file_upload" runat="server" accept=".png,.jpg,.jpeg,.gif" />
                    <br />
                    <asp:Button runat="server" ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" />
                    <br />
                    <asp:Label ID="lbl_step_image1" runat="server" Visible="true"></asp:Label>
                    You can use this address to post images 
                </div>
                <div style="vertical-align:bottom; margin-bottom : 20px;">
                     <asp:Button ID="btn_create_question" CssClass="btn_create btn_belows" runat="server" OnClientClick="return ShowModalPopup()" Style="margin: auto;" Text="Create Question" OnClick="btn_create_question_Click" />
                    <asp:Button ID="btn_chose_question" CssClass="btn_create btn_belows" runat="server" OnClientClick="return ShowModalPopup2()" Style="margin: auto;" Text="Choose Questions" OnClick="btn_chose_question_Click" />
                
                    <asp:Button ID="btn_prev" runat="server" CssClass="btn_belows btn_prev" OnClientClick="TransferDivContent()" Style="float: left;" Text="Previous" OnClick="btn_prev_Click" />
                <asp:Button ID="btn_next" runat="server" CssClass="btn_belows btn_forw" OnClientClick="TransferDivContent()" Style="float: right;" Text="Save & Next" OnClick="btn_next_Click" />
            
                </div>

                <asp:Panel ID="panel_question" runat="server" Visible="true">
                    <table class="add_question_table">
                        <tr>
                            <td>
                                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                                <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                                <cc1:ModalPopupExtender ID="mpShow" BehaviorID="mpe" runat="server" PopupControlID="pnlPopUp" X="10" Y="0"
                                    TargetControlID="lnkFake" BackgroundCssClass="modalBackground" CancelControlID="btnClosePopup">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnlPopUp" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="body">

                                        <asp:ImageButton ID="btnClosePopup" runat="server" Style="float: right" ImageUrl="~/images/cross_red.png" />
                                        <iframe id="iFramePersonal" src="../Exams/CreateQuestions.aspx" style='height: 550px; width: 1050px;'></iframe>
                                    </div>
                                </asp:Panel>


                                <asp:LinkButton ID="lnkFake2" runat="server"></asp:LinkButton>
                                <cc1:ModalPopupExtender ID="mpShow2" BehaviorID="mpe2" runat="server" PopupControlID="pnlPopUp2" X="10" Y="0"
                                    TargetControlID="lnkFake2" BackgroundCssClass="modalBackground" CancelControlID="btnClosePopup2">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnlPopUp2" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="body">

                                        <asp:ImageButton ID="btnClosePopup2" runat="server" Style="float: right" ImageUrl="~/images/cross_red.png" />
                                        <iframe id="iFramePersonal2" src="../Exams/ChooseQuestions_Mini.aspx" style='height: 550px; width: 1050px;'></iframe>
                                    </div>
                                </asp:Panel>

                            </td>
                            <td style="text-align: center; width: 800px;"></td>
                        </tr>
                    </table>
                </asp:Panel>


            </td>
        </tr>
        <tr>
            <td>
                <div style="float: right;">
                </div>
            </td>
        </tr>

    </table>






    <table style="width: 800px;">
        <tr>
            <td>
            </td>
        </tr>
        <tr>
            <td style="align-content: center; text-align: center;"></td>
        </tr>
    </table>
    <asp:Button ID="btn_finish" runat="server" CssClass="btn_belows btn_finish" Text="Finish Designing Training" OnClientClick="TransferDivContent(); return confirmation_LOGOUT();" OnClick="btn_finish_Click" />





    <asp:Label ID="lbl_trn_id" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_step_orderid" runat="server" Visible="false" Text="0"></asp:Label>
    <asp:Label ID="lbl_step_db_id" runat="server" Visible="false"></asp:Label>


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
        $('#ContentPlaceHolder1_upper_div')
            .trumbowyg({
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
                    ['superscript', 'subscript'],
                    ['link'],
                    ['image'], // Our fresh created dropdown
                    ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
                    ['unorderedList', 'orderedList'],
                    ['horizontalRule'],
                    ['removeformat'],
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
    </script>
</asp:Content>
