﻿<%@ Page Language="C#" MasterPageFile="~/Masters/MainsMaster.Master" AutoEventWireup="true" CodeBehind="DAILYTR_ASS_RAD.aspx.cs" Inherits="AviaTrain.Reports.DAILYTR_ASS_RAD" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" runat="server" media="screen" href="../Css/TR.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel ID="pnl_wrapper" runat="server" Style="float: left;">
        <div id="reportinfo_div">
            <table class="main_table">
                <tr style="height: 50px; border-bottom: 1px solid black;">
                    <td style="background-color: #0080ff; font-size: larger; padding-left: 10px; font-weight: bold; color: white; border: 1px solid black;">
                        <div>Daily Training Report - Assist Radar </div>
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
                                                <td style="width: 250px; height: 100px; text-align: center; vertical-align: middle;">Trainee Initials/Signature
                                                    <br />
                                                    <asp:DropDownList ID="ddl_trainees" runat="server" Style="margin-top: 5px;"></asp:DropDownList>
                                                </td>
                                                <td style="width: 250px; height: 100px; text-align: center; vertical-align: middle;">OJTI Initials/Signature
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
                                                  <td style="width=50%; text-align:center;">
                                                    <asp:CheckBox ID="chk_OJT" Text="OJT" runat="server" />
                                                </td>
                                                 <td style="width=50%; text-align:center;">
                                                    <asp:CheckBox ID="chk_Ass" Text="Assessment" AutoPostBack="true" OnCheckedChanged="chk_Ass_CheckedChanged" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                               <td colspan="2" style="text-align:center;">
                                                    <asp:RadioButtonList ID="rad_passfail"  style="width:100%;" runat="server" RepeatDirection="Horizontal" Visible="false" >
                                                        <asp:ListItem Value="1"  Text="PASSED" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="0" Text="FAILED"  ></asp:ListItem>
                                                    </asp:RadioButtonList>
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
                <tr style="border: 1px solid black;">
                    <td style="width: 550px; border: 1px solid black;">
                        <table style="width: 100% !important;">
                            <tr style="border-bottom: 1px solid black;">
                                <td style="height: 40px;">Date : 
                                      <asp:TextBox ID="txt_date" runat="server" TextMode="Date"></asp:TextBox>
                                </td>
                                <td>Position :
                                        <asp:DropDownList ID="ddl_positions" DataSourceID="" runat="server"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 50%;">Traffic Density   :</td>
                                <td>
                                    <div>
                                        Light
                                                        <asp:CheckBox ID="chk_den_L" runat="server" />
                                        Mod
                                                        <asp:CheckBox ID="chk_den_M" runat="server" />
                                        Heavy
                                                        <asp:CheckBox ID="chk_den_H" runat="server" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 50%;">Complexity     : </td>
                                <td>
                                    <div>
                                        Low
                                                        <asp:CheckBox ID="chk_comp_L" runat="server" />
                                        Mod
                                                        <asp:CheckBox ID="chk_comp_M" runat="server" />
                                        High
                                                        <asp:CheckBox ID="chk_comp_H" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table style="padding: 5px; width: 100%;">
                            <tr>
                                <th style="width: 16%;"></th>
                                <th style="width: 25%;">Time One</th>
                                <th style="width: 30%;">Time Off</th>
                                <th style="width: 29%;"></th>
                            </tr>
                            <tr>
                                <td style="font-weight: bold; text-align: center;">Scheduled : </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="txt_timeon_sch" TextMode="Time" runat="server"></asp:TextBox>
                                </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="txt_timeoff_sch" TextMode="Time" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chk_noshow" runat="server" AutoPostBack="true" OnCheckedChanged="chk_noshow_CheckedChanged" />
                                    No-Show
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: bold; text-align: center;">Actual : </td>
                                <td style="text-align: center;">
                                    <asp:TextBox ID="txt_timeon_act" TextMode="Time" runat="server" AutoPostBack="true" OnTextChanged="txt_timeon_act_TextChanged"></asp:TextBox>
                                </td>
                                <td style="text-align: center; padding: 5px !important;">
                                    <asp:TextBox ID="txt_timeoff_act" TextMode="Time" runat="server" AutoPostBack="true" OnTextChanged="txt_timeon_act_TextChanged"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chk_notraining" runat="server" AutoPostBack="true" OnCheckedChanged="chk_noshow_CheckedChanged" />
                                    No Training Value
                                </td>
                            </tr>
                            <tr style="border-top: 1px solid black;">
                                <td colspan="2" style="text-align: center; padding: 5px !important;">Hours :
                                        <asp:TextBox ID="txt_hours" Enabled="false" Width="70"  runat="server"></asp:TextBox>
                                </td>
                                <td colspan="2">Total Hours :
                                        <asp:TextBox ID="txt_totalhours" Width="70" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>
        </div>
        <!-- EVAULUATIONS -->
        <div id="evaluation_div">
            <br />

            <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Always">
                <Triggers>
                    <asp:PostBackTrigger ControlID="UploadButton" />
                </Triggers>
                <ContentTemplate>
                    <asp:Panel ID="evaluation_panel" runat="server">

                        <table id="evaluation_main_table" class="evaluation_main_table">
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
                                        <asp:RadioButtonList ID="rad_prebrief_comment" CssClass="rad_prebrief_comment" runat="server" Width="300" AutoPostBack="true" OnSelectedIndexChanged="rad_prebrief_comment_SelectedIndexChanged" RepeatDirection="Horizontal">
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
                                    <th>2. Strip Marking and Board Management </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2a_na" Checked="true" runat="server" GroupName="gr2A" /></td>
                                    <td>A. Marks strips whenever time permits</td>
                                    <td>
                                        <asp:RadioButton ID="rd2as" runat="server" GroupName="gr2A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2ani" runat="server" GroupName="gr2A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2b_na" Checked="true" runat="server" GroupName="gr2B" /></td>
                                    <td>B. Correctly marks strips</td>
                                    <td>
                                        <asp:RadioButton ID="rd2bs" runat="server" GroupName="gr2B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2bni" runat="server" GroupName="gr2B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2c_na" Checked="true" runat="server" GroupName="gr2C" /></td>
                                    <td>C. Understands the standard board layout </td>
                                    <td>
                                        <asp:RadioButton ID="rd2cs" runat="server" GroupName="gr2C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2cni" runat="server" GroupName="gr2C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2d_na" Checked="true" runat="server" GroupName="gr2D" /></td>
                                    <td>D. Varies the board layout for individual controller preference</td>
                                    <td>
                                        <asp:RadioButton ID="rd2ds" runat="server" GroupName="gr2D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2dni" runat="server" GroupName="gr2D" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2e_na" Checked="true" runat="server" GroupName="gr2E" /></td>
                                    <td>E. Ensures strips and board management are up to date and ??</td>
                                    <td>
                                        <asp:RadioButton ID="rd2es" runat="server" GroupName="gr2E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2eni" runat="server" GroupName="gr2E" /></td>
                                </tr>
                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>3. Estimates and Weather  </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd3a_na" Checked="true" runat="server" GroupName="gr3A" /></td>
                                    <td>A. Passes estimates in time  </td>
                                    <td>
                                        <asp:RadioButton ID="rd3as" runat="server" GroupName="gr3A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3ani" runat="server" GroupName="gr3A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd3b_na" Checked="true" runat="server" GroupName="gr3B" /></td>
                                    <td>B. Passes estimates in the correct format  </td>
                                    <td>
                                        <asp:RadioButton ID="rd3bs" runat="server" GroupName="gr3B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3bni" runat="server" GroupName="gr3B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd3c_na" Checked="true" runat="server" GroupName="gr3C" /></td>
                                    <td>C. Requests weather information at the correct time </td>
                                    <td>
                                        <asp:RadioButton ID="rd3cs" runat="server" GroupName="gr3C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3cni" runat="server" GroupName="gr3C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd3d_na" Checked="true" runat="server" GroupName="gr3D" /></td>
                                    <td>D. Writes weather info clearly and in the correct format </td>
                                    <td>
                                        <asp:RadioButton ID="rd3ds" runat="server" GroupName="gr3D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3dni" runat="server" GroupName="gr3D" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd3e_na" Checked="true" runat="server" GroupName="gr3E" /></td>
                                    <td>E. Informs the controller of significant weather changes  </td>
                                    <td>
                                        <asp:RadioButton ID="rd3es" runat="server" GroupName="gr3E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3eni" runat="server" GroupName="gr3E" /></td>
                                </tr>
                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>4. Releases (Aprroach Only) </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd4a_na" Checked="true" runat="server" GroupName="gr4A" /></td>
                                    <td>A. Writes details of outbounds clearly and correctly </td>
                                    <td>
                                        <asp:RadioButton ID="rd4as" runat="server" GroupName="gr4A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4ani" runat="server" GroupName="gr4A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd4b_na" Checked="true" runat="server" GroupName="gr4B" /></td>
                                    <td>B. Correctly writes the release on the strip </td>
                                    <td>
                                        <asp:RadioButton ID="rd4bs" runat="server" GroupName="gr4B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4bni" runat="server" GroupName="gr4B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd4c_na" Checked="true" runat="server" GroupName="gr4C" /></td>
                                    <td>C. Correctly repeats the clearance to the tower </td>
                                    <td>
                                        <asp:RadioButton ID="rd4cs" runat="server" GroupName="gr4C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4cni" runat="server" GroupName="gr4C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd4d_na" Checked="true" runat="server" GroupName="gr4D" /></td>
                                    <td>D. Listens for incorrect readbacks from the tower  </td>
                                    <td>
                                        <asp:RadioButton ID="rd4ds" runat="server" GroupName="gr4D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4dni" runat="server" GroupName="gr4D" /></td>
                                </tr>
                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>5. Frequency Management and Coordination </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd5a_na" Checked="true" runat="server" GroupName="gr5A" /></td>
                                    <td>A. Able to select correct frequencies for the sector  </td>
                                    <td>
                                        <asp:RadioButton ID="rd5as" runat="server" GroupName="gr5A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5ani" runat="server" GroupName="gr5A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd5b_na" Checked="true" runat="server" GroupName="gr5B" /></td>
                                    <td>B. Correct use of speaker to avoid distracting the controller  </td>
                                    <td>
                                        <asp:RadioButton ID="rd5bs" runat="server" GroupName="gr5B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5bni" runat="server" GroupName="gr5B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd5c_na" Checked="true" runat="server" GroupName="gr5C" /></td>
                                    <td>C. Listens before speaking when calling on the hotline   </td>
                                    <td>
                                        <asp:RadioButton ID="rd5cs" runat="server" GroupName="gr5C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5cni" runat="server" GroupName="gr5C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd5d_na" Checked="true" runat="server" GroupName="gr5D" /></td>
                                    <td>D. Identify themselves on first contact   </td>
                                    <td>
                                        <asp:RadioButton ID="rd5ds" runat="server" GroupName="gr5D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5dni" runat="server" GroupName="gr5D" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd5e_na" Checked="true" runat="server" GroupName="gr5E" /></td>
                                    <td>E. Clear and accurate coordination with other sectors  </td>
                                    <td>
                                        <asp:RadioButton ID="rd5es" runat="server" GroupName="gr5E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5eni" runat="server" GroupName="gr5E" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd5f_na" Checked="true" runat="server" GroupName="gr5F" /></td>
                                    <td>F. Handover/Take over position  </td>
                                    <td>
                                        <asp:RadioButton ID="rd5fs" runat="server" GroupName="gr5F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5fni" runat="server" GroupName="gr5F" /></td>
                                </tr>
                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>6. General </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6a_na" Checked="true" runat="server" GroupName="gr6A" /></td>
                                    <td>A. Basic sector knowledge satisfactory for an assist   </td>
                                    <td>
                                        <asp:RadioButton ID="rd6as" runat="server" GroupName="gr6A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6ani" runat="server" GroupName="gr6A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6b_na" Checked="true" runat="server" GroupName="gr6B" /></td>
                                    <td>B. Runway configurations  </td>
                                    <td>
                                        <asp:RadioButton ID="rd6bs" runat="server" GroupName="gr6B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6bni" runat="server" GroupName="gr6B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6c_na" Checked="true" runat="server" GroupName="gr6C" /></td>
                                    <td>C. Types of approaches available (approach only)  </td>
                                    <td>
                                        <asp:RadioButton ID="rd6cs" runat="server" GroupName="gr6C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6cni" runat="server" GroupName="gr6C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6d_na" Checked="true" runat="server" GroupName="gr6D" /></td>
                                    <td>D. Satisfactory knowledge of callsign abbreviations   </td>
                                    <td>
                                        <asp:RadioButton ID="rd6ds" runat="server" GroupName="gr6D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6dni" runat="server" GroupName="gr6D" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6e_na" Checked="true" runat="server" GroupName="gr6E" /></td>
                                    <td>E. Satisfactory knowledge of airport designators  </td>
                                    <td>
                                        <asp:RadioButton ID="rd6es" runat="server" GroupName="gr6E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6eni" runat="server" GroupName="gr6E" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6f_na" Checked="true" runat="server" GroupName="gr6F" /></td>
                                    <td>F. Change strip printer roll  </td>
                                    <td>
                                        <asp:RadioButton ID="rd6fs" runat="server" GroupName="gr6F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6fni" runat="server" GroupName="gr6F" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6g_na" Checked="true" runat="server" GroupName="gr6G" /></td>
                                    <td>G. Reports and logs equipment failures  </td>
                                    <td>
                                        <asp:RadioButton ID="rd6gs" runat="server" GroupName="gr6G" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6gni" runat="server" GroupName="gr6G" /></td>
                                </tr>
                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>7. Overall </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd7a_na" Checked="true" runat="server" GroupName="gr7A" /></td>
                                    <td>A. Works independently without instructor help   </td>
                                    <td>
                                        <asp:RadioButton ID="rd7as" runat="server" GroupName="gr7A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7ani" runat="server" GroupName="gr7A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd7b_na" Checked="true" runat="server" GroupName="gr7B" /></td>
                                    <td>B. Verbal Test – Assessment only   </td>
                                    <td>
                                        <asp:RadioButton ID="rd7bs" runat="server" GroupName="gr7B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7bni" runat="server" GroupName="gr7B" /></td>
                                </tr>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <br />

        <div id="additionalcomments_div">
            <table id="additionalcomments_table" class="additionalcomments_table">
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
                <table id="studentcomments_table" class="studentcomments_table">
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

        <div id="reportversion_div" class="reportversion_div">
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
        <asp:Label ID="lbl_viewmode" runat="server" Visible="false" Text="Label"></asp:Label>
        <asp:Label ID="lbl_genid" runat="server" Visible="false" Text=""></asp:Label>
        <asp:Label ID="lbl_stepid" runat="server" Visible="false" Text=""></asp:Label>
    </asp:Panel>
</asp:Content>


