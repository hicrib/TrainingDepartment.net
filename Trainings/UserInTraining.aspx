<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="UserInTraining.aspx.cs" Inherits="AviaTrain.Trainings.UserInTraining" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tabs {
            position: relative;
            top: 1px;
            z-index: 2;
            width: 500px;
        }

        .tab {
            border: 2px solid #a52a2a;
            background-color: lightgray;
            background-repeat: repeat-x;
            color: White;
            padding: 2px 10px;
            width: 150px;
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
            border: 2px solid #a52a2a;
            padding: 10px;
            width: 975px;
            height: 500px;
            background-color: #e1e1e1;
        }

        .main_tbl {
            width: 1000px;
            border-collapse: collapse;
        }

            .main_tbl th {
                text-align: center;
                background-color: #a52a2a;
                color: white;
                font-size: large;
                font-weight: bold;
            }

        .ops_table {
            width: 100%;
            min-height: 300px;
        }

            .ops_table td {
                border-collapse: collapse;
                height: 40px;
                padding: 5px;
                font-weight: bold;
                font-size: medium;
            }

                .ops_table td:first-child {
                    width: 30px;
                }

                .ops_table td:nth-child(2) {
                    width: 20px;
                }

                .ops_table td:nth-child(3) {
                    width: 20px;
                    font-size: large;
                    font-weight: bold;
                }

                .ops_table td:last-child {
                    width: 500px;
                }


        .chk_ops {
            width: 20px;
        }

        .fill_elements {
            margin-top: 10px;
            font-size: large;
        }

        .btn {
            background-color: #a52a2a;
            color: white;
            font-weight: bold;
        }

        .prev {
            float: left;
            background-color : #b09494 !important;
            color : black !important;
        }

        .show {
            margin: auto;
            background-color : #b09494 !important;
            color : black !important;
        }

        .next {
            float: right;
            background-color : #b09494 !important;
            color : black !important;
        }

        .nav {
            height: 30px;
            width: 120px;
            background-color : #a52a2a !important;
            color : white !important;
            font-size : large;
            font-weight : bold;
        }
    </style>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
            <asp:PostBackTrigger ControlID="Menu1" />
            <%--<asp:AsyncPostBackTrigger ControlID="Menu1" />--%>
        </Triggers>
        <ContentTemplate>

            <table style="border-collapse:collapse; padding:0px;margin:0px;">
                <tr>
                    <td style="width: 1000px;">
                        <table class="main_tbl">
                            <tr>
                                <th>
                                    <asp:Label ID="lbl_trn_name" runat="server"></asp:Label>
                                </th>
                            </tr>
                        </table>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Menu ID="Menu1" Orientation="Horizontal" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selected" CssClass="tabs" runat="server"
                            OnMenuItemClick="Menu1_MenuItemClick">
                            <Items>
                                <asp:MenuItem Text="TRAINING" Value="0" Selected="true"></asp:MenuItem>
                                <asp:MenuItem Text="QUESTIONS" Value="1"></asp:MenuItem>
                            </Items>
                        </asp:Menu>

                        <div class="tabcontents">
                            <asp:MultiView ID="MultiView1" ActiveViewIndex="0" runat="server">
                                <asp:View ID="View1" runat="server">
                                    <div id="div_content" runat="server">
                                    </div>
                                    <div>
                                        <asp:Button ID="btn_trn_EXAM" runat="server" Visible="false" CssClass="nav" Width="300" Text="Start Training Exam" OnClick="btn_trn_EXAM_Click" />
                                    </div>
                                </asp:View>

                                <asp:View ID="View2" runat="server">
                                    <table style="width: 100%; border-collapse: collapse;">
                                        <tr>
                                            <td style="min-height: 400px;">
                                                <asp:Panel ID="pnl_question" runat="server">
                                                    <asp:Panel ID="panel_ops" runat="server">
                                                        <asp:Table ID="ops_table" runat="server" CssClass="ops_table">
                                                            <asp:TableRow ID="row_q" runat="server">
                                                                <asp:TableCell ColumnSpan="4">
                                                                    <asp:Label ID="lbl_ops_question" CssClass="ops_lbl" runat="server" Text="Question Question Question Question Question Question Question Question Question Question Question "></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="row_a" runat="server">
                                                                <asp:TableCell>
                                                                    <asp:Image ID="img_a" runat="server" ImageUrl="" Visible="false" />
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:CheckBox ID="chk_a" runat="server" AutoPostBack="true" CssClass="chk_ops" OnCheckedChanged="chk_a_CheckedChanged" />
                                                                </asp:TableCell>
                                                                <asp:TableCell>A-)</asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lbl_a" runat="server" CssClass="ops_lbl"></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="row_b" runat="server">
                                                                <asp:TableCell>
                                                                    <asp:Image ID="img_b" runat="server" ImageUrl="" Visible="false" />
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:CheckBox ID="chk_b" runat="server" AutoPostBack="true" CssClass="chk_ops" OnCheckedChanged="chk_a_CheckedChanged" />
                                                                </asp:TableCell>
                                                                <asp:TableCell>B-)</asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lbl_b" runat="server" CssClass="ops_lbl"></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="row_c" runat="server">
                                                                <asp:TableCell>
                                                                    <asp:Image ID="img_c" runat="server" ImageUrl="" Visible="false" />
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:CheckBox ID="chk_c" runat="server" AutoPostBack="true" CssClass="chk_ops" OnCheckedChanged="chk_a_CheckedChanged" />
                                                                </asp:TableCell>
                                                                <asp:TableCell>C-)</asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lbl_c" runat="server" CssClass="ops_lbl"></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow ID="row_d" runat="server">
                                                                <asp:TableCell>
                                                                    <asp:Image ID="img_d" runat="server" ImageUrl="" Visible="false" />
                                                                </asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:CheckBox ID="chk_d" runat="server" AutoPostBack="true" CssClass="chk_ops" OnCheckedChanged="chk_a_CheckedChanged" />
                                                                </asp:TableCell>
                                                                <asp:TableCell>D-)</asp:TableCell>
                                                                <asp:TableCell>
                                                                    <asp:Label ID="lbl_d" runat="server" CssClass="ops_lbl"></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </asp:Panel>


                                                    <asp:Panel ID="panel_fill" runat="server">
                                                        <table class="ops_table">
                                                            <tr>
                                                                <td style="max-height: 40px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="font-weight: bold; font-size: medium; padding: 10px !important; text-align: left; vertical-align: top; margin-top: 20px;">
                                                                    <asp:Label CssClass="fill_elements" ID="fill_text1" runat="server" Visible="false"></asp:Label>

                                                                    <asp:TextBox CssClass="fill_elements" ID="fill_fill_1" runat="server" Visible="false"></asp:TextBox>
                                                                    <asp:Image ID="img_fill1" runat="server" Visible="false" />
                                                                    <asp:DropDownList ID="ddl_fill1" runat="server" Visible="false"></asp:DropDownList>

                                                                    <asp:Label CssClass="fill_elements" ID="fill_text2" runat="server" Visible="false"></asp:Label>

                                                                    <asp:TextBox CssClass="fill_elements" ID="fill_fill_2" runat="server" Visible="false"></asp:TextBox>
                                                                    <asp:Image ID="img_fill2" runat="server" Visible="false" />
                                                                    <asp:DropDownList ID="ddl_fill2" runat="server" Visible="false"></asp:DropDownList>

                                                                    <asp:Label CssClass="fill_elements" ID="fill_text3" runat="server" Visible="false"></asp:Label>

                                                                    <asp:TextBox CssClass="fill_elements" ID="fill_fill_3" runat="server" Visible="false"></asp:TextBox>
                                                                    <asp:Image ID="img_fill3" runat="server" Visible="false" />
                                                                    <asp:DropDownList ID="ddl_fill3" runat="server" Visible="false"></asp:DropDownList>

                                                                    <asp:Label CssClass="fill_elements" ID="fill_text4" runat="server" Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:Panel ID="panel_q_btn" runat="server" style="width:60% !important; display:inline-block;">
                                                    <asp:Button ID="btn_prev_q" runat="server" CssClass="btn btn_q prev" Text="Previous Question" OnClick="btn_prev_q_Click" />
                                                    <asp:Button ID="btn_show_answer" runat="server" CssClass="btn btn_q show" Text="Show Answer" OnClick="btn_show_answer_Click" />
                                                    <asp:Button ID="btn_next_q" runat="server" CssClass="btn btn_q next" Text="Next Question" OnClick="btn_next_q_Click" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>
                                    </table>
                                </asp:View>
                            </asp:MultiView>
                        </div>


                    </td>
                    <td>
                        <asp:Button ID="btn_prev_step" runat="server" CssClass="nav"  Text="Back" OnClick="btn_prev_step_Click" />
                        <asp:Button ID="btn_next_step" runat="server" CssClass="nav" Text="Continue" OnClick="btn_next_step_Click" />
                        <asp:Button ID="btn_finish_Training" runat="server" CssClass="nav" Text="Finish Training" Visible="false" OnClick="btn_finish_Training_Click" />
                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:Label ID="lbl_assignid" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_trnid" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_stepid" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="lbl_prevstepid" runat="server" Text="0" Visible="false"></asp:Label>
    <asp:Label ID="lbl_nextstepid" runat="server" Text="0" Visible="false"></asp:Label>

    <asp:Label ID="lbl_q_type" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_current_qid" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lbl_current_orderby" runat="server" Text="1" Visible="false"></asp:Label>
    <asp:Label ID="lbl_current_answers" runat="server" Visible="false"></asp:Label>
</asp:Content>
