<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="CreateExam.aspx.cs" Inherits="AviaTrain.Exams.CreateExam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        

        .tbl_exam_info {
            width: 1000px;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid #b63838;
        }

            .tbl_exam_info th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: white;
                background-color: #b63838;
            }

            .tbl_exam_info td {
                padding: 5px;
                min-height: 25px;
            }

        .tbl_create_exam {
            width: 1000px;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid gray;
        }

            .tbl_create_exam th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: black;
                background-color: lightgray;
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

                .grid_all_questions td :nth-child(2) {
                    width: 30px;
                }

        .btn_submitexam {
            width: 100%;
            font-weight: bold;
            border: 1px solid #b63838;
            background-color: #b63838;
            color: white;
            height: 40px;
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

        .grid_chosens {
            background-color: #c3fadd;
            font-size: small;
            width: 100%;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <table class="tbl_exam_info">
                <tr>
                    <th colspan="2">CREATE EXAM
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_name" runat="server" Width="160" Font-Bold="true" Text="Exam Name : "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_examname" runat="server" Width="400" Font-Bold="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_passpercent" runat="server" Width="160" Font-Bold="true" Text="Pass Grade (Percent) :"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_passpercent" runat="server" Width="400" Font-Bold="true" TextMode="Number"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="2" style="align-content: center !important;">
                        <asp:Label ID="Label3" runat="server" Font-Bold="true"
                            Text="Instructions : "></asp:Label>
                        <br />
                        <asp:Label ID="lbl_instructions" runat="server" Font-Bold="false"
                            Text="Choose Questions from below"></asp:Label>
                        <br />
                        <asp:Label ID="Label1" runat="server" Font-Bold="false"
                            Text="Either enter Points(%) for every question  OR  leave all blank for equal points for every question "></asp:Label>
                        <br />
                        <asp:Label ID="Label2" runat="server" Font-Bold="false"
                            Text="Submit The Exam"></asp:Label>
                    </td>
                </tr>

                <%--   <tr>
                    <td colspan="2">
                        <asp:Button ID="btn_check_questions" runat="server" CssClass="btn_submitexam" Text="View Questions" OnClick="btn_check_questions_Click" />
                    </td>
                </tr>--%>

                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grid_chosens" runat="server" CssClass="grid_chosens"
                            OnRowCommand="grid_chosens_RowCommand">
                            <Columns>
                                <asp:ButtonField ButtonType="Image" HeaderText="" CommandName="REMOVE" ImageUrl="~/Images/remove.png" />
                                <asp:TemplateField HeaderText="Points">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_points" Width="30" runat="server" AutoPostBack="true" OnTextChanged="txt_points_TextChanged" TextMode="Number" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lbl_pageresult" runat="server" Visible="false" CssClass="errorbox"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btn_submit_exam" runat="server" CssClass="btn_submitexam" Text="SUBMIT EXAM" OnClick="btn_submit_exam_Click" />
                    </td>
                </tr>


            </table>

            <br />
            <br />
            <br />
            <table class="tbl_create_exam">
                <thead>
                    <tr>
                        <th>Choose Questions for Exam</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="text-align: center; font-weight: bold; font-size: medium; height: 40px;">Question Sector : 
                            <asp:DropDownList ID="ddl_sector" runat="server" Height="30" Width="150" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ddl_sector_SelectedIndexChanged">
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
                    <tr>
                        <td style="height: 10px;"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grid_all_questions" runat="server" CssClass="grid_all_questions"
                                OnRowDataBound="grid_all_questions_RowDataBound" OnRowCommand="grid_all_questions_RowCommand"
                                PageSize="20" OnPageIndexChanged="grid_all_questions_PageIndexChanged" AllowPaging="true"
                                OnPageIndexChanging="grid_all_questions_PageIndexChanging">
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" HeaderText="" CommandName="ADD" ImageUrl="~/Images/create.png" />
                                    <asp:ButtonField ButtonType="Image" HeaderText="" CommandName="REMOVE" ImageUrl="~/Images/remove.png" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>





        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
