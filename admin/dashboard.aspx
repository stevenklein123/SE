<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Project_Tracking.admin.dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initia603l-scale=1">
    <title>AVON Admin Inventory</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">

    <!-- Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet">

    <!-- GLOBAL CSS -->
    <link rel="stylesheet" href="../assets/style/global_desktop.css">
    <link rel="stylesheet" href="../assets/style/global_mobile.css">

    <style>
        .card-custom {
            border: none;
            border-radius: 18px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
        }

        .stats-card {
            border-radius: 16px;
            padding: 18px;
            color: white;
        }

        .pink {
            background: linear-gradient(135deg, #ff4f87, #e4004b);
        }

        .purple {
            background: linear-gradient(135deg, #7b61ff, #5c43f5);
        }

        .blue {
            background: linear-gradient(135deg, #38b6ff, #0077ff);
        }

        .orange {
            background: linear-gradient(135deg, #ffae42, #ff7b00);
        }

        .product-img-box {
            width: 100%;
            height: 180px;
            background: #eee;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
        }

        .product-img-box img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

        .badge-soft {
            padding: 5px 10px;
            border-radius: 20px;
            font-size: 12px;
        }

        .soft-green {
            background: #d1ffe3;
            color: #008f4f;
        }

        .soft-yellow {
            background: #fff2c7;
            color: #b78100;
        }
    </style>
</head>

<body>

    <!-- OVERLAY -->
    <div class="overlay" id="overlay"></div>

    <!-- SIDEBAR -->
    <div class="sidebar" id="sidebar">
        <div class="sub-menu d-flex justify-content-between">
            <h5 onclick="toggleSidebar()" style="cursor:pointer;">MENU</h5>
            <button class="btn text-reset" onclick="toggleSidebar()">
                <i class="bi bi-list fs-3"></i>
            </button>
        </div>

        <div class="menu-item"><a href="/webpage/dashboard.html">DASHBOARD</a></div>
        <div class="menu-item"><a href="../webpage/RecordSales.aspx">SALES</a></div>
        <div class="menu-item"><a href="../admin/inventory.aspx">INVENTORY</a></div>
        <div class="menu-item"><a href="/webpage/dealers.html">DEALERS</a></div>
    </div>

    <!-- NAVBAR -->
    <nav class="navbar navbar-custom px-3">
        <button class="btn text-white" onclick="toggleSidebar()">
            <i class="bi bi-list fs-3"></i>
        </button>

        <span class="mx-auto fw-bold fs-4">AVON ADMIN</span>

        <i class="bi bi-bell fs-4 text-white"></i>
    </nav>

    <!-- CONTENT -->
    <div class="content container-fluid">

        <!-- STATS -->
        <div class="row g-3 mt-2">
            <div class="col-6 col-lg-3">
                <div class="stats-card pink">
                    <h6>Total Products</h6>
                    <h3>1250</h3>
                </div>
            </div>

            <div class="col-6 col-lg-3">
                <div class="stats-card purple">
                    <h6>Low Stock</h6>
                    <h3>25</h3>
                </div>
            </div>

            <div class="col-6 col-lg-3">
                <div class="stats-card blue">
                    <h6>Out of Stock</h6>
                    <h3>8</h3>
                </div>
            </div>

            <div class="col-6 col-lg-3">
                <div class="stats-card orange">
                    <h6>Sales</h6>
                    <h3>₱48K</h3>
                </div>
            </div>
        </div>

        <!-- MAIN SECTION -->
        <div class="row g-4 mt-3">

            <!-- ADD / EDIT PRODUCT -->
            <div class="col-lg-4">
                <div class="card card-custom p-3">
                    <h5 class="mb-3">Add / Edit Product</h5>

                    <div class="product-img-box mb-2" id="imgPreviewBox">
                        <span class="text-muted" id="imgPlaceholder">Product Image</span>
                        <img id="previewImg" style="display:none;" />
                    </div>

                    <input type="file" class="form-control mb-2" id="imageInput" accept="image/*">
                    <input type="text" class="form-control mb-2" placeholder="Product Name">
                    <textarea class="form-control mb-2" placeholder="Description"></textarea>
                    <input type="number" class="form-control mb-2" placeholder="Price">
                    <input type="number" class="form-control mb-3" placeholder="Stock">

                    <button class="btn btn-danger w-100 mb-2">Save Product</button>
                    <button class="btn btn-outline-secondary w-100">Clear</button>
                </div>
            </div>

            <!-- PRODUCT LIST -->
            <div class="col-lg-8">
                <div class="card card-custom p-3">
                    <h5 class="mb-3">Products</h5>

                    <div class="d-flex border-bottom py-2 align-items-center gap-3">
                        <div style="width:60px;height:60px;background:#eee;border-radius:10px;"></div>
                        <div class="flex-grow-1">
                            <h6 class="mb-0">AVON Lipstick</h6>
                            <small class="text-muted">Matte finish lipstick</small>
                        </div>
                        <span class="badge-soft soft-green">Active</span>
                        <button class="btn btn-sm btn-outline-primary"><i class="bi bi-pencil"></i></button>i
                        <button class="btn btn-sm btn-outline-danger"><i class="bi bi-trash"></i></button>
                    </div>

                    <div class="d-flex border-bottom py-2 align-items-center gap-3">
                        <div style="width:60px;height:60px;background:#eee;border-radius:10px;"></div>
                        <div class="flex-grow-1">
                            <h6 class="mb-0">AVON Perfume</h6>
                            <small class="text-muted">Floral scent</small>
                        </div>
                        <span class="badge-soft soft-yellow">Low</span>
                        <button class="btn btn-sm btn-outline-primary"><i class="bi bi-pencil"></i></button>
                        <button class="btn btn-sm btn-outline-danger"><i class="bi bi-trash"></i></button>
                    </div>
                </div>
            </div>
        </div>

        <!-- LOWER SECTION -->
        <div class="row g-4 mt-4">

            <!-- NOTIFICATIONS -->
            <div class="col-lg-5">
                <div class="card card-custom p-3">
                    <h5>Notifications</h5>

                    <div class="border-bottom py-2">
                        <strong>Low Stock Alert</strong>
                        <p class="mb-0 text-muted small">Perfume only has 5 left</p>
                    </div>

                    <div class="border-bottom py-2">
                        <strong>New Order</strong>
                        <p class="mb-0 text-muted small">Maria placed an order</p>
                    </div>

                    <div class="py-2">
                        <strong>Payment Reminder</strong>
                        <p class="mb-0 text-muted small">Due in 2 days</p>
                    </div>
                </div>
            </div>

            <!-- PENDING ORDERS -->
            <div class="col-lg-7">
                <div class="card card-custom p-3">
                    <h5 class="mb-3">Pending Dealer Orders</h5>

                    <table class="table align-middle">
                        <thead>
                            <tr>
                                <th>Dealer</th>
                                <th>Product</th>
                                <th>Qty</th>
                                <th>Status</th>
                                <th></th>
                            </tr>
                        </thead>

                        <tbody>
                            <tr>
                                <td>Maria Santos</td>
                                <td>Lipstick</td>
                                <td>12</td>
                                <td><span class="badge bg-warning text-dark">Pending</span></td>
                                <td>
                                    <button class="btn btn-success btn-sm">Approve</button>
                                    <button class="btn btn-danger btn-sm">Reject</button>
                                </td>
                            </tr>

                            <tr>
                                <td>Angela Reyes</td>
                                <td>Perfume</td>
                                <td>5</td>
                                <td><span class="badge bg-warning text-dark">Pending</span></td>
                                <td>
                                    <button class="btn btn-success btn-sm">Approve</button>
                                    <button class="btn btn-danger btn-sm">Reject</button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

        </div>
    </div>

    <!-- MOBILE NAV -->
    <div class="mobile-nav d-flex d-md-none">
        <a href="#" class="nav-item"><i class="bi bi-house"></i><span>Dashboard</span></a>
        <a href="#" class="nav-item"><i class="bi bi-graph-up"></i><span>Sales</span></a>
        <a href="#" class="nav-item active"><i class="bi bi-box"></i><span>Inventory</span></a>
        <a href="#" class="nav-item"><i class="bi bi-people"></i><span>Dealers</span></a>
    </div>

    <script src="../assets/script/menu.js"></script>

    <script>
        document.getElementById("imageInput").addEventListener("change", function (event) {
            const file = event.target.files[0];
            const previewImg = document.getElementById("previewImg");
            const placeholder = document.getElementById("imgPlaceholder");

            if (file) {
                const reader = new FileReader();

                reader.onload = function (e) {
                    previewImg.src = e.target.result;
                    previewImg.style.display = "block";
                    placeholder.style.display = "none";
                }

                reader.readAsDataURL(file);
            } else {
                previewImg.style.display = "none";
                placeholder.style.display = "block";
            }
        });
    </script>

</body>

</html>

