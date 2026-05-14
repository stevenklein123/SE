<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="benefits.aspx.cs" Inherits="Project_Tracking.dealers.benefits" %>

<%@ Register Src="~/views/sidebar.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>My Benefits | AVON Portal</title>
    <link rel="icon" type="image/png" href="../assets/images/avon.png" />
    <link href="../assets/style/global.css" rel="stylesheet" />
    <link href="../assets/style/dashboard.css" rel="stylesheet" />
    <style>
        .benefits-summary-card { background: #fff; border: 1px solid var(--border); border-radius: 15px; padding: 25px; margin-top: 20px; }
        .current-rank-header { display: flex; justify-content: space-between; align-items: center; border-bottom: 1px solid var(--border); padding-bottom: 20px; margin-bottom: 20px; }
        .benefits-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 20px; }
        .benefit-item { display: flex; align-items: center; gap: 15px; padding: 15px; border-radius: 10px; background: var(--bg-body); }
        .benefit-item.highlight { background: var(--primary-light); border: 1px solid var(--primary); }
        .benefit-item .icon { font-size: 24px; }
        .benefit-item label { display: block; font-size: 12px; color: var(--text-light); margin-bottom: 4px; }
        .benefit-item h4 { margin: 0; font-size: 20px; color: var(--text); }
        .next-goal-card { margin-top: 20px; background: linear-gradient(135deg, var(--primary), var(--primary-hover)); color: white; padding: 20px; border-radius: 12px; }
        .badge.large { padding: 8px 20px; font-size: 16px; border-radius: 20px; color: white; font-weight: bold; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboard-container">
            <uc:Sidebar ID="Sidebar1" runat="server" />

            <main class="main-content">
                <h2>🎁 My Member Benefits</h2>
                <p>Base sa iyong benta ngayong buwan, narito ang iyong mga perks:</p>

                <div class="benefits-summary-card">
                    <div class="current-rank-header">
                        <div>
                            <span style="color: var(--text-light);">Current Status</span>
                            <h3 style="margin: 5px 0 0 0;">AVON REWARDS</h3>
                        </div>
                        <div id="rankBadge" runat="server" class="badge large">---</div>
                    </div>
                    
                    <div class="benefits-grid">
                        <div class="benefit-item">
                            <span class="icon">📉</span>
                            <div class="benefit-info">
                                <label>Monthly Discount</label>
                                <h4 id="lblDiscount" runat="server">0%</h4>
                            </div>
                        </div>

                        <div class="benefit-item">
                            <span class="icon">💰</span>
                            <div class="benefit-info">
                                <label>Earnings from Discount</label>
                                <h4 id="lblEarnFromDiscount" runat="server">₱0.00</h4>
                            </div>
                        </div>

                        <div class="benefit-item">
                            <span class="icon">⭐</span>
                            <div class="benefit-info">
                                <label>Suki Perks Potential</label>
                                <h4 id="lblSukiPerks" runat="server">₱0.00</h4>
                            </div>
                        </div>

                        <div class="benefit-item highlight">
                            <span class="icon">🚀</span>
                            <div class="benefit-info">
                                <label>Total Potential Earnings</label>
                                <h4 id="lblTotalPotential" runat="server">₱0.00</h4>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="next-goal-card">
                    <h3 style="margin:0;">Next Status Goal</h3>
                    <p id="lblNextGoal" runat="server" style="margin: 10px 0 0 0;">Loading your progress...</p>
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
<script src="<%= ResolveUrl("~/assets/script/notifications.js") %>"></script>
</body>
</html>
