<%@ Page Language="C#" AutoEventWireup="true"
CodeBehind="webpage.aspx.cs"
Inherits="Project_Tracking.admin.webpage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Inventory | AVON Admin</title>
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
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
</style>
</head>

<body>

<form id="form1" runat="server">

<div class="overlay" id="overlay" onclick="toggleSidebar()"></div>
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
    </div>
</div>

<!-- NAV -->
<nav class="navbar bg-white shadow-sm px-3">
    <button type="button" class="btn" onclick="toggleSidebar()">
        <i class="bi bi-list fs-3"></i>
    </button>
    <h5 class="mb-0">Inventory</h5>

    <asp:Button ID="btnViewProduct" runat="server"
        Text="Refresh"
        CssClass="btn btn-outline-secondary btn-sm"
        OnClick="btnViewProduct_Click" />
</nav>

<div class="container-fluid p-4">
    <div class="row">

        <!-- LEFT PANEL -->
        <div class="col-md-4">

            <!-- ADD -->
            <div class="card admin-card p-3 mb-3">
                <h6>Add Product</h6>

                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control mb-2" placeholder="Name" />
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control mb-2" placeholder="Price" />
                <asp:TextBox ID="txtStock" runat="server" CssClass="form-control mb-2" placeholder="Stock" />
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control mb-2" TextMode="MultiLine" />

                <asp:FileUpload ID="fuProductImage" runat="server" CssClass="form-control mb-3" />

                <asp:Button ID="btnSaveProduct" runat="server"
                    Text="Save"
                    CssClass="btn btn-pink w-100"
                    OnClick="btnSaveProduct_Click" />
            </div>

            <!-- UPDATE -->
            <div class="card admin-card p-3 mb-3">
                <h6>Update Product</h6>

                <asp:TextBox ID="txtUpdateProductID" runat="server" CssClass="form-control mb-2" placeholder="ID" />
                <asp:TextBox ID="txtUpdateName" runat="server" CssClass="form-control mb-2" placeholder="Name" />
                <asp:TextBox ID="txtUpdatePrice" runat="server" CssClass="form-control mb-2" placeholder="Price" />
                <asp:TextBox ID="txtUpdateStock" runat="server" CssClass="form-control mb-2" placeholder="Stock" />
                <asp:TextBox ID="txtUpdateDescription" runat="server" CssClass="form-control mb-2" placeholder="Description" />

                <asp:Button ID="btnSaveUpdate" runat="server"
                    Text="Update"
                    CssClass="btn btn-warning w-100"
                    OnClick="btnSaveUpdate_Click" />
            </div>

            <!-- DELETE -->
            <div class="card admin-card p-3">
                <h6>Delete</h6>

                <div class="input-group">
                    <asp:TextBox ID="txtProductID" runat="server" CssClass="form-control" placeholder="ID" />
                    <asp:Button ID="btnDeleteProduct" runat="server"
                        Text="Delete"
                        CssClass="btn btn-dark"
                        OnClick="btnDeleteProduct_Click" />
                </div>
            </div>

        </div>

        <!-- RIGHT PANEL -->
        <div class="col-md-8">

            <div class="card admin-card p-3">
                <h6>Inventory List</h6>

                <asp:Label ID="lblTotalCount" runat="server" Text="0" CssClass="badge bg-dark mb-2" />

                <div class="table-responsive">

                    <!-- FIXED GRIDVIEW -->
<asp:GridView ID="GridView1" runat="server"
    CssClass="table table-hover"
    AutoGenerateColumns="False">

    <Columns>

        <asp:BoundField DataField="product_id" HeaderText="ID" />
        <asp:BoundField DataField="product_name" HeaderText="Name" />
        <asp:BoundField DataField="price" HeaderText="Price" />
        <asp:BoundField DataField="stock" HeaderText="Stock" />
        <asp:BoundField DataField="description" HeaderText="Description" />
        <asp:BoundField DataField="created_at" HeaderText="Date Created" />

        <asp:TemplateField HeaderText="Image">
            <ItemTemplate>
                <asp:Image runat="server"
                    ImageUrl='<%# ResolveUrl(Eval("image_path").ToString()) %>'
                    Width="60px" Height="60px"
                    CssClass="rounded shadow-sm" />
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>

</asp:GridView>

                </div>
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