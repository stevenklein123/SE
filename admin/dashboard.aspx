<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="dashboard.aspx.cs"
    Inherits="Project_Tracking.admin.dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>AVON Admin Dashboard</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">

    <!-- Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet">

    <!-- CSS -->
    <link rel="stylesheet" href="../assets/style/global_desktop.css">
    <link rel="stylesheet" href="../assets/style/global_mobile.css">

    <style>

        body {
            background: #f5f5f5;
        }

        .card-custom {
            border: none;
            border-radius: 18px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.08);
        }

        .stats-card {
            border-radius: 16px;
            padding: 18px;
            color: white;
        }

        .pink { background: linear-gradient(135deg,#ff4f87,#e4004b); }
        .purple { background: linear-gradient(135deg,#7b61ff,#5c43f5); }
        .blue { background: linear-gradient(135deg,#38b6ff,#0077ff); }
        .orange { background: linear-gradient(135deg,#ffae42,#ff7b00); }

    </style>

</head>

<body>

<form id="form1" runat="server">

    <!-- OVERLAY -->
    <div class="overlay" id="overlay"></div>

    <!-- SIDEBAR -->
    <div class="sidebar" id="sidebar">

        <div class="d-flex justify-content-between align-items-center mb-3">
            <h5 style="cursor:pointer;" onclick="toggleSidebar()">MENU</h5>
            <button type="button" class="btn text-white" onclick="toggleSidebar()">
                <i class="bi bi-x-lg"></i>
            </button>
        </div>

        <div class="menu-item"><a href="dashboard.aspx">DASHBOARD</a></div>
        <div class="menu-item"><a href="sales.aspx">SALES</a></div>
        <div class="menu-item"><a href="webpage.aspx">INVENTORY</a></div>
        <div class="menu-item"><a href="dealers.aspx">DEALERS</a></div>

    </div>

    <!-- NAVBAR -->
    <nav class="navbar navbar-dark px-3" style="background:#e4004b;">
        <button type="button" class="btn text-white" onclick="toggleSidebar()">
            <i class="bi bi-list fs-3"></i>
        </button>

        <span class="mx-auto fw-bold fs-4">AVON ADMIN</span>

        <i class="bi bi-bell fs-4 text-white"></i>
    </nav>
            <div class="card card-custom p-3 mt-4">

            <h5>Welcome Admin 👋</h5>
        </div>
    <!-- CONTENT -->
    <div class="container-fluid mt-3">

        <!-- STATS -->
        <div class="row g-3">

            <div class="col-6 col-lg-3">
                <div class="stats-card pink">
                    <h6>Total Products</h6>
                    <h3>
                        <asp:Label ID="lblTotalProducts" runat="server" Text="0"></asp:Label>
                    </h3>
                </div>
            </div>

            <div class="col-6 col-lg-3">
                <div class="stats-card purple">
                    <h6>Low Stock</h6>
                    <h3>
                        <asp:Label ID="lblLowStock" runat="server" Text="0"></asp:Label>
                    </h3>
                </div>
            </div>

            <div class="col-6 col-lg-3">
                <div class="stats-card blue">
                    <h6>Out of Stock</h6>
                    <h3>
                        <asp:Label ID="lblOutStock" runat="server" Text="0"></asp:Label>
                    </h3>
                </div>
            </div>

            <div class="col-6 col-lg-3">
                <div class="stats-card orange">
                    <h6>Total Sales</h6>
                    <h3>
                        ₱<asp:Label ID="lblSales" runat="server" Text="0"></asp:Label>
                    </h3>
                </div>
            </div>

        </div>

        <!-- PLACEHOLDER SECTION -->


    </div>

</form>

<script>

    function toggleSidebar() {
        document.getElementById("sidebar").classList.toggle("active");
        document.getElementById("overlay").classList.toggle("active");
    }

    document.getElementById("overlay").addEventListener("click", toggleSidebar);

</script>

</body>
</html>