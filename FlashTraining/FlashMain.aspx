<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlashMain.aspx.cs" Inherits="AviaTrain.FlashTraining.FlashMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .usertable {
            border-collapse :collapse;
            border : 1px solid indianred;
            font-weight : bold;
            font-size : medium;
            height : 100px;
            width : 1100px;
        }
        .usertable td {
            border : 1px dashed indianred;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div> 
            <table class="usertable">
                <tr>
                    <td style="width : 200px;"> User Information</td>
                    <td style="width : 700px;">  Training Timing etc. NECESSARY FUNCTIONS </td>
                    <td style="width : 200px;"> Navigation Buttons </td>
                </tr>
            </table>
        </div>
        <div>
           <iframe id="train_iframe" src="main_lessons.htm" style="width : 1100px ; height:1000px;"> </iframe>
        </div>
    </form>
</body>
</html>
