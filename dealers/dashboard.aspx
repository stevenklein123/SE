<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Project_Tracking.dealers.dashboard" %>
<%@ Register TagPrefix="uc"
    TagName="Sidebar"
    Src="~/views/sidebar.ascx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Dealer Portal | AVON Products</title>

    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
    <link href="../assets/style/dashboard.css" rel="stylesheet" />
    <link href="../assets/style/global.css" rel="stylesheet" />
</head>
<body>

<form id="form1" runat="server">

    <div class="dashboard-container">

        <header class="mobile-header">
            <button type="button" class="hamburger">
                <span>☰</span>
            </button>

            <div class="header-title">💄 AVON Dealer Portal</div>

            <button type="button" class="cart-icon" id="cartIcon">
                🛒 <span class="cart-badge" id="cartBadge">0</span>
            </button>
        </header>

            <uc:Sidebar ID="Sidebar1" runat="server" />

        <div class="sidebar-overlay" id="sidebarOverlay"></div>

        <main class="main-content">

            <asp:Label ID="lblWelcome" runat="server" CssClass="welcome-user"></asp:Label>

            <div class="header-text">
                <h2>AVON Product Catalog</h2>
                <p>Welcome! Browse our latest beauty products and add to your order.</p>
            </div>

            <div class="search">
                <input type="text"
                       id="searchInput"
                       placeholder="Search products (e.g. Lotion, Serum, Sweet Honesty)..." />
            </div>

            <div class="product-grid">

                <asp:Repeater ID="rptProducts" runat="server">
                    <ItemTemplate>

                        <div class="product-card">

                            <div class="product-image">
    <img src='<%# ResolveUrl(Eval("image_path").ToString()) %>'
         alt="Product Image"
         style="width:100%; height:150px; object-fit:cover; border-radius:10px;" />
</div>

                            <h3><%# Eval("product_name") %></h3>

                            <p class="sku">
                                Stocks: <%# Eval("stock") %>
                            </p>

                            <div class="price-text">
                                ₱<%# Eval("price", "{0:N2}") %>
                            </div>

                        <button type="button"
                                class="btn-add-cart"
                                onclick="addToCart(this)"
                                data-id='<%# Eval("product_id") %>'
                                data-name='<%# Eval("product_name") %>'
                                data-price='<%# Eval("price") %>'
                                data-image='<%# Eval("image_path") %>'>

                            Add to Cart
                        </button>

                        </div>

                    </ItemTemplate>
                </asp:Repeater>

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
    window.chatbaseConfig = {
        chatbotId: "bebNRvwQS_WvHNUnRo4sS",

        userId: "<%= Session["ChatSessionId"] %>",

    userName: "<%= Session["Username"] %>",
    userRole: "<%= Session["Role"] %>"
    };
</script>
<script>
    window.chatbaseConfig = {

        chatbotId: "bebNRvwQS_WvHNUnRo4sS",

        userId: "<%= Session["ChatSessionId"] %>",

    userName: "<%= Session["Username"] %>",

    userRole: "<%= Session["Role"] %>"
};

(function () {

    if (!window.chatbase ||
        window.chatbase("getState") !== "initialized") {

        window.chatbase = (...args) => {

            if (!window.chatbase.q) {
                window.chatbase.q = [];
            }

            window.chatbase.q.push(args);
        };

        window.chatbase = new Proxy(
            window.chatbase,
            {
                get(target, prop) {

                    if (prop === "q") {
                        return target.q;
                    }

                    return (...args) =>
                        target(prop, ...args);
                }
            }
        );
    }

    function loadChatbase() {

        const script =
            document.createElement("script");

        script.src =
            "https://www.chatbase.co/embed.min.js";

        script.id =
            "bebNRvwQS_WvHNUnRo4sS";

        script.domain =
            "www.chatbase.co";

        script.onload = function () {

            const token =
                "<%= Session["chat_token"] %>";

                if (token && window.chatbase) {

                    window.chatbase(
                        "identify",
                        {
                            token: token
                        }
                    );
                }
            };

            document.body.appendChild(script);
        }

        if (document.readyState === "complete") {

            loadChatbase();

        } else {

            window.addEventListener(
                "load",
                loadChatbase
            );
        }

    })();
</script>



<script src="<%= ResolveUrl("~/assets/js/script.js") %>"></script>
<script src="<%= ResolveUrl("~/assets/js/notifications.js") %>"></script>

</body>


</html>