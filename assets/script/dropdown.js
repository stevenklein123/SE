document.addEventListener('DOMContentLoaded', function () {

    const settingsItem = document.getElementById('settingsItem');
    const settingsDropdown = document.getElementById('settingsDropdown');

    if (!settingsItem || !settingsDropdown) return;

    // TOGGLE DROPDOWN
    settingsItem.addEventListener('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        settingsDropdown.classList.toggle('show');
    });

    // CLICK OUTSIDE CLOSE
    document.addEventListener('click', function (e) {
        if (!settingsItem.contains(e.target) && !settingsDropdown.contains(e.target)) {
            settingsDropdown.classList.remove('show');
        }
    });

});