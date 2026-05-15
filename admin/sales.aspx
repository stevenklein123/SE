﻿<%@ Page Language="C#" AutoEventWireup="true"
CodeBehind="sales.aspx.cs"
Inherits="Project_Tracking.admin.sales" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Sales</title>
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <style>
        :root {
            --avon-pink: #e91e63;
            --sidebar-bg: #1e1e2d;
        }

        body {
            background-color: #f8f9fa;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        /* SIDEBAR */
        .sidebar {
            position: fixed;
            top: 0;
            left: -280px;
            width: 280px;
            height: 100%;
            background: var(--sidebar-bg);
            color: #a2a3b7;
            transition: all 0.3s ease;
            z-index: 1050;
        }

        .sidebar.active { left: 0; }

        .sidebar-header {
            padding: 2rem 1.5rem;
            background: rgba(0,0,0,0.1);
            color: white;
            text-align: center;
        }

        .menu-item a {
            color: #a2a3b7;
            text-decoration: none;
            padding: 15px 25px;
            display: flex;
            align-items: center;
            gap: 12px;
            transition: 0.3s;
        }

        .menu-item a:hover, .menu-item.active a {
            background: rgba(255, 255, 255, 0.05);
            color: var(--avon-pink);
        }

        .overlay {
            position: fixed;
            inset: 0;
            background: rgba(0,0,0,0.4);
            opacity: 0;
            visibility: hidden;
            transition: 0.3s;
            z-index: 1040;
        }

        .overlay.active { opacity: 1; visibility: visible; }

        /* CARDS */
        .card-stat {
            background: white;
            border: none;
            border-radius: 12px;
            padding: 25px;
            box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
        }

        .icon-box {
            width: 48px; height: 48px; border-radius: 10px;
            display: flex; align-items: center; justify-content: center;
            font-size: 24px; margin-bottom: 15px;
        }

        .bg-light-pink { background: rgba(233, 30, 99, 0.1); color: var(--avon-pink); }
        .bg-light-purple { background: rgba(123, 97, 255, 0.1); color: #7b61ff; }
        .bg-light-blue { background: rgba(56, 182, 255, 0.1); color: #38b6ff; }
        .bg-light-orange { background: rgba(255, 174, 66, 0.1); color: #ffae42; }

        .stat-label { color: #6c757d; font-size: 0.9rem; font-weight: 600; }
        .stat-value { font-size: 1.8rem; font-weight: 700; color: #343a40; }

        .hero-card {
            background: linear-gradient(to right, #e91e63, #c2185b);
            border-radius: 15px;
            color: white;
            padding: 30px;
            border: none;
            margin-bottom: 30px;
        }
        #salesChart {
            background: white;
            padding: 20px;
            border-radius: 15px;
            box-shadow: 0 0.125rem 0.25rem rgba(0,0,0,0.08);
            margin-bottom: 30px;
        }
    </style>
</head>

<body>

<form id="form1" runat="server">

<div class="overlay" id="overlay" onclick="toggleSidebar()"></div>

<!-- SIDEBAR -->
<div class="sidebar" id="sidebar">
    <div class="sidebar-header">
        <h4 class="mb-0 fw-bold" style="letter-spacing: 2px;">AVON ADMIN</h4>
    </div>
    <div class="mt-3">
        <div class="menu-item"><a href="dashboard.aspx"><i class="bi bi-speedometer2"></i> DASHBOARD</a></div>
        <div class="menu-item active"><a href="sales.aspx"><i class="bi bi-graph-up"></i> SALES ANALYTICS</a></div>
        <div class="menu-item"><a href="webpage.aspx"><i class="bi bi-box-seam"></i> INVENTORY</a></div>
        <div class="menu-item"><a href="dealers_monitoring.aspx"><i class="bi bi-people"></i> DEALERS</a></div>
        <div class="menu-item"><a href="admin_orders.aspx"><i class="bi bi-cart-check"></i> ORDERS</a></div>
        <div class="menu-item"><a href="../auth_pages/logout.aspx"><i class="bi bi-box-arrow-right"></i> LOGOUT</a></div>
    </div>
</div>

<!-- NAVBAR -->
<nav class="navbar shadow-sm px-3" style="background-color: #e91e63 !important;">
    <button type="button" class="btn" onclick="toggleSidebar()" style="color: white;">
        <i class="bi bi-list fs-3"></i>
    </button>
    <span class="fw-bold" style="color: white;">SALES ANALYTICS</span>
</nav>

<div class="container mt-4">

    <!-- FILTERS (FIXED: NOW SERVER CONTROLS) -->
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
            <asp:Button ID="btnFilter" runat="server"
                Text="Apply Filter"
                CssClass="btn btn-danger w-100"
                OnClick="btnFilter_Click" />
        </div>

    </div>

    <!-- TOTAL -->
    <div class="alert alert-success">
        Total Revenue:
        ₱ <asp:Label ID="lblTotalRevenue" runat="server" Text="0"></asp:Label>
    </div>

    <!-- CHART -->
    <canvas id="salesChart">
        
    </canvas>

    <hr />

    <!-- GRID -->
    <asp:GridView ID="gvSales" runat="server"
        CssClass="table table-bordered" />

</div>

</form>

<script>

    function toggleSidebar() {
        document.getElementById("sidebar").classList.toggle("active");
        document.getElementById("overlay").classList.toggle("active");
    }

    var labels = <%= chartLabels %>;
    var data = <%= chartData %>;

    const ctx = document.getElementById("salesChart");

    if (window.salesChartInstance) {
        window.salesChartInstance.destroy();
    }

    window.salesChartInstance = new Chart(ctx, {

        type: 'line',

        data: {
            labels: labels,

            datasets: [{
                label: 'Sales Revenue',
                data: data,
                borderColor: '#e91e63',
                backgroundColor: 'rgba(233, 30, 99, 0.15)',
                fill: true,
                tension: 0.4,
                borderWidth: 3,
                pointRadius: 5
            }]
        },

        options: {
            responsive: true,

            plugins: {
                legend: {
                    display: true
                }
            },

            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }

    });

</script>
</body>
</html>