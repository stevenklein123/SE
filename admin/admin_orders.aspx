<%@ Page Language="C#" AutoEventWireup="true"
CodeBehind="admin_orders.aspx.cs"
Inherits="Project_Tracking.admin.admin_orders" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Orders | AVON Admin</title>

    <link rel="icon" type="image/png" href="../assets/images/avon.png" />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet" />

    <style>

        :root {
            --avon-pink: #e91e63;
            --sidebar-bg: #1e1e2d;
        }

        body {
            background-color: #f8f9fa;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

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

        .menu-item a:hover,
        .menu-item.active a {
            background: rgba(255,255,255,0.05);
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

        .overlay.active {
            opacity: 1;
            visibility: visible;
        }

        .status-pending {
            background: #fff3cd;
            color: #856404;
            padding: 6px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
        }

        .status-approved {
            background: #d4edda;
            color: #155724;
            padding: 6px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
        }

        .status-rejected {
            background: #f8d7da;
            color: #721c24;
            padding: 6px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
        }

        .table thead th {
            white-space: nowrap;
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

<!-- OVERLAY -->
<div class="overlay" id="overlay" onclick="toggleSidebar()"></div>

<!-- SIDEBAR -->
<div class="sidebar" id="sidebar">

    <div class="sidebar-header">
        <h4 class="mb-0 fw-bold" style="letter-spacing:2px;">AVON ADMIN</h4>
    </div>

    <div class="mt-3">
        <div class="menu-item"><a href="dashboard.aspx"><i class="bi bi-speedometer2"></i> DASHBOARD</a></div>
        <div class="menu-item"><a href="sales.aspx"><i class="bi bi-graph-up"></i> SALES ANALYTICS</a></div>
        <div class="menu-item"><a href="webpage.aspx"><i class="bi bi-box-seam"></i> INVENTORY</a></div>
        <div class="menu-item"><a href="dealers_monitoring.aspx"><i class="bi bi-people"></i> DEALERS</a></div>
        <div class="menu-item active"><a href="admin_orders.aspx"><i class="bi bi-cart-check"></i> ORDERS</a></div>
        <div class="menu-item"><a href="../auth_pages/logout.aspx"><i class="bi bi-box-arrow-right"></i> LOGOUT</a></div>
    </div>

</div>

<!-- NAVBAR -->
<nav class="navbar shadow-sm px-3 d-flex justify-content-between align-items-center"
     style="background-color:#e91e63;">

    <button type="button" class="btn" onclick="toggleSidebar()" style="color:white;">
        <i class="bi bi-list fs-3"></i>
    </button>

    <div class="d-flex align-items-center gap-3">
        <span class="d-none d-md-inline text-white fw-semibold">
            Orders Management
        </span>
        <div class="rounded-circle p-2" style="background:rgba(255,255,255,0.2);">
            <i class="bi bi-cart-check fs-4 text-white"></i>
        </div>
    </div>

</nav>

<!-- STATUS MESSAGE BELOW NAVBAR -->
<div id="statusMessage" runat="server" visible="false"
     class="alert mx-4 mt-3 mb-0" role="alert">
    <asp:Literal ID="lblMessage" runat="server" />
</div>

<!-- MAIN CONTENT -->
<div class="container-fluid p-4">

    <div class="card shadow-lg border-0 rounded-4">

        <div class="card-body">

            <div class="d-flex justify-content-between align-items-center mb-4 flex-wrap gap-2">

                <h2>
                    <i class="bi bi-cart-check-fill text-primary"></i>
                    Orders Management
                </h2>

                <div class="d-flex gap-2">
                    <asp:TextBox ID="txtSearch" runat="server"
                        CssClass="form-control"
                        placeholder="Search Reference No" />
                    <asp:Button ID="btnSearch" runat="server"
                        Text="Search"
                        CssClass="btn btn-primary"
                        OnClick="btnSearch_Click" />
                </div>

            </div>

            <div class="mb-3 d-flex gap-2 flex-wrap">
                <asp:Button ID="btnPending" runat="server" Text="Pending"
                    CssClass="btn btn-warning" OnClick="FilterPending" />
                <asp:Button ID="btnApproved" runat="server" Text="Approved"
                    CssClass="btn btn-success" OnClick="FilterApproved" />
                <asp:Button ID="btnRejected" runat="server" Text="Rejected"
                    CssClass="btn btn-danger" OnClick="FilterRejected" />
            </div>

            <div class="table-responsive">

                <asp:GridView ID="gvOrders" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-hover align-middle"
                    GridLines="None"
                    OnRowCommand="gvOrders_RowCommand">

                    <HeaderStyle CssClass="table-dark" />

                    <Columns>
                        <asp:BoundField DataField="order_id"       HeaderText="Order ID" />
                        <asp:BoundField DataField="reference_no"   HeaderText="Reference No" />
                        <asp:BoundField DataField="username"       HeaderText="Customer" />
                        <asp:BoundField DataField="item_count"     HeaderText="Items" />
                        <asp:BoundField DataField="shipping_method" HeaderText="Shipping" />
                        <asp:BoundField DataField="total_amount"   HeaderText="Total" />
                        <asp:BoundField DataField="order_date"     HeaderText="Date" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# GetStatusClass(Eval("status") == null ? "" : Eval("status").ToString()) %>'>
                                    <%# Eval("status") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server"
                                    CommandName="ApproveOrder"
                                    CommandArgument='<%# Eval("order_id") %>'
                                    CssClass="btn btn-success btn-sm"
                                    Visible='<%# Eval("status") != null && Eval("status").ToString() == "Pending" %>'>
                                    Approve
                                </asp:LinkButton>
                                <asp:LinkButton runat="server"
                                    CommandName="RejectOrder"
                                    CommandArgument='<%# Eval("order_id") %>'
                                    CssClass="btn btn-danger btn-sm ms-2"
                                    Visible='<%# Eval("status") != null && Eval("status").ToString() == "Pending" %>'>
                                    Reject
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </div>

</div>

</form>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

<script>
    function toggleSidebar() {
        document.getElementById("sidebar").classList.toggle("active");
        document.getElementById("overlay").classList.toggle("active");
    }
</script>

</body>
</html>