document.addEventListener("DOMContentLoaded", function () {

    // SIDEBAR
    function toggleSidebar() {
        document.getElementById("sidebar").classList.toggle("active");
        document.getElementById("overlay").classList.toggle("active");
    }

    // Make toggleSidebar global if used inline
    window.toggleSidebar = toggleSidebar;

    // Overlay click
    document.getElementById("overlay")?.addEventListener("click", toggleSidebar);

    // DROPDOWN
    window.toggleDropdown = function (el) {
        let parent = el.parentElement;
        parent.classList.toggle("active");
    };

    // MOBILE NAV
    document.querySelectorAll('.mobile-nav .nav-item').forEach(item => {
        item.addEventListener('click', function () {
            document.querySelectorAll('.mobile-nav .nav-item')
                .forEach(i => i.classList.remove('active'));
            this.classList.add('active');
        });
    });

});


