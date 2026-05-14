<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="sales.aspx.cs"
    Inherits="Project_Tracking.admin.sales" %>

<!DOCTYPE html>
<html>
<head runat="server">

    <title>Sales Analytics</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

</head>

<body>

<form id="form1" runat="server">

<div class="container mt-4">

    <h3>Sales Analytics Dashboard</h3>

    <!-- FILTERS -->
    <div class="row mb-3">

        <div class="col-md-4">
            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control">
                <asp:ListItem Text="All Months" Value="" />
                <asp:ListItem Text="January" Value="1" />
                <asp:ListItem Text="February" Value="2" />
                <asp:ListItem Text="March" Value="3" />
                <asp:ListItem Text="April" Value="4" />
                <asp:ListItem Text="May" Value="5" />
                <asp:ListItem Text="June" Value="6" />
                <asp:ListItem Text="July" Value="7" />
                <asp:ListItem Text="August" Value="8" />
                <asp:ListItem Text="September" Value="9" />
                <asp:ListItem Text="October" Value="10" />
                <asp:ListItem Text="November" Value="11" />
                <asp:ListItem Text="December" Value="12" />
            </asp:DropDownList>
        </div>

        <div class="col-md-4">
            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>

        <div class="col-md-4">
            <asp:Button ID="btnFilter" runat="server" Text="Apply Filter"
                CssClass="btn btn-primary w-100"
                OnClick="btnFilter_Click" />
        </div>

    </div>

    <!-- TOTAL REVENUE -->
    <div class="alert alert-success">
        Total Revenue:
        ₱ <asp:Label ID="lblTotalRevenue" runat="server" Text="0"></asp:Label>
    </div>

    <!-- CHART -->
    <canvas id="salesChart" height="120"></canvas>

    <hr />

    <!-- GRID -->
    <asp:GridView ID="gvSales" runat="server"
        AutoGenerateColumns="True"
        CssClass="table table-bordered" />

</div>

</form>

<script>

    var labels = [<%= chartLabels %>];
    var data = [<%= chartData %>];

    new Chart(document.getElementById("salesChart"), {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Revenue',
                data: data,
                borderWidth: 2
            }]
        }
    });

</script>

</body>
</html>