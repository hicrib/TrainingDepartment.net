<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="EditQuestions.aspx.cs" Inherits="AviaTrain.Exams.EditQuestions" %>

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
                font-size: medium;
                font-weight: bold;
                color: black;
                background-color: lightgray;
            }

            .tbl_create_question td:first-child {
                align-content: center;
                font-weight: bold;
                font-size: medium;
                height: 40px;
            }

        .tbl_ops_question {
            width: 99%;
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

        .grid_all_questions {
            width: 100%;
            border-collapse: collapse;
            font-size: small;
        }

            .grid_all_questions td {
                padding: 5px;
            }

                .grid_all_questions td :first-child {
                    width: 30px;
                }

                .grid_all_questions td :nth-child(4) {
                    text-align : center !important;
                }
                .grid_all_questions td :nth-child(5) {
                    width: 30px;
                    text-align:left !important;
                }

        .fill_textarea {
            width: 98%;
            background-color: lightyellow;
            resize: none;
            height: 30px;
        }

        .tbl_fill {
            border-collapse: collapse;
            border: 1px solid green;
            width: 99%;
        }

        .small_image {
            max-height: 25px;
            max-width: 25px;
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
                                    <th>Question Text
                                    </th>
                                    <th>Sectors
                                    </th>
                                    <th>Creater
                                    </th>
                                    <th>Only Active</th>
                                    <th></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt_qtext" runat="server" Width="200" Font-Bold="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_sector" Width="200" Font-Bold="true" runat="server">
                                            <asp:ListItem Value="ALL" Text="ALL"></asp:ListItem>
                                            <asp:ListItem Value="GEN" Text="General "></asp:ListItem>
                                            <asp:ListItem Value="TWR" Text="Tower General "></asp:ListItem>
                                            <asp:ListItem Value="ACC" Text="ACC General "></asp:ListItem>
                                            <asp:ListItem Value="APP" Text="APP General "></asp:ListItem>
                                            <asp:ListItem Value="ACC-NR"></asp:ListItem>
                                            <asp:ListItem Value="ACC-SR"></asp:ListItem>
                                            <asp:ListItem Value="APP-AR"></asp:ListItem>
                                            <asp:ListItem Value="APP-BR"></asp:ListItem>
                                            <asp:ListItem Value="APP-KR"></asp:ListItem>
                                            <asp:ListItem Value="TWR-GMC"></asp:ListItem>
                                            <asp:ListItem Value="TWR-ADC"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_by" runat="server" Width="200" Font-Bold="true"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_active" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_search" runat="server" Text="Search" style="width:100px; font-weight:bold; height:25px; background-color:indianred; color:white; border:1px solid black;" OnClick="btn_search_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnl_2op" runat="server" Visible="false">

                                <asp:Table ID="tbl_ops" runat="server" class="tbl_ops_question">
                                    <asp:TableRow>
                                        <asp:TableCell ID="row__q" runat="server" colspan="3">
                                            Question : 
                                            <asp:TextBox ID="txt_question_ops" ClientIDMode="Static" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="row__a" runat="server">
                                        <asp:TableCell runat="server" ID="sdf">A-)</asp:TableCell>
                                        <asp:TableCell runat="server" ID="kf">
                                            <asp:TextBox ID="txt_ops_a" ClientIDMode="Static" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell runat="server" ID="TableCell1">
                                            <asp:CheckBox ID="chk_a" runat="server" AutoPostBack="true" OnCheckedChanged="chk_b_CheckedChanged" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="row__b" runat="server">
                                        <asp:TableCell runat="server" ID="TableCell2">B-)</asp:TableCell>
                                        <asp:TableCell runat="server" ID="TableCell3">
                                            <asp:TextBox ID="txt_ops_b" ClientIDMode="Static" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell runat="server" ID="TableCell4">
                                            <asp:CheckBox ID="chk_b" runat="server" AutoPostBack="true" OnCheckedChanged="chk_b_CheckedChanged" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="row__c" runat="server">
                                        <asp:TableCell runat="server" ID="TableCell7">C-)</asp:TableCell>
                                        <asp:TableCell runat="server" ID="TableCell6">
                                            <asp:TextBox ID="txt_ops_c" CssClass="ops_textarea" ClientIDMode="Static" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell runat="server" ID="TableCell8">
                                            <asp:CheckBox ID="chk_c" runat="server" AutoPostBack="true" OnCheckedChanged="chk_b_CheckedChanged" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="row__d" runat="server">
                                        <asp:TableCell runat="server" ID="TableCell9">D-)</asp:TableCell>
                                        <asp:TableCell runat="server" ID="TableCell10">
                                            <asp:TextBox ID="txt_ops_d" CssClass="ops_textarea" ClientIDMode="Static" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell runat="server" ID="TableCell11">
                                            <asp:CheckBox ID="chk_d" runat="server" AutoPostBack="true" OnCheckedChanged="chk_b_CheckedChanged" />
                                        </asp:TableCell>
                                    </asp:TableRow>

                                </asp:Table>

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
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_page_result" runat="server" Visible="false" CssClass="errorbox"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btn_push_question" runat="server" Text="Save"  Visible="false" CssClass="ops_submit" OnClick="btn_push_question_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grid_questions" runat="server" CssClass="grid_all_questions" 
                                OnRowDataBound="grid_questions_RowDataBound" OnRowCommand="grid_questions_RowCommand"
                                PageSize="15" OnPageIndexChanged="grid_questions_PageIndexChanged" AllowPaging="true"
                                OnPageIndexChanging="grid_questions_PageIndexChanging">
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" ControlStyle-CssClass="small_image" ImageUrl="~/images/edit.png" CommandName="EDIT_Q" />
                                    <asp:TemplateField HeaderText="Active">
                                        <ItemTemplate >
                                            <asp:hiddenfield runat="server" id="QID_hiddenfield" Value='<%# Eval("ID") %>' />
                                            <asp:CheckBox ID="chk_q_active" runat="server" HeaderText="ISACTIVE" AutoPostBack="true" 
                                                Checked='<%#Convert.ToBoolean(Eval("ISACTIVE")) %>'  OnCheckedChanged="chk_q_active_CheckedChanged"  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </td>
                    </tr>
                </tbody>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:Label ID="lbl_edit_qid" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_edit_qtype" runat="server" Visible="false"></asp:Label>



    <asp:Label ID="lbl_lastqid" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_mode" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_use_lastadded" runat="server" Text="1" Visible="false"></asp:Label>
</asp:Content>

