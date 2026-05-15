<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="transactions.aspx.cs" Inherits="Project_Tracking.dealers.transactions" %>

<%@ Register Src="~/views/sidebar.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Dealer Portal | AVON Transactions</title>
    <link href="../assets/style/global.css" rel="stylesheet" />
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
    <link href="../assets/style/global.css" rel="stylesheet" />
    <link href="../assets/style/dashboard.css" rel="stylesheet" />
    <%-- ADD THIS --%>
    <style>
        #transactionsList {
            display: flex;
            flex-wrap: wrap;
            gap: 16px;
        }

        .transaction-card {
            flex: 1 1 280px;
            max-width: 100%;
            box-sizing: border-box;
            min-width: 0;
        }

        .transaction-card h3 {
            word-break: break-all;
            font-size: 0.95rem;
        }
    </style>
</head>
<body>

<form id="form1" runat="server">

<div class="dashboard-container">

    <uc:Sidebar ID="Sidebar1" runat="server" />

    <div class="sidebar-overlay"></div>

    <main class="main-content">

        <h2>💳 My Transactions</h2>

        <div id="transactionsList" class="product-grid">
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

<div id="receiptModal" class="receipt-modal" style="display:none;poasition:fixed;left:0;top:0;width:100%;height:100%;background:rgba(0,0,0,0.5);z-index:10001;">
    <div class="receipt-content" style="background:#fff;max-width:720px;margin:5% auto;padding:20px;border-radius:6px;position:relative;">
        <div class="receipt-header" style="display:flex;justify-content:space-between;align-items:center;">
            <h3 style="margin:0;">Receipt</h3>
            <button type="button" id="closeReceiptBtn" onclick="closeReceipt()" style="font-size:20px;background:none;border:0;cursor:pointer;">&times;</button>
        </div>

        <div id="receiptBody" class="receipt-body" style="margin-top:12px;max-height:60vh;overflow:auto;"></div>

        <div style="margin-top:12px;text-align:right;">
            <button type="button" id="closeReceiptOnly" onclick="closeReceipt()" style="padding:8px 12px;">Close</button>
        </div>
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
<script src="<%= ResolveUrl("../assets/script/script.js") %>"></script>
<script src="<%= ResolveUrl("../assets/script/notifications.js") %>"></script>
<script src="<%= ResolveUrl("../assets/script/transactions.js") %>"></script>
</body>
</html>
