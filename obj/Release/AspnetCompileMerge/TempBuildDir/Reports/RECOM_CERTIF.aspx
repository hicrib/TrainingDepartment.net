<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RECOM_CERTIF.aspx.cs" Inherits="AviaTrain.Reports.RECOM_CERTIF" %>

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

        #trainee_sign_date_tbl td {
            min-width: 130px;
            font-size: medium;
            font-weight: bold;
            padding: 5px;
        }

        #review_header_row td {
            font-size: medium;
            font-weight: bold;
            padding: 5px;
            border-collapse: collapse;
            width: 100%;
        }

            #review_header_row td:first-child {
                min-width: 300px;
                border-right: none;
                text-align: center;
                font-size: large !important;
            }

            #review_header_row td:nth-child(2) {
                min-width: 200px;
                text-align: left;
                border-left: none;
            }

            #review_header_row td:nth-child(3) {
                min-width: 50px;
                border-right: none !important;
            }

            #review_header_row td:last-child {
                min-width: 250px !important;
                padding: 5px;
                border-left: none !important;
                border-right: none;
            }

        #team_members_tbl {
            width: 100%;
        }

            #team_members_tbl td {
                font-size: medium;
                font-weight: bold;
                padding: 5px;
                border-collapse: collapse;
            }

                #team_members_tbl td:nth-child(2n+1) {
                    min-width: 150px;
                }

                #team_members_tbl td:nth-child(2n) {
                    min-width: 250px;
                }

        .submit_button {
            float: left;
            background-color: #b63838	;
            font-weight: bold;
            font-size: medium;
            color: white;
            width: 800px;
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
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Always">
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_submit" />
            </Triggers>
            <ContentTemplate>
                <div>

                    <table id="main_lvlass_tbl">
                        <thead>
                            <tr>
                                <th>Recommendation for Certification
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
                                                <asp:Button ID="btn_ojtisign" runat="server" Text="Sign" OnClick="btn_ojtisign_Click" />
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
                                            <td>Minimum Experience Required (MER) met</td>
                                            <asp:Label ID="lbl_MER" runat="server" Text=""></asp:Label>
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
                                    <table id="trainee_sign_date_tbl">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_controller" runat="server" Text="Controller/Trainee Sign"></asp:Label></td>
                                            <td>
                                               
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_controller_sign" runat="server" Text="Signature"></asp:Label></td>
                                            <td>
                                                <asp:Button ID="btn_sign_controller" runat="server" Text="Sign" OnClick="btn_sign_controller_Click" />
                                                <asp:Image ID="img_sign_controller" runat="server" Visible="false" />
                                                <asp:Label ID="lbl_controllersigned" runat="server" Visible="false" Text="0"></asp:Label>

                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_controller_date" runat="server" Text="Date : "></asp:Label></td>
                                            <td>
                                                <asp:DropDownList ID="ddl_DAY_controller" runat="server">
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
                                                <asp:DropDownList ID="ddl_MONTH_controller" runat="server">
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
                                                <asp:DropDownList ID="ddl_YEAR_controller" runat="server">
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
                                                </asp:DropDownList>
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
                                    <table id="review_header_row">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_reviewteam" runat="server" Text="Review Team Approval"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rad_YES" runat="server" Text="YES" GroupName="GRYESNO" />
                                                <asp:RadioButton ID="rad_NO" runat="server" Text="NO" Checked="true" GroupName="GRYESNO" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_review_date" runat="server" Text="Date :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_DAY_review" runat="server">
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
                                                <asp:DropDownList ID="ddl_MONTH_review" runat="server">
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
                                                <asp:DropDownList ID="ddl_YEAR_review" runat="server">
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
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="team_members_tbl">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_member1" runat="server" Text="Team Member :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_member1" runat="server"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Signature :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_member1" runat="server" Text="Sign" OnClick="btn_member1_Click" />
                                                <asp:Image ID="img_member1_sign" runat="server" Visible="false" />
                                                <asp:Label ID="lbl_member1_signed" runat="server" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_member2" runat="server" Text="Team Member :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_member2" runat="server"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Signature :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_member2" runat="server" Text="Sign" OnClick="btn_member2_Click" />
                                                <asp:Image ID="img_member2_sign" runat="server" Visible="false" />
                                                <asp:Label ID="lbl_member2_signed" runat="server" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_member3" runat="server" Text="Team Member :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_member3" runat="server"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="Signature :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_member3" runat="server" Text="Sign" OnClick="btn_member3_Click" />
                                                <asp:Image ID="img_member3_sign" runat="server" Visible="false" />
                                                <asp:Label ID="lbl_member3_signed" runat="server" Visible="false" />
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
                                                <asp:Label ID="lbl_comments" runat="server" Text="Comments:"></asp:Label></td>
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
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_pageresult" CssClass="errorbox" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btn_Submit" Text="SUBMIT" CssClass="submit_button" runat="server" OnClick="btn_Submit_Click" /></td>
                        </tr>
                    </table>


                    <asp:Label ID="lbl_recom_ojti_signed" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lbl_recom_trainee_signed" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lbl_genid" runat="server" Visible="false" Text=""></asp:Label>
                    <asp:Label ID="lbl_viewmode" runat="server" Visible="false" Text=""></asp:Label>
                    <asp:Label ID="lbl_reportnumber" runat="server" Visible="false" Text=""></asp:Label>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

