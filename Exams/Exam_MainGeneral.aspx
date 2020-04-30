<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="Exam_MainGeneral.aspx.cs" Inherits="AviaTrain.Exams.Exam_MainGeneral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .awaiting_assignments {
            width: 998px;
            border-collapse: collapse;
            padding: 0px;
            margin: 0px;
            border: 3px solid #b63838;
        }

            .awaiting_assignments th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: white;
                background-color: #b63838;
            }

            .awaiting_assignments td {
                padding: 5px;
                min-height: 25px;
                min-height: 30px;
            }

        .grid_assignments {
            width: 100%;
            font-weight: bold;
            text-align: center;
        }

            .grid_assignments th {
                background-color: #c49c93;
                color: black;
            }

        .grid_completed {
            width: 100%;
            font-weight: bold;
            text-align: center;
        }

            .grid_completed th {
                background-color: #c49c93;
                color: black;
            }

        .container {
            width: 1000px;
            /*border: 2px solid indianred;*/
        }


        ul.tabs {
            margin: auto;
            padding: 15px;
            padding-bottom: 0px !important;
            list-style: none;
            background-color: none;
        }

            ul.tabs li {
                color: white;
                font-size: medium;
                font-weight: bold;
                color: #cbbbbb;
                display: inline-block;
                padding: 0px 0px;
                cursor: pointer;
                text-align: center;
                vertical-align: middle;
            }

                ul.tabs li.current {
                    background: #dedada;
                    color: #222;
                }

        .tab-content {
            display: none;
            background: #dedada;
            padding: 0px;
            border: 1px solid black;
        }

            .tab-content.current {
                display: inherit;
            }
    </style>

    <script>

        $(document).ready(function () {

            $('ul.tabs li').click(function () {
                var tab_id = $(this).attr('data-tab');

                $('ul.tabs li').removeClass('current');
                $('.tab-content').removeClass('current');

                $(this).addClass('current');
                $("#" + tab_id).addClass('current');
            })

        })

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <div class="container">

                <ul class="tabs">
                    <div style="text-align: center;">
                        <li class="tab-link current" data-tab="tab-1">
                            <div style="width: 130px; height: 30px; border: 1px solid black; display: inline-block; font-size: x-large;">
                                EXAMS
                            </div>
                        </li>
                        <li class="tab-link" data-tab="tab-2">
                            <div style="width: 160px; height: 30px; border: 1px solid black; display: inline-block; font-size: x-large;">
                                TRAININGS
                            </div>
                        </li>
                    </div>
                </ul>

                <div id="tab-1" class="tab-content current">
                    <br />
                    <br />
                    <table class="awaiting_assignments">
                        <tr>
                            <th>ASSIGNED EXAMS
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="grid_examassignments" runat="server" OnRowCommand="grid_examassignments_RowCommand"
                                    OnRowDataBound="grid_examassignments_RowDataBound" CssClass="grid_assignments">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="GO" ImageUrl="~/images/exam.png" Text="Take Exam" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                    <table class="awaiting_assignments">
                        <tr>
                            <th>COMPLETED EXAMS
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="grid_examcompleted" runat="server" CssClass="grid_completed" OnRowDataBound="grid_examcompleted_RowDataBound"></asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="tab-2" class="tab-content">
                    <br />
                    <br />
                    <table class="awaiting_assignments">
                        <tr>
                            <th>ASSIGNED TRAININGS
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="grid_assigned_training" runat="server"  CssClass="grid_assignments"
                                    OnRowCommand="grid_assigned_training_RowCommand"
                                    OnRowDataBound="grid_assigned_training_RowDataBound" >
                                    <Columns>
                                        <asp:ButtonField ButtonType="Image" CommandName="GO" ImageUrl="~/images/exam.png" Text="Do Training" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table class="awaiting_assignments">
                        <tr>
                            <th>TRAININGS
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="btn_unusual" runat="server" Text="Unusual / Emergency Training" PostBackUrl="~/Exams/Unusual/UnusualTraining.aspx"></asp:LinkButton>

                            </td>
                        </tr>
                    </table>
                </div>


            </div>
            <!-- container -->

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
