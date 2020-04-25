<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DAILYTR_ASS_TWR.aspx.cs" Inherits="AviaTrain.Reports.DAILYTR_ASS_TWR" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" runat="server" media="screen" href="~/css/TR.css" />
</head>
<body>
    <div id="pagediv" style="margin-left: 20px; margin-top: 30px;">


        <form id="form1" runat="server">
            <div>
            </div>
            <div style="float: left;">
                <div id="reportinfo_div">
                    <table class="main_table">
                        <tr style="height: 50px; border-bottom: 1px solid black;">
                            <td style="background-color: #0080ff; font-size: larger; padding-left: 10px; font-weight: bold; color: white; border: 1px solid black;">
                                <div>Daily Training Report - Assist Tower </div>
                            </td>
                            <td style="background-color: white; font-weight: bold; padding-left: 5px; border: 1px solid black;">
                                <div>
                                    Report Number : 
                           <asp:Label ID="lbl_reportnumber" runat="server" Font-Size="Larger" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <!-- second row -->
                        <tr style="height: 100px; border-bottom: 1px solid black;">
                            <td colspan="2" style="">

                                <table style="border-collapse: collapse;">
                                    <tr>
                                        <td style="border-collapse: collapse; border-right: 1px solid black; width: 50%;">
                                            <div>
                                                <table>
                                                    <!-- initials signatures -->
                                                    <tr>
                                                        <td style="width: 250px; height: 100px; text-align: center; vertical-align: center;">Trainee Initials/Signature
                                                    <br />
                                                            <asp:DropDownList ID="ddl_trainees" runat="server" Style="margin-top: 5px;"></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 250px; height: 100px; text-align: center; vertical-align: center;">OJTI Initials/Signature
                                                    <br />
                                                            <asp:DropDownList ID="ddl_ojtis" runat="server" Style="margin-top: 5px;"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                        <td style="width: 50%;">
                                            <div>
                                                <!-- checkboxes -->
                                                <table style="width: 550px; height: 100px;">
                                                    <tr>
                                                        <td style="width=50%;">
                                                            <asp:CheckBox ID="chk_OJT" Text="OJT" runat="server" />
                                                        </td>
                                                        <td style="">
                                                            <asp:CheckBox ID="chk_Ass" Text="Assessment" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <!-- third row -->
                        <tr style="border-bottom: 1px solid black;">
                            <td colspan="2">
                                <div>
                                    <table style="width: 100% !important;">
                                        <tr>
                                            <td>Date : 
                                       <asp:DropDownList ID="ddl_DAY" runat="server">
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
                                                </asp:DropDownList>
                                            </td>
                                            <td>Position :
                                        <asp:DropDownList ID="ddl_positions" DataSourceID="" runat="server">
                                        </asp:DropDownList>
                                            </td>
                                            <td>Time On :
                                        <asp:TextBox ID="txt_timeon" TextMode="Time" runat="server"></asp:TextBox>
                                            </td>
                                            <td>Time Off :
                                        <asp:TextBox ID="txt_timeoff" TextMode="Time" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <!-- third row -->
                        <tr>
                            <td colspan="2">
                                <div>
                                    <table style="width: 100% !important;">
                                        <tr>
                                            <td colspan="1">
                                                <table>
                                                    <tr>
                                                        <td style="width: 150px;">Traffic Density   :</td>
                                                        <td>
                                                            <div>
                                                                <asp:RadioButtonList ID="radio_density" RepeatDirection="Horizontal" runat="server">
                                                                    <asp:ListItem Text="Light" Value="L" />
                                                                    <asp:ListItem Text="Mod" Value="M" />
                                                                    <asp:ListItem Text="Heavy" Value="H" />
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 150px;">Complexity     : </td>
                                                        <td>
                                                            <div>
                                                                <asp:RadioButtonList ID="radio_complexity" RepeatDirection="Horizontal" runat="server">
                                                                    <asp:ListItem Text="Low" Value="L" />
                                                                    <asp:ListItem Text="Mod" Value="M" />
                                                                    <asp:ListItem Text="High" Value="H" />
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>Hours :
                                        <asp:TextBox ID="txt_hours" TextMode="Time" runat="server"></asp:TextBox>
                                            </td>
                                            <td>Total Hours :
                                        <asp:TextBox ID="txt_totalhours" TextMode="Time" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- EVAULUATIONS -->
                <div id="evaluation_div">
                    <br />
                    <asp:ScriptManager ID="ScriptManager1" runat="server" />
                    <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Always">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="UploadButton" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Panel ID="evaluation_panel" runat="server">

                                <table id="evaluation_main_table">
                                    <thead>
                                        <tr>
                                            <th class="na">N/A</th>
                                            <th style="border-left: 2px solid black !important;">Objectives</th>
                                            <th class="th_skills">S</th>
                                            <th class="th_skills">N/I</th>

                                            <th style="border-left: 2px solid black !important; width: 600px;">Comments</th>


                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="skill_categ">
                                            <th></th>
                                            <th>1. Flight Plans</th>
                                            <th class="th_skills"></th>
                                            <th class="th_skills"></th>
                                            <th style="border: none !important;">
                                                <asp:RadioButtonList ID="rad_prebrief_comment" runat="server" Width="300" AutoPostBack="true" OnSelectedIndexChanged="rad_prebrief_comment_SelectedIndexChanged" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="File" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="Text"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd1a_na" Checked="true" runat="server" GroupName="gr1A" /></td>
                                            <td>A. Creates flight plans quickly</td>
                                            <td>
                                                <asp:RadioButton ID="rd1as" runat="server" GroupName="gr1A" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd1ani" runat="server" GroupName="gr1A" /></td>
                                            <td rowspan="0" style="vertical-align: top; text-align: left;">
                                                <asp:FileUpload ID="file_prebrief_comment" runat="server" accept=".png,.jpg,.jpeg,.gif" />
                                                <asp:Button runat="server" ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" />
                                                <asp:Label ID="uploadedfilename" runat="server" Visible="false"></asp:Label>
                                                <br />
                                                <asp:Label ID="statuslabel" runat="server" Font-Size="Large" ForeColor="Red"></asp:Label>
                                                <asp:TextBox ID="txt_prebrief_comment" TextMode="MultiLine" PlaceHolder="Please focus on areas that need improvement, rather than on things that were done well. If the OJTI does not make a comment we will assume the objective was carried out well." CssClass="CommentBoxes" Visible="false" runat="server"></asp:TextBox>
                                                <asp:Image ID="img_file" Visible="false" Style="width: 100%;" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd1b_na" Checked="true" runat="server" GroupName="gr1B" /></td>
                                            <td>B. Enters correct routings and details</td>
                                            <td>
                                                <asp:RadioButton ID="rd1bs" runat="server" GroupName="gr1B" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd1bni" runat="server" GroupName="gr1B" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd1c_na" Checked="true" runat="server" GroupName="gr1C" /></td>
                                            <td>C. Resolves FPL errors such as duplicate codes etc.</td>
                                            <td>
                                                <asp:RadioButton ID="rd1cs" runat="server" GroupName="gr1C" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd1cni" runat="server" GroupName="gr1C" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd1d_na" Checked="true" runat="server" GroupName="gr1D" /></td>
                                            <td>D. Resolves FPL jurisdiction issues</td>
                                            <td>
                                                <asp:RadioButton ID="rd1ds" runat="server" GroupName="gr1D" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd1dni" runat="server" GroupName="gr1D" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd1e_na" Checked="true" runat="server" GroupName="gr1E" /></td>
                                            <td>E. Resolves ‘print strip’ issues </td>
                                            <td>
                                                <asp:RadioButton ID="rd1es" runat="server" GroupName="gr1E" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd1eni" runat="server" GroupName="gr1E" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd1f_na" Checked="true" runat="server" GroupName="gr1F" /></td>
                                            <td>F. Overall competence with flight plans   </td>
                                            <td>
                                                <asp:RadioButton ID="rd1fs" runat="server" GroupName="gr1F" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd1fni" runat="server" GroupName="gr1F" /></td>
                                        </tr>
                                        <tr class="skill_categ">
                                            <th class="na"></th>
                                            <th>2. Equipment </th>
                                            <th class="th_skills"></th>
                                            <th class="th_skills"></th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd2a_na" Checked="true" runat="server" GroupName="gr2A" /></td>
                                            <td>A. Able to select and deselect frequencies</td>
                                            <td>
                                                <asp:RadioButton ID="rd2as" runat="server" GroupName="gr2A" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd2ani" runat="server" GroupName="gr2A" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd2b_na" Checked="true" runat="server" GroupName="gr2B" /></td>
                                            <td>B. Monitors correct frequencies</td>
                                            <td>
                                                <asp:RadioButton ID="rd2bs" runat="server" GroupName="gr2B" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd2bni" runat="server" GroupName="gr2B" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd2c_na" Checked="true" runat="server" GroupName="gr2C" /></td>
                                            <td>C. Able to operate the telephones correctly</td>
                                            <td>
                                                <asp:RadioButton ID="rd2cs" runat="server" GroupName="gr2C" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd2cni" runat="server" GroupName="gr2C" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd2d_na" Checked="true" runat="server" GroupName="gr2D" /></td>
                                            <td>D. Change strip printer roll</td>
                                            <td>
                                                <asp:RadioButton ID="rd2ds" runat="server" GroupName="gr2D" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd2dni" runat="server" GroupName="gr2D" /></td>
                                        </tr>
                                       
                                        <tr class="skill_categ">
                                            <th class="na"></th>
                                            <th>3. Weather  </th>
                                            <th class="th_skills"></th>
                                            <th class="th_skills"></th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd3a_na" Checked="true" runat="server" GroupName="gr3A" /></td>
                                            <td>A. Records ATIS  </td>
                                            <td>
                                                <asp:RadioButton ID="rd3as" runat="server" GroupName="gr3A" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd3ani" runat="server" GroupName="gr3A" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd3b_na" Checked="true" runat="server" GroupName="gr3B" /></td>
                                            <td>B. ATIS updated on time  </td>
                                            <td>
                                                <asp:RadioButton ID="rd3bs" runat="server" GroupName="gr3B" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd3bni" runat="server" GroupName="gr3B" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd3c_na" Checked="true" runat="server" GroupName="gr3C" /></td>
                                            <td>C. Understands the AWOS system </td>
                                            <td>
                                                <asp:RadioButton ID="rd3cs" runat="server" GroupName="gr3C" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd3cni" runat="server" GroupName="gr3C" /></td>
                                        </tr>
                                       
                                        <tr class="skill_categ">
                                            <th class="na"></th>
                                            <th>4. Coordination </th>
                                            <th class="th_skills"></th>
                                            <th class="th_skills"></th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd4a_na" Checked="true" runat="server" GroupName="gr4A" /></td>
                                            <td>A. Identifies themselves on first contact </td>
                                            <td>
                                                <asp:RadioButton ID="rd4as" runat="server" GroupName="gr4A" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd4ani" runat="server" GroupName="gr4A" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd4b_na" Checked="true" runat="server" GroupName="gr4B" /></td>
                                            <td>B. Clear and accurate coordination with Tehran </td>
                                            <td>
                                                <asp:RadioButton ID="rd4bs" runat="server" GroupName="gr4B" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd4bni" runat="server" GroupName="gr4B" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd4c_na" Checked="true" runat="server" GroupName="gr4C" /></td>
                                            <td>C. Takeover/handover position </td>
                                            <td>
                                                <asp:RadioButton ID="rd4cs" runat="server" GroupName="gr4C" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd4cni" runat="server" GroupName="gr4C" /></td>
                                        </tr>
                                       
                                        <tr class="skill_categ">
                                            <th class="na"></th>
                                            <th>5. General </th>
                                            <th class="th_skills"></th>
                                            <th class="th_skills"></th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd5a_na" Checked="true" runat="server" GroupName="gr5A" /></td>
                                            <td>A. Basic sector knowledge satisfactory for an assist  </td>
                                            <td>
                                                <asp:RadioButton ID="rd5as" runat="server" GroupName="gr5A" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd5ani" runat="server" GroupName="gr5A" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd5b_na" Checked="true" runat="server" GroupName="gr5B" /></td>
                                            <td>B. Runway configurations   </td>
                                            <td>
                                                <asp:RadioButton ID="rd5bs" runat="server" GroupName="gr5B" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd5bni" runat="server" GroupName="gr5B" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd5c_na" Checked="true" runat="server" GroupName="gr5C" /></td>
                                            <td>C. Satisfactory knowledge of callsign abbreviations   </td>
                                            <td>
                                                <asp:RadioButton ID="rd5cs" runat="server" GroupName="gr5C" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd5cni" runat="server" GroupName="gr5C" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd5d_na" Checked="true" runat="server" GroupName="gr5D" /></td>
                                            <td>D. Satisfactory knowledge of airport designators   </td>
                                            <td>
                                                <asp:RadioButton ID="rd5ds" runat="server" GroupName="gr5D" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd5dni" runat="server" GroupName="gr5D" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd5e_na" Checked="true" runat="server" GroupName="gr5E" /></td>
                                            <td>E. Provides assistance to the controller when required </td>
                                            <td>
                                                <asp:RadioButton ID="rd5es" runat="server" GroupName="gr5E" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd5eni" runat="server" GroupName="gr5E" /></td>
                                        </tr>
                                        
                                        <tr class="skill_categ">
                                            <th class="na"></th>
                                            <th>6. Overall </th>
                                            <th class="th_skills"></th>
                                            <th class="th_skills"></th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd6a_na" Checked="true" runat="server" GroupName="gr6A" /></td>
                                            <td>A. Works independently without instructor help   </td>
                                            <td>
                                                <asp:RadioButton ID="rd6as" runat="server" GroupName="gr6A" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd6ani" runat="server" GroupName="gr6A" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rd6b_na" Checked="true" runat="server" GroupName="gr6B" /></td>
                                            <td>B. Assessment only – Verbal test  </td>
                                            <td>
                                                <asp:RadioButton ID="rd6bs" runat="server" GroupName="gr6B" /></td>
                                            <td>
                                                <asp:RadioButton ID="rd6bni" runat="server" GroupName="gr6B" /></td>
                                        </tr>
                                        
                                        <tr class="skill_categ">
                                            <th class="na"></th>
                                            <th>7. Overall </th>
                                            <th class="th_skills"></th>
                                            <th class="th_skills"></th>
                                        </tr>
                                       
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <br />

                <div id="additionalcomments_div">
                    <table id="additionalcomments_table">
                        <thead>
                            <tr>
                                <th>Additional Comments
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_additionalcomments" CssClass="CommentBoxes" TextMode="MultiLine" Height="300" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                &nbsp;
            <div id="studentcomments_div">
                <table id="studentcomments_table">
                    <thead>
                        <tr>
                            <th colspan="3">Student Comments
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="3">
                                <asp:TextBox ID="txt_studentcomments" CssClass="CommentBoxes" TextMode="MultiLine" Height="300" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px; border-bottom: 1px solid black;"></td>
                            <td style="width: 700px;"></td>
                            <td style="width: 200px; border-bottom: 1px solid black;"></td>
                        </tr>
                        <tr>
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
                                <Triggers>
                                </Triggers>
                                <ContentTemplate>
                                    <td style="height: 100px; text-align: center; vertical-align: top;">Trainee Signature 
                                &nbsp;
                                <asp:Image ID="img_traineesign" Visible="false" runat="server" Style="width: 100px; height: 100px;" />
                                        <asp:Button ID="btn_sign_trainee" runat="server" Text="Sign for Trainee" OnClick="btn_sign_trainee_Click" />
                                    </td>
                                    <td></td>
                                    <td style="height: 100px; text-align: center; vertical-align: top;">OJTI Signature
                                &nbsp;
                                <asp:Image ID="img_ojtisign" Visible="false" runat="server" Style="width: 100px; height: 100px;" />
                                        <asp:Button ID="btn_sign_ojti" runat="server" Text="Sign for OJTI" OnClick="btn_sign_ojti_Click" />

                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                    </tbody>
                </table>
            </div>

                <div id="reportversion_div">
                    <table>
                        <tr>
                            <td>Doc. No : GECA - QWI-06-25</td>
                            <td>Issue No.02</td>
                            <td>Date: 16 July 2018</td>
                        </tr>
                    </table>
                    <br />
                </div>
                <br />

                <div style="width: 500px;">
                    <asp:Label ID="lbl_pageresult" runat="server" Visible="false" CssClass="errorbox"></asp:Label>
                </div>
                <br />
                <div>
                    <asp:Button ID="btn_submit" runat="server" CssClass="submit_button" Text="SUBMIT REPORT" OnClick="btn_submit_Click" />
                </div>
                <asp:Label ID="lbl_ojti_signed" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lbl_trainee_signed" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lbl_viewmode" runat="server" Visible="false" Text=""></asp:Label>
                 <asp:Label ID="lbl_STEPID" runat="server" Visible="false" Text=""></asp:Label>
            </div>
        </form>
    </div>

</body>
</html>

