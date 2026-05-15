﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sidebar.ascx.cs" Inherits="Project_Tracking.sidebar" %>

<div class="mobile-header">
    <button type="button" id="hamburger" class="hamburger-btn">☰</button>
    <div class="brand-section">💄 AVON Portal</div>
    <div style="width:28px"></div>
</div>
<style>
    .settings-dropdown {
        display: none;
        flex-direction: column;
        margin-left: 15px;
        margin-top: 8px;
        gap: 6px;
    }

    .settings-dropdown.active {
        display: flex;
    }

    .dropdown-item {
        padding: 10px;
        border-radius: 8px;
        text-decoration: none;
        color: white;
        background: rgba(255,255,255,0.08);
    }

    .dropdown-item:hover {
        background: rgba(255,255,255,0.15);
    }

    .settings-toggle {
        display: flex;
        justify-content: space-between;
        align-items: center;
        cursor: pointer;
    }
</style>
<aside class="sidebar" id="sidebar">

    <div class="sidebar-header">
        <div class="brand-section">💄 AVON Portal</div>
        <button type="button" class="close-btn" id="closeSidebar">&times;</button>
    </div>

    <nav class="nav-menu">

        <a href="/dealers/dashboard.aspx" class="nav-item">🏠 Products</a>
        <a href="/dealers/cart.aspx" class="nav-item">🛒 Cart</a>
        <a href="/dealers/transactions.aspx" class="nav-item">💳 Transactions</a>
        <a href="/dealers/orders.aspx" class="nav-item">📄 Orders</a>
        <a href="/dealers/ranking.aspx" class="nav-item">🏆 Ranking</a>
        <a href="/dealers/benefits.aspx" class="nav-item">📈 Benefits</a>
        <a href="/dealers/notifications.aspx" class="nav-item" id="notificationItem">🔔 Notifications
    <span id="sidebarNotifBadge" style="
        background:#e91e63;
        color:#fff;
        border-radius:50%;
        padding:1px 7px;
        font-size:0.75rem;
        margin-left:6px;
        display:none;">
    </span>
</a>

        <a href="/auth_pages/logout.aspx" class="nav-item">🚪 Logout</a>

        <div class="sidebar-footer">

            <div class="nav-item settings-toggle" id="settingsItem">
                <span>⚙️ Settings</span>
                <span id="settingsArrow">▼</span>
            </div>

            <div id="settingsDropdown" class="settings-dropdown">
                <a href="/views/personal_info.aspx" class="dropdown-item">Personal Information
                </a>

                <a href="/auth_pages/change_password.aspx" class="dropdown-item">Change Password
                </a>
            </div>

        </div>

    </nav>
    <script src="<%= ResolveUrl("../assets/script/sidebar.js") %>"></script>

    <script>
        fetch('/dealers/notifications.aspx/GetUnreadCount', {
            method: 'POST',
            credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json' },
            body: '{}'
        })
            .then(r => r.json())
            .then(res => {
                var count = res && res.d ? res.d : 0;
                var badge = document.getElementById('sidebarNotifBadge');
                if (badge && count > 0) {
                    badge.textContent = count;
                    badge.style.display = 'inline';
                }
            })
            .catch(function () { });
    </script>
</aside>

<div id="sidebarOverlay" class="sidebar-overlay"></div>