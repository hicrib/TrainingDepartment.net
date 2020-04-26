<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ExamsMaster.Master" AutoEventWireup="true" CodeBehind="Exam_MainGeneral.aspx.cs" Inherits="AviaTrain.Exams.Exam_MainGeneral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .awaiting_assignments {
            width: 1000px;
            border-collapse: collapse;
            padding: 10px;
            margin: 0px;
            border: 3px solid #b63838	;
        }

            .awaiting_assignments th {
                text-align: center;
                font-size: large;
                font-weight: bold;
                color: white;
                background-color: #b63838	;
            }

            .awaiting_assignments td {
                padding: 5px;
                min-height: 25px;
                min-height : 30px;
            }

        .grid_assignments {
            width: 100%;
            font-weight: bold;
            text-align: center;
        }
        .grid_assignments th {
            background-color : #c49c93;
            color : black;
        }
        
        .grid_completed {
            width: 100%;
            font-weight: bold;
            text-align: center;
        }
        .grid_completed th {
          background-color : #c49c93;
            color : black;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Always" ChildrenAsTriggers="true">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <table>
                <tr>
                    <td>

                    </td>
                </tr>
            </table>

            <table class="awaiting_assignments">
                <tr>
                    <th>ASSIGNMENTS
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grid_assignments" runat="server" OnRowCommand="grid_assignments_RowCommand"
                            OnRowDataBound="grid_assignments_RowDataBound" CssClass="grid_assignments">
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
                    <th>COMPLETED
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grid_completed" runat="server" CssClass="grid_completed" OnRowDataBound="grid_completed_RowDataBound"></asp:GridView>
                    </td>
                </tr>
            </table>

            <br />
            <br />
            <br />


            <table class="awaiting_assignments">
                <tr>
                    <th>TRAININGS
                    </th>
                </tr>
                <tr>
                    <td>
                     <asp:LinkButton ID="btn_unusual" runat="server" Text="Unusual / Emergency Training"  PostBackUrl="~/Exams/Unusual/UnusualTraining.aspx"></asp:LinkButton>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
