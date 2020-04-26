<%@ Page Title=""  ValidateRequest="false" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="CreateTraining.aspx.cs" Inherits="AviaTrain.Trainings.CreateTraining" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style>
        .editable {
            background-color: white;
            width: 1000px;
        }

        .trn_info_tbl {
            width: 1000px;
            border-collapse: collapse;
        }

            .trn_info_tbl th {
                text-align: center;
                font-weight: bold;
                font-size: large;
                background-color: indianred;
                color: white;
            }
            .trn_info_tbl td:first-child {
                width : 300px;
                padding :5px;
                font-weight : bold;
            }

        .page_design_table_uppers {
            width: 1000px;
        }

        .add_question_table {
        }
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="UploadButton" />
            <asp:PostBackTrigger ControlID="UploadButton2" />--%>
        </Triggers>
        <ContentTemplate>
         
                <table class="trn_info_tbl">
                    <tr>
                        <th colspan="2">CREATE TRAINING PACKAGE
                        </th>
                    </tr>
                    <tr>
                        <td>NAME  : </td>
                        <td>
                            <asp:TextBox ID="txt_examname" Width="200" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            EFFECTIVE DATE :
                        </td>
                        <td> 
                            <asp:TextBox ID="txt_effective" Width="200" runat="server" TextMode="Date"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            SECTOR : 
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_sectors" runat="server" Width="200">
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
                        <td colspan="2">
                            <asp:Label ID="lbl_info_error" runat="server" Visible="false"></asp:Label>
                            <asp:Button ID="btn_create_trn" runat="server" OnClick="btn_create_trn_Click" Text="Start Designing Training"
                                Style="width: 400px; background-color: indianred;color : white; font-size: medium; font-weight: bold;" />
                        </td>
                    </tr>
                </table>


 
               

        </ContentTemplate>
    </asp:UpdatePanel>


  

</asp:Content>
