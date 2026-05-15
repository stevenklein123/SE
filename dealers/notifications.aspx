﻿<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="notifications.aspx.cs"
    Inherits="Project_Tracking.dealers.notifications" %>

<%@ Register Src="~/views/sidebar.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Notifications | AVON</title>
    <link href="../assets/style/global.css" rel="stylesheet" />
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
    <link href="../assets/style/dashboard.css" rel="stylesheet" />
    <style>
        .notif-page-list {
            display: flex;
            flex-direction: column;
            gap: 12px;
            max-width: 700px;
        }

        .notif-item {
            background: #fff;
            border-radius: 12px;
            padding: 16px 20px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.07);
            display: flex;
            align-items: flex-start;
            gap: 14px;
            border-left: 4px solid #ccc;
            transition: 0.2s;
        }

        .notif-item.unread {
            border-left-color: #e91e63;
            background: #fff5f8;
        }

        .notif-item.order_approved { border-left-color: #28a745; }
        .notif-item.order_rejected { border-left-color: #dc3545; }
        .notif-item.new_product    { border-left-color: #007bff; }

        .notif-icon {
            font-size: 1.6rem;
            line-height: 1;
        }

        .notif-text {
            flex: 1;
        }

        .notif-text p {
            margin: 0 0 4px;
            font-weight: 500;
            color: #222;
        }

        .notif-text small {
            color: #888;
            font-size: 0.82rem;
        }

        .unread-dot {
            width: 10px;
            height: 10px;
            background: #e91e63;
            border-radius: 50%;
            margin-top: 5px;
            flex-shrink: 0;
        }

        .btn-mark-all {
            background: #e91e63;
            color: white;
            border: none;
            border-radius: 8px;
            padding: 8px 18px;
            cursor: pointer;
            font-size: 0.9rem;
            margin-bottom: 20px;
        }

        .btn-mark-all:hover { background: #c2185b; }

        .empty-state {
            color: #aaa;
            font-size: 1rem;
            padding: 30px 0;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
<div class="dashboard-container">

    <uc:Sidebar ID="Sidebar1" runat="server" />
    <div class="sidebar-overlay"></div>

    <main class="main-content">

        <h2>🔔 Notifications</h2>

        <button type="button" class="btn-mark-all" onclick="markAllRead()">
            Mark all as read
        </button>

        <div id="notifPageList" class="notif-page-list">
            <div class="empty-state">Loading...</div>
        </div>

    </main>
</div>
</form>

<script src="../assets/script/script.js"></script>
<script src="../assets/script/notifications.js"></script>
<script>
function getIcon(type) {
    if (type === 'order_approved') return '✅';
    if (type === 'order_rejected') return '❌';
    if (type === 'new_product')    return '🛍️';
    return '🔔';
}

function loadPageNotifications() {
    fetch('notifications.aspx/GetNotifications', {
        method: 'POST',
        credentials: 'same-origin',
        headers: { 'Content-Type': 'application/json' },
        body: '{}'
    })
    .then(r => r.json())
    .then(res => {
        var list = res && res.d ? res.d : [];
        var container = document.getElementById('notifPageList');

        if (list.length === 0) {
            container.innerHTML = '<div class="empty-state">You have no notifications yet.</div>';
            return;
        }

        var html = '';
        list.forEach(function(n) {
            var unreadClass = n.isRead ? '' : 'unread';
            var dot = n.isRead ? '' : '<div class="unread-dot"></div>';
            html += `<div class="notif-item ${n.type} ${unreadClass}">
                <div class="notif-icon">${getIcon(n.type)}</div>
                <div class="notif-text">
                    <p>${n.message}</p>
                    <small>${n.date}</small>
                </div>
                ${dot}
            </div>`;
        });

        container.innerHTML = html;
    });
}

function markAllRead() {
    fetch('notifications.aspx/MarkAllRead', {
        method: 'POST',
        credentials: 'same-origin',
        headers: { 'Content-Type': 'application/json' },
        body: '{}'
    })
    .then(() => loadPageNotifications());
}

loadPageNotifications();
</script>
</body>
</html>