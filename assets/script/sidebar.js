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
    if (settingsItem) {
        settingsItem.addEventListener("click", function (e) {
            e.preventDefault();
            settingsDropdown.classList.toggle("active");
        });
    }

});