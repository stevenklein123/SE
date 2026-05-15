
/* BEGIN EXTERNAL SOURCE */

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

/* END EXTERNAL SOURCE */
