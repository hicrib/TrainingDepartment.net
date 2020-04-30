<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="UserInExam.aspx.cs" Inherits="AviaTrain.Exams.UserInExam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function confirmation_FINISHEXAM() {
           if (confirm('Are you sure you want to FINISH THE EXAM ?')) {
           return true;
           }else{
           return false;
           }
       }
   </script>
    <style>
        .main_tbl {
            border-collapse: collapse;
             
            width: 1000px;
        }

            .main_tbl th {
                border-collapse: collapse;
                background-color : #b63838	;
                color:white;
                font-size : large;
                font-weight : bold;
                text-align : center;
            }

            .main_tbl td {
                border-collapse: collapse;
            }

                .main_tbl td:first-child {
                    
                }

                .main_tbl td:last-child {
                    margin-left : 30px;
                }

        .grid_questions_map {
            text-align: center;
        }.grid_questions_map th {
            text-align: center;
            padding : 5px;
        }
         .grid_questions_map td {
            text-align: center;
            padding : 5px;
        }

        .ops_table {
            width: 100%;
            min-height: 300px;
        }

            .ops_table td {
                height: 40px;
                padding: 5px;
                font-weight : bold;
                font-size : medium;
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


        .chk_ops {
            width: 20px;
        }

        .fill_elements {
            margin-top: 10px;
            font-size: large;
        }

        .btn_important {
            width: 100%;
            background-color: #b63838	;
            color: white;
            font-size: medium;
            font-weight: bold;
            border: 1px solid black;
        }

        .right {
            width: 30% !important; 
            float: right !important;
        }
    </style>


</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>

            <table class="main_tbl">
                <tr>
                    <th colspan="2">
                        <asp:Label ID="lbl_examname" runat="server"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td style="width: 200px; align-content: center; text-align: center; vertical-align: top;">
                        <asp:GridView ID="grid_questions_map" runat="server" CssClass="grid_questions_map" OnRowDataBound="grid_questions_map_RowDataBound"
                            OnRowCommand="grid_questions_map_RowCommand">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" HeaderText="" ItemStyle-Width="30" ItemStyle-Height="30" CommandName="CHOOSE" ImageUrl="~/Images/view.png" />
                            </Columns>
                        </asp:GridView>
                    </td>

                    <td style="width: 800px; vertical-align: top;">
                        <asp:Panel ID="panel_ops" runat="server" Visible="false">

                            <asp:Table ID="ops_table" runat="server" CssClass="ops_table">
                                <asp:TableRow ID="row_q" runat="server">
                                    <asp:TableCell ColumnSpan="3">
                                        <asp:Label ID="lbl_ops_question" CssClass="ops_lbl" runat="server" Text="Question Question Question Question Question Question Question Question Question Question Question "></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                

                                <asp:TableRow ID="row_a" runat="server">
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
                                        <asp:CheckBox ID="chk_b" runat="server" AutoPostBack="true" CssClass="chk_ops" OnCheckedChanged="chk_a_CheckedChanged" />
                                    </asp:TableCell>
                                    <asp:TableCell>B-)</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lbl_b" runat="server" CssClass="ops_lbl"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="row_c" runat="server">
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
                                        <asp:CheckBox ID="chk_d" runat="server" AutoPostBack="true" CssClass="chk_ops" OnCheckedChanged="chk_a_CheckedChanged" />
                                    </asp:TableCell>
                                    <asp:TableCell>D-)</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="lbl_d" runat="server" CssClass="ops_lbl"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>




                        <asp:Panel ID="panel_fill" runat="server" Visible="false">
                            <table class="ops_table">
                                <tr>
                                    <td style="max-height: 40px;"></td>
                                </tr>
                                <tr>
                                    <td style="font-weight: bold; font-size: medium; padding: 10px !important; text-align: left; vertical-align: top; margin-top: 20px;">
                                        <asp:Label CssClass="fill_elements" ID="fill_text1" runat="server" Visible="false"></asp:Label>

                                        <asp:TextBox CssClass="fill_elements" ID="fill_fill_1" runat="server" Visible="false"></asp:TextBox>

                                        <asp:Label CssClass="fill_elements" ID="fill_text2" runat="server" Visible="false"></asp:Label>

                                        <asp:TextBox CssClass="fill_elements" ID="fill_fill_2" runat="server" Visible="false"></asp:TextBox>

                                        <asp:Label CssClass="fill_elements" ID="fill_text3" runat="server" Visible="false"></asp:Label>

                                        <asp:TextBox CssClass="fill_elements" ID="fill_fill_3" runat="server" Visible="false"></asp:TextBox>

                                        <asp:Label CssClass="fill_elements" ID="fill_text4" runat="server" Visible="false"></asp:Label>
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

                        <asp:Button ID="btn_save_n_next" runat="server" CssClass="btn_important right" Text="Save Answer" OnClick="btn_save_n_next_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btn_finish_exam" runat="server" OnClientClick="return confirmation_FINISHEXAM();" CssClass="btn_important" Text="Finish Exam" OnClick="btn_finish_exam_Click" /></td>
                    <td>
                        </td>
                </tr>
            </table>


            <asp:Label ID="lbl_assignid" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lbl_examid" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lbl_total_questions" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lbl_q_type" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lbl_current_orderby" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lbl_current_qid" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lbl_current_answers" runat="server" Visible="false"></asp:Label>



            <asp:Label ID="lbl_trn_assignid" runat="server"  Visible="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>
