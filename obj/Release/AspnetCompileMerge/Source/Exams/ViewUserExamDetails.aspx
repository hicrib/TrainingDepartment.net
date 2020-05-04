<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="ViewUserExamDetails.aspx.cs" Inherits="AviaTrain.Exams.ViewUserExamDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">
        function changeWidth() {
            var e1 = document.getElementById("e1");
            e1.style.width = 400;
        }
        window.onload = function () {
        };
    </script>

    <style>
        body {
            background-color: white;
        }

        .tbl_master_user {
            display: none;
        }

        .page_wrapper_table {
            border-collapse: collapse;
            border: 1px solid black;
        }

        .exam_header {
            border-collapse : collapse;
            width :550px;
            border : 1px solid black;
            margin-bottom : 50px;
            margin-left : 50px;
            margin-right : 50px;
        }

        .exam_header th {
            width:100%;
            background-color : black;
            color : white;
        }
        .exam_header td:first-child {
            font-size: large;
            font-weight : bold;
        }
        .exam_header td {
            padding: 5px;
        }
        .ops_q_table {
            width: 550px;
            border-collapse: collapse;
            margin: 50px;
        }

            .ops_q_table td {
                height: 30px;
            }

        .ops_q_head_span {
            font-size: large;
            font-weight: bold;
        }

        .ops_q_span {
            font-weight: bold;
        }

        .ops_empty_row {
            height: 10px !important;
        }

        .ops_image_td {
            width: 100px;
        }

        .ops_ABCD_td {
            width: 40px;
            font-weight: bold;
            font-size: large;
        }

        .ops_ops_td {
            width: 600px;
        }

        .fill_q_table {
            margin-bottom: 40px;
            width: 550px;
            border-collapse: collapse;
            margin: 50px;
        }

        .fill_q_head_td {
            font-size: large;
            font-weight: bold;
            margin-bottom: 10px;
        }

        .fill_text_span {
            font-weight: bold;
        }

        .div_blank {
        }

        input[type="text"]:disabled {
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="page_wrapper_table">
        <tr>
            <td>
                <div id="div_result_html" runat="server"></div>
            </td>
        </tr>
    </table>







</asp:Content>
