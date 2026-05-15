﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ranking.aspx.cs" Inherits="Project_Tracking.dealers.ranking" %>

<%@ Register Src="~/views/sidebar.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Ranking | AVON Portal</title>
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
    <link href="../assets/style/dashboard.css" rel="stylesheet" />
    <link href="../assets/style/global.css" rel="stylesheet" />
    <link href="../assets/style/ranking.css" rel="stylesheet" />
</head>

<body>

<form id="form1" runat="server">

<div class="dashboard-container">

    <header class="mobile-header">
        <button type="button" class="hamburger" id="hamburger">☰</button>
        <div class="header-title">🏆 Ranking</div>
    </header>

    <!-- SIDEBAR GLOBAL -->
    <uc:Sidebar ID="Sidebar1" runat="server" />

    <!-- OVERLAY -->
    <div class="sidebar-overlay" id="sidebarOverlay"></div>

    <!-- MAIN CONTENT -->
    <main class="main-content">

        <h2>🏆 Dealer Ranking System</h2>

        <!-- FILTER -->
        <div class="rank-filters">
            <button type="button" onclick="filterRank('All')">All</button>
            <button type="button" onclick="filterRank('Diamond')">Diamond</button>
            <button type="button" onclick="filterRank('Platinum')">Platinum</button>
            <button type="button" onclick="filterRank('Gold')">Gold</button>
            <button type="button" onclick="filterRank('Silver')">Silver</button>
            <button type="button" onclick="filterRank('Bronze')">Bronze</button>
        </div>

        <!-- SEARCH -->
        <input type="text" id="searchInput" placeholder="Search user..." onkeyup="searchUser()" />

        <!-- USERS -->
        <asp:Repeater ID="rptUsers" runat="server">
            <ItemTemplate>

                <div class="user-card" data-rank='<%# Eval("rank") %>'>

                    <h3><%# Eval("username") %></h3>
                    <p><%# Eval("email") %></p>
                    <p>₱<%# Eval("total_sales", "{0:N2}") %></p>

                    <span class="badge <%# Eval("rank") %>">
                        <%# Eval("rank") %>
                    </span>

                </div>

            </ItemTemplate>
        </asp:Repeater>

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
<script src="<%= ResolveUrl("~/assets/script/ranking.js") %>"></script>
</body>
</html>