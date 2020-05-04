<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="CreateQuestions.aspx.cs" Inherits="AviaTrain.Exams.CreateQuestions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ops_table {
            width: 100%;
            min-height: 300px;
        }

            .ops_table td {
                height: 40px;
                padding: 5px;
                font-weight: bold;
                font-size: medium;
            }

                .ops_table td:first-child {
                    width: 20px;
                }

                .ops_table td:nth-child(2) {
                    width: 20px;
                    font-size: large;
                    font-weight: bold;
                }

                .ops_table td:last-child {
                    width: 500px;
                }

        .modalBackgroundempty {
            background-color: lightgray;
            filter: alpha(opacity=50);
            opacity: 0.2;
        }

        .modalPopupempty {
            background-color: lightgray;
            width: 50%;
            height: 50%;
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
            width: 50%;
            border: 3px solid #a52a2a;
            height: 50%;
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

        .tbl_create_question {
            width: 1000px;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid #a52a2a;
        }

            .tbl_create_question th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: white;
                background-color: #a52a2a;
            }

            .tbl_create_question td:first-child {
                align-content: center;
                text-align: center;
                font-weight: bold;
                font-size: medium;
                height: 40px;
            }

        .tbl_ops_question {
            width: 100%;
            border: 1px solid green;
            border-collapse: collapse;
        }

            .tbl_ops_question td:first-child {
                width: 30px;
                font-size: medium;
                font-weight: bold;
                padding: 5px;
            }

            .tbl_ops_question td:nth-child(2) {
                width: 700px;
                font-size: medium;
                font-weight: bold;
                padding: 5px;
            }

        .ops_textarea {
            width: 100%;
            background-color: lightyellow;
            resize: none;
            height: 50px;
        }

        .ops_submit {
            width: 100%;
            font-weight: bold;
            border: 1px solid #a52a2a;
            background-color: #a52a2a;
            color: white;
            height: 30px;
        }

        .errorbox {
            width: 100%;
            height: 50px;
            background-color: lightgray;
            color: black;
            font-size: x-large;
            border: 2px solid red;
            text-align: center;
            padding: 10px;
        }

        .grid_questions {
            width: 100%;
            background-color: #d2d5d9;
            font-size: small;
        }

            .grid_questions td {
                padding: 5px;
            }

            .grid_questions th:first-child {
                width: 5%;
                background-color: #d2d5d9;
                color: black;
                text-align: center;
            }

            .grid_questions th:nth-child(2) {
                width: 5%;
                background-color: #d2d5d9;
                color: black;
                text-align: center;
            }

            .grid_questions th:nth-child(3) {
                width: 10%;
                background-color: #d2d5d9;
                color: black;
            }

            .grid_questions th:nth-child(4) {
                width: 55%;
                background-color: #d2d5d9;
                color: black;
            }

            .grid_questions th:last-child {
                width: 25%;
                background-color: #d2d5d9;
                color: black;
            }

            .grid_questions td:first-child {
                background-color: #edeff7;
                text-align: left !important;
                font-weight: normal !important;
                text-align: center;
            }

            .grid_questions td:nth-child(2) {
                background-color: #edeff7;
                text-align: left !important;
                font-weight: normal !important;
                text-align: center;
            }

            .grid_questions td:nth-child(3) {
                background-color: #edeff7;
                text-align: left !important;
                font-weight: normal !important;
            }

            .grid_questions td:nth-child(4) {
                background-color: #edeff7;
                text-align: left !important;
                font-weight: normal !important;
            }

            .grid_questions td:last-child {
                background-color: #edeff7;
                text-align: left !important;
                font-weight: normal !important;
            }

        .fill_textarea {
            width: 98%;
            background-color: lightyellow;
            resize: none;
            height: 30px;
        }

        .tbl_fill {
            border-collapse: collapse;
            border: 1px solid #a52a2a;
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        function Remove_Info() {
            alert("IF Question belongs to an exam. It can't be deleted!");
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>


            <table class="tbl_create_question">
                <tbody>
                    <tr>
                        <td>
                            <table style="border: 1px solid #a52a2a; width: 100%;">
                                <tr>
                                    <td style="width: 200px; padding: 5px; font-weight: bold; font-size: medium; text-align: center;">Question Type :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_qtypes" Width="200" Font-Bold="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_qtypes_SelectedIndexChanged">
                                            <asp:ListItem Value="-" Text=" -- "></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2 Options"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3 Options"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="4 Options"></asp:ListItem>
                                            <asp:ListItem Value="FILL" Text="Fill in the blanks"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; padding: 5px; font-weight: bold; font-size: medium; text-align: center;">Sector       :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_sector" Width="200" Font-Bold="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_sector_SelectedIndexChanged">
                                            <asp:ListItem Value="GEN" Text=" General "></asp:ListItem>
                                            <asp:ListItem Value="LATSI"></asp:ListItem>
                                            <asp:ListItem Value="AIP"></asp:ListItem>
                                            <asp:ListItem Value="TWR" Text=" Tower General "></asp:ListItem>
                                            <asp:ListItem Value="ACC" Text=" ACC General "></asp:ListItem>
                                            <asp:ListItem Value="APP" Text=" APP General "></asp:ListItem>
                                            <asp:ListItem Value="ACC-NR"></asp:ListItem>
                                            <asp:ListItem Value="ACC-SR"></asp:ListItem>
                                            <asp:ListItem Value="APP-AR"></asp:ListItem>
                                            <asp:ListItem Value="APP-BR"></asp:ListItem>
                                            <asp:ListItem Value="APP-KR"></asp:ListItem>
                                            <asp:ListItem Value="TWR-GMC"></asp:ListItem>
                                            <asp:ListItem Value="TWR-ADC"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnl_2op" runat="server" Visible="false">

                                <table class="tbl_ops_question">
                                    <tr>
                                        <td colspan="3">Question : 
                                            <asp:TextBox ID="txt_question_ops" ClientIDMode="Static" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>A-)</td>
                                        <td>
                                            <asp:TextBox ID="txt_ops_a" ClientIDMode="Static" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_a" runat="server" AutoPostBack="true" OnCheckedChanged="chk_a_CheckedChanged1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>B-)</td>
                                        <td>
                                            <asp:TextBox ID="txt_ops_b" ClientIDMode="Static" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_b" runat="server" AutoPostBack="true" OnCheckedChanged="chk_a_CheckedChanged1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>C-) </td>
                                        <td>
                                            <asp:TextBox ID="txt_ops_c" CssClass="ops_textarea" ClientIDMode="Static" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_c" runat="server" AutoPostBack="true" OnCheckedChanged="chk_a_CheckedChanged1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>D-)</td>
                                        <td>
                                            <asp:TextBox ID="txt_ops_d" CssClass="ops_textarea" ClientIDMode="Static" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_d" runat="server" AutoPostBack="true" OnCheckedChanged="chk_a_CheckedChanged1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lbl_ops_result" CssClass="errorbox" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button ID="btn_ops_preview" ClientIDMode="Static" CssClass="" runat="server" OnClientClick="return ShowModalPopup()" Style="margin: auto;" Text="PreView Question" />

                                            <asp:Button ID="btn_questionOPS_submit" CssClass="ops_submit" runat="server" Text="Submit Question" OnClick="btn_questionOPS_submit_Click" />
                                        </td>
                                    </tr>
                                </table>

                            </asp:Panel>

                            <asp:Panel ID="pnl_fill" runat="server" Visible="false">
                                <table class="tbl_fill">
                                    <tr>
                                        <td>Question with Blanks </td>
                                    </tr>
                                    <tr>
                                        <td>Instructions : Fill textboxes starting from first one. Fill at least one acceptable answer for a blank</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_Text1" ClientIDMode="Static" CssClass="fill_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="blank1" Enabled="false" PlaceHolder="Blank 1" runat="server"></asp:TextBox>
                                            Acceptable Answers :
                                            <asp:TextBox ID="fill_1_ans1" ClientIDMode="Static" runat="server" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_1_ans2" ClientIDMode="Static" runat="server" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_1_ans3" ClientIDMode="Static" runat="server" CssClass="acceptables"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_Text2" ClientIDMode="Static" CssClass="fill_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="blank2" Enabled="false" PlaceHolder="Blank 2" runat="server"></asp:TextBox>
                                            Acceptable Answers :
                                            <asp:TextBox ID="fill_2_ans1" ClientIDMode="Static" runat="server" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_2_ans2" ClientIDMode="Static" runat="server" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_2_ans3" ClientIDMode="Static" runat="server" CssClass="acceptables"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_Text3" ClientIDMode="Static" CssClass="fill_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="blank3" Enabled="false" PlaceHolder="Blank 3" runat="server"></asp:TextBox>
                                            Acceptable Answers :
                                            <asp:TextBox ID="fill_3_ans1" runat="server" ClientIDMode="Static" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_3_ans2" runat="server" ClientIDMode="Static" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_3_ans3" runat="server" ClientIDMode="Static" CssClass="acceptables"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_Text4" ClientIDMode="Static" CssClass="fill_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_fill_result" CssClass="errorbox" runat="server" Visible="false"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>

                                            <asp:Button ID="btn_fill_preview" ClientIDMode="Static" CssClass="" runat="server" OnClientClick="return ShowModalPopup2()" Style="margin: auto;" Text="Preview Question" />

                                            <asp:Button ID="btn_fill_submit" CssClass="ops_submit" runat="server" Text="Submit Question" OnClick="btn_fill_submit_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>

                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chk_createexam" runat="server" AutoPostBack="true" Text="Create Exam With These Questions" OnCheckedChanged="chk_createexam_CheckedChanged" />
                            <asp:Panel ID="panel_create_exam" Visible="false" Style="text-align: center;" runat="server">
                                <div style="display: inline-block;">
                                    <table style="border: 1px solid #a52a2a;">
                                        <tr>
                                            <td style="width: 100px;">Exam Name</td>
                                            <td>
                                                <asp:TextBox ID="txt_examname" runat="server" Width="200"></asp:TextBox></td>
                                            <td>
                                                <asp:Button ID="btn_createexam" runat="server" Text="Create Exam With These Questions" CssClass="ops_submit" OnClick="btn_createexam_Click" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px;">Pass Percent</td>
                                            <td>
                                                <asp:TextBox ID="txt_passpercent" runat="server" Width="25"></asp:TextBox></td>
                                            <td>
                                                <asp:Label ID="lbl_createexamresult" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3"></td>
                                        </tr>
                                    </table>
                                </div>


                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>NOTE : Questions belonging to an Exam can not be deleted.
                            <asp:GridView ID="grid_questions" runat="server" CssClass="grid_questions"
                                OnRowDeleting="grid_questions_RowDeleting"
                                OnRowDataBound="grid_questions_RowDataBound" OnRowCommand="grid_questions_RowCommand1">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" ImageUrl="~/images/delete.png" CommandName="Delete" />
                                </Columns>
                            </asp:GridView>

                        </td>
                    </tr>
                </tbody>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        function ShowModalPopup() {
            $('#lbl_ops_question').text($('#txt_question_ops').val());
            $('#lbl_a').text($('#txt_ops_a').val());
            $('#lbl_b').text($('#txt_ops_b').val());
            $('#lbl_c').text($('#txt_ops_c').val());
            $('#lbl_d').text($('#txt_ops_d').val());
            $find("mpe").show();
            return false;
        }
        function ShowModalPopup2() {

            $('#fill_text1').text($('#txt_Text1').val());
            $('#fill_text2').text($('#txt_Text2').val());
            $('#fill_text3').text($('#txt_Text3').val());
            $('#fill_text4').text($('#txt_Text4').val());

            $('#fill_fill_1').text($('#fill_1_ans1').val());
            $('#fill_fill_2').text($('#fill_2_ans1').val());
            $('#fill_fill_3').text($('#fill_3_ans1').val());


            if ($('#fill_1_ans1').val() == "")
                $('#fill_fill_1').hide();

            if ($('#fill_1_ans2').val() == "")
                $('#fill_fill_2').hide();

            if ($('#fill_1_ans3').val() == "")
                $('#fill_fill_3').hide();


            $find("mpe2").show();
            return false;
        }
    </script>

    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mpShow" BehaviorID="mpe" runat="server" PopupControlID="pnlPopUp" X="10" Y="0"
        TargetControlID="lnkFake" BackgroundCssClass="modalBackground" CancelControlID="btnClosePopup">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlPopUp" runat="server" CssClass="modalPopup" Style="display: none">
        <div class="body">

            <asp:ImageButton ID="btnClosePopup" runat="server" Style="float: right;" ImageUrl="~/images/cross_red.png" />
            <asp:Panel ID="panel_ops" runat="server">

                <asp:Table ID="ops_table" runat="server" CssClass="ops_table">
                    <asp:TableRow ID="row_q" runat="server">
                        <asp:TableCell ColumnSpan="3">
                            <asp:Label ID="lbl_ops_question" ClientIDMode="Static" CssClass="ops_lbl" runat="server" Text=" "></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>


                    <asp:TableRow ID="row_a" runat="server">
                        <asp:TableCell>
                            <asp:CheckBox ID="CheckBox1" runat="server" CssClass="chk_ops" />
                        </asp:TableCell>
                        <asp:TableCell>A-)</asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lbl_a" runat="server" ClientIDMode="Static" CssClass="ops_lbl"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="row_b" runat="server">
                        <asp:TableCell>
                            <asp:CheckBox ID="CheckBox2" runat="server" CssClass="chk_ops" />
                        </asp:TableCell>
                        <asp:TableCell>B-)</asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lbl_b" runat="server" ClientIDMode="Static" CssClass="ops_lbl"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="row_c" runat="server">
                        <asp:TableCell>
                            <asp:CheckBox ID="CheckBox3" runat="server" CssClass="chk_ops" />
                        </asp:TableCell>
                        <asp:TableCell>C-)</asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lbl_c" runat="server" ClientIDMode="Static" CssClass="ops_lbl"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="row_d" runat="server">
                        <asp:TableCell>
                            <asp:CheckBox ID="CheckBox4" runat="server" ClientIDMode="Static" CssClass="chk_ops" />
                        </asp:TableCell>
                        <asp:TableCell>D-)</asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lbl_d" runat="server" CssClass="ops_lbl"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
        </div>
    </asp:Panel>


    <asp:LinkButton ID="lnkFake2" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mpShow2" BehaviorID="mpe2" runat="server" PopupControlID="pnlPopUp2" X="10" Y="0"
        TargetControlID="lnkFake2" BackgroundCssClass="modalBackground" CancelControlID="btnClosePopup2">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlPopUp2" runat="server" CssClass="modalPopup" Style="display: none">
        <div class="body">

            <asp:ImageButton ID="btnClosePopup2" runat="server" Style="float: right;" ImageUrl="~/images/cross_red.png" />
            <asp:Panel ID="panel_fill" runat="server">
                <table class="ops_table">
                    <tr>
                        <td style="max-height: 40px;"></td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: medium; padding: 10px !important; text-align: left; vertical-align: top; margin-top: 20px;">
                            <asp:Label CssClass="fill_elements" ClientIDMode="Static" ID="fill_text1" runat="server"></asp:Label>

                            <asp:TextBox CssClass="fill_elements" ClientIDMode="Static" ID="fill_fill_1" runat="server"></asp:TextBox>

                            <asp:Label CssClass="fill_elements" ClientIDMode="Static" ID="fill_text2" runat="server"></asp:Label>

                            <asp:TextBox CssClass="fill_elements" ClientIDMode="Static" ID="fill_fill_2" runat="server"></asp:TextBox>

                            <asp:Label CssClass="fill_elements" ClientIDMode="Static" ID="fill_text3" runat="server"></asp:Label>

                            <asp:TextBox CssClass="fill_elements" ClientIDMode="Static" ID="fill_fill_3" runat="server"></asp:TextBox>

                            <asp:Label CssClass="fill_elements" ClientIDMode="Static" ID="fill_text4" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="max-height: 40px;"></td>
                    </tr>
                    <tr>
                        <td style="max-height: 40px;"></td>
                    </tr>
                </table>
            </asp:Panel>

        </div>
    </asp:Panel>




    <asp:Label ID="lbl_lastqid" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_mode" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_use_lastadded" runat="server" Text="1" Visible="false"></asp:Label>
</asp:Content>
