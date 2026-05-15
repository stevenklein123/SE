document.addEventListener("DOMContentLoaded", function () {

    const hamburger = document.getElementById("hamburger");
    const sidebar = document.getElementById("sidebar");
    const overlay = document.getElementById("sidebarOverlay");
    const closeBtn = document.getElementById("closeSidebar");
    const settingsItem = document.getElementById("settingsItem");
    const settingsDropdown = document.getElementById("settingsDropdown");

    // ===== OPEN SIDEBAR =====
    if (hamburger) {
        hamburger.addEventListener("click", function (e) {
            e.preventDefault();
            sidebar.classList.add("active");
            overlay.classList.add("active");
        });
    }

    // ===== CLOSE SIDEBAR =====
    function closeSidebar() {
        sidebar.classList.remove("active");
        overlay.classList.remove("active");
    }

    if (overlay) overlay.addEventListener("click", closeSidebar);
    if (closeBtn) closeBtn.addEventListener("click", closeSidebar);

    // ===== SETTINGS DROPDOWN =====
    if (settingsItem && settingsDropdown) {

        const settingsArrow = document.getElementById("settingsArrow");

        settingsItem.addEventListener("click", function (e) {

            e.preventDefault();

            settingsDropdown.classList.toggle("active");

            const dropdownItems = settingsDropdown.querySelectorAll(".dropdown-item");

            if (settingsDropdown.classList.contains("active")) {

                if (settingsArrow) {
                    settingsArrow.innerHTML = "▲";
                }

                dropdownItems.forEach(item => {
                    item.style.color = "#8e6b7a";
                });

            } else {

                if (settingsArrow) {
                    settingsArrow.innerHTML = "▼";
                }

                dropdownItems.forEach(item => {
                    item.style.color = "";
                });

            }

        });

}});

fetch('notifications.aspx/GetUnreadCount', {
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
});