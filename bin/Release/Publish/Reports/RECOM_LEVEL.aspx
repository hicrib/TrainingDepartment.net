<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RECOM_LEVEL.aspx.cs" Inherits="AviaTrain.Reports.RECOM_LEVEL" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        table, tr, td {
            margin: 0px;
            padding: 0px;
            border-collapse: collapse;
        }

        #main_lvlass_tbl {
            margin-top: 30px;
            margin-left: 30px;
            width: 800px;
            border-collapse: collapse;
            border: 3px solid black;
        }

            #main_lvlass_tbl th {
                background-color: #0080ff;
                font-weight: bold;
                font-size: x-large;
                color: white;
                height: 40px;
                padding: 3px;
                border-top: 2px solid black;
                border-bottom: 1px solid black;
            }

            #main_lvlass_tbl td {
                border: 1px solid black;
                height: 30px;
            }

        #name_tbl {
            width: 100%;
        }

            #name_tbl td:first-child {
                font-weight: bold;
                font-size: medium;
                padding: 5px;
                border-right: none;
            }

            #name_tbl td:last-child {
                font-weight: bold;
                font-size: medium;
                padding: 5px;
                border-left: none;
            }

        #ojtis_row {
            width: 100%;
        }

            #ojtis_row td {
                border-collapse: collapse;
                min-width: 200px;
                border-left: 1px solid black;
                padding: 5px;
                font-size: medium;
                font-weight: bold;
            }

        .sectors_tbl {
            width: 100%;
        }

            .sectors_tbl td {
                min-width: 400px;
                padding: 5px;
                font-size: medium;
                font-weight: bold;
            }

        #requirements_tbl {
            width: 100%;
        }

            #requirements_tbl td:first-child {
                min-width: 600px;
                padding: 5px;
                font-size: medium;
                font-weight: bold;
            }

            #requirements_tbl td:last-child {
                min-width: 200px;
                vertical-align: middle;
                text-align: center;
            }

        input[type='checkbox'] {
            height: 20px;
            width: 20px;
        }

        #signatures_tbl {
            width: 100%;
        }

            #signatures_tbl td:first-child {
                min-width: 200px;
                background-color: #0080ff;
                font-weight: bold;
                font-size: medium;
                color: white;
                padding: 5px;
            }

            #signatures_tbl td:last-child {
                padding: 5px;
            }

        .submit_button {
            float: left;
            background-color: #a52a2a	;
            font-weight: bold;
            font-size: medium;
            color: white;
            width: 1100px;
            height: 50px;
            border: 2px solid black;
        }

        .errorbox {
            width: 500px;
            height: 50px;
            background-color: lightgray;
            color: black;
            font-size: x-large;
            border: 2px solid red;
            text-align: center;
            padding: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <table id="main_lvlass_tbl">
                <thead>
                    <tr>
                        <th>Recommendation for Level 
                            <asp:DropDownList ID="ddl_Level" runat="server" Style="font-size: large; background-color: #0080ff; color: white; font-weight: bold; margin-left: 5px; border: none;">
                                <asp:ListItem Value=""></asp:ListItem>
                                <asp:ListItem Value="1"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                                <asp:ListItem Value="3"></asp:ListItem>
                            </asp:DropDownList>
                            Assessment
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <table id="name_tbl">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_name" Text="NAME : " Width="150" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_trainee" runat="server" Width="200"></asp:DropDownList></td>
                                </tr>
                            </table>
                    </tr>
                    <tr>
                        <td>
                            <div style="padding: 10px;">
                                <asp:Label ID="lbl_text" Style="margin: 5px; font-size: medium; font-weight: bold;" runat="server" Text="<br />This certifies that the above-mentioned Controller/Trainee has completed all the requirements listed in the Unit Training Plan for the sector(s) indicated. <br/> <br/> They have demonstrated consistent performance in the application of standarts and procedures. <br/><br/>"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="max-height: 10px; background-color: lightgray;"></td>
                    </tr>
                    <tr>
                        <td>
                            <table id="ojtis_row">
                                <tr>
                                    <td>Recommending OJTI</td>
                                    <td>
                                        <asp:DropDownList ID="ddl_ojtis" runat="server"></asp:DropDownList></td>
                                    <td>Signature
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_ojtisign" runat="server" OnClick="btn_ojtisign_Click" Text="Sign" />
                                        <asp:Image ID="img_ojtisign" Visible="false" runat="server" />
                                        <asp:Label ID="lbl_ojtisigned" runat="server" Visible="false" Text="0"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table class="sectors_tbl">
                                <tr>
                                    <td>Recommended on the following Sector
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_sectors" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total hours on Sector
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_totalhours" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="max-height: 10px; background-color: lightgray;"></td>
                    </tr>
                    <tr>
                        <td>
                            <table id="requirements_tbl">
                                <tr>
                                    <td>Minimum Experience Required (MER) met 
                                        <asp:Label ID="lbl_MER" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_MER" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>All required reading material read and signed off</td>
                                    <td>
                                        <asp:CheckBox ID="chk_reading" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>All set objectives met and signed off</td>
                                    <td>
                                        <asp:CheckBox ID="chk_objectives" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td>The assigned training folder is updated and complete</td>
                                    <td>
                                        <asp:CheckBox ID="chk_folder" runat="server" /></td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                    <tr>
                        <td style="max-height: 10px; background-color: lightgray;"></td>
                    </tr>
                    <tr>
                        <td>
                            <table id="signatures_tbl">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_date" runat="server" Text="Date"> </asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txt_date" runat="server" TextMode="DateTime"></asp:TextBox>
                                        <%--<asp:DropDownList ID="ddl_DAY" runat="server">
                                            <asp:ListItem Value="1"></asp:ListItem>
                                            <asp:ListItem Value="2"></asp:ListItem>
                                            <asp:ListItem Value="3"></asp:ListItem>
                                            <asp:ListItem Value="4"></asp:ListItem>
                                            <asp:ListItem Value="5"></asp:ListItem>
                                            <asp:ListItem Value="6"></asp:ListItem>
                                            <asp:ListItem Value="7"></asp:ListItem>
                                            <asp:ListItem Value="8"></asp:ListItem>
                                            <asp:ListItem Value="9"></asp:ListItem>
                                            <asp:ListItem Value="10"></asp:ListItem>
                                            <asp:ListItem Value="11"></asp:ListItem>
                                            <asp:ListItem Value="12"></asp:ListItem>
                                            <asp:ListItem Value="13"></asp:ListItem>
                                            <asp:ListItem Value="14"></asp:ListItem>
                                            <asp:ListItem Value="15"></asp:ListItem>
                                            <asp:ListItem Value="16"></asp:ListItem>
                                            <asp:ListItem Value="17"></asp:ListItem>
                                            <asp:ListItem Value="18"></asp:ListItem>
                                            <asp:ListItem Value="19"></asp:ListItem>
                                            <asp:ListItem Value="20"></asp:ListItem>
                                            <asp:ListItem Value="21"></asp:ListItem>
                                            <asp:ListItem Value="22"></asp:ListItem>
                                            <asp:ListItem Value="23"></asp:ListItem>
                                            <asp:ListItem Value="24"></asp:ListItem>
                                            <asp:ListItem Value="25"></asp:ListItem>
                                            <asp:ListItem Value="26"></asp:ListItem>
                                            <asp:ListItem Value="27"></asp:ListItem>
                                            <asp:ListItem Value="28"></asp:ListItem>
                                            <asp:ListItem Value="29"></asp:ListItem>
                                            <asp:ListItem Value="30"></asp:ListItem>
                                            <asp:ListItem Value="31"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_MONTH" runat="server">
                                            <asp:ListItem Text="JANUARY" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="FEBRUARY" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="MARCH" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="APRIL" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="MAY" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="JUNE" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="JULY" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="AUGUST" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="SEPTEMBER" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="OCTOBER" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="NOVEMBER" Value="11"></asp:ListItem>
                                            <asp:ListItem Text="DECEMBER" Value="12"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_YEAR" runat="server">
                                            <asp:ListItem Value="2012"></asp:ListItem>
                                            <asp:ListItem Value="2013"></asp:ListItem>
                                            <asp:ListItem Value="2014"></asp:ListItem>
                                            <asp:ListItem Value="2015"></asp:ListItem>
                                            <asp:ListItem Value="2016"></asp:ListItem>
                                            <asp:ListItem Value="2017"></asp:ListItem>
                                            <asp:ListItem Value="2018"></asp:ListItem>
                                            <asp:ListItem Value="2019"></asp:ListItem>
                                            <asp:ListItem Value="2020"></asp:ListItem>
                                            <asp:ListItem Value="2021"></asp:ListItem>
                                            <asp:ListItem Value="2022"></asp:ListItem>
                                            <asp:ListItem Value="2023"></asp:ListItem>
                                            <asp:ListItem Value="2024"></asp:ListItem>
                                            <asp:ListItem Value="2025"></asp:ListItem>
                                            <asp:ListItem Value="2026"></asp:ListItem>
                                            <asp:ListItem Value="2027"></asp:ListItem>
                                            <asp:ListItem Value="2028"></asp:ListItem>
                                            <asp:ListItem Value="2029"></asp:ListItem>
                                        </asp:DropDownList>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_traineeinitials" runat="server" Text="Trainee Initials and Signature"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_traineesign" runat="server" OnClick="btn_traineesign_Click" Text="Sign" />
                                        <asp:Image ID="img_traineesign" Visible="false" runat="server" />
                                        <asp:Label ID="lbl_traineesigned" runat="server" Visible="false" Text="0"></asp:Label>
                                    </td>

                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_trainingdept" runat="server" Text="Training Department Approval - Initials and Signature"> </asp:Label>
                                    </td>
                                    <td>todo : WHO SIGNS HERE?
                                         <asp:Label ID="lbl_departmentsigned" runat="server" Visible="false" Text="0"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="max-height: 10px; background-color: lightgray;"></td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_comments" runat="server" Font-Bold="true" Text="Comments:"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt_comments" Style="width: 99%; resize: none;" TextMode="MultiLine" Rows="30" Height="300" BorderStyle="None" runat="server">
                                        </asp:TextBox></td>
                                </tr>
                            </table>


                        </td>
                    </tr>

                </tbody>
            </table>
            <div style="width: 500px;">
                <asp:Label ID="lbl_pageresult" runat="server" Visible="false" CssClass="errorbox"></asp:Label>
            </div>
            <br />
            <div>
                <asp:Button ID="btn_submit" runat="server" CssClass="submit_button" Text="SUBMIT REPORT" OnClick="btn_submit_Click" />
            </div>
            <asp:Label ID="lbl_genid" runat="server" Visible="false" Text=""></asp:Label>
            <asp:Label ID="lbl_viewmode" runat="server" Visible="false" Text=""></asp:Label>
            <asp:Label ID="lbl_reportnumber" runat="server" Visible="false" Text=""></asp:Label>


        </div>
    </form>
</body>
</html>
