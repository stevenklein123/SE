document.addEventListener("DOMContentLoaded", function () {
    document.addEventListener("click", function (e) {
        const btn = e.target.closest(".toggle-pass");
        if (!btn) return;

        const inputId = btn.getAttribute("data-target");
        const input = document.getElementById(inputId);

        if (!input) return;

        if (input.type === "password") {
            input.type = "text";
            btn.textContent = "Hide";
        } else {
            input.type = "password";
            btn.textContent = "Show";
        }
    });
});

