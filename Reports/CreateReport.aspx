<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/MainsMaster.Master" AutoEventWireup="true" CodeBehind="CreateReport.aspx.cs" Inherits="AviaTrain.Reports.CreateReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .create_tbl {
            width: 100%;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
        }

            .create_tbl th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: white;
                background-color: #b63838	;
            }

            .create_tbl td:first-child {
                font-size: medium;
                font-weight: bold;
                padding-right: 5px;
                width: 330px;
            }

            .create_tbl td:nth-child(2) {
                font-size: medium;
                font-weight: bold;
                padding-right: 5px;
                width: 330px;
            }

            .create_tbl td:last-child {
                font-size: medium;
                font-weight: bold;
                padding-right: 5px;
                width: 330px;
            }

        .dropdown {
            width: 100%;
            font-weight: bold;
            border: 1px solid #b63838	;
            height : 30px;
        }
        .findbutton {
            width: 100%;
            font-weight: bold;
            border: 1px solid #b63838	;
            background-color : #b63838	 ;
            color : white;
            height : 30px;
        }
        
        .hidden-cell input {
            display : none !important;
        }

        .grid_folder {
            width : 100%;
            border-collapse : collapse;
            border : 1px solid #b63838	;
        }
        .grid_folder td {
            padding : 5px;
            margin : 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel runat="server" ID="uppanel_evaluation" UpdateMode="Always">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <div style="width: 1000px;">

                <table class="create_tbl">
                    <thead>
                        <tr>
                            <th colspan="3">CREATE REPORT FOR TRAINEE</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Trainee :</td>
                            <td>
                                <asp:DropDownList ID="ddl_trainees" runat="server"  CssClass="dropdown" OnSelectedIndexChanged="ddl_sectors_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>Position :</td>
                            <td>
                                <asp:DropDownList ID="ddl_positions" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddl_positions_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_sectors" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddl_sectors_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btn_findfolder" runat="server" Text="Find Training Folder" CssClass="findbutton" OnClick="btn_findfolder_Click" />
                            </td>
                            <td></td>
                        </tr>
                    </tbody>
                </table>

                <asp:Button ID="btn_createFolder" runat="server" Visible="false" OnClick="btn_createFolder_Click" Text="Create Training Folder"/>
                <br />
                <asp:Label ID="lbl_findresult" runat="server" Visible="false"></asp:Label>
                


                <asp:GridView ID="grid_folder" CssClass="grid_folder"  runat="server" OnRowCommand="grid_folder_RowCommand"  OnRowDataBound="grid_folder_RowDataBound" >
                    <Columns>
                        <asp:ButtonField ButtonType="Image" HeaderText ="Training Rpt" CommandName="Training" ImageUrl="~/Images/create.png"/>
                        <asp:ButtonField ButtonType="Image" HeaderText ="Recommend"  CommandName="Recommend" ImageUrl="~/Images/create.png"/>
                    </Columns>
                </asp:GridView>


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
