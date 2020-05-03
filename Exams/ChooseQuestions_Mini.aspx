<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="ChooseQuestions_Mini.aspx.cs" Inherits="AviaTrain.Exams.ChooseQuestions_Mini" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tbl_create_exam {
            width: 800px;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid gray;
        }

            .tbl_create_exam th {
                text-align: center;
                font-size: medium;
                font-weight: bold;
                color: black;
                background-color: lightgray;
            }

        .small {
            font-size: small !important;
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

        .grid_chosens {
            width: 100%;
        }

            .grid_chosens td {
                font-size: small;
                padding: 5px;
            }

            .grid_chosens th {
                background-color: #a52a2a;
                color: white;
                font-size: medium;
                font-weight: bold;
            }

        .tbl_master_user {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <table class="tbl_create_exam" style="border: 2px solid #a52a2a !important;">
                <thead>
                    <tr>
                        <th style="background-color: #a52a2a !important; font-size: large !important; margin-top: 10px; margin-bottom: 10px !important; color: white !important;">Chosen Questions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <asp:GridView ID="grid_chosens" runat="server" CssClass="grid_chosens"
                                OnRowCommand="grid_chosens_RowCommand">
                                <Columns>
                                    <asp:ButtonField ButtonType="Image" HeaderText="" CommandName="REMOVE" ImageUrl="~/Images/remove.png" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>
            <br />
            <br />
            <br />

            <table class="tbl_create_exam small">
                <thead>
                    <tr>
                        <th>ALL Questions</th>
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
