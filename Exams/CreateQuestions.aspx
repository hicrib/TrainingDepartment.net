<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="CreateQuestions.aspx.cs" Inherits="AviaTrain.Exams.CreateQuestions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tbl_create_question {
            width: 1000px;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid #b63838	;
        }

            .tbl_create_question th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: white;
                background-color: #b63838	;
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
            border: 1px solid #b63838	;
            background-color: #b63838	;
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
            font-size : small;
        }
        .grid_questions td{
            padding : 5px;
        }

            .grid_questions th:first-child {
                width: 5%;
                background-color: #d2d5d9;
                color: black;
                text-align : center;
            }

            .grid_questions th:nth-child(2) {
                width: 10% ;
                background-color: #d2d5d9;
                color: black;
                text-align : center;
            }
            .grid_questions th:nth-child(3) {
                width: 50% ;
                background-color: #d2d5d9;
                color: black;
            }

            .grid_questions th:last-child {
                width: 35%;
                background-color: #d2d5d9;
                color: black;
            }

            .grid_questions td:first-child {
                background-color: #edeff7;
                text-align: left !important;
                font-weight: normal !important;
                text-align : center;
            }

            .grid_questions td:nth-child(2) {
  
                background-color: #edeff7;
                text-align: left !important;
                font-weight: normal !important;
                text-align :center;
            }
            .grid_questions td:nth-child(3) {
      
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
            border-collapse : collapse ;
            border : 1px solid #b63838	;
            width : 100%;
        }
    </style>
    <script type="text/javascript">
        function Remove_Info()() {
            alert("IF Question belongs to an exam. It can't be deleted!");
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>


            <table class="tbl_create_question">
                <thead>
                    <tr>
                        <th>CREATE QUESTION</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>

                        <td>
                            <table style="border: 1px solid #b63838	; width: 100%;">
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
                                            <asp:TextBox ID="txt_question_ops" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>A-)</td>
                                        <td>
                                            <asp:TextBox ID="txt_ops_a" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_a" runat="server" AutoPostBack="true"  OnCheckedChanged="chk_a_CheckedChanged1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>B-)</td>
                                        <td>
                                            <asp:TextBox ID="txt_ops_b" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_b" runat="server" AutoPostBack="true" OnCheckedChanged="chk_a_CheckedChanged1"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>C-) </td>
                                        <td>
                                            <asp:TextBox ID="txt_ops_c" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_c" runat="server" AutoPostBack="true" OnCheckedChanged="chk_a_CheckedChanged1"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>D-)</td>
                                        <td>
                                            <asp:TextBox ID="txt_ops_d" CssClass="ops_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_d" runat="server" AutoPostBack="true" OnCheckedChanged="chk_a_CheckedChanged1"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lbl_ops_result" CssClass="errorbox" runat="server" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
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
                                            <asp:TextBox ID="txt_Text1" CssClass="fill_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="blank1" Enabled="false" PlaceHolder="Blank 1" runat="server"></asp:TextBox>
                                            Acceptable Answers :
                                            <asp:TextBox ID="fill_1_ans1" runat="server" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_1_ans2" runat="server" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_1_ans3" runat="server" CssClass="acceptables"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_Text2" CssClass="fill_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="blank2" Enabled="false" PlaceHolder="Blank 2" runat="server"></asp:TextBox>
                                            Acceptable Answers :
                                            <asp:TextBox ID="fill_2_ans1" runat="server" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_2_ans2" runat="server" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_2_ans3" runat="server" CssClass="acceptables"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_Text3" CssClass="fill_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="blank3" Enabled="false" PlaceHolder="Blank 3" runat="server"></asp:TextBox>
                                            Acceptable Answers :
                                            <asp:TextBox ID="fill_3_ans1" runat="server" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_3_ans2" runat="server" CssClass="acceptables"></asp:TextBox>
                                            <asp:TextBox ID="fill_3_ans3" runat="server" CssClass="acceptables"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_Text4" CssClass="fill_textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td> <asp:Label ID="lbl_fill_result" CssClass="errorbox" runat="server" Visible="false"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>    <asp:Button ID="btn_fill_submit" CssClass="ops_submit" runat="server" Text="Submit Question" OnClick="btn_fill_submit_Click" />
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
                            NOTE : Questions belonging to an Exam can not be deleted.
                            <asp:GridView ID="grid_questions" runat="server" CssClass="grid_questions" 
                                OnRowDeleting="grid_questions_RowDeleting"
                                OnRowDataBound="grid_questions_RowDataBound" OnRowCommand="grid_questions_RowCommand1">
                                <Columns>
                                    <asp:ButtonField  ButtonType="Image" ImageUrl="~/images/delete.png" CommandName="Delete" />
                                </Columns>
                            </asp:GridView>

                        </td>
                    </tr>
                </tbody>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>







    <asp:Label ID="lbl_mode" runat="server" Visible="false"></asp:Label>

</asp:Content>
