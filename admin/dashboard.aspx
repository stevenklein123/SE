﻿<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="dashboard.aspx.cs"
    Inherits="Project_Tracking.admin.dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Professional Dashboard | AVON Admin</title>
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet">

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
        <div class="menu-item active"><a href="dashboard.aspx"><i class="bi bi-speedometer2"></i> DASHBOARD</a></div>
        <div class="menu-item"><a href="sales.aspx"><i class="bi bi-graph-up"></i> SALES ANALYTICS</a></div>
        <div class="menu-item"><a href="webpage.aspx"><i class="bi bi-box-seam"></i> INVENTORY</a></div>
        <div class="menu-item"><a href="dealers_monitoring.aspx"><i class="bi bi-people"></i> DEALERS</a></div>
        <div class="menu-item"><a href="admin_orders.aspx"><i class="bi bi-cart-check"></i> ORDERS</a></div>
        <div class="menu-item"><a href="../auth_pages/logout.aspx"><i class="bi bi-box-arrow-right"></i> LOGOUT</a></div>
    </div>
</div>

<!-- NAVBAR -->
<nav class="navbar shadow-sm px-3 d-flex justify-content-between align-items-center" style="background-color: #e91e63 !important;">
    <button type="button" class="btn" onclick="toggleSidebar()" style="color: white;">
        <i class="bi bi-list fs-3"></i>
    </button>
    <div class="d-flex align-items-center gap-3">
        <span class="d-none d-md-inline" style="color: white;">Welcome back, Admin</span>
        <div class="rounded-circle p-2" style="background: rgba(255,255,255,0.2);">
            <i class="bi bi-person-circle fs-4" style="color: white;"></i>
        </div>
    </div>
</nav>

<div class="container-fluid p-4">

    <!-- HERO SECTION -->
    <div class="card hero-card shadow-sm">
        <div class="row align-items-center">
            <div class="col-md-8">
                <h2 class="fw-bold">Orders Management</h2>
                <p class="opacity-75">
                    You have 
                    <asp:Label ID="lblPendingOrders" runat="server" Text="0" Font-Bold="true" Font-Size="Large"></asp:Label> 
                    pending orders waiting for your approval. Keep the business moving!
                </p>
                <a href="admin_orders.aspx" class="btn btn-light fw-bold px-4 py-2" style="color: #e91e63;">
                    Review Pending Orders
                </a>
            </div>
            <div class="col-md-4 text-end d-none d-md-block">
                <h1 style="font-size: 4rem; opacity: 0.5;"><i class="bi bi-cart4"></i></h1>
            </div>
        </div>
    </div>

    <!-- STATS GRID -->
    <div class="row g-4">
        <div class="col-6 col-lg-3">
            <div class="card-stat">
                <div class="icon-box bg-light-pink"><i class="bi bi-tags"></i></div>
                <div class="stat-label text-uppercase">Total Products</div>
                <div class="stat-value"><asp:Label ID="lblTotalProducts" runat="server" Text="0" /></div>
            </div>
        </div>
        <div class="col-6 col-lg-3">
            <div class="card-stat">
                <div class="icon-box bg-light-purple"><i class="bi bi-exclamation-triangle"></i></div>
                <div class="stat-label text-uppercase">Low Stock</div>
                <div class="stat-value"><asp:Label ID="lblLowStock" runat="server" Text="0" /></div>
            </div>
        </div>
        <div class="col-6 col-lg-3">
            <div class="card-stat">
                <div class="icon-box bg-light-blue"><i class="bi bi-x-octagon"></i></div>
                <div class="stat-label text-uppercase">Out of Stock</div>
                <div class="stat-value"><asp:Label ID="lblOutStock" runat="server" Text="0" /></div>
            </div>
        </div>
        <div class="col-6 col-lg-3">
            <div class="card-stat">
                <div class="icon-box bg-light-orange"><i class="bi bi-currency-dollar"></i></div>
                <div class="stat-label text-uppercase">Total Sales</div>
                <div class="stat-value">₱<asp:Label ID="lblSales" runat="server" Text="0" /></div>
            </div>
        </div>
    </div>

</div>

</form>

<script>
    function toggleSidebar() {
        document.getElementById("sidebar").classList.toggle("active");
        document.getElementById("overlay").classList.toggle("active");
    }
</script>

</body>
</html>