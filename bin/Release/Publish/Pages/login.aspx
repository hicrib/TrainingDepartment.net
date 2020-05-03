<%@ Page Language="C#" EnableSessionState="True" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="AviaTrain.Pages.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #main_div {
            margin-left: 15%;
            margin-top: 15%;
            border-collapse: collapse;
        }

        .submit_button {
            float: right;
            background-color: #a52a2a	;
            font-weight: bold;
            font-size: medium;
            color: white;
            width: 200px;
            height: 30px;
            border: 2px solid black;
        }
        .ErrorText{
            margin-top : 20px;
            float: right;
            font-weight : bold;
            
            color : #a52a2a	;
            border : 2px solid black;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="main_div">
            <div>
                <table id="login_table">
                    <thead>
                        <tr>
                            <th style="width: 100px; font-weight: bold;"></th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td style="width: 100px; font-weight: bold;">
                                <asp:Label ID="Label1" runat="server" Text="ID"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="username_txt" runat="server"  Height="35px" Width="192px"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 100px; font-weight: bold;">
                                <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="password_txt" runat="server" TextMode="Password" Height="35px" Width="192px"></asp:TextBox>
                            </td>

                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:Button ID="login_btn" runat="server" Text="LOGIN" OnClick="login_btn_Click" CssClass="submit_button" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="result_lbl" runat="server" Text="" CssClass="ErrorText"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
                
                <br />
                
            </div>
        </div>
    </form>
</body>
</html>
