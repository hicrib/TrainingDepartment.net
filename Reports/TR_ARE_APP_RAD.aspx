<%@ Page Language="C#" MasterPageFile="~/Masters/MainsMaster.Master" AutoEventWireup="true" CodeBehind="TR_ARE_APP_RAD.aspx.cs" Inherits="AviaTrain.Reports.TR_ARE_APP_RAD" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" runat="server" media="screen" href="../Css/TR.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnl_wrapper" runat="server" Style="float: left;">
        <div id="reportinfo_div">
            <table class="main_table">
                <tr>
                    <td colspan="2">
                        <table style="width: 100%;">
                            <tr style="height: 50px; border-bottom: 1px solid black;">
                                <td style="background-color: #0080ff; font-size: larger; padding-left: 10px; font-weight: bold; color: white; border: 1px solid black;">
                                    <div>Training Report - Area and Approach Radar</div>
                                </td>
                                <td style="background-color: white; font-weight: bold; padding-left: 5px; border: 1px solid black;">
                                    <div>
                                        Report Number : 
                                        <asp:Label ID="lbl_reportnumber" runat="server" Font-Size="Larger" Text=""></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!-- second row -->
                <tr style="height: 100px; border-bottom: 1px solid black;">
                    <td colspan="2" style="">
                        <table style="border-collapse: collapse;">
                            <tr>
                                <td style="border-collapse: collapse; border-right: 1px solid black; width: 550px;">
                                    <!-- initials signatures -->
                                    <table>
                                        <tr>
                                            <td style="width: 275px; height: 100px; text-align: center; vertical-align: middle;">Trainee Initials/Signature
                                                    <br />
                                                <asp:DropDownList ID="ddl_trainees" runat="server" Style="margin-top: 5px;"></asp:DropDownList>
                                            </td>
                                            <td style="width: 275px; height: 100px; text-align: center; vertical-align: middle;">OJTI Initials/Signature
                                                    <br />
                                                <asp:DropDownList ID="ddl_ojtis" runat="server" Style="margin-top: 5px;"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <!-- checkboxes -->
                                    <table style="width: 100%; height: 100px;">
                                        <tr>
                                            <td style="width: 170px;">
                                                <asp:CheckBox ID="chk_OJT" Text="OJT" runat="server" />
                                            </td>
                                            <td style="width: 170px;">
                                                <asp:CheckBox ID="chk_LvlAss" AutoPostBack="true" OnCheckedChanged="chk_Assess_CheckedChanged" Text="Level Assessment" runat="server" />
                                            </td>
                                            <td style="width: 170px;">
                                                <asp:CheckBox ID="chk_RemAss" AutoPostBack="true" OnCheckedChanged="chk_Assess_CheckedChanged" Text="Remedial Assessment" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 170px;">
                                                <asp:CheckBox ID="chk_PREOJT" Text="Pre-OJT" runat="server" />
                                            </td>
                                            <td style="width: 170px;">
                                                <asp:CheckBox ID="chk_ProgAss" AutoPostBack="true" OnCheckedChanged="chk_Assess_CheckedChanged" Text="Progress Assessment" runat="server" />
                                            </td>
                                            <td style="width: 170px;">
                                                <asp:CheckBox ID="chk_OST" Text="Over the shoulder" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 170px;">
                                                <asp:CheckBox ID="chk_Sim" Text="Simulator" AutoPostBack="true" OnCheckedChanged="txt_timeon_act_TextChanged" runat="server" />
                                            </td>
                                            <td style="width: 170px;">
                                                <asp:CheckBox ID="chk_CocAss" AutoPostBack="true" OnCheckedChanged="chk_Assess_CheckedChanged" Text="Coc Assessment" Enabled="false" runat="server" />
                                            </td>
                                            <td style="width: 170px;"></td>
                                        </tr>
                                        <tr>
                                             <td style="text-align:center;">
                                                <asp:Label ID="lbl_phase" runat="server" Style=" font-weight:bold;"  Text=""></asp:Label>
                                            </td>
                                            <td colspan="2" style="text-align: center; font-weight:bold;">
                                                <asp:RadioButtonList ID="rad_passfail" runat="server" RepeatDirection="Horizontal" Visible="false">
                                                    <asp:ListItem Value="1" Text="PASSED" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="FAILED"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
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
                                        <asp:TextBox ID="txt_hours" Width="70" Enabled="false" runat="server"></asp:TextBox>
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
                                    <th style="border-left: 2px solid black !important;">Critical Skills</th>
                                    <th class="th_skills">A</th>
                                    <th class="th_skills">B</th>
                                    <th class="th_skills">C</th>
                                    <th class="th_skills">D</th>

                                    <th style="border-left: 2px solid black !important; width: 600px;">Pre-Brief and Debrief Comments</th>


                                </tr>
                            </thead>
                            <tbody>
                                <tr class="skill_categ">
                                    <th></th>
                                    <th>1. Seperation</th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
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
                                        <asp:RadioButton ID="rd1a_na" Checked="true" runat="server" GroupName="gr1A" />
                                    </td>
                                    <td>A. Scans Effectively</td>
                                    <td>
                                        <asp:RadioButton ID="rd1aa" runat="server" GroupName="gr1A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1ab" runat="server" GroupName="gr1A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1ac" runat="server" GroupName="gr1A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1ad" runat="server" GroupName="gr1A" /></td>

                                    <td rowspan="0" style="vertical-align: top; text-align: left;">
                                        <asp:FileUpload ID="file_prebrief_comment" runat="server" accept=".png,.jpg,.jpeg,.gif" />
                                        <asp:Button runat="server" ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" />
                                        <asp:Label ID="uploadedfilename" runat="server" Visible="false"></asp:Label>

                                        <br />
                                        <asp:Label ID="statuslabel" runat="server" Font-Size="Large" ForeColor="Red"></asp:Label>
                                        <asp:TextBox ID="txt_prebrief_comment" TextMode="MultiLine" PlaceHolder="Comments" CssClass="CommentBoxes" Visible="false" runat="server"></asp:TextBox>
                                        <asp:Image ID="img_file" Visible="false" Style="width: 100%;" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd1b_na" Checked="true" runat="server" GroupName="gr1B" /></td>
                                    <td>B. Seperation is Assured</td>
                                    <td>
                                        <asp:RadioButton ID="rd1ba" runat="server" GroupName="gr1B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1bb" runat="server" GroupName="gr1B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1bc" runat="server" GroupName="gr1B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1bd" runat="server" GroupName="gr1B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd1c_na" Checked="true" runat="server" GroupName="gr1C" /></td>
                                    <td>C. Efficiently Seperates Traffic</td>
                                    <td>
                                        <asp:RadioButton ID="rd1ca" runat="server" GroupName="gr1C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1cb" runat="server" GroupName="gr1C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1cc" runat="server" GroupName="gr1C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1cd" runat="server" GroupName="gr1C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd1d_na" Checked="true" runat="server" GroupName="gr1D" /></td>
                                    <td>D. Safety Alerts</td>
                                    <td>
                                        <asp:RadioButton ID="rd1da" runat="server" GroupName="gr1D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1db" runat="server" GroupName="gr1D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1dc" runat="server" GroupName="gr1D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1dd" runat="server" GroupName="gr1D" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd1e_na" Checked="true" runat="server" GroupName="gr1E" /></td>
                                    <td>E. Traffic Information  </td>
                                    <td>
                                        <asp:RadioButton ID="rd1ea" runat="server" GroupName="gr1E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1eb" runat="server" GroupName="gr1E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1ec" runat="server" GroupName="gr1E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1ed" runat="server" GroupName="gr1E" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd1f_na" Checked="true" runat="server" GroupName="gr1F" /></td>
                                    <td>F. Overall Awareness  </td>
                                    <td>
                                        <asp:RadioButton ID="rd1fa" runat="server" GroupName="gr1F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1fb" runat="server" GroupName="gr1F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1fc" runat="server" GroupName="gr1F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd1fd" runat="server" GroupName="gr1F" /></td>
                                </tr>
                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>2. Traffic Management </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2a_na" Checked="true" runat="server" GroupName="gr2A" /></td>
                                    <td>A. Planning / Flexibility</td>
                                    <td>
                                        <asp:RadioButton ID="rd2aa" runat="server" GroupName="gr2A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2ab" runat="server" GroupName="gr2A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2ac" runat="server" GroupName="gr2A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2ad" runat="server" GroupName="gr2A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2b_na" Checked="true" runat="server" GroupName="gr2B" /></td>
                                    <td>B. Efficiency / Priorities</td>
                                    <td>
                                        <asp:RadioButton ID="rd2ba" runat="server" GroupName="gr2B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2bb" runat="server" GroupName="gr2B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2bc" runat="server" GroupName="gr2B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2bd" runat="server" GroupName="gr2B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2c_na" Checked="true" runat="server" GroupName="gr2C" /></td>
                                    <td>C. Vectoring / Spacing</td>
                                    <td>
                                        <asp:RadioButton ID="rd2ca" runat="server" GroupName="gr2C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2cb" runat="server" GroupName="gr2C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2cc" runat="server" GroupName="gr2C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2cd" runat="server" GroupName="gr2C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2d_na" Checked="true" runat="server" GroupName="gr2D" /></td>
                                    <td>D. Speed Control</td>
                                    <td>
                                        <asp:RadioButton ID="rd2da" runat="server" GroupName="gr2D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2db" runat="server" GroupName="gr2D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2dc" runat="server" GroupName="gr2D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2dd" runat="server" GroupName="gr2D" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2e_na" Checked="true" runat="server" GroupName="gr2E" /></td>
                                    <td>E. Use Aircraft Performance</td>
                                    <td>
                                        <asp:RadioButton ID="rd2ea" runat="server" GroupName="gr2E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2eb" runat="server" GroupName="gr2E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2ec" runat="server" GroupName="gr2E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2ed" runat="server" GroupName="gr2E" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2f_na" Checked="true" runat="server" GroupName="gr2F" /></td>
                                    <td>F. Radar Identification</td>
                                    <td>
                                        <asp:RadioButton ID="rd2fa" runat="server" GroupName="gr2F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2fb" runat="server" GroupName="gr2F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2fc" runat="server" GroupName="gr2F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2fd" runat="server" GroupName="gr2F" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2g_na" Checked="true" runat="server" GroupName="gr2G" /></td>
                                    <td>G. Mode C Verification</td>
                                    <td>
                                        <asp:RadioButton ID="rd2ga" runat="server" GroupName="gr2G" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2gb" runat="server" GroupName="gr2G" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2gc" runat="server" GroupName="gr2G" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2gd" runat="server" GroupName="gr2G" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2h_na" Checked="true" runat="server" GroupName="gr2H" /></td>
                                    <td>H. Initiative</td>
                                    <td>
                                        <asp:RadioButton ID="rd2Ha" runat="server" GroupName="gr2H" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Hb" runat="server" GroupName="gr2H" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Hc" runat="server" GroupName="gr2H" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Hd" runat="server" GroupName="gr2H" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2i_na" Checked="true" runat="server" GroupName="gr2I" /></td>
                                    <td>I. Effective Working Speed</td>
                                    <td>
                                        <asp:RadioButton ID="rd2Ia" runat="server" GroupName="gr2I" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Ib" runat="server" GroupName="gr2I" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Ic" runat="server" GroupName="gr2I" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Id" runat="server" GroupName="gr2I" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2j_na" Checked="true" runat="server" GroupName="gr2J" /></td>
                                    <td>J. Unusual Occurences</td>
                                    <td>
                                        <asp:RadioButton ID="rd2Ja" runat="server" GroupName="gr2J" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Jb" runat="server" GroupName="gr2J" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Jc" runat="server" GroupName="gr2J" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Jd" runat="server" GroupName="gr2J" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd2k_na" Checked="true" runat="server" GroupName="gr2K" /></td>
                                    <td>K. Emergencies</td>
                                    <td>
                                        <asp:RadioButton ID="rd2Ka" runat="server" GroupName="gr2K" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Kb" runat="server" GroupName="gr2K" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Kc" runat="server" GroupName="gr2K" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd2Kd" runat="server" GroupName="gr2K" /></td>
                                </tr>


                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>3. Knowledge  </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd3a_na" Checked="true" runat="server" GroupName="gr3A" /></td>
                                    <td>A. Map / Sector / Airspace  </td>
                                    <td>
                                        <asp:RadioButton ID="rd3aa" runat="server" GroupName="gr3A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3ab" runat="server" GroupName="gr3A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3ac" runat="server" GroupName="gr3A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3ad" runat="server" GroupName="gr3A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd3b_na" Checked="true" runat="server" GroupName="gr3B" /></td>
                                    <td>B. Procedures   </td>
                                    <td>
                                        <asp:RadioButton ID="rd3ba" runat="server" GroupName="gr3B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3bb" runat="server" GroupName="gr3B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3bc" runat="server" GroupName="gr3B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3bd" runat="server" GroupName="gr3B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd3c_na" Checked="true" runat="server" GroupName="gr3C" /></td>
                                    <td>C. Documents / LATSI / LOAs etc.   </td>
                                    <td>
                                        <asp:RadioButton ID="rd3ca" runat="server" GroupName="gr3C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3cb" runat="server" GroupName="gr3C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3cc" runat="server" GroupName="gr3C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3cd" runat="server" GroupName="gr3C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd3d_na" Checked="true" runat="server" GroupName="gr3D" /></td>
                                    <td>D. Briefed NOTAMs / POIs / TOIs etc.  </td>
                                    <td>
                                        <asp:RadioButton ID="rd3da" runat="server" GroupName="gr3D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3db" runat="server" GroupName="gr3D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3dc" runat="server" GroupName="gr3D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd3dd" runat="server" GroupName="gr3D" /></td>
                                </tr>
                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>4. Co-ordination </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd4a_na" Checked="true" runat="server" GroupName="gr4A" /></td>
                                    <td>A. Internal </td>
                                    <td>
                                        <asp:RadioButton ID="rd4aa" runat="server" GroupName="gr4A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4ab" runat="server" GroupName="gr4A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4ac" runat="server" GroupName="gr4A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4ad" runat="server" GroupName="gr4A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd4b_na" Checked="true" runat="server" GroupName="gr4B" /></td>
                                    <td>B. External </td>
                                    <td>
                                        <asp:RadioButton ID="rd4ba" runat="server" GroupName="gr4B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4bb" runat="server" GroupName="gr4B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4bc" runat="server" GroupName="gr4B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4bd" runat="server" GroupName="gr4B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd4c_na" Checked="true" runat="server" GroupName="gr4C" /></td>
                                    <td>C. Handoff / Point Out </td>
                                    <td>
                                        <asp:RadioButton ID="rd4ca" runat="server" GroupName="gr4C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4cb" runat="server" GroupName="gr4C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4cc" runat="server" GroupName="gr4C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4cd" runat="server" GroupName="gr4C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd4d_na" Checked="true" runat="server" GroupName="gr4D" /></td>
                                    <td>D. Handover / Takeover Position  </td>
                                    <td>
                                        <asp:RadioButton ID="rd4da" runat="server" GroupName="gr4D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4db" runat="server" GroupName="gr4D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4dc" runat="server" GroupName="gr4D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd4dd" runat="server" GroupName="gr4D" /></td>
                                </tr>
                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>5. Equipment </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd5a_na" Checked="true" runat="server" GroupName="gr5A" /></td>
                                    <td>A. Equipment Functions   </td>
                                    <td>
                                        <asp:RadioButton ID="rd5aa" runat="server" GroupName="gr5A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5ab" runat="server" GroupName="gr5A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5ac" runat="server" GroupName="gr5A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5ad" runat="server" GroupName="gr5A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd5b_na" Checked="true" runat="server" GroupName="gr5B" /></td>
                                    <td>B. Reporting Failures / Log Entries   </td>
                                    <td>
                                        <asp:RadioButton ID="rd5ba" runat="server" GroupName="gr5B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5bb" runat="server" GroupName="gr5B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5bc" runat="server" GroupName="gr5B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd5bd" runat="server" GroupName="gr5B" /></td>
                                </tr>
                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>6. Data Management </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6a_na" Checked="true" runat="server" GroupName="gr6A" /></td>
                                    <td>A. Strip Marking     </td>
                                    <td>
                                        <asp:RadioButton ID="rd6aa" runat="server" GroupName="gr6A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6ab" runat="server" GroupName="gr6A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6ac" runat="server" GroupName="gr6A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6ad" runat="server" GroupName="gr6A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6b_na" Checked="true" runat="server" GroupName="gr6B" /></td>
                                    <td>B. Board Management    </td>
                                    <td>
                                        <asp:RadioButton ID="rd6ba" runat="server" GroupName="gr6B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6bb" runat="server" GroupName="gr6B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6bc" runat="server" GroupName="gr6B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6bd" runat="server" GroupName="gr6B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6c_na" Checked="true" runat="server" GroupName="gr6C" /></td>
                                    <td>C. Label Inputs    </td>
                                    <td>
                                        <asp:RadioButton ID="rd6ca" runat="server" GroupName="gr6C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6cb" runat="server" GroupName="gr6C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6cc" runat="server" GroupName="gr6C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6cd" runat="server" GroupName="gr6C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6d_na" Checked="true" runat="server" GroupName="gr6D" /></td>
                                    <td>D. Label Management     </td>
                                    <td>
                                        <asp:RadioButton ID="rd6da" runat="server" GroupName="gr6D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6db" runat="server" GroupName="gr6D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6dc" runat="server" GroupName="gr6D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6dd" runat="server" GroupName="gr6D" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd6e_na" Checked="true" runat="server" GroupName="gr6E" /></td>
                                    <td>E. Flight Plan System Input    </td>
                                    <td>
                                        <asp:RadioButton ID="rd6ea" runat="server" GroupName="gr6E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6eb" runat="server" GroupName="gr6E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6ec" runat="server" GroupName="gr6E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd6ed" runat="server" GroupName="gr6E" /></td>
                                </tr>
                                <tr class="skill_categ">
                                    <th class="na"></th>
                                    <th>7. Phraseology & R/T </th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                    <th class="th_skills"></th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd7a_na" Checked="true" runat="server" GroupName="gr7A" /></td>
                                    <td>A. Standart Phrases   </td>
                                    <td>
                                        <asp:RadioButton ID="rd7aa" runat="server" GroupName="gr7A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7ab" runat="server" GroupName="gr7A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7ac" runat="server" GroupName="gr7A" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7ad" runat="server" GroupName="gr7A" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd7b_na" Checked="true" runat="server" GroupName="gr7B" /></td>
                                    <td>B. Clearance Delivery   </td>
                                    <td>
                                        <asp:RadioButton ID="rd7ba" runat="server" GroupName="gr7B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7bb" runat="server" GroupName="gr7B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7bc" runat="server" GroupName="gr7B" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7bd" runat="server" GroupName="gr7B" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd7c_na" Checked="true" runat="server" GroupName="gr7C" /></td>
                                    <td>C. Comprehension   </td>
                                    <td>
                                        <asp:RadioButton ID="rd7ca" runat="server" GroupName="gr7C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7cb" runat="server" GroupName="gr7C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7cc" runat="server" GroupName="gr7C" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7cd" runat="server" GroupName="gr7C" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd7d_na" Checked="true" runat="server" GroupName="gr7D" /></td>
                                    <td>D. Active Listening / Readbacks   </td>
                                    <td>
                                        <asp:RadioButton ID="rd7da" runat="server" GroupName="gr7D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7db" runat="server" GroupName="gr7D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7dc" runat="server" GroupName="gr7D" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7dd" runat="server" GroupName="gr7D" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd7e_na" Checked="true" runat="server" GroupName="gr7E" /></td>
                                    <td>E. Pronunciation    </td>
                                    <td>
                                        <asp:RadioButton ID="rd7ea" runat="server" GroupName="gr7E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7eb" runat="server" GroupName="gr7E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7ec" runat="server" GroupName="gr7E" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7ed" runat="server" GroupName="gr7E" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd7f_na" Checked="true" runat="server" GroupName="gr7F" /></td>
                                    <td>F. Speech Rate    </td>
                                    <td>
                                        <asp:RadioButton ID="rd7fa" runat="server" GroupName="gr7F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7fb" runat="server" GroupName="gr7F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7fc" runat="server" GroupName="gr7F" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7fd" runat="server" GroupName="gr7F" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd7g_na" Checked="true" runat="server" GroupName="gr7G" /></td>
                                    <td>G. Radio Technique    </td>
                                    <td>
                                        <asp:RadioButton ID="rd7ga" runat="server" GroupName="gr7G" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7gb" runat="server" GroupName="gr7G" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7gc" runat="server" GroupName="gr7G" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7gd" runat="server" GroupName="gr7G" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rd7h_na" Checked="true" runat="server" GroupName="gr7H" /></td>
                                    <td>H. Frequency Management    </td>
                                    <td>
                                        <asp:RadioButton ID="rd7ha" runat="server" GroupName="gr7H" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7hb" runat="server" GroupName="gr7H" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7hc" runat="server" GroupName="gr7H" /></td>
                                    <td>
                                        <asp:RadioButton ID="rd7hd" runat="server" GroupName="gr7H" /></td>
                                </tr>
                            </tbody>
                        </table>


                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        &nbsp;
            <div id="explanation_div">
                <table id="explanation_table" class="explanation_table">
                    <thead>
                        <tr>
                            <th colspan="2">Training Report Explanation</th>
                        </tr>
                        <tr>
                            <th colspan="2">If traffic levels are too low to make an assessment an indication should be made in the ‘not observed’ column.</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td style="width: 80px;">A
                            </td>
                            <td>The trainee achieved the standard* during this training session at the traffic levels indicated
                            </td>
                        </tr>
                        <tr>
                            <td>B
                            </td>
                            <td>The trainee mostly achieved the standard
                            </td>
                        </tr>
                        <tr>
                            <td>C
                            </td>
                            <td>The trainee sometimes achieved the standard
                            </td>
                        </tr>
                        <tr>
                            <td>D
                            </td>
                            <td>The trainee never achieved the standard or a safety critical error occurred (this may be issued in addition to B or C)
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">To achieve the standard, tasks should be carried out correctly, confidently, and without hesitation
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

        &nbsp;
            <div id="notes_div">
                <table id="notes_table" class="notes_table">
                    <thead>
                        <tr>
                            <th>Notes
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <asp:TextBox ID="txt_notes" CssClass="CommentBoxes" TextMode="MultiLine" Height="300" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>

                </table>

            </div>

        &nbsp;
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
