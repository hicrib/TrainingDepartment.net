<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="CreateTrainingFinish.aspx.cs" Inherits="AviaTrain.Trainings.CreateTrainingFinish" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
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

        .trn_info_tbl {
            border-collapse: collapse;
            width: 1000px;
        }
        .trn_info_tbl th {
            width: 100%;
            font-size : large;
            font-weight:bold;
            background-color : #a52a2a;
            color : white;
        }
         .trn_info_tbl td {
            height : 20px;
            margin : 5px;
        }
        .btn_exams {
            width: 200px;
            background-color: #a52a2a;
            color: white;
        }

        .exam_creation_tbl{
            width : 600px;
            display:inline-block;
            border : 1px solid #a52a2a;
        }
        .exam_creation_panel {
            width : 1000px;
            text-align : center;
        }
        .finish {
            display : inline:block;
            width : 100%;
        }
        .btn_finish {
            height : 30px;
            margin : 10px;
            width : 80%;
            background-color : #a52a2a;
            color : white;
            font-weight : bold;
            font-size : large;
        }
        .pageresult {
            background-color : lightgray;
            color : #a52a2a;
            border : 1px solid black;
            font-size : large;
            font-weight : bold;
        }
    </style>
    <script type="text/javascript">
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

        function confirmation_FINISHTRAINING() {
            if (confirm('Are you sure you want to Finish Designing Training?')) {
                return true;
            } else {
                return false;
            }
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="trn_info_tbl">
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:CheckBox ID="chk_addexam" runat="server" Text="Add Exam" AutoPostBack="true" OnCheckedChanged="chk_addexam_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="panel_add_exam" runat="server" CssClass="exam_creation_panel" Visible="false">
                    <table class="exam_creation_tbl">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddl_exams" runat="server"></asp:DropDownList></td>
                            <td>
                                <asp:Button ID="btn_update_exams" runat="server" Text="Update Exam List" OnClick="btn_update_exams_Click" /></td>
                            <td></td>
                            <td>
                                <asp:Button ID="btn_createexam" runat="server" CssClass="btn_exams" Text="Create Exam" OnClientClick="return ShowModalPopup2()"  ToolTip="Uses questions in the system" />
                                <br />
                                <asp:Button ID="btn_createquestions" runat="server" CssClass="btn_exams" Text="Create Questions/Exam" OnClientClick="return ShowModalPopup()" ToolTip="Creates Exam with new questions" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4"></td>
                        </tr>

                    </table>

                </asp:Panel>
            </td>
        </tr>
    </table>
    <div style="text-align:center; width:1000px;">
    <table class="finish">
        <tr>
            <td colspan="4">
                <asp:Label ID="lbl_pageresult" runat="server" Visible="false" CssClass="pageresult"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Button ID="btn_finish_training" runat="server" CssClass="btn_finish" Text="Finish Training" OnClientClick="return confirmation_FINISHTRAINING();" OnClick="btn_finish_training_Click" />
            </td>
        </tr>
    </table>
        </div>


    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mpShow" BehaviorID="mpe" runat="server" PopupControlID="pnlPopUp" X="10" Y="0"
        TargetControlID="lnkFake" BackgroundCssClass="modalBackground" CancelControlID="btnClosePopup">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlPopUp" runat="server" CssClass="modalPopup" Style="display: none">
        <div class="body">

            <asp:ImageButton ID="btnClosePopup" runat="server" Style="float: right;" ImageUrl="~/images/cross_red.png" />
            <iframe id="iFramePersonal" src="../Exams/CreateQuestions.aspx?NoLast=1" style='height: 99%; width: 95%;'></iframe>
        </div>
    </asp:Panel>


    <asp:LinkButton ID="lnkFake2" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mpShow2" BehaviorID="mpe2" runat="server" PopupControlID="pnlPopUp2" X="10" Y="0"
        TargetControlID="lnkFake2" BackgroundCssClass="modalBackground" CancelControlID="btnClosePopup2">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlPopUp2" runat="server" CssClass="modalPopup" Style="display: none">
        <div class="body">

            <asp:ImageButton ID="btnClosePopup2" runat="server" Style="float: right;" ImageUrl="~/images/cross_red.png" />
            <iframe id="iFramePersonal2" src="../Exams/CreateExam.aspx" style='height: 99%; width: 95%;'></iframe>
        </div>
    </asp:Panel>


    <asp:Label ID="lbl_trnid" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_prev_stepid" runat="server" Visible="false"></asp:Label>
</asp:Content>
