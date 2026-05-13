<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sidebar.ascx.cs" Inherits="Project_Tracking.sidebar" %>

<div class="mobile-header">
    <button type="button" id="hamburger" class="hamburger-btn">☰</button>
    <div class="brand-section">💄 AVON Portal</div>
    <div style="width:28px"></div>
</div>

<aside class="sidebar" id="sidebar">

    <div class="sidebar-header">
        <div class="brand-section">💄 AVON Portal</div>
        <button type="button" class="close-btn" id="closeSidebar">&times;</button>
    </div>

    <nav class="nav-menu">

        <a href="/dealers/dashboard.aspx" class="nav-item">🏠 Products</a>
        <a href="/dealers/cart.aspx" class="nav-item">🛒 Cart</a>
        <a href="/dealers/transaction.aspx" class="nav-item">💳 Transactions</a>
        <a href="/dealers/orders.aspx" class="nav-item">📄 Orders</a>
        <a href="/dealers/ranking.aspx" class="nav-item">🏆 Ranking</a>
        <a href="/dealers/benefits.aspx" class="nav-item">📈 Benefits</a>
        <a href="javascript:void(0)" class="nav-item" id="notificationItem">🔔 Notifications</a>
        <a href="/auth_pages/logout.aspx" class="nav-item">🚪 Logout</a>

        <div class="sidebar-footer">
        <div class="nav-item" id="settingsItem" style="cursor:pointer; user-select:none;">
            ⚙️ Settings
        </div>

            <div id="settingsDropdown" class="settings-dropdown">
                <a href="/views/personal_info.aspx" class="nav-item">Personal Information</a>
                <a href="/auth_pages/change_password.aspx" class="nav-item">Change Password</a>
            </div>

        </div>

    </nav>
</aside>

<div id="sidebarOverlay" class="sidebar-overlay"></div>

<script src="<%= ResolveUrl("../assets/js/sidebar.js") %>"></script>