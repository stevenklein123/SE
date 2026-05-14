l<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orders.aspx.cs" Inherits="Project_Tracking.dealers.orders" %>

<%@ Register Src="~/views/sidebar.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Order History | Dealer Portal</title>

    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
    <link href="../assets/style/orders.css" rel="stylesheet" />
    <link href="../assets/style/global.css" rel="stylesheet" />
</head>
<body>

<form id="form1" runat="server">

    <div class="dashboard-container">

        <header class="mobile-header">
            <button type="button" class="hamburger">
                <span>☰</span>
            </button>

            <div class="header-title">📋 Orders</div>

            <button type="button" class="cart-icon" id="cartIcon">
                🛒 <span class="cart-badge" id="cartBadge">0</span>
            </button>
        </header>

        <uc:Sidebar ID="Sidebar1" runat="server" />

        <div class="sidebar-overlay" id="sidebarOverlay"></div>


        <main class="main-content">

            <div class="header-text">
                <h2>Order History</h2>
                <p>Manage and track the status of your past purchases</p>
            </div>

            <div class="orders-wrapper">

                <div class="filter-bar">
                    <button type="button" class="filter-btn active" data-filter="all">All</button>
                    <button type="button" class="filter-btn" data-filter="pending">Pending</button>
                    <button type="button" class="filter-btn" data-filter="processing">Processing</button>
                    <button type="button" class="filter-btn" data-filter="shipped">Shipped</button>
                    <button type="button" class="filter-btn" data-filter="delivered">Delivered</button>
                </div>

                <div id="ordersEmpty" class="orders-empty" style="display:none;">
                    <div class="orders-empty-icon">📋</div>
                    <h3>No orders found</h3>
                    <p>You haven't placed any orders in this category yet.</p>

                    <a href="dashboard.aspx" class="btn-shop-now">
                        Start Shopping
                    </a>
                </div>

                <div id="ordersList" class="orders-list"></div>

            </div>

        </main>

    </div>

</form>

<div id="notifModal" class="notif-modal">
    <div class="notif-content">

        <div class="notif-header">
            <h3>Notifications</h3>
            <button type="button" id="closeNotif">&times;</button>
        </div>

        <div id="notifList" class="notif-list"></div>

    </div>
</div>
<script>

    // CLEAR OLD CHAT CACHE
    localStorage.removeItem("chatbase-session");

    sessionStorage.removeItem("chatbase-session");

    // CHATBASE CONFIG
    window.chatbaseConfig = {
        chatbotId: "tARzRJPxLzrHZlYqWgswv",
        domain: "www.chatbase.co"
    };

    (function () {

        var script =
            document.createElement("script");

        script.src =
            "https://www.chatbase.co/embed.min.js";

        script.defer = true;

        script.onload = function () {

            setTimeout(function () {

                // IDENTIFY CURRENT USER
                window.chatbase("identify", {
                    token: '<%= Session["chat_token"] %>'
                });

                // FORCE NEW CONVERSATION
                window.chatbase("newConversation");

            }, 1500);

        };

        document.body.appendChild(script);

    })();

</script>
<script src="<%= ResolveUrl("~/assets/script/script.js") %>"></script>
<script src="<%= ResolveUrl("~/assets/script/notifications.js") %>"></script>
<script src="<%= ResolveUrl("~/assets/script/orders.js") %>"></script>
</body>
</html>
