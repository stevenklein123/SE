<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="Project_Tracking.dealers.cart" %>

<%@ Register TagPrefix="uc"
    TagName="Sidebar"
    Src="~/views/sidebar.ascx" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Your Cart | AVON Dealer Portal</title>

    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
    <link href="../assets/style/cart.css" rel="stylesheet" />
    <link href="../assets/style/global.css" rel="stylesheet" />
</head>
<body>

<form id="form1" runat="server">

    <div class="dashboard-container">

        <header class="mobile-header">
            <button type="button" class="hamburger">
                <span>☰</span>
            </button>

            <div class="header-title">🛒 My Cart</div>

            <button type="button" class="cart-icon">
                🛒 <span class="cart-badge" id="cartBadgeMobile">0</span>
            </button>
        </header>

           <uc:Sidebar ID="Sidebar1" runat="server" />

        <div class="sidebar-overlay" id="sidebarOverlay"></div>

        <main class="main-content">

            <div class="header-text">
                <h2>Shopping Cart</h2>
                <p>Review and manage your AVON items before checkout</p>
            </div>

            <div class="cart-wrapper">

                <div class="cart-section-left">

                    <div id="cartEmpty" class="cart-empty" style="display:none;">
                        <div class="cart-empty-icon">💄</div>
                        <h3>Your cart is empty</h3>
                        <p>Looks like you haven't added any beauty products yet.</p>
                        <a href="dashboard.aspx" class="btn-browse">Start Shopping</a>
                    </div>

                    <div id="cartItemsContainer" class="cart-items-list"></div>

                </div>

                <div class="cart-section-right" id="cartSummary" style="display:none;">

                    <div class="summary-card">

                        <div class="cart-actions-top">
                            <h3>Order Summary</h3>
                            <button type="button" class="btn-clear-cart" id="btnClearCart">
                                Clear All
                            </button>
                        </div>

                        <div class="summary-details">

                            <div class="summary-row">
                                <span>Subtotal</span>
                                <span class="summary-value" id="subtotal">₱0.00</span>
                            </div>

                            <div class="summary-row">
                                <span>Shipping Method</span>
                                <div class="shipping-buttons">
                                    <button type="button" class="ship-btn active" data-method="Delivery">Delivery (+₱50)</button>
                                    <button type="button" class="ship-btn" data-method="Pickup">Pickup (Free)</button>
                                    <input type="hidden" id="shippingMethod" value="Delivery" />
                                </div>
                            </div>

                            <div class="summary-row">
                                <span>Shipping Fee</span>
                                <span class="summary-value" id="shipping">₱0.00</span>
                            </div>

                            <div class="summary-row total">
                                <span>Total</span>
                                <span class="summary-value" id="total">₱0.00</span>
                            </div>

                        </div>

                        <div class="payment-methods">

                            <h4>Payment Method</h4>

                            <select id="paymentMethod">
                                <option value="GCash">GCash</option>
                                <option value="Maya">Maya</option>
                                <option value="COD">Cash on Delivery</option>
                            </select>

                        </div>

                        <asp:Button 
                            ID="btnCheckout" 
                            runat="server" 
                            Text="Complete Purchase" 
                            CssClass="btn-checkout"
                            OnClientClick="checkout(); return false;" 
                        />

                        <a href="dashboard.aspx" class="btn-continue">
                            Continue Shopping
                        </a>

                    </div>

                </div>

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
<script src="<%= ResolveUrl("~/assets/script/cart.js") %>"></script>

<div id="removeConfirmModal" class="confirm-modal" aria-hidden="true">
    <div class="confirm-modal-content">
        <h3>Remove Item</h3>
        <p>Are you sure you want to remove this item from your cart?</p>
        <div class="confirm-actions">
            <button type="button" class="btn-cancel" onclick="hideRemoveModal()">Cancel</button>
            <button type="button" class="btn-confirm-remove" onclick="confirmRemove()">Confirm Remove</button>
        </div>
    </div>
</div>

<div id="clearAllModal" class="confirm-modal" aria-hidden="true">
    <div class="confirm-modal-content">
        <h3>Clear All Items</h3>
        <p>Are you sure you want to remove all items from your cart?</p>
        <div class="confirm-actions">
            <button type="button" class="btn-cancel" onclick="hideClearModal()">Cancel</button>
            <button type="button" class="btn-confirm-remove" onclick="confirmClear()">Confirm Remove</button>
        </div>
    </div>
</div>

</body>
</html>
